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
    /// Interaction logic for KRChart.xaml
    /// </summary>
    public partial class KRChart : UserControl
    {
        public KRChart()
        {
            InitializeComponent();
            InitContextMenu();

            InitAnnotition(financialChart);
            
            this.financialChart.Loaded += (o, e) =>
            {
                if (AL != null)
                {
                    AL.ClientAreaHeight = this.financialChart.PlotRect.Top + this.financialChart.PlotRect.Height;
                }
            };
            this.financialChart.SizeChanged += (o, e) =>
            {
                if (AL != null)
                {
                    AL.ClientAreaHeight = this.financialChart.PlotRect.Top + this.financialChart.PlotRect.Height;
                }
            };

            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "ChartType")
                {
                    if (ViewModel.ViewModel.Instance.ChartType == FinancialChartType.Kagi ||
                               ViewModel.ViewModel.Instance.ChartType == FinancialChartType.Renko ||
                               ViewModel.ViewModel.Instance.ChartType == FinancialChartType.PointAndFigure)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.financialChart.ChartType = ViewModel.ViewModel.Instance.ChartType;

                            if (this.financialChart.ChartType == FinancialChartType.PointAndFigure)
                            {
                                financialSeries.Style = new C1.WPF.Chart.ChartStyle();
                                financialSeries.AltStyle = new C1.WPF.Chart.ChartStyle();
                                financialSeries.Style.Stroke = new SolidColorBrush(Colors.Black);
                                financialSeries.AltStyle.Stroke = new SolidColorBrush(Colors.Red);
                                
                            }
                            else
                             {
                                financialSeries.Style.Stroke = new SolidColorBrush(Color.FromArgb(255, 51, 103, 214));
                                financialSeries.Style.Fill = new SolidColorBrush(Color.FromArgb(128, 66, 133, 244));
                            }


                            IEnumerable<Data.SettingParam> settings;
                            if (settingsCache.TryGetValue(financialChart.ChartType, out settings))
                            {
                                var reversalSetting = (from p in settings where p.Key == "Reversal" select p).FirstOrDefault();
                                if (reversalSetting != null)
                                {
                                    var value = System.Convert.ToInt32(reversalSetting.Value);
                                    this.financialChart.Options.ReversalAmount = value > 1 ? value : 1;
                                }
                            }

                            if (AL != null)
                            {
                                AL.Annotations.Clear();
                            }
                        }));
                    }

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
                    }));
                }
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitAndBindingData(FinancialChartType.Kagi);
            InitAndBindingData(FinancialChartType.Renko);
            InitAndBindingPointAndFigureData(FinancialChartType.PointAndFigure);
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

        Dictionary<FinancialChartType, IEnumerable<Data.SettingParam>> settingsCache = new Dictionary<FinancialChartType, IEnumerable<Data.SettingParam>>();
        
        private void InitAndBindingData(FinancialChartType type)
        {
            IEnumerable<Data.SettingParam> settings;
            if (ViewModel.ViewModel.Instance.Settings.TryGetValue(Enum.GetName(typeof(FinancialChartType), type), out settings))
            {
                var reversalSetting = (from p in settings where p.Key == "ReversalAmount" select p).FirstOrDefault();
                if (reversalSetting != null)
                {
                    var value = System.Convert.ToDouble(reversalSetting.Value);
                    this.financialChart.Options.ReversalAmount = value > 1 ? value : 1;
                    this.financialChart.Options.BoxSize = value > 1 ? value : 1;
                    reversalSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = System.Convert.ToDouble(reversalSetting.Value);
                            if(value > 0)
                            {
                                this.financialChart.Options.ReversalAmount = value;
                                this.financialChart.Options.BoxSize = value;
                            }
                        }
                    };
                }
                var rangeModeSetting = (from p in settings where p.Key == "RangeMode" select p).FirstOrDefault();
                if (rangeModeSetting != null)
                {
                    var value = (RangeMode)rangeModeSetting.Value;
                    this.financialChart.Options.RangeMode = value;
                    rangeModeSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = (RangeMode)rangeModeSetting.Value;
                            this.financialChart.Options.RangeMode = value;

                            if (value == RangeMode.Percentage)
                            {
                                this.financialChart.Options.ReversalAmount = 0.05;

                                reversalSetting.Type = typeof(double);
                                reversalSetting.Value = 0.05;
                                reversalSetting.Minimum = 0;
                            }
                            else
                            {
                                this.financialChart.Options.ReversalAmount = 2;

                                reversalSetting.Type = typeof(uint);
                                reversalSetting.Value = 2;
                                reversalSetting.Minimum = 1;
                            }
                        }
                    };
                }
                var dataFieldsSetting = (from p in settings where p.Key == "DataFields" select p).FirstOrDefault();
                if (dataFieldsSetting != null)
                {
                    var value = (DataFields)dataFieldsSetting.Value;
                    this.financialChart.Options.DataFields = value;
                    dataFieldsSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = (DataFields)dataFieldsSetting.Value;
                            this.financialChart.Options.DataFields = value;
                        }
                    };
                }
                settingsCache.Add(type, settings);
            }
        }

        private void InitAndBindingPointAndFigureData(FinancialChartType type)
        {
            IEnumerable<Data.SettingParam> settings;
            if (ViewModel.ViewModel.Instance.Settings.TryGetValue(Enum.GetName(typeof(FinancialChartType), type), out settings))
            {
                var reversalSetting = (from p in settings where p.Key == "ReversalAmount" select p).FirstOrDefault();
                if (reversalSetting != null)
                {
                    var value = System.Convert.ToDouble(reversalSetting.Value);
                    this.financialChart.Options.ReversalAmount = value > 1 ? value : 1;
                    reversalSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = System.Convert.ToDouble(reversalSetting.Value);
                            if (value > 0)
                            {
                                this.financialChart.Options.ReversalAmount = value;
                            }
                        }
                    };
                }
                var dataFieldsSetting = (from p in settings where p.Key == "DataFields" select p).FirstOrDefault();
                if (dataFieldsSetting != null)
                {
                    var value = (DataFields)dataFieldsSetting.Value;
                    this.financialChart.Options.DataFields = value;
                    dataFieldsSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = (DataFields)dataFieldsSetting.Value;
                            this.financialChart.Options.DataFields = value;
                        }
                    };
                }

                var scalingSetting = (from p in settings where p.Key == "Scaling" select p).FirstOrDefault();
                if (scalingSetting != null)
                {
                    var value = (C1.Chart.Finance.PointAndFigureScaling)scalingSetting.Value;
                    (this.financialChart.Options as C1.WPF.Chart.Finance.PointAndFigureOptions).Scaling = value;
                    scalingSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = (C1.Chart.Finance.PointAndFigureScaling)scalingSetting.Value;
                            (this.financialChart.Options as C1.WPF.Chart.Finance.PointAndFigureOptions).Scaling = value;
                        }
                    };
                }
                var boxSizeSetting = (from p in settings where p.Key == "BoxSize" select p).FirstOrDefault();
                if (boxSizeSetting != null)
                {
                    var value = System.Convert.ToDouble(boxSizeSetting.Value);
                    this.financialChart.Options.BoxSize = value > 1 ? value : 1;
                    boxSizeSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = System.Convert.ToDouble(boxSizeSetting.Value);
                            if (value > 0)
                            {
                                this.financialChart.Options.BoxSize = value;
                            }
                        }
                    };
                }
                var periodSetting = (from p in settings where p.Key == "Period" select p).FirstOrDefault();
                if (periodSetting != null)
                {
                    var value = System.Convert.ToInt32(periodSetting.Value);
                    (this.financialChart.Options as C1.WPF.Chart.Finance.PointAndFigureOptions).Period = value;
                    periodSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = System.Convert.ToInt32(periodSetting.Value);
                            if (value > 0)
                            {
                                (this.financialChart.Options as C1.WPF.Chart.Finance.PointAndFigureOptions).Period = value;
                            }
                        }
                    };
                }


                var squareGridSetting = (from p in settings where p.Key == "SquareGrid" select p).FirstOrDefault();
                if (squareGridSetting != null)
                {
                    var value = System.Convert.ToBoolean(squareGridSetting.Value);
                    (this.financialChart.Options as C1.WPF.Chart.Finance.PointAndFigureOptions).SquareGrid = value;
                    squareGridSetting.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == "Value" && financialChart.ChartType == type)
                        {
                            value = System.Convert.ToBoolean(squareGridSetting.Value);
                            (this.financialChart.Options as C1.WPF.Chart.Finance.PointAndFigureOptions).SquareGrid = value;
                        }
                    };
                }


                settingsCache.Add(type, settings);
            }
        }


        public ViewModel.ViewModel Model
        {
            get { return ViewModel.ViewModel.Instance; }
        }


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
                        if (ViewModel.ViewModel.Instance.NewAnnotationType == Data.NewAnnotationType.None)
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
