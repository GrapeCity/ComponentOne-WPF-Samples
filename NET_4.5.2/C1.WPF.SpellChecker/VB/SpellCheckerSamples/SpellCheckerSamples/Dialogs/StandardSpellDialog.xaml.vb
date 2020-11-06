Imports System.Text
Imports C1.WPF.SpellChecker
Imports C1.WPF

Namespace SpellCheckerSamples
	''' <summary>
	''' Interaction logic for StandardSpellDialog.xaml
	''' </summary>
	Partial Public Class StandardSpellDialog
		Inherits C1Window
        Implements ISpellDialog
        '------------------------------------------------------------------------
#Region "** fields"

        Private _spell As C1SpellChecker ' spell-checker to use
        Private _editor As ISpellCheckableEditor ' generic wrapper for editor
        Private _errors As CharRangeList ' current error list
        Private _errorIndex As Integer = -1 ' current error index
        Private _errorCount As Integer ' error count
        Private _brNormalText As Brush ' brush used to show correct text
        Private _brErrorText As Brush ' brush used to show misspelled text
        Private _acceptButton As Button

        Private _changeAll As New Dictionary(Of String, String)()

#End Region

        Public Sub New()
            InitializeComponent()

            ' center dialog on screen
            CenterOnScreen()

            ' allow users to see the text in the control being checked
            ModalBackground = New SolidColorBrush(Color.FromArgb(40, 0, 0, 0))

            ' create brushes used to show regular and misspelled text
            _brNormalText = _txtChangeTo.Foreground
            _brErrorText = New SolidColorBrush(Color.FromArgb(255, 150, 0, 0))
        End Sub

        '------------------------------------------------------------------------
#Region "** object model"

        ''' <summary>
        ''' Initializes the dialog to use the given parameters.
        ''' </summary>
        ''' <param name="spell"><see cref="C1SpellChecker"/> to use for spelling.</param>
        ''' <param name="editor"><see cref="ISpellCheckableEditor"/> that contains the text to spell-check.</param>
        ''' <param name="errors"><see cref="CharRangeList"/> that contains the initial error list.</param>
        Public Sub Initialize(ByVal spell As C1SpellChecker, ByVal editor As ISpellCheckableEditor, ByVal errors As CharRangeList) Implements C1.WPF.SpellChecker.ISpellDialog.Initialize
            ' save references to all objects
            _spell = spell
            _editor = editor
            _errors = errors
            If _errors Is Nothing Then
                _errors = _spell.CheckText(_editor.Text)
            End If
            _errorCount += _errors.Count

            ' go show the first error
            ErrorIndex = 0
        End Sub
        ''' <summary>
        ''' Gets the total number of errors detected in the control.
        ''' </summary>
        Public ReadOnly Property ErrorCount() As Integer Implements C1.WPF.SpellChecker.ISpellDialog.ErrorCount
            Get
                Return _errorCount
            End Get
        End Property
        ''' <summary>
        ''' Gets a reference to the <see cref="ISpellCheckableEditor"/> object being spell-checked.
        ''' </summary>
        Public ReadOnly Property Editor() As ISpellCheckableEditor Implements C1.WPF.SpellChecker.ISpellDialog.Editor
            Get
                Return _editor
            End Get
        End Property
        ''' <summary>
        ''' Gets a <see cref="CharRangeList"/> object with all the errors detected by the 
        ''' <see cref="C1SpellChecker"/> component that owns the dialog.
        ''' </summary>
        Public ReadOnly Property Errors() As CharRangeList
            Get
                Return _errors
            End Get
        End Property
        ''' <summary>
        ''' Gets or sets the index of the current error into the <see cref="Errors"/> list.
        ''' </summary>
        Public Property ErrorIndex() As Integer
            Get
                Return _errorIndex
            End Get
            Set(ByVal value As Integer)
                _errorIndex = value
                UpdateCurrentError()
            End Set
        End Property
        ''' <summary>
        ''' Gets the <see cref="CharRange"/> object that represents the error currently 
        ''' displayed in the dialog.
        ''' </summary>
        Public ReadOnly Property CurrentError() As CharRange
            Get
                Return _errors(_errorIndex)
            End Get
        End Property
        ''' <summary>
        ''' Event that fires when the <see cref="C1SpellDialog"/> displays an error
        ''' to the user.
        ''' </summary>
        ''' <remarks>
        ''' You can use the <see cref="ErrorIndex"/> and <see cref="Errors"/> properties
        ''' to display information about the error in a status bar.
        ''' </remarks>
        Public Event ErrorDisplayed As EventHandler
        ''' <summary>
        ''' Raises the <see cref="ErrorDisplayed"/> event.
        ''' </summary>
        ''' <param name="e"><see cref="EventArgs"/> that contains the event data.</param>
        Protected Overridable Sub OnErrorDisplayed(ByVal e As EventArgs)
            RaiseEvent ErrorDisplayed(Me, e)
        End Sub

