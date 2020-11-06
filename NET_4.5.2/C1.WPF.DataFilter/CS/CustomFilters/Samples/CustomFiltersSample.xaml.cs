using CustomFilters.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomFilters
{
    /// <summary>
    /// Interaction logic for CustomFiltersSample.xaml
    /// </summary>
    public partial class CustomFiltersSample : UserControl
    {
        private DataProvider _dataProvider = new DataProvider();
        private IEnumerable<CountInStore> _data;
        public CustomFiltersSample()
        {
            InitializeComponent();
            _data = _dataProvider.GetCarsInStores().ToList();
            c1DataFilter.ItemsSource = _data;
            UpdateTreeViewData(_data);

            InitFilters();
        }

        private void InitFilters()
        {
            // Map filter
            var mf = new MapFilter()
            {
                HeaderText = "Store",
                PropertyName = "Store.ID"
            };
            mf.SetStores(_dataProvider.GetStores());
            c1DataFilter.Filters.Add(mf);

            //Model filter
            var mdf = new ModelFilter()
            {
                HeaderText = "Model",
                PropertyName = "Car.Model"
            };
            mdf.SetTagList(_data.GroupBy(x => x.Car).Select(g => g.Key));
            c1DataFilter.Filters.Add(mdf);

            // Price filter
            var pf = new PriceFilter()
            {
                HeaderText = "Price",
                PropertyName = "Car.Price",
                ShowSelectAll = false,
                ShowSearchBox = false
            };
            var intervals = new List<PriceInterval>
            {
                new PriceInterval { MaxPrice = 20000 },
                new PriceInterval { MinPrice = 20000, MaxPrice = 40000 },
                new PriceInterval { MinPrice = 40000, MaxPrice = 70000 },
                new PriceInterval { MinPrice = 70000, MaxPrice = 100000 },
                new PriceInterval { MinPrice = 100000 }
            };
            pf.SetPriceIntervals(intervals);
            c1DataFilter.Filters.Add(pf);

            // Transmiss filter
            var tf = new TransmissionFilter
            {
                PropertyName = "Car.TransmissAutomatic",
                HeaderText = "Automatic transmission"
            };
            c1DataFilter.Filters.Add(tf);

            // Color filter
            var cf = new ColorFilter()
            {
                HeaderText = "Color",
                PropertyName = "Color"
            };
            cf.SetColors(_dataProvider.Colors.Select(x => new SolidColorBrush((Color)ColorConverter.ConvertFromString(x))));
            c1DataFilter.Filters.Add(cf);

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
