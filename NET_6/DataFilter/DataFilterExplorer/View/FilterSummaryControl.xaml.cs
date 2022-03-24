using C1.DataCollection;
using C1.WPF.DataFilter;
using DataFilterExplorer.Resources;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataFilterExplorer
{
    /// <summary>
    /// Interaction logic for FilterSummarySample.xaml
    /// </summary>
    public partial class FilterSummarySample : UserControl
    {
        C1DataCollection<Car> _data;

        public FilterSummarySample()
        {
            InitializeComponent();
            Tag = AppResources.FilterSummaryDescription;
            _data = new C1DataCollection<Car>(DataProvider.GetCars());
            flexGrid.ItemsSource = _data;
            c1DataFilter1.ItemsSource = _data;
        }

        private void C1DataFilter1_FilterAutoGenerating(object sender, FilterAutoGeneratingEventArgs e)
        {
            foreach (Filter f in c1DataFilter1.Filters)
            {
                if (f.PropertyName == "Brand")
                {
                    var brandFilter = f as ChecklistFilter;
                    brandFilter.FilterSummary.Label = "Models:";
                    brandFilter.FilterSummary.PropertyName = "Brand";
                    brandFilter.FilterSummary.AggregateType = AggregateType.Count;
                }

                if (f.PropertyName == "Model")
                {
                    var modelFilter = f as ChecklistFilter;
                    modelFilter.FilterSummary.AggregateType = AggregateType.Max;
                    modelFilter.FilterSummary.CustomFormat = "C0";
                    modelFilter.FilterSummary.Label = "Max price: ";
                    modelFilter.FilterSummary.PropertyName = "Price";
                }

                if (f.PropertyName == "Price")
                {
                    var modelFilter = f as RangeFilter;
                    modelFilter.Maximum = _data.Max(x => (x as Car).Price);
                    modelFilter.Minimum = _data.Min(x => (x as Car).Price);
                    modelFilter.Format = "F0";
                }
            }
        }

        private void CbAutoApply_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (c1DataFilter1 != null)
            {
                c1DataFilter1.AutoApply = cbAutoApply.IsChecked == true;
            }
        }

        private async void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            await c1DataFilter1.ApplyFilterAsync();
        }
    }
}
