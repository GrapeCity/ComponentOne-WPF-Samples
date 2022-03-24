using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DynamicConditionalFormatting
{
    /// <summary>
    /// An IValueConverter that takes double values and converts them into brushes
    /// depending on their magnitude compared to the LowerValue and UpperValue
    /// properties.
    /// Values below the LowerValue are converted into a LowValueBrush, values
    /// above the UpperValue are converted into a HighValueBrush, and values in 
    /// between are converted into a NormalValueBrush.
    /// </summary>
    public class DynamicRangeConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// Gets or sets the lower value in the range converter.
        /// Values below this threshold will be displayed using the <see cref="LowValueBrush"/> value.
        /// </summary>
        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }
        public static readonly DependencyProperty LowerValueProperty =
            DependencyProperty.Register("LowerValue", typeof(double),
                typeof(DynamicRangeConverter), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the upper value in the range converter.
        /// Values below this threshold will be displayed using the <see cref="HighValueBrush"/> value.
        /// </summary>
        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }
        public static readonly DependencyProperty UpperValueProperty =
            DependencyProperty.Register("UpperValue", typeof(double),
                typeof(DynamicRangeConverter), new PropertyMetadata(1000.0));

        /// <summary>
        /// Brush returned for values below the <see cref="LowerValue"/>.
        /// </summary>
        public Brush LowValueBrush
        {
            get { return (Brush)GetValue(LowValueBrushProperty); }
            set { SetValue(LowValueBrushProperty, value); }
        }
        public static readonly DependencyProperty LowValueBrushProperty =
            DependencyProperty.Register("LowValueBrush", typeof(Brush),
                typeof(DynamicRangeConverter), new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        /// <summary>
        /// Brush returned for values below the <see cref="Maximum"/>.
        /// </summary>
        public Brush HighValueBrush
        {
            get { return (Brush)GetValue(HighValueBrushProperty); }
            set { SetValue(HighValueBrushProperty, value); }
        }
        public static readonly DependencyProperty HighValueBrushProperty =
            DependencyProperty.Register("HighValueBrush", typeof(Brush),
                typeof(DynamicRangeConverter), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        /// <summary>
        /// Brush returned for values between the <see cref="LowerValue"/> and <see cref="Maximum"/>.
        /// </summary>
        public Brush NormalValueBrush
        {
            get { return (Brush)GetValue(NormalValueBrushProperty); }
            set { SetValue(NormalValueBrushProperty, value); }
        }
        public static readonly DependencyProperty NormalValueBrushProperty =
            DependencyProperty.Register("NormalValueBrush", typeof(Brush),
                typeof(DynamicRangeConverter), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        // ** IValueConverter
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                var v = (double)value;
                return
                    v < LowerValue ? LowValueBrush :
                    v > UpperValue ? HighValueBrush :
                    NormalValueBrush;
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
