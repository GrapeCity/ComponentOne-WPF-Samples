using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

using C1.Chart;
using C1.WPF.Chart;

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for AxisGrouping.xaml
    /// </summary>
    public partial class DateTimeAxisGrouping : UserControl
    {
        Random rnd = new Random();
        List<string> _groupSeparator;
        List<string> _expansionOptions;
        List<string> _majorUnitOptions;

        public DateTimeAxisGrouping()
        {
            InitializeComponent();

            // Instantiate and assign the AxisGroupProvider,
            //and set the TimeUnits for each desired level.
            // To use the built-in provider:
            //   DateTimeGroupProvider mtgp = new DateTimeGroupProvider(axis);

            // For the purposes of the sample, use the included provider.
            Axis axis = flexChart.AxisX;
            MyDateTimeGroupProvider mtgp = new MyDateTimeGroupProvider(axis);
            axis.GroupProvider = mtgp;

            // Explicitly set Groups by TimeUnit
            //mtgp.GroupTypes.Add(TimeUnits.Month);
            //mtgp.GroupTypes.Add(TimeUnits.Quarter);
            //mtgp.GroupTypes.Add(TimeUnits.Year);

            // Non-default formatting is available.
            //mtgp.GroupFormats.Add(TimeUnits.Quarter, "Q{0} / {1}");

            // can force a label for the minimum and maximum limits on the axis.
            //axis.LabelMin = axis.LabelMax = true;

            // can provide and set MajorUnit in terms of TimeUnits.
            // below the MajorUnit is set to 2 months.
            //axis.TimeUnit = TimeUnits.Month;
            //axis.MajorUnit = 2;
        }

        public List<string> GroupSeparator
        {
            get
            {
                if (_groupSeparator == null)
                {
                    _groupSeparator = Enum.GetNames(typeof(AxisGroupSeparator)).ToList();
                }

                return _groupSeparator;
            }
        }

        public List<string> ExpansionOptions
        {
            get
            {
                if (_expansionOptions == null)
                {
                    _expansionOptions = (Enum.GetValues(typeof(GroupVisibilityOptions)) as GroupVisibilityOptions[])
                        .OrderBy(g => (int)g)
                        .Select(p => p.ToString().Replace("__",", ").Replace("_"," ") + " = " +((int)p).ToString()).ToList();
                }
                return _expansionOptions;
            }
        }

        public List<string> MajorUnitOptions
        {
            get
            {
                if (_majorUnitOptions == null)
                {
                    _majorUnitOptions = new List<string>()
                    { 
                        "Default",
                        "1 Week", "2 Weeks",
                        "1 Month", "2 Months", "3 Months"
                    };
                }
                return _majorUnitOptions;
            }
        }

        public List<Quote> Data
        {
            get { return CreateData(); }
        }

        List<Quote> CreateData()
        {
            List<Quote> list = new List<Quote>();
            var dt = DateTime.Today;
            var price = 1000;
            for (var i = 0; i < 1020; i++)
            {
                price += rnd.Next(10) % 2 == 0 ? rnd.Next(50) : -rnd.Next(50);
                list.Add(new Quote() { Time = dt.AddDays(i), Price = price });
            }

            return list;
        }

        private void cbMajorUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flexChart == null) return;

            ComboBox cb = sender as ComboBox;
            int idx = cb.SelectedIndex;
            Axis ax = flexChart.AxisX;

            switch (idx)
            {
                case 0:
                    ax.MajorUnit = double.NaN;
                    ax.TimeUnit = TimeUnits.Day;
                    break;
                case 1:
                    ax.MajorUnit = 1;
                    ax.TimeUnit = TimeUnits.Week;
                    break;
                case 2:
                    ax.MajorUnit = 2;
                    ax.TimeUnit = TimeUnits.Week;
                    break;
                case 3:
                    ax.MajorUnit = 1;
                    ax.TimeUnit = TimeUnits.Month;
                    break;
                case 4:
                    ax.MajorUnit = 2;
                    ax.TimeUnit = TimeUnits.Month;
                    break;
                case 5:
                    ax.MajorUnit = 3;
                    ax.TimeUnit = TimeUnits.Month;
                    break;
            }
            MyDateTimeGroupProvider mtgp = flexChart.AxisX.GroupProvider as MyDateTimeGroupProvider;
            if (mtgp != null) mtgp.GroupTypes.Clear(); // reset automatic calculation.
        }

        public class Quote
        {
            public DateTime Time { get; set; }
            public double Price { get; set; }
        }

        /// <summary>
        /// Provides basic groups for DateTime values that extend beyond one month.  Although
        /// default groups are provided, explicit selection of the groups and format are possible.
        /// </summary>
        public class MyDateTimeGroupProvider : IAxisGroupProvider
        {
            ObservableCollection<TimeUnits> _groupTypes = new ObservableCollection<TimeUnits>();
            Dictionary<TimeUnits, string> _groupFormats = new Dictionary<TimeUnits, string>();
            Axis _groupAxis = null;

            /// <summary>
            /// This contstructor allows automatic generation of groups based upon the axis range.
            /// </summary>
            public MyDateTimeGroupProvider() { }

            /// <summary>
            /// This constructor allows automatic generation of groups based upon the axis range
            /// and the axis MajorUnit value.
            /// </summary>
            /// <param name="axis"></param>
            public MyDateTimeGroupProvider(Axis axis)
            {
                _groupAxis = axis;
            }

            /// <summary>
            /// As the first method called by FlexChart, this method provides the number of group
            /// levels provided by the class.
            /// </summary>
            /// <param name="range">
            /// Specifies the full range of the axis data.
            /// </param>
            /// <returns>Specifies number of group levels.</returns>
            public int GetLevels(IRange range)
            {
                // Base default groups on the specified range.
                // Require group is larger than the MajorUnit, has more than one member
                // and does not have too many members that they do not fit along the axis.
                if (_groupTypes.Count == 0)
                {
                    double majorUnitDays = (range.Max - range.Min) / 12;    // approximate if MajorUnit = NaN

                    // if there are MajorUnit values, then request groupings with larger intervals
                    if (_groupAxis != null && !double.IsNaN(_groupAxis.MajorUnit)) majorUnitDays =
                        (new double[] { 1, 7, 30, 90, 365 })[(int)_groupAxis.TimeUnit] * _groupAxis.MajorUnit;

                    List<DateTime> dates = new List<DateTime>();
                    List<string> labels = new List<string>();

                    DateTime start = DateTime.FromOADate(range.Min);
                    DateTime end = DateTime.FromOADate(range.Max);

                    // only default to most commonly desired groups.
                    TimeUnits[] tu = { TimeUnits.Month, TimeUnits.Quarter, TimeUnits.Year };
                    double[] daysPerGroupUnit = { 30, 90, 365 };

                    for (int i = 0; i < tu.Length; i++)
                    {
                        if (majorUnitDays < daysPerGroupUnit[i])
                        {
                            CreateTimeValues(tu[i], start, end, 1, dates, labels);
                            if (dates.Count > 1 && dates.Count < 15) _groupTypes.Add(tu[i]);
                            dates.Clear(); labels.Clear();
                        }
                    }
                }
                return _groupTypes.Count;
            }

            /// <summary>
            /// Gets the collection of group specifiers using the TimeUnits enumeration.  The index of each
            /// specifier indicates (level-1).  Specifiers modified using the collection Add, Insert and Remove
            /// methods of the collection.  If no values are specified, values are automatically selected
            /// based on the range.
            /// </summary>
            public ObservableCollection<TimeUnits> GroupTypes
            {
                get { return _groupTypes; }
            }

            /// <summary>
            /// Get a dictionary of formats keyed by the TimeUnits enum of each group.  Each value specifies
            /// the string.Format() of the numeric value followed by the year, with the exception of TimeUnits.Month
            /// for which the format specifies either all "M" characters (for the month name) or the numeric
            /// value of the month and year. Note if {1} is not included in the format, the year is not included.
            /// 
            /// Default formats are Day="{0}", Week="Week {0}, Month="MMM", Quarter="Q{0}", "Year="{0}".
            /// </summary>
            public Dictionary<TimeUnits, string> GroupFormats
            {
                get { return _groupFormats; }
            }

            /// <summary>
            /// Returns a list of IRange values for the level specified the by the appropriate
            /// entry in the GroupTypes collection.
            /// </summary>
            /// <param name="range">
            /// Specifies the full range of the axis.
            /// </param>
            /// <param name="level">
            /// Specifies the level of IRange values for the level specified by the appropriate
            /// entry in the GroupTypes collection.
            /// </param>
            /// <returns></returns>
            public IList<IRange> GetRanges(IRange range, int level)
            {
                if (level - 1 < 0 || level - 1 > _groupTypes.Count)
                    return null;        // invalid group request

                var timeRange = range as TimeRange;
                if (timeRange == null)
                    return null;

                var min = timeRange.TimeMin;
                var max = timeRange.TimeMax;
                var span = max - min;

                if (span.TotalDays < 1)
                    return null;

                List<DateTime> dates = new List<DateTime>();
                List<string> labels = new List<string>();
                TimeUnits mtu = _groupTypes[level - 1];
                CreateTimeValues(mtu, min, max, 1, dates, labels);

                List<IRange> ranges = new List<IRange>();
                for (int d = 1; d < dates.Count; d++)
                {
                    TimeRange tr = new TimeRange(labels[d - 1], dates[d - 1], dates[d]);
                    ranges.Add(tr);
                }
                return ranges;
            }

            private void CreateTimeValues(TimeUnits mtu, DateTime startDate, DateTime endDate, int delta, List<DateTime> dates, List<string> labels)
            {
                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
                string fmt = _groupFormats.ContainsKey(mtu) ? _groupFormats[mtu] : null;
                string label = null;

                DateTime currentDate = GetTimeUnit(mtu, startDate, 0, fmt, out label);
                while (currentDate <= endDate)
                {
                    dates.Add(currentDate);
                    labels.Add(label);
                    currentDate = GetTimeUnit(mtu, currentDate, delta, fmt, out label);
                }

                if (currentDate > endDate)
                {
                    dates.Add(endDate);
                    labels.Add(label);
                }
            }

            private DateTime GetTimeUnit(TimeUnits mtu, DateTime dt, int delta, string fmt, out string label)
            {
                DateTime dtn = new DateTime(dt.Year, dt.Month, dt.Day);
                label = null;
                int unit = 0;

                switch (mtu)
                {
                    case TimeUnits.Day:
                        dtn = dtn.AddDays(delta);
                        label = fmt == null ? dt.Day.ToString() : string.Format(fmt, dtn.Day, dtn.Year);
                        break;

                    case TimeUnits.Month:
                        // first day of the month
                        dtn = dtn.AddDays(1 - dtn.Day).AddMonths(delta);
                        if (fmt == null)
                            label = dtn.ToString("MMM");
                        else if (string.IsNullOrEmpty(fmt.Replace("M", "")))
                            label = dtn.ToString(fmt);
                        else
                            label = string.Format(fmt, dtn.Month, dtn.Year);
                        break;

                    case TimeUnits.Quarter:
                        // first day of the quarter
                        unit = (dtn.Month + 2) / 3;
                        dtn = dtn.AddDays(1 - dtn.DayOfYear).AddMonths(((unit - 1 + delta) * 3));
                        unit = (dtn.Month + 2) / 3;
                        label = (fmt == null) ? "Q" + unit.ToString() : string.Format(fmt, unit, dtn.Year);
                        break;

                    case TimeUnits.Week:
                        // first day of the week
                        unit = (dtn.DayOfYear + 6) / 7;
                        dtn = dtn.AddDays(1 - dtn.DayOfYear).AddDays((unit - 1 + delta) * 7);
                        unit = (dtn.DayOfYear + 6) / 7;
                        label = (fmt == null) ? "Week" + unit.ToString() : string.Format(fmt, unit, dtn.Year);
                        break;

                    case TimeUnits.Year:
                        // first day of the year
                        dtn = dtn.AddDays(1 - dtn.DayOfYear).AddYears(delta);
                        label = (fmt == null) ? dtn.Year.ToString() : string.Format(fmt, dtn.Year);
                        break;
                }
                return dtn;
            }
        }
    }

    public enum GroupVisibilityOptions
    {
        Expand_from_axis__Expanded = -2,
        Expand_from_axis__Collapsed = -1,
        No_Interaction = 0,
        Expand_toward_axis__Expanded = 1,
        Expand_toward_axis__Collapsed = 2
    }

    public class IntToGroupVisibilityLevelConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GroupVisibilityOptions gvp = (GroupVisibilityOptions)value;
            return gvp.ToString().Replace("__", ", ").Replace("_", " ") + " = " + (int)gvp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string svalue = value as string;
            return int.Parse(svalue.Substring(svalue.Length - 2));
        }
    }
}
