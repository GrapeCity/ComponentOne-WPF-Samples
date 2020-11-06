using System;
using System.Windows;
using System.Windows.Controls;

namespace ControlExplorer
{
    public partial class NumericProperties : UserControl
    {
        public NumericProperties()
        {
            InitializeComponent();
			slider.SetBinding(StyleProperty, new System.Windows.Data.Binding("SliderStyle") { Source = this });
			checkNullable.SetBinding(StyleProperty, new System.Windows.Data.Binding("CheckboxStyle") { Source = this });
        }

        private double? _value;
        private Type type;
        private bool _isSettingValue = false;
        private double _fontSize;

        public double FontSize
        {
            get { return _fontSize; }
            set { 
                _fontSize = value;
                lblNumber.FontSize = value;
            }
        }



		public Style SliderStyle
		{
			get { return (Style)GetValue(SliderStyleProperty); }
			set { SetValue(SliderStyleProperty, value); }
		}

		public static readonly DependencyProperty SliderStyleProperty =
			DependencyProperty.Register("SliderStyle", typeof(Style), typeof(NumericProperties), new PropertyMetadata(null));



		public Style CheckboxStyle
		{
			get { return (Style)GetValue(CheckboxStyleProperty); }
			set { SetValue(CheckboxStyleProperty, value); }
		}

		public static readonly DependencyProperty CheckboxStyleProperty =
			DependencyProperty.Register("CheckboxStyle", typeof(Style), typeof(NumericProperties), new PropertyMetadata(null));

		
		
        public event EventHandler<RoutedPropertyChangedEventArgs<double>> ValueChanged;
        public double MinimumValue
        {
            get { return slider.Minimum; }
            set { slider.Minimum = value; }
        }

        public double MaximumValue
        {
            get { return slider.Maximum; }
            set { slider.Maximum = value; }
        }

        public double? Value
        {
            get { return _value; }
            set {
                _isSettingValue = true;
                _value = value;
                type = value != null ? value.GetType() : typeof(Nullable<double>);
                if (_value is double && double.IsNaN((double)_value))
                {
                    checkNullable.IsChecked = false;
                    slider.IsEnabled = false;
                }
                else
                {
                    checkNullable.IsChecked = true;
                    slider.Value = _value ?? slider.Minimum;
                    UpdateText();
                }
                _isSettingValue = false;
            }
        }

        private void UpdateText()
        {
            if (_value is double)
            {
                if (double.IsNaN((double)_value))
                {
                    lblNumber.Text = "";
                }
                else
                {
                    var format = MaximumValue - MinimumValue < 10? "F1" : "F0";
                    lblNumber.Text = ((double)_value).ToString(format);
                }
            }
            else if (_value is int)
            {
                lblNumber.Text = ((int)_value).ToString("F0");
            }
            else if (_value is long)
            {
                lblNumber.Text = ((long)_value).ToString("F0");
            }
        }

        public bool ShowCheck
        {
            get { return (checkNullable.Visibility == Visibility.Visible); }
            set { checkNullable.Visibility = (value ? Visibility.Visible : Visibility.Collapsed); }
        }

        private void checkNullable_Checked(object sender, RoutedEventArgs e)
        {
            if (!_isSettingValue)
            {
                _value = slider.Value;
            }
            slider.IsEnabled = true;
            UpdateText();
            OnValueChanged(double.NaN, slider.Value);
        }

        private void checkNullable_Unchecked(object sender, RoutedEventArgs e)
        {
            _value = double.NaN;
            slider.IsEnabled = false;
            OnValueChanged(slider.Value, double.NaN);
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_value is double)
            {
                _value = slider.Value;
            }
            else if (_value is int)
            {
                _value = Convert.ToInt32(slider.Value);
            }
            else if (_value is long)
            {
                _value = Convert.ToInt64(slider.Value);
            }
            OnValueChanged(e.OldValue, e.NewValue);
        }

        private void OnValueChanged(double oldValue, double newValue)
        {
            if (ValueChanged != null)
            {
                UpdateText();
                ValueChanged(this, new RoutedPropertyChangedEventArgs<double>(oldValue, newValue));
            }
        }
    }
}
