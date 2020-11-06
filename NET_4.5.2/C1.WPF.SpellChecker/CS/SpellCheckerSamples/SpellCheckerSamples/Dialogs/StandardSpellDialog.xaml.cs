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
using C1.WPF;

namespace SpellCheckerSamples
{
    /// <summary>
    /// Interaction logic for StandardSpellDialog.xaml
    /// </summary>
    public partial class StandardSpellDialog : C1Window, ISpellDialog
    {
        //------------------------------------------------------------------------
        #region ** fields

        C1SpellChecker _spell;          // spell-checker to use
        ISpellCheckableEditor _editor;  // generic wrapper for editor
        CharRangeList _errors;          // current error list
        int _errorIndex = -1;           // current error index
        int _errorCount;                // error count
        Brush _brNormalText;            // brush used to show correct text
        Brush _brErrorText;             // brush used to show misspelled text
        Button _acceptButton;

        Dictionary<string, string> _changeAll = new Dictionary<string, string>();

        #endregion

        public StandardSpellDialog()
        {
            InitializeComponent();

            // center dialog on screen
            CenterOnScreen();

            // allow users to see the text in the control being checked
            ModalBackground = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));

            // create brushes used to show regular and misspelled text
            _brNormalText = _txtChangeTo.Foreground;
            _brErrorText = new SolidColorBrush(Color.FromArgb(255, 150, 0, 0));
        }

        //------------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Initializes the dialog to use the given parameters.
        /// </summary>
        /// <param name="spell"><see cref="C1SpellChecker"/> to use for spelling.</param>
        /// <param name="editor"><see cref="ISpellCheckableEditor"/> that contains the text to spell-check.</param>
        /// <param name="errors"><see cref="CharRangeList"/> that contains the initial error list.</param>
        public void Initialize(C1SpellChecker spell, ISpellCheckableEditor editor, CharRangeList errors)
        {
            // save references to all objects
            _spell = spell;
            _editor = editor;
            _errors = errors;
            if (_errors == null)
            {
                _errors = _spell.CheckText(_editor.Text);
            }
            _errorCount += _errors.Count;

            // go show the first error
            ErrorIndex = 0;
        }
        /// <summary>
        /// Gets the total number of errors detected in the control.
        /// </summary>
        public int ErrorCount
        {
            get { return _errorCount; }
        }
        /// <summary>
        /// Gets a reference to the <see cref="ISpellCheckableEditor"/> object being spell-checked.
        /// </summary>
        public ISpellCheckableEditor Editor
        {
            get { return _editor; }
        }
        /// <summary>
        /// Gets a <see cref="CharRangeList"/> object with all the errors detected by the 
        /// <see cref="C1SpellChecker"/> component that owns the dialog.
        /// </summary>
        public CharRangeList Errors
        {
            get { return _errors; }
        }
        /// <summary>
        /// Gets or sets the index of the current error into the <see cref="Errors"/> list.
        /// </summary>
        public int ErrorIndex
        {
            get { return _errorIndex; }
            set
            {
                _errorIndex = value;
                UpdateCurrentError();
            }
        }
        /// <summary>
        /// Gets the <see cref="CharRange"/> object that represents the error currently 
        /// displayed in the dialog.
        /// </summary>
        public CharRange CurrentError
        {
            get { return _errors[_errorIndex]; }
        }
        /// <summary>
        /// Event that fires when the <see cref="C1SpellDialog"/> displays an error
        /// to the user.
        /// </summary>
        /// <remarks>
        /// You can use the <see cref="ErrorIndex"/> and <see cref="Errors"/> properties
        /// to display information about the error in a status bar.
        /// </remarks>
        public event EventHandler ErrorDisplayed;
        /// <summary>
        /// Raises the <see cref="ErrorDisplayed"/> event.
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnErrorDisplayed(EventArgs e)
        {
            if (ErrorDisplayed != null)
                ErrorDisplayed(this, e);
        }

        #endregion

        //------------------------------------------------------------------------
        #region ** private stuff

        // update dialog to show current error
        void UpdateCurrentError()
        {
            // design time
            if (_errors == null)
            {
                return;
            }

            // finished with this batch of errors
            if (ErrorIndex >= _errors.Count)
            {
                // check whether the editor has more text to check
                while (_editor.HasMoreText())
                {
                    _errors = _spell.CheckText(_editor.Text);
                    if (_errors.Count > 0)
                    {
                        _errorCount += _errors.Count;
                        ErrorIndex = 0;
                        return;
                    }
                }

                // editor has no more text...
                DialogResult = MessageBoxResult.OK;
                return;
            }

            // update current error
            CharRange err = CurrentError;

            // select word in editor
            _editor.Select(err.Start, err.Text.Length);

            // honor 'change all' list
            if (_changeAll.ContainsKey(err.Text))
            {
                _txtChangeTo.Text = _changeAll[err.Text];
                _btnChange_Click(this, new RoutedEventArgs());
                return;
            }

            // raise 'BadWordFound' event
            BadWordEventArgs e = new BadWordEventArgs(this, _editor as Control, err, _errors);
            _spell.OnBadWordFound(e);
            if (e.Cancel)
            {
                DialogResult = MessageBoxResult.Cancel;
                return;
            }

            // show bad word, new text
            _txtError.Text = err.Text;
            _txtChangeTo.Text = string.Empty;

            // repeated word?
            if (err.Duplicate)
            {
                // adjust dialog
                _lblNotInDictionary.Visibility = Visibility.Collapsed;
                _lblRepeatedWord.Visibility = Visibility.Visible;
                _btnIgnoreAll.IsEnabled = false;
                _btnChangeAll.IsEnabled = false;
                _btnAdd.IsEnabled = false;

                // no suggestions
                _listSuggestions.Items.Clear();
            }
            else
            {
                // adjust dialog
                _lblRepeatedWord.Visibility = Visibility.Collapsed;
                _lblNotInDictionary.Visibility = Visibility.Visible;
                _btnIgnoreAll.IsEnabled = true;
                _btnChangeAll.IsEnabled = true;
                _btnAdd.IsEnabled = true;

                // show suggestions
                UpdateSuggestions(err.Text);
            }

            // focus to new word
            _txtChangeTo.SelectAll();
            _txtChangeTo.Focus();
            _btnSuggest.IsEnabled = false;
            AcceptButton = _btnIgnore;

            // show 'Add' button only if user dictionary is enabled
            _btnAdd.Visibility = _spell.UserDictionary.Enabled ? Visibility.Visible : Visibility.Collapsed;

            // update button status
            UpdateButtonStatus();

            // all ready, fire ErrorDisplayed event
            OnErrorDisplayed(EventArgs.Empty);
        }

        // get or set the button that executes when the user presses enter
        Button AcceptButton
        {
            get { return _acceptButton; }
            set
            {
                if (_acceptButton != value)
                {
                    if (_acceptButton != null)
                    {
                        _acceptButton.FontWeight = FontWeights.Normal;
                    }
                    _acceptButton = value;
                    if (_acceptButton != null)
                    {
                        _acceptButton.FontWeight = FontWeights.Bold;
                    }
                }
            }
        }

        // update suggestions in the list
        void UpdateSuggestions(string word)
        {
            _listSuggestions.Items.Clear();
            string[] suggestions = _spell.GetSuggestions(word);
            if (suggestions.Length > 0)
            {
                foreach (var suggestion in suggestions)
                {
                    AddSuggestion(suggestion);
                }
                _listSuggestions.SelectedIndex = 0;
                _listSuggestions.IsEnabled = true;
            }
            else
            {
                AddSuggestion(_lblNoSuggestions.Text);
                _listSuggestions.SelectedIndex = -1;
                _listSuggestions.IsEnabled = false;
            }
        }

        // suggestions in list are TextBlock elements
        void AddSuggestion(string suggestion)
        {
            TextBlock tb = new TextBlock();
            C1TapHelper th = new C1TapHelper(tb);
            th.DoubleTapped += th_DoubleTapped;
            tb.Text = suggestion;
            _listSuggestions.Items.Add(tb);
        }

        // double-click selects word and changes it
        void th_DoubleTapped(object sender, C1TappedEventArgs e)
        {
            var tb = _listSuggestions.SelectedItem as TextBlock;
            if (tb != null && tb.Text == _txtChangeTo.Text)
            {
                _btnChange_Click(this, null);
            }
        }

        // disable ChangeAll button when deleting words
        void UpdateButtonStatus()
        {
            // update button status
            if (_txtChangeTo.Text.Length > 0)
            {
                // update change buttons
                _btnChange.Content = _lblChange.Text;
                _btnChangeAll.IsEnabled = _errors.Count > 0 && !CurrentError.Duplicate;

                // update suggest button
                string text = _txtChangeTo.Text;
                if (text.IndexOf(' ') < 0 && !_listSuggestions.Items.Contains(text))
                {
                    bool enable = true;
                    for (int i = 0; i < text.Length && enable; i++)
                    {
                        if (!char.IsLetterOrDigit(text, i) && text[i] != '\'')
                            enable = false;
                    }
                    _btnSuggest.IsEnabled = enable;
                }

                // indicate whether current text is valid
                _txtChangeTo.Foreground = _spell.CheckText(_txtChangeTo.Text).Count == 0
                    ? _brNormalText
                    : _brErrorText;
            }
            else
            {
                _btnChange.Content = _lblRemove.Text;
                _btnChangeAll.IsEnabled = false;
                _btnSuggest.IsEnabled = false;
                _txtChangeTo.Foreground = _brNormalText;
            }

            // change default button
            AcceptButton = _btnChange;
        }

        #endregion

        //------------------------------------------------------------------------
        #region ** event handlers

        // update ChangeTo text when a suggestion is selected
        private void _listSuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tb = _listSuggestions.SelectedItem as TextBlock;
            if (tb != null && tb.Text != _lblNoSuggestions.Text)
            {
                _txtChangeTo.Text = tb.Text;
            }
        }

        // move to the next error (ignore the current one)
        void _btnIgnore_Click(object sender, RoutedEventArgs e)
        {
            ErrorIndex++;
        }

        // add current word to the ignore list in the spell-checker
        void _btnIgnoreAll_Click(object sender, RoutedEventArgs e)
        {
            _spell.IgnoreList.Add(CurrentError.Text);
            _errors = _spell.CheckText(_editor.Text, CurrentError.Start);
            UpdateCurrentError();
        }

        // change current word into 'ChangeTo' value
        void _btnChange_Click(object sender, RoutedEventArgs e)
        {
            // save starting point to continue checking from here
            int start = CurrentError.Start;

            // get replacement text
            string replacement = _txtChangeTo.Text;
            if (replacement.Length == 0)
            {
                // if replacement is empty, expand over spaces and commas
                CharRange delete = CharRange.ExpandOverWhitespace(_editor.Text, CurrentError);
                _editor.Select(delete.Start, delete.Text.Length);
            }

            // replace word in text and re-check
            _editor.SelectedText = replacement;
            _errors = _spell.CheckText(_editor.Text, start);

            // update index
            ErrorIndex = 0;
        }

        // change all instances of the current word into 'ChangeTo' value
        void _btnChangeAll_Click(object sender, RoutedEventArgs e)
        {
            _changeAll[CurrentError.Text] = _txtChangeTo.Text;
            _btnChange_Click(this, e);
        }

        // add current word to the user dictionary
        void _btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _spell.UserDictionary.AddWord(CurrentError.Text);
            _errors = _spell.CheckText(_editor.Text, CurrentError.Start);
            ErrorIndex = 0;
        }

        // update suggestion list with current content
        void _btnSuggest_Click(object sender, RoutedEventArgs e)
        {
            UpdateSuggestions(_txtChangeTo.Text);
        }

        // update button status when ChangeTo text changes
        void _txtChangeTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonStatus();
        }

        // cancel dialog
        void _btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = MessageBoxResult.Cancel;
        }

        // handle enter/escape
        private void C1SpellDialog_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // accept/ignore
                case Key.Enter:
                    if (AcceptButton == _btnChange)
                    {
                        _btnChange_Click(this, null);
                    }
                    else if (AcceptButton == _btnIgnore)
                    {
                        _btnIgnore_Click(this, null);
                    }
                    break;

                // cancel spell-checking
                case Key.Escape:
                    DialogResult = MessageBoxResult.Cancel;
                    break;
            }
        }
        #endregion


    }
}
