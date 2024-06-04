using C1.Schedule;
using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

// todo:
// - filter by appointment owner or resources?

namespace C1.WPF.Schedule
{
    /// <summary>
    /// Grid control used as a base for different table views.
    /// </summary>
    public class BaseTableView : C1FlexGrid, IDisposable
    {
        //------------------------------------------------
        #region ** Initializing
        /// <summary>
        /// Initializes the new instance of the <see cref="BaseTableView"/> class.
        /// </summary>
        public BaseTableView() : base()
        {
            this.AutoGenerateColumns = false;
            this.Rows.DefaultSize = 20;
            IsVisibleChanged += BaseTableView_IsVisibleChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RefreshView();
        }

        private void BaseTableView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnIsVisibleChanged();
        }
        #endregion

        //------------------------------------------------
        #region ** Parent C1Scheduler control
        /// <summary>
        /// Gets or sets the <see cref="C1Scheduler"/> control which data should be reflected in the current view.
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
            DependencyProperty.Register("Scheduler", typeof(C1Scheduler), 
                typeof(BaseTableView), new PropertyMetadata(null, OnSchedulerChanged));

        private static void OnSchedulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BaseTableView)d).OnSchedulerChanged(e);
        }
        private void OnSchedulerChanged(DependencyPropertyChangedEventArgs e)
        {
            if ( e.OldValue != null)
            {
                DetachScheduler(e.OldValue as C1Scheduler);
            }
            if (e.NewValue != null)
            {
                AttachScheduler(e.NewValue as C1Scheduler);
            }
        }

        internal virtual void AttachScheduler(C1Scheduler scheduler)
        {
            scheduler.AppointmentAdded += _schedule_AppointmentAdded;
            scheduler.AppointmentDeleted += _schedule_AppointmentDeleted;
            scheduler.AppointmentChanged += _schedule_AppointmentChanged;
            scheduler.Loaded += _schedule_Loaded;
        }

        internal virtual void DetachScheduler(C1Scheduler scheduler)
        {
            if (scheduler != null)
            {
                scheduler.Loaded -= _schedule_Loaded;
                scheduler.AppointmentAdded -= _schedule_AppointmentAdded;
                scheduler.AppointmentDeleted -= _schedule_AppointmentDeleted;
                scheduler.AppointmentChanged -= _schedule_AppointmentChanged;
            }
        }

        private void _schedule_Loaded(object sender, RoutedEventArgs e)
        {
            // The C1Scheduler doesn't fire AppointmentAdded and other events while it is not loaded. 
            // So refresh view when it is loaded in case we missed some events.
            RefreshView(); 
        }

        private void _schedule_AppointmentChanged(object sender, AppointmentActionEventArgs e)
        {
            OnAppointmentChanged(e);
        }

        private void _schedule_AppointmentDeleted(object sender, AppointmentActionEventArgs e)
        {
            OnAppointmentDeleted(e);
        }

        private void _schedule_AppointmentAdded(object sender, AppointmentActionEventArgs e)
        {
            OnAppointmentAdded(e);
        }
        #endregion

        //------------------------------------------------
        #region ** protected
        protected virtual void RefreshView()
        {
            // does nothing here
        }

        protected virtual void OnAppointmentAdded(AppointmentActionEventArgs e)
        {
            RefreshView();
        }

        protected virtual void OnAppointmentDeleted(AppointmentActionEventArgs e)
        {
            RefreshView();
        }

        protected virtual void OnAppointmentChanged(AppointmentActionEventArgs e)
        {
            RefreshView();
        }

        /// <summary>
        /// Returns the <see cref="Appointment"/> object associated with the specified FlexGrid row index in the current view.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>The <see cref="Appointment"/> object.</returns>
        protected Appointment GetRowAppointment(int row)
        {
            if ( row < 0)
            {
                return null;
            }
            var gridRow = Rows[row];
            Appointment app = gridRow.Tag as Appointment ?? gridRow.DataItem as Appointment;
            if ( app != null)
            {
                return app;
            }
            DataRowView drv = gridRow.DataItem as DataRowView;
            if (drv == null)
            {
                // possible for group rows
                return null;
            }

            // todo: what else can be here? Check
            object key = drv["Id"];
            if (key.GetType() != typeof(Guid))
            {
                return null;
            }
            app = Scheduler.DataStorage.AppointmentStorage.Appointments[(Guid)key];
            return app;
        }

        /// <summary>
        /// Returns the <see cref="Appointment"/> object associated with the specified row in the current view.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>The <see cref="Appointment"/> object.</returns>
        protected Appointment GetRowAppointment(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            object key = row["Id"];
            if (key.GetType() != typeof(Guid))
            {
                return null;
            }
            Appointment app = Scheduler.DataStorage.AppointmentStorage.Appointments[(Guid)key];
            return app;
        }
        #endregion

        //------------------------------------------------
        #region ** Selection
        private List<Appointment> _selectedApps = new List<Appointment>();
        /// <summary>
        /// Gets the <see cref="List{Appointment}"/> object containing 
        /// the list of the currently selected <see cref="Appointment"/> objects.
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public List<Appointment> SelectedAppointments
        {
            get
            {
                return _selectedApps;
            }
        }

        /// <summary>
        /// Occurs when the list of selected appointments is changed.
        /// </summary>
        public event EventHandler SelectedAppointmentsChanged;
        internal void OnSelectedAppointmentsChanged(EventArgs args)
        {
            SelectedAppointmentsChanged?.Invoke(this, args);
        }

        protected override void OnSelectionChanged(CellRangeEventArgs e)
        {
            base.OnSelectionChanged(e);
            // find selected appointments
            List<Appointment> appsForSelect = new List<Appointment>();
            var range = Selection;
            if (range.IsValid && range.BottomRow < Rows.Count)
            {
                for (int i = range.TopRow; i <= range.BottomRow; i++)
                {
                    Appointment app = this.GetRowAppointment(i);
                    if (app != null)
                    {
                        appsForSelect.Add(app);
                    }
                }
            }
            bool needUpdate = _selectedApps.Count != appsForSelect.Count;
            if (!needUpdate)
            {
                foreach (var app in appsForSelect)
                {
                    if (!_selectedApps.Contains(app))
                    {
                        needUpdate = true;
                        break;
                    }
                }
            }
            if (needUpdate)
            {
                _selectedApps.Clear();
                _selectedApps.AddRange(appsForSelect);
                OnSelectedAppointmentsChanged(null);
            }
        }
        #endregion

        //------------------------------------------------
        #region ** Overrides
        protected virtual void OnIsVisibleChanged()
        {
            if (IsVisible)
            {
                OnSelectionChanged(null);
            }
        }

        protected override void OnDoubleClick(MouseButtonEventArgs e)
        {
            var hitTest = HitTest(e);
            if (IsReadOnly || Columns[hitTest.Column].IsReadOnly)
            {
                // open EditAppointmentDialog on double click on readonly areas
                Appointment app = GetRowAppointment(hitTest.Row);
                if (app != null)
                {
                    this.FinishEditing();
                    Scheduler.EditAppointmentDialog(app);
                    e.Handled = true;
                }
            }
            base.OnDoubleClick(e);
        }
        #endregion

        //------------------------------------------------
        #region ** Disposing
        public void Dispose()
        {
            // detach
            DetachScheduler(Scheduler);
        }
        #endregion

        //------------------------------------------------
        #region ** Hidden FlexGrid Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new bool IsReadOnly
        {
            get
            {
                return base.IsReadOnly;
            }
            set
            {
                base.IsReadOnly = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new AllowMerging AllowMerging
        {
            get
            {
                return base.AllowMerging;
            }
            set
            {
                base.AllowMerging = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new bool AutoGenerateColumns
        {
            get
            {
                return base.AutoGenerateColumns;
            }
            set
            {
                base.AutoGenerateColumns = value;
            }
        }
        #endregion
    }
}
