using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using C1.Schedule;
using C1.WPF;
using C1.WPF.DateTimeEditors;
using C1.WPF.Schedule;
using C1.WPF.Core;
using C1.WPF.Docking;
using C1.WPF.Localization;
using C1.Schedule;

namespace SchedulerExplorer
{
    /// <summary>
    /// The <see cref="EditAppointmentControl"/> control allows editing of all appointment properties.
    /// </summary>
    public partial class EditAppointmentControl : UserControl
    {
        #region ** fields
        private ContentControl _parentWindow = null;
        private Appointment _appointment;
        private C1Scheduler _scheduler;
        private bool _isLoaded = false;

        private TimeSpan _defaultStart;
        private TimeSpan _defaultDuration;
        #endregion

        #region ** initialization
        /// <summary>
        /// Creates the new instance of the <see cref="EditAppointmentControl"/> class.
        /// </summary>
        public EditAppointmentControl()
        {
            InitializeComponent();
        }

        private void EditAppointmentControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isLoaded)
            {
                _appointment = DataContext as Appointment;
                _defaultStart = _appointment.AllDayEvent ? TimeSpan.FromHours(8) : _appointment.Start.TimeOfDay;
                _defaultDuration = _appointment.AllDayEvent ? TimeSpan.FromMinutes(30) : _appointment.Duration;
                if (!System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted)
                {
                    _parentWindow = (ContentControl)VTreeHelper.GetParentOfType(this, typeof(Window));
                }
                else
                    _parentWindow = (ContentControl)VTreeHelper.GetParentOfType(this, typeof(C1Window));
                if (_parentWindow != null)
                {
                    Binding bnd = new Binding("Header");
                    bnd.Source = this;
                    if (_parentWindow is Window)
                    {
                        _parentWindow.SetBinding(Window.TitleProperty, bnd);
                    }
                    else
                    {
                        _parentWindow.SetBinding(C1Window.HeaderProperty, bnd);
                    }

                    if (_parentWindow is Window)
                    {
                        ((Window)_parentWindow).Closed += new EventHandler(_parentWindow_Closed);
                    }
                    else
                        ((C1Window)_parentWindow).Closed += new EventHandler(_parentWindow_Closed);
                }
                if (_appointment != null)
                {
                    ((System.ComponentModel.INotifyPropertyChanged)_appointment).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(appointment_PropertyChanged);
                    if (_appointment.ParentCollection != null)
                    {
                        _scheduler = (C1Scheduler)((C1.WPF.Schedule.C1ScheduleStorage)_appointment.ParentCollection.ParentStorage.ScheduleStorage).Scheduler;
                        if (_appointment.AllDayEvent)
                        {
                            _defaultStart = _scheduler.CalendarHelper.StartDayTime;
                            _defaultDuration = _scheduler.CalendarHelper.Info.TimeScale;
                        }
                    }
                    UpdateWindowHeader();
                    UpdateRecurrenceState();
                    UpdateCollections();
                    UpdateEndCalendar();
                    if (_appointment.AllDayEvent)
                    {
                        startCalendar.EditMode = endCalendar.EditMode = C1DateTimePickerEditMode.Date;
                    }
                    else
                    {
                        startCalendar.EditMode = endCalendar.EditMode = C1DateTimePickerEditMode.DateTime;
                    }
                    UpdateEditingControls();
                }
                if (_parentWindow != null && _appointment != null)
                {
                    _isLoaded = true;
                }
            }
            subject.Focus();
        }
#pragma warning disable 1591
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            UpdateCollections();
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdateCollections();
        }
