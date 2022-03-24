Imports System.Text
Imports C1.WPF.Schedule
Imports C1.C1Schedule
Imports System.ComponentModel
Imports System.Collections.Specialized

Namespace MultiUser
	''' <summary>
	''' Interaction logic for Window1.xaml
	''' </summary>
	Partial Public Class Window1
		Inherits Window
		#Region "** fields"
		Private appointmentsTableAdapter As MultiUser.NwindDataSetTableAdapters.AppointmentsTableAdapter = New NwindDataSetTableAdapters.AppointmentsTableAdapter()
		Private employeesTableAdapter As MultiUser.NwindDataSetTableAdapters.EmployeesTableAdapter = New NwindDataSetTableAdapters.EmployeesTableAdapter()
		Private customersTableAdapter As MultiUser.NwindDataSetTableAdapters.CustomersTableAdapter = New NwindDataSetTableAdapters.CustomersTableAdapter()
		Private dataSet As New NwindDataSet()
		#End Region

		#Region "** initializing"
		Public Sub New()
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name)
			InitializeComponent()

			AddHandler Scheduler.ReminderFire, AddressOf scheduler_ReminderFire

			AddHandler Scheduler.GroupItems.CollectionChanged, AddressOf GroupItems_CollectionChanged

			' get data from the data base
			Me.employeesTableAdapter.Fill(dataSet.Employees)
			Me.customersTableAdapter.Fill(dataSet.Customers)
			Me.appointmentsTableAdapter.Fill(dataSet.Appointments)

			' set mappings and DataSource for the AppointmentStorages
			Dim storage As AppointmentStorage = Scheduler.DataStorage.AppointmentStorage
			storage.Mappings.AppointmentProperties.MappingName = "Properties"
			storage.Mappings.Body.MappingName = "Description"
			storage.Mappings.End.MappingName = "End"
			storage.Mappings.IdMapping.MappingName = "AppointmentId"
			storage.Mappings.Location.MappingName = "Location"
			storage.Mappings.Start.MappingName = "Start"
			storage.Mappings.Subject.MappingName = "Subject"
			storage.Mappings.OwnerIndexMapping.MappingName = "Owner"
			storage.DataSource = dataSet.Appointments

			' set mappings and DataSource for the OwnerStorage
			Dim ownerStorage As ContactStorage = Scheduler.DataStorage.OwnerStorage
			AddHandler(CType(ownerStorage.Contacts, INotifyCollectionChanged)).CollectionChanged, AddressOf Owners_CollectionChanged
			ownerStorage.Mappings.IndexMapping.MappingName = "EmployeeId"
			ownerStorage.Mappings.TextMapping.MappingName = "FirstName"
			ownerStorage.DataSource = dataSet.Employees

			' set mappings and DataSource for the ContactStorage
			Dim cntStorage As ContactStorage = Scheduler.DataStorage.ContactStorage
			AddHandler(CType(cntStorage.Contacts, INotifyCollectionChanged)).CollectionChanged, AddressOf Contacts_CollectionChanged
			cntStorage.Mappings.IdMapping.MappingName = "CustomerId"
			cntStorage.Mappings.TextMapping.MappingName = "CompanyName"
			cntStorage.DataSource = dataSet.Customers

			btnDay.IsChecked = True
			AddHandler Scheduler.StyleChanged, AddressOf Scheduler_StyleChanged
		End Sub

		Private Sub GroupItems_CollectionChanged(ByVal sender As Object, ByVal e As NotifyCollectionChangedEventArgs)
			For Each group As SchedulerGroupItem In Scheduler.GroupItems
				If group.Owner IsNot Nothing Then
					' set SchedulerGroupItem.Tag property to the data row. That allows to use data row fields in xaml for binding
					Dim index As Integer = CInt(Fix(group.Owner.Key(0)))
					Dim row As MultiUser.NwindDataSet.EmployeesRow = TryCast(dataSet.Employees.Rows.Find(index), MultiUser.NwindDataSet.EmployeesRow)
					group.Tag = row
				End If
			Next group
		End Sub

		Private Sub Owners_CollectionChanged(ByVal sender As Object, ByVal e As NotifyCollectionChangedEventArgs)
			If e.Action = NotifyCollectionChangedAction.Add Then
				For Each cnt As Contact In e.NewItems
					Dim row As MultiUser.NwindDataSet.EmployeesRow = TryCast(dataSet.Employees.Rows.Find(cnt.Key(0)), MultiUser.NwindDataSet.EmployeesRow)
					If row IsNot Nothing Then
						' set Contact.MenuCaption to the FirstName + LastName string
						cnt.MenuCaption = row("FirstName").ToString() & " " & row("LastName").ToString()
					End If
				Next cnt
			End If
		End Sub
		Private Sub Contacts_CollectionChanged(ByVal sender As Object, ByVal e As NotifyCollectionChangedEventArgs)
			If e.Action = NotifyCollectionChangedAction.Add Then
				For Each cnt As Contact In e.NewItems
					Dim row As MultiUser.NwindDataSet.CustomersRow = TryCast(dataSet.Customers.Rows.Find(cnt.Key(0)), MultiUser.NwindDataSet.CustomersRow)
					If row IsNot Nothing Then
						' set Contact.MenuCaption to the CompanyName + ContactName string
						cnt.MenuCaption = row("CompanyName").ToString() & " (" & row("ContactName").ToString() & ")"
					End If
				Next cnt
			End If
		End Sub

		#End Region

		#Region "** misc"
		' save changes
		Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
			Me.appointmentsTableAdapter.Update(dataSet.Appointments)
			MyBase.OnClosing(e)
		End Sub
		' prevent showing reminders
		Private Sub scheduler_ReminderFire(ByVal sender As Object, ByVal e As ReminderActionEventArgs)
			e.Handled = True
		End Sub
		Private Sub Scheduler_StyleChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Scheduler.Style Is Scheduler.TimeLineStyle Then
                ' update group header (use different headers for TimeLine and other views)
                Scheduler.GroupHeaderTemplate = CType(Resources("myCustomTimeLineGroupHeaderTemplate"), DataTemplate)
                btnTimeLine.IsChecked = True
            Else
                ' update group header (use different headers for TimeLine and other views)
                Scheduler.GroupHeaderTemplate = CType(Resources("myCustomGroupHeaderTemplate"), DataTemplate)
                ' update toolbar buttons state according to the current C1Scheduler view
                If Scheduler.Style Is Scheduler.WorkingWeekStyle Then
                    btnWorkWeek.IsChecked = True
                ElseIf Scheduler.Style Is Scheduler.WeekStyle Then
                    btnWeek.IsChecked = True
                ElseIf Scheduler.Style Is Scheduler.MonthStyle Then
                    btnMonth.IsChecked = True
                Else
                    btnDay.IsChecked = True
                End If
            End If
		End Sub
		#End Region
	End Class
End Namespace
