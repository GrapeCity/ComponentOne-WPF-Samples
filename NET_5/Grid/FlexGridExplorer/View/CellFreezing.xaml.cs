using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for CellFreezing.xaml
    /// </summary>
    public partial class CellFreezing : UserControl
    {
        public CellFreezing()
        {
            InitializeComponent();
            Tag = AppResources.CellFreezingDescription;
            var data = Customer.GetCustomerList(1000);
            grid.ItemsSource = data;
            Dictionary<int, string> dct = new Dictionary<int, string>();
            foreach (var country in Customer.GetCountries())
            {
                dct[dct.Count] = country.Value;
            }
            grid.Columns["CountryID"].DataMap = new GridDataMap { ItemsSource = dct, SelectedValuePath = "Key", DisplayMemberPath = "Value" };
            grid.Columns["CountryID"].AllowMerging = true;
            grid.MinColumnWidth = 85;
        }
    }
}
