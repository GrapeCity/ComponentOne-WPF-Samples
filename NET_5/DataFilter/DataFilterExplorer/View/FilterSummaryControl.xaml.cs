using C1.DataCollection;
using C1.WPF.DataFilter;
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
            Tag = "This sample demonstrates how to use the FilterSummary for the Checklist filter. Also shows how to use different aggregate expressions and custom format of filter summaries.";
            _data = new C1DataCollection<Car>(DataProvider.GetCars());
            flexGrid.ItemsSource = _data;
            c1DataFilter1.ItemsSource = _data;

            foreach (ChecklistFilter filter in c1DataFilter1.Filters.Where(f => f is ChecklistFilter))
                filter.SelectAll();
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
                    modelFilter.Digits = 0;
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
