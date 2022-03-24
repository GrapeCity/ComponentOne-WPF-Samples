using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using C1.WPF;
using C1.WPF.Extended;

namespace ControlExplorer
{
	public class CornerRadiusEditor : C1TextBoxBase, ITypeEditorControl
    {

        #region ITypeEditorControl Members

        public ITypeEditorControl Create()
        {
			return new CornerRadiusEditor();
        }

        public bool Supports(PropertyAttribute Property)
        {
			return Property.PropertyInfo.PropertyType == typeof(CornerRadius);
        }

        public void Attach(PropertyAttribute property)
        {
            var binding = new Binding(property.PropertyInfo.Name)
            {
                Mode = BindingMode.TwoWay,
                Source = property.SelectedObject,
                ValidatesOnExceptions = true,
				Converter = new CornerRadiusConverter()
            };
            this.SetBinding(C1TextBoxBase.C1TextProperty, binding);
        }

        public void Detach(PropertyAttribute property)
        {
        }

        public event System.ComponentModel.PropertyChangedEventHandler ValueChanged;

        #endregion
    }

	public class CornerRadiusConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			if (value is CornerRadius)
            {
				return ((CornerRadius)value).ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                try
                {
					return XamlReader.Parse(string.Format(@"<CornerRadius xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>{0}</CornerRadius>", value));
                }
                catch
                {
                }
            }
            return value;
        }

        #endregion
    }
}
