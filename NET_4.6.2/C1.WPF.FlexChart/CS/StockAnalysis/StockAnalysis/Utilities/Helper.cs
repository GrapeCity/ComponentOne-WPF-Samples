using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace StockAnalysis.Utilities
{
    public class Helper
    {
        public static T FindParent<T>(DependencyObject obj)
        {
            var curr = VisualTreeHelper.GetParent(obj);
            var parent = (curr as FrameworkElement) == null ? null : (curr as FrameworkElement).Parent;
            if (parent == null || parent is T)
            {
                return (T)Convert.ChangeType(parent, typeof(T));
            }
            else
            {
                return FindParent<T>(parent);
            }
        }

        
        //Binding view mode property setting panel
        public static void BindingSettingsParams(C1.WPF.Chart.C1FlexChart chart, object source, Type srcType, string srckey, IEnumerable<Data.PropertyParam> properties, Action propertyChangedCallback = null)
        {
            IEnumerable<Data.SettingParam> settings;
            if (ViewModel.ViewModel.Instance.Settings.TryGetValue(srckey, out settings))
            {
                foreach (var param in properties)
                {
                    var setting = (from p in settings where (param.Group == null) ? p.Key == param.Key : (p.Key == param.Key && p.Group == param.Group) select p).FirstOrDefault();
                    if (setting != null)
                    {
                        Queue<string> keys = new Queue<string>(param.Key.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries));
                        KeyValuePair<System.Reflection.PropertyInfo, object> propertyInfo = GetPropertyInfo(source, srcType, keys);

                        SetPropertyValue(setting, param, propertyInfo);
                        setting.PropertyChanged += (o, e) =>
                        {
                            SetPropertyValue(setting, param, propertyInfo);
                            if (propertyChangedCallback != null)
                            {
                                propertyChangedCallback();
                            }
                            chart.Invalidate();
                        };
                    }
                }
            }
        }
        
        public static Data.SettingParam GetSettingsParam(string srckey, Data.PropertyParam param)
        {
            IEnumerable<Data.SettingParam> settings;
            if (ViewModel.ViewModel.Instance.Settings.TryGetValue(srckey, out settings))
            {
                var setting = (from p in settings where (param.Group == null) ? p.Key == param.Key : (p.Key == param.Key && p.Group == param.Group) select p).FirstOrDefault();

                return setting;
            }
            return null;
        }

        private static void SetPropertyValue(Data.SettingParam setting, Data.PropertyParam param, KeyValuePair<System.Reflection.PropertyInfo, object> propertyInfo)
        {
            if (setting.Value is IConvertible)
            {
                object value = Convert.ChangeType(setting.Value, param.Type);
                if (param.Type == typeof(int))
                {
                    object min = Convert.ChangeType(setting.Minimum, param.Type);
                    if (Convert.ToDouble(value) < Convert.ToDouble(min))
                    {
                        return;
                    }
                }
                propertyInfo.Key.SetValue(propertyInfo.Value, value, null);
            }
            else
            {
                propertyInfo.Key.SetValue(propertyInfo.Value, setting.Value, null);
            }
        }

        private static KeyValuePair<System.Reflection.PropertyInfo, object> GetPropertyInfo(object obj, Type type, Queue<string> keys)
        {
            var key = keys.Dequeue();
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(key);

            if (keys.Any())
            {
                return GetPropertyInfo(propertyInfo.GetValue(obj, null), propertyInfo.PropertyType, keys);
            }
            else
            {
                return new KeyValuePair<System.Reflection.PropertyInfo, object>(propertyInfo, obj);
            }
        }
    }
}
