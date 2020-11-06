using System;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;

namespace GroupPanel
{
    /// <summary>
    /// Group converter for <see cref="DateTime"/> values.
    /// </summary>
    /// <remarks>
    /// <para>This converter converts dates into one or more categories:
    /// "This week", "This month", "This year", "Last year", and "Before last year".</para>
    /// <para>The converter returns a list since values may fall into more than one 
    /// category (for example, today belongs to "this week", "this month", and also "this year").</para>
    /// </remarks>
    public class DateTimeGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var list = new List<string>();
            var date = (DateTime)value;
            var today = DateTime.Today;
            if (today.Subtract(date).TotalDays <= 7) list.Add("This week");
            if (date.Year == today.Year && date.Month == today.Month) list.Add("This month");
            if (date.Year == today.Year)
            {
                list.Add("This year");
            }
            else if (date.Year == today.Year - 1)
            {
                list.Add("Last year");
            }
            else
            {
                list.Add("Before last year");
            }
            return list;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
