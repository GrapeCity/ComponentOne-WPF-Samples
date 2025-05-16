using C1.DataCollection;
using C1.WPF.DataFilter;
using C1.WPF.Document;
using C1.WPF.Grid;
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
        List<string> _treeLinesModes;
        ObservableCollection<Person> _persons;
        List<ProjectTask> _tasks;

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
            themes.SelectedIndex = 7;
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

            _persons = new ObservableCollection<Person>(Person.Generate(80));

            flexGrid.ItemsSource = _persons;
            flexGrid.Columns["Age"].AllowGrouping = true;
            flexGrid.Columns["Born"].AllowGrouping = true;
            listView.ItemsSource = _persons;
            //     dataGrid.ItemsSource = persons;

            flexGrid2.ItemsSource = _persons;

            var pagedPersons = new C1PagedDataCollection<Person>(_persons) { PageSize = 10 };
            pager.Source = pagedPersons;
            listView2.ItemsSource = pagedPersons;

            var viewTypes = new List<string>
            {
                Properties.Resources.ViewType_Month,
                Properties.Resources.ViewType_Day,
                Properties.Resources.ViewType_Week,
                Properties.Resources.ViewType_WorkingWeek,
                Properties.Resources.ViewType_TimeLine
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
            sched1.ClearValue(C1.WPF.Schedule.C1Scheduler.ShowWeekTabsProperty);
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

        #region C1FlexGrid
        private void C1CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            flexGrid.ItemsSource = Tasks;

            flexGrid.ChildItemsPath = "SubTasks";
            flexGrid.TreeColumnIndex = 1;
            flexGrid.TreeExpandMode = GridTreeExpandMode.Expanded;

            flexGrid.Columns["WBS"].Width = GridLength.Auto;
            flexGrid.Columns["Duration"].ValueConverter = C1.WPF.Core.DelegateConverter.Create(
                    (value, type, parameter, culture) => string.Format("{0:N1} days?", ((TimeSpan)value).TotalDays),
                    (value, type, parameter, culture) =>
                    {
                        var str = value?.ToString() ?? "";
                        TimeSpan timeSpan;
                        if (TimeSpan.TryParse(str, out timeSpan))
                            return timeSpan;
                        if (str.EndsWith(" days?"))
                            str = str.Substring(0, str.Length - " days?".Length);
                        double totalDays;
                        if (double.TryParse(str, out totalDays))
                            return TimeSpan.FromDays(totalDays);
                        return TimeSpan.Zero;
                    });

            var nameColumn = flexGrid.Columns["Name"];
            nameColumn.AllowResizing = false;
            nameColumn.MinWidth = 300;
            nameColumn.Width = new GridLength(1, GridUnitType.Star);
        }

        private void C1CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            flexGrid.ChildItemsPath = string.Empty;
            flexGrid.TreeColumnIndex = 0;
            flexGrid.ItemsSource = _persons;

            flexGrid.Columns["Age"].AllowGrouping = true;
            flexGrid.Columns["Born"].AllowGrouping = true;
        }

        List<ProjectTask> Tasks
        {
            get
            {
                if (_tasks is null)
                {
                    var task1 = new ProjectTask() { WBS = "1", Name = "Requirements", Duration = new TimeSpan(50, 0, 0, 0), Start = new DateTime(2009, 12, 4) };
                    var task11 = new ProjectTask() { WBS = "1.1", Name = "Analysis", Duration = new TimeSpan(38, 3, 0, 0), Start = new DateTime(2009, 12, 4), ParentTask = task1 };
                    var task111 = new ProjectTask() { WBS = "1.1.1", Name = "Analyze online reservations", Duration = new TimeSpan(12, 12, 0, 0), Start = new DateTime(2009, 12, 4), ParentTask = task11 };
                    var task112 = new ProjectTask() { WBS = "1.1.2", Name = "Analyze query processes", Duration = new TimeSpan(12, 12, 0, 0), Start = new DateTime(2009, 12, 4), ParentTask = task11 };
                    var task113 = new ProjectTask() { WBS = "1.1.3", Name = "Analyze multimedia enhancements", Duration = new TimeSpan(12, 12, 0, 0), Start = new DateTime(2010, 1, 4), ParentTask = task11 };
                    var task114 = new ProjectTask() { WBS = "1.1.4", Name = "Draft Preliminary requirements", Duration = new TimeSpan(5, 0, 0, 0), Start = new DateTime(2010, 1, 14), ParentTask = task11 };
                    var task115 = new ProjectTask() { WBS = "1.1.5", Name = "Review preliminary requirements", Duration = new TimeSpan(2, 12, 0, 0), Start = new DateTime(2010, 1, 14), ParentTask = task11 };
                    var task116 = new ProjectTask() { WBS = "1.1.6", Name = "Incorporate feedback on requirements", Duration = new TimeSpan(2, 12, 0, 0), Start = new DateTime(2010, 1, 14), ParentTask = task11 };
                    var task117 = new ProjectTask() { WBS = "1.1.7", Name = "Obtain approval to proceed", Duration = new TimeSpan(3, 2, 0, 0), Start = new DateTime(2010, 2, 6), ParentTask = task11 };
                    task11.SubTasks = new List<ProjectTask> { task111, task112, task113, task114, task115, task116, task117 };
                    var task12 = new ProjectTask() { WBS = "1.2", Name = "Acceptance Test Plan", Duration = new TimeSpan(12, 3, 0, 0), Start = new DateTime(2010, 6, 23), ParentTask = task1 };
                    var task121 = new ProjectTask() { WBS = "1.2.1", Name = "Write acceptance test plans", Duration = new TimeSpan(5, 2, 0, 0), Start = new DateTime(2010, 6, 23), ParentTask = task12 };
                    var task122 = new ProjectTask() { WBS = "1.2.2", Name = "Draft acceptance test plan", Duration = new TimeSpan(5, 0, 0, 0), Start = new DateTime(2010, 6, 23), ParentTask = task12 };
                    var task123 = new ProjectTask() { WBS = "1.2.3", Name = "Review acceptance test plan", Duration = new TimeSpan(5, 6, 0, 0), Start = new DateTime(2010, 7, 4), ParentTask = task12 };
                    var task124 = new ProjectTask() { WBS = "1.2.4", Name = "Incorporate feedback on acceptance", Duration = new TimeSpan(5, 0, 0, 0), Start = new DateTime(2010, 7, 4), ParentTask = task12 };
                    task12.SubTasks = new List<ProjectTask> { task121, task122, task123, task124 };
                    task1.SubTasks = new List<ProjectTask> { task11, task12 };
                    var task2 = new ProjectTask() { WBS = "2", Name = "Design", Duration = new TimeSpan(55, 0, 0, 0), Start = new DateTime(2010, 8, 12) };
                    var task21 = new ProjectTask() { WBS = "2.1", Name = "Top-level Design", Duration = new TimeSpan(27, 12, 0, 0), Start = new DateTime(2010, 8, 12), ParentTask = task2 };
                    var task211 = new ProjectTask() { WBS = "2.1.1", Name = "Design online reservations", Duration = new TimeSpan(10, 0, 0, 0), Start = new DateTime(2010, 9, 7), ParentTask = task21 };
                    var task212 = new ProjectTask() { WBS = "2.1.2", Name = "Design query processes", Duration = new TimeSpan(10, 12, 0, 0), Start = new DateTime(2010, 9, 14), ParentTask = task21 };
                    var task213 = new ProjectTask() { WBS = "2.1.3", Name = "Design multimedia enhancements", Duration = new TimeSpan(10, 6, 0, 0), Start = new DateTime(2010, 10, 4), ParentTask = task21 };
                    var task214 = new ProjectTask() { WBS = "2.1.4", Name = "Review design specification", Duration = new TimeSpan(5, 12, 0, 0), Start = new DateTime(2010, 10, 9), ParentTask = task21 };
                    var task215 = new ProjectTask() { WBS = "2.1.5", Name = "Incorporate feedback into design", Duration = new TimeSpan(2, 2, 0, 0), Start = new DateTime(2010, 10, 9), ParentTask = task21 };
                    var task216 = new ProjectTask() { WBS = "2.1.6", Name = "Top-level design approved", Duration = new TimeSpan(1, 0, 0, 0), Start = new DateTime(2010, 12, 1), ParentTask = task21 };
                    task21.SubTasks = new List<ProjectTask> { task211, task212, task213, task214, task215, task216 };
                    var task22 = new ProjectTask() { WBS = "2.2", Name = "Detailed Design", Duration = new TimeSpan(23, 0, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task2 };
                    var task221 = new ProjectTask() { WBS = "2.2.1", Name = "Draft design specifications", Duration = new TimeSpan(17, 12, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task22 };
                    var task222 = new ProjectTask() { WBS = "2.2.2", Name = "Review design specifications", Duration = new TimeSpan(17, 0, 0, 0), Start = new DateTime(2010, 12, 8), ParentTask = task22 };
                    var task223 = new ProjectTask() { WBS = "2.2.3", Name = "Incorporate feedback on design specifications", Duration = new TimeSpan(17, 6, 0, 0), Start = new DateTime(2010, 12, 14), ParentTask = task22 };
                    var task224 = new ProjectTask() { WBS = "2.2.4", Name = "Obtain approval to proceed", Duration = new TimeSpan(5, 0, 0, 0), Start = new DateTime(2010, 12, 24), ParentTask = task22 };
                    var task225 = new ProjectTask() { WBS = "2.2.5", Name = "Detailed design approved", Duration = new TimeSpan(2, 12, 0, 0), Start = new DateTime(2010, 12, 28), ParentTask = task22 };
                    task22.SubTasks = new List<ProjectTask> { task221, task222, task223, task224, task225 };
                    task2.SubTasks = new List<ProjectTask> { task21, task22 };
                    var task3 = new ProjectTask() { WBS = "3", Name = "Code and Unit Test", Duration = new TimeSpan(32, 4, 0, 0), Start = new DateTime(2010, 12, 4) };
                    var task31 = new ProjectTask() { WBS = "3.1", Name = "Assign development staff", Duration = new TimeSpan(2, 3, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task3 };
                    var task32 = new ProjectTask() { WBS = "3.2", Name = "Develop Code", Duration = new TimeSpan(10, 3, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task3 };
                    var task321 = new ProjectTask() { WBS = "3.2.1", Name = "Develop online reservations", Duration = new TimeSpan(10, 2, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task32 };
                    var task322 = new ProjectTask() { WBS = "3.2.2", Name = "Test online reservations", Duration = new TimeSpan(1, 11, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task32 };
                    var task323 = new ProjectTask() { WBS = "3.2.3", Name = "Develop query processes", Duration = new TimeSpan(10, 0, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task32 };
                    var task324 = new ProjectTask() { WBS = "3.2.4", Name = "Test query processes", Duration = new TimeSpan(1, 4, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task32 };
                    task32.SubTasks = new List<ProjectTask> { task321, task322, task323, task324 };
                    task3.SubTasks = new List<ProjectTask> { task31, task32 };
                    _tasks = new List<ProjectTask> { task1, task2, task3 };
                }
                return _tasks;
            }
        }

        public List<string> TreeLinesModes => _treeLinesModes ??= [.. Enum.GetNames(typeof(GridTreeLinesMode))];

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
