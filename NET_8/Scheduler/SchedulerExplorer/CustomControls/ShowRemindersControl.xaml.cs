using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using C1.Schedule;
using C1.WPF;
using C1.WPF.Schedule;
using System.Windows.Input;
using C1.WPF.Core;
using C1.WPF.Docking;
using C1.WPF.Localization;

namespace SchedulerExplorer
{
    /// <summary>
    /// The <see cref="ShowRemindersControl"/> displays currently active reminders and allows to control them.
    /// </summary>
    public partial class ShowRemindersControl : UserControl
    {
        #region dependency properties
        /// <summary>
        /// Gets or sets reference to the <see cref="C1Scheduler"/> control 
        /// which is an owner of displayed <see cref="Reminder"/> objects.
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
            DependencyProperty.Register("Scheduler", typeof(C1Scheduler), typeof(ShowRemindersControl),
                                        new PropertyMetadata(OnSchedulerChanged));

        private static void OnSchedulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShowRemindersControl)d).OnSchedulerChanged(e);
        }
        private void OnSchedulerChanged(DependencyPropertyChangedEventArgs e)
        {
            C1Scheduler oldSch = e.OldValue as C1Scheduler;
            if (oldSch != null)
            {
                ((INotifyCollectionChanged)oldSch.ActiveReminders).CollectionChanged -= new NotifyCollectionChangedEventHandler(ShowRemindersControl_CollectionChanged);
            }
            if (Scheduler != null)
            {
                ((INotifyCollectionChanged)Scheduler.ActiveReminders).CollectionChanged += new NotifyCollectionChangedEventHandler(ShowRemindersControl_CollectionChanged);
            }
            UpdateTitle();
        }
        #endregion

        #region ** fields
        private ContentControl _parentWindow = null;
        private readonly DispatcherTimer _timer;
        private bool _isLoaded = false;
        #endregion

        #region ** initializing
        /// <summary>
        /// Initializes a new instance of the <see cref="ShowRemindersControl"/> class.
        /// </summary>
        public ShowRemindersControl()
        {
            InitializeComponent();
            snoozeTimeSpansCombo.Value = TimeSpan.FromMinutes(5);
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Timer_Tick);
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isLoaded)
            {
                if (!System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted)
                {
                    _parentWindow = (ContentControl)VTreeHelper.GetParentOfType(this, typeof(Window));
                }
                else
                    _parentWindow = (ContentControl)VTreeHelper.GetParentOfType(this, typeof(C1Window));
                if (_parentWindow != null)
                {
                    if (Scheduler == null)
                    {
                        Scheduler = _parentWindow.DataContext as C1Scheduler;
                    }
                    if (_parentWindow is Window)
                    {
                        ((Window)_parentWindow).Closed += new EventHandler(_parentWindow_Closed);
                    }
                    else
                    {
                        ((C1Window)_parentWindow).Closed += new EventHandler(_parentWindow_Closed);
                        ((C1Window)_parentWindow).WindowStateChanged += new EventHandler<PropertyChangedEventArgs<C1WindowState>>(_parentWindow_WindowStateChanged);
                    }
                }
                UpdateTitle();
                remList.SelectionChanged += new SelectionChangedEventHandler(remList_SelectionChanged);
                UpdateTimer(1);
                remList.Focus();
                _isLoaded = true;
            }
            ShowRemindersControl_CollectionChanged(null, null);
        }

        void _parentWindow_Closed(object sender, EventArgs e)
        {
            _timer.Tick -= new EventHandler(Timer_Tick);
            _timer.Stop();
            if (_parentWindow is Window)
            {
                ((Window)_parentWindow).Closed -= new EventHandler(_parentWindow_Closed);
            }
            else
            {
                ((C1Window)_parentWindow).Closed -= new EventHandler(_parentWindow_Closed);
                ((C1Window)_parentWindow).WindowStateChanged -= new EventHandler<PropertyChangedEventArgs<C1WindowState>>(_parentWindow_WindowStateChanged);
            }
            if (Scheduler != null)
            {
                ((INotifyCollectionChanged)Scheduler.ActiveReminders).CollectionChanged -= new NotifyCollectionChangedEventHandler(ShowRemindersControl_CollectionChanged);
            }
            remList.SelectionChanged -= new SelectionChangedEventHandler(remList_SelectionChanged);
        }

        // update reminders due is time on timer ticks
        private void UpdateTimer(int seconds)
        {
            _timer.Stop();
            _timer.Interval = TimeSpan.FromSeconds(seconds);
            _timer.Start();
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            if (_parentWindow != null)
            {
                Reminder rem = _parentWindow.Tag as Reminder;
                if (rem != null && (_needChangeSelection || remList.SelectedItems.Count == 0))
                {
                    _needChangeSelection = false;
                    remList.SelectedItem = rem;
                }
            }
            UpdateTimer(20);
        }
        void _parentWindow_WindowStateChanged(object sender, PropertyChangedEventArgs<C1WindowState> e)
        {
            if (e.NewValue != C1WindowState.Minimized)
            {
                UpdateTimer(1);
            }
        }
        #endregion

        #region ** private stuff
        bool _needChangeSelection = false;
        void ShowRemindersControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTimer(1);
            _needChangeSelection = true;
            if (_parentWindow != null)
            {
                Reminder rem = _parentWindow.Tag as Reminder;
                if (rem != null)
                {
                    remList.SelectedItem = rem;
                }
            }
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (_parentWindow != null)
            {
                string title = String.Empty;
                ReadOnlyObservableCollection<Reminder> rems = Scheduler.ActiveReminders;
                if (rems == null || rems.Count == 0)
                {
                    title = C1Localizer.GetString("ShowReminders", "Title", "Reminders");
                }
                else
                {
                    if (rems.Count == 1)
                    {
                        title = C1Localizer.GetString("ShowReminders", "OneReminder", "1 Reminder");
                    }
                    else
                    {
                        title = string.Format(C1Localizer.GetString("ShowReminders",
                            "RemindersNumber", "{0} Reminders"), rems.Count);
                    }
                }
                if (_parentWindow is Window)
                {
                    ((Window)_parentWindow).Title = title;
                }
                else
                    ((C1Window)_parentWindow).Header = title;
            }
        }

        private void snoozeButton_Click(object sender, RoutedEventArgs e)
        {
            Array ar = new object[2] { snoozeTimeSpansCombo.Value.Value, remList.SelectedItems };
            C1Scheduler.SnoozeCommand.Execute(ar, Scheduler);
        }

        void remList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (remList.Items.Count > 0 && remList.SelectedItems.Count == 0)
            {
                remList.SelectedIndex = 0;
            }
            UpdateSelectedInfo();
        }

        void UpdateSelectedInfo()
        {
            int count = remList.SelectedItems.Count;
            if (count == 1)
            {
                info.Visibility = subject.Visibility = Visibility.Visible;
                selectedReminders.Visibility = Visibility.Collapsed;
                snoozeButton.IsEnabled = true;
                dismissButton.IsEnabled = true;
                openButton.IsEnabled = true;
            }
            else if (count > 1)
            {
                info.Visibility = subject.Visibility = Visibility.Collapsed;
                selectedReminders.Visibility = Visibility.Visible;
                snoozeButton.IsEnabled = true;
                dismissButton.IsEnabled = true;
                openButton.IsEnabled = true;
            }
            else
            {
                info.Visibility = subject.Visibility = Visibility.Collapsed;
                selectedReminders.Visibility = Visibility.Visible;
                snoozeButton.IsEnabled = false;
                dismissButton.IsEnabled = false;
                openButton.IsEnabled = false;
            }
        }
#pragma warning disable 1591
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
                        ((C1Window)_parentWindow).Close();
                    }
                }
            }
            base.OnKeyDown(e);
        }
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            base.OnMouseWheel(e);
        }
#pragma warning restore 1591
        #endregion
    }
}
