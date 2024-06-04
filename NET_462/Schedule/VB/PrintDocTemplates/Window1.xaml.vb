Imports System.Reflection
Imports C1.C1Preview
Imports C1.Schedule


Namespace PrintDocTemplates
	''' <summary>
	''' Interaction logic for Window1.xaml
	''' </summary>
	Partial Public Class Window1
		Inherits Window
		#Region "** fields"
		' C1PrintDocument controls
		Private _printDoc As New C1PrintDocument()
		' preview window
		Private _preview As PrintPreviewWindow
		#End Region

		#Region "** ctor"
		''' <summary>
		''' Initializes a ne instance of the <see cref="Window1"/> class.
		''' </summary>
		Public Sub New()
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name)
			InitializeComponent()

			Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30)

			minutesGroup.Visibility = Visibility.Visible

			Scheduler.CalendarHelper.StartDayTime = TimeSpan.Parse("09:00:00")
			Scheduler.Settings.FirstVisibleTime = TimeSpan.Parse("09:00:00")

			Scheduler.CalendarHelper.EndDayTime = TimeSpan.Parse("18:00:00")

			' subscribe to the DocumentStarting event
			AddHandler _printDoc.DocumentStarting, AddressOf _printDoc_DocumentStarting
		End Sub
		#End Region

		#Region "** object model"
		''' <summary>
		''' Gets or sets the <see cref="PrintPreviewWindow"/> object, used for previewing. 
		''' </summary>
		Public Property PrintPreviewDialog() As PrintPreviewWindow
			Get
				If _preview Is Nothing Then
					_preview = New PrintPreviewWindow()
				End If
				Return _preview
			End Get
			Set(ByVal value As PrintPreviewWindow)
				_preview = value
			End Set
		End Property
		#End Region

		#Region "** printing"
		' adds assembly references and initializes document tags
		Private Sub _printDoc_DocumentStarting(ByVal sender As Object, ByVal e As EventArgs)
			' add references needed for document scripts execution 
			_printDoc.ScriptingOptions.ExternalAssemblies.Add(Assembly.GetAssembly(GetType(C1.WPF.Schedule.C1Scheduler)).Location)
            _printDoc.ScriptingOptions.ExternalAssemblies.Add(Assembly.GetAssembly(GetType(System.Windows.Media.Color)).Location)

            _printDoc.ScriptingOptions.ExternalAssemblies.Add(Assembly.GetAssembly(GetType(C1.Schedule.Appointment)).Location)
            _printDoc.ScriptingOptions.ExternalAssemblies.Add(Assembly.GetAssembly(GetType(C1.WPF.Schedule.C1ScheduleStorage)).Location)
            _printDoc.ScriptingOptions.ExternalAssemblies.Add("WindowsBase.dll")
            _printDoc.ScriptingOptions.ExternalAssemblies.Add("netstandard.dll")

            _printDoc.ScriptingOptions.ExternalAssemblies.Add(Assembly.GetAssembly(GetType(System.Collections.Specialized.INotifyCollectionChanged)).Location)

            ' initialize document tags
            Dim start As Date = Scheduler.VisibleDates(0)
			Dim [end] As Date = Scheduler.VisibleDates(Scheduler.VisibleDates.Count - 1)

			Dim tag As Tag = _printDoc.Tags("StartDate")
			If tag IsNot Nothing AndAlso tag.Type Is GetType(Date) Then
				tag.Value = start
			End If
			tag = _printDoc.Tags("EndDate")
			If tag IsNot Nothing AndAlso tag.Type Is GetType(Date) Then
				tag.Value = [end]
			End If

			' show tag input form to user
			_printDoc.EditTags()

			tag = _printDoc.Tags("StartDate")
			If tag IsNot Nothing AndAlso tag.Type Is GetType(Date) Then
				start = CDate(tag.Value)
			End If
			tag = _printDoc.Tags("EndDate")
			If tag IsNot Nothing AndAlso tag.Type Is GetType(Date) Then
				[end] = CDate(tag.Value)
			End If

			tag = _printDoc.Tags("Appointment")
			If tag IsNot Nothing AndAlso tag.Type Is GetType(Appointment) AndAlso Scheduler.SelectedAppointment IsNot Nothing Then
				tag.Value = Scheduler.SelectedAppointment
			End If

			tag = _printDoc.Tags("CalendarInfo")
			If tag IsNot Nothing AndAlso tag.Type Is GetType(CalendarInfo) Then
				tag.Value = Scheduler.CalendarHelper.Info
			End If

			tag = _printDoc.Tags("Appointments")
			If tag IsNot Nothing Then
				If tag.Type Is GetType(AppointmentCollection) Then
					tag.Value = Scheduler.DataStorage.AppointmentStorage.Appointments
				ElseIf tag.Type Is GetType(IList(Of Appointment)) Then
					If Scheduler.SelectedAppointment IsNot Nothing Then
						Dim list As New List(Of Appointment)()
						list.Add(Scheduler.SelectedAppointment)
						tag.Value = list
					Else
						' get appointments for the currently selected SchedulerGroupItem if any, 
						' or all appointments otherwise.
						Dim list As AppointmentList = Scheduler.DataStorage.AppointmentStorage.Appointments.GetOccurrences(If(Scheduler.SelectedGroupItem Is Nothing, Nothing, Scheduler.SelectedGroupItem.Owner), Scheduler.GroupBy, start, [end].AddDays(1), True)
						list.Sort()
						tag.Value = list
					End If
				End If
			End If
		End Sub

		' create and preview daily style
		Private Sub day_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Utils.MakeDay(_printDoc)
			_printDoc.Save("day.c1d")
			' clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear()
			_printDoc.Load("day.c1d")
			Preview()
		End Sub

		' create and preview weekly style
		Private Sub week_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Utils.MakeWeek(_printDoc)
			_printDoc.Save("week.c1d")
			' clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear()
			_printDoc.Load("week.c1d")
			Preview()
		End Sub

		' create and preview monthly style
		Private Sub month_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Utils.MakeMonth(_printDoc)
			_printDoc.Save("month.c1d")
			' clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear()
			_printDoc.Load("month.c1d")
			Preview()
		End Sub

		' create and preview details style
		Private Sub detail_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Utils.MakeDetails(_printDoc)
			_printDoc.Save("details.c1d")
			' clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear()
			_printDoc.Load("details.c1d")
			Preview()
		End Sub

		' create and preview memo style
		Private Sub memo_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Utils.MakeMemo(_printDoc)
			_printDoc.Save("memo.c1d")
			' clear and re-load to be sure that all works when document is loaded from file
			_printDoc.Clear()
			_printDoc.Load("memo.c1d")
			Preview()
		End Sub

		''' <summary>
		''' Shows C1PrintDocument content in a print preview dialog.
		''' </summary>
		Private Sub Preview()
			' generate the document
			If PrintPreviewDialog IsNot Nothing Then
				_preview.Document = _printDoc.FixedDocumentSequence
				_preview.ShowDialog()
				_preview.Activate()
				_preview = Nothing
			End If
		End Sub
		#End Region

		#Region "** navigation"
		Private Sub rbChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			ChangeStyle()
		End Sub

		Private Sub ChangeStyle()
			If Not IsLoaded Then
				Return
			End If
            Dim curCursor As System.Windows.Input.Cursor = Cursor
            Cursor = System.Windows.Input.Cursors.Wait
			Scheduler.BeginUpdate()
			Try
				If oneDayRb.IsChecked.Value Then
					Scheduler.ChangeStyle(Scheduler.OneDayStyle)
					minutesGroup.Visibility = Visibility.Visible
					minutesChecked(Nothing, Nothing)
				ElseIf workingWeekRb.IsChecked.Value Then
					Scheduler.ChangeStyle(Scheduler.WorkingWeekStyle)
					minutesGroup.Visibility = Visibility.Visible
					minutesChecked(Nothing, Nothing)
				ElseIf weekRb.IsChecked.Value Then
					minutesGroup.Visibility = Visibility.Visible
					Scheduler.ChangeStyle(Scheduler.WeekStyle)
					minutesChecked(Nothing, Nothing)
				Else
					minutesGroup.Visibility = Visibility.Collapsed
					Scheduler.ChangeStyle(Scheduler.MonthStyle)
				End If
			Finally
				Scheduler.EndUpdate()
				Cursor = curCursor
			End Try
		End Sub

		Private Sub minutesChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If Not IsLoaded Then
				Return
			End If
            Dim curCursor As System.Windows.Input.Cursor = Cursor
            Cursor = System.Windows.Input.Cursors.Wait
			Try
				If m60.IsChecked.Value Then
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(60)
				ElseIf m30.IsChecked.Value Then
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30)
				ElseIf m15.IsChecked.Value Then
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(15)
				End If
			Finally
				Cursor = curCursor
			End Try
		End Sub

		Private Sub btnToday_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim targetDate As Date = Date.Today
            If Scheduler.Style IsNot Scheduler.OneDayStyle Then
                ' find the first week day
                Do While targetDate.DayOfWeek <> Scheduler.CalendarHelper.WeekStart
                    targetDate = targetDate.AddDays(-1)
                Loop
            End If

			' navigate to the current week 
			Scheduler.VisualStartTime = targetDate
		End Sub

		Private Sub btnPrev_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Scheduler.Style Is Scheduler.WorkingWeekStyle OrElse Scheduler.Style Is Scheduler.WeekStyle Then
                Scheduler.VisualStartTime = Scheduler.VisualStartTime.AddDays(-7)
            Else
                Scheduler.VisualStartTime = Scheduler.VisualStartTime.Add(Scheduler.VisualTimeSpan.Negate())
            End If

		End Sub
		Private Sub btnNext_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Scheduler.Style Is Scheduler.WorkingWeekStyle OrElse Scheduler.Style Is Scheduler.WeekStyle Then
                Scheduler.VisualStartTime = Scheduler.VisualStartTime.AddDays(7)
            Else
                Scheduler.VisualStartTime = Scheduler.VisualStartTime.Add(Scheduler.VisualTimeSpan)
            End If
		End Sub

		Private Sub Scheduler_LayoutUpdated(ByVal sender As Object, ByVal e As EventArgs)
			' refresh buttons' state if Scheduler's layout has been changed
			' by Scheduler's commands defined in default templates.
            If Scheduler.Style Is Scheduler.OneDayStyle Then
                oneDayRb.IsChecked = True
                minutesGroup.Visibility = Visibility.Visible
            ElseIf Scheduler.Style Is Scheduler.WorkingWeekStyle Then
                workingWeekRb.IsChecked = True
                minutesGroup.Visibility = Visibility.Visible
            ElseIf Scheduler.Style Is Scheduler.WeekStyle Then
                weekRb.IsChecked = True
                minutesGroup.Visibility = Visibility.Visible
            Else
                minutesGroup.Visibility = Visibility.Collapsed
                monthRb.IsChecked = True
            End If
		End Sub
		#End Region
	End Class
End Namespace
