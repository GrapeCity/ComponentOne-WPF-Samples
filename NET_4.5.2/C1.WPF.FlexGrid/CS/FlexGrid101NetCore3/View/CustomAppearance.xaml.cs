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
    /// Interaction logic for CustomAppearance.xaml
    /// </summary>
    public partial class CustomAppearance : Page
    {
        public CustomAppearance()
        {
            InitializeComponent();

            PopulateEditGrid();
        }

        private void PopulateEditGrid()
        {
            // create the data
            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.MinColumnWidth = 85;

            // hide read-only "Country" column 
            var col = grid.Columns["Country"];
            col.Visible = false;

            // map countryID column so it shows country names instead of their IDs
            Dictionary<int, string> dct = new Dictionary<int, string>();
            foreach (var country in Customer.GetCountries())
            {
                dct[country.Key] = country.Value;
            }
            col = grid.Columns["CountryID"];
            col.ValueConverter = new ColumnValueConverter(dct);
            col.HorizontalAlignment = HorizontalAlignment.Left;
            col.Width = new GridLength(120);

            // provide auto-complete lists for first and last name columns
            col = grid.Columns["FirstName"];
            col.ValueConverter = new ColumnValueConverter(Customer.GetFirstNames(), false);
            col = grid.Columns["LastName"];
            col.ValueConverter = new ColumnValueConverter(Customer.GetLastNames(), false);
            
            // make read-only columns look disabled
            var readOnlyBrush = new SolidColorBrush(Color.FromArgb(0xe0, 0xe0, 0xe0, 0xe0));
            foreach (var c in grid.Columns)
            {
                if (c.PropertyInfo != null && !c.PropertyInfo.CanWrite)
                {
                    c.Background = readOnlyBrush;
                }
            }

            grid.Columns.Move(grid.Columns["Name"].Index, 1);
        }
    }
}
