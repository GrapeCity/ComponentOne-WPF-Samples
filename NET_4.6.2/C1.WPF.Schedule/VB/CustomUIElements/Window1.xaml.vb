Imports C1.C1Schedule

Namespace CustomUIElements
	''' <summary>
	''' Interaction logic for Window1.xaml
	''' </summary>
	Partial Public Class Window1
		Inherits Window
		Public Sub New()
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name)
			InitializeComponent()

			Scheduler.NextAppointmentText = "NEXT"
			Scheduler.PreviousAppointmentText = "PREVIOUS"

			Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30)

			Scheduler.CalendarHelper.StartDayTime = TimeSpan.Parse("09:00:00")
			Scheduler.Settings.FirstVisibleTime = TimeSpan.Parse("09:00:00")

			Scheduler.CalendarHelper.EndDayTime = TimeSpan.Parse("18:00:00")

			Dim app As New Appointment(Date.Now, Date.Now.AddMinutes(45), "Test Appointment")
			app.Location = "Test location"
			Scheduler.DataStorage.AppointmentStorage.Appointments.Add(app)

			AddHandler Scheduler.StyleChanged, AddressOf Scheduler_StyleChanged
			Scheduler_StyleChanged(Nothing, Nothing)
		End Sub

		Private Sub Scheduler_StyleChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' update toolbar buttons state according to the current C1Scheduler view
            If Scheduler.Style Is Scheduler.WorkingWeekStyle Then
                btnWorkWeek.IsChecked = True
            ElseIf Scheduler.Style Is Scheduler.WeekStyle Then
                btnWeek.IsChecked = True
            ElseIf Scheduler.Style Is Scheduler.MonthStyle Then
                btnMonth.IsChecked = True
            ElseIf Scheduler.Style Is Scheduler.OneDayStyle Then
                btnDay.IsChecked = True
            Else
                btnTimeLine.IsChecked = True
            End If

		End Sub
	End Class
End Namespace
