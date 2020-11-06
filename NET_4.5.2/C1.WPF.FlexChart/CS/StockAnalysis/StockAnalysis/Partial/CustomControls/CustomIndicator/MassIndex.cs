using C1.WPF.Chart;
using C1.WPF.Chart.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StockAnalysis.Object;

namespace StockAnalysis.Partial.CustomControls.CustomIndicator
{
    public class MassIndex : IndicatorSeries.IndicatorSeriesBase
    {
        MassIndexSeries series;
        IndicatorSeries.ThresholdSeries seriesThreshold;
        public MassIndex(C1FlexChart chart, string plotAreaName) : base()
        {
            Chart = chart;
            Chart.BeginUpdate();
            
            AxisY.TitleStyle = new ChartStyle();
            AxisY.TitleStyle.FontWeight = FontWeights.Bold;
            AxisY.Position = C1.Chart.Position.Right;
            AxisY.PlotAreaName = plotAreaName;
            AxisY.Title = "MI";
            AxisY.Labels = false;
            AxisY.MajorTickMarks = AxisY.MinorTickMarks = C1.Chart.TickMark.None;


            series = new MassIndexSeries();
            series.ChartType = C1.Chart.ChartType.Line;
            series.Style = new C1.WPF.Chart.ChartStyle();
            series.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            series.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            series.Style.StrokeThickness = 1;
            //dip.BindingX = "Date";
            //dip.Binding = "High,Low,Close";
            series.AxisY = AxisY;
            Chart.Series.Add(series);


            seriesThreshold = new IndicatorSeries.ThresholdSeries();
            seriesThreshold.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            seriesThreshold.Style = new C1.WPF.Chart.ChartStyle();
            seriesThreshold.Style.Stroke = new SolidColorBrush(ViewModel.IndicatorPalettes.StockGreen);
            seriesThreshold.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            seriesThreshold.Style.StrokeThickness = 1;
            seriesThreshold.AxisY = AxisY;
            Chart.Series.Add(seriesThreshold);
            


            Utilities.Helper.BindingSettingsParams(chart, series, typeof(MassIndexSeries), "Mass Index",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Period", typeof(int)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush)),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            Utilities.Helper.BindingSettingsParams(chart, seriesThreshold, typeof(IndicatorSeries.ThresholdSeries), "Mass Index",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Threshold", typeof(Object.Threshold)),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            seriesThreshold.OnThesholdChanged += (o, e) =>
            {
                this.OnSettingParamsChanged();
            };

            //binding series color to axis title.
            Binding binding = new Binding("Stroke");
            binding.Source = series.Style;
            BindingOperations.SetBinding(AxisY.TitleStyle, ChartStyle.StrokeProperty, binding);



            Object.Quote quote = ViewModel.ViewModel.Instance.CurrectQuote;
            if (quote != null)
                MassIndexCalculator.Instance.Source = quote.Data;

            series.Calculator = MassIndexCalculator.Instance;
            MassIndexCalculator.Instance.Period = series.Period;

            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "CurrectQuote")
                {
                    quote = ViewModel.ViewModel.Instance.CurrectQuote;
                    MassIndexCalculator.Instance.Source = quote.Data;
                }
            };

            Chart.EndUpdate();

            this.Series = new Series[] { series, seriesThreshold };
        }




        internal QuoteRange GetPivotPointsRange(double s, double e)
        {
            List<QuoteRange> ranges = new List<QuoteRange>();

            var range = GetSeriesYRange(series, s, e);
            ranges.Add(range);
            range = GetSeriesYRange(seriesThreshold, s, e);
            ranges.Add(range);

            return new QuoteRange() { Min = ranges.Min(p => p.Min), Max = ranges.Max(p => p.Max) };
        }


        private Object.QuoteRange GetSeriesYRange(Series series, double s, double e)
        {
            Object.QuoteRange qr = null;
            double[] quoteX = series.GetValues(1).ToArray();
            IEnumerable<double> quoteData = series.GetValues(0);

            var start = -1;
            var end = -1;
            for (int i = 0; i < quoteX.Count(); i++)
            {
                var x = quoteX[i];
                if (start == -1 && x > s)
                {
                    start = i - 1;
                    end = quoteX.Count() - 3;
                }
                if (end == -1 && x > e)
                {
                    end = i - 1;
                }
            }



            var len = end - start + 1;

            IEnumerable<double> quoteCache = quoteData.Skip(start).Take(len);
            if (quoteCache.Any())
            {
                qr = new Object.QuoteRange();
                var min = quoteCache.Min();
                var max = quoteCache.Max();
                qr.Min = min;
                qr.Max = max;
            }
            return qr;
        }

    }
}
