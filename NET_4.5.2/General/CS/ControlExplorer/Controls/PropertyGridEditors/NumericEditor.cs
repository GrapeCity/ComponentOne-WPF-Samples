using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using C1.WPF;
using C1.WPF.Extended;

namespace ControlExplorer
{
    public class NumericEditor : Grid, ITypeEditorControl
    {
        private PropertyAttribute _selectedProperty;
        private NumericProperties _slider;
        private C1NumericBox _numeric;
        public event System.ComponentModel.PropertyChangedEventHandler ValueChanged;

        public NumericEditor()
        {
            _slider = new NumericProperties() { Visibility = Visibility.Collapsed };
            _slider.SetBinding(NumericProperties.SliderStyleProperty, new Binding("SliderStyle") { Source = this });
            _slider.SetBinding(NumericProperties.CheckboxStyleProperty, new Binding("CheckboxStyle") { Source = this });
            _numeric = new C1NumericBox() { Visibility = Visibility.Collapsed };
            _numeric.SetBinding(StyleProperty, new Binding("NumericStyle") { Source = this });
            this.Children.Add(_slider);
            this.Children.Add(_numeric);
        }

        public Style NumericStyle
        {
            get { return (Style)GetValue(NumericStyleProperty); }
            set { SetValue(NumericStyleProperty, value); }
        }

        public static readonly DependencyProperty NumericStyleProperty =
            DependencyProperty.Register("NumericStyle", typeof(Style), typeof(NumericEditor), new PropertyMetadata(null));



        public Style SliderStyle
        {
            get { return (Style)GetValue(SliderStyleProperty); }
            set { SetValue(SliderStyleProperty, value); }
        }

        public static readonly DependencyProperty SliderStyleProperty =
            DependencyProperty.Register("SliderStyle", typeof(Style), typeof(NumericEditor), new PropertyMetadata(null));



        public Style CheckboxStyle
        {
            get { return (Style)GetValue(CheckboxStyleProperty); }
            set { SetValue(CheckboxStyleProperty, value); }
        }

        public static readonly DependencyProperty CheckboxStyleProperty =
            DependencyProperty.Register("CheckboxStyle", typeof(Style), typeof(NumericEditor), new PropertyMetadata(null));



        public ITypeEditorControl Create()
        {
            return new NumericEditor();
        }

        public bool Supports(PropertyAttribute Property)
        {
            return Property.PropertyInfo.PropertyType.GetNonNullableType().IsNumeric();
        }

        public void Attach(PropertyAttribute property)
        {
            _selectedProperty = property;

            double defaultValue = double.NaN;
            bool valueChanged = false;
            if (property.PropertyInfo.CanRead)
            {
                var propertyValue = property.PropertyInfo.GetValue(property.SelectedObject, null);
                defaultValue = (propertyValue == null) ? double.NaN : Convert.ToDouble(propertyValue, CultureInfo.InvariantCulture);
            }
            try
            {
                if (property.DefaultValue != null)
                {
                    defaultValue = Convert.ToDouble(property.DefaultValue, CultureInfo.InvariantCulture);
                    valueChanged = true;
                }
            }
            catch { }

            if (double.IsNaN(property.MinimumValue) || double.IsNaN(property.MaximumValue))
            {
                PrepareNumeric(property, defaultValue, valueChanged);
                BindingHelper.BindEditor(this._numeric, C1NumericBox.ValueProperty, _selectedProperty, null/* new NumericValueConverter()*/);
            }
            else
            {
                PrepareSlider(property, defaultValue, valueChanged);
            }

            _numeric.ValueChanged += numeric_ValueChanged;
            _slider.ValueChanged += slider_ValueChanged;
        }

        private void PrepareSlider(PropertyAttribute property, double defaultValue, bool valueChanged)
        {
            _slider.Visibility = Visibility.Visible;
            _slider.MinimumValue = property.MinimumValue;
            _slider.MaximumValue = property.MaximumValue;
            _slider.checkNullable.IsChecked = !double.IsNaN(defaultValue);
            _slider.ShowCheck = Convert.ToBoolean(property.Tag);
            _slider.Value = defaultValue;
            if (valueChanged) slider_ValueChanged(null, null);
        }

        private void PrepareNumeric(PropertyAttribute property, double defaultValue, bool valueChanged)
        {
            _numeric.Visibility = Visibility.Visible;
            _numeric.AllowNull = property.PropertyInfo.PropertyType.IsNullableType();
            if (!double.IsNaN(property.MinimumValue))
            {
                _numeric.Minimum = property.MinimumValue;
            }
            if (!double.IsNaN(property.MaximumValue))
            {
                _numeric.Maximum = property.MaximumValue;
            }

            // set format
            if (property.PropertyInfo.PropertyType.GetNonNullableType() == typeof(double))
            {
                _numeric.Format = "F2";
            }
            else if (property.PropertyInfo.PropertyType.GetNonNullableType() == typeof(Int32))
            {
                _numeric.Format = "F0";
            }
            else
            {
                _numeric.Format = "F0";
            }
            _numeric.Value = defaultValue;
            if (valueChanged) numeric_ValueChanged(null, null);
        }

        public void Detach(PropertyAttribute property)
        {
            _slider.ValueChanged -= slider_ValueChanged;
            _numeric.ValueChanged -= numeric_ValueChanged;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ApplyNumericValue(_slider.Value);
        }

        private void numeric_ValueChanged(object sender, EventArgs e)
        {
            ApplyNumericValue(_numeric.Value);
        }

        private void ApplyNumericValue(object value)
        {
            if (_selectedProperty.PropertyInfo.CanWrite)
            {
                try
                {
                    Type destinationType = _selectedProperty.PropertyInfo.PropertyType.GetNonNullableType();
                    var newValue = Convert.ChangeType(value, destinationType, null);
                    _selectedProperty.PropertyInfo.SetValue(_selectedProperty.SelectedObject, newValue, null);
                    OnSOPropertyChanged();
                }
                catch { }
            }
        }

        private void OnSOPropertyChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new PropertyChangedEventArgs(_selectedProperty.MemberName));
            }
        }
    }
}
