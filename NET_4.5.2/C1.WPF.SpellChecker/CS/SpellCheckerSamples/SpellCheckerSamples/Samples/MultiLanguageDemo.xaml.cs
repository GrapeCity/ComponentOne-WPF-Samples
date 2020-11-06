using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.SpellChecker;
using System.Reflection;
using System.IO;

namespace SpellCheckerSamples
{
    /// <summary>
    /// Interaction logic for MultiLanguageDemo.xaml
    /// </summary>
    public partial class MultiLanguageDemo : UserControl
    {
        // declare the C1SpellChecker
        C1SpellChecker _c1SpellChecker = new C1SpellChecker();

        public MultiLanguageDemo()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MultiLanguage_Loaded);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _c1SpellChecker.CheckControlAsync(_plainTextBox);
        }

        void MultiLanguage_Loaded(object sender, RoutedEventArgs e)
        {
            cmbLanguage.SelectionChanged += new SelectionChangedEventHandler(cmbLanguage_SelectionChanged);
            cmbLanguage.ItemTemplate = (DataTemplate)Resources["DictionaryTemplate"];
            cmbLanguage.ItemsSource = SpellDictionaryWrapper.GetLanguages();
            cmbLanguage.SelectedIndex = 0;

            // load sample text into text boxes
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpellCheckerSamples.Resources.test.txt"))
            using (var sr = new StreamReader(stream))
            {
                var text = sr.ReadToEnd();
                _plainTextBox.Text = text;
            }
        }

        void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetDictionaryFromResources();
        }

        // keep text selected while spell-checking dialog is open
        private void _plainTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void GetDictionaryFromResources()
        {
            var dctWrapper = (SpellDictionaryWrapper)cmbLanguage.SelectedItem;
            if (dctWrapper != null)
            {
                // load the dictionary from the server
                var dict = _c1SpellChecker.MainDictionary;
                dict.Load(Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component" + dctWrapper.Address, UriKind.Relative)).Stream);
                
                // dictionary loaded, update status
                if (_c1SpellChecker.MainDictionary.State == DictionaryState.Loaded)
                {
                    WriteLine("Loaded {0} dictionary ({1:n0} words).", dctWrapper.Name, _c1SpellChecker.MainDictionary.WordCount);
                }
                else
                {
                    WriteLine("Failed to load {0} dictionary: {1}", dctWrapper.Name, dict.State);
                }
            }
        }

        //------------------------------------------------------------
        #region ** utilities

        void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }
        void WriteLine(string text)
        {
            _outputTextBox.Text = text + "\r\n";
        }

        #endregion

        
    }

    #region SpellDictionaryWrapper

    public class SpellDictionaryWrapper
    {
        public static List<SpellDictionaryWrapper> GetLanguages()
        {
            var list = new List<SpellDictionaryWrapper>();
            list.Add(new SpellDictionaryWrapper() { Address = "/Resources/C1Spell_en-US.dct", Name = "English (USA)", FlagUri = new Uri("/Resources/USA.png", UriKind.Relative) });
            list.Add(new SpellDictionaryWrapper() { Address = "/Resources/C1Spell_fr-FR.dct", Name = "French (France)", FlagUri = new Uri("/Resources/France.png", UriKind.Relative) });
            return list;
        }

        public string Address { get; set; }
        public string Name { get; set; }
        public Uri FlagUri { get; set; }
    }

    #endregion
}
