using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using C1.Chart;
using C1.Chart.Finance;
using StockAnalysis.Object;
using System.Windows.Media;
using C1.WPF.Chart;
using System.Windows.Data;
using C1.WPF.Chart.Finance;

namespace StockAnalysis.Partial.CustomControls.CustomIndicator
{
    public class ADX : IndicatorSeries.IndicatorSeriesBase
    {
        public ADX(C1FlexChart chart, string plotAreaName) : base()
        {
            Chart = chart;
            Chart.BeginUpdate();
            
            AxisY.TitleStyle = new ChartStyle();
            AxisY.TitleStyle.FontWeight = FontWeights.Bold;
            AxisY.Position = C1.Chart.Position.Right;
            AxisY.PlotAreaName = plotAreaName;
            AxisY.Title = "ADX";
            AxisY.Labels = false;
            AxisY.MajorTickMarks = AxisY.MinorTickMarks = C1.Chart.TickMark.None;


            DIPSeries dip = new DIPSeries();
            dip.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            dip.Style = new C1.WPF.Chart.ChartStyle();
            dip.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            dip.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            dip.Style.StrokeThickness = 1;
            //dip.BindingX = "Date";
            //dip.Binding = "High,Low,Close";
            dip.AxisY = AxisY;
            Chart.Series.Add(dip);

            DINSeries din = new DINSeries();
            din.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            din.Style = new C1.WPF.Chart.ChartStyle();
            din.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            din.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            din.Style.StrokeThickness = 1;
            //din.BindingX = "Date";
            //din.Binding = "High,Low,Close";
            din.AxisY = AxisY;
            Chart.Series.Add(din);

            ADXSeries adx = new ADXSeries();
            adx.ChartType = C1.Chart.Finance.FinancialChartType.Line;
            adx.Style = new C1.WPF.Chart.ChartStyle();
            adx.Style.Stroke = new SolidColorBrush(ViewModel.IndicatorPalettes.DefaultBorder);
            adx.Style.StrokeThickness = 1;
            //adx.BindingX = "Date";
            //adx.Binding = "High,Low,Close";
            adx.AxisY = AxisY;
            Chart.Series.Add(adx);



            Utilities.Helper.BindingSettingsParams(chart, dip, typeof(DIPSeries), "Average Directional Index (ADX)",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Period", typeof(int)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "D+"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            Utilities.Helper.BindingSettingsParams(chart, din, typeof(DINSeries), "Average Directional Index (ADX)",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Period", typeof(int)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "D-"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            Utilities.Helper.BindingSettingsParams(chart, adx, typeof(ADXSeries), "Average Directional Index (ADX)",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Period", typeof(int)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "ADX"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            //binding series color to axis title.
            Binding binding = new Binding("Stroke");
            binding.Source = adx.Style;
            BindingOperations.SetBinding(AxisY.TitleStyle, ChartStyle.StrokeProperty, binding);


            Chart.EndUpdate();

            this.Series = new FinancialSeries[] { dip, din, adx };




            Object.Quote quote = ViewModel.ViewModel.Instance.CurrectQuote;
            if (quote != null)
                ADXCalculator.Instance.Source = quote.Data;
            if (dip != null)
                dip.Calculator = ADXCalculator.Instance;
            if (din != null)
                din.Calculator = ADXCalculator.Instance;
            if (adx != null)
                adx.Calculator = ADXCalculator.Instance;

            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "CurrectQuote")
                {
                    quote = ViewModel.ViewModel.Instance.CurrectQuote;
                    ADXCalculator.Instance.Source = quote.Data;
                }
            };
        }
    }
    

}
