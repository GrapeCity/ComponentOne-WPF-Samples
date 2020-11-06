Imports System.Text

Namespace CustomDialogs
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name)
			InitializeComponent()
		End Sub
		Private Sub UserControl_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' update toolbar buttons state
			sched1_StyleChanged(Nothing, Nothing)
		End Sub

		Private Sub btnToday_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			sched1.VisibleDates.BeginUpdate()
			sched1.VisibleDates.Clear()
			sched1.VisibleDates.Add(Date.Today)
			sched1.VisibleDates.EndUpdate()
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

	End Class
End Namespace
