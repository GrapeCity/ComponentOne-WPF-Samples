using C1.WPF;
using C1.WPF.FlexGrid;

using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;


namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for CustomSortIcon.xaml
    /// </summary>
    public partial class CustomSortIcon : Page
    {
        public CustomSortIcon()
        {
            InitializeComponent();

            grid.ItemsSource = Customer.GetCustomerList(100);
            grid.MinColumnWidth = 85;
            grid.AllowDragging = C1.WPF.FlexGrid.AllowDragging.Both;
            grid.AllowResizing = C1.WPF.FlexGrid.AllowResizing.Both;
            grid.CellFactory = new MyCellFactory();

            sortIconType.Items.Add("Arrow");
            sortIconType.Items.Add("Chevron");
            sortIconType.Items.Add("Triangle");
            sortIconType.SelectedIndex = 0;

            foreach (Column c in grid.Columns)
            {
                if (c.ColumnName == "Country" || c.ColumnName == "Name" || c.ColumnName == "OrderAverage")
                {
                    //Hide these columns.
                    c.Visible = false;
                }
                else if (c.ColumnName == "Id")
                {
                    c.IsReadOnly = true;
                }
                else if (c.ColumnName == "CountryId")
                {
                    //Sets the DataMap to the country column so a picker is used to select the country.
                    Dictionary<int, string> dct = new Dictionary<int, string>();
                    foreach (var country in Customer.GetCountries())
                    {
                        dct[dct.Count] = country.Value;
                    }
                    c.ValueConverter = new ColumnValueConverter(dct);
                    c.Header = "Country";
                    c.HorizontalAlignment = HorizontalAlignment.Left;
                    c.Width = new GridLength(120);
                }
                else if (c.ColumnName == "OrderTotal" || c.ColumnName == "OrderAverage")
                {
                    //Sets currency format the these columns
                    c.Format = "C";
                }
                else if (c.ColumnName == "Address")
                {
                    c.HeaderTextWrapping = true;
                }
            }
        }

        private void sortIconType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (sortIconType.SelectedIndex)
            {
                case 0:
                    grid.SortAscendingIconTemplate = C1IconTemplate.ArrowUp;
                    grid.SortDescendingIconTemplate = C1IconTemplate.ArrowDown;
                    break;
                case 1:
                    grid.SortAscendingIconTemplate = C1IconTemplate.ChevronUp;
                    grid.SortDescendingIconTemplate = C1IconTemplate.ChevronDown;
                    break;
                case 2:
                    grid.SortAscendingIconTemplate = C1IconTemplate.TriangleNorth;
                    grid.SortDescendingIconTemplate = C1IconTemplate.TriangleSouth;
                    break;
            }
            grid.ColumnHeaders.Invalidate();
        }
    }
}
