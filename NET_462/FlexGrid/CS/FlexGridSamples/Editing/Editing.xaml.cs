using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using C1.WPF.FlexGrid;

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for Editing.xaml
    /// </summary>
    public partial class Editing : UserControl
    {
        public Editing()
        {
            InitializeComponent();
            PopulateEditGrid();
        }

        void PopulateEditGrid()
        {
            // create the data
            var data = Customer.GetCustomerList(100);
            var view = new ListCollectionView(data);
            _flexEdit.ItemsSource = view;

            // hide read-only "Country" column 
            var col = _flexEdit.Columns["Country"];
            col.Visible = false;

            // map countryID column so it shows country names instead of their IDs
            Dictionary<int, string> dct = new Dictionary<int, string>();
            foreach (var country in Customer.GetCountries())
            {
                dct[dct.Count] = country;
            }
            col = _flexEdit.Columns["CountryID"];
            col.ValueConverter = new ColumnValueConverter(dct);
            col.HorizontalAlignment = HorizontalAlignment.Left;
            col.Width = new GridLength(120);

            // provide auto-complete lists for first and last name columns
            col = _flexEdit.Columns["First"];
            col.ValueConverter = new ColumnValueConverter(Customer.GetFirstNames(), false);
            col = _flexEdit.Columns["Last"];
            col.ValueConverter = new ColumnValueConverter(Customer.GetLastNames(), false);

            // make read-only columns look disabled
            var readOnlyBrush = new SolidColorBrush(Color.FromArgb(0xe0, 0xe0, 0xe0, 0xe0));
            foreach (var c in _flexEdit.Columns)
            {
                if (c.PropertyInfo != null && !c.PropertyInfo.CanWrite)
                {
                    c.Background = readOnlyBrush;
                }
            }
        }
    }
}
