using C1.WPF.SpellChecker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Res = SpellCheckerExplorer.Properties.Resources;

namespace SpellCheckerExplorer
{
    /// <summary>
    /// Interaction logic for SpellCheckerDemo.xaml
    /// </summary>
    public partial class SpellCheckerTextDemo : UserControl
    {
        // declare the C1SpellChecker
        C1SpellChecker _c1SpellChecker = new C1SpellChecker();

        public SpellCheckerTextDemo()
        {
            InitializeComponent();
            this.Tag = Properties.Resources.SpellCheckerTextDemoDescription;
            Loaded += Page_Loaded;
            Unloaded += Page_Unloaded;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // load sample text into text boxes
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpellCheckerExplorer.Resources.test.txt"))
            using (var sr = new StreamReader(stream))
            {
                var text = sr.ReadToEnd();
                _plainTextBox.Text = text;
            }

            // set up ignore list
            WordList il = _c1SpellChecker.IgnoreList;
            il.Add("ComponentOne");
            il.Add("Silverlight");

            // monitor events
            _c1SpellChecker.BadWordFound += _c1SpellChecker_BadWordFound;
            _c1SpellChecker.CheckControlCompleted += _c1SpellChecker_CheckControlCompleted;

            // load main dictionary
            if (_c1SpellChecker.MainDictionary.State != DictionaryState.Loaded)
                _c1SpellChecker.MainDictionary.Load(Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/C1Spell_en-US.dct", UriKind.Relative)).Stream);
            if (_c1SpellChecker.MainDictionary.State == DictionaryState.Loaded)
            {
                _btnBatch.IsEnabled = _btnModal.IsEnabled = true;
                WriteLine($"{Res.DictionaryOk} ({0:n0} {Res.Word}).", _c1SpellChecker.MainDictionary.WordCount);
            }
            else
            {
                WriteLine($"{Res.DictionaryFailed}: {0}", _c1SpellChecker.MainDictionary.State);                    
            }
            // load user dictionary
            //UserDictionary ud = _c1SpellChecker.UserDictionary;
            //ud.LoadFromIsolatedStorage("Custom.dct");

            // save user dictionary when app exits
            App.Current.Exit += App_Exit;
        }

        void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _c1SpellChecker.BadWordFound -= _c1SpellChecker_BadWordFound;
            _c1SpellChecker.CheckControlCompleted -= _c1SpellChecker_CheckControlCompleted;
            
        }

        // app closing, save modified user dictionary into compressed isolated storage
        void App_Exit(object sender, EventArgs e)
        {
            //UserDictionary ud = _c1SpellChecker.UserDictionary;
            //ud.SaveToIsolatedStorage("Custom.dct");
            
        }

        #region batch spell-checking

        private void _btnBatch_Click(object sender, RoutedEventArgs e)
        {
            // check a word
            foreach (string word in "yes;no;yesno;do;don't;ain't;aint".Split(';'))
            {
                WriteLine("CheckWord(\"{0}\") = {1}", word, _c1SpellChecker.CheckWord(word));
            }

            // check some text
            var someText = "this text comtains two errrors.";
            var errors = _c1SpellChecker.CheckText(someText);
            WriteLine("CheckText(\"{0}\") =", someText);
            foreach (var error in errors)
            {
                WriteLine("\t{0}, {1}-{2}", error.Text, error.Start, error.Length);
                foreach (string suggestion in _c1SpellChecker.GetSuggestions(error.Text, 1000))
                {
                    WriteLine("\t\t{0}?", suggestion);
                }
            }

            // get suggestions
            var badWord = "traim";
            WriteLine("GetSuggestions(\"{0}\")", badWord);
            foreach (string suggestion in _c1SpellChecker.GetSuggestions(badWord, 1000))
            {
                WriteLine("\t{0}?", suggestion);
            }
        }

        #endregion

        #region modal spell-checking
        int selectedSpellDialogIndex;
        private void _btnModal_Click(object sender, RoutedEventArgs e)
        {
            // select control to spell-check
            object editor = _plainTextBox;

            // select type of modal dialog to use
            Window dialog = null;
            switch (selectedSpellDialogIndex)
            {
                // C1SpellChecker built-in dialog
                case 0:
                    dialog = new C1SpellDialog();
                    break;

                // Customized C1SpellChecker built-in dialog
                case 1:
                    dialog = new C1SpellDialog();
                    dialog.Title = "Customized Caption!";
                    break;

                // Standard dialog
                case 2:
                    dialog = new StandardSpellDialog();
                    break;

                // Word-style dialog
                case 3:
                    dialog = new WordSpellDialog();
                    break;
            }

            // spell-check the control
            _c1SpellChecker.CheckControlAsync(editor, false, (ISpellDialog)dialog);
        }

        #endregion

        #region event handlers

        // monitor spell-checker events
        void _c1SpellChecker_CheckControlCompleted(object sender, CheckControlCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                var msg = string.Format(Res.SpellCheckCompletedWithErrors, e.ErrorCount);
                MessageBox.Show(msg, "Spelling");
            }
            WriteLine("CheckControlCompleted: {0} errors found", e.ErrorCount);
            if (e.Cancelled)
            {
                WriteLine("\t(cancelled...)");
            }
        }
        void _c1SpellChecker_BadWordFound(object sender, BadWordEventArgs e)
        {
            WriteLine("BadWordFound: \"{0}\" {1}", e.BadWord.Text, e.BadWord.Duplicate ? "(duplicate)" : string.Empty);
        }

        // keep text selected while spell-checking dialog is open
        private void _plainTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region utilities

        // trace execution
        void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }
        void WriteLine(string text)
        {
            _outputTextBox.Text += text + "\r\n";
        }

        // helper class used for binding to the DataGrid
        public class Beatle : INotifyPropertyChanged
        {
            string _name, _comments;


            Beatle()
            {
            }
            public string Name { get { return _name; } set { _name = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name")); } }
            public string Comments { get { return _comments; } set { _comments = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Comments")); } }
            public static List<Beatle> GetBeatles()
            {
                var list = new List<Beatle>();
                list.Add(new Beatle { Name = "Paul McCartney", Comments = "Great songriter, bass playier, singer" });
                list.Add(new Beatle { Name = "John Lennon", Comments = "Great songriter, gitar playier, excelent singer" });
                list.Add(new Beatle { Name = "Ringo Starr", Comments = "Great drummmer" });
                list.Add(new Beatle { Name = "George Harrison", Comments = "Great guiter player, songriter, vocallist" });
                return list;
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }
        #endregion

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if ((rb.Content as string) == "Built-In")
                selectedSpellDialogIndex = 0;
            else if ((rb.Content as string) == "Customized")
                selectedSpellDialogIndex = 1;
            else if ((rb.Content as string) == "Default Style")
                selectedSpellDialogIndex = 2;
            else if ((rb.Content as string) == "Word Style")
                selectedSpellDialogIndex = 3;

        }
    }
}
