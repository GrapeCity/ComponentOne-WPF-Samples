using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.FlexGrid;
using System.ComponentModel;

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for CustomGrouping.xaml
    /// </summary>
    public partial class CustomGrouping : UserControl
    {
        CollectionViewSource cvs;
        
        public CustomGrouping()
        {
            InitializeComponent();

            // create a data source
            var list = new List<Product>();
            for (int i = 0; i < 200; i++)
            {
                list.Add(new Product());
            }

            // assign the data source to grid
            cvs = new CollectionViewSource() { Source = list };
            _flexCustomGrouping.ItemsSource = cvs.View;

            _groupPanelCustomGrouping.PropertyGroupCreated += _groupPanel_PropertyGroupCreated;

        }

        void _groupPanelCustomGrouping_Loaded(object sender, RoutedEventArgs e)
        {
            cvs.GroupDescriptions.Add(new PropertyGroupDescription("Introduced"));
        }

        /// <summary>
        /// Customize group descriptors created by the C1FlexGridGroupPanel.
        /// </summary>
        void _groupPanel_PropertyGroupCreated(object sender, PropertyGroupCreatedEventArgs e)
        {
            var pgd = e.PropertyGroupDescription;
            switch (pgd.PropertyName)
            {
                case "Introduced":
                    pgd.Converter = new DateTimeGroupConverter();
                    break;
                case "Price":
                    pgd.Converter = new AmountGroupConverter(1000);
                    break;
                case "Cost":
                    pgd.Converter = new AmountGroupConverter(300);
                    break;
            }
        }
    }

    /// <summary>
    /// Group converter for amounts.
    /// </summary>
    /// <remarks>
    /// This converter converts amounts into one of four categories:
    /// "High", "Moderate", "Low", and "Very Low". The category is calculated
    /// based on a maximum value provided to the constructor.
    /// </remarks>
    public class AmountGroupConverter : IValueConverter
    {
        double _maxValue;
        public AmountGroupConverter(double maxValue)
        {
            _maxValue = maxValue;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var pct = (double)value / _maxValue;
            if (pct < .25) return "Very Low";
            if (pct < .50) return "Low";
            if (pct < .75) return "Moderate";
            return "High";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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
