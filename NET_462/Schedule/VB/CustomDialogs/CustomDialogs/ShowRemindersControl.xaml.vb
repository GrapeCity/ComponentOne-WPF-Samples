Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.Globalization
Imports System.Threading
Imports System.Windows.Threading

Imports C1.Schedule
Imports C1.WPF
Imports C1.WPF.Localization
Imports C1.WPF.Schedule

Namespace CustomDialogs
	''' <summary>
	''' The <see cref="ShowRemindersControl"/> displays currently active reminders and allows to control them.
	''' </summary>
	Partial Public Class ShowRemindersControl
		Inherits UserControl
		#Region "dependency properties"
		''' <summary>
		''' Gets or sets reference to the <see cref="C1Scheduler"/> control 
		''' which is an owner of displayed <see cref="Reminder"/> objects.
		''' </summary>
		Public Property Scheduler() As C1Scheduler
			Get
				Return CType(GetValue(SchedulerProperty), C1Scheduler)
			End Get
			Set(ByVal value As C1Scheduler)
				SetValue(SchedulerProperty, value)
			End Set
		End Property

		''' <summary>
		''' Identifies the <see cref="Scheduler"/> dependency property.
		''' </summary>
		Public Shared ReadOnly SchedulerProperty As DependencyProperty = DependencyProperty.Register("Scheduler", GetType(C1Scheduler), GetType(ShowRemindersControl), New PropertyMetadata(AddressOf OnSchedulerChanged))

		Private Shared Sub OnSchedulerChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			CType(d, ShowRemindersControl).OnSchedulerChanged(e)
		End Sub
		Private Sub OnSchedulerChanged(ByVal e As DependencyPropertyChangedEventArgs)
			Dim oldSch As C1Scheduler = TryCast(e.OldValue, C1Scheduler)
			If oldSch IsNot Nothing Then
				RemoveHandler(CType(oldSch.ActiveReminders, INotifyCollectionChanged)).CollectionChanged, AddressOf ShowRemindersControl_CollectionChanged
			End If
			If Scheduler IsNot Nothing Then
				AddHandler(CType(Scheduler.ActiveReminders, INotifyCollectionChanged)).CollectionChanged, AddressOf ShowRemindersControl_CollectionChanged
			End If
			UpdateTitle()
		End Sub
		#End Region

		#Region "** fields"
		Private _parentWindow As ContentControl = Nothing
		Private ReadOnly _timer As DispatcherTimer
		Private _isLoaded As Boolean = False
		#End Region

		#Region "** initializing"
		''' <summary>
		''' Initializes a new instance of the <see cref="ShowRemindersControl"/> class.
		''' </summary>
		Public Sub New()
			InitializeComponent()
			snoozeTimeSpansCombo.Value = TimeSpan.FromMinutes(5)
			_timer = New DispatcherTimer()
			AddHandler _timer.Tick, AddressOf Timer_Tick
		End Sub

		Private Sub root_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If Not _isLoaded Then
				If Not System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted Then
					_parentWindow = CType(VTreeHelper.GetParentOfType(Me, GetType(Window)), ContentControl)
				Else
					_parentWindow = CType(VTreeHelper.GetParentOfType(Me, GetType(C1Window)), ContentControl)
				End If
				If _parentWindow IsNot Nothing Then
					If Scheduler Is Nothing Then
						Scheduler = TryCast(_parentWindow.DataContext, C1Scheduler)
					End If
					If TypeOf _parentWindow Is Window Then
						AddHandler(CType(_parentWindow, Window)).Closed, AddressOf _parentWindow_Closed
					Else
						AddHandler(CType(_parentWindow, C1Window)).Closed, AddressOf _parentWindow_Closed
						AddHandler(CType(_parentWindow, C1Window)).WindowStateChanged, AddressOf _parentWindow_WindowStateChanged
					End If
				End If
				UpdateTitle()
				AddHandler remList.SelectionChanged, AddressOf remList_SelectionChanged
				UpdateTimer(1)
				remList.Focus()
				_isLoaded = True
			End If
			ShowRemindersControl_CollectionChanged(Nothing, Nothing)
		End Sub

		Private Sub _parentWindow_Closed(ByVal sender As Object, ByVal e As EventArgs)
			RemoveHandler _timer.Tick, AddressOf Timer_Tick
			_timer.Stop()
			If TypeOf _parentWindow Is Window Then
				RemoveHandler(CType(_parentWindow, Window)).Closed, AddressOf _parentWindow_Closed
			Else
				RemoveHandler(CType(_parentWindow, C1Window)).Closed, AddressOf _parentWindow_Closed
				RemoveHandler(CType(_parentWindow, C1Window)).WindowStateChanged, AddressOf _parentWindow_WindowStateChanged
			End If
			If Scheduler IsNot Nothing Then
				RemoveHandler(CType(Scheduler.ActiveReminders, INotifyCollectionChanged)).CollectionChanged, AddressOf ShowRemindersControl_CollectionChanged
			End If
			RemoveHandler remList.SelectionChanged, AddressOf remList_SelectionChanged
		End Sub

		' update reminders due is time on timer ticks
		Private Sub UpdateTimer(ByVal seconds As Integer)
			_timer.Stop()
			_timer.Interval = TimeSpan.FromSeconds(seconds)
			_timer.Start()
		End Sub
		Private Sub Timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
			_timer.Stop()
			If _parentWindow IsNot Nothing Then
				Dim [rem] As Reminder = TryCast(_parentWindow.Tag, Reminder)
				If [rem] IsNot Nothing AndAlso (_needChangeSelection OrElse remList.SelectedItems.Count = 0) Then
					_needChangeSelection = False
					remList.SelectedItem = [rem]
				End If
			End If
			UpdateTimer(20)
		End Sub
		Private Sub _parentWindow_WindowStateChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs(Of C1WindowState))
			If e.NewValue <> C1WindowState.Minimized Then
				UpdateTimer(1)
			End If
		End Sub
		#End Region

#Region "** private stuff"
		Private _needChangeSelection As Boolean = False
		Private Sub ShowRemindersControl_CollectionChanged(ByVal sender As Object, ByVal e As NotifyCollectionChangedEventArgs)
			UpdateTimer(1)
			_needChangeSelection = True
			If _parentWindow IsNot Nothing Then
				Dim [rem] As Reminder = TryCast(_parentWindow.Tag, Reminder)
				If [rem] IsNot Nothing Then
					remList.SelectedItem = [rem]
				End If
			End If
			UpdateTitle()
		End Sub

		Private Sub UpdateTitle()
			If _parentWindow IsNot Nothing Then
				Dim title As String = String.Empty
				Dim rems As ReadOnlyObservableCollection(Of Reminder) = Scheduler.ActiveReminders
				If rems Is Nothing OrElse rems.Count = 0 Then
					title = C1Localizer.GetString("ShowReminders", "Title", "Reminders")
				Else
					If rems.Count = 1 Then
						title = C1Localizer.GetString("ShowReminders", "OneReminder", "1 Reminder")
					Else
						title = String.Format(C1Localizer.GetString("ShowReminders", "RemindersNumber", "{0} Reminders"), rems.Count)
					End If
				End If
				If TypeOf _parentWindow Is Window Then
					CType(_parentWindow, Window).Title = title
				Else
					CType(_parentWindow, C1Window).Header = title
				End If
			End If
		End Sub

		Private Sub snoozeButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim ar As Array = New Object(1) {snoozeTimeSpansCombo.Value.Value, remList.SelectedItems}
			C1Scheduler.SnoozeCommand.Execute(ar, Scheduler)
		End Sub

		Private Sub remList_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			If remList.Items.Count > 0 AndAlso remList.SelectedItems.Count = 0 Then
				remList.SelectedIndex = 0
			End If
			UpdateSelectedInfo()
		End Sub

		Private Sub UpdateSelectedInfo()
			Dim count As Integer = remList.SelectedItems.Count
			If count = 1 Then
				subject.Visibility = Visibility.Visible
				info.Visibility = subject.Visibility
				selectedReminders.Visibility = Visibility.Collapsed
				snoozeButton.IsEnabled = True
				dismissButton.IsEnabled = True
				openButton.IsEnabled = True
			ElseIf count > 1 Then
				subject.Visibility = Visibility.Collapsed
				info.Visibility = subject.Visibility
				selectedReminders.Visibility = Visibility.Visible
				snoozeButton.IsEnabled = True
				dismissButton.IsEnabled = True
				openButton.IsEnabled = True
			Else
				subject.Visibility = Visibility.Collapsed
				info.Visibility = subject.Visibility
				selectedReminders.Visibility = Visibility.Visible
				snoozeButton.IsEnabled = False
				dismissButton.IsEnabled = False
				openButton.IsEnabled = False
			End If
		End Sub
		Protected Overrides Sub OnPreviewKeyDown(ByVal e As KeyEventArgs)
			If e.Key = Key.Escape Then
				Dim parentControl As DependencyObject = VTreeHelper.GetParentOfType(CType(e.OriginalSource, DependencyObject), Me.GetType())
				If parentControl IsNot Nothing Then
					If TypeOf _parentWindow Is Window Then
						CType(_parentWindow, Window).Close()
					Else
						CType(_parentWindow, C1Window).Close()
					End If
				End If
			End If
			MyBase.OnKeyDown(e)
		End Sub
		Protected Overrides Sub OnMouseWheel(ByVal e As System.Windows.Input.MouseWheelEventArgs)
			e.Handled = True
			MyBase.OnMouseWheel(e)
		End Sub
#End Region
	End Class
End Namespace
