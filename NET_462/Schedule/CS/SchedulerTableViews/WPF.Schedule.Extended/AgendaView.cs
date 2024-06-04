using C1.Schedule;
using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace C1.WPF.Schedule
{
    // todo:
    // - add properties for Start and End dates, to allow date range different from Scheduler dates
    // - filter by appointment owner or resources?
    // - view title?

    /// <summary>
    /// Determines view type for Agenda View.
    /// </summary>
    public enum AgendaViewType
    {
        /// <summary>
        /// Show agenda for current day.
        /// </summary>
        Today,
        /// <summary>
        /// Show agenda for next 7 days, starting from the current day.
        /// </summary>
        Week,
        /// <summary>
        /// Show agenda for date range.
        /// </summary>
        DateRange
    }

    /// <summary>
    /// Represents a view that displays appointments grouped by date in a table, single row for appointment.
    /// </summary>
    public class C1AgendaView : BaseTableView
    {
        //------------------------------------------------
        #region ** Fields
        private bool _showEmptyDays = false;
        private AgendaViewType _viewType = AgendaViewType.Week;
        private DayCollection _days;
        private DateTime _start;
        private DateTime _end;
        #endregion

        //------------------------------------------------
        #region ** Initializing
        /// <summary>
        /// Initializes the new instance of the <see cref="AgendaView"/> class.
        /// </summary>
        public C1AgendaView() : base()
        {
            HeadersVisibility = HeadersVisibility.None;
            this.SelectionMode = FlexGrid.SelectionMode.Row;
            IsReadOnly = true;

            // appointment time
            Column col = new Column();
            col.DataType = typeof(string);
            col.HorizontalAlignment = HorizontalAlignment.Left;
            col.Width = new GridLength(78);
            this.Columns.Add(col);

            // availability status (only draw corresponding brush)
            col = new Column();
            col.Width = new GridLength(5);
            this.Columns.Add(col);

            // appointment subject
            col = new Column();
            col.DataType = typeof(string);
            col.HorizontalAlignment = HorizontalAlignment.Left;
            col.TextWrapping = true;
            col.Width = new GridLength(1, GridUnitType.Star);
            this.Columns.Add(col);

            base.CellFactory = new StatusCellFactory();
        }
        #endregion

        //------------------------------------------------
        #region ** Object Model
        /// <summary>
        /// Specifies current view type.
        /// </summary>
        [DefaultValue(AgendaViewType.Week)]
        public AgendaViewType ViewType
        {
            get { return _viewType; }
            set
            {
                if (_viewType != value)
                {
                    _viewType = value;
                    RefreshView();
                }
            }
        }

        /// <summary>
        /// Specifies whether days without events should be displayed.
        /// </summary>
        [DefaultValue(false)]
        public bool ShowEmptyDays
        {
            get { return _showEmptyDays; }
            set
            {
                if (_showEmptyDays != value)
                {
                    _showEmptyDays = value;
                    RefreshView();
                }
            }
        }
        #endregion

        //------------------------------------------------
        #region ** Overrides

        protected override void OnIsVisibleChanged()
        {
            base.OnIsVisibleChanged();
            RefreshView();
        }

        internal override void AttachScheduler(C1Scheduler owner)
        {
            base.AttachScheduler(owner);
            _days = owner.DataStorage.Days;
            RefreshView();
        }

        internal override void DetachScheduler(C1Scheduler owner)
        {
            base.DetachScheduler(owner);
            _days = null;
        }

        protected override void RefreshView()
        {
            if (!IsVisible || Scheduler == null)
            {
                // only refresh when not hidden
                return;
            }

            Selection = new CellRange();

            // get new days
            _start = DateTime.Today;
            _end = _start;
            switch (_viewType)
            {
                case AgendaViewType.DateRange:
                    _start = Scheduler.Start;
                    _end = Scheduler.End;
                    break;
                case AgendaViewType.Week:
                    _end = _start.AddDays(7);
                    break;
            }

            // clear rows 
            Rows.Clear();

            // re-fill grid with data
            // fill rows
            var start = _start;
            while (start <= _end)
            {
                var day = _days[start];
                if (_showEmptyDays || day.HasActivity)
                {
                    var dayRow = new GroupRow();
                    Rows.Add(dayRow);
                    dayRow.Tag = day;
                    dayRow[0] = GetDayDescription(day.Date);
                    CellRange rg = new CellRange(dayRow.Index, 0, dayRow.Index, 2);
                    if (day.HasActivity)
                    {
                        var appointments = day.Appointments;
                        appointments.Sort(Scheduler.AppointmentComparison);
                        for (int i = 0; i < appointments.Count; i++)
                        {
                            // create appointment row
                            Appointment app = appointments[i];
                            var row = new Row();
                            row.Background = ((C1.WPF.Schedule.C1Brush)app.Label.Brush).Brush; // background the same as Appointment label
                            row.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black); // always black foreground
                            Rows.Add(row);
                            row.Tag = app;
                            row[2] = app.Subject;
                            if (!string.IsNullOrEmpty(app.Location))
                            {
                                row[2] += " (" + app.Location + ")";
                            }
                            if (app.AllDayEvent)
                            {
                                row[0] = "All day";
                            }
                            else
                            {
                                if (app.Start.Date == start)
                                {
                                    // short appointment
                                    row[0] = app.Start.ToShortTimeString();
                                    if (app.End.AddMilliseconds(-1).Date == start)
                                    {
                                        row[0] += "-" + app.End.ToShortTimeString();
                                    }
                                }
                                else if (app.End.AddMilliseconds(-1).Date == start)
                                {
                                    row[0] = "ends " + app.End.ToShortTimeString();
                                }
                            }
                        }
                    }
                    else
                    {
                        dayRow[0] += ":  No events";
                    }
                }
                start = start.AddDays(1);
            }
            AutoSizeRows(0, Rows.Count - 1, 2, true);

            if (_viewType == AgendaViewType.DateRange && Rows.Count > 2)
            {
                // scroll to current date (or first day with activity if current date is invisible)
                var day = _days[DateTime.Today];
                if (!_showEmptyDays)
                {
                    while (!day.HasActivity && day.Date <= _end)
                    {
                        day = _days[day.Date.AddDays(1)];
                    }
                }
                if (day != null)
                {
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        if (day.Equals(Rows[i].Tag))
                        {
                            base.ScrollIntoView(Rows.Count - 1, 0);
                            base.ScrollIntoView(i, 0); 
                            break;
                        }
                    }
                }
            }
        }

        private string GetDayDescription(DateTime date)
        {
            date = date.Date;
            if (date == DateTime.Today)
                return "Today";
            if (date == DateTime.Today.AddDays(1))
                return "Tomorrow";
            return date.ToShortDateString();
        }
        #endregion
    }

    // CellFactory for status cells
    public class StatusCellFactory : CellFactory
    {

        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            if ( rng.Column == 1)
            {
                var r = grid.Rows[rng.Row];
                Appointment app = r.Tag as Appointment;
                if (app != null && app.BusyStatus != null)
                {
                    bdr.Background = ((C1.WPF.Schedule.C1Brush)app.BusyStatus.Brush).Brush;
                }
                return;
            }
            base.CreateCellContent(grid, bdr, rng);
        }
    }
}
