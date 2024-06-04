using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CustomTypeDescriptor
{
    public partial class SearchBox : UserControl
    {
        PropertyList _propertyInfo = new PropertyList();
        C1.Util.Timer _timer = new C1.Util.Timer();

        public SearchBox()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromMilliseconds(800);
            _timer.Tick += _timer_Tick;
        }
        public ICollectionView View { get; set; }
        public PropertyList FilterProperties
        {
            get { return _propertyInfo; }
        }
        void _txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_imgClear != null)
            {
                // update clear button visibility
                _imgClear.Visibility = string.IsNullOrEmpty(_txtSearch.Text)
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                // start ticking
                _timer.Stop();
                _timer.Start();
            }
        }
        void _imgClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _txtSearch.Text = string.Empty;
        }
        void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            if (View != null && _propertyInfo.Count > 0)
            {
                View.Filter = null;
                View.Filter = (object item) =>
                {
                    // get search text
                    var srch = _txtSearch.Text;

                    // no text? show all items
                    if (string.IsNullOrEmpty(srch))
                    {
                        return true;
                    }

                    // show items that contain the text in any of the specified properties
                    foreach (PropertyInfo pi in _propertyInfo)
                    {
                        var value = pi.GetValue(item, null) as string;
                        if (value != null && value.IndexOf(srch, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            return true;
                        }
                    }

                    // exclude this item...
                    return false;
                };
            }
        }
    }
    /// <summary>
    /// This list allows adding PropertyDescriptor items in addition to regular
    /// PropertyInfo items. It does this by automatically creating CustomPropertyInfo
    /// objects to wrap the PropertyDescriptor items.
    /// </summary>
    public class PropertyList : List<PropertyInfo>
    {
        // automatically convert PropertyDescriptor into PropertyInfo
        public void Add(PropertyDescriptor pd)
        {
            base.Add(new CustomPropertyInfo(pd));
        }
        internal class CustomPropertyInfo : PropertyInfo
        {
            PropertyDescriptor _pd;
            public CustomPropertyInfo(PropertyDescriptor pd)
            {
                _pd = pd;
            }
            public override string Name
            {
                get { return _pd.Name; }
            }
            public override Type PropertyType
            {
                get { return _pd.PropertyType; }
            }
            public override object GetValue(object obj, object[] index)
            {
                return _pd.GetValue(obj);
            }
            public override object GetValue(object obj, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, object[] index, System.Globalization.CultureInfo culture)
            {
                return _pd.GetValue(obj);
            }
            public override void SetValue(object obj, object value, object[] index)
            {
                _pd.SetValue(obj, value);
            }
            public override void SetValue(object obj, object value, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, object[] index, System.Globalization.CultureInfo culture)
            {
                _pd.SetValue(obj, value);
            }
            public override bool CanRead
            {
                get { return true; }
            }
            public override bool CanWrite
            {
                get { return !_pd.IsReadOnly; }
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }
            public override object[] GetCustomAttributes(bool inherit)
            {
                throw new NotImplementedException();
            }
            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }
            public override Type DeclaringType
            {
                get { throw new NotImplementedException(); }
            }
            public override Type ReflectedType
            {
                get { throw new NotImplementedException(); }
            }
            public override System.Reflection.PropertyAttributes Attributes
            {
                get { throw new NotImplementedException(); }
            }
            public override System.Reflection.ParameterInfo[] GetIndexParameters()
            {
                throw new NotImplementedException();
            }
            public override System.Reflection.MethodInfo GetSetMethod(bool nonPublic)
            {
                throw new NotImplementedException();
            }
            public override System.Reflection.MethodInfo GetGetMethod(bool nonPublic)
            {
                throw new NotImplementedException();
            }
            public override System.Reflection.MethodInfo[] GetAccessors(bool nonPublic)
            {
                throw new NotImplementedException();
            }
        }
    }
}
