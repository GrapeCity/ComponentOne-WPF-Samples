Imports System.Text
Imports C1.WPF.SpellChecker
Imports System.Reflection
Imports System.IO

Namespace SpellCheckerSamples
	''' <summary>
	''' Interaction logic for MultiLanguageDemo.xaml
	''' </summary>
	Partial Public Class MultiLanguageDemo
		Inherits UserControl
		' declare the C1SpellChecker
		Private _c1SpellChecker As New C1SpellChecker()

		Public Sub New()
			InitializeComponent()

			AddHandler Loaded, AddressOf MultiLanguage_Loaded
		End Sub

		Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_c1SpellChecker.CheckControlAsync(_plainTextBox)
		End Sub

		Private Sub MultiLanguage_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			AddHandler cmbLanguage.SelectionChanged, AddressOf cmbLanguage_SelectionChanged
			cmbLanguage.ItemTemplate = CType(Resources("DictionaryTemplate"), DataTemplate)
			cmbLanguage.ItemsSource = SpellDictionaryWrapper.GetLanguages()
			cmbLanguage.SelectedIndex = 0

			' load sample text into text boxes
            Using stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("test.txt")
                Using sr = New StreamReader(stream)
                    Dim text = sr.ReadToEnd()
                    _plainTextBox.Text = text
                End Using
            End Using
		End Sub

		Private Sub cmbLanguage_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			GetDictionaryFromResources()
		End Sub

		' keep text selected while spell-checking dialog is open
		Private Sub _plainTextBox_LostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			e.Handled = True
		End Sub

		Private Sub GetDictionaryFromResources()
			Dim dctWrapper = CType(cmbLanguage.SelectedItem, SpellDictionaryWrapper)
			If dctWrapper IsNot Nothing Then
				' load the dictionary from the server
				Dim dict = _c1SpellChecker.MainDictionary
				dict.Load(Application.GetResourceStream(New Uri("/" & New AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName).Name & ";component" & dctWrapper.Address, UriKind.Relative)).Stream)

				' dictionary loaded, update status
				If _c1SpellChecker.MainDictionary.State = DictionaryState.Loaded Then
					WriteLine("Loaded {0} dictionary ({1:n0} words).", dctWrapper.Name, _c1SpellChecker.MainDictionary.WordCount)
				Else
					WriteLine("Failed to load {0} dictionary: {1}", dctWrapper.Name, dict.State)
				End If
			End If
		End Sub

		'------------------------------------------------------------
		#Region "** utilities"

		Private Sub WriteLine(ByVal format As String, ParamArray ByVal args() As Object)
			WriteLine(String.Format(format, args))
		End Sub
		Private Sub WriteLine(ByVal text As String)
			_outputTextBox.Text = text & vbCrLf
		End Sub

		#End Region


	End Class

	#Region "SpellDictionaryWrapper"

	Public Class SpellDictionaryWrapper
        Public Shared Function GetLanguages() As List(Of SpellDictionaryWrapper)
            Dim list = New List(Of SpellDictionaryWrapper)()
            list.Add(New SpellDictionaryWrapper() With {.Address = "/Resources/C1Spell_en-US.dct", .Name = "English (USA)", .FlagUri = New Uri("/Resources/USA.png", UriKind.Relative)})
            list.Add(New SpellDictionaryWrapper() With {.Address = "/Resources/C1Spell_fr-FR.dct", .Name = "French (France)", .FlagUri = New Uri("/Resources/France.png", UriKind.Relative)})
            Return list
        End Function

        Public Property Address() As String
		Public Property Name() As String
		Public Property FlagUri() As Uri
	End Class

	#End Region
End Namespace
