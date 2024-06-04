using C1.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace C1.WPF.Schedule
{
    /// <summary>
    /// The <see cref="AppointmentField"/> class describes appointment fileds which can be used in table views.
    /// </summary>
    public class AppointmentField : INotifyPropertyChanged
    {
        //-----------------------------------------------------
        #region ** Fields
        private bool _visible;
        private ListSortDirection? _sort;
        private int _groupIndex;
        private string _caption;
        private PropertyInfo _pi; // cache PropertyInfo for C1.Schedule.Appointment property
        #endregion

        //-----------------------------------------------------
        #region ** Initializing
        // internal because only predefined fields are allowed
        internal AppointmentField(string name, Type dataType, bool editable = true, bool hidden=false)
        {
            _caption = Name = name;
            Hidden = hidden;
            DataType = dataType;
            ReadOnly = !editable;
            _visible = !hidden;
            _groupIndex = -1;
            _pi = typeof(Appointment).GetProperty(name);
        }

        internal AppointmentField(string name, Type dataType, string caption)
            :this(name, dataType)
        {
            _caption = caption;
        }
        #endregion

        //-----------------------------------------------------
        #region ** Object Model
        /// <summary>
        /// Gets field name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Determines whether field can be shown and available for sorting, filtering and grouping operations. 
        /// </summary>
        /// <remarks>If this property is true, 
        /// the <see cref="Visible"/>, <see cref="Sort"/>, <see cref="GroupIndex"/> and <see cref="Caption"/>
        /// property values will be ignored and property change notifications won't be fired.</remarks>
        public bool Hidden { get; private set; }
        /// <summary>
        /// Determines field data type.
        /// </summary>
        public Type DataType { get; private set; }
        /// <summary>
        /// Determines whether field should be editable by end-user.
        /// </summary>
        [DefaultValue(true)]
        public bool ReadOnly { get; private set; }
        /// <summary>
        /// Determines whether field should be visible to end-user. 
        /// </summary>
        [DefaultValue(true)]
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value && !Hidden)
                {
                    _visible = value;
                    OnPropertyChanged("Visible");
                }
            }
        }
        /// <summary>
        /// Determines field sort options. 
        /// </summary>
        [DefaultValue(null)]
        public ListSortDirection? Sort
        {
            get { return _sort; }
            set
            {
                if (_sort != value && !Hidden)
                {
                    _sort = value;
                    OnPropertyChanged("Sort");
                }
            }
        }
        /// <summary>
        /// Specifies the column order according to which data is grouped. Values must be unique.
        /// The default value is -1, which means no grouping by this field.
        /// </summary>
        [DefaultValue(-1)]
        public int GroupIndex
        {
            get { return _groupIndex; }
            set
            {
                if (_groupIndex != value && !Hidden)
                {
                    _groupIndex = value;
                    OnPropertyChanged("GroupIndex");
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="string"/> value to display column caption for this field.
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption != value && !Hidden)
                {
                    _caption = value;
                    OnPropertyChanged("Caption");
                }
            }
        }
        #endregion

        //-----------------------------------------------------
        #region ** Get/Set appointment property
        internal object GetPropertyValue(Appointment app)
        {
            return _pi?.GetValue(app, null);
        }

        internal void SetPropertyValue(Appointment app, object value)
        {
            _pi?.SetValue(app, value, null);
        }
        #endregion

        //-----------------------------------------------------
        #region ** PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires property change notification.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    /// <summary>
    /// The <see cref="AppointmentFields"/> dictionary contains all fileds which can be used in a view.
    /// </summary>
    public class AppointmentFields : Dictionary<string, AppointmentField>
    {
        // todo: Dispose to unsubscribe from events?

        bool _initialized = false;
        internal AppointmentFields()
        {
            // fill possible fields

            // Id - hidden, needed for data synchronization
            AddField("Id", typeof(Guid), false, true);

            // fields visible by default
            var field = AddField("Icon", typeof(string), false);
            field.Caption = " ";
            AddField("Subject", typeof(string));
            AddField("Location", typeof(string));
            AddField("Start", typeof(DateTime));
            AddField("End", typeof(DateTime));
            field = AddField("RecurrencePattern", typeof(string), false);
            field.Caption = "Recurrence Pattern";
            AddField("Categories", typeof(string), false);
            AddField("Recurrence", typeof(string), false);

            // fields not visible by default
            field = AddInvisibleField("AllDayEvent", typeof(bool));
            field.Caption = "All Day Event";
            field = AddInvisibleField("Links", typeof(string), false);
            field.Caption = "Contacts";
            AddInvisibleField("Duration", typeof(TimeSpan), false);
            AddInvisibleField("Label", typeof(string), false); // todo: editable?
            //RecurrenceRangeStart and RecurrenceRangeEnd need special handling 
            // as implemented in RecurrencePattern class, not in Appointment.
            //Don't implement this now.
            //field = AddInvisibleField("RecurrenceRangeStart", typeof(DateTime), false);
            //field.Caption = "Range Start";
            //field = AddInvisibleField("RecurrenceRangeEnd", typeof(DateTime), false);
            //field.Caption = "Range End";
            AddInvisibleField("ReminderSet", typeof(bool), true);
            AddInvisibleField("Resources", typeof(string), false);
            AddInvisibleField("Private", typeof(bool), true);
            field = AddInvisibleField("BusyStatus", typeof(string), false); // todo: editable?
            field.Caption = "Show Time As";
            _initialized = true;
        }

        internal AppointmentField AddField(string name, Type dataType, bool editable=true, bool hidden=false)
        {
            var field = new AppointmentField(name, dataType, editable, hidden);
            field.PropertyChanged += Field_PropertyChanged;
            base.Add(name, field);
            return field;
        }

        internal AppointmentField AddInvisibleField(string name, Type dataType, bool editable = true, bool hidden = false)
        {
            var field = new AppointmentField(name, dataType, editable, hidden);
            field.Visible = false;
            field.PropertyChanged += Field_PropertyChanged;
            base.Add(name, field);
            return field;
        }

        private void Field_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_initialized)
            {
                OnFieldChanged();
            }
        }

        /// <summary>
        /// Fires when one of fields changes.
        /// </summary>
        public event EventHandler FieldChanged;
        protected virtual void OnFieldChanged()
        {
            FieldChanged?.Invoke(this, null);
        }

        /// <summary>
        /// Returns dictionary of fields which should be used to display in view. This includes Id and all visible fields.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AppointmentField> GetFieldsForView()
        {
            var result = new Dictionary<string, AppointmentField>();
            result.Add("Id", this["Id"]);
            foreach(KeyValuePair<string, AppointmentField> pair in this)
            {
                if ( pair.Value.Visible)
                {
                    result.Add(pair.Key, pair.Value);
                }
            }
            return result;
        }
    }
}
