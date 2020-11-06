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
    /// Interaction logic for ColumnDefinitions.xaml
    /// </summary>
    public partial class ColumnDefinitions : Page
    {
        public ColumnDefinitions()
        {
            InitializeComponent();

            var data = Customer.GetCustomerList(1000);
            grid.ItemsSource = data;
            grid.Columns.RemoveAt(1);
            Dictionary<int, string> dct = new Dictionary<int, string>();
            foreach (var country in Customer.GetCountries())
            {
                dct[dct.Count] = country.Value;
            }
            grid.Columns["CountryID"].ValueConverter = new ColumnValueConverter(dct);
            grid.MinColumnWidth = 85;
            grid.CellFactory = new MyCellFactory();
        }
    }
}
