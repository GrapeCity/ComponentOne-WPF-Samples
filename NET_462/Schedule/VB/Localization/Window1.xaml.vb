Imports System.Text
Imports System.Globalization
Imports System.Threading

Namespace Localization
	''' <summary>
	''' Interaction logic for Window1.xaml
	''' </summary>
	Partial Public Class Window1
		Inherits Window
		Public Sub New()
			InitializeComponent()
			cmbCultures.Items.Add("default")
			cmbCultures.Items.Add("ar-SA")
			cmbCultures.Items.Add("cs-CZ")
			cmbCultures.Items.Add("da-DK")
			cmbCultures.Items.Add("de-DE")
			cmbCultures.Items.Add("el-GR")
			cmbCultures.Items.Add("en-US")
			cmbCultures.Items.Add("es-ES")
			cmbCultures.Items.Add("fi-FI")
			cmbCultures.Items.Add("fr-FR")
			cmbCultures.Items.Add("he-IL")
			cmbCultures.Items.Add("it-IT")
			cmbCultures.Items.Add("ja-JP")
			cmbCultures.Items.Add("nl-NL")
			cmbCultures.Items.Add("nb-NO")
			cmbCultures.Items.Add("pl-PL")
			cmbCultures.Items.Add("pt-BR")
            cmbCultures.Items.Add("ro-RO")
            cmbCultures.Items.Add("ru-RU")
			cmbCultures.Items.Add("sk-SK")
			cmbCultures.Items.Add("sv-SE")
			cmbCultures.Items.Add("tr-TR")
            cmbCultures.Items.Add("zh")
            cmbCultures.Items.Add("zh-Hans")
            cmbCultures.Items.Add("zh-Hant")
            Dim currCulture As CultureInfo = CultureInfo.CurrentUICulture
			Dim o As Object = Nothing ' current culture
			For Each cultureName As String In cmbCultures.Items
				If Not cultureName.Equals("default") Then
					If cultureName.StartsWith(currCulture.TwoLetterISOLanguageName) OrElse (currCulture.TwoLetterISOLanguageName = "no" AndAlso cultureName.Equals("nb-NO")) Then
						o = cultureName
						Exit For
					End If
				End If
			Next cultureName
			If o IsNot Nothing Then
				cmbCultures.SelectedItem = o
			Else
				cmbCultures.SelectedIndex = 0
			End If
			AddHandler sched1.StyleChanged, AddressOf sched1_StyleChanged
			sched1_StyleChanged(Nothing, Nothing)
		End Sub

		Private Sub sched1_StyleChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' update toolbar buttons state according to the current C1Scheduler view
            If sched1.Style Is sched1.WorkingWeekStyle Then
                btnWorkWeek.IsChecked = True
            ElseIf sched1.Style Is sched1.WeekStyle Then
                btnWeek.IsChecked = True
            ElseIf sched1.Style Is sched1.MonthStyle Then
                btnMonth.IsChecked = True
            ElseIf sched1.Style Is sched1.OneDayStyle Then
                btnDay.IsChecked = True
            Else
                btnTimeLine.IsChecked = True
            End If
		End Sub

		Private Sub cmbCultures_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			Dim cultureName As String = If(cmbCultures.SelectedIndex = 0, "en-US", CStr(cmbCultures.SelectedItem))
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(cultureName)
			Thread.CurrentThread.CurrentUICulture = New CultureInfo(cultureName)
			Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture
		End Sub

	End Class
End Namespace
