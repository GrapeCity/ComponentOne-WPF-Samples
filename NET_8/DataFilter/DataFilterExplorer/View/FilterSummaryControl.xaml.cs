using C1.DataCollection;
using C1.WPF.DataFilter;
using DataFilterExplorer.Resources;
using System;
using System.Collections.Generic;
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
            _data = new C1DataCollection<Car>(DataProvider.GetCars().ToList());
            flexGrid.ItemsSource = _data;
            c1DataFilter1.ItemsSource = _data;
        }

        private void C1DataFilter1_FilterAutoGenerating(object sender, FilterAutoGeneratingEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Brand":
                    var brandFilter = (ChecklistFilter)e.Filter;
                    brandFilter.FilterSummary.Label = "Models:";
                    brandFilter.FilterSummary.PropertyName = "Brand";
                    brandFilter.FilterSummary.AggregateType = AggregateType.Count;
                    break;
                case "Model":
                    var modelFilter = (ChecklistFilter)e.Filter;
                    modelFilter.FilterSummary.AggregateType = AggregateType.Max;
                    modelFilter.FilterSummary.CustomFormat = "C0";
                    modelFilter.FilterSummary.Label = "Max price: ";
                    modelFilter.FilterSummary.PropertyName = "Price";
                    break;
                case "Price":
                    var pf = (RangeFilter)e.Filter;
                    pf.Maximum = _data.Max(x => (x as Car).Price);
                    pf.Minimum = _data.Min(x => (x as Car).Price);
                    pf.Increment = 1000;
                    pf.Format = "F0";
                    break;
                case "TransmissAutomatic":
                    var taFilter = (ChecklistFilter)e.Filter;
                    taFilter.HeaderText = "Transmiss Automatic";
                    taFilter.ItemsSource = new List<TransmissAutomatic>()
                    {
                        new TransmissAutomatic() { DisplayValue = "Yes", Value = "Yes" },
                        new TransmissAutomatic() { DisplayValue = "No", Value = "No" },
                        new TransmissAutomatic() { DisplayValue = "Empty", Value = null }
                    };
                    taFilter.DisplayMemberPath = "DisplayValue";
                    taFilter.ValueMemberPath = "Value";
                    taFilter.ShowSelectAll = false;
                    break;
                case "DateProductionLine":
                    var drf = (DateTimeRangeFilter)e.Filter;
                    drf.UpperValue = _data.Max(x => (x as Car).DateProductionLine);
                    drf.LowerValue = _data.Min(x => (x as Car).DateProductionLine);

                    drf.Maximum = drf.UpperValue.Value;
                    drf.Minimum = drf.LowerValue.Value;
                    break;
                default:
                    e.Cancel = true;
                    break;
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
