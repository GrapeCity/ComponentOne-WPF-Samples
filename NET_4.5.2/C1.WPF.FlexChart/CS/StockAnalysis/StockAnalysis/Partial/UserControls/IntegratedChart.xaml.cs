using C1.Chart.Finance;
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

namespace StockAnalysis.Partial.UserControls
{
    /// <summary>
    /// Interaction logic for IntegratedChart.xaml
    /// </summary>
    public partial class IntegratedChart : UserControl
    {
        CustomControls.CustomIndicator.PivotPoint pivotPoint;
        public IntegratedChart()
        {
            InitializeComponent();
            
            InitContextMenu();

            pivotPoint = new CustomControls.CustomIndicator.PivotPoint(this.financialChart);

            this.financialChart.Loaded += (o, e) =>
            {
                if (AL != null)
                {
                    AL.ClientAreaHeight = this.financialChart.PlotRect.Top + this.financialChart.PlotRect.Height;
                }
            };
            this.financialChart.SizeChanged += (o, e) =>
            {
                try
                {
                    var mainPlotHeight = this.financialChart.PlotRect.Top + this.financialChart.PlotRect.Height;
                    foreach (var area in financialChart.PlotAreas)
                    {
                        if (area.Height.GridUnitType == GridUnitType.Pixel)
                        {
                            mainPlotHeight -= area.Height.Value;
                        }
                    }
                    if (AL != null)
                    {
                        AL.ClientAreaHeight = mainPlotHeight;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            };

            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "LowerValue" ||
                    e.PropertyName == "UpperValue" ||
                    e.PropertyName == "OverlayTypes")
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        RangeSelectorChanged();
                    }));
                }
                else if (e.PropertyName == "ChartType")
                {
                    //for disable annotation when select volume chart type. --START
                    if (ViewModel.ViewModel.Instance.NewAnnotationType == Data.NewAnnotationType.None ||
                        ViewModel.ViewModel.Instance.ChartType == FinancialChartType.ArmsCandleVolume ||
                        ViewModel.ViewModel.Instance.ChartType == FinancialChartType.CandleVolume)
                    {
                        AL.AllowAdd = false;
                        AL.AllowMove = false;
                    }
                    else
                    {
                        AL.AllowAdd = true;
                        AL.AllowMove = true;
                    }
                    //for disable annotation when select volume chart type. --END


                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (ViewModel.ViewModel.Instance.ChartType == FinancialChartType.Kagi ||
                            ViewModel.ViewModel.Instance.ChartType == FinancialChartType.Renko ||
                               ViewModel.ViewModel.Instance.ChartType == FinancialChartType.PointAndFigure)
                        {
                        }
                        else
                        {
                            this.financialChart.ChartType = ViewModel.ViewModel.Instance.ChartType;

                        }
                        if (AL != null)
                        {
                            AL.Annotations.Clear();
                        }
                    }));
                }
                else if (e.PropertyName == "ChartTypeCategory")
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (AL != null)
                        {
                            AL.Annotations.Clear();
                        }
                    }));
                }
                else if (e.PropertyName == "CurrectQuote")
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (AL != null)
                        {
                            AL.Annotations.Clear();
                        }
                        RangeSelectorChanged();
                    }));
                }

            };

            ViewModel.ViewModel.Instance.IndicatorCharts.CollectionChanged += IndicatorCharts_CollectionChanged;
            
        }

        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(double), typeof(IntegratedChart),
            new PropertyMetadata(double.NaN, new PropertyChangedCallback(
                (o, e) =>
                {
                    IntegratedChart ic = o as IntegratedChart;
                    if (ic!= null)
                    {
                        ic.financialChart.AxisX.Min = Convert.ToDouble(e.NewValue);
                    }
                }
                )));
        
        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(IntegratedChart),
            new PropertyMetadata(double.NaN, new PropertyChangedCallback(
                (o, e) =>
                {
                    IntegratedChart ic = o as IntegratedChart;
                    if (ic != null)
                    {
                        ic.financialChart.AxisX.Max = Convert.ToDouble(e.NewValue);
                    }
                }
                )));


        EditableAnnotitions.EditableAnnotationLayer _al;
        public EditableAnnotitions.EditableAnnotationLayer AL
        {
            get
            {
                if (_al == null)
                {
                    _al = new EditableAnnotitions.EditableAnnotationLayer(financialChart);
                }
                return _al;
            }
        }
        public ViewModel.ViewModel Model
        {
            get { return ViewModel.ViewModel.Instance; }
        }


        private void InitContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.Style = this.FindResource("ContextMenuStyle") as Style;

            Binding binding = new Binding("Foreground");
            binding.Source = this;
            BindingOperations.SetBinding(contextMenu, ContextMenu.ForegroundProperty, binding);

            MenuItem miEdit = new MenuItem() { Header = "Edit" };
            miEdit.Click += (o, e) =>
            {
                Point pt = App.Current.MainWindow.PointFromScreen(contextMenu.PointToScreen(new Point(0, 0)));
                AL.ShowContentEditor(pt);
            };
            contextMenu.Items.Add(miEdit);

            MenuItem miDelete = new MenuItem() { Header = "Delete" };
            miDelete.Click += (o, e) =>
            {
                AL.Annotations.Remove(AL.SelectedAnnotation);
            };
            contextMenu.Items.Add(miDelete);

            financialChart.MouseDown += (o, e) =>
            {
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    var selectedAnno = AL.HitTest(e.GetPosition(financialChart));
                    if (selectedAnno != null)
                    {
                        financialChart.ContextMenu = contextMenu;
                        financialChart.ContextMenu.IsOpen = true;
                    }
                    else
                        financialChart.ContextMenu = null;
                }
            };
            financialChart.KeyDown += (o, e) =>
            {
                if (e.Key == Key.Delete)
                {
                    AL.Annotations.Remove(AL.SelectedAnnotation);
                }
            };
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            BindingOverlays(alligator, Data.OverlayType.Alligator);
            BindingOverlays(ichimoku, Data.OverlayType.IchimokuCloud);
            BindingOverlays(bollinger, Data.OverlayType.BollingerBands);
            BindingOverlays(envelopes, Data.OverlayType.Envelopes);
            BindingOverlays(retracements, Data.OverlayType.FibonacciRetracements);
            BindingOverlays(arcs, Data.OverlayType.FibonacciArcs);
            BindingOverlays(fans, Data.OverlayType.FibonacciFans);
            BindingOverlays(timeZones, Data.OverlayType.FibonacciTimeZones);
            BindingOverlays(zigZag, Data.OverlayType.ZigZag);

            // for PivotPoint series cluster Visibility  biding
            Binding binding = new Binding();
            binding.Path = new PropertyPath("OverlayTypes");
            binding.Source = ViewModel.ViewModel.Instance;
            binding.Converter = new Converter.OverlayTypes2SeriesVisibilityConverter();
            binding.ConverterParameter = Data.OverlayType.PivotPoint;
            BindingOperations.SetBinding(pivotPoint, CustomControls.CustomIndicator.PivotPoint.VisibilityProperty, binding);
            
            // for set axisY's MajorGrid hiden when Retracements series shown
            this.financialChart.AxisY.MinorGrid = false;
            binding = new Binding();
            binding.Path = new PropertyPath("Visibility");
            binding.Source = this.retracements;
            binding.Converter = new Converter.SeriesVisibility2BooleanConverter();
            BindingOperations.SetBinding(this.financialChart.AxisY, C1.WPF.Chart.Axis.MajorGridProperty, binding);

            BindingSettingsParams(alligator, typeof(C1.WPF.Chart.Finance.Alligator), "Alligator",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("JawPeriod", typeof(int)),
                    new Data.PropertyParam("TeethPeriod", typeof(int)),
                    new Data.PropertyParam("LipsPeriod", typeof(int)),                   
                    new Data.PropertyParam("JawLineStyle.Stroke", typeof(Brush)),
                    new Data.PropertyParam("TeethLineStyle.Stroke", typeof(Brush)),
                    new Data.PropertyParam("LipsLineStyle.Stroke", typeof(Brush))
                }
            );

            BindingSettingsParams(ichimoku, typeof(C1.WPF.Chart.Finance.IchimokuCloud), "IchimokuCloud",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("ConversionPeriod", typeof(int)),
                    new Data.PropertyParam("BasePeriod", typeof(int)),
                    new Data.PropertyParam("LeadingPeriod", typeof(int)),
                    new Data.PropertyParam("LaggingPeriod", typeof(int)),
                    new Data.PropertyParam("ConversionLineStyle.Stroke", typeof(Brush)),
                    new Data.PropertyParam("BaseLineStyle.Stroke", typeof(Brush)),
                    new Data.PropertyParam("LeadingSpanALineStyle.Stroke", typeof(Brush)),
                    new Data.PropertyParam("LeadingSpanBLineStyle.Stroke", typeof(Brush)),
                    new Data.PropertyParam("LaggingLineStyle.Stroke", typeof(Brush)),
                    new Data.PropertyParam("BearishCloudColor", typeof(Brush)),
                    new Data.PropertyParam("BullishCloudColor", typeof(Brush))
                }
            );

            BindingSettingsParams(bollinger, typeof(C1.WPF.Chart.Finance.BollingerBands), "Bollinger Bands",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Period", typeof(int)),
                    new Data.PropertyParam("Multiplier", typeof(int)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush)),
                }
            );

            BindingSettingsParams(envelopes, typeof(C1.WPF.Chart.Finance.Envelopes), "Envelopes",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Period", typeof(int)),
                    new Data.PropertyParam("Size", typeof(double)),
                    new Data.PropertyParam("Type", typeof(MovingAverageType)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush)),
                }
            );
            BindingSettingsParams(retracements, typeof(C1.WPF.Chart.Finance.Fibonacci), "Fibonacci Retracements",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("Uptrend", typeof(bool)),
                    new Data.PropertyParam("LabelPosition", typeof(C1.Chart.LabelPosition)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush)),
                }
            );
            BindingSettingsParams(arcs, typeof(C1.WPF.Chart.Finance.FibonacciArcs), "Fibonacci Arcs",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("StartX", typeof(int)),
                    new Data.PropertyParam("EndX", typeof(int)),
                    new Data.PropertyParam("StartY", typeof(int)),
                    new Data.PropertyParam("EndY", typeof(int)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush)),
                }
            );
            BindingSettingsParams(fans, typeof(C1.WPF.Chart.Finance.FibonacciFans), "Fibonacci Fans",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("StartX", typeof(int)),
                    new Data.PropertyParam("EndX", typeof(int)),
                    new Data.PropertyParam("StartY", typeof(int)),
                    new Data.PropertyParam("EndY", typeof(int)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush)),
                }
            );
            BindingSettingsParams(timeZones, typeof(C1.WPF.Chart.Finance.FibonacciTimeZones), "Fibonacci Time Zones",
                new Data.PropertyParam[]
                {
                    new Data.PropertyParam("StartX", typeof(int)),
                    new Data.PropertyParam("EndX", typeof(int)),
                    new Data.PropertyParam("Style.Stroke", typeof(Brush)),
                }
            );

            BindingSettingsParams(zigZag, typeof(C1.WPF.Chart.Finance.ZigZag), "ZigZag",
               new Data.PropertyParam[]
               {
                    new Data.PropertyParam("Distance", typeof(int)),
                   new Data.PropertyParam("Style.Stroke", typeof(Brush))
               }
           );

            InitAnnotition(financialChart);
        }
        
        private void BindingOverlays(C1.WPF.Chart.Series series, Data.OverlayType type)
        {
            Binding binding = new Binding();
            binding.Path = new PropertyPath("OverlayTypes");
            binding.Source = ViewModel.ViewModel.Instance;
            binding.Converter = new Converter.OverlayTypes2SeriesVisibilityConverter();
            binding.ConverterParameter = type;
            BindingOperations.SetBinding(series, C1.WPF.Chart.Finance.BollingerBands.VisibilityProperty, binding);
        }

        private void BindingSettingsParams(object source, Type srcType, string srckey, IEnumerable<Data.PropertyParam> properties)
        {
            Utilities.Helper.BindingSettingsParams(financialChart, source, srcType, srckey, properties);
        }

        internal void RangeSelectorChanged()
        {
            if (financialChart != null && Model.LowerValue != null && Model.UpperValue != null)
            {
                financialChart.BeginUpdate();

                IEnumerable<Object.QuoteRange> ranges = GetYRange(Model.LowerValue.Value, Model.UpperValue.Value);
                if (ranges != null && ranges.Any())
                {
                    if (financialChart != null)
                    {
                        App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            mainSeries.AxisY.Min
                            = financialChart.AxisY.Min
                            = ranges.Min(p => { return p == null ? int.MaxValue : p.Min; });
                            mainSeries.AxisY.Max
                            = financialChart.AxisY.Max
                            = ranges.Max(p => { return p == null ? int.MinValue : p.Max; });

                            IndicatorsRangeSelectorChanged();
                            if (arcs.Visibility == C1.Chart.SeriesVisibility.Visible)
                            {
                                Data.ArcsRange range = GetArcsRange(Model.LowerValue.Value, Model.UpperValue.Value);

                                arcs.StartX = range.StartX;
                                arcs.EndX = range.EndX;

                                arcs.StartY = range.StartY;
                                arcs.EndY = range.EndY;

                                IEnumerable<Data.SettingParam> settings;
                                if (ViewModel.ViewModel.Instance.Settings.TryGetValue("Fibonacci Arcs", out settings))
                                {
                                    Data.SettingParam param = (from p in settings where p.Key == "StartX" select p).FirstOrDefault();
                                    param.Value = arcs.StartX;
                                    param = (from p in settings where p.Key == "EndX" select p).FirstOrDefault();
                                    param.Value = arcs.EndX;

                                    param = (from p in settings where p.Key == "StartY" select p).FirstOrDefault();
                                    param.Value = arcs.StartY;
                                    param = (from p in settings where p.Key == "EndY" select p).FirstOrDefault();
                                    param.Value = arcs.EndY;
                                }
                            }
                            else if (fans.Visibility == C1.Chart.SeriesVisibility.Visible)
                            {
                                Data.ArcsRange range = GetArcsRange(Model.LowerValue.Value, Model.UpperValue.Value);

                                fans.StartX = range.EndX;
                                fans.EndX = range.StartX;

                                fans.StartY = range.EndY;
                                fans.EndY = range.StartY;

                                IEnumerable<Data.SettingParam> settings;
                                if (ViewModel.ViewModel.Instance.Settings.TryGetValue("Fibonacci Fans", out settings))
                                {
                                    Data.SettingParam param = (from p in settings where p.Key == "StartX" select p).FirstOrDefault();
                                    param.Value = fans.StartX;
                                    param = (from p in settings where p.Key == "EndX" select p).FirstOrDefault();
                                    param.Value = fans.EndX;

                                    param = (from p in settings where p.Key == "StartY" select p).FirstOrDefault();
                                    param.Value = fans.StartY;
                                    param = (from p in settings where p.Key == "EndY" select p).FirstOrDefault();
                                    param.Value = fans.EndY;
                                }
                            }

                        }));
                    }
                    var quote = ranges.First();
                }
                financialChart.EndUpdate();
            }
        }
        
        internal Data.ArcsRange GetArcsRange(double s, double e)
        {
            Data.ArcsRange range = new Data.ArcsRange();
            if (Model.CurrectQuote != null && Model.CurrectQuote.Data != null && Model.CurrectQuote.Data.Any())
            {
                IEnumerable<Object.QuoteData> quoteCache = Model.CurrectQuote.Data.Skip(Convert.ToInt32(s)).Take(Convert.ToInt32(e) - Convert.ToInt32(s));

                if (quoteCache.Any())
                {
                    range.EndY = quoteCache.Min(p => p.Close);
                    range.StartY = quoteCache.Max(p => p.Close);

                    range.StartX = s + quoteCache.TakeWhile(p => p.Close != range.StartY).Count();
                    range.EndX = s + quoteCache.TakeWhile(p => p.Close != range.EndY).Count();
                }
            }
            return range;
        }

        internal IEnumerable<Object.QuoteRange> GetYRange(double low, double high)
        {
            Queue<Object.QuoteRange> ranges = new Queue<Object.QuoteRange>();
            if (Model.CurrectQuote != null && Model.CurrectQuote.Data != null && Model.CurrectQuote.Data.Any())
            {
                var range = GetSymbolYRange(Model.CurrectQuote.Data, low, high);
                ranges.Enqueue(range);
            }

            if (bollinger.Visibility == C1.Chart.SeriesVisibility.Visible)
            {
                var range = GetOverlayYRange(bollinger.GetValues(0), bollinger.Period, low, high);
                ranges.Enqueue(range);
            }
            if (envelopes.Visibility == C1.Chart.SeriesVisibility.Visible)
            {
                var range = GetOverlayYRange(envelopes.GetValues(0), envelopes.Period, low, high);
                ranges.Enqueue(range);
            }

            if (pivotPoint.Visibility == C1.Chart.SeriesVisibility.Visible)
            {
                var range = pivotPoint.GetPivotPointsRange(low, high);
                ranges.Enqueue(range);
            }

            return ranges;
        }


        public Object.QuoteRange GetSymbolYRange(IEnumerable<Object.QuoteData> quoteData, double s, double e)
        {
            Object.QuoteRange qr = null;


            var start = Convert.ToInt32(s);
            var len = Convert.ToInt32(e) - start + 1;


            IEnumerable<Object.QuoteData> quoteCache = quoteData.Skip(start).Take(len);

            if (quoteCache.Any())
            {
                qr = new Object.QuoteRange();
                qr.Min = quoteCache.Min((k) =>
                {
                    return k.Low;
                });
                qr.Max = quoteCache.Max((k) =>
                {
                    return k.High;
                });
            }
            return qr;
        }

        public Object.QuoteRange GetOverlayYRange(double[] data, int period, double s, double e)
        {
            Object.QuoteRange qr = null;
            int halfLen = data.Length / 2;
            var dl1 = data.Take(halfLen);
            var dl2 = data.Skip(halfLen).Take(halfLen);

            var start = s < period ? 0 : s - period;

            var len = e - Math.Max(s, period);

            IEnumerable<double> quoteCache1 = dl1.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(len));
            IEnumerable<double> quoteCache2 = dl2.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(len));

            var quoteCache = quoteCache1.Union(quoteCache2);

            if (quoteCache.Any())
            {
                qr = new Object.QuoteRange();
                qr.Min = quoteCache.Min();
                qr.Max = quoteCache.Max();
            }
            return qr;
        }



        private void InitAnnotition(C1.WPF.Chart.C1FlexChart chart)
        {
            #region Annotations Setup

            AL.PolygonAddFunc = (pt) =>
            {
                return new C1.WPF.Chart.Annotation.Polygon("Polygon")
                {
                    Points =
                    {
                        pt,pt,pt
                    }
                };
            };
            chart.Layers.Add(AL);

            AL.PolygonReSizeFunc = (poly, rectangle) =>
            {
                var top = new Point((float)(rectangle.Left + rectangle.Width / 2), rectangle.Y);
                var left = new Point(rectangle.Left, rectangle.Bottom);
                var right = new Point(rectangle.Right, rectangle.Bottom);
                poly.Points[0] = EditableAnnotitions.Helpers.CoordsToAnnoPoint(chart, poly, top);
                poly.Points[1] = EditableAnnotitions.Helpers.CoordsToAnnoPoint(chart, poly, left);
                poly.Points[2] = EditableAnnotitions.Helpers.CoordsToAnnoPoint(chart, poly, right);
            };

            AL.ContentEditor = new EditableAnnotitions.AnnotationEditor();
            #endregion

            AL.Attachment = C1.Chart.Annotation.AnnotationAttachment.DataCoordinate;

            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "NewAnnotationType")
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (ViewModel.ViewModel.Instance.NewAnnotationType == StockAnalysis.Data.NewAnnotationType.None)
                        {
                            AL.AllowAdd = false;
                        }
                        else
                        {
                            AL.AllowAdd = true;
                            switch (ViewModel.ViewModel.Instance.NewAnnotationType)
                            {
                                case Data.NewAnnotationType.Circle:
                                    AL.NewAnnotationType = typeof(C1.WPF.Chart.Annotation.Circle);
                                    break;
                                case Data.NewAnnotationType.Ellipse:
                                    AL.NewAnnotationType = typeof(C1.WPF.Chart.Annotation.Ellipse);
                                    break;
                                case Data.NewAnnotationType.Line:
                                    AL.NewAnnotationType = typeof(C1.WPF.Chart.Annotation.Line);
                                    break;
                                case Data.NewAnnotationType.Polygon:
                                    AL.NewAnnotationType = typeof(C1.WPF.Chart.Annotation.Polygon);
                                    break;
                                case Data.NewAnnotationType.Rectangle:
                                    AL.NewAnnotationType = typeof(C1.WPF.Chart.Annotation.Rectangle);
                                    break;
                                case Data.NewAnnotationType.Square:
                                    AL.NewAnnotationType = typeof(C1.WPF.Chart.Annotation.Square);
                                    break;
                                case Data.NewAnnotationType.Text:
                                    AL.NewAnnotationType = typeof(C1.WPF.Chart.Annotation.Text);
                                    break;
                                case Data.NewAnnotationType.None:
                                default:
                                    AL.AllowAdd = false;
                                    break;
                            }
                        }
                    }));

                }
            };
        }

    }
}
