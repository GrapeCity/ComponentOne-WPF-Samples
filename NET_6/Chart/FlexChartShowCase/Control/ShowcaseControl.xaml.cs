using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using C1.Chart;
using C1.WPF.Chart;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using C1.WPF.Chart.Interaction;
using System.Windows.Documents;
using System.Windows.Media;
using C1.Chart.Annotation;
using C1.WPF.Chart.Annotation;
using System.Windows.Input;
using FlexChartShowcase.Resources;

namespace FlexChartShowcase
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowcaseControl : UserControl
    {
        Random rnd = new Random();
        IRenderEngine _engine;
        private ContextMenu _flexChartContextMenu;
        private List<CityDataItem> _data;
        private Axis xAxis;
        private C1LineMarker lineMarker;
        private AnnotationLayer annotationLayer;

        public ShowcaseControl()
        {
            InitializeComponent();
            Tag = AppResources.Desc;

            GetTemperatureData(new string[] { "Tokyo", "London" }, true, 40, true);
            SetupChart();
            checkMultiplePlots.IsChecked = true;

            cbPalette.ItemsSource = Enum.GetNames(typeof(Palette)).ToList();
            //cbPalette.SetBinding(ComboBox.SelectedValueProperty, new Binding() { ElementName = "flexChart", Path = new PropertyPath("Palette"), Mode = BindingMode.TwoWay, Converter = new EnumConverter() });
        }

        private void FlexChart_Rendered(object sender, RenderEventArgs e)
        {
            SetupAxisScrollbar();
        }

        private void SetupAxisScrollbar()
        {

            if (xAxis.Scrollbar != null)
                return;
            xAxis.Scrollbar = new C1AxisScrollbar();
            xAxis.Scrollbar.LowerValue = xAxis.Scrollbar.UpperValue - 100;
        }

        private void SetupChart()
        {
            flexChart.ChartType = ChartType.Column;
            flexChart.BindingX = "Date";
            flexChart.Background = new SolidColorBrush(Colors.White);

            // setup common X Axis
            xAxis = new Axis
            {
                Format = "d MMM",
                GroupProvider = new MyDateTimeGroupProvider(),
                GroupSeparator = AxisGroupSeparator.Horizontal
            };

            flexChart.Header = "Monthly Temperature (°F)";
            flexChart.HeaderStyle = new ChartStyle
            {
                FontSize = 25
            };
            //flexChart.Footer = "";
            flexChart.SelectionMode = ChartSelectionMode.Series;
            flexChart.LegendToggle = true;

            flexChart.ToolTip = null;


            annotationLayer = new AnnotationLayer();
            flexChart.Layers.Add(annotationLayer);

            lineMarker = new C1LineMarker
            {
                LineWidth = 2,
                LineBrush = new SolidColorBrush(Colors.Gray)
            };
            lineMarker.PositionChanged += OnLineMarkerPositionChanged;
            flexChart.Layers.Add(lineMarker);

            flexChart.Rendered += FlexChart_Rendered;
            flexChart.Rendering += FlexChart_Rendering;

            flexChart.AnimationSettings = AnimationSettings.Load;
            flexChart.AnimationLoad.Type = AnimationType.Series;

            //flexChart.Palette = Palette.Cerulean;

            flexChart.LegendGroupHeaderStyle = new ChartStyle
            {
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Stroke = new SolidColorBrush(Color.FromRgb(82, 82, 82))
            };
        }

        private void SetupPlotAreas(bool isMultiplePlots)
        {
            flexChart.PlotAreas.Clear();
            flexChart.Series.Clear();

            //setup common axes for shared plot
            //Left
            var yAxis = new Axis
            {
                AxisLine = false,
                Position = Position.Auto,
                MajorGrid = false,
                MajorTickMarks = TickMark.None,
                Title = "Temperature (°F)"
            };
            //Right
            var yAxis2 = new Axis
            {
                AxisLine = false,
                Position = Position.Right,
                MajorGrid = false,
                MajorTickMarks = TickMark.None,
                Title = "Precipitation"
            };

            // trend style
            ChartStyle trendStyle = new ChartStyle
            {
                StrokeDashArray = new DoubleCollection() { 3, 5 }
            };

            foreach (var item in _data)
            {
                if (isMultiplePlots)
                {
                    var plotArea = new PlotArea
                    {
                        PlotAreaName = item.Name,

                        Row = _data.IndexOf(item),
                    };
                    flexChart.PlotAreas.Add(plotArea);

                    var tempAxis = new Axis
                    {
                        AxisLine = false,
                        Position = Position.Auto,
                        PlotAreaName = item.Name,
                        MajorGrid = false,
                        MajorTickMarks = TickMark.None,
                        Title = "Temperature (°F)",
                    };
                    var precipAxis = new Axis
                    {
                        AxisLine = false,
                        Position = Position.Right,
                        MajorGrid = false,
                        MajorTickMarks = TickMark.None,
                        PlotAreaName = item.Name,
                        Title = "Precipitation",
                    };
                    var series = new Series
                    {
                        SeriesName = item.Name,
                        ChartType = ChartType.LineSymbols,
                        Binding = "HighTemp",
                        ItemsSource = item.Data,
                        LegendGroup = "Temperature",
                        AxisY = tempAxis,
                        AxisX = xAxis,
                    };
                    flexChart.Series.Add(series);

                    var series2 = new Series
                    {
                        SeriesName = item.Name,

                        Binding = "Precipitation",
                        ItemsSource = item.Data,
                        AxisY = precipAxis,
                        LegendGroup = "Precipitation",
                        AxisX = xAxis,

                    };
                    flexChart.Series.Add(series2);

                    var trend = new TrendLine
                    {
                        SeriesName = item.Name + " Trend",
                        FitType = FitType.Linear,
                        Binding = "HighTemp",
                        ItemsSource = item.Data,
                        AxisY = tempAxis,
                        LegendGroup = "Temperature",
                        AxisX = xAxis,
                        Style = trendStyle,
                    };
                    flexChart.Series.Add(trend);
                }
                else
                {
                    var plotArea = new PlotArea
                    {
                        PlotAreaName = item.Name
                    };
                    flexChart.PlotAreas.Add(plotArea);

                    var series = new Series
                    {
                        SeriesName = item.Name,
                        ChartType = ChartType.LineSymbols,
                        Binding = "HighTemp",
                        ItemsSource = item.Data,
                        LegendGroup = "Temperature",
                        AxisY = yAxis,
                        AxisX = xAxis,
                    };
                    flexChart.Series.Add(series);

                    var series2 = new Series
                    {
                        SeriesName = item.Name,

                        Binding = "Precipitation",
                        ItemsSource = item.Data,
                        AxisY = yAxis2,
                        AxisX = xAxis,
                        LegendGroup = "Precipitation",

                    };
                    flexChart.Series.Add(series2);

                    var trend = new TrendLine
                    {
                        SeriesName = item.Name + " Trend",
                        FitType = FitType.Linear,
                        Binding = "HighTemp",
                        ItemsSource = item.Data,
                        AxisY = yAxis,
                        LegendGroup = "Temperature",
                        AxisX = xAxis,
                        Style = trendStyle,
                    };
                    flexChart.Series.Add(trend);
                }
            }
        }

        private void FlexChart_Rendering(object sender, RenderEventArgs e)
        {
            if (_engine == null)
            {
                _engine = e.Engine;
                SetupAnnotations();
            }
        }

        void SetupAnnotations()
        {
            int index = 0;
            foreach (var item in _data)
            {
                //Min temperature
                double minTemp = item.Data.Min(t => t.HighTemp);
                int minIndex = item.Data.FindIndex(t => t.HighTemp == minTemp);
                var minPoints = new PointCollection()
                {
                    new Point{X = -10, Y = -5},
                    new Point{X = 135, Y = -5},
                    new Point{X = 135, Y = 60},
                    new Point{X = 70, Y = 60},
                    new Point{X = 60, Y = 75},
                    new Point{X = 50, Y = 60},
                    new Point{X = -10, Y = 60}
                };

                SetupAnnotationData(minTemp, minIndex, index, item, minPoints);

                //Max temperature
                double maxTemp = item.Data.Max(t => t.HighTemp);
                int maxIndex = item.Data.FindIndex(t => t.HighTemp == maxTemp);
                var maxPoints = new PointCollection()
                {
                    new Point{X = -10, Y = -5},
                    new Point{X = 50, Y = -5},
                    new Point{X = 60, Y = -15},
                    new Point{X = 70, Y = -5},
                    new Point{X = 135, Y = -5},
                    new Point{X = 135, Y = 60},
                    new Point{X = -10, Y = 60}
                };
                SetupAnnotationData(maxTemp, maxIndex, index, item, maxPoints, false);
                index += 3;
            }
        }

        private void SetupAnnotationData(double temp, int itemIndex, int index, CityDataItem item, PointCollection points, bool isMinAnnotation = true)
        {
            //Create canvas
            var canvasFactory = CreateCanvasFactory();

            //Polygon
            AddPolygonFactory(canvasFactory, points);

            //Create Stack Panel Factory
            var spFactory = CreateStackPanelFactory();
            //Temperature
            AddTemperatureFactory(spFactory, temp);
            //Text Block Temperature
            AddTextBlockFactory(spFactory, isMinAnnotation ? "Minimum Temperature" : "Maximum Temperature");
            //Item Name
            AddItemFactory(spFactory, item);
            canvasFactory.AppendChild(spFactory);

            //Create Data Template
            var dataTemplate = new DataTemplate
            {
                VisualTree = canvasFactory
            };
            dataTemplate.Seal();

            var serie = flexChart.Series[index];
            var xVals = serie.GetValues(1);
            var yVals = serie.GetValues(0);

            var locationX = xVals == null ? serie.AxisX.Convert(itemIndex) : serie.AxisX.Convert(xVals[itemIndex]);
            var locationY = yVals == null ? serie.AxisY.Convert(itemIndex) : serie.AxisY.Convert(yVals[itemIndex]);
            if (isMinAnnotation)
            {
                locationX -= 60;
                locationY -= 80;
            }
            else
            {
                locationX -= 60;
                locationY += 20;
            }
            //Create TemplateAnnotation
            var annoTemplate = new Template
            {
                Style = new ChartStyle()
                {
                    Fill = new SolidColorBrush(Colors.White),
                    Stroke = new SolidColorBrush(Colors.Gray)
                },
                Attachment = AnnotationAttachment.Absolute,
                SeriesIndex = index,
                PointIndex = itemIndex,
                AnnoTemplate = dataTemplate,
                Location = new Point(locationX, locationY)
            };

            annotationLayer.Annotations.Add(annoTemplate);
        }

        private FrameworkElementFactory CreateStackPanelFactory()
        {
            var spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.SetValue(StackPanel.OrientationProperty, System.Windows.Controls.Orientation.Vertical);
            return spFactory;
        }
        private FrameworkElementFactory CreateCanvasFactory()
        {
            var canvasFactory = new FrameworkElementFactory(typeof(Canvas));
            return canvasFactory;
        }
        private void AddTemperatureFactory(FrameworkElementFactory parent, double temp)
        {
            var tbFactory = new FrameworkElementFactory(typeof(TextBlock));
            tbFactory.SetValue(TextBlock.TextProperty, string.Format("{0}°F", temp.ToString("n0")));
            tbFactory.SetValue(TextBlock.FontSizeProperty, 16.0);
            tbFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            tbFactory.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Color.FromRgb(76, 76, 76)));
            parent.AppendChild(tbFactory);
        }
        private void AddTextBlockFactory(FrameworkElementFactory parent, string content)
        {
            var tbFactory = new FrameworkElementFactory(typeof(TextBlock));
            tbFactory.SetValue(TextBlock.TextProperty, content);
            tbFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            tbFactory.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Color.FromRgb(109, 109, 109)));
            parent.AppendChild(tbFactory);
        }
        private void AddItemFactory(FrameworkElementFactory parent, CityDataItem item)
        {
            var itemFactory = new FrameworkElementFactory(typeof(TextBlock));
            itemFactory.SetValue(TextBlock.TextProperty, item.Name.ToUpper());
            itemFactory.SetValue(TextBlock.FontSizeProperty, 13.0);
            itemFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            itemFactory.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Color.FromRgb(186, 28, 245)));
            parent.AppendChild(itemFactory);
        }

        private void AddPolygonFactory(FrameworkElementFactory parent, PointCollection points)
        {
            var polygonFactory = new FrameworkElementFactory(typeof(System.Windows.Shapes.Polygon));
            //rect.SetValue(System.Windows.Shapes.Polygon.NameProperty, "TestRect");
            polygonFactory.SetValue(System.Windows.Shapes.Polygon.StrokeProperty, new SolidColorBrush(Colors.Gray));
            polygonFactory.SetValue(System.Windows.Shapes.Polygon.FillProperty, new SolidColorBrush(Colors.White));
            polygonFactory.SetValue(System.Windows.Shapes.Polygon.StrokeThicknessProperty, 1.0);
            polygonFactory.SetValue(System.Windows.Shapes.Polygon.PointsProperty, points);

            parent.AppendChild(polygonFactory);
        }

        private void OnLineMarkerPositionChanged(object sender, PositionChangedArgs e)
        {
            if (flexChart != null)
            {
                var info = flexChart.HitTest(new Point(e.Position.X, double.NaN));

                if (info.PointIndex != -1)
                {
                    var doc = new FlowDocument();
                    var graph = new Paragraph { LineHeight = 20, Padding = new Thickness(5) };
                    graph.Inlines.Add(new Bold(new Run(string.Format("{0:MMMM yyyy}\n", info.X))));

                    foreach (var item in _data)
                    {
                        var temp = GetTemperatureValueAt(item.Name, (DateTime)info.X);
                        if (temp != null)
                        {
                            graph.Inlines.Add(new Run()
                            {
                                Text = string.Format("{0}: {1}°F, Precip: {2} inches\n", item.Name, temp.HighTemp.ToString(), temp.Precipitation.ToString())
                            });
                        }
                    }
                    doc.Blocks.Add(graph);
                    var rtb = new RichTextBox { Document = doc, Width = 220, Height = 70 };
                    lineMarker.Content = rtb;
                }
            }
        }

        private Temperature GetTemperatureValueAt(string city, DateTime date)
        {
            var cityItem = _data.Find(c => c.Name.Equals(city));
            var dataItem = cityItem.Data.Find(t => t.Date.Year == date.Year && t.Date.Month == date.Month);
            return dataItem;
        }

        public void GetTemperatureData(string[] cities, bool monthly = false, int count = 30, bool isFahrenheit = false)
        {
            _data = new List<CityDataItem>();
            var startDate = new DateTime(2019, 1, 1);
            foreach (string city in cities)
            {
                var dataItem = new CityDataItem() { Name = city };
                for (int i = 0; i < count; i++)
                {
                    var temp = new Temperature();
                    DateTime date;
                    if (monthly)
                        date = startDate.AddMonths(i);
                    else
                        date = startDate.AddDays(i);
                    temp.Date = date;
                    if (date.Month <= 8)
                    {
                        temp.HighTemp = rnd.Next(3 * date.Month, 8 * date.Month);
                    }
                    else
                    {
                        temp.HighTemp = rnd.Next((13 - date.Month - 2) * date.Month, (13 - date.Month) * date.Month);
                    }
                    temp.Precipitation = (date.Month < 4 || date.Month > 8) ? rnd.Next(100, 150) : rnd.Next(150, 200);
                    if (isFahrenheit) temp.HighTemp = temp.HighTemp * 1.8 + 32;
                    dataItem.Data.Add(temp);
                }
                _data.Add(dataItem);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //GetTemperatureData(new string[] { "Tokyo", "London" }, true, 40, true);
            checkMultiplePlots.IsChecked = false;
            checkMultiplePlots.IsChecked = true;

            annotationLayer.Annotations.Clear();

            SetupAnnotations();
            flexChart.Invalidate();
            flexChart.InvalidateArrange();
            flexChart.InvalidateMeasure();
            flexChart.InvalidateVisual();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog()
            {
                FileName = DateTime.Now.Ticks.ToString(),
                Filter = "PNG|*.png|JPEG |*.jpeg|BMP|*.bmp|SVG|*.svg"
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    var extension = dialog.SafeFileName.Split('.')[1];
                    ImageFormat fmt = (ImageFormat)Enum.Parse(typeof(ImageFormat), extension, true);
                    flexChart.SaveImage(stream, fmt);
                }
            }
        }

        private void checkMultiplePlots_Checked(object sender, RoutedEventArgs e)
        {
            SetupPlotAreas((bool)checkMultiplePlots.IsChecked);
        }
    }

    /// <summary>
    /// Provides basic groups for DateTime values that extend beyond one month.  Although
    /// default groups are provided, explicit selection of the groups and format are possible.
    /// </summary>
    public class MyDateTimeGroupProvider : IAxisGroupProvider
    {
        ObservableCollection<TimeUnits> _groupTypes = new ObservableCollection<TimeUnits>();
        Dictionary<TimeUnits, string> _groupFormats = new Dictionary<TimeUnits, string>();
        Axis _groupAxis = null;

        /// <summary>
        /// This contstructor allows automatic generation of groups based upon the axis range.
        /// </summary>
        public MyDateTimeGroupProvider() { }

        /// <summary>
        /// This constructor allows automatic generation of groups based upon the axis range
        /// and the axis MajorUnit value.
        /// </summary>
        /// <param name="axis"></param>
        public MyDateTimeGroupProvider(Axis axis)
        {
            _groupAxis = axis;
        }

        /// <summary>
        /// As the first method called by FlexChart, this method provides the number of group
        /// levels provided by the class.
        /// </summary>
        /// <param name="range">
        /// Specifies the full range of the axis data.
        /// </param>
        /// <returns>Specifies number of group levels.</returns>
        public int GetLevels(IRange range)
        {
            // Base default groups on the specified range.
            // Require group is larger than the MajorUnit, has more than one member
            // and does not have too many members that they do not fit along the axis.
            if (_groupTypes.Count == 0 && range != null)
            {
                double majorUnitDays = (range.Max - range.Min) / 12;    // approximate if MajorUnit = NaN

                // if there are MajorUnit values, then request groupings with larger intervals
                if (_groupAxis != null && !double.IsNaN(_groupAxis.MajorUnit)) majorUnitDays =
                    (new double[] { 1, 7, 30, 365 })[(int)_groupAxis.TimeUnit] * _groupAxis.MajorUnit;

                List<DateTime> dates = new List<DateTime>();
                List<string> labels = new List<string>();

                DateTime start = DateTime.FromOADate(range.Min);
                DateTime end = DateTime.FromOADate(range.Max);

                // only default to most commonly desired groups.
                TimeUnits[] tu = { TimeUnits.Month, TimeUnits.Year };
                double[] daysPerGroupUnit = { 30, 365 };

                for (int i = 0; i < tu.Length; i++)
                {
                    if (majorUnitDays < daysPerGroupUnit[i])
                    {
                        CreateTimeValues(tu[i], start, end, 1, dates, labels);
                        if (dates.Count > 1 && dates.Count < 15) _groupTypes.Add(tu[i]);
                        dates.Clear(); labels.Clear();
                    }
                }
            }
            return _groupTypes.Count;
        }

        /// <summary>
        /// Gets the collection of group specifiers using the TimeUnits enumeration.  The index of each
        /// specifier indicates (level-1).  Specifiers modified using the collection Add, Insert and Remove
        /// methods of the collection.  If no values are specified, values are automatically selected
        /// based on the range.
        /// </summary>
        public ObservableCollection<TimeUnits> GroupTypes
        {
            get { return _groupTypes; }
        }

        /// <summary>
        /// Get a dictionary of formats keyed by the TimeUnits enum of each group.  Each value specifies
        /// the string.Format() of the numeric value followed by the year, with the exception of TimeUnits.Month
        /// for which the format specifies either all "M" characters (for the month name) or the numeric
        /// value of the month and year. Note if {1} is not included in the format, the year is not included.
        /// 
        /// Default formats are Day="{0}", Week="Week {0}, Month="MMM", Quarter="Q{0}", "Year="{0}".
        /// </summary>
        public Dictionary<TimeUnits, string> GroupFormats
        {
            get { return _groupFormats; }
        }

        /// <summary>
        /// Returns a list of IRange values for the level specified the by the appropriate
        /// entry in the GroupTypes collection.
        /// </summary>
        /// <param name="range">
        /// Specifies the full range of the axis.
        /// </param>
        /// <param name="level">
        /// Specifies the level of IRange values for the level specified by the appropriate
        /// entry in the GroupTypes collection.
        /// </param>
        /// <returns></returns>
        public IList<IRange> GetRanges(IRange range, int level)
        {
            if (level - 1 < 0 || level - 1 > _groupTypes.Count)
                return null;        // invalid group request

            var timeRange = range as TimeRange;
            if (timeRange == null)
                return null;

            var min = timeRange.TimeMin;
            var max = timeRange.TimeMax;
            var span = max - min;

            if (span.TotalDays < 1)
                return null;

            List<DateTime> dates = new List<DateTime>();
            List<string> labels = new List<string>();
            TimeUnits mtu = _groupTypes[level - 1];
            CreateTimeValues(mtu, min, max, 1, dates, labels);

            List<IRange> ranges = new List<IRange>();
            for (int d = 1; d < dates.Count; d++)
            {
                TimeRange tr = new TimeRange(labels[d - 1], dates[d - 1], dates[d]);
                ranges.Add(tr);
            }
            return ranges;
        }

        private void CreateTimeValues(TimeUnits mtu, DateTime startDate, DateTime endDate, int delta, List<DateTime> dates, List<string> labels)
        {
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            string fmt = _groupFormats.ContainsKey(mtu) ? _groupFormats[mtu] : null;
            string label = null;

            DateTime currentDate = GetTimeUnit(mtu, startDate, 0, fmt, out label);
            while (currentDate <= endDate)
            {
                dates.Add(currentDate);
                labels.Add(label);
                currentDate = GetTimeUnit(mtu, currentDate, delta, fmt, out label);
            }

            if (currentDate > endDate)
            {
                dates.Add(endDate);
                labels.Add(label);
            }
        }

        private DateTime GetTimeUnit(TimeUnits mtu, DateTime dt, int delta, string fmt, out string label)
        {
            DateTime dtn = new DateTime(dt.Year, dt.Month, dt.Day);
            label = null;
            int unit = 0;

            switch (mtu)
            {
                case TimeUnits.Day:
                    dtn = dtn.AddDays(delta);
                    label = fmt == null ? dt.Day.ToString() : string.Format(fmt, dtn.Day, dtn.Year);
                    break;

                case TimeUnits.Month:
                    // first day of the month
                    dtn = dtn.AddDays(1 - dtn.Day).AddMonths(delta);
                    if (fmt == null)
                        label = dtn.ToString("MMM");
                    else if (string.IsNullOrEmpty(fmt.Replace("M", "")))
                        label = dtn.ToString(fmt);
                    else
                        label = string.Format(fmt, dtn.Month, dtn.Year);
                    break;

                case TimeUnits.Quarter:
                    // first day of the quarter
                    unit = (dtn.Month + 2) / 3;
                    dtn = dtn.AddDays(1 - dtn.DayOfYear).AddMonths(((unit - 1 + delta) * 3));
                    unit = (dtn.Month + 2) / 3;
                    label = (fmt == null) ? "Q" + unit.ToString() : string.Format(fmt, unit, dtn.Year);
                    break;

                case TimeUnits.Week:
                    // first day of the week
                    unit = (dtn.DayOfYear + 6) / 7;
                    dtn = dtn.AddDays(1 - dtn.DayOfYear).AddDays((unit - 1 + delta) * 7);
                    unit = (dtn.DayOfYear + 6) / 7;
                    label = (fmt == null) ? "Week" + unit.ToString() : string.Format(fmt, unit, dtn.Year);
                    break;

                case TimeUnits.Year:
                    // first day of the year
                    dtn = dtn.AddDays(1 - dtn.DayOfYear).AddYears(delta);
                    label = (fmt == null) ? dtn.Year.ToString() : string.Format(fmt, dtn.Year);
                    break;
            }
            return dtn;
        }
    }

    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return Enum.Parse(targetType, value.ToString());
        }
    }

    public class Temperature
    {
        public DateTime Date { get; set; }
        public double HighTemp { get; set; }
        public double Precipitation { get; set; }
    }
    public class CityDataItem
    {
        private List<Temperature> _data;
        public string Name { get; set; }
        public List<Temperature> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<Temperature>();
                }
                return _data;
            }
        }
    }
}
