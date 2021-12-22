using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using FlexGridExplorer.Resources;

namespace FlexGridExplorer
{
    public partial class CustomFilters : UserControl
    {
        private List<CountInStore> _data;

        public CustomFilters()
        {
            InitializeComponent();
            Tag = AppResources.CustomFiltersDescription;
            _data = DataProvider.GetCarsInStores().ToList();
            grid.Columns["Store.ID"].DataMap = new C1.WPF.Grid.GridDataMap { ItemsSource = DataProvider.GetStores(), DisplayMemberPath = "City", SelectedValuePath = "ID" };
            grid.ItemsSource = _data;
            grid.Columns["Color"].ValueConverter = new CustomColorConverter();
        }

        private void OnStoreFilterLoading(object sender, C1.WPF.Grid.GridColumnFilterLoadingEventArgs e)
        {
            e.DataFilter.Filters.Clear();
            var mapFilter = new MapFilter() { PropertyName = "Store.ID" };
            mapFilter.SetStores(DataProvider.GetStores());
            e.DataFilter.Filters.Add(mapFilter);
            e.ShowApplyButton = true;
            e.ShowClearButton = true;
        }

        private void OnModelFilterLoading(object sender, C1.WPF.Grid.GridColumnFilterLoadingEventArgs e)
        {
            e.DataFilter.Filters.Clear();
            var modelFilter = new ModelFilter() { PropertyName = "Car.Model" };
            modelFilter.SetTagList(_data.GroupBy(x => x.Car).Select(g => g.Key));
            e.DataFilter.Filters.Add(modelFilter);
            e.ShowApplyButton = true;
            e.ShowClearButton = true;
        }

        private void OnPriceFilterLoading(object sender, C1.WPF.Grid.GridColumnFilterLoadingEventArgs e)
        {
            e.DataFilter.Filters.Clear();
            var priceFilter = new PriceFilter() { PropertyName = "Car.Price" };
            var intervals = new List<PriceInterval>
            {
                new PriceInterval { MaxPrice = 20000 },
                new PriceInterval { MinPrice = 20000, MaxPrice = 40000 },
                new PriceInterval { MinPrice = 40000, MaxPrice = 70000 },
                new PriceInterval { MinPrice = 70000, MaxPrice = 100000 },
                new PriceInterval { MinPrice = 100000 }
            };
            priceFilter.SetPriceIntervals(intervals);
            e.DataFilter.Filters.Add(priceFilter);
        }

        private void OnTransmissionFilterLoading(object sender, C1.WPF.Grid.GridColumnFilterLoadingEventArgs e)
        {
            e.DataFilter.Filters.Clear();
            var transmissionFilter = new TransmissionFilter() { PropertyName = "Car.TransmissAutomatic" };
            e.DataFilter.Filters.Add(transmissionFilter);
        }

        private void OnColorFilterLoading(object sender, C1.WPF.Grid.GridColumnFilterLoadingEventArgs e)
        {
            e.DataFilter.Filters.Clear();
            var colorFilter = new ColorFilter() { PropertyName = "Color" };
            colorFilter.SetColors(DataProvider.Colors.Select(x => (Color)ColorConverter.ConvertFromString(x)).ToList());
            e.DataFilter.Filters.Add(colorFilter);
            e.ShowClearButton = true;
        }

        private class CustomColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
            {
                return Convert(value, targetType);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return Convert(value, targetType);
            }

            private object Convert(object value, Type targetType)
            {
                try
                {
                    if (targetType == typeof(Color))
                    {
                        if (value is string)
                        {
                            return (Color)ColorConverter.ConvertFromString(value as string);
                        }
                        return Colors.Transparent;
                    }

                    return null;
                }
                catch (Exception e)
                {
                    return Colors.Transparent;
                }
            }
        }
    }
}
