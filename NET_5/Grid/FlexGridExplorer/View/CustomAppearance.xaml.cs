using C1.DataCollection;
using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for CustomAppearance.xaml
    /// </summary>
    public partial class CustomAppearance : UserControl
    {
        public CustomAppearance()
        {
            InitializeComponent();
            Tag = AppResources.CustomAppearanceDescription;

            var styles = Resources.Keys.Cast<string>().OrderBy(s => s).ToList();
            Themes.ItemsSource = styles;
            Themes.SelectedIndex = styles.IndexOf("Material");

            PopulateEditGrid();
        }

        private async void PopulateEditGrid()
        {
            // create the data
            var data = new C1DataCollection<Customer>(Customer.GetCustomerList(100));
            await data.SortAsync("Name");
            grid.ItemsSource = data;
            grid.MinColumnWidth = 85;

            //// hide read-only "Country" column 
            //var col = grid.Columns["Country"];
            //col.IsVisible = false;
            GridColumn col;

            //// map countryID column so it shows country names instead of their IDs
            //Dictionary<int, string> dct = new Dictionary<int, string>();
            //foreach (var country in Customer.GetCountries())
            //{
            //    dct[country.Key] = country.Value;
            //}
            //col = grid.Columns["CountryId"];
            //col.DataMap = new GridDataMap { ItemsSource = dct, SelectedValuePath = "Key", DisplayMemberPath = "Value" };
            //col.HorizontalAlignment = HorizontalAlignment.Left;
            //col.Width = new GridLength(120);

            // provide auto-complete lists for first and last name columns
            col = grid.Columns["FirstName"];
            col.DataMap = new GridDataMap { ItemsSource = Customer.GetFirstNames() };
            col = grid.Columns["LastName"];
            col.DataMap = new GridDataMap { ItemsSource = Customer.GetLastNames() };

            //grid.Columns.Move(grid.Columns["Name"].Index, 1);
        }

        private void OnThemeChanged(object sender, SelectionChangedEventArgs e)
        {
            grid.Style = Resources[Themes.SelectedValue as string] as Style;
        }
    }
}
