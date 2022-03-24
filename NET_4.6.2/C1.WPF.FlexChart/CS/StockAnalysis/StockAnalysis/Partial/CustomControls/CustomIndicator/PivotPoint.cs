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
    public enum PivotPointType
    {
        Standard,
        Fibonacci
    };
    public class PivotPoint : IndicatorSeries.IndicatorSeriesBase
    {
        public new Axis AxisY
        {
            get;
            private set;
        }
        public PivotPointType PivotType
        {
            get
            {
                return PivotPointCalculator.Instance.PivotType;
            }
            set
            {
                PivotPointCalculator.Instance.PivotType = value;
            }
        }

        public C1.Chart.SeriesVisibility Visibility
        {
            get { return (C1.Chart.SeriesVisibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        /// <summary>
        /// Identifies the Visibility dependency property.
        /// </summary> 
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register("Visibility", typeof(C1.Chart.SeriesVisibility), typeof(PivotPoint),
            new PropertyMetadata(C1.Chart.SeriesVisibility.Visible, new PropertyChangedCallback(OnVisiblityChanged)));

        private static void OnVisiblityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PivotPoint pp = sender as PivotPoint;
            C1.Chart.SeriesVisibility visibility = (C1.Chart.SeriesVisibility)e.NewValue;
            pp.series.Visibility =
                pp.r1.Visibility = pp.r2.Visibility = pp.r3.Visibility =
                pp.s1.Visibility = pp.s2.Visibility = pp.s3.Visibility = visibility;
        }

        PivotPointSeries series;
        R1Series r1;
        R2Series r2;
        R3Series r3;
        S1Series s1;
        S2Series s2;
        S3Series s3;
        public PivotPoint(C1FlexChart chart, string plotAreaName = null) : base()
        {
            Chart = chart;
            Chart.BeginUpdate();

            if (!string.IsNullOrEmpty(plotAreaName))
            {
                this.AxisY = new Axis();
                this.AxisY.TitleStyle = new ChartStyle();
                this.AxisY.TitleStyle.FontWeight = FontWeights.Bold;
                this.AxisY.Position = C1.Chart.Position.Right;
                this.AxisY.PlotAreaName = plotAreaName;
                this.AxisY.Title = "P&P";
                this.AxisY.Labels = false;
                this.AxisY.MajorTickMarks = this.AxisY.MinorTickMarks = C1.Chart.TickMark.None;
            }

            series = new PivotPointSeries();
            series.SeriesName = "Pivot";
            series.ChartType = C1.Chart.ChartType.Step;
            series.Style = new C1.WPF.Chart.ChartStyle();
            series.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            series.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            series.Style.StrokeThickness = 1;
            series.AxisY = AxisY;
            Chart.Series.Add(series);

            r1 = new R1Series();
            r1.SeriesName = "R1";
            r1.ChartType = C1.Chart.ChartType.Step;
            r1.Style = new C1.WPF.Chart.ChartStyle();
            r1.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            r1.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            r1.Style.StrokeThickness = 1;
            r1.AxisY = AxisY;
            Chart.Series.Add(r1);

            r2 = new R2Series();
            r2.SeriesName = "R2";
            r2.ChartType = C1.Chart.ChartType.Step;
            r2.Style = new C1.WPF.Chart.ChartStyle();
            r2.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            r2.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            r2.Style.StrokeThickness = 1;
            r2.AxisY = AxisY;
            Chart.Series.Add(r2);

            r3 = new R3Series();
            r3.SeriesName = "R3";
            r3.ChartType = C1.Chart.ChartType.Step;
            r3.Style = new C1.WPF.Chart.ChartStyle();
            r3.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            r3.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            r3.Style.StrokeThickness = 1;
            r3.AxisY = AxisY;
            Chart.Series.Add(r3);

            s1 = new S1Series();
            s1.SeriesName = "S1";
            s1.ChartType = C1.Chart.ChartType.Step;
            s1.Style = new C1.WPF.Chart.ChartStyle();
            s1.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            s1.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            s1.Style.StrokeThickness = 1;
            s1.AxisY = AxisY;
            Chart.Series.Add(s1);

            s2 = new S2Series();
            s2.SeriesName = "S2";
            s2.ChartType = C1.Chart.ChartType.Step;
            s2.Style = new C1.WPF.Chart.ChartStyle();
            s2.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            s2.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            s2.Style.StrokeThickness = 1;
            s2.AxisY = AxisY;
            Chart.Series.Add(s2);

            s3 = new S3Series();
            s3.SeriesName = "S3";
            s3.ChartType = C1.Chart.ChartType.Step;
            s3.Style = new C1.WPF.Chart.ChartStyle();
            s3.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
            s3.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
            s3.Style.StrokeThickness = 1;
            s3.AxisY = AxisY;
            Chart.Series.Add(s3);


            Utilities.Helper.BindingSettingsParams(chart, this, typeof(PivotPoint), "Pivot Point",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("PivotType", typeof(PivotPointType)),
                },
                () =>
                {
                    PivotPointCalculator.Instance.Create();
                    this.OnSettingParamsChanged();
                }
            );

            Utilities.Helper.BindingSettingsParams(chart, series, typeof(PivotPointSeries), "Pivot Point",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "Pivot"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            Utilities.Helper.BindingSettingsParams(chart, r1, typeof(PivotPointSeries), "Pivot Point",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "R1"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            Utilities.Helper.BindingSettingsParams(chart, r2, typeof(PivotPointSeries), "Pivot Point",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "R2"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            Utilities.Helper.BindingSettingsParams(chart, r3, typeof(PivotPointSeries), "Pivot Point",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "R3"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            Utilities.Helper.BindingSettingsParams(chart, s1, typeof(PivotPointSeries), "Pivot Point",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "S1"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            Utilities.Helper.BindingSettingsParams(chart, s2, typeof(PivotPointSeries), "Pivot Point",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "S2"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );
            Utilities.Helper.BindingSettingsParams(chart, s3, typeof(PivotPointSeries), "Pivot Point",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Style.Stroke", typeof(Brush), "S3"),
                },
                () =>
                {
                    this.OnSettingParamsChanged();
                }
            );

            if (this.AxisY != null && this.AxisY.TitleStyle != null)
            {
                //binding series color to axis title.
                Binding binding = new Binding("Stroke");
                binding.Source = series.Style;
                BindingOperations.SetBinding(AxisY.TitleStyle, ChartStyle.StrokeProperty, binding);
            }
            
            Object.Quote quote = ViewModel.ViewModel.Instance.CurrectQuote;
            if (quote != null)
                PivotPointCalculator.Instance.Source = quote.Data;
            if (series != null)
                series.Calculator = PivotPointCalculator.Instance;
            if (r1 != null)
                r1.Calculator = PivotPointCalculator.Instance;
            if (r2 != null)
                r2.Calculator = PivotPointCalculator.Instance;
            if (r3 != null)
                r3.Calculator = PivotPointCalculator.Instance;
            if (s1 != null)
                s1.Calculator = PivotPointCalculator.Instance;
            if (s2 != null)
                s2.Calculator = PivotPointCalculator.Instance;
            if (s3 != null)
                s3.Calculator = PivotPointCalculator.Instance;

            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "CurrectQuote")
                {
                    quote = ViewModel.ViewModel.Instance.CurrectQuote;
                    PivotPointCalculator.Instance.Source = quote.Data;
                }
            };
            
            Chart.EndUpdate();

            this.Series = new Series[] { series, r1, r2, r3, s1, s2, s3 };
        }


        internal QuoteRange GetPivotPointsRange(double s, double e)
        {
           List <QuoteRange> ranges = new List<QuoteRange>();

            var range = GetSeriesYRange(series, s, e);
            ranges.Add(range);
            range = GetSeriesYRange(r1, s, e);
            ranges.Add(range);
            range = GetSeriesYRange(r2, s, e);
            ranges.Add(range);
            range = GetSeriesYRange(r3, s, e);
            ranges.Add(range);
            range = GetSeriesYRange(s1, s, e);
            ranges.Add(range);
            range = GetSeriesYRange(s2, s, e);
            ranges.Add(range);
            range = GetSeriesYRange(s3, s, e);
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
                if (start == -1 && x >= s)
                {
                    start = i - 1;
                }
                if (end == -1 && x >= e)
                {
                    end = i;
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
