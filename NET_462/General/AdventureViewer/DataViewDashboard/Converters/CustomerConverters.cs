using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using DataViewDashboard.AdventureWorksService;

namespace DataViewDashboard.Converters
{
    public class CustomerOrdersVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Customer customer = value as Customer;
            if (customer != null)
            {
                if (customer.SalesOrderHeaders.Count > 0)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SalesPersonNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String name = value.ToString();
            if (!String.IsNullOrEmpty(name))
            {
                name = name.Substring(name.LastIndexOf('\\') + 1);
                name = name.Substring(0, name.Length - 1);
                string firstChar = name[0].ToString().ToUpper();
                name = firstChar + name.Substring(1, name.Length - 1);
                return name;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
