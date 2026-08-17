using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Data.Objects.DataClasses;
using System.Windows;
using System.Windows.Media;
using DataViewDashboard.AdventureWorksService;
using System.Data.Services.Client;

namespace DataViewDashboard.Converters
{
    public class HasOrdersVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataServiceCollection<SalesOrderDetail> details = value as DataServiceCollection<SalesOrderDetail>;
            if(details != null && details.Count > 0)
                return Visibility.Visible;

            DataServiceCollection<SalesOrderHeader> orders = value as DataServiceCollection<SalesOrderHeader>;
            if (orders != null && orders.Count > 0)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value.ToString();
            if (status.Equals("5"))
            {
                return new SolidColorBrush(Colors.LimeGreen);
            }
            else
            {
                return new SolidColorBrush(Colors.Maroon);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class StatusLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value.ToString();
            if (status.Equals("5"))
            {
                return "Shipped";
            }
            else
            {
                return "Not Shipped";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OrderQuantityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataServiceCollection<SalesOrderDetail> details = value as DataServiceCollection<SalesOrderDetail>;
            int q = 0;
            if (details != null)
            {
                foreach (SalesOrderDetail order in details)
                {
                    q += order.OrderQty;
                }
            }
            return q;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OrderCostConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Product product = value as Product;
            if (product != null)
            {
                if (product.SalesOrderDetails != null && product.SalesOrderDetails.Count > 0)
                {
                    int q = 0;
                    foreach (SalesOrderDetail order in product.SalesOrderDetails)
                    {
                        q += order.OrderQty;
                    }
                    return String.Format("{0:c2}", q * product.StandardCost);
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OrderLineTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataServiceCollection<SalesOrderDetail> details = value as DataServiceCollection<SalesOrderDetail>;
            decimal t = 0;
            if (details != null)
            {
                foreach (SalesOrderDetail order in details)
                {
                    t += order.LineTotal;
                }
            }
            return String.Format("{0:c2}", t);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
