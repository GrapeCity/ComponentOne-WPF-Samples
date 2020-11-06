using System;
using System.Globalization;
using System.Windows.Data;

namespace DataGridSamples
{
    public enum NumberRange
    {
        LessThan0,
        Between0And10,
        Between10And50,
        Between50And100,
        Between100And500,
        Between500And1000,
        Between1000And5000,
        GratherThan5000
    }

    public class NumberRangeGroupConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double number = (double)value;
                if (number < 0)
                {
                    return NumberRange.LessThan0;
                }
                if (number < 10)
                {
                    return NumberRange.Between0And10;
                }
                if (number < 50)
                {
                    return NumberRange.Between10And50;
                }
                if (number < 100)
                {
                    return NumberRange.Between50And100;
                }
                if (number < 500)
                {
                    return NumberRange.Between100And500;
                }
                if (number < 1000)
                {
                    return NumberRange.Between500And1000;
                }
                if (number < 5000)
                {
                    return NumberRange.Between1000And5000;
                }
                return NumberRange.GratherThan5000;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    public class NumberRangeGroupNameConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NumberRange && targetType.Name == "String")
            {
                NumberRange range = (NumberRange)value;
                switch (range)
                {
                    case NumberRange.LessThan0:
                        return "Less Than 0";
                    case NumberRange.Between0And10:
                        return "Between 0 And 10";
                    case NumberRange.Between10And50:
                        return "Between 10 And 50";
                    case NumberRange.Between50And100:
                        return "Between 50 And 100";
                    case NumberRange.Between100And500:
                        return "Between 100 And 500";
                    case NumberRange.Between500And1000:
                        return "Between 500 And 1000";
                    case NumberRange.Between1000And5000:
                        return "Between 1000 And 5000";
                    case NumberRange.GratherThan5000:
                        return "Greater Than 5000";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
