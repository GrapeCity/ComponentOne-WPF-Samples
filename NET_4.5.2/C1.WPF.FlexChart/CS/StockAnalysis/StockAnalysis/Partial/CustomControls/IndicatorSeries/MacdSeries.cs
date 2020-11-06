using C1.WPF.Chart;
using C1.WPF.Chart.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StockAnalysis.Partial.CustomControls.IndicatorSeries
{
    public class MacdSeries : IndicatorSeriesBase
    {
        public MacdSeries(C1FlexChart chart, string plotAreaName) : base()
        {
            Chart = chart;
            Chart.BeginUpdate();
            
            AxisY.TitleStyle = new ChartStyle();
            AxisY.TitleStyle.FontWeight = FontWeights.Bold;
            AxisY.Position = C1.Chart.Position.Right;
            AxisY.PlotAreaName = plotAreaName;
            AxisY.Title = "MACD";
            AxisY.Labels = false;
            AxisY.MajorTickMarks = AxisY.MinorTickMarks = C1.Chart.TickMark.None;


            Macd series = new Macd();
            series.MacdLineStyle = new ChartStyle();
            series.MacdLineStyle.StrokeThickness = 1;
            series.SignalLineStyle = new ChartStyle();
            series.SignalLineStyle.StrokeThickness = 1;
            series.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            series.Style = new C1.WPF.Chart.ChartStyle();
            series.Style.StrokeThickness = 1;
            series.BindingX = "Date";
            series.Binding = "High,Low,Close";

            series.AxisY = AxisY;
            Chart.Series.Add(series);



            MacdHistogram histogram = new MacdHistogram();
            histogram.Style = new C1.WPF.Chart.ChartStyle();
            histogram.Style.StrokeThickness = 1;
            histogram.BindingX = "Date";
            histogram.Binding = "High,Low,Close";


            double[] values = null;
            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "CurrectQuote")
                {
                    values = histogram.GetValues(0); // this is the value;
                }
            };

            histogram.SymbolRendering += (sender, ev) =>
            {
                if (values == null)
                {
                    values = histogram.GetValues(0); // this is the volume value;
                }
                if (values != null)
                {
                    if (values[ev.Index] > 0)
                    {
                        if (IncreasingBar is SolidColorBrush)
                        {
                            Color c = (IncreasingBar as SolidColorBrush).Color;
                            Color cf = Color.FromArgb(128, c.R, c.G, c.B);
                            ev.Engine.SetStroke(c.ToArgb());
                            ev.Engine.SetFill(cf.ToArgb());
                        }
                    }
                    else
                    {
                        if (DecreasingBar is SolidColorBrush)
                        {
                            Color c = (DecreasingBar as SolidColorBrush).Color;
                            Color cf = Color.FromArgb(128, c.R, c.G, c.B);
                            ev.Engine.SetStroke(c.ToArgb());
                            ev.Engine.SetFill(cf.ToArgb());
                        }
                    }
                }
            };
            
            histogram.AxisY = AxisY;
            Chart.Series.Add(histogram);




            Utilities.Helper.BindingSettingsParams(chart, series, typeof(Macd), "MACD",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("FastPeriod", typeof(int)),
                    new Data.PropertyParam("SlowPeriod", typeof(int)),
                    new Data.PropertyParam("SmoothingPeriod", typeof(int)),
                    new Data.PropertyParam("MacdLineStyle.Stroke", typeof(Brush), "Series"),
                    new Data.PropertyParam("SignalLineStyle.Stroke", typeof(Brush), "Series"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            //Utilities.Helper.BindingSettingsParams(chart, histogram, typeof(MacdHistogram), "MACD",
            //    new Data.PropertyParam[]
            //    {
            //        new Data.PropertyParam("Style.Stroke", typeof(Brush), "Histogram"),
            //        new Data.PropertyParam("Style.Fill", typeof(Brush), "Histogram"),
            //    },
            //    () =>
            //    {
            //        this.OnSettingParamsChanged();
            //    }
            //);

            Utilities.Helper.BindingSettingsParams(chart, this, typeof(MacdSeries), "MACD",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("IncreasingBar", typeof(Brush)),
                    new Data.PropertyParam("DecreasingBar", typeof(Brush)),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            //binding series color to axis title.
            Binding binding = new Binding("Stroke");
            binding.Source = series.MacdLineStyle;
            BindingOperations.SetBinding(AxisY.TitleStyle, ChartStyle.StrokeProperty, binding);

            Chart.EndUpdate();

            this.Series = new FinancialSeries[] { series, histogram };
        }


        public Brush IncreasingBar
        {
            get;
            set;
        }
        public Brush DecreasingBar
        {
            get;
            set;
        }
    }
}
