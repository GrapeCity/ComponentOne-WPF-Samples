using C1.Chart;
using C1.WPF;
using C1.WPF.Chart;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for Indicators.xaml
    /// </summary>
    public partial class Indicators : UserControl
    {
        DataService dataService = DataService.GetService();

        public Indicators()
        {
            InitializeComponent();
        }

        public List<Quote> Data
        {
            get
            {
                return dataService.GetSymbolData("box");
            }
        }

        public List<string> IndicatorType
        {
            get
            {
                return new List<string>()
                {
                    "Average True Range",
                    "Relative Strength Index",
                    "Commodity Channel Index",
                    "Williams %R",
                    "MACD",
                    "Stochastic"
                };
            }
        }

        void OnFinancialChartRendered(object sender, RenderEventArgs e)
        {
            if (indicatorChart != null)
            {
                indicatorChart.AxisX.Min = ((IAxis)financialChart.AxisX).GetMin();
                indicatorChart.AxisX.Max = ((IAxis)financialChart.AxisX).GetMax();
            }
        }

        void OnPeriodValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            atr.Period = rsi.Period = cci.Period = wi.Period = CorrectValue(e.NewValue);
        }

        void OnFastPeriodValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            macd.FastPeriod = CorrectValue(e.NewValue);
            histogram.FastPeriod = CorrectValue(e.NewValue);
        }

        void OnSlowPeriodValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            macd.SlowPeriod = CorrectValue(e.NewValue);
            histogram.SlowPeriod = CorrectValue(e.NewValue);
        }

        void OnSmoothingPeriodValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            macd.SmoothingPeriod = CorrectValue(e.NewValue);
            histogram.SmoothingPeriod = CorrectValue(e.NewValue);
        }

        private void OnKPeriodValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            stochastic.KPeriod = CorrectValue(e.NewValue);
        }

        private void OnDPeriodValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            stochastic.DPeriod = CorrectValue(e.NewValue);
        }

        private void OnStochasticSmoothingPeriodValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            stochastic.SmoothingPeriod = CorrectValue(e.NewValue, 1);
        }

        private int CorrectValue(double newValue, int minimum = 2)
        {
            return Math.Max(Math.Min(86, (int)newValue), minimum);
        }

        private void indicatorChart_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var pt = e.GetPosition(indicatorChart);
            var hitTest = indicatorChart.HitTest(pt);
            var series = hitTest != null ? hitTest.Series : null;
            if (series != null)
            {
                if (series == macd)
                {
                    indicatorChart.ToolTipContent = "Date: {Date}\u000AMACD: {Macd:n2}\u000ASignal: {Signal:n2}";
                }
                else if (series == stochastic)
                {
                    indicatorChart.ToolTipContent = "Date: {Date}\u000A%K: {K:n2}\u000A%D: {D:n2}";
                }
                else
                {
                    indicatorChart.ToolTipContent = "{seriesName}\u000ADate: {Date}\u000AY: {y:n2}\u000AVolume: {Volume:n0}";
                }
            }
        }
    }
}
