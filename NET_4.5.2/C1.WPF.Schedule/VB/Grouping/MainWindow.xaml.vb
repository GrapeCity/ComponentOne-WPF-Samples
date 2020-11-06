Imports System.Text
Imports C1.C1Schedule

Namespace Grouping
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name)
			InitializeComponent()
			cmbGroup.Items.Add("None")
			cmbGroup.Items.Add("Category")
			cmbGroup.Items.Add("Resource")
			cmbGroup.Items.Add("Contact")
			cmbGroup.SelectedIndex = 3

			' add some resources
			Dim res As New Resource()
			res.Text = "Meeting room"
			res.Color = Color.FromArgb(255, 218, 186, 198)
			sched1.DataStorage.ResourceStorage.Resources.Add(res)
			Dim res1 As New Resource()
			res1.Text = "Conference hall"
			res1.Color = Color.FromArgb(255, 220, 236, 201)
			sched1.DataStorage.ResourceStorage.Resources.Add(res1)

			' add some contacts
			Dim cnt As New Contact()
			cnt.Text = "Andy Garcia"
			sched1.DataStorage.ContactStorage.Contacts.Add(cnt)
			Dim cnt1 As New Contact()
			cnt1.Text = "Nancy Drew"
			sched1.DataStorage.ContactStorage.Contacts.Add(cnt1)
			Dim cnt2 As New Contact()
			cnt2.Text = "Robert Clark"
			sched1.DataStorage.ContactStorage.Contacts.Add(cnt2)

			' add sample appointments
			Dim app As New Appointment(Date.Today.AddHours(12), TimeSpan.FromHours(1))
			app.Subject = "Sales meeting"
			sched1.DataStorage.AppointmentStorage.Appointments.Add(app)
			app.Resources.Add(res)
			app.Links.Add(cnt1)
			app.Links.Add(cnt2)

			app = New Appointment(Date.Today.AddHours(14), TimeSpan.FromHours(3))
			app.Subject = "Retirement Planning Session"
			app.Body = "A retirement planning education session. Please attend if possible."
			sched1.DataStorage.AppointmentStorage.Appointments.Add(app)
			app.Resources.Add(res1)
			app.Links.Add(cnt1)
			app.Links.Add(cnt2)
			app.Links.Add(cnt)

			app = New Appointment(Date.Today.AddHours(10), TimeSpan.FromMinutes(15))
			app.Subject = "Conference call"
			sched1.DataStorage.AppointmentStorage.Appointments.Add(app)
			app.Links.Add(cnt)

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

		Private Sub ComboBox_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			Dim str As String = CStr(cmbGroup.SelectedItem)
			If str = "None" Then
				sched1.GroupBy = String.Empty
			Else
				sched1.GroupBy = str
			End If
		End Sub
	End Class
End Namespace
