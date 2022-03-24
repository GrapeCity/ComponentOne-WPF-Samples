using C1.WPF.FlexGrid;
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

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for ConditionalFormatting.xaml
    /// </summary>
    public partial class ConditionalFormatting : Page
    {
        public ConditionalFormatting()
        {
            InitializeComponent();

            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            Dictionary<int, string> dct = new Dictionary<int, string>();
            foreach (var country in Customer.GetCountries())
            {
                dct[dct.Count] = country.Value;
            }
            grid.Columns["CountryID"].ValueConverter = new ColumnValueConverter(dct);

            grid.CellFactory = new MyCellFactory();
            grid.MinColumnWidth = 85;
        }
    }

    public class ForegroundConverter : System.Windows.Data.IValueConverter
    {
        SolidColorBrush _brBlack = new SolidColorBrush(Colors.Black);
        SolidColorBrush _brRed = new SolidColorBrush(Colors.Red);
        SolidColorBrush _brGreen = new SolidColorBrush(Colors.Green);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var range = (Point)parameter;
            double val = 0;
            double.TryParse(value.ToString(), out val);
            return
                val < range.X ? _brRed :
                val > range.Y ? _brGreen :
                _brBlack;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BackgroundConverter : System.Windows.Data.IValueConverter
    {
        SolidColorBrush _brBlack = new SolidColorBrush(Colors.Black);
        SolidColorBrush _brRed = new SolidColorBrush(Color.FromArgb(255, 255, 112, 112));
        SolidColorBrush _brGreen = new SolidColorBrush(Color.FromArgb(255, 142, 233, 142));

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var range = (Point)parameter;
            double val = 0;
            double.TryParse(value.ToString(), out val);
            return
                val < range.X ? _brRed :
                val >= range.Y ? _brGreen :
                _brBlack;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
