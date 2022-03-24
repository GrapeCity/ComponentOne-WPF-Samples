using C1.WPF.Chart;
using C1.WPF.Chart.Finance;
using StockAnalysis.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StockAnalysis.Partial.CustomControls.IndicatorSeries
{
    public enum StochasticPresetType { Manual, Fast, Slow, Full };
    public class StochasticSeries : IndicatorSeriesBase
    {
        StochasticPresetType stochasticType;
        
        public StochasticPresetType StochasticType
        {
            get { return stochasticType; }
            set
            {
                if (value != stochasticType)
                {
                    stochasticType = value;

                    if (series != null &&
                        DPeriodSettingParam != null &&
                        KPeriodSettingParam != null &&
                        SmoothingPeriodSettingParam != null)
                    {
                        SmoothingPeriodSettingParam.PropertyChanged -= SmoothingPeriodSettingParam_PropertyChanged;
                        switch (value)
                        {
                            case StochasticPresetType.Fast:
                                series.DPeriod = 3;
                                series.KPeriod = 14;
                                series.SmoothingPeriod = 1;

                                DPeriodSettingParam.Value = series.DPeriod;
                                KPeriodSettingParam.Value = series.KPeriod;
                                SmoothingPeriodSettingParam.Value = series.SmoothingPeriod;

                                DPeriodSettingParam.IsEditable = KPeriodSettingParam.IsEditable = SmoothingPeriodSettingParam.IsEditable = false;
                                break;

                            case StochasticPresetType.Slow:
                                series.DPeriod = 3;
                                series.KPeriod = 14;
                                series.SmoothingPeriod = 3;
                                
                                DPeriodSettingParam.Value = series.DPeriod;
                                KPeriodSettingParam.Value = series.KPeriod;
                                SmoothingPeriodSettingParam.Value = series.SmoothingPeriod;

                                DPeriodSettingParam.IsEditable = KPeriodSettingParam.IsEditable = SmoothingPeriodSettingParam.IsEditable = false;

                                break;

                            case StochasticPresetType.Full:
                                if (series.SmoothingPeriod == 1)
                                    series.SmoothingPeriod = 3;
                                series.DPeriod = series.SmoothingPeriod;
                                series.KPeriod = 14;

                                DPeriodSettingParam.Value = series.DPeriod;
                                KPeriodSettingParam.Value = series.KPeriod;
                                SmoothingPeriodSettingParam.Value = series.SmoothingPeriod;

                                DPeriodSettingParam.IsEditable = KPeriodSettingParam.IsEditable = false;
                                SmoothingPeriodSettingParam.IsEditable = true;

                                SmoothingPeriodSettingParam.PropertyChanged += SmoothingPeriodSettingParam_PropertyChanged;

                                break;
                            default:
                                DPeriodSettingParam.IsEditable = KPeriodSettingParam.IsEditable = SmoothingPeriodSettingParam.IsEditable = true;
                                break;
                        }
                    }
                }
            }
        }
        
        private void SmoothingPeriodSettingParam_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (series != null &&
                DPeriodSettingParam != null)
            {
                series.DPeriod = series.SmoothingPeriod;
                DPeriodSettingParam.Value = series.DPeriod;
            }
        }

        private Data.SettingParam dPeriodSettingParam;
        public SettingParam DPeriodSettingParam
        {
            get
            {
                if (dPeriodSettingParam == null)
                {
                    dPeriodSettingParam = Utilities.Helper.GetSettingsParam("Stochastic", new Data.PropertyParam("DPeriod", typeof(int)));
                }
                return dPeriodSettingParam;
            }
        }
        private Data.SettingParam kPeriodSettingParam;
        public SettingParam KPeriodSettingParam
        {
            get
            {
                if (kPeriodSettingParam == null)
                {
                    kPeriodSettingParam = Utilities.Helper.GetSettingsParam("Stochastic", new Data.PropertyParam("KPeriod", typeof(int)));
                }
                return kPeriodSettingParam;
            }
        }
        private Data.SettingParam smoothingPeriodSettingParam;
        public SettingParam SmoothingPeriodSettingParam
        {
            get
            {
                if (smoothingPeriodSettingParam == null)
                {
                    smoothingPeriodSettingParam = Utilities.Helper.GetSettingsParam("Stochastic", new Data.PropertyParam("SmoothingPeriod", typeof(int)));
                }
                return smoothingPeriodSettingParam;
            }
        }

        public 

        Stochastic series;
        public StochasticSeries(C1FlexChart chart, string plotAreaName) : base()
        {
            Chart = chart;
            Chart.BeginUpdate();
            
            AxisY.TitleStyle = new ChartStyle();
            AxisY.TitleStyle.FontWeight = FontWeights.Bold;
            AxisY.Position = C1.Chart.Position.Right;
            AxisY.PlotAreaName = plotAreaName;
            AxisY.Title = "Stoc";
            AxisY.Labels = false;
            AxisY.MajorTickMarks = AxisY.MinorTickMarks = C1.Chart.TickMark.None;

            series = new Stochastic();
            series.DLineStyle = new ChartStyle();
            series.DLineStyle.StrokeThickness = 1;
            series.KLineStyle = new ChartStyle();
            series.KLineStyle.StrokeThickness = 1;
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


            Utilities.Helper.BindingSettingsParams(chart, this, typeof(StochasticSeries), "Stochastic",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("StochasticType", typeof(StochasticPresetType)),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            Utilities.Helper.BindingSettingsParams(chart, series, typeof(Stochastic), "Stochastic",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("KPeriod", typeof(int)),
                    new Data.PropertyParam("DPeriod", typeof(int)),
                    new Data.PropertyParam("SmoothingPeriod", typeof(int)),
                    new Data.PropertyParam("DLineStyle.Stroke", typeof(Brush)),
                    new Data.PropertyParam("KLineStyle.Stroke", typeof(Brush)),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );


            Utilities.Helper.BindingSettingsParams(chart, overBought, typeof(ThresholdSeries), "Stochastic",
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
            Utilities.Helper.BindingSettingsParams(chart, overSold, typeof(ThresholdSeries), "Stochastic",
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
