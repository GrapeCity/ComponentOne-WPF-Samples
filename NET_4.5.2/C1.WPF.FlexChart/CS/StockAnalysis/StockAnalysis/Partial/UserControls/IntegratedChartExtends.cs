using C1.WPF.Chart.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using StockAnalysis.Partial.CustomControls.IndicatorSeries;

namespace StockAnalysis.Partial.UserControls
{
    public partial class IntegratedChart
    {
        List<KeyValuePair<Type, IIndicator>> _indicators = new List<KeyValuePair<Type, IIndicator>>();
        private void IndicatorCharts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            financialChart.BeginUpdate();

            var newItems = new List<Type>();
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    newItems.Add(item as System.Type);
                }
            }
            var oldItems = new List<Type>();
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    oldItems.Add(item as System.Type);
                }
            }
            
            var addTypes = newItems.Where(p => !_indicators.Select(q=>q.Key).Contains(p));
            var removeTypes = oldItems.Where(p => _indicators.Select(q => q.Key).Contains(p));
            var count = _indicators.Count + addTypes.Count() - removeTypes.Count();

            CreatePlotArea(1 + count);
            List<Type> types = new List<Type>(_indicators.Select(q => q.Key));
            
            foreach (var type in removeTypes)
            {
                types.Remove(type);
                var removeKV = _indicators.FirstOrDefault(p => p.Key == type);

                removeKV.Value.RemoveAllSeriesFromChart();
                removeKV.Value.SettingParamsChanged -= NewIndicator_SettingParamsChanged;
                _indicators.Remove(removeKV);
            }
            types.AddRange(addTypes);
            types.Reverse();


            foreach (var type in addTypes)
            {
                var newIndicator = GetIndicator(type, "plot" + (types.IndexOf(type) + 1));
                newIndicator.SettingParamsChanged += NewIndicator_SettingParamsChanged;
                _indicators.Add(new KeyValuePair<Type, IIndicator>(type, newIndicator));
            }

            foreach (var kv in _indicators)
            {
                _indicators.FirstOrDefault(p => p.Key == kv.Key).Value.PlotAreaName = "plot" + (types.IndexOf(kv.Key) + 1);
            }

            financialChart.EndUpdate();
            IndicatorsRangeSelectorChanged();
        }

        private void NewIndicator_SettingParamsChanged(object sender, EventArgs e)
        {
            financialChart.BeginUpdate();
            IIndicator indcator = sender as IIndicator;

            IEnumerable<Object.QuoteRange> ranges = indcator.GetYRange(Model.LowerValue.Value, Model.UpperValue.Value);

            indcator.AxisY.Min
                        = ranges.Min(p => { return p == null ? int.MaxValue : p.Min; });
            indcator.AxisY.Max
            = ranges.Max(p => { return p == null ? int.MinValue : p.Max; });
            financialChart.EndUpdate();
        }

        private void IndicatorsRangeSelectorChanged()
        {
            foreach (var kv in _indicators)
            {
                IEnumerable<Object.QuoteRange> ranges = kv.Value.GetYRange(Model.LowerValue.Value, Model.UpperValue.Value);

                kv.Value.AxisY.Min
                            = ranges.Min(p => { return p == null ? int.MaxValue : p.Min; });
                kv.Value.AxisY.Max
                = ranges.Max(p => { return p == null ? int.MinValue : p.Max; });
            }
        }

        private void CreatePlotArea(int rowCount)
        {
            financialChart.PlotAreas.Clear();

            var mainPlotHeight = this.financialChart.PlotRect.Top + this.financialChart.PlotRect.Height;

            financialChart.PlotAreas.Add(new C1.WPF.Chart.PlotArea() { PlotAreaName = "plot0", Row = 0});
            int rowIndex = 1;
            for (int i = 1; i < rowCount; i++)
            {
                financialChart.PlotAreas.Add(new C1.WPF.Chart.PlotArea() { PlotAreaName = "spacing" + i, Row = rowIndex++, Height = new System.Windows.GridLength( i == 1 ? 15 : 5) });
                financialChart.PlotAreas.Add(new C1.WPF.Chart.PlotArea() { PlotAreaName = "plot" + i, Row = rowIndex++, Height = new System.Windows.GridLength(30) });

                mainPlotHeight -= ((i == 1 ? 15 : 5) + 30);
            }
            if (AL != null)
            {
                AL.ClientAreaHeight = mainPlotHeight;
            }
        }


        private IIndicator GetIndicator(Type type, string plotAreaName)
        {
            IIndicator indicator = type.Assembly.CreateInstance(type.FullName,false, System.Reflection.BindingFlags.CreateInstance, null, new object[] {this.financialChart, plotAreaName }, null, null) as IIndicator;
            return indicator;
        }
        
        public double MainPlotHeight
        {
            get;
            set;
        }
        
    }
    
}