#pragma warning restore 1591
        void _parentWindow_Closed(object sender, EventArgs e)
        {
            ((System.ComponentModel.INotifyPropertyChanged)_appointment).PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(appointment_PropertyChanged);
            if (_parentWindow is Window)
            {
                ((Window)_parentWindow).Closed -= new EventHandler(_parentWindow_Closed);
            }
            else
                ((C1Window)_parentWindow).Closed -= new EventHandler(_parentWindow_Closed);
        }

        #endregion

        #region ** object model
        /// <summary>
        /// Gets or sets an <see cref="Appointment"/> object representing current DataContext.
        /// </summary>
        public Appointment Appointment
        {
            get
            {
                return _appointment;
            }
            set
            {
                _appointment = value;
                if (_parentWindow != null)
                {
                    _parentWindow.Content =
                    _parentWindow.DataContext = value;
                }
                DataContext = value;
                if (_appointment != null)
                {
                    UpdateWindowHeader();
                    UpdateRecurrenceState();
                    UpdateCollections();
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="String"/> value which can be used as an Appointment window header.
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            private set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string),
            typeof(EditAppointmentControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets recurrence pattern description.
        /// </summary>
        public string PatternDescription
        {
            get { return (string)GetValue(PatternDescriptionProperty); }
            private set { SetValue(PatternDescriptionProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PatternDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PatternDescriptionProperty =
            DependencyProperty.Register("PatternDescription", typeof(string),
            typeof(EditAppointmentControl), new PropertyMetadata(string.Empty));

        #endregion

        #region ** private stuff
        private void UpdateWindowHeader()
        {
            string result;
            string subject = string.Empty;
            bool allDay = false;
            if (_appointment != null)
            {
                subject = _appointment.Subject;
                allDay = chkAllDay.IsChecked.Value;
            }
            if (String.IsNullOrEmpty(subject))
            {
                subject = C1Localizer.GetString("EditAppointment", "Untitled", "Untitled");
            }
            if (allDay)
            {
                result = C1Localizer.GetString("EditAppointment", "Event", "Event") + " - " + subject;
            }
            else
            {
                result = C1Localizer.GetString("EditAppointment", "Appointment", "Appointment") + " - " + subject;
            }

            Header = result;
        }

        private void UpdateRecurrenceState()
        {
            switch (_appointment.RecurrenceState)
            {
                case RecurrenceStateEnum.Master:
                    PatternDescription = Appointment.GetRecurrencePattern().Description;
                    startEndPanel.Visibility = Visibility.Collapsed;
                    recurrenceInfoPanel.Visibility = Visibility.Visible;
                    break;
                default:
                    PatternDescription = string.Empty;
                    startEndPanel.Visibility = Visibility.Visible;
                    recurrenceInfoPanel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        void appointment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateRecurrenceState();
            UpdateEndCalendar();
        }

        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                PART_DialogSaveButton.IsEnabled = false;
                saveAsButton.IsEnabled = false;
                reccButton.IsEnabled = false;
            }
            else
            {
                PART_DialogSaveButton.IsEnabled = true;
                saveAsButton.IsEnabled = true;
                reccButton.IsEnabled = true;
            }
        }

        private void SetAppointment()
        {
            subject.Focus();
            location.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            body.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void UpdateCollections()
        {
            if (Appointment != null)
            {
                resources.Text = Appointment.Resources.ToString();
                contacts.Text = Appointment.Links.ToString();
                categories.Text = Appointment.Categories.ToString();
            }
        }

        private void UpdateEditingControls()
        {
            if (_scheduler != null)
            {
                C1SchedulerSettings settings = _scheduler.Settings;
                categories.ParentAppointment = resources.ParentAppointment = contacts.ParentAppointment = _appointment;
                categories.Scheduler = resources.Scheduler = contacts.Scheduler = _scheduler;
                if (!settings.AllowCategoriesEditing && _scheduler.DataStorage.CategoryStorage.Categories.Count == 0)
                {
                    categories.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    categories.SourceCollection = _scheduler.DataStorage.CategoryStorage.Categories;
                    categories.TargetCollection = _appointment.Categories;
                    categories.ItemType = typeof(Category);
                    categories.WindowTitle = "Categories";
                    categories.ShowButton = settings.AllowCategoriesMultiSelection || settings.AllowCategoriesEditing;
                }
                if (!settings.AllowResourcesEditing && _scheduler.DataStorage.ResourceStorage.Resources.Count == 0)
                {
                    resources.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    resources.SourceCollection = _scheduler.DataStorage.ResourceStorage.Resources;
                    resources.TargetCollection = _appointment.Resources;
                    resources.ItemType = typeof(Resource);
                    resources.WindowTitle = "Resources";
                    resources.ShowButton = settings.AllowResourcesEditing || settings.AllowResourcesMultiSelection;
                }
                if (!settings.AllowContactsEditing && _scheduler.DataStorage.ContactStorage.Contacts.Count == 0)
                {
                    contacts.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    contacts.SourceCollection = _scheduler.DataStorage.ContactStorage.Contacts;
                    contacts.TargetCollection = _appointment.Links;
                    contacts.ItemType = typeof(Contact);
                    contacts.WindowTitle = "Contacts";
                    contacts.ShowButton = settings.AllowContactsEditing || settings.AllowContactsMultiSelection;
                }
            }
        }

        private void PART_DialogSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SetAppointment();
            if (_parentWindow is Window)
            {
                _parentWindow.Tag = "true";
                ((Window)_parentWindow).Close();
            }
            else
                ((C1Window)_parentWindow).DialogResult = MessageBoxResult.OK;
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            SetAppointment();
        }

        private void subject_TextChanged(object sender, TextChangedEventArgs e)
        {
            subject.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateWindowHeader();
        }

        private void chkAllDay_Checked(object sender, RoutedEventArgs e)
        {
            startCalendar.EditMode = endCalendar.EditMode = C1DateTimePickerEditMode.Date;
            UpdateWindowHeader();
        }

        private void chkAllDay_Unchecked(object sender, RoutedEventArgs e)
        {
            _appointment.Start = _appointment.Start.Add(_defaultStart);
            _appointment.Duration = _defaultDuration;
            startCalendar.EditMode = endCalendar.EditMode = C1DateTimePickerEditMode.DateTime;
            UpdateWindowHeader();
        }

        private void endCalendar_DateTimeChanged(object sender, NullablePropertyChangedEventArgs<DateTime> e)
        {
            if (_appointment != null)
            {
                DateTime end = endCalendar.DateTime.Value;
                if (_appointment.AllDayEvent)
                {
                    end = end.AddDays(1);
                }
                if (end < Appointment.Start)
                {
                    endCalendar.BorderBrush = endCalendar.Foreground = new SolidColorBrush(Colors.Red);
                    endCalendar.BorderThickness = new Thickness(2);
                    ToolTipService.SetToolTip(endCalendar, C1Localizer.GetString("Exceptions", "StartEndValidationFailed", "The End value should be greater than Start value."));
                    PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled = false;
                }
                else
                {
                    _appointment.End = end;
                    if (!PART_DialogSaveButton.IsEnabled)
                    {
                        PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled = true;
                        endCalendar.ClearValue(Control.ForegroundProperty);
                        endCalendar.ClearValue(Control.BorderBrushProperty);
                        endCalendar.ClearValue(Control.BorderThicknessProperty);
                        endCalendar.ClearValue(ToolTipService.ToolTipProperty);
                    }
                }
            }
        }

        private void UpdateEndCalendar()
        {
            DateTime end = _appointment.End;
            if (_appointment.AllDayEvent)
            {
                end = end.AddDays(-1);
            }
            endCalendar.DateTime = end;
            if (!PART_DialogSaveButton.IsEnabled)
            {
                PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled = true;
                endCalendar.ClearValue(Control.BackgroundProperty);
                endCalendar.ClearValue(Control.ForegroundProperty);
                endCalendar.ClearValue(Control.BorderBrushProperty);
                endCalendar.ClearValue(Control.BorderThicknessProperty);
                endCalendar.ClearValue(ToolTipService.ToolTipProperty);
            }
        }

#pragma warning disable 1591
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            base.OnMouseWheel(e);
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DependencyObject parentControl = VTreeHelper.GetParentOfType((DependencyObject)e.OriginalSource, GetType());
                if (parentControl != null)
                {
                    if (_parentWindow is Window)
                    {
                        ((Window)_parentWindow).Close();
                    }
                    else
                    {
                        ((C1Window)_parentWindow).DialogResult = MessageBoxResult.Cancel;
                    }
                }
            }
            base.OnPreviewKeyDown(e);
        }

#pragma warning restore 1591
        #endregion
    }
}
