using C1.DataCollection;
using C1.WPF.DataFilter;
using C1.WPF.Document;
using C1.WPF.TabControl;
using C1.WPF.Themes;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Serialization;
using ThemeExplorer.Helpers;

namespace ThemeExplorer
{
    /// <summary>
    /// Interaction logic for ControlGallery.xaml
    /// </summary>
    public partial class Gallery : UserControl
    {
        Color _lastSystemForeground;

        public Gallery()
        {
            Tag = Properties.Resources.Tag;
            _lastSystemForeground = SystemColors.ControlTextColor;

            // Set foreground to Black if theme is not applied. It allows to keep black foreground fore default styles if end-user has HighContrast settings turned on.
            Foreground = Brushes.Black;
            Background = Brushes.White;
            InitializeComponent();

            //Background = Resources["WindowBackground"] as LinearGradientBrush;
            themes.ItemsSource = typeof(C1AvailableThemes).GetEnumValues();
            themes.SelectedIndex = 2;
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

            var persons = new ObservableCollection<Person>(Person.Generate(80));

            flexGrid.ItemsSource = persons;
            flexGrid.Columns["Age"].AllowGrouping = true;
            listView.ItemsSource = persons;
       //     dataGrid.ItemsSource = persons;

            var pagedPersons = new C1PagedDataCollection<Person>(persons) { PageSize = 10 };
            pager.Source = pagedPersons;
            listView2.ItemsSource = pagedPersons;

            var viewTypes = new List<C1.WPF.Schedule.ViewType>
            {
                C1.WPF.Schedule.ViewType.Month,
                C1.WPF.Schedule.ViewType.Day,
                C1.WPF.Schedule.ViewType.Week,
                C1.WPF.Schedule.ViewType.WorkingWeek,
                C1.WPF.Schedule.ViewType.TimeLine
            };
            ViewType.ItemsSource = viewTypes;

        }

        #region Theme
        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            // if system theme is applied, foreground might not apply automatically when end-user changes system theme
            // re-apply system theme in this case
            if (_lastSystemForeground != SystemColors.ControlTextColor && themes.Items.Count > 0 && themes.SelectedItem != null && (C1AvailableThemes)(themes.SelectedItem) == C1AvailableThemes.System)
            {
                themes_SelectedItemChanged(null, null);
                _lastSystemForeground = SystemColors.ControlTextColor;
            }
        }

        private void themes_SelectedItemChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<object> e)
        {
            C1Theme theme = null;
            if (themes.SelectedItem != null)
            {
                theme = C1ThemeFactory.GetTheme((C1AvailableThemes)themes.SelectedItem);
                ClearValue(BackgroundProperty);
            }
            if (theme == null)
            {
                // apply default resources
                var rd = C1Theme.GetDefaultResources();
                C1Theme.ApplyTheme(LayoutRoot, rd);
                Foreground = System.Windows.Media.Brushes.Black;
                Background = System.Windows.Media.Brushes.White;
                CurrentTheme = null;
                return;
            }

            C1Theme.ApplyTheme(LayoutRoot, theme);
            CurrentTheme = theme;
        }
        public Theme CurrentTheme
        {
            get { return (Theme)GetValue(CurrentThemeProperty); }
            set { SetValue(CurrentThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTheme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentThemeProperty =
            DependencyProperty.Register("CurrentTheme", typeof(Theme), typeof(Gallery), new PropertyMetadata(OnThemeChanged));

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Gallery)d).OnThemeChanged();
        }

        private void OnThemeChanged()
        {
            var theme = CurrentTheme != null ? CurrentTheme.GetNewResourceDictionary() : C1Theme.GetDefaultResources();

            //update themes in individual controls
            pivot.ThemeResources = theme;
            sched1.Theme = theme;
            ganttPage.Theme = theme;
            inputAndEditing.Theme = theme;
        }
        #endregion

        private void DataManagement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
                return;
            var item = e.AddedItems[0] as C1TabItem;
            #region Filter Editor
            if (item.Header.Equals("Filter Editor") && filterEditor.ItemsSource == null)
            {
                C1DataCollection<Person> data = new C1DataCollection<Person>(Person.Generate(150));
                filterEditor.ItemsSource = data;
                filterEditorFlexGrid.ItemsSource = data;
                InitExpressionAndApply();
            }
            #endregion
            #region Pivot
            if (item.Header.Equals("FlexPivot") && pivot.DataSource == null)
            {
                var persons = Person.Generate(80);
                pivot.DataSource = persons;
                pivot.C1PivotEngine.RowFields.Add("Occupation");
                pivot.C1PivotEngine.ColumnFields.Add("Residence");
                pivot.C1PivotEngine.ValueFields.Add("Age");
            }
            #endregion
            #region DataFilter
            if (item.Header.Equals("Data Filter") && c1DataFilter1.ItemsSource == null)
            {
                var persons = Person.Generate(150);
                occupationFilter.ItemsSource = Person.Occupations;
                dataFilterFlexGrid.ItemsSource = persons;
                c1DataFilter1.ItemsSource = dataFilterFlexGrid.DataCollection;
            }
            #endregion
        }

        #region FilterEditor
        async void InitExpressionAndApply()
        {
            string pathToXmlFile = Directory.GetCurrentDirectory() + "\\FilterExpression.xml";
            if (File.Exists(pathToXmlFile))
            {
                var filterExpression = LoadFilterFromFile(pathToXmlFile);
                filterEditor.Expression = filterExpression;
            }

            await filterEditor.ApplyFilterAsync();
        }

        CombinationExpression LoadFilterFromFile(string filePath)
        {
            CombinationExpression filterExpression;
            var xmlSerializer = new XmlSerializer(typeof(CombinationExpression));
            using (var fs = File.Open(filePath, FileMode.Open))
            {
                filterExpression = xmlSerializer.Deserialize(fs) as CombinationExpression;
            }

            return filterExpression;
        }

        private void filterEditor_FilterChanged(object sender, EventArgs e)
        {
            string pathToXmlFile = Directory.GetCurrentDirectory() + "\\FilterExpression.xml";
            SaveFilterToFile(pathToXmlFile);
        }

        private void SaveFilterToFile(string filePath)
        {
            var expression = filterEditor.Expression;
            var xmlSerializer = new XmlSerializer(typeof(CombinationExpression));
            using (var fs = File.Create(filePath))
            {
                xmlSerializer.Serialize(fs, expression);
            }
        }
        #endregion

        #region C1Scheduler specifics
        private void Grouping_Checked(object sender, RoutedEventArgs e)
        {
            sched1.GroupBy = "Category";
        }

        private void Grouping_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sched1 != null)
            {
                sched1.GroupBy = "";
            }
        }
        #endregion

        private void fv_OpenAction(object sender, EventArgs e)
        {
            C1PdfDocumentSource pdfDocumentSource = new C1PdfDocumentSource();
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Title = "Select the PDF file",
                Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            };
            fv.Focus();

            if (!openFileDialog.ShowDialog().Value)
                return;

            // load document
            while (true)
            {
                try
                {
                    pdfDocumentSource.LoadFromFile(openFileDialog.FileName);
                    fv.DocumentSource = pdfDocumentSource;
                    break;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }


        }
    }
}
