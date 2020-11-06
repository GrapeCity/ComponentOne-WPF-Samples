using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using C1.WPF.Gauge;

namespace GaugeSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class GaugeUtil
    {
        public static double LinearPointToValue(C1LinearGauge linearGauge, Point point)
        {
            if(linearGauge.Orientation.Equals(Orientation.Horizontal))
            {
                double min = linearGauge.ActualWidth * linearGauge.XAxisLocation;
                double max = linearGauge.ActualWidth * (1 - linearGauge.XAxisLocation);
                if (point.X <= min)
                    return 0;
                if (point.X >= max)
                    return 100;
                double maxvalue = linearGauge.ActualWidth * linearGauge.XAxisLength;
                double locatX = point.X - min;
                return locatX / maxvalue * 100;
            }
            else
            {
                if (point.Y < 0)
                    return 0;
                if (point.Y > linearGauge.ActualHeight)
                    return 100;
                return point.Y / linearGauge.ActualHeight * 100;
            }
        }

        public static double RadialPointToValue(C1RadialGauge radialGauge, Point point)
        {
            Point center = new Point(radialGauge.PointerOrigin.X * radialGauge.RenderSize.Width, radialGauge.PointerOrigin.Y * radialGauge.RenderSize.Height);
            double angle = Mod360(Math.Atan2(point.X - center.X, center.Y - point.Y) * 180 / Math.PI);
            return AngleToValue(radialGauge, angle);
        }

        static double Mod360(double value)
        {
            double result = value % 360;
            if (value < 0)
                result += 360;
            return result;
        }

        static double AngleToValue(C1RadialGauge radialGauge, double angle)
        {
            double alpha = AngleToLogical(radialGauge, angle);
            return LogicalToValue(radialGauge, alpha);
        }

        static double AngleToLogical(C1RadialGauge radialGauge, double angle)
        {
            var relativeAngle = Mod360(angle - radialGauge.StartAngle);
            var absSweepAngle = radialGauge.SweepAngle;
            if (absSweepAngle == 0 || relativeAngle == 0)
                return 0;
            if (absSweepAngle < 0)
            {
                relativeAngle = 360 - relativeAngle;
                absSweepAngle = -absSweepAngle;
            }
            var overflow = relativeAngle - absSweepAngle;
            if (overflow > 0)
            {
                var underflow = 360 - relativeAngle;
                return overflow < underflow ? 1 : 0;
            }
            return relativeAngle / absSweepAngle;
        }

        static double LogicalToValue(C1RadialGauge radialGauge, double alpha)
        {
            if (alpha <= 0)
                return radialGauge.Minimum;
            if (1 <= alpha)
                return radialGauge.Maximum;

            double linearValue;
            if (!radialGauge.IsLogarithmic)
            {
                linearValue = alpha;
            }
            else
            {
                if (radialGauge.LogarithmicBase <= 1)
                    return radialGauge.Minimum;

                linearValue = (Math.Pow(radialGauge.LogarithmicBase, alpha) - 1) / (radialGauge.LogarithmicBase - 1);
            }
            return (radialGauge.Minimum + (radialGauge.Maximum - radialGauge.Minimum) * linearValue);
        }
    }

    class ValueTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value).ToString("F0");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
