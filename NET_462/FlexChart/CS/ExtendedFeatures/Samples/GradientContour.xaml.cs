using C1.WPF.Chart.Extended;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExtendedFeatures.Samples
{
    public class ColorInfo
    {
        public Color Color { get; set; }
        public string Name { get; set; }

        public ColorInfo(Color color, string name)
        {
            Color = color;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Interaction logic for GradientContour.xaml
    /// </summary>
    public partial class GradientContour : UserControl
    {
        public ObservableCollection<ColorInfo> StrokeColor { get; set; }
        private Contour _contour;
        private List<int> palette = new List<int> { 0x67001f, 0xb2182b, 0xd6604d, 0xf4a582, 0xfddbc7, 0xf7f7f7, 0xd1e5f0, 0x92c5de, 0x4393c3, 0x2166ac, 0x053061 };

        public GradientContour()
        {
            InitializeComponent();
            InitializeStrokeColors();
            this.DataContext = this;
            
            // Do everything in UserControl.Loaded to ensure proper timing
            this.Loaded += (s, e) => 
            {
                InitializeChart();
                InitializeControls();
                
                if (StrokeColorComboBox.Items.Count > 0)
                {
                    StrokeColorComboBox.SelectedIndex = 0;
                }
            };
        }

        private void InitializeStrokeColors()
        {
            StrokeColor = new ObservableCollection<ColorInfo>
            {
                new ColorInfo(Colors.Black, "Black"),
                new ColorInfo(Colors.Red, "Red"),
                new ColorInfo(Colors.Blue, "Blue"),
                new ColorInfo(Colors.Green, "Green"),
                new ColorInfo(Colors.Orange, "Orange"),
                new ColorInfo(Colors.Purple, "Purple"),
                new ColorInfo(Colors.Brown, "Brown"),
                new ColorInfo(Colors.Gray, "Gray"),
                new ColorInfo(Colors.DarkBlue, "Dark Blue"),
                new ColorInfo(Colors.DarkRed, "Dark Red")
            };
        }

        private void InitializeChart()
        {
            if (chart.Series.Count > 0)
                return;

            chart.BeginUpdate();
            chart.RenderMode = C1.WPF.Chart.RenderMode.Direct2D;

            _contour = new Contour();
            var data = MonkeySaddleData();
            var scale = new GradientColorScale() { Min = 0, Max = 100 };
            scale.Colors = palette.Select(i => Color.FromArgb(255, (byte)(i >> 16), (byte)(i >> 8), (byte)i)).Reverse().ToList();

            _contour.ItemsSource = data;
            _contour.ColorScale = scale;

            chart.Series.Add(_contour);
            chart.AxisX.Max = data.GetLength(0) - 1;
            chart.AxisY.Max = data.GetLength(1) - 1;
            chart.DataLabel = null;
            chart.EndUpdate();
        }

        private void InitializeControls()
        {
            // Set default values for numeric controls using direct access
            ContourLinesNumericBox.Value = _contour.NumberOfLevels;
            ContourThicknessNumericBox.Value = _contour.Style.StrokeThickness;
            StrokePattern1NumericBox.Value = 1;
            StrokePattern2NumericBox.Value = 0;
        }

        private void ContourLinesNumericBox_ValueChanged(object sender, C1.WPF.PropertyChangedEventArgs<double> e)
        {
            if (_contour != null && e.NewValue != double.NaN)
            {
                _contour.NumberOfLevels = (int)e.NewValue;
                RefreshContour();
            }
        }

        private void StrokePatternNumericBox_ValueChanged(object sender, C1.WPF.PropertyChangedEventArgs<double> e)
        {
            if (_contour != null && StrokePattern1NumericBox != null && StrokePattern2NumericBox != null)
            {
                double value1 = StrokePattern1NumericBox.Value;
                double value2 = StrokePattern2NumericBox.Value;
                
                if (value1 > 0 || value2 > 0)
                {
                    _contour.Style.StrokeDashArray = new DoubleCollection(new double[] { value1, value2 });
                }
                else
                {
                    _contour.Style.StrokeDashArray = null; // Solid line
                }
                
                RefreshContour();
            }
        }

        private void StrokeColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_contour != null && sender is ComboBox comboBox && comboBox.SelectedItem is ColorInfo colorInfo)
            {
                _contour.Style.Stroke = new SolidColorBrush(colorInfo.Color);
                RefreshContour();
            }
        }

        private void ContourThicknessNumericBox_ValueChanged(object sender, C1.WPF.PropertyChangedEventArgs<double> e)
        {
            if (_contour != null && e.NewValue != double.NaN)
            {
                _contour.Style.StrokeThickness = (int)e.NewValue;
                RefreshContour();
            }
        }

        private void RefreshContour()
        {
            if (_contour != null)
            {
                chart.BeginUpdate();
                chart.Series.Clear();
                chart.Series.Add(_contour);
                chart.EndUpdate();
            }
        }

        private static double[,] MonkeySaddleData()
        {
            int width = 20;
            int height = 20;
            double[,] data = new double[width, height];
            double minValue = 0;
            double maxValue = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Center the coordinates
                    double xc = x - width / 2.0;
                    double yc = y - height / 2.0;

                    // Monkey saddle equation: z = x³ - 3xy²
                    data[x, y] = (xc * xc * xc - 3 * xc * yc * yc);

                    // set initial min and max value, can't be 0.
                    if (x == 0 && y == 0)
                    {
                        maxValue = data[0, 0];
                        minValue = data[0, 0];
                    }

                    // Track min and max values
                    minValue = Math.Min(minValue, data[x, y]);
                    maxValue = Math.Max(maxValue, data[x, y]);
                }
            }

            // normalize to range [0,100] 
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    data[x, y] = 100 * (data[x, y] - minValue) / (maxValue - minValue);

            return data;
        }

    }
}