Imports System.Text
Imports C1.WPF.SpellChecker
Imports System.Reflection
Imports System.IO
Imports System.ComponentModel
Imports C1.WPF
Imports System.Net

Namespace SpellCheckerSamples
	''' <summary>
	''' Interaction logic for SpellCheckerDemo.xaml
	''' </summary>
	Partial Public Class SpellCheckerDemo
		Inherits UserControl
		' declare the C1SpellChecker
		Private _c1SpellChecker As New C1SpellChecker()

		Public Sub New()
			InitializeComponent()
			AddHandler Loaded, AddressOf Page_Loaded
		End Sub
		Private Sub Page_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' connect toolbar to C1RichTextBox
			_rtbToolbar.RichTextBox = _richTextBox
			_richTextBox.SpellChecker = _c1SpellChecker

			' bind DataGrid
            _dataGrid.ItemsSource = Beatle.GetBeatles()

			' load sample text into text boxes
            Using stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("test.txt")
                Using sr = New StreamReader(stream)
                    Dim text = sr.ReadToEnd()
                    _plainTextBox.Text = text
                    _richTextBox.Text = text
                End Using
            End Using

			' set up ignore list
			Dim il As WordList = _c1SpellChecker.IgnoreList
			il.Add("ComponentOne")
			il.Add("Silverlight")


			' monitor events
            AddHandler _c1SpellChecker.BadWordFound, AddressOf _c1SpellChecker_BadWordFound
			AddHandler _c1SpellChecker.CheckControlCompleted, AddressOf _c1SpellChecker_CheckControlCompleted

			' load main dictionary
			_c1SpellChecker.MainDictionary.Load(Application.GetResourceStream(New Uri("/" & New AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName).Name & ";component/Resources/C1Spell_en-US.dct", UriKind.Relative)).Stream)

			' dictionary loaded, enable buttons
			If _c1SpellChecker.MainDictionary.State = DictionaryState.Loaded Then
				_btnModal.IsEnabled = True
				_btnBatch.IsEnabled = _btnModal.IsEnabled
				WriteLine("loaded main dictionary ({0:n0} words).", _c1SpellChecker.MainDictionary.WordCount)
			Else
				WriteLine("failed to load dictionary: {0}", _c1SpellChecker.MainDictionary.State)
			End If
			' load user dictionary
			'UserDictionary ud = _c1SpellChecker.UserDictionary;
			'ud.LoadFromIsolatedStorage("Custom.dct");

			' save user dictionary when app exits
			AddHandler App.Current.Exit, AddressOf App_Exit

			_cmbControl.SelectedIndex = 0
			' set focus to textbox
			'_plainTextBox.Focus();
		End Sub
		' app closing, save modified user dictionary into compressed isolated storage
		Private Sub App_Exit(ByVal sender As Object, ByVal e As EventArgs)
			'UserDictionary ud = _c1SpellChecker.UserDictionary;
			'ud.SaveToIsolatedStorage("Custom.dct");

		End Sub

		'------------------------------------------------------------
		#Region "** batch spell-checking"

		Private Sub _btnBatch_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' check a word
			For Each word As String In "yes;no;yesno;do;don't;ain't;aint".Split(";"c)
				WriteLine("CheckWord(""{0}"") = {1}", word, _c1SpellChecker.CheckWord(word))
			Next word

			' check some text
			Dim someText = "this text comtains two errrors."
			Dim errors = _c1SpellChecker.CheckText(someText)
			WriteLine("CheckText(""{0}"") =", someText)
			For Each [error] In errors
				WriteLine(vbTab & "{0}, {1}-{2}", [error].Text, [error].Start, [error].Length)
				For Each suggestion As String In _c1SpellChecker.GetSuggestions([error].Text, 1000)
					WriteLine(vbTab & vbTab & "{0}?", suggestion)
				Next suggestion
			Next [error]

			' get suggestions
			Dim badWord = "traim"
			WriteLine("GetSuggestions(""{0}"")", badWord)
			For Each suggestion As String In _c1SpellChecker.GetSuggestions(badWord, 1000)
				WriteLine(vbTab & "{0}?", suggestion)
			Next suggestion
		End Sub

		#End Region

		'------------------------------------------------------------
		#Region "** modal spell-checking"

		Private Sub _btnModal_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' select control to spell-check
			Dim editor As Object = Nothing
			Select Case _cmbControl.SelectedIndex
				Case 0
					editor = _plainTextBox
				Case 1
					editor = _richTextBox
				Case 2
					editor = New DataGridSpellWrapper(_dataGrid)
			End Select

			' select type of modal dialog to use
			Dim dialog As C1Window = Nothing
			Select Case _cmbDialog.SelectedIndex
				' C1SpellChecker built-in dialog
				Case 0
					dialog = New C1SpellDialog()

				' Customized C1SpellChecker built-in dialog
				Case 1
					dialog = New C1SpellDialog()
					dialog.Header = "Customized Caption!"

				' Standard dialog
				Case 2
					dialog = New StandardSpellDialog()

				' Word-style dialog
				Case 3
					dialog = New WordSpellDialog()
			End Select

			' spell-check the control
			_c1SpellChecker.CheckControlAsync(editor, False, CType(dialog, ISpellDialog))
		End Sub

		#End Region

		'------------------------------------------------------------
		#Region "** event handlers"

		' select control being displayed/spell-checked
		Private Sub _cmbControl_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			If _cmbControl IsNot Nothing Then
				Dim index As Integer = _cmbControl.SelectedIndex

				' set visibility
				_plainTextBox.Visibility = If(index = 0, Visibility.Visible, Visibility.Collapsed)
				_richTextBoxPanel.Visibility = If(index = 1, Visibility.Visible, Visibility.Collapsed)
				_dataGrid.Visibility = If(index = 2, Visibility.Visible, Visibility.Collapsed)

				' set focus
				Select Case index
					Case 0
						_plainTextBox.Focus()
					Case 1
						_richTextBox.Focus()
					Case 2
						_dataGrid.Focus()
				End Select
			End If
		End Sub

		' monitor spell-checker events
		Private Sub _c1SpellChecker_CheckControlCompleted(ByVal sender As Object, ByVal e As CheckControlCompletedEventArgs)
			If Not e.Cancelled Then
				Dim msg = String.Format("Spell-check complete, {0} errors found.", e.ErrorCount)
				C1MessageBox.Show(msg, "Spelling")
			End If
			WriteLine("CheckControlCompleted: {0} errors found", e.ErrorCount)
			If e.Cancelled Then
				WriteLine(vbTab & "(cancelled...)")
			End If
		End Sub
		Private Sub _c1SpellChecker_BadWordFound(ByVal sender As Object, ByVal e As BadWordEventArgs)
			WriteLine("BadWordFound: ""{0}"" {1}", e.BadWord.Text,If(e.BadWord.Duplicate, "(duplicate)", String.Empty))
		End Sub

		' keep text selected while spell-checking dialog is open
		Private Sub _plainTextBox_LostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			e.Handled = True
		End Sub

		#End Region

		'------------------------------------------------------------
		#Region "** utilities"

		' trace execution
		Private Sub WriteLine(ByVal format As String, ParamArray ByVal args() As Object)
			WriteLine(String.Format(format, args))
		End Sub
		Private Sub WriteLine(ByVal text As String)
			_outputTextBox.Text += text & vbCrLf
		End Sub

		' helper class used for binding to the DataGrid
		Public Class Beatle
			Implements INotifyPropertyChanged
			Private _name, _comments As String


			Private Sub New()
			End Sub
			Public Property Name() As String
				Get
					Return _name
				End Get
				Set(ByVal value As String)
					_name = value
					RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Name"))
				End Set
			End Property
			Public Property Comments() As String
				Get
					Return _comments
				End Get
				Set(ByVal value As String)
					_comments = value
					RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comments"))
				End Set
			End Property
			Public Shared Function GetBeatles() As List(Of Beatle)
				Dim list = New List(Of Beatle)()
				list.Add(New Beatle With {.Name = "Paul McCartney", .Comments = "Great songriter, bass playier, singer"})
				list.Add(New Beatle With {.Name = "John Lennon", .Comments = "Great songriter, gitar playier, excelent singer"})
				list.Add(New Beatle With {.Name = "Ringo Starr", .Comments = "Great drummmer"})
				list.Add(New Beatle With {.Name = "George Harrison", .Comments = "Great guiter player, songriter, vocallist"})
				Return list
			End Function

			#Region "INotifyPropertyChanged Members"

			Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

			#End Region
		End Class
		#End Region


	End Class
End Namespace
