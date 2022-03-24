using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using C1.Schedule;
using C1.WPF.Calendar;
using C1.WPF.Core;
using C1.WPF.DateTimeEditors;
using C1.WPF.Input;
using C1.WPF.Schedule;
using SchedulerExplorer.Resources;
using DateList = C1.WPF.Schedule.DateList;
using System.Globalization;

namespace SchedulerExplorer
{
    /// <summary>
    ///     Interaction logic for DataBinding.xaml
    /// </summary>
    public partial class DataBinding : UserControl
    {
        AppointmentBOList _list = null;
        bool _updatingSelection = false;

        public DataBinding()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            // create collection of custom business objects
            _list = new AppointmentBOList();
            InitializeComponent();
            Tag = AppResources.DataBindingDescription;
            scheduler1.StyleChanged += new System.EventHandler<RoutedEventArgs>(scheduler1_StyleChanged);
            scheduler1_StyleChanged(null, null);
            scheduler1.LayoutUpdated += new EventHandler(scheduler1_LayoutUpdated);
            scheduler1.VisibleDates.CollectionChanged += VisibleDates_CollectionChanged;
        }

        void scheduler1_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateCalendarSelection();
        }

        private void VisibleDates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCalendarSelection();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // add a new AppointmentBusinessObject (that creates a new appointment in a C1Scheduler control)
            AppointmentBusinessObject newObject = new AppointmentBusinessObject();
            newObject.Subject = "The test business object " + _list.Count.ToString();
            _list.Add(newObject);
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            // clear the list of business objects (that clears all appointments from the C1Scheduler control)
            _list.Clear();
        }

        /// <summary>
        /// Gets a collection of custom business objects, used as a data source for C1Scheduler's AppointmentStorage.
        /// </summary>
        public AppointmentBOList Appointments
        {
            get
            {
                return _list;
            }
        }

        private void scheduler1_StyleChanged(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state according to the current C1Scheduler view
            if (scheduler1.Style == scheduler1.WorkingWeekStyle)
            {
                btnWorkWeek.IsChecked = true;
            }
            else if (scheduler1.Style == scheduler1.WeekStyle)
            {
                btnWeek.IsChecked = true;
            }
            else if (scheduler1.Style == scheduler1.MonthStyle)
            {
                btnMonth.IsChecked = true;
            }
            else if (scheduler1.Style == scheduler1.OneDayStyle)
            {
                btnDay.IsChecked = true;
            }
            else
            {
                btnTimeLine.IsChecked = true;
            }
            UpdateCalendarSelection();
        }

        void selected_date_changed(object sender, CalendarSelectionChangedEventArgs e)
        {
            if (!_updatingSelection)
            {
                var calendar = sender as C1Calendar;
                if (calendar?.SelectedDate == default(DateTime))
                    return;

                scheduler1.VisibleDates.BeginUpdate();
                scheduler1.VisibleDates.Clear();
                foreach (var d in calendar.SelectedDates)
                {
                    scheduler1.VisibleDates.Add(d);
                }
                scheduler1.VisibleDates.EndUpdate();
            }
        }

        void UpdateCalendarSelection()
        {
            if (!_updatingSelection && (calendar1.SelectedDates == null ||
                (scheduler1.VisibleDates.Count != calendar1.SelectedDates.Count
                || (calendar1.SelectedDates.Count > 0 && scheduler1.VisibleDates[0] != calendar1.SelectedDates[0]))))
            {
                _updatingSelection = true;
                calendar1.SelectedDates = scheduler1.VisibleDates.ToList();
                _updatingSelection = false;
            }
        }
    }
}