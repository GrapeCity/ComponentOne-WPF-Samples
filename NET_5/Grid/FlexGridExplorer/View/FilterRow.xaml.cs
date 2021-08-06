using FlexGridExplorer.Resources;
using System.Linq;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for FilterRow.xaml
    /// </summary>
    public partial class FilterRow : UserControl
    {
        public FilterRow()
        {
            InitializeComponent();
            Tag = AppResources.FilterRowDescription;
            var data = DataProvider.GetCarsInStores().ToList();
            grid.ItemsSource = data;
            grid.Columns["Store.ID"].DataMap = new C1.WPF.Grid.GridDataMap { ItemsSource = DataProvider.GetStores(), DisplayMemberPath = "City", SelectedValuePath = "ID" };
        }
    }
}
