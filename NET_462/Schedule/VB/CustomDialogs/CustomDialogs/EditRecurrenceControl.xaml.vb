Imports System.ComponentModel
Imports System.Globalization
Imports System.Threading

Imports C1.Schedule
Imports C1.WPF
Imports C1.WPF.DateTimeEditors
Imports C1.WPF.Localization
Imports C1.WPF.Schedule

Namespace CustomDialogs
	''' <summary>
	''' The <see cref="EditRecurrenceControl"/> control allows editing of all recurrence pattern properties.
	''' </summary>
	Partial Public Class EditRecurrenceControl
		Inherits UserControl
		#Region "dependency properties"
		''' <summary>
		''' Gets or sets reference to the parent <see cref="Scheduler"/> control.
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
		Public Shared ReadOnly SchedulerProperty As DependencyProperty = DependencyProperty.Register("Scheduler", GetType(C1Scheduler), GetType(EditRecurrenceControl), Nothing)
		#End Region

		#Region "** fields"
		Private _parentWindow As ContentControl = Nothing
		Private _pattern As RecurrencePattern
		Private _appointment As Appointment
		Private _isOld As Boolean = True

		Private _weekOfMonthValues As List(Of WeekOfMonthEnum)
		Private _weekDaysValues As List(Of WeekDaysEnum)
		Private _isLoaded As Boolean = False
		#End Region

		#Region "** initializing"
		''' <summary>
		''' Creates the new instance of the <see cref="EditRecurrenceControl"/> class.
		''' </summary>
		Public Sub New()
			_weekOfMonthValues = New List(Of WeekOfMonthEnum)()
			_weekOfMonthValues.Add(WeekOfMonthEnum.First)
			_weekOfMonthValues.Add(WeekOfMonthEnum.Second)
			_weekOfMonthValues.Add(WeekOfMonthEnum.Third)
			_weekOfMonthValues.Add(WeekOfMonthEnum.Fourth)
			_weekOfMonthValues.Add(WeekOfMonthEnum.Last)

			_weekDaysValues = New List(Of WeekDaysEnum)()
			_weekDaysValues.Add(WeekDaysEnum.Monday)
			_weekDaysValues.Add(WeekDaysEnum.Tuesday)
			_weekDaysValues.Add(WeekDaysEnum.Wednesday)
			_weekDaysValues.Add(WeekDaysEnum.Thursday)
			_weekDaysValues.Add(WeekDaysEnum.Friday)
			_weekDaysValues.Add(WeekDaysEnum.Saturday)
			_weekDaysValues.Add(WeekDaysEnum.Sunday)
			_weekDaysValues.Add(WeekDaysEnum.WeekendDays)
			_weekDaysValues.Add(WeekDaysEnum.WorkDays)
			_weekDaysValues.Add(WeekDaysEnum.EveryDay)
			InitializeComponent()
			MonthlyDetails21.ItemsSource = _weekOfMonthValues
			YearlyDetails21.ItemsSource = _weekOfMonthValues
			MonthlyDetails22.ItemsSource = _weekDaysValues
			YearlyDetails22.ItemsSource = _weekDaysValues
		End Sub

		Private Sub UserControl_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If Not _isLoaded Then
				_pattern = TryCast(DataContext, RecurrencePattern)

				If Not System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted Then
					_parentWindow = CType(VTreeHelper.GetParentOfType(Me, GetType(Window)), ContentControl)
				Else
					_parentWindow = CType(VTreeHelper.GetParentOfType(Me, GetType(C1Window)), ContentControl)
				End If
				If _parentWindow IsNot Nothing Then
					_appointment = TryCast(_parentWindow.Tag, Appointment)
					If _appointment IsNot Nothing AndAlso _appointment.ParentCollection IsNot Nothing Then
                        Scheduler = CType(_appointment.ParentCollection.ParentStorage.ScheduleStorage, C1.WPF.Schedule.C1ScheduleStorage).Scheduler
                    End If
					If _parentWindow.Resources.Contains("IsOld") Then
						_isOld = CBool(_parentWindow.Resources("IsOld"))
					End If
					PART_DialogCustomButton.IsEnabled = _isOld
					If TypeOf _parentWindow Is Window Then
						AddHandler(CType(_parentWindow, Window)).Closed, AddressOf _parentWindow_Closed
					Else
						AddHandler(CType(_parentWindow, C1Window)).Closed, AddressOf _parentWindow_Closed
					End If
				End If
				If _parentWindow IsNot Nothing Then
					AddHandler(CType(_pattern, INotifyPropertyChanged)).PropertyChanged, AddressOf EditRecurrenceControl_PropertyChanged
					InitializeEnums()
				End If
				startTime.Focus()
				endTime.DateTime = _pattern.EndTime
				duration.Value = _pattern.Duration
				patternEndDate.DateTime = _pattern.PatternEndDate

				UpdateRecurrenceType()
				UpdateDayOfWeekMaskControls()
				UpdateRangeControls()
				_isLoaded = True
			End If
		End Sub

		Private Sub _parentWindow_Closed(ByVal sender As Object, ByVal e As EventArgs)
			RemoveHandler(CType(_pattern, INotifyPropertyChanged)).PropertyChanged, AddressOf EditRecurrenceControl_PropertyChanged
			If TypeOf _parentWindow Is Window Then
				RemoveHandler(CType(_parentWindow, Window)).Closed, AddressOf _parentWindow_Closed
			Else
				RemoveHandler(CType(_parentWindow, C1Window)).Closed, AddressOf _parentWindow_Closed
			End If
		End Sub

		Private Sub InitializeEnums()
			Dim bnd As New Binding("FullMonthNames")
			bnd.Source = Scheduler.CalendarHelper
			YearlyDetails12.SetBinding(ItemsControl.ItemsSourceProperty, bnd)
			YearlyDetails23.SetBinding(ItemsControl.ItemsSourceProperty, bnd)
			bnd = New Binding("MonthOfYear")
			bnd.Source = _pattern
			bnd.Mode = BindingMode.TwoWay
			bnd.Converter = DecrementConverter.Default
			YearlyDetails12.SetBinding(ComboBox.SelectedIndexProperty, bnd)
			YearlyDetails23.SetBinding(ComboBox.SelectedIndexProperty, bnd)
		End Sub
		#End Region

		#Region "** private stuff"
		Private Sub PART_DialogCustomButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			RemoveHandler(CType(_pattern, INotifyPropertyChanged)).PropertyChanged, AddressOf EditRecurrenceControl_PropertyChanged
			_isLoaded = False
			_pattern.ParentAppointment.ClearRecurrencePattern()
			If _parentWindow IsNot Nothing Then
				If TypeOf _parentWindow Is Window Then
					CType(_parentWindow, Window).DialogResult = True
				Else
					CType(_parentWindow, C1Window).DialogResult = MessageBoxResult.OK
				End If
			End If
		End Sub

		Private Sub PART_DialogCancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			RemoveHandler(CType(_pattern, INotifyPropertyChanged)).PropertyChanged, AddressOf EditRecurrenceControl_PropertyChanged
			_isLoaded = False
			If _parentWindow IsNot Nothing Then
				If TypeOf _parentWindow Is Window Then
					CType(_parentWindow, Window).DialogResult = False
				Else
					CType(_parentWindow, C1Window).DialogResult = MessageBoxResult.Cancel
				End If
			End If
		End Sub

		Private Sub PART_DialogOkButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			RemoveHandler(CType(_pattern, INotifyPropertyChanged)).PropertyChanged, AddressOf EditRecurrenceControl_PropertyChanged
			_isLoaded = False
			' validate DayOfWeekMask
			Select Case _pattern.RecurrenceType
				Case RecurrenceTypeEnum.Workdays
					_pattern.DayOfWeekMask = WeekDaysEnum.WorkDays
				Case RecurrenceTypeEnum.YearlyNth
					_pattern.DayOfWeekMask = CType(YearlyDetails22.SelectedItem, WeekDaysEnum)
				Case RecurrenceTypeEnum.MonthlyNth
					_pattern.DayOfWeekMask = CType(Me.MonthlyDetails22.SelectedItem, WeekDaysEnum)
			End Select
			If _parentWindow IsNot Nothing Then
				If TypeOf _parentWindow Is Window Then
					CType(_parentWindow, Window).DialogResult = True
				Else
					CType(_parentWindow, C1Window).DialogResult = MessageBoxResult.OK
				End If
			End If
		End Sub
		#End Region

		#Region "** recurrence type"
		Private Sub CheckBox_changed(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				SetDayOfWeekMask()
			End If
		End Sub
		Private Sub UpdateDayOfWeekMaskControls()
			Dim mask As WeekDaysEnum = _pattern.DayOfWeekMask
			MondayCB.IsChecked = (mask And WeekDaysEnum.Monday) = WeekDaysEnum.Monday
			TuesdayCB.IsChecked = (mask And WeekDaysEnum.Tuesday) = WeekDaysEnum.Tuesday
			WednesdayCB.IsChecked = (mask And WeekDaysEnum.Wednesday) = WeekDaysEnum.Wednesday
			ThursdayCB.IsChecked = (mask And WeekDaysEnum.Thursday) = WeekDaysEnum.Thursday
			FridayCB.IsChecked = (mask And WeekDaysEnum.Friday) = WeekDaysEnum.Friday
			SaturdayCB.IsChecked = (mask And WeekDaysEnum.Saturday) = WeekDaysEnum.Saturday
			SundayCB.IsChecked = (mask And WeekDaysEnum.Sunday) = WeekDaysEnum.Sunday
		End Sub

		Private Sub SetDayOfWeekMask()
			Dim values() As Object = { MondayCB.IsChecked.Value, TuesdayCB.IsChecked.Value, WednesdayCB.IsChecked.Value, ThursdayCB.IsChecked.Value, FridayCB.IsChecked.Value, SaturdayCB.IsChecked.Value, SundayCB.IsChecked.Value }
			If _pattern.DayOfWeekMask = WeekDaysEnum.None OrElse _pattern.RecurrenceType = RecurrenceTypeEnum.Weekly Then
				_pattern.DayOfWeekMask = CType(Convert(values, GetType(WeekDaysEnum), "Monday;Tuesday;Wednesday;Thursday;Friday;Saturday;Sunday"), WeekDaysEnum)
			End If
			Validate()
		End Sub

		Private Shared Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object) As Object
			Dim paramArr() As System.Enum = ConvertParam(targetType, parameter)
			Dim ret As Long = 0
			For i As Integer = 0 To paramArr.Length - 1
				Dim isChecked As Boolean = TypeOf values(i) Is Boolean AndAlso CBool(values(i))
				If isChecked Then
					Dim paramInt As Long = CLng(Fix(Convert.ChangeType(paramArr(i), GetType(Long), CultureInfo.InvariantCulture)))
					ret = ret Or paramInt
				End If
			Next i
			Return System.Enum.ToObject(targetType, ret)
		End Function

		Friend Shared Function ConvertParam(ByVal enumType As Type, ByVal parameter As Object) As [Enum]()
			Dim ret() As System.Enum = New [Enum](){}
			Dim strParam As String = TryCast(parameter, String)
			If String.IsNullOrEmpty(strParam) Then
				Return ret
			End If
			Dim strParamArr() As String = strParam.Split(";"c, ","c)
			If strParamArr.Length = 0 Then
				Return ret
			End If
			ret = New [Enum](strParamArr.Length - 1){}
			For i As Integer = 0 To strParamArr.Length - 1
				ret(i) = CType(System.Enum.Parse(enumType, strParamArr(i), False), [Enum])
			Next i
			Return ret
		End Function

		Private Sub YearlyDetails12_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded AndAlso Me.YearlyDetails.Visibility = Visibility.Visible Then
				YearlyRB.IsChecked = True
			End If
		End Sub
		Private Sub YearlyDetails21_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded AndAlso Me.YearlyDetails.Visibility = Visibility.Visible Then
				YearlyNthRB.IsChecked = True
			End If
		End Sub
		Private Sub MonthlyDetails21_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded AndAlso Me.MonthlyDetails.Visibility = Visibility.Visible Then
				MonthlyNthRB.IsChecked = True
			End If
		End Sub
		Private Sub MonthlyDetails11_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded AndAlso Me.MonthlyDetails.Visibility = Visibility.Visible Then
				MonthlyRB.IsChecked = True
			End If
		End Sub
		Private Sub DailyDetails11_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)

			If _isLoaded AndAlso DailyDetails.Visibility = Visibility.Visible Then
				DailyRB.IsChecked = True
			End If
		End Sub

		Private Sub UpdateRecurrenceType()
			Select Case _pattern.RecurrenceType
				Case RecurrenceTypeEnum.Daily
					If Not DailyGroupRB.IsChecked.Value Then
						DailyGroupRB.IsChecked = True
					End If
					DailyRB.IsChecked = True
				Case RecurrenceTypeEnum.Workdays
					If Not DailyGroupRB.IsChecked.Value Then
						DailyGroupRB.IsChecked = True
					End If
					WorkdaysRB.IsChecked = True
				Case RecurrenceTypeEnum.Weekly
					WeeklyRB.IsChecked = True
				Case RecurrenceTypeEnum.Monthly
					If Not MonthlyGroupRB.IsChecked.Value Then
						MonthlyGroupRB.IsChecked = True
					End If
					MonthlyRB.IsChecked = True
				Case RecurrenceTypeEnum.MonthlyNth
					If Not MonthlyGroupRB.IsChecked.Value Then
						MonthlyGroupRB.IsChecked = True
					End If
					MonthlyNthRB.IsChecked = True
				Case RecurrenceTypeEnum.Yearly
					If Not YearlyGroupRB.IsChecked.Value Then
						YearlyGroupRB.IsChecked = True
					End If
					YearlyRB.IsChecked = True
				Case RecurrenceTypeEnum.YearlyNth
					If Not YearlyGroupRB.IsChecked.Value Then
						YearlyGroupRB.IsChecked = True
					End If
					YearlyNthRB.IsChecked = True
			End Select
			Validate()
		End Sub
		Private Sub EditRecurrenceControl_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
			If _isLoaded Then
				Select Case e.PropertyName
					Case "RecurrenceType"
						UpdateRecurrenceType()
					Case "StartTime"
						endTime.DateTime = _pattern.EndTime
					Case "EndTime"
						duration.Value = _pattern.Duration
					Case "Duration"
						endTime.DateTime = _pattern.EndTime
					Case "PatternStartDate"
						patternEndDate.DateTime = _pattern.PatternEndDate
				End Select
			End If

		End Sub
		Private Sub DailyRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.RecurrenceType = RecurrenceTypeEnum.Daily
			End If
		End Sub
		Private Sub WorkdaysRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.RecurrenceType = RecurrenceTypeEnum.Workdays
			End If
		End Sub
		Private Sub MonthlyRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.RecurrenceType = RecurrenceTypeEnum.Monthly
			End If
		End Sub
		Private Sub MonthlyNthRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.RecurrenceType = RecurrenceTypeEnum.MonthlyNth
			End If
		End Sub
		Private Sub YearlyRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.RecurrenceType = RecurrenceTypeEnum.Yearly
			End If
		End Sub
		Private Sub YearlyNthRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.RecurrenceType = RecurrenceTypeEnum.YearlyNth
			End If
		End Sub

		Private Function IsValidDayMonth() As Boolean
			Dim month As Integer = YearlyDetails12.SelectedIndex + 1
			Dim dayOfMonth As Integer = CInt(Fix(YearlyDetails11.Value))
			If dayOfMonth > 30 AndAlso (month = 4 OrElse month = 6 OrElse month = 9 OrElse month = 11) Then
				Return False
			End If
			If dayOfMonth > 29 AndAlso month = 2 Then
				Return False
			End If
			Return True
		End Function
		Private Sub ValidateYearly()
			If IsValidDayMonth() Then
				PART_DialogOkButton.IsEnabled = True
				YearlyDetails12.ClearValue(Control.ForegroundProperty)
				YearlyDetails12.ClearValue(Control.BorderBrushProperty)
				YearlyDetails12.ClearValue(Control.BorderThicknessProperty)
				YearlyDetails12.ClearValue(ToolTipService.ToolTipProperty)
				YearlyDetails11.ClearValue(Control.ForegroundProperty)
				YearlyDetails11.ClearValue(Control.BorderBrushProperty)
				YearlyDetails11.ClearValue(Control.BorderThicknessProperty)
				YearlyDetails11.ClearValue(ToolTipService.ToolTipProperty)
			Else
				PART_DialogOkButton.IsEnabled = False
				YearlyDetails12.Foreground = New SolidColorBrush(Colors.Red)
				YearlyDetails11.Foreground = YearlyDetails12.Foreground
				YearlyDetails12.BorderBrush = YearlyDetails11.Foreground
				YearlyDetails11.BorderBrush = YearlyDetails12.BorderBrush
				YearlyDetails12.BorderThickness = New Thickness(2)
				YearlyDetails11.BorderThickness = YearlyDetails12.BorderThickness
				Dim max As Integer = 31
				Dim month As Integer = YearlyDetails12.SelectedIndex + 1
				If month = 4 OrElse month = 6 OrElse month = 9 OrElse month = 11 Then
					max = 30
				End If
				If month = 2 Then
					max = 29
				End If
				Dim [error] As String = String.Format(C1Localizer.GetString("ValidationErrors", "NumberIsOutOfRange", "Please enter value in the range: {0}."), CStr("1 - " & max))
				ToolTipService.SetToolTip(YearlyDetails12, [error])
				ToolTipService.SetToolTip(YearlyDetails11, [error])
			End If
		End Sub
		Private Sub YearlyDetails12_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			If _isLoaded Then
				ValidateYearly()
			End If
		End Sub
		Private Sub YearlyDetails11_ValueChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs(Of Double))
			If _isLoaded Then
				ValidateYearly()
			End If
		End Sub
		#End Region

		#Region "** range"
		Private Sub UpdateRangeControls()
			If _pattern Is Nothing Then
				Return
			End If
			If _pattern.NoEndDate Then
				noEndDate.IsChecked = True
			ElseIf _pattern.Occurrences > 0 Then
				endAfter.IsChecked = True
			Else
				endBy.IsChecked = True
			End If
		End Sub
		Private Sub noEndDate_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.NoEndDate = True
				Validate()
			End If
		End Sub
		Private Sub endAfter_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.NoEndDate = False
				If _pattern.Occurrences <= 0 Then
					_pattern.Occurrences = 10
				End If
				Validate()
			End If
		End Sub
		Private Sub endBy_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.NoEndDate = False
				_pattern.Occurrences = 0
				Validate()
			End If
		End Sub
		Private Sub occurences_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				endAfter.IsChecked = True
			End If
		End Sub
		Private Sub patternEndDate_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				endBy.IsChecked = True
			End If
		End Sub
		Private Sub occurences_ValueChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs(Of Double))
			If _isLoaded Then
				Validate()
				If CInt(Fix(occurences.Value)) = 0 Then
					occurences.Foreground = New SolidColorBrush(Colors.Red)
					occurences.BorderBrush = occurences.Foreground
					occurences.BorderThickness = New Thickness(2)
					Dim [error] As String = String.Format(C1Localizer.GetString("ValidationErrors", "NumberIsOutOfRange", "Please enter value in the range: {0}."), CStr("1 - 999"))
					ToolTipService.SetToolTip(occurences, [error])
				End If
			End If
		End Sub
		#End Region

		#Region "** when"
		Private Sub Validate()
			If _isLoaded Then
				Dim isValid As Boolean = True

				If _pattern.RecurrenceType = RecurrenceTypeEnum.Yearly AndAlso (Not IsValidDayMonth()) Then
					isValid = False
				ElseIf endAfter.IsChecked.Value AndAlso occurences.Value = 0 Then
					isValid = False
				ElseIf endTime.DateTime.Value < _pattern.StartTime Then
					isValid = False
				ElseIf endBy.IsChecked.Value AndAlso _pattern.PatternStartDate > patternEndDate.DateTime.Value Then
					isValid = False
				End If
				If _pattern.RecurrenceType = RecurrenceTypeEnum.Weekly AndAlso _pattern.DayOfWeekMask = WeekDaysEnum.None Then
					isValid = False
				End If

				If (Not endAfter.IsChecked.Value) OrElse occurences.Value > 0 Then
					occurences.ClearValue(Control.ForegroundProperty)
					occurences.ClearValue(Control.BorderBrushProperty)
					occurences.ClearValue(Control.BorderThicknessProperty)
					occurences.ClearValue(ToolTipService.ToolTipProperty)
				End If

				If isValid Then
					PART_DialogOkButton.IsEnabled = True
				Else
					PART_DialogOkButton.IsEnabled = False
				End If
			End If
		End Sub

		Private Sub endTime_DateTimeChanged(ByVal sender As Object, ByVal e As NullablePropertyChangedEventArgs(Of Date))
			If _isLoaded Then
				Try
					_pattern.EndTime = endTime.DateTime.Value
					If Not PART_DialogOkButton.IsEnabled Then
						endTime.ClearValue(Control.ForegroundProperty)
						endTime.ClearValue(Control.BorderBrushProperty)
						endTime.ClearValue(Control.BorderThicknessProperty)
						endTime.ClearValue(ToolTipService.ToolTipProperty)
						Validate()
					End If
				Catch ex As Exception
					endTime.Foreground = New SolidColorBrush(Colors.Red)
					endTime.BorderBrush = endTime.Foreground
					endTime.BorderThickness = New Thickness(2)
					ToolTipService.SetToolTip(endTime, ex.Message)
					PART_DialogOkButton.IsEnabled = False
				End Try
			End If
		End Sub

		Private Sub duration_ValueChanged(ByVal sender As Object, ByVal e As NullablePropertyChangedEventArgs(Of TimeSpan))
			If _isLoaded Then
				Try
					_pattern.Duration = duration.Value.Value
					If Not PART_DialogOkButton.IsEnabled Then
						duration.ClearValue(Control.ForegroundProperty)
						duration.ClearValue(Control.BorderBrushProperty)
						duration.ClearValue(Control.BorderThicknessProperty)
						duration.ClearValue(ToolTipService.ToolTipProperty)
						Validate()
					End If
				Catch ex As Exception
					duration.Foreground = New SolidColorBrush(Colors.Red)
					duration.BorderBrush = duration.Foreground
					duration.BorderThickness = New Thickness(2)
					ToolTipService.SetToolTip(duration, ex.Message)
					PART_DialogOkButton.IsEnabled = False
				End Try
			End If
		End Sub

		Private Sub patternEndDate_DateTimeChanged(ByVal sender As Object, ByVal e As NullablePropertyChangedEventArgs(Of Date))
			If _isLoaded Then
				Try
					_pattern.PatternEndDate = patternEndDate.DateTime.Value
					If Not PART_DialogOkButton.IsEnabled Then
						patternEndDate.ClearValue(Control.ForegroundProperty)
						patternEndDate.ClearValue(Control.BorderBrushProperty)
						patternEndDate.ClearValue(Control.BorderThicknessProperty)
						patternEndDate.ClearValue(ToolTipService.ToolTipProperty)
						Validate()
					End If
				Catch ex As Exception
					patternEndDate.Foreground = New SolidColorBrush(Colors.Red)
					patternEndDate.BorderBrush = patternEndDate.Foreground
					patternEndDate.BorderThickness = New Thickness(2)
					ToolTipService.SetToolTip(patternEndDate, ex.Message)
					PART_DialogOkButton.IsEnabled = False
				End Try
			End If
		End Sub
		#End Region

		Private Sub WeeklyRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _isLoaded Then
				_pattern.RecurrenceType = RecurrenceTypeEnum.Weekly
			End If
			WeeklyDetails.Visibility = Visibility.Visible
			MonthlyDetails.Visibility = Visibility.Collapsed
			YearlyDetails.Visibility = Visibility.Collapsed
			DailyDetails.Visibility = Visibility.Collapsed
		End Sub

		Private Sub MonthlyGroupRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			MonthlyDetails.Visibility = Visibility.Visible
			WeeklyDetails.Visibility = Visibility.Collapsed
			YearlyDetails.Visibility = Visibility.Collapsed
			DailyDetails.Visibility = Visibility.Collapsed
		End Sub

		Private Sub YearlyGroupRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			YearlyDetails.Visibility = Visibility.Visible
			WeeklyDetails.Visibility = Visibility.Collapsed
			MonthlyDetails.Visibility = Visibility.Collapsed
			DailyDetails.Visibility = Visibility.Collapsed
		End Sub

		Private Sub DailyGroupRB_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			DailyDetails.Visibility = Visibility.Visible
			WeeklyDetails.Visibility = Visibility.Collapsed
			MonthlyDetails.Visibility = Visibility.Collapsed
			YearlyDetails.Visibility = Visibility.Collapsed
		End Sub
		Protected Overrides Sub OnPreviewKeyDown(ByVal e As KeyEventArgs)
			If e.Key = Key.Escape Then
				Dim parentControl As DependencyObject = VTreeHelper.GetParentOfType(CType(e.OriginalSource, DependencyObject), Me.GetType())
				If parentControl IsNot Nothing Then
					PART_DialogCancelButton_Click(Me, Nothing)
				End If
			End If
			MyBase.OnPreviewKeyDown(e)
		End Sub
        Protected Overrides Sub OnMouseWheel(ByVal e As System.Windows.Input.MouseWheelEventArgs)
            e.Handled = True
            MyBase.OnMouseWheel(e)
        End Sub
	End Class
End Namespace
