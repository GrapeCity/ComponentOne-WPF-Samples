using System;
using System.Globalization;
using System.Windows.Data;

namespace DataGridSamples
{
    public enum OutlookDate
    {
        Future,
        Today,
        Yesterday,
        LastWeek,
        TwoWeeksAgo,
        ThreeWeeksAgo,
        FourWeeksAgo,
        LastMonth,
        Older,
        NotSpecified
    }

    public class OutlookDateGroupConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return OutlookDate.NotSpecified;
            }
            if (value is DateTime)
            {
                DateTime date = ((DateTime)value).Date;
                if (date > DateTime.Today)
                {
                    return OutlookDate.Future;
                }
                if (date == DateTime.Today)
                {
                    return OutlookDate.Today;
                }
                if (date == DateTime.Today.Subtract(TimeSpan.FromDays(1)))
                {
                    return OutlookDate.Yesterday;
                }
                if (date >= DateTime.Today.Subtract(TimeSpan.FromDays(7)))
                {
                    return OutlookDate.LastWeek;
                }
                if (date >= DateTime.Today.Subtract(TimeSpan.FromDays(14)))
                {
                    return OutlookDate.TwoWeeksAgo;
                }
                if (date >= DateTime.Today.Subtract(TimeSpan.FromDays(21)))
                {
                    return OutlookDate.ThreeWeeksAgo;
                }
                if (date >= DateTime.Today.Subtract(TimeSpan.FromDays(31)))
                {
                    return OutlookDate.FourWeeksAgo;
                }
                if (date >= DateTime.Today.Subtract(TimeSpan.FromDays(62)))
                {
                    return OutlookDate.LastMonth;
                }
                return OutlookDate.Older;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    public class OutlookDateGroupNameConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OutlookDate && targetType.Name == "String")
            {
                OutlookDate date = (OutlookDate)value;
                switch (date)
                {
                    case OutlookDate.Future:
                        return "Future";
                    case OutlookDate.Today:
                        return "Today";
                    case OutlookDate.Yesterday:
                        return "Yesterday";
                    case OutlookDate.LastWeek:
                        return "Last Week";
                    case OutlookDate.TwoWeeksAgo:
                        return "Two Weeks Ago";
                    case OutlookDate.ThreeWeeksAgo:
                        return "Three Weeks Ago";
                    case OutlookDate.FourWeeksAgo:
                        return "Four Weeks Ago";
                    case OutlookDate.LastMonth:
                        return "Last Month";
                    case OutlookDate.Older:
                        return "Older";
                    case OutlookDate.NotSpecified:
                        return "Not specified";
                    default:
                        return "";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
