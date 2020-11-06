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

namespace StockAnalysis.Partial.CustomControls.IndicatorSeries
{
    public class ATRSeries : IndicatorSeriesBase
    {
        public ATRSeries(C1FlexChart chart, string plotAreaName) : base()
        {
            Chart = chart;
            Chart.BeginUpdate();

            
            AxisY.TitleStyle = new ChartStyle();
            AxisY.TitleStyle.FontWeight = FontWeights.Bold;
            AxisY.Position = C1.Chart.Position.Right;
            AxisY.PlotAreaName = plotAreaName;
            AxisY.Title = "ATR";
            AxisY.Labels = false;
            AxisY.MajorTickMarks = AxisY.MinorTickMarks = C1.Chart.TickMark.None;


            ATR series = new ATR();

            series.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            series.Style = new C1.WPF.Chart.ChartStyle();
            series.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            series.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            series.Style.StrokeThickness = 1;
            series.BindingX = "Date";
            series.Binding = "High,Low,Close";


            series.AxisY = AxisY;
            Chart.Series.Add(series);
            

            Utilities.Helper.BindingSettingsParams(chart, series, typeof(ATR), "Average True Range (ATR)",
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

            //binding series color to axis title.
            Binding binding = new Binding("Stroke");
            binding.Source = series.Style;
            BindingOperations.SetBinding(AxisY.TitleStyle, ChartStyle.StrokeProperty, binding);


            Chart.EndUpdate();

            this.Series = new FinancialSeries[] { series };
        }
    }
}
