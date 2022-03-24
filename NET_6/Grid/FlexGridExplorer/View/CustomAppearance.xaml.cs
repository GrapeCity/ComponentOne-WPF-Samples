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

            var styles = Resources.MergedDictionaries[0].Keys.Cast<string>().OrderBy(s => s).ToList();
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
            grid.Columns["FirstName"].DataMap = new GridDataMap { ItemsSource = Customer.GetFirstNames() };
            grid.Columns["LastName"].DataMap = new GridDataMap { ItemsSource = Customer.GetLastNames() };
            grid.Columns["City"].AllowGrouping = true;
        }

        private void OnThemeChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
                grid.Style = Resources.MergedDictionaries[0][e.AddedItems[0] as string] as Style;
        }
    }
}
