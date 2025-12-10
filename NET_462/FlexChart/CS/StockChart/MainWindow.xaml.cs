using C1.Chart;
using C1.WPF.Chart.Finance;
using C1.WPF.Theming;
using C1.WPF.Theming.ExpressionDark;
using StockChart.ViewModel;
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

namespace StockChart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModel.ChartViewModel.Instance;
            
            this.Loaded += (o, e) =>
            {
                var theme = new C1ThemeExpressionDark();
                C1Theme.ApplyTheme(this, theme);


                ViewModel.ChartViewModel.Instance.MainChart = financialChart1;
                ViewModel.ChartViewModel.Instance.MainSeries = mainSeries;
                ViewModel.ChartViewModel.Instance.MainMovingAverage = mainMovingAverage;
                ViewModel.ChartViewModel.Instance.VolumeSeries = volumeSeries;
                ViewModel.ChartViewModel.Instance.RangeSelector = rangeSelector;
                ViewModel.ChartViewModel.Instance.AnnotationLayer = annotationLayer;
                ViewModel.ChartViewModel.Instance.LineMarker = lineMarker;

                this.rangeSelector.UpperValue = DateTime.Now.ToOADate();
                this.rangeSelector.LowerValue = DateTime.Parse(string.Format("01-01-{0}", DateTime.Now.Year)).ToOADate();
                
                this.financialChart2.AxisX.Min = new DateTime(2007, 12, 31).ToOADate();

            };

            //Change Line marker color.
            this.lineMarker.Loaded += (o, e) =>
            {
                Brush whiteBrush = new SolidColorBrush(Colors.White);
                IEnumerable<DependencyObject> visuals = GetLineVisual(this.lineMarker);
                foreach (var item in visuals)
                {
                    var line = item as Line;
                    line.Fill = line.Stroke = whiteBrush;
                }
            };
        }

        private void financialChart1_MouseMove(object sender, MouseEventArgs e)
        {
            var ht = financialChart1.HitTest(e.GetPosition(financialChart1));
            if (ht != null)
            {
                var result = new StringBuilder();
                if (ht.PointIndex >= 0)
                {
                    DateTime currDate = DateTime.MaxValue;
                    if (ht.Series != null && ht.Item != null)
                    {
                        currDate = Convert.ToDateTime(ht.Item.GetRefValue("date"));
                    }
                    if (ht.Series is C1.WPF.Chart.Finance.MovingAverage)
                    {
                        currDate = currDate.AddDays(0 - (ht.Series as C1.WPF.Chart.Finance.MovingAverage).Period);
                    }

                    if (currDate == DateTime.MaxValue)
                    {
                        return;
                    }

                    Stack<QuoteInfo> quotes = new Stack<QuoteInfo>();
                    string binding = ViewModel.ChartViewModel.Instance.Binding == "high,low,open,close" ? "close" : ViewModel.ChartViewModel.Instance.Binding;
                    if (ViewModel.ChartViewModel.Instance.DataSource != null)
                    {
                        result.Append(string.Format("{0}: ", ViewModel.ChartViewModel.Instance.MainSymbol));
                        var quote = (from p in ViewModel.ChartViewModel.Instance.DataSource where (p.date >= currDate) select p).FirstOrDefault();

                        if (quote != null)
                        {
                            double dd = quote.GetValue(binding);

                            quotes.Push(new QuoteInfo()
                            {
                                Code = ViewModel.ChartViewModel.Instance.MainSymbol,
                                Color = Color.FromArgb(255, 255,165,0),
                                Value = dd,
                                Volume = Convert.ToInt32(quote.GetRefValue("volume")),
                                Date = Convert.ToDateTime(quote.GetRefValue("date"))
                            });
                        }
                    }

                    if (ViewModel.ChartViewModel.Instance.SymbolCollection != null && ViewModel.ChartViewModel.Instance.SymbolCollection.Any())
                    {
                        foreach (var series in financialChart1.Series)
                        {
                            if (series.Visibility == SeriesVisibility.Visible && !(series is MovingAverage) && !string.IsNullOrEmpty(series.SeriesName))
                            {
                                var currSeries = (from p in ViewModel.ChartViewModel.Instance.SymbolCollection where p.Series == series select p).FirstOrDefault();

                                if (currSeries != null)
                                {

                                    var quote = (from p in currSeries.DataSource where (p.date >= currDate) select p).FirstOrDefault();

                                    if (quote != null)
                                    {
                                        double dd = quote.GetValue(binding);

                                        quotes.Push(new QuoteInfo()
                                        {
                                            Code = currSeries.Code,
                                            Color = currSeries.Color,
                                            Value = dd,
                                            Volume = Convert.ToInt32(quote.GetRefValue("volume")),
                                            Date = Convert.ToDateTime(quote.GetRefValue("date"))
                                        });
                                    }
                                }
                            }
                        }
                    }
                    ViewModel.ChartViewModel.Instance.ComparisonQuoteInfo = quotes;
                }
            }
        }
       
        IEnumerable<DependencyObject> GetLineVisual(DependencyObject obj)
        {
            List<DependencyObject> objs = new List<DependencyObject>();
           for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var visual = VisualTreeHelper.GetChild(obj, i);
                
                if (visual is Line)
                {
                    objs.Add(visual);
                }
                else
                {
                    objs.AddRange(GetLineVisual(visual)); 
                }

            }
            return objs;

        }

        private void OnLineMarkerPositionChanged(object sender, C1.WPF.Chart.Interaction.PositionChangedArgs e)
        {
            if (financialChart1 != null)
            {
                var info = financialChart1.HitTest(new Point(e.Position.X, e.Position.Y));
                var value = lineMarker.Y; // financialChart1.AxisY.Min + (financialChart1.AxisY.Max - financialChart1.AxisY.Min) * (financialChart1.PlotRect.Top + financialChart1.PlotRect.Height - lineMarker.Y) / financialChart1.PlotRect.Height;

                //draw Y rectangle

                var format = string.Format((value >= 0 ? "+{0:P2}" : "{0:P2}"), value);
                if (value == 0)
                {
                    format = "{0:P2}";
                }
                var text = string.Format(ViewModel.ChartViewModel.Instance.IsComparisonMode ? format : "{0:.##}", value);

                var tb = new TextBlock();
                tb.Padding = new Thickness(10);
                tb.Background = new SolidColorBrush(Color.FromArgb(255, 34, 34, 34));

                tb.Inlines.Add(new Run()
                {
                    Text = string.Format("{0:MMM dd, yyyy} \r\n", info.X),
                    Foreground = new SolidColorBrush() { Color = Color.FromArgb(255, 170, 170, 170) },
                });

                tb.Inlines.Add(new Run()
                {
                    Text = text,
                    Foreground = new SolidColorBrush() { Color = Color.FromArgb(255, 170, 170, 170) },
                    FontWeight = FontWeights.Bold
                });
                lineMarker.Content = tb;
            }
        }

    }
}
