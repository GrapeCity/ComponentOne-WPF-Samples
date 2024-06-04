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

namespace FlexSheetSample
{
    /// <summary>
    /// ColorSelector.xaml 
    /// </summary>
    public partial class ColorSelector : UserControl
    {
        public static readonly DependencyProperty CustomColorProperty =
      DependencyProperty.Register("CustomColor", typeof(Color), typeof(ColorSelector), new PropertyMetadata((Color)Colors.Transparent));
        
        public Color CustomColor
        {
            get { return (Color)GetValue(CustomColorProperty); }
            set
            {
                SetValue(CustomColorProperty, value);
            }
        }

        public ColorSelector()
        {
            InitializeComponent();
            InitialWork();
        }

        void txtAlpha_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text)) return;
                int val = Convert.ToInt32(((TextBox)sender).Text);
                if (val > 255)

                    ((TextBox)sender).Text = "255";
                else
                {
                    byte byteValue = Convert.ToByte(((TextBox)sender).Text);
                    CustomColor =
                       Color.FromArgb(
                            byteValue,
                           CustomColor.R,
                           CustomColor.G,
                           CustomColor.B);

                }
            }
            catch
            {
            }
        }

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }

        private void InitialWork()
        {
            DefaultPicker.Items.Clear();
            CustomColors customColors = new CustomColors();
            foreach (var item in customColors.SelectableColors)
            {
                DefaultPicker.Items.Add(item);
            }
            DefaultPicker.SelectionChanged += new SelectionChangedEventHandler(DefaultPicker_SelectionChanged);
        }

        public void UnSelectAll()
        {
            DefaultPicker.UnselectAll();
            CustomColor = Colors.Transparent;
        }

        public event SelectionChangedEventHandler SelectionChanged;
        protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            SelectionChangedEventHandler handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        void DefaultPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DefaultPicker.SelectedValue != null)
            {
                CustomColor = (Color)DefaultPicker.SelectedValue;
                OnSelectionChanged(e);
            }

            FrameworkElement frameworkElement = this;
            while (true)
            {
                if (frameworkElement == null) break;
                if (frameworkElement is ContextMenu)
                {
                    ((ContextMenu)frameworkElement).IsOpen = false;
                    break;
                }
                if (frameworkElement.Parent != null)
                    frameworkElement = (FrameworkElement)frameworkElement.Parent;
                else
                    break;
            }
        }

        private bool SimmilarColor(Color pointColor, Color selectedColor)
        {
            int diff = Math.Abs(pointColor.R - selectedColor.R) + Math.Abs(pointColor.G - selectedColor.G) + Math.Abs(pointColor.B - selectedColor.B);
            if (diff < 20) return true;
            else
                return false;
        }

        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }

    class CustomColors
    {
        List<Color> _SelectableColors = null;

        public List<Color> SelectableColors
        {
            get { return _SelectableColors; }
            set { _SelectableColors = value; }
        }

        public CustomColors()
        {
            _SelectableColors = new List<Color>();
            var list = GenerateLuminance(new[] 
                {
                    // office color palette
                    Rgb(0xFFFFFF),
                    Rgb(0x000000),
                    Rgb(0xEEECE1),
                    Rgb(0x1F497D),
                    Rgb(0x4F81BD),
                    Rgb(0xC0504D),
                    Rgb(0x9BBB59),
                    Rgb(0x8064A2),
                    Rgb(0x4BACC6),
                    Rgb(0xF79646),               
                }).Concat(new[]
                {
                    // standard color palette
                    Rgb(0xC00000),
                    Rgb(0xFF0000),
                    Rgb(0xFFC000),
                    Rgb(0xFFFF00),
                    Rgb(0x92D050),
                    Rgb(0x00B050),
                    Rgb(0x00B0F0),
                    Rgb(0x0070C0),
                    Rgb(0x002060),
                    Rgb(0x7030A0)
                });
            _SelectableColors.AddRange(list);
        }

        static Color Rgb(int rgb)
        {
            byte r = (byte)((rgb & 0x00FF0000) >> 16);
            byte g = (byte)((rgb & 0x0000FF00) >> 8);
            byte b = (byte)((rgb & 0x000000FF));

            return Color.FromArgb(255, r, g, b);
        }

        static Color ChangeColorLuminance(Color rgbColor, double newLum)
        {
            double r = rgbColor.R;
            double g = rgbColor.G;
            double b = rgbColor.B;
            double min = r < g ? (r < b ? r : b) : (g < b ? g : b);
            double max = r > g ? (r > b ? r : b) : (g > b ? g : b);
            double h = 0;
            if (max == min)
                h = 240;
            else if (max == r)
            {
                h = 60 * (g - b) / (max - min);
                if (g < b)
                    h += 360;
            }
            else if (max == g)
                h = 60 * (b - r) / (max - min) + 120;
            else if (max == b)
                h = 60 * (r - g) / (max - min) + 240;
            double l = ((double)(min + max)) / 510;
            double s;
            if (max == min)
                s = 0;
            else if (l <= 0.5)
                s = ((double)(max - min)) / (max + min);
            else // if (_l > 0.5f)
                s = ((double)(max - min)) / (510 - max - min);
            h /= 360;

            l = newLum / 255.0;

            double q = l < 0.5 ? l * (s + 1.0) : l + s - (l * s);
            double p = l + l - q;
            double tr = h + 1 / 3.0;
            if (tr > 1.0)
                tr -= 1.0;
            double tg = h;
            double tb = h - 1 / 3.0;
            if (tb < 0.0)
                tb += 1.0;
            if (tr < 1 / 6.0)
                tr = (q - p) * tr * 6 + p;
            else if (tr < 0.5)
                tr = q;
            else if (tr < 2 / 3.0)
                tr = (q - p) * (2 / 3.0 - tr) * 6 + p;
            else
                tr = p;
            if (tg < 1 / 6.0)
                tg = (q - p) * tg * 6 + p;
            else if (tg < 0.5)
                tg = q;
            else if (tg < 2 / 3.0)
                tg = (q - p) * (2 / 3.0 - tg) * 6 + p;
            else
                tg = p;
            if (tb < 1 / 6.0)
                tb = (q - p) * tb * 6 + p;
            else if (tb < 0.5)
                tb = q;
            else if (tb < 2 / 3.0)
                tb = (q - p) * (2 / 3.0 - tb) * 6 + p;
            else
                tb = p;

            return Color.FromArgb(255, Convert.ToByte(tr * 255), Convert.ToByte(tg * 255), Convert.ToByte(tb * 255));
        }

        static IEnumerable<Color> GenerateLuminance(IEnumerable<Color> baseColors)
        {
            var lums = new[] { 235, 236, 197, 119, 79 };
            return baseColors.Concat(lums.SelectMany(lum => baseColors.Select(c => ChangeColorLuminance(c, lum))));
        }

    }

    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
