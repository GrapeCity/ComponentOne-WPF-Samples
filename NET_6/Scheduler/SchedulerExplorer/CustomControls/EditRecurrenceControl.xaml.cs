using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using C1.C1Schedule;
using C1.WPF;
using C1.WPF.DateTimeEditors;
using C1.WPF.Schedule;
using System.Windows.Input;
using C1.WPF.Core;
using C1.WPF.Docking;
using C1.WPF.Localization;

namespace SchedulerExplorer
{
    /// <summary>
    /// The <see cref="EditRecurrenceControl"/> control allows editing of all recurrence pattern properties.
    /// </summary>
    public partial class EditRecurrenceControl : UserControl
    {
        #region dependency properties
        /// <summary>
        /// Gets or sets reference to the parent <see cref="Scheduler"/> control.
        /// </summary>
        public C1Scheduler Scheduler
        {
            get { return (C1Scheduler)GetValue(SchedulerProperty); }
            set { SetValue(SchedulerProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Scheduler"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SchedulerProperty =
            DependencyProperty.Register("Scheduler", typeof(C1Scheduler), typeof(EditRecurrenceControl), null);
        #endregion

        #region ** fields
        private ContentControl _parentWindow = null;
        private RecurrencePattern _pattern;
        private Appointment _appointment;
        private bool _isOld = true;

        private List<WeekOfMonthEnum> _weekOfMonthValues;
        private List<WeekDaysEnum> _weekDaysValues;
        private bool _isLoaded = false;
        #endregion

        #region ** initializing
        /// <summary>
        /// Creates the new instance of the <see cref="EditRecurrenceControl"/> class.
        /// </summary>
        public EditRecurrenceControl()
        {
            _weekOfMonthValues = new List<WeekOfMonthEnum>();
            _weekOfMonthValues.Add(WeekOfMonthEnum.First);
            _weekOfMonthValues.Add(WeekOfMonthEnum.Second);
            _weekOfMonthValues.Add(WeekOfMonthEnum.Third);
            _weekOfMonthValues.Add(WeekOfMonthEnum.Fourth);
            _weekOfMonthValues.Add(WeekOfMonthEnum.Last);

            _weekDaysValues = new List<WeekDaysEnum>();
            _weekDaysValues.Add(WeekDaysEnum.Monday);
            _weekDaysValues.Add(WeekDaysEnum.Tuesday);
            _weekDaysValues.Add(WeekDaysEnum.Wednesday);
            _weekDaysValues.Add(WeekDaysEnum.Thursday);
            _weekDaysValues.Add(WeekDaysEnum.Friday);
            _weekDaysValues.Add(WeekDaysEnum.Saturday);
            _weekDaysValues.Add(WeekDaysEnum.Sunday);
            _weekDaysValues.Add(WeekDaysEnum.WeekendDays);
            _weekDaysValues.Add(WeekDaysEnum.WorkDays);
            _weekDaysValues.Add(WeekDaysEnum.EveryDay);
            InitializeComponent();
            MonthlyDetails21.ItemsSource = _weekOfMonthValues;
            YearlyDetails21.ItemsSource = _weekOfMonthValues;
            MonthlyDetails22.ItemsSource = _weekDaysValues;
            YearlyDetails22.ItemsSource = _weekDaysValues;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isLoaded)
            {
                _pattern = DataContext as RecurrencePattern;

                if (!System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted)
                {
                    _parentWindow = (ContentControl)VTreeHelper.GetParentOfType(this, typeof(Window));
                }
                else
                    _parentWindow = (ContentControl)VTreeHelper.GetParentOfType(this, typeof(C1Window));
                if (_parentWindow != null)
                {
                    _appointment = _parentWindow.Tag as Appointment;
                    if (_appointment != null && _appointment.ParentCollection != null)
                    {
                        Scheduler = ((C1ScheduleStorage)_appointment.ParentCollection.ParentStorage.ScheduleStorage).Scheduler;
                    }
                    if (_parentWindow.Resources.Contains("IsOld"))
                    {
                        _isOld = (bool)_parentWindow.Resources["IsOld"];
                    }
                    PART_DialogCustomButton.IsEnabled = _isOld;
                    if (_parentWindow is Window)
                    {
                        ((Window)_parentWindow).Closed += new EventHandler(_parentWindow_Closed);
                    }
                    else
                        ((C1Window)_parentWindow).Closed += new EventHandler(_parentWindow_Closed);
                }
                if (_parentWindow != null)
                {
                    ((INotifyPropertyChanged)_pattern).PropertyChanged += new PropertyChangedEventHandler(EditRecurrenceControl_PropertyChanged);
                    InitializeEnums();
                }
                startTime.Focus();
                endTime.DateTime = _pattern.EndTime;
                duration.Value = _pattern.Duration;
                patternEndDate.DateTime = _pattern.PatternEndDate;

                UpdateRecurrenceType();
                UpdateDayOfWeekMaskControls();
                UpdateRangeControls();
                _isLoaded = true;
            }
        }

        void _parentWindow_Closed(object sender, EventArgs e)
        {
            ((INotifyPropertyChanged)_pattern).PropertyChanged -= new PropertyChangedEventHandler(EditRecurrenceControl_PropertyChanged);
            if (_parentWindow is Window)
            {
                ((Window)_parentWindow).Closed -= new EventHandler(_parentWindow_Closed);
            }
            else
                ((C1Window)_parentWindow).Closed -= new EventHandler(_parentWindow_Closed);
        }

        private void InitializeEnums()
        {
            Binding bnd = new Binding("FullMonthNames");
            bnd.Source = Scheduler.CalendarHelper;
            YearlyDetails12.SetBinding(ItemsControl.ItemsSourceProperty, bnd);
            YearlyDetails23.SetBinding(ItemsControl.ItemsSourceProperty, bnd);
            bnd = new Binding("MonthOfYear");
            bnd.Source = _pattern;
            bnd.Mode = BindingMode.TwoWay;
            bnd.Converter = DecrementConverter.Default;
            YearlyDetails12.SetBinding(ComboBox.SelectedIndexProperty, bnd);
            YearlyDetails23.SetBinding(ComboBox.SelectedIndexProperty, bnd);
        }
        #endregion

        #region ** private stuff
        private void PART_DialogCustomButton_Click(object sender, RoutedEventArgs e)
        {
            ((INotifyPropertyChanged)_pattern).PropertyChanged -= new PropertyChangedEventHandler(EditRecurrenceControl_PropertyChanged);
            _isLoaded = false;
            _pattern.ParentAppointment.ClearRecurrencePattern();
            if (_parentWindow != null)
            {
                if (_parentWindow is Window)
                {
                    ((Window)_parentWindow).DialogResult = true;
                }
                else
                    ((C1Window)_parentWindow).DialogResult = MessageBoxResult.OK;
            }
        }

        private void PART_DialogCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ((INotifyPropertyChanged)_pattern).PropertyChanged -= new PropertyChangedEventHandler(EditRecurrenceControl_PropertyChanged);
            _isLoaded = false;
            if (_parentWindow != null)
            {
                if (_parentWindow is Window)
                {
                    ((Window)_parentWindow).DialogResult = false;
                }
                else
                    ((C1Window)_parentWindow).DialogResult = MessageBoxResult.Cancel;
            }
        }

        private void PART_DialogOkButton_Click(object sender, RoutedEventArgs e)
        {
            ((INotifyPropertyChanged)_pattern).PropertyChanged -= new PropertyChangedEventHandler(EditRecurrenceControl_PropertyChanged);
            _isLoaded = false;
            // validate DayOfWeekMask
            switch (_pattern.RecurrenceType)
            {
                case RecurrenceTypeEnum.Workdays:
                    _pattern.DayOfWeekMask = WeekDaysEnum.WorkDays;
                    break;
                case RecurrenceTypeEnum.YearlyNth:
                    _pattern.DayOfWeekMask = (WeekDaysEnum)YearlyDetails22.SelectedItem;
                    break;
                case RecurrenceTypeEnum.MonthlyNth:
                    _pattern.DayOfWeekMask = (WeekDaysEnum)this.MonthlyDetails22.SelectedItem;
                    break;
            }
            if (_parentWindow != null)
            {
                if (_parentWindow is Window)
                {
                    ((Window)_parentWindow).DialogResult = true;
                }
                else
                    ((C1Window)_parentWindow).DialogResult = MessageBoxResult.OK;
            }
        }
        #endregion

        #region ** recurrence type
        private void CheckBox_changed(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                SetDayOfWeekMask();
            }
        }
        private void UpdateDayOfWeekMaskControls()
        {
            WeekDaysEnum mask = _pattern.DayOfWeekMask;
            MondayCB.IsChecked = (mask & WeekDaysEnum.Monday) == WeekDaysEnum.Monday;
            TuesdayCB.IsChecked = (mask & WeekDaysEnum.Tuesday) == WeekDaysEnum.Tuesday;
            WednesdayCB.IsChecked = (mask & WeekDaysEnum.Wednesday) == WeekDaysEnum.Wednesday;
            ThursdayCB.IsChecked = (mask & WeekDaysEnum.Thursday) == WeekDaysEnum.Thursday;
            FridayCB.IsChecked = (mask & WeekDaysEnum.Friday) == WeekDaysEnum.Friday;
            SaturdayCB.IsChecked = (mask & WeekDaysEnum.Saturday) == WeekDaysEnum.Saturday;
            SundayCB.IsChecked = (mask & WeekDaysEnum.Sunday) == WeekDaysEnum.Sunday;
        }

