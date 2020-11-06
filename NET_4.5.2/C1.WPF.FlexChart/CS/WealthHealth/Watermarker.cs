using C1.WPF.Chart;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WealthHealth
{
    public class Watermarker : ContentControl
    {
        private TextBlock _marker;

        public C1FlexChart ParentChart
        {
            get { return (C1FlexChart)GetValue(ParentChartProperty); }
            set { SetValue(ParentChartProperty, value); }
        }

        public static readonly DependencyProperty ParentChartProperty =
            DependencyProperty.Register("ParentChart", typeof(C1FlexChart), typeof(Watermarker), new PropertyMetadata(null));

        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(int), typeof(Watermarker), new PropertyMetadata(0, OnYearPropertyChanged));

        static void OnYearPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var w = obj as Watermarker;
            if (w != null)
            {
                w.OnYearChanged();
            }
        }

        void OnYearChanged()
        {
            if (_marker == null)
            {
                _marker = new TextBlock()
                {
                    Opacity = 0.1,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00916f"))
                };
            }
            InvalidateMeasure();
            InvalidateArrange();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (_marker != null)
            {
                if (constraint.Height != 0 && constraint.Width != 0)
                {
                    _marker.FontSize = Math.Min(constraint.Height, constraint.Width) / 2;
                    _marker.Text = Year.ToString();
                    _marker.Measure(new Size(constraint.Width, constraint.Height));
                }
            }
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (_marker != null && ParentChart != null)
            {
                var rect = ParentChart.PlotRect;
                var desiredSize = _marker.DesiredSize;
                _marker.Arrange(new Rect(rect.Right - desiredSize.Width, rect.Bottom - desiredSize.Height, desiredSize.Width, desiredSize.Height));
            }
            if (this.Content == null)
            {
                Content = _marker;
            }

            return arrangeBounds;
        }
    }
}
