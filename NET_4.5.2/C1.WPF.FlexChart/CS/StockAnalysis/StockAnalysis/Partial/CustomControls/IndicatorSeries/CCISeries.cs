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
    public class CCISeries : IndicatorSeriesBase
    {
        public CCISeries(C1FlexChart chart, string plotAreaName) : base()
        {
            Chart = chart;
            Chart.BeginUpdate();

            
            AxisY.TitleStyle = new ChartStyle();
            AxisY.TitleStyle.FontWeight = FontWeights.Bold;
            AxisY.Position = C1.Chart.Position.Right;
            AxisY.PlotAreaName = plotAreaName;
            AxisY.Title = "CCI";
            AxisY.Labels = false;
            AxisY.MajorTickMarks = AxisY.MinorTickMarks = C1.Chart.TickMark.None;

            CCI series = new CCI();

            series.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            series.Style = new C1.WPF.Chart.ChartStyle();
            series.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            series.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            series.Style.StrokeThickness = 1;
            series.BindingX = "Date";
            series.Binding = "High,Low,Close";


            series.AxisY = AxisY;
            Chart.Series.Add(series);


            ThresholdSeries overBought = new ThresholdSeries();
            overBought.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            overBought.Style = new C1.WPF.Chart.ChartStyle();
            overBought.Style.Stroke = new SolidColorBrush(ViewModel.IndicatorPalettes.StockGreen);
            overBought.Style.StrokeThickness = 1;

            overBought.AxisY = AxisY;
            Chart.Series.Add(overBought);

            ThresholdSeries overSold = new ThresholdSeries();
            overSold.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            overSold.Style = new C1.WPF.Chart.ChartStyle();
            overSold.Style.Stroke = new SolidColorBrush(ViewModel.IndicatorPalettes.StockRed);
            overSold.Style.StrokeThickness = 1;

            overSold.AxisY = AxisY;
            Chart.Series.Add(overSold);

            Utilities.Helper.BindingSettingsParams(chart, series, typeof(CCI), "Commodity Channel Index (CCI)",
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

            Utilities.Helper.BindingSettingsParams(chart, overBought, typeof(ThresholdSeries), "Commodity Channel Index (CCI)",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("ZonesVisibility", typeof(bool)),
                    new Data.PropertyParam("Threshold", typeof(int), "OverBought"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            Utilities.Helper.BindingSettingsParams(chart, overSold, typeof(ThresholdSeries), "Commodity Channel Index (CCI)",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("ZonesVisibility", typeof(bool)),
                    new Data.PropertyParam("Threshold", typeof(int), "OverSold"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            overBought.OnThesholdChanged += (o, e) =>
            {
                this.OnSettingParamsChanged();
            };
            overSold.OnThesholdChanged += (o, e) =>
            {
                this.OnSettingParamsChanged();
            };



            //binding series color to axis title.
            Binding binding = new Binding("Stroke");
            binding.Source = series.Style;
            BindingOperations.SetBinding(AxisY.TitleStyle, ChartStyle.StrokeProperty, binding);


            Chart.EndUpdate();

            this.Series = new FinancialSeries[] { series, overBought, overSold };
        }
    }
}