        private void SetDayOfWeekMask()
        {
            object[] values = new object[7]
				{
					MondayCB.IsChecked.Value, 
					TuesdayCB.IsChecked.Value,
					WednesdayCB.IsChecked.Value,
					ThursdayCB.IsChecked.Value,
					FridayCB.IsChecked.Value,
					SaturdayCB.IsChecked.Value,
					SundayCB.IsChecked.Value
				};
            if (_pattern.DayOfWeekMask == WeekDaysEnum.None || _pattern.RecurrenceType == RecurrenceTypeEnum.Weekly)
            {
                _pattern.DayOfWeekMask = (WeekDaysEnum)Convert(values, typeof(WeekDaysEnum),
                    "Monday;Tuesday;Wednesday;Thursday;Friday;Saturday;Sunday");
            }
            Validate();
        }

        private static object Convert(object[] values, Type targetType, object parameter)
        {
            Enum[] paramArr = ConvertParam(targetType, parameter);
            long ret = 0;
            for (int i = 0; i < paramArr.Length; i++)
            {
                bool isChecked = values[i] is bool && (bool)values[i];
                if (isChecked)
                {
                    long paramInt = (long)System.Convert.ChangeType(paramArr[i], typeof(long), CultureInfo.InvariantCulture);
                    ret |= paramInt;
                }
            }
            return Enum.ToObject(targetType, ret);
        }

