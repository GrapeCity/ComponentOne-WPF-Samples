using DataFilterExplorer.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataFilterExplorer
{
    /// <summary>
    /// Interaction logic for CustomFiltersSample.xaml
    /// </summary>
    public partial class CustomFiltersSample : UserControl
    {
        private IEnumerable<CountInStore> _data;

        public CustomFiltersSample()
        {
            InitializeComponent();
            Tag = AppResources.CustomFiltersTag;
            _data = DataProvider.GetCarsInStores().ToList();
            c1DataFilter.ItemsSource = _data;
            UpdateTreeViewData(_data);

            InitFilters();
        }

        private void InitFilters()
        {
            // Map filter
            mf.SetStores(DataProvider.GetStores());

            //Model filter
            mdf.SetTagList(_data.GroupBy(x => x.Car).Select(g => g.Key));

            // Price filter
            var intervals = new List<PriceInterval>
            {
                new PriceInterval { MaxPrice = 20000 },
                new PriceInterval { MinPrice = 20000, MaxPrice = 40000 },
                new PriceInterval { MinPrice = 40000, MaxPrice = 70000 },
                new PriceInterval { MinPrice = 70000, MaxPrice = 100000 },
                new PriceInterval { MinPrice = 100000 }
            };
            pf.SetPriceIntervals(intervals);

            // Color filter
            cf.SetColors(DataProvider.Colors.Select(x => new SolidColorBrush((Color)ColorConverter.ConvertFromString(x))));

        }

        private void c1DataFilter_FilterChanged(object sender, System.EventArgs e)
        {
            UpdateTreeViewData(c1DataFilter.View.Cast<CountInStore>());
        }
        private void UpdateTreeViewData(IEnumerable<CountInStore> data)
        {
            treeView.ItemsSource = data.GroupBy(x => x.Car).Select(g => new CarStoreGroup { Car = g.Key, CountInStores = g.ToList() });
        }
    }
}