#End Region

        '------------------------------------------------------------------------
#Region "** private stuff"

        ' update dialog to show current error
        Private Sub UpdateCurrentError()
            ' design time
            If _errors Is Nothing Then
                Return
            End If

            ' finished with this batch of errors
            If ErrorIndex >= _errors.Count Then
                ' check whether the editor has more text to check
                Do While _editor.HasMoreText()
                    _errors = _spell.CheckText(_editor.Text)
                    If _errors.Count > 0 Then
                        _errorCount += _errors.Count
                        ErrorIndex = 0
                        Return
                    End If
                Loop

                ' editor has no more text...
                DialogResult = MessageBoxResult.OK
                Return
            End If

            ' update current error
            Dim err As CharRange = CurrentError

            ' select word in editor
            _editor.Select(err.Start, err.Text.Length)

            ' honor 'change all' list
            If _changeAll.ContainsKey(err.Text) Then
                _txtChangeTo.Text = _changeAll(err.Text)
                _btnChange_Click(Me, New RoutedEventArgs())
                Return
            End If

            ' raise 'BadWordFound' event
            Dim e As New BadWordEventArgs(Me, TryCast(_editor, Control), err, _errors)
            _spell.OnBadWordFound(e)
            If e.Cancel Then
                DialogResult = MessageBoxResult.Cancel
                Return
            End If

            ' show bad word, new text
            _txtError.Text = err.Text
            _txtChangeTo.Text = String.Empty

            ' repeated word?
            If err.Duplicate Then
                ' adjust dialog
                _lblNotInDictionary.Visibility = Visibility.Collapsed
                _lblRepeatedWord.Visibility = Visibility.Visible
                _btnIgnoreAll.IsEnabled = False
                _btnChangeAll.IsEnabled = False
                _btnAdd.IsEnabled = False

                ' no suggestions
                _listSuggestions.Items.Clear()
            Else
                ' adjust dialog
                _lblRepeatedWord.Visibility = Visibility.Collapsed
                _lblNotInDictionary.Visibility = Visibility.Visible
                _btnIgnoreAll.IsEnabled = True
                _btnChangeAll.IsEnabled = True
                _btnAdd.IsEnabled = True

                ' show suggestions
                UpdateSuggestions(err.Text)
            End If

            ' focus to new word
            _txtChangeTo.SelectAll()
            _txtChangeTo.Focus()
            _btnSuggest.IsEnabled = False
            AcceptButton = _btnIgnore

            ' show 'Add' button only if user dictionary is enabled
            _btnAdd.Visibility = If(_spell.UserDictionary.Enabled, Visibility.Visible, Visibility.Collapsed)

            ' update button status
            UpdateButtonStatus()

            ' all ready, fire ErrorDisplayed event
            OnErrorDisplayed(EventArgs.Empty)
        End Sub

        ' get or set the button that executes when the user presses enter
        Private Property AcceptButton() As Button
            Get
                Return _acceptButton
            End Get
            Set(ByVal value As Button)
                If _acceptButton IsNot value Then
                    If _acceptButton IsNot Nothing Then
                        _acceptButton.FontWeight = FontWeights.Normal
                    End If
                    _acceptButton = value
                    If _acceptButton IsNot Nothing Then
                        _acceptButton.FontWeight = FontWeights.Bold
                    End If
                End If
            End Set
        End Property

        ' update suggestions in the list
        Private Sub UpdateSuggestions(ByVal word As String)
            _listSuggestions.Items.Clear()
            Dim suggestions() As String = _spell.GetSuggestions(word)
            If suggestions.Length > 0 Then
                For Each suggestion In suggestions
                    AddSuggestion(suggestion)
                Next suggestion
                _listSuggestions.SelectedIndex = 0
                _listSuggestions.IsEnabled = True
            Else
                AddSuggestion(_lblNoSuggestions.Text)
                _listSuggestions.SelectedIndex = -1
                _listSuggestions.IsEnabled = False
            End If
        End Sub

        ' suggestions in list are TextBlock elements
        Private Sub AddSuggestion(ByVal suggestion As String)
            Dim tb As New TextBlock()
            Dim th As New C1TapHelper(tb)
            AddHandler th.DoubleTapped, AddressOf th_DoubleTapped
            tb.Text = suggestion
            _listSuggestions.Items.Add(tb)
        End Sub

        ' double-click selects word and changes it
        Private Sub th_DoubleTapped(ByVal sender As Object, ByVal e As C1TappedEventArgs)
            Dim tb = TryCast(_listSuggestions.SelectedItem, TextBlock)
            If tb IsNot Nothing AndAlso tb.Text = _txtChangeTo.Text Then
                _btnChange_Click(Me, Nothing)
            End If
        End Sub

        ' disable ChangeAll button when deleting words
        Private Sub UpdateButtonStatus()
            ' update button status
            If _txtChangeTo.Text.Length > 0 Then
                ' update change buttons
                _btnChange.Content = _lblChange.Text
                _btnChangeAll.IsEnabled = _errors.Count > 0 AndAlso Not CurrentError.Duplicate

                ' update suggest button
                Dim text As String = _txtChangeTo.Text
                If text.IndexOf(" "c) < 0 AndAlso (Not _listSuggestions.Items.Contains(text)) Then
                    Dim enable As Boolean = True
                    Dim i As Integer = 0
                    Do While i < text.Length AndAlso enable
                        If (Not Char.IsLetterOrDigit(text, i)) AndAlso text.Chars(i) <> "'"c Then
                            enable = False
                        End If
                        i += 1
                    Loop
                    _btnSuggest.IsEnabled = enable
                End If

                ' indicate whether current text is valid
                _txtChangeTo.Foreground = If(_spell.CheckText(_txtChangeTo.Text).Count = 0, _brNormalText, _brErrorText)
            Else
                _btnChange.Content = _lblRemove.Text
                _btnChangeAll.IsEnabled = False
                _btnSuggest.IsEnabled = False
                _txtChangeTo.Foreground = _brNormalText
            End If

            ' change default button
            AcceptButton = _btnChange
        End Sub

