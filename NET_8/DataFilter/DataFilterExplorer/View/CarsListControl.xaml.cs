using C1.DataCollection;
using C1.WPF.DataFilter;
using DataFilterExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DataFilterExplorer
{
    /// <summary>
    /// Interaction logic for CarsListControl.xaml
    /// </summary>
    public partial class CarsListControl : UserControl
    {
        private readonly string _fileName = "Expressions.xml";
        readonly C1DataCollection<Car> _source;
        public CarsListControl()
        {
            InitializeComponent();
            Tag = AppResources.CarListDescription;
            //Get Cars list
            var carsTable = DataProvider.GetCarTable();
            _source = new C1DataCollection<Car>(DataProvider.GetCarDataCollection(carsTable));
            c1DataFilter.ItemsSource = _source;
            flexGrid.ItemsSource = _source;
            c1DataFilter.SaveFilterExpression(_fileName);
            c1DataFilter.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        private void ItemContainerGenerator_StatusChanged(object sender, System.EventArgs e)
        {
            if (c1DataFilter.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                c1DataFilter.Items[0].IsExpanded = false;
            }
        }

        private void C1DataFilter_FilterAutoGenerating(object sender, FilterAutoGeneratingEventArgs e)
        {
            switch (e.Property.Name)
            {
                case nameof(Car.Model):
                case nameof(Car.Brand):
                    break;
                case nameof(Car.Liter):
                    var literFilter = (RangeFilter)e.Filter;
                    literFilter.Maximum = _source.Max(x => ((Car)x).Liter);
                    literFilter.Minimum = _source.Min(x => ((Car)x).Liter);
                    literFilter.Increment = 0.01;
                    break;
                case nameof(Car.TransmissSpeedCount):
                    var transmissFilter = (ChecklistFilter)e.Filter;
                    transmissFilter.HeaderText = "Transmiss Speed Count";
                    transmissFilter.ShowSelectAll = false;
                    break;
                case nameof(Car.Category):
                    var categoryFilter = (ChecklistFilter)e.Filter;
                    categoryFilter.ShowSelectAll = false;
                    break;
                case nameof(Car.TransmissAutomatic):
                    var taFilter = (ChecklistFilter)e.Filter;
                    taFilter.HeaderText = "Transmiss Automatic";
                    taFilter.ItemsSource = new List<TransmissAutomatic>()
                    {
                        new() { DisplayValue = "Yes", Value = "Yes" },
                        new() { DisplayValue = "No", Value = "No" },
                        new() { DisplayValue = "Empty", Value = null }
                    };
                    taFilter.DisplayMemberPath = "DisplayValue";
                    taFilter.ValueMemberPath = "Value";
                    taFilter.ShowSelectAll = false;
                    break;
                case nameof(Car.Price):
                    var priceFilter = (RangeFilter)e.Filter;
                    priceFilter.Maximum = _source.Max(x => ((Car)x).Price); ;
                    priceFilter.Minimum = _source.Min(x => ((Car)x).Price);
                    priceFilter.Increment = 1000;
                    priceFilter.Format = "F0";
                    break;
                case nameof(Car.DateProductionLine):
                    var dateTimeRangeFilter = (DateTimeRangeFilter)e.Filter;
                    dateTimeRangeFilter.Maximum = _source.Max(x => ((Car)x).DateProductionLine);
                    dateTimeRangeFilter.Minimum = _source.Min(x => ((Car)x).DateProductionLine);
                    dateTimeRangeFilter.TimeIncrement = TimeSpan.FromMinutes(1);
                    break;
                case nameof(Car.PresentationDate):
                    var dateTimeOffsetRangeFilter = (DateTimeOffsetRangeFilter)e.Filter;
                    dateTimeOffsetRangeFilter.Maximum = _source.Max(x => ((Car)x).PresentationDate);
                    dateTimeOffsetRangeFilter.Minimum = _source.Min(x => ((Car)x).PresentationDate);
                    dateTimeOffsetRangeFilter.TimeIncrement = TimeSpan.FromMinutes(1);
                    break;
                case nameof(Car.ManufactureDate):
                    var dateOnlyRangeFilter = (DateOnlyRangeFilter)e.Filter;
                    dateOnlyRangeFilter.Maximum = _source.Max(x => ((Car)x).ManufactureDate);
                    dateOnlyRangeFilter.Minimum = _source.Min(x => ((Car)x).ManufactureDate);
                    break;
                case nameof(Car.FuelConsumption):
                    var timeOnlyRangeFilter = (TimeOnlyRangeFilter)e.Filter;
                    timeOnlyRangeFilter.Maximum = _source.Max(x => ((Car)x).FuelConsumption);
                    timeOnlyRangeFilter.Minimum = _source.Min(x => ((Car)x).FuelConsumption);
                    timeOnlyRangeFilter.Format = "hh:mm";
                    timeOnlyRangeFilter.Increment = TimeSpan.FromSeconds(1);
                    break;
                case nameof(Car.Acceleration):
                    var timeSpanRangeFilter = (TimeSpanRangeFilter)e.Filter;
                    timeSpanRangeFilter.Maximum = _source.Max(x => ((Car)x).Acceleration);
                    timeSpanRangeFilter.Minimum = _source.Min(x => ((Car)x).Acceleration);
                    timeSpanRangeFilter.Increment = TimeSpan.FromSeconds(1);
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
            e.Filter.ToolTip = $"Filter for underlie {typeof(Car).GetProperty(e.Filter.PropertyName).PropertyType.Name} data type";
        }

        private void CbAutoApply_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (c1DataFilter != null)
            {
                c1DataFilter.AutoApply = cbAutoApply.IsChecked == true;
            }
        }

        private void BtnSaveFilter_Click(object sender, RoutedEventArgs e)
        {
            c1DataFilter.SaveFilterExpression(_fileName);
        }

        private async void BtnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            c1DataFilter.LoadFilterExpression(_fileName);
            await c1DataFilter.ApplyFilterAsync();
        }

        private async void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            await c1DataFilter.ApplyFilterAsync();
        }

        private void OnFlexGridAutoGeneratingColumn(object sender, C1.WPF.Grid.GridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Binding == nameof(Car.DateProductionLine))
            {
                e.Column.Format = "g";
            }
            else if (e.Column.Binding == nameof(Car.PresentationDate))
            {
                e.Column.Format = "MM/dd/yy H:mm:ss zzz";
            }
            else if (e.Column.Binding == nameof(Car.FuelConsumption))
            {
                e.Column.Format = "hh:mm";
            }
        }
    }
}
