using C1.C1Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace C1.WPF.Schedule
{
    /// <summary>
    /// Represents one day in the C1Schedule. 
    /// </summary>
    internal class Day
    {
        #region fields
        private DateTime _date;
        private List<Appointment> _appointments;
        #endregion

        #region ctor
        /// <summary>
        /// Creates new Day object.
        /// </summary>
        internal Day(DateTime date)
        {
            _date = date.Date;
            _appointments = new List<Appointment>();
        }
        #endregion

        #region object model
        internal DateTime Date
        {
            get
            {
                return _date;
            }
        }

        internal bool HasActivity
        {
            get
            {
                return (_appointments.Count != 0);
            }
        }

        internal List<Appointment> Appointments
        {
            get
            {
                return _appointments;
            }
        }

        internal void AddAppointment(Appointment app)
        {
            if (!_appointments.Contains(app))
            {
                _appointments.Add(app);
            }
        }

        internal void Clear()
        {
            _appointments.Clear();
        }

        // removes specified appointment and returns the number of remaining appointments
        internal int RemoveAppointment(Appointment app)
        {
            _appointments.Remove(app);
            return _appointments.Count;
        }
        #endregion

        #region overrides
        public override string ToString()
        {
            return Date.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Collection of all days in the C1Schedule component.
    /// </summary>
    internal class DayCollection : KeyedCollection<DateTime, Day>, IDisposable
    {
        #region fields
        internal C1.C1Schedule.DateList _boldedDates; // list of dates with appointments
        private CalendarInfo _info;
        #endregion

        #region ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="DayCollection"/> class.
        /// </summary>
        internal DayCollection(CalendarInfo info)
        {
            _info = info;
            _boldedDates = new C1.C1Schedule.DateList();
        }
        #endregion

        #region interface
        /// <summary>
        /// Returns DayCollection for all dates from the start till the end.
        /// </summary>
        /// <returns></returns>
        internal void FillDayCollection(DateTime start, DateTime end)
        {
            start = start.Date;
            while (start <= end)
            {
                AddDay(start);
                start = start.AddDays(1);
            }
        }

        // returns the first day of week, containing specified date.
        internal DateTime GetLastWeekDay(DateTime day)
        {
            day = day.Date;
            while (true)
            {
                DateTime nextDay = day.AddDays(1);
                if (nextDay.DayOfWeek != _info.WeekStart)
                {
                    day = nextDay;
                    AddDay(day);
                }
                else
                {
                    break;
                }
            }
            return day;

        }

        // adds appointment
        internal void AddAppointment(Appointment appointment)
        {
            DateTime start = appointment.Start.Date;
            DateTime end = (appointment.End == start) ? start.AddMinutes(1) : appointment.End;
            while (start < end)
            {
                if (this.Contains(start))
                {
                    this[start].AddAppointment(appointment);
                }
                if (appointment.RecurrenceState != RecurrenceStateEnum.Removed)
                {
                    _boldedDates.Add(start);
                }
                start = start.AddDays(1);
            }
        }

        internal void ClearAppointments()
        {
            int boldedCount = _boldedDates.Count;
            if (boldedCount > 0)
            {
                for (int i = boldedCount - 1; i >= 0; i--)
                {
                    this[_boldedDates.Items[i]].Clear();
                }
                _boldedDates.Clear();
            }
        }

        // removes appointment or all its' occurrences
        internal void RemoveAppointment(Appointment appointment)
        {
            RemoveAppointment(appointment, false);
        }

        // removes appointment or all its' occurrences
        internal void RemoveAppointment(Appointment appointment, bool recurrent)
        {
            if (recurrent || appointment.IsRecurring)
            {
                List<DateTime> boldedDates = _boldedDates.Items;
                for (int i = boldedDates.Count - 1; i >= 0; i--)
                {
                    List<Appointment> appList = this[boldedDates[i]].Appointments;
                    for (int j = appList.Count - 1; j >= 0; j--)
                    {
                        Appointment app = appList[j];
                        if (app.ParentRecurrence == appointment || app == appointment || app.Key.Equals(appointment.Key)
                            || (app.ParentRecurrence != null && app.ParentRecurrence.Key.Equals(appointment.Key)))
                        {
                            appList.Remove(app);
                            if (appList.Count == 0)
                            {
                                _boldedDates.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            else
            {
                RemoveSingleAppointment(appointment);
            }
        }

        internal void RemoveSingleAppointment(Appointment appointment)
        {
            List<DateTime> boldedDates = _boldedDates.Items;
            for (int i = boldedDates.Count - 1; i >= 0; i--)
            {
                if (this[boldedDates[i]].RemoveAppointment(appointment) == 0)
                {
                    _boldedDates.RemoveAt(i);
                }
            }
        }

        // changes an appointment
        internal void ChangeAppointment(Appointment appointment)
        {
            RemoveSingleAppointment(appointment);
            DateTime start = appointment.Start.Date;
            DateTime end = (appointment.End == start) ? start.AddMinutes(1) : appointment.End;
            while (start < end)
            {
                this[start].AddAppointment(appointment);
                if (appointment.RecurrenceState != RecurrenceStateEnum.Removed)
                {
                    _boldedDates.Add(start);
                }
                start = start.AddDays(1);
            }
        }
        #endregion

        #region overrides
        public new Day this[DateTime value]
        {
            get
            {
                return AddDay(value.Date);
            }
        }

        protected override void ClearItems()
        {
            _boldedDates.Clear();
            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            _boldedDates.Remove(this[index].Date);
            base.RemoveItem(index);
        }

        /// <summary>
        /// Extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>The key for the specified element.</returns>
        protected override DateTime GetKeyForItem(Day item)
        {
            return item.Date;
        }
        #endregion

        #region ** private stuff
        // Returns Day object for specified date.
        // Make sure that date.TimeOfDay is zero before call.
        private Day GetDay(DateTime date)
        {
            Day day = new Day(date);
            return day;
        }

        // Make sure that date.TimeOfDay is zero before call.
        private Day AddDay(DateTime day)
        {
            if (Contains(day))
            {
                return base[day];
            }
            else
            {
                Day newDay = GetDay(day);
                Add(newDay);
                return newDay;
            }
        }

        public void Dispose()
        {
            _info = null;
            ClearAppointments();
            ClearItems();
        }
        #endregion
    }


}
