using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for GettingStarted.xaml
    /// </summary>
    public partial class GettingStarted : UserControl
    {
        public GettingStarted()
        {
            InitializeComponent();
            Tag = AppResources.GettingStartedDescription;
            grid.ItemsSource = Customer.GetCustomerList(100);
            grid.MinColumnWidth = 85;
            grid.AllowDragging = GridAllowDragging.Both;
            grid.AllowResizing = GridAllowResizing.Both;

            foreach (GridColumn c in grid.Columns)
            {
                if (c.ColumnName == "Country" || c.ColumnName == "Name" || c.ColumnName == "OrderAverage")
                {
                    //Hide these columns.
                    c.IsVisible = false;
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
                    c.DataMap = new GridDataMap { ItemsSource = dct, SelectedValuePath = "Key", DisplayMemberPath = "Value" };
                    c.Header = "Country";
                    c.HorizontalAlignment = HorizontalAlignment.Left;
                    c.Width = new GridLength(120);
                }
                else if (c.ColumnName == "OrderTotal" || c.ColumnName == "OrderAverage")
                {
                    //Sets currency format the these columns
                    c.Format = "C";
                }
                //else if (c.ColumnName == "Address")
                //{
                //    c.HeaderTextWrapping = true;
                //}
            }
        }
    }
}
