using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using C1.WPF;
using C1.WPF.DataGrid.Filters;

namespace DataGridSamples
{
    public partial class CustomFilters : UserControl
    {
        public CustomFilters()
        {
            InitializeComponent();

            DataContext = Data.GetProducts((product) => !string.IsNullOrEmpty(product.Element("ProductModelID").Value)
                && product.Element("Image").Value != "no_image_available_small.jpg");
        }

        private void DataGridImageColumn_FilterLoading(object sender, C1.WPF.DataGrid.DataGridFilterLoadingEventArgs e)
        {
            var filter = ((DataGridContentFilter)e.Filter).Content as DataGridToggleValuesFilter;
            filter.SetBinding(DataGridToggleValuesFilter.ItemsSourceProperty, new Binding("ItemsSource")
            {
                Source = grid,
                Converter = new ImageFilterConverter(),
            });
        }

        private void DataGridTextColumn_FilterLoading(object sender, C1.WPF.DataGrid.DataGridFilterLoadingEventArgs e)
        {
            var treeFilter = ((DataGridFilterList)((DataGridContentFilter)e.Filter).Content).Items[0] as DataGridTreeViewFilter;
            treeFilter.SetBinding(DataGridTreeViewFilter.SourceProperty, new Binding("ItemsSource")
            {
                Source = grid,
                Converter = new TreeFilterConverter(),
            });
        }

        private void DataGridNumericColumn_FilterLoading(object sender, C1.WPF.DataGrid.DataGridFilterLoadingEventArgs e)
        {
            var filter = ((DataGridFilterList)((DataGridContentFilter)e.Filter).Content).Items[0] as DataGridHistogramFilter;
            filter.SetBinding(DataGridHistogramFilter.ItemsSourceProperty, new Binding("ItemsSource")
            {
                Source = grid,
            });
        }

        private void DataGridDateTimeColumn_FilterLoading(object sender, C1.WPF.DataGrid.DataGridFilterLoadingEventArgs e)
        {
            var rangeFilter = ((DataGridFilterList)((DataGridContentFilter)e.Filter).Content).Items[0] as DataGridDateRangeFilter;
            rangeFilter.Maximum = DateTime.Today;
            rangeFilter.Minimum = DateTime.Today.Subtract(TimeSpan.FromDays(30));
        }
    }

    public class ImageFilterConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var selector = C1PropertyPathHelper.CreateSelector<Product, object>("ImageUrl");
            return (value as IEnumerable).Cast<Product>().Select(selector).Distinct();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class TreeFilterConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var treeSource = from p in (value as IEnumerable).Cast<Product>()
                             group p by p.ProductNumber.Substring(0, 2) into gr
                             orderby gr.Key
                             select new GroupElement(gr);
            return treeSource.ToList();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