        internal static Enum[] ConvertParam(Type enumType, object parameter)
        {
            Enum[] ret = Array.Empty<Enum>();
            string strParam = parameter as string;
            if (string.IsNullOrEmpty(strParam))
                return ret;
            string[] strParamArr = strParam.Split(';', ',');
            if (strParamArr.Length == 0)
                return ret;
            ret = new Enum[strParamArr.Length];
            for (int i = 0; i < strParamArr.Length; i++)
            {
                ret[i] = (Enum)Enum.Parse(enumType, strParamArr[i], false);
            }
            return ret;
        }

        private void YearlyDetails12_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_isLoaded && this.YearlyDetails.Visibility == Visibility.Visible)
            {
                YearlyRB.IsChecked = true;
            }
        }
        private void YearlyDetails21_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_isLoaded && this.YearlyDetails.Visibility == Visibility.Visible)
            {
                YearlyNthRB.IsChecked = true;
            }
        }
        private void MonthlyDetails21_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_isLoaded && this.MonthlyDetails.Visibility == Visibility.Visible)
            {
                MonthlyNthRB.IsChecked = true;
            }
        }
        private void MonthlyDetails11_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_isLoaded && this.MonthlyDetails.Visibility == Visibility.Visible)
            {
                MonthlyRB.IsChecked = true;
            }
        }
        private void DailyDetails11_GotFocus(object sender, RoutedEventArgs e)
        {

            if (_isLoaded && DailyDetails.Visibility == Visibility.Visible)
            {
                DailyRB.IsChecked = true;
            }
        }

        private void UpdateRecurrenceType()
        {
            switch (_pattern.RecurrenceType)
            {
                case RecurrenceTypeEnum.Daily:
                    if (!DailyGroupRB.IsChecked.Value)
                        DailyGroupRB.IsChecked = true;
                    DailyRB.IsChecked = true;
                    break;
                case RecurrenceTypeEnum.Workdays:
                    if (!DailyGroupRB.IsChecked.Value)
                        DailyGroupRB.IsChecked = true;
                    WorkdaysRB.IsChecked = true;
                    break;
                case RecurrenceTypeEnum.Weekly:
                    WeeklyRB.IsChecked = true;
                    break;
                case RecurrenceTypeEnum.Monthly:
                    if (!MonthlyGroupRB.IsChecked.Value)
                        MonthlyGroupRB.IsChecked = true;
                    MonthlyRB.IsChecked = true;
                    break;
                case RecurrenceTypeEnum.MonthlyNth:
                    if (!MonthlyGroupRB.IsChecked.Value)
                        MonthlyGroupRB.IsChecked = true;
                    MonthlyNthRB.IsChecked = true;
                    break;
                case RecurrenceTypeEnum.Yearly:
                    if (!YearlyGroupRB.IsChecked.Value)
                        YearlyGroupRB.IsChecked = true;
                    YearlyRB.IsChecked = true;
                    break;
                case RecurrenceTypeEnum.YearlyNth:
                    if (!YearlyGroupRB.IsChecked.Value)
                        YearlyGroupRB.IsChecked = true;
                    YearlyNthRB.IsChecked = true;
                    break;
            }
            Validate();
        }
        void EditRecurrenceControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_isLoaded)
            {
                switch (e.PropertyName)
                {
                    case "RecurrenceType":
                        UpdateRecurrenceType();
                        break;
                    case "StartTime":
                        endTime.DateTime = _pattern.EndTime;
                        break;
                    case "EndTime":
                        duration.Value = _pattern.Duration;
                        break;
                    case "Duration":
                        endTime.DateTime = _pattern.EndTime;
                        break;
                    case "PatternStartDate":
                        patternEndDate.DateTime = _pattern.PatternEndDate;
                        break;
                }
            }

        }
        private void DailyRB_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.RecurrenceType = RecurrenceTypeEnum.Daily;
            }
        }
        private void WorkdaysRB_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.RecurrenceType = RecurrenceTypeEnum.Workdays;
            }
        }
        private void MonthlyRB_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.RecurrenceType = RecurrenceTypeEnum.Monthly;
            }
        }
        private void MonthlyNthRB_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.RecurrenceType = RecurrenceTypeEnum.MonthlyNth;
            }
        }
        private void YearlyRB_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.RecurrenceType = RecurrenceTypeEnum.Yearly;
            }
        }
        private void YearlyNthRB_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.RecurrenceType = RecurrenceTypeEnum.YearlyNth;
            }
        }

        private bool IsValidDayMonth()
        {
            int month = YearlyDetails12.SelectedIndex + 1;
            int dayOfMonth = (int)YearlyDetails11.Value;
            if (dayOfMonth > 30 &&
                (month == 4 || month == 6 || month == 9 || month == 11))
            {
                return false;
            }
            if (dayOfMonth > 29 && month == 2)
            {
                return false;
            }
            return true;
        }
        private void ValidateYearly()
        {
            if (IsValidDayMonth())
            {
                PART_DialogOkButton.IsEnabled = true;
                YearlyDetails12.ClearValue(Control.ForegroundProperty);
                YearlyDetails12.ClearValue(Control.BorderBrushProperty);
                YearlyDetails12.ClearValue(Control.BorderThicknessProperty);
                YearlyDetails12.ClearValue(ToolTipService.ToolTipProperty);
                YearlyDetails11.ClearValue(Control.ForegroundProperty);
                YearlyDetails11.ClearValue(Control.BorderBrushProperty);
                YearlyDetails11.ClearValue(Control.BorderThicknessProperty);
                YearlyDetails11.ClearValue(ToolTipService.ToolTipProperty);
            }
            else
            {
                PART_DialogOkButton.IsEnabled = false;
                YearlyDetails11.BorderBrush = YearlyDetails12.BorderBrush =
                YearlyDetails11.Foreground = YearlyDetails12.Foreground = new SolidColorBrush(Colors.Red);
                YearlyDetails11.BorderThickness = YearlyDetails12.BorderThickness = new Thickness(2);
                int max = 31;
                int month = YearlyDetails12.SelectedIndex + 1;
                if (month == 4 || month == 6 || month == 9 || month == 11)
                {
                    max = 30;
                }
                if (month == 2)
                {
                    max = 29;
                }
                string error = String.Format(C1Localizer.GetString("ValidationErrors",
                    "NumberIsOutOfRange", "Please enter value in the range: {0}."), (string)("1 - " + max));
                ToolTipService.SetToolTip(YearlyDetails12, error);
                ToolTipService.SetToolTip(YearlyDetails11, error);
            }
        }
        private void YearlyDetails12_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoaded)
            {
                ValidateYearly();
            }
        }
        private void YearlyDetails11_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (_isLoaded)
            {
                ValidateYearly();
            }
        }
        #endregion

        #region ** range
        private void UpdateRangeControls()
        {
            if (_pattern == null)
            {
                return;
            }
            if (_pattern.NoEndDate)
            {
                noEndDate.IsChecked = true;
            }
            else if (_pattern.Occurrences > 0)
            {
                endAfter.IsChecked = true;
            }
            else
            {
                endBy.IsChecked = true;
            }
        }
        private void noEndDate_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.NoEndDate = true;
                Validate();
            }
        }
        private void endAfter_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.NoEndDate = false;
                if (_pattern.Occurrences <= 0)
                {
                    _pattern.Occurrences = 10;
                }
                Validate();
            }
        }
        private void endBy_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.NoEndDate = false;
                _pattern.Occurrences = 0;
                Validate();
            }
        }
        private void occurences_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                endAfter.IsChecked = true;
            }
        }
        private void patternEndDate_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                endBy.IsChecked = true;
            }
        }
        private void occurences_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (_isLoaded)
            {
                Validate();
                if ((int)occurences.Value == 0)
                {
                    occurences.BorderBrush = occurences.Foreground = new SolidColorBrush(Colors.Red);
                    occurences.BorderThickness = new Thickness(2);
                    string error = String.Format(C1Localizer.GetString("ValidationErrors",
                        "NumberIsOutOfRange", "Please enter value in the range: {0}."), (string)("1 - 999"));
                    ToolTipService.SetToolTip(occurences, error);
                }
            }
        }
        #endregion

        #region ** when
        private void Validate()
        {
            if (_isLoaded)
            {
                bool isValid = true;

                if (_pattern.RecurrenceType == RecurrenceTypeEnum.Yearly && !IsValidDayMonth())
                {
                    isValid = false;
                }
                else if (endAfter.IsChecked.Value && occurences.Value == 0)
                {
                    isValid = false;
                }
                else if (endTime.DateTime.Value < _pattern.StartTime)
                {
                    isValid = false;
                }
                else if (endBy.IsChecked.Value && _pattern.PatternStartDate > patternEndDate.DateTime.Value)
                {
                    isValid = false;
                }
                if (_pattern.RecurrenceType == RecurrenceTypeEnum.Weekly && _pattern.DayOfWeekMask == WeekDaysEnum.None)
                {
                    isValid = false;
                }

                if (!endAfter.IsChecked.Value || occurences.Value > 0)
                {
                    occurences.ClearValue(Control.ForegroundProperty);
                    occurences.ClearValue(Control.BorderBrushProperty);
                    occurences.ClearValue(Control.BorderThicknessProperty);
                    occurences.ClearValue(ToolTipService.ToolTipProperty);
                }

                if (isValid)
                {
                    PART_DialogOkButton.IsEnabled = true;
                }
                else
                {
                    PART_DialogOkButton.IsEnabled = false;
                }
            }
        }

        private void endTime_DateTimeChanged(object sender, NullablePropertyChangedEventArgs<DateTime> e)
        {
            if (_isLoaded)
            {
                try
                {
                    _pattern.EndTime = endTime.DateTime.Value;
                    if (!PART_DialogOkButton.IsEnabled)
                    {
                        endTime.ClearValue(Control.ForegroundProperty);
                        endTime.ClearValue(Control.BorderBrushProperty);
                        endTime.ClearValue(Control.BorderThicknessProperty);
                        endTime.ClearValue(ToolTipService.ToolTipProperty);
                        Validate();
                    }
                }
                catch (Exception ex)
                {
                    endTime.BorderBrush = endTime.Foreground = new SolidColorBrush(Colors.Red);
                    endTime.BorderThickness = new Thickness(2);
                    ToolTipService.SetToolTip(endTime, ex.Message);
                    PART_DialogOkButton.IsEnabled = false;
                }
            }
        }

        private void duration_ValueChanged(object sender, NullablePropertyChangedEventArgs<TimeSpan> e)
        {
            if (_isLoaded)
            {
                try
                {
                    _pattern.Duration = duration.Value.Value;
                    if (!PART_DialogOkButton.IsEnabled)
                    {
                        duration.ClearValue(Control.ForegroundProperty);
                        duration.ClearValue(Control.BorderBrushProperty);
                        duration.ClearValue(Control.BorderThicknessProperty);
                        duration.ClearValue(ToolTipService.ToolTipProperty);
                        Validate();
                    }
                }
                catch (Exception ex)
                {
                    duration.BorderBrush = duration.Foreground = new SolidColorBrush(Colors.Red);
                    duration.BorderThickness = new Thickness(2);
                    ToolTipService.SetToolTip(duration, ex.Message);
                    PART_DialogOkButton.IsEnabled = false;
                }
            }
        }

        private void patternEndDate_DateTimeChanged(object sender, NullablePropertyChangedEventArgs<DateTime> e)
        {
            if (_isLoaded)
            {
                try
                {
                    _pattern.PatternEndDate = patternEndDate.DateTime.Value;
                    if (!PART_DialogOkButton.IsEnabled)
                    {
                        patternEndDate.ClearValue(Control.ForegroundProperty);
                        patternEndDate.ClearValue(Control.BorderBrushProperty);
                        patternEndDate.ClearValue(Control.BorderThicknessProperty);
                        patternEndDate.ClearValue(ToolTipService.ToolTipProperty);
                        Validate();
                    }
                }
                catch (Exception ex)
                {
                    patternEndDate.BorderBrush = patternEndDate.Foreground = new SolidColorBrush(Colors.Red);
                    patternEndDate.BorderThickness = new Thickness(2);
                    ToolTipService.SetToolTip(patternEndDate, ex.Message);
                    PART_DialogOkButton.IsEnabled = false;
                }
            }
        }
        #endregion

        private void WeeklyRB_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                _pattern.RecurrenceType = RecurrenceTypeEnum.Weekly;
            }
            WeeklyDetails.Visibility = Visibility.Visible;
            MonthlyDetails.Visibility = Visibility.Collapsed;
            YearlyDetails.Visibility = Visibility.Collapsed;
            DailyDetails.Visibility = Visibility.Collapsed;
        }

        private void MonthlyGroupRB_Checked(object sender, RoutedEventArgs e)
        {
            MonthlyDetails.Visibility = Visibility.Visible;
            WeeklyDetails.Visibility = Visibility.Collapsed;
            YearlyDetails.Visibility = Visibility.Collapsed;
            DailyDetails.Visibility = Visibility.Collapsed;
        }

        private void YearlyGroupRB_Checked(object sender, RoutedEventArgs e)
        {
            YearlyDetails.Visibility = Visibility.Visible;
            WeeklyDetails.Visibility = Visibility.Collapsed;
            MonthlyDetails.Visibility = Visibility.Collapsed;
            DailyDetails.Visibility = Visibility.Collapsed;
        }

        private void DailyGroupRB_Checked(object sender, RoutedEventArgs e)
        {
            DailyDetails.Visibility = Visibility.Visible;
            WeeklyDetails.Visibility = Visibility.Collapsed;
            MonthlyDetails.Visibility = Visibility.Collapsed;
            YearlyDetails.Visibility = Visibility.Collapsed;
        }
#pragma warning disable 1591
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DependencyObject parentControl = VTreeHelper.GetParentOfType((DependencyObject)e.OriginalSource, GetType());
                if (parentControl != null)
                {
                    PART_DialogCancelButton_Click(this, null);
                }
            }
            base.OnPreviewKeyDown(e);
        }
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            base.OnMouseWheel(e);
        }
#pragma warning restore 1591
    }
}
