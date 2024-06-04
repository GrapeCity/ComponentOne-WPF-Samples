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

namespace ExcelGrid
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            // show Excel-style marquee
            _flex.ShowMarquee = true;
            _flex.CursorBackground = null;
            _flex.GridLinesVisibility = GridLinesVisibility.All;

            // use custom cell factory to generate Excel-style row and column headers
            // (columns labeled A, B, C, etc, rows labeled 1, 2, 3, etc)
            _flex.CellFactory = new ExcelCellFactory();

            // start with blue color scheme
            ColorSchemeManager.ApplyColorScheme(_flex, ColorScheme.Blue);

            // start in bound mode
            PopulateGrid(true);

            // give grid the focus when the app loads
            Loaded += MainPage_Loaded;
        }

        // give grid the focus when the app loads
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _flex.Focus();
        }

        // pass focus to grid when user hits enter on the header TextBox
        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _flex.Focus();
            }
        }

        // assign cell content from header TextBox
        void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var sel = _flex.Selection;
                _flex[sel.Row, sel.Column] = ((TextBox)sender).Text;
            }
            catch { }
        }

        // change color scheme (blue, silver, black)
        void ComboBox_ColorSchemeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_flex != null)
            {
                var cs = (ColorScheme)((ComboBox)sender).SelectedIndex;
                ColorSchemeManager.ApplyColorScheme(_flex, cs);
                _flex.Select(0, 0);
                _flex.Focus();
            }
        }

        // change data mode (bound, unbound)
        void ComboBox_DataModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_flex != null)
            {
                PopulateGrid(((ComboBox)sender).SelectedIndex == 0);
                _flex.Select(0, 0);
                _flex.Focus();
            }
        }

        // populate the grid (bound or unbound)
        void PopulateGrid(bool bound)
        {
            if (bound)
            {
                // set item source
                _flex.Columns.Clear();
                _flex.ItemsSource = Customer.GetCustomerList(250);

                // hide read-only "Country" column 
                var col = _flex.Columns["Country"];
                col.Visible = false;

                // map countryID column so it shows country names instead of their IDs
                Dictionary<int, string> dct = new Dictionary<int, string>();
                foreach (var country in Customer.GetCountries())
                {
                    dct[dct.Count] = country;
                }
                col = _flex.Columns["CountryID"];
                col.ValueConverter = new ColumnValueConverter(dct);
                col.HorizontalAlignment = HorizontalAlignment.Left;
                col.Width = new GridLength(120);

                // provide auto-complete lists for first and last name columns
                col = _flex.Columns["First"];
                col.ValueConverter = new ColumnValueConverter(Customer.GetFirstNames(), false);
                col = _flex.Columns["Last"];
                col.ValueConverter = new ColumnValueConverter(Customer.GetLastNames(), false);

                // make read-only columns look disabled
                var readOnlyBrush = new SolidColorBrush(Color.FromArgb(0xe0, 0xe0, 0xe0, 0xe0));
                foreach (var c in _flex.Columns)
                {
                    if (c.PropertyInfo != null && !c.PropertyInfo.CanWrite)
                    {
                        c.Background = readOnlyBrush;
                    }
                }
            }
            else
            {
                // remove all rows and columns
                _flex.ItemsSource = null;
                _flex.Rows.Clear();
                _flex.Columns.Clear();

                // create 256 columns and 1500 rows (unbound)
                using (_flex.Rows.DeferNotifications())
                using (_flex.Columns.DeferNotifications())
                {
                    while (_flex.Columns.Count < 256)
                    {
                        _flex.Columns.Add(new Column());
                    }
                    while (_flex.Rows.Count < 1500)
                    {
                        _flex.Rows.Add(new Row());
                    }
                }
                var col = _flex.RowHeaders.Columns[0];
                col.Width = new GridLength(35);
                col.AllowResizing = false;
            }
        }
    }
}
