using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace CustomLocalization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Keep CurrentCulture, CurrentUICulture and Language in sync to localize all possible parts.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentUICulture.Name);

            InitializeComponent();

            var langs = new List<CultureInfo>();
            foreach (var cultureName in new[] { "", "ar", "cs", "da", "de", "el", "es", "fi", "fr", "he", "it", "ja", "nl", "no", "pl", "pt", "ro", "ru", "en", "sk", "sv", "tr", "zh", "zh-Hans", "zh-Hant" })
            {
                var culture = new CultureInfo(cultureName);
                langs.Add(culture);
            }
            localizationCombo.ItemsSource = langs;
            localizationCombo.SelectedItem = langs[0];
            foreach (var neutralLanguage in langs)
            {
                if (neutralLanguage.TwoLetterISOLanguageName == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName)
                {
                    localizationCombo.SelectedItem = neutralLanguage;
                    break;
                }
            }
            localizationCombo.DisplayMemberPath = "DisplayName";
            localizationCombo.SelectionChanged += (s, e) =>
            {
                string cultureName = localizationCombo.SelectedIndex == 0 ? "en-US" : (localizationCombo.SelectedItem as CultureInfo).Name;
                // first change thread cultures
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
                // then change Language property. 
                // It's important to change Language in the last place, as at this point WPF 
                // updates all xaml bindings and thread cultures should already have the correct value set.
                this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(cultureName);
            };

            flexGrid.ItemsSource = Person.Generate(100);

            //listMsgBoxOptions.ItemsSource = Enum.GetValues(typeof(C1MessageBoxButton));
        }

        //C1MessageBoxButton _selectedButton;

        //private void btnOpenMsgBox_Click(object sender, RoutedEventArgs e)
        //{
        //    C1MessageBox.Show("", "", _selectedButton);
        //}

        //private void listMsgBoxOptions_ItemClick(object sender, SourcedEventArgs e)
        //{
        //    var menuItem = (C1MenuItem)e.Source;
        //    var selection = (C1MessageBoxButton)menuItem.Header;

        //    SelectMessageBoxType(selection);

        //    C1MessageBox.Show("", "", _selectedButton);
        //}

        //private void SelectMessageBoxType(C1MessageBoxButton selection)
        //{
        //    _selectedButton = selection;
        //    btnOpenMsgBox.Header = selection.ToString().Replace("C1.Silverlight.", "");
        //}
    }
}
