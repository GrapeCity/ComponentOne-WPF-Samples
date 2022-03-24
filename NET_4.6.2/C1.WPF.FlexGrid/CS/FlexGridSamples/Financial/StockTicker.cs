using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MainTestApplication;

namespace FlexGridSamples
{
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "Arrow", Type = typeof(Polygon))]
    [TemplatePart(Name = "TxtChange", Type = typeof(TextBlock))]
    [TemplatePart(Name = "TxtValue", Type = typeof(TextBlock))]
    [TemplatePart(Name = "SparkLine", Type = typeof(Polyline))]
    public class StockTicker : Control
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(StockTicker),
            new PropertyMetadata(0.0, ValueChanged));

        string _format = "n2";
        Storyboard _flash;
        string _bindingSource;
        bool _firstTime = true;
        Grid _root;
        Polygon _arrow;
        TextBlock _txtChange;
        TextBlock _txtValue;
        Polyline _sparkLine;

        static Brush _brNegative = new SolidColorBrush(Colors.Red);
        static Brush _brPositive = new SolidColorBrush(Colors.Green);
        static Color _clrNegative = Color.FromArgb(100, 0xff, 0, 0);
        static Color _clrPositive = Color.FromArgb(100, 0, 0xff, 0);

        static StockTicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StockTicker),
                new FrameworkPropertyMetadata(typeof(StockTicker)));
            IsHitTestVisibleProperty.OverrideMetadata(typeof(StockTicker), new FrameworkPropertyMetadata(false));
        }

        public StockTicker()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _root = GetTemplateChild("LayoutRoot") as Grid;
            bool error = false;
            if (_root == null)
            {
                error = true;
            }
            else
            {
                _arrow = GetTemplateChild("Arrow") as Polygon;
                _txtChange = GetTemplateChild("TxtChange") as TextBlock;
                _txtValue = GetTemplateChild("TxtValue") as TextBlock;
                _sparkLine = GetTemplateChild("SparkLine") as Polyline;
                error |= _arrow == null || _txtChange == null || _txtValue == null || _sparkLine == null;
            }
            if (error)
            {
                throw new System.Exception("StockTicker ControlTemplate doesn't contain all required members");
            }
            OnValueChanged(double.NaN, Value);
        }

        private Storyboard Flash
        {
            get
            {
                if (_flash == null)
                {
                    _flash = (Storyboard)_root.Resources["Flash"];
                }
                return _flash;
            }
        }
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as StockTicker).OnValueChanged((double)e.OldValue, (double)e.NewValue);
        }

        private void OnValueChanged(double oldValue, double value)
        {
            if (!IsLoaded)
            {
                return;
            }

            // resetting
            if (double.IsNaN(value))
            {
                if (_flash != null)
                {
                    _flash.Stop();
                }
                _root.Background = new SolidColorBrush(Colors.Transparent);
                _arrow.Fill = null;
                _txtChange.Foreground = _txtValue.Foreground;
                return;
            }

            // get historical data
            var data = Tag as FinancialData;
            var list = BindingSource != null ? data.GetHistory(BindingSource) : new System.Collections.Generic.List<decimal>();
            if (list != null && list.Count > 1)
            {
                oldValue = (double)list[list.Count - 2];
            }

            // calculate percentage change
            var change = oldValue == 0 || double.IsNaN(oldValue) ? 0 : (value - oldValue) / oldValue;

            // update text
            _txtValue.Text = value.ToString(_format);
            _txtChange.Text = string.Format("({0:0.0}%)", change * 100);

            // update flash color
            ColorAnimation ca;

            // update symbol
            if (change == 0)
            {
                _arrow.Fill = null;
                _txtChange.Foreground = _txtValue.Foreground;
            }
            else if (change < 0)
            {
                var tr = new ScaleTransform(1, -1, 0, 4);
                _arrow.RenderTransform = tr;
                _txtChange.Foreground = _arrow.Fill = _brNegative;
                ca = Flash.Children[0] as ColorAnimation;
                ca.From = _clrNegative;
            }
            else
            {
                var tr = new ScaleTransform(1, 1, 0, 4);
                _arrow.RenderTransform = tr;
                _txtChange.Foreground = _arrow.Fill = _brPositive;
                ca = Flash.Children[0] as ColorAnimation;
                ca.From = _clrPositive;
            }

            // update sparkline
            if (list != null)
            {
                var points = _sparkLine.Points;
                points.Clear();
                for (int x = 0; x < list.Count; x++)
                {
                    points.Add(new Point(x, (double)list[x]));
                }
            }

            // flash new value (but not right after the control was created)
            if (!_firstTime && change != 0)
            {
                _flash.Begin();
            }
            _firstTime = false;
        }

        public string BindingSource
        {
            get
            {
                return _bindingSource;
            }
            set
            {
                _bindingSource = value;
            }
        }
        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
                _txtValue.Text = Value.ToString(_format);
            }
        }
    }
}
