Imports System.Globalization
Imports System.Threading

Imports C1.C1Schedule
Imports C1.WPF
Imports C1.WPF.DateTimeEditors
Imports C1.WPF.Localization
Imports C1.WPF.Schedule

Namespace CustomDialogs
	''' <summary>
	''' The <see cref="EditAppointmentControl"/> control allows editing of all appointment properties.
	''' </summary>
	Partial Public Class EditAppointmentControl
		Inherits UserControl
		#Region "** fields"
		Private _parentWindow As ContentControl = Nothing
		Private _appointment As Appointment
		Private _scheduler As C1Scheduler
		Private _isLoaded As Boolean = False

		Private _defaultStart As TimeSpan
		Private _defaultDuration As TimeSpan
#End Region

		#Region "** initialization"
		''' <summary>
		''' Creates the new instance of the <see cref="EditAppointmentControl"/> class.
		''' </summary>
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub EditAppointmentControl_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If Not _isLoaded Then
				_appointment = TryCast(DataContext, Appointment)
				_defaultStart = CType(IIf(_appointment.AllDayEvent, TimeSpan.FromHours(8), _appointment.Start.TimeOfDay), TimeSpan)
				_defaultDuration = CType(IIf(_appointment.AllDayEvent, TimeSpan.FromMinutes(30), _appointment.Duration), TimeSpan)
				If Not System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted Then
					_parentWindow = CType(VTreeHelper.GetParentOfType(Me, GetType(Window)), ContentControl)
				Else
					_parentWindow = CType(VTreeHelper.GetParentOfType(Me, GetType(C1Window)), ContentControl)
				End If
				If _parentWindow IsNot Nothing Then
					Dim bnd As New Binding("Header")
					bnd.Source = Me
					If TypeOf _parentWindow Is Window Then
						_parentWindow.SetBinding(Window.TitleProperty, bnd)
					Else
						_parentWindow.SetBinding(C1Window.HeaderProperty, bnd)
					End If

					If TypeOf _parentWindow Is Window Then
						AddHandler(CType(_parentWindow, Window)).Closed, AddressOf _parentWindow_Closed
					Else
						AddHandler(CType(_parentWindow, C1Window)).Closed, AddressOf _parentWindow_Closed
					End If
				End If
				If _appointment IsNot Nothing Then
					AddHandler(CType(_appointment, System.ComponentModel.INotifyPropertyChanged)).PropertyChanged, AddressOf appointment_PropertyChanged
					If _appointment.ParentCollection IsNot Nothing Then
                        _scheduler = CType(_appointment.ParentCollection.ParentStorage.ScheduleStorage, C1ScheduleStorage).Scheduler
						If _appointment.AllDayEvent Then
							_defaultStart = _scheduler.CalendarHelper.StartDayTime
							_defaultDuration = _scheduler.CalendarHelper.Info.TimeScale
						End If
					End If
					UpdateWindowHeader()
					UpdateRecurrenceState()
					UpdateCollections()
					UpdateEndCalendar()
					If _appointment.AllDayEvent Then
						endCalendar.EditMode = C1DateTimePickerEditMode.Date
						startCalendar.EditMode = endCalendar.EditMode
					Else
						endCalendar.EditMode = C1DateTimePickerEditMode.DateTime
						startCalendar.EditMode = endCalendar.EditMode
					End If
					UpdateEditingControls()
				End If
				If _parentWindow IsNot Nothing AndAlso _appointment IsNot Nothing Then
					_isLoaded = True
				End If
			End If
			subject.Focus()
		End Sub

		Protected Overrides Sub OnMouseEnter(ByVal e As MouseEventArgs)
			MyBase.OnMouseEnter(e)
			UpdateCollections()
		End Sub
		Protected Overrides Sub OnGotFocus(ByVal e As RoutedEventArgs)
			MyBase.OnGotFocus(e)
			UpdateCollections()
		End Sub

		Private Sub _parentWindow_Closed(ByVal sender As Object, ByVal e As EventArgs)
			RemoveHandler(CType(_appointment, System.ComponentModel.INotifyPropertyChanged)).PropertyChanged, AddressOf appointment_PropertyChanged
			If TypeOf _parentWindow Is Window Then
				RemoveHandler(CType(_parentWindow, Window)).Closed, AddressOf _parentWindow_Closed
			Else
				RemoveHandler(CType(_parentWindow, C1Window)).Closed, AddressOf _parentWindow_Closed
			End If
		End Sub

		#End Region

		#Region "** object model"
		''' <summary>
		''' Gets or sets an <see cref="Appointment"/> object representing current DataContext.
		''' </summary>
		Public Property Appointment() As Appointment
			Get
				Return _appointment
			End Get
			Set(ByVal value As Appointment)
				_appointment = value
				If _parentWindow IsNot Nothing Then
					_parentWindow.DataContext = value
					_parentWindow.Content = _parentWindow.DataContext
				End If
				DataContext = value
				If _appointment IsNot Nothing Then
					UpdateWindowHeader()
					UpdateRecurrenceState()
					UpdateCollections()
				End If
			End Set
		End Property

		''' <summary>
		''' Gets a <see cref="String"/> value which can be used as an Appointment window header.
		''' </summary>
		Public Property Header() As String
			Get
				Return CStr(GetValue(HeaderProperty))
			End Get
			Private Set(ByVal value As String)
				SetValue(HeaderProperty, value)
			End Set
		End Property

		''' <summary>
		''' Identifies the <see cref="Header"/> dependency property.
		''' </summary>
		Private Shared ReadOnly HeaderProperty As DependencyProperty = DependencyProperty.Register("Header", GetType(String), GetType(EditAppointmentControl), New PropertyMetadata(String.Empty))

		''' <summary>
		''' Gets recurrence pattern description.
		''' </summary>
		Public Property PatternDescription() As String
			Get
				Return CStr(GetValue(PatternDescriptionProperty))
			End Get
			Private Set(ByVal value As String)
				SetValue(PatternDescriptionProperty, value)
			End Set
		End Property

		''' <summary>
		''' Identifies the <see cref="PatternDescription"/> dependency property.
		''' </summary>
		Public Shared ReadOnly PatternDescriptionProperty As DependencyProperty = DependencyProperty.Register("PatternDescription", GetType(String), GetType(EditAppointmentControl), New PropertyMetadata(String.Empty))

		#End Region

		#Region "** private stuff"
		Private Sub UpdateWindowHeader()
			Dim result As String
			Dim subject As String = String.Empty
			Dim allDay As Boolean = False
			If _appointment IsNot Nothing Then
				subject = _appointment.Subject
				allDay = chkAllDay.IsChecked.Value
			End If
			If String.IsNullOrEmpty(subject) Then
				subject = C1Localizer.GetString("EditAppointment", "Untitled", "Untitled")
			End If
			If allDay Then
				result = C1Localizer.GetString("EditAppointment", "Event", "Event") & " - " & subject
			Else
				result = C1Localizer.GetString("EditAppointment", "Appointment", "Appointment") & " - " & subject
			End If

			Header = result
		End Sub

		Private Sub UpdateRecurrenceState()
			Select Case _appointment.RecurrenceState
				Case RecurrenceStateEnum.Master
					PatternDescription = Appointment.GetRecurrencePattern().Description
					startEndPanel.Visibility = Visibility.Collapsed
					recurrenceInfoPanel.Visibility = Visibility.Visible
				Case Else
					PatternDescription = String.Empty
					startEndPanel.Visibility = Visibility.Visible
					recurrenceInfoPanel.Visibility = Visibility.Collapsed
			End Select
		End Sub

		Private Sub appointment_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
			UpdateRecurrenceState()
			UpdateEndCalendar()
		End Sub

		Private Sub LayoutRoot_BindingValidationError(ByVal sender As Object, ByVal e As ValidationErrorEventArgs)
			If e.Action = ValidationErrorEventAction.Added Then
				PART_DialogSaveButton.IsEnabled = False
				saveAsButton.IsEnabled = False
				reccButton.IsEnabled = False
			Else
				PART_DialogSaveButton.IsEnabled = True
				saveAsButton.IsEnabled = True
				reccButton.IsEnabled = True
			End If
		End Sub

		Private Sub SetAppointment()
			subject.Focus()
			location.GetBindingExpression(TextBox.TextProperty).UpdateSource()
			body.GetBindingExpression(TextBox.TextProperty).UpdateSource()
		End Sub

		Private Sub UpdateCollections()
			If Appointment IsNot Nothing Then
				resources.Text = Appointment.Resources.ToString()
				contacts.Text = Appointment.Links.ToString()
				categories.Text = Appointment.Categories.ToString()
			End If
		End Sub

		Private Sub UpdateEditingControls()
			If _scheduler IsNot Nothing Then
				Dim settings As C1SchedulerSettings = _scheduler.Settings
				contacts.ParentAppointment = _appointment
				resources.ParentAppointment = contacts.ParentAppointment
				categories.ParentAppointment = resources.ParentAppointment
				contacts.Scheduler = _scheduler
				resources.Scheduler = contacts.Scheduler
				categories.Scheduler = resources.Scheduler
				If (Not settings.AllowCategoriesEditing) AndAlso _scheduler.DataStorage.CategoryStorage.Categories.Count = 0 Then
					categories.Visibility = Visibility.Collapsed
				Else
					categories.SourceCollection = _scheduler.DataStorage.CategoryStorage.Categories
					categories.TargetCollection = _appointment.Categories
					categories.ItemType = GetType(Category)
					categories.WindowTitle = "Categories"
					categories.ShowButton = settings.AllowCategoriesMultiSelection OrElse settings.AllowCategoriesEditing
				End If
				If (Not settings.AllowResourcesEditing) AndAlso _scheduler.DataStorage.ResourceStorage.Resources.Count = 0 Then
					resources.Visibility = Visibility.Collapsed
				Else
					resources.SourceCollection = _scheduler.DataStorage.ResourceStorage.Resources
					resources.TargetCollection = _appointment.Resources
					resources.ItemType = GetType(Resource)
					resources.WindowTitle = "Resources"
					resources.ShowButton = settings.AllowResourcesEditing OrElse settings.AllowResourcesMultiSelection
				End If
				If (Not settings.AllowContactsEditing) AndAlso _scheduler.DataStorage.ContactStorage.Contacts.Count = 0 Then
					contacts.Visibility = Visibility.Collapsed
				Else
					contacts.SourceCollection = _scheduler.DataStorage.ContactStorage.Contacts
					contacts.TargetCollection = _appointment.Links
					contacts.ItemType = GetType(Contact)
					contacts.WindowTitle = "Contacts"
					contacts.ShowButton = settings.AllowContactsEditing OrElse settings.AllowContactsMultiSelection
				End If
			End If
		End Sub

		Private Sub PART_DialogSaveButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			SetAppointment()
			If TypeOf _parentWindow Is Window Then
				_parentWindow.Tag = "true"
				CType(_parentWindow, Window).Close()
			Else
				CType(_parentWindow, C1Window).DialogResult = MessageBoxResult.OK
			End If
		End Sub

		Private Sub saveAsButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			SetAppointment()
		End Sub

		Private Sub subject_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			subject.GetBindingExpression(TextBox.TextProperty).UpdateSource()
			UpdateWindowHeader()
		End Sub

		Private Sub chkAllDay_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			endCalendar.EditMode = C1DateTimePickerEditMode.Date
			startCalendar.EditMode = endCalendar.EditMode
			UpdateWindowHeader()
		End Sub

		Private Sub chkAllDay_Unchecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_appointment.Start = _appointment.Start.Add(_defaultStart)
			_appointment.Duration = _defaultDuration
			endCalendar.EditMode = C1DateTimePickerEditMode.DateTime
			startCalendar.EditMode = endCalendar.EditMode
			UpdateWindowHeader()
		End Sub

		Private Sub endCalendar_DateTimeChanged(ByVal sender As Object, ByVal e As NullablePropertyChangedEventArgs(Of Date))
			If _appointment IsNot Nothing Then
				Dim [end] As Date = endCalendar.DateTime.Value
				If _appointment.AllDayEvent Then
					[end] = [end].AddDays(1)
				End If
				If [end] < Appointment.Start Then
					endCalendar.Foreground = New SolidColorBrush(Colors.Red)
					endCalendar.BorderBrush = endCalendar.Foreground
					endCalendar.BorderThickness = New Thickness(2)
					ToolTipService.SetToolTip(endCalendar, C1Localizer.GetString("Exceptions", "StartEndValidationFailed", "The End value should be greater than Start value."))
					saveAsButton.IsEnabled = False
					PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled
				Else
					_appointment.End = [end]
					If Not PART_DialogSaveButton.IsEnabled Then
						saveAsButton.IsEnabled = True
						PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled
						endCalendar.ClearValue(Control.ForegroundProperty)
						endCalendar.ClearValue(Control.BorderBrushProperty)
						endCalendar.ClearValue(Control.BorderThicknessProperty)
						endCalendar.ClearValue(ToolTipService.ToolTipProperty)
					End If
				End If
			End If
		End Sub

		Private Sub UpdateEndCalendar()
			Dim [end] As Date = _appointment.End
			If _appointment.AllDayEvent Then
				[end] = [end].AddDays(-1)
			End If
			endCalendar.DateTime = [end]
			If Not PART_DialogSaveButton.IsEnabled Then
				saveAsButton.IsEnabled = True
				PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled
				endCalendar.ClearValue(Control.BackgroundProperty)
				endCalendar.ClearValue(Control.ForegroundProperty)
				endCalendar.ClearValue(Control.BorderBrushProperty)
				endCalendar.ClearValue(Control.BorderThicknessProperty)
				endCalendar.ClearValue(ToolTipService.ToolTipProperty)
			End If
		End Sub

        Protected Overrides Sub OnMouseWheel(ByVal e As System.Windows.Input.MouseWheelEventArgs)
            e.Handled = True
            MyBase.OnMouseWheel(e)
        End Sub
		Protected Overrides Sub OnPreviewKeyDown(ByVal e As KeyEventArgs)
			If e.Key = Key.Escape Then
				Dim parentControl As DependencyObject = VTreeHelper.GetParentOfType(CType(e.OriginalSource, DependencyObject), Me.GetType())
				If parentControl IsNot Nothing Then
					If TypeOf _parentWindow Is Window Then
						CType(_parentWindow, Window).Close()
					Else
						CType(_parentWindow, C1Window).DialogResult = MessageBoxResult.Cancel
					End If
				End If
			End If
			MyBase.OnPreviewKeyDown(e)
		End Sub

		#End Region
	End Class
End Namespace
