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
    public class VolumeSeries : IndicatorSeriesBase
    {
        public VolumeSeries(C1FlexChart chart, string plotAreaName) : base()
        {
            Chart = chart;
            Chart.BeginUpdate();

            AxisY.TitleStyle = new ChartStyle();
            AxisY.TitleStyle.FontWeight = FontWeights.Bold;
            AxisY.Position = C1.Chart.Position.Right;
            AxisY.PlotAreaName = plotAreaName;
            AxisY.Title = "Vol";
            AxisY.Labels = false;
            AxisY.MajorTickMarks = AxisY.MinorTickMarks = C1.Chart.TickMark.None;

            FinancialSeries series = new FinancialSeries();

            series.ChartType = C1.Chart.Finance.FinancialChartType.ColumnVolume;
            series.Style = new C1.WPF.Chart.ChartStyle();
            series.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            series.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            series.Style.StrokeThickness = 1;
            series.BindingX = "Date";
            series.Binding = "Volume,Close";


            double[] values = null;
            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "CurrectQuote")
                {
                    values = series.GetValues(2); // this is the closing value;
                }
            };
            series.SymbolRendering += (sender, ev) =>
            {
                if (ev.Index == 0)
                {
                    return;
                }
                if (values == null)
                {
                    values = series.GetValues(2); // this is the closing value;
                }
                if (values != null)
                {
                    if (values[ev.Index - 1] > values[ev.Index])
                    {
                        if (DownVolume is SolidColorBrush)
                        {
                            Color c = (DownVolume as SolidColorBrush).Color;
                            Color cf = Color.FromArgb(128, c.R, c.G, c.B);
                            ev.Engine.SetStroke(c.ToArgb());
                            ev.Engine.SetFill(cf.ToArgb());
                        }
                    }
                    else
                    {
                        if (UpVolume is SolidColorBrush)
                        {
                            Color c = (UpVolume as SolidColorBrush).Color;
                            Color cf = Color.FromArgb(128, c.R, c.G, c.B);
                            ev.Engine.SetStroke(c.ToArgb());
                            ev.Engine.SetFill(cf.ToArgb());
                        }
                    }
                }
            };


            series.AxisY = AxisY;
            Chart.Series.Add(series);


            //Utilities.Helper.BindingSettingsParams(chart, series, typeof(FinancialSeries), "Volume",
            //    new Data.PropertyParam[]
            //    {
            //        new Data.PropertyParam("Style.Fill", typeof(Brush)),
            //        new Data.PropertyParam("Style.Stroke", typeof(Brush)),
            //    },
            //    () =>
            //    {
            //        this.OnSettingParamsChanged();
            //    }
            //);

            Utilities.Helper.BindingSettingsParams(chart, this, typeof(VolumeSeries), "Volume",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("UpVolume", typeof(Brush)),
                    new Data.PropertyParam("DownVolume", typeof(Brush)),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            //binding series color to axis title.
            Binding binding = new Binding("UpVolume");
            binding.Source = this;
            BindingOperations.SetBinding(AxisY.TitleStyle, ChartStyle.StrokeProperty, binding);

            Chart.EndUpdate();

            this.Series = new FinancialSeries[] { series };
        }


        public Brush UpVolume
        {
            get;
            set;
        }
        public Brush DownVolume
        {
            get;
            set;
        }
    }
}