#End Region

        '------------------------------------------------------------------------
#Region "** event handlers"

        ' update ChangeTo text when a suggestion is selected
        Private Sub _listSuggestions_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            Dim tb = TryCast(_listSuggestions.SelectedItem, TextBlock)
            If tb IsNot Nothing AndAlso tb.Text <> _lblNoSuggestions.Text Then
                _txtChangeTo.Text = tb.Text
            End If
        End Sub

        ' move to the next error (ignore the current one)
        Private Sub _btnIgnore_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ErrorIndex += 1
        End Sub

        ' add current word to the ignore list in the spell-checker
        Private Sub _btnIgnoreAll_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            _spell.IgnoreList.Add(CurrentError.Text)
            _errors = _spell.CheckText(_editor.Text, CurrentError.Start)
            UpdateCurrentError()
        End Sub

        ' change current word into 'ChangeTo' value
        Private Sub _btnChange_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' save starting point to continue checking from here
            Dim start As Integer = CurrentError.Start

            ' get replacement text
            Dim replacement As String = _txtChangeTo.Text
            If replacement.Length = 0 Then
                ' if replacement is empty, expand over spaces and commas
                Dim delete As CharRange = CharRange.ExpandOverWhitespace(_editor.Text, CurrentError)
                _editor.Select(delete.Start, delete.Text.Length)
            End If

            ' replace word in text and re-check
            _editor.SelectedText = replacement
            _errors = _spell.CheckText(_editor.Text, start)

            ' update index
            ErrorIndex = 0
        End Sub

        ' change all instances of the current word into 'ChangeTo' value
        Private Sub _btnChangeAll_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            _changeAll(CurrentError.Text) = _txtChangeTo.Text
            _btnChange_Click(Me, e)
        End Sub

        ' add current word to the user dictionary
        Private Sub _btnAdd_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            _spell.UserDictionary.AddWord(CurrentError.Text)
            _errors = _spell.CheckText(_editor.Text, CurrentError.Start)
            ErrorIndex = 0
        End Sub

        ' update suggestion list with current content
        Private Sub _btnSuggest_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            UpdateSuggestions(_txtChangeTo.Text)
        End Sub

        ' update button status when ChangeTo text changes
        Private Sub _txtChangeTo_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
            UpdateButtonStatus()
        End Sub

        ' cancel dialog
        Private Sub _btnCancel_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            DialogResult = MessageBoxResult.Cancel
        End Sub

        ' handle enter/escape
        Private Sub C1SpellDialog_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            Select Case e.Key
                ' accept/ignore
                Case Key.Enter
                    If AcceptButton Is _btnChange Then
                        _btnChange_Click(Me, Nothing)
                    ElseIf AcceptButton Is _btnIgnore Then
                        _btnIgnore_Click(Me, Nothing)
                    End If

                    ' cancel spell-checking
                Case Key.Escape
                    DialogResult = MessageBoxResult.Cancel
            End Select
        End Sub
#End Region

    End Class
End Namespace
