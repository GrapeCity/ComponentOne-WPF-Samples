using C1.DataCollection;
using C1.WPF.Core;
using C1.WPF.ListView;
using C1.WPF.RulesManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RulesManagerExplorer
{
    public class TemperatureControlValueToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is DataGridCell cell && cell.DataContext is not null && values[1] is TemperatureTableControl control && parameter is string strParam)
            {
                RulesEngineStyle style = control.GetConditionalFormattingStyle(cell);
                var value = style.GetType().GetProperty(strParam).GetValue(style);
                if (strParam == "FontSize" && value.Equals(double.NaN))
                    return DependencyProperty.UnsetValue;
                return value ?? DependencyProperty.UnsetValue;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class DataGridValueToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is DataGridCell cell && cell.DataContext is not null && values[1] is DataGrid grid && values[2] is C1RulesEngine engine)
            {
                // Skip rows that do not refer to data (eg. NewItemPlaceholder)
                if (grid.Items.IndexOf(cell.DataContext) >= 0)
                {
                    var style = engine.GetStyle(new DataGridRulesEngineSource(grid), DataGridRulesEngineSource.GetIndex(grid, cell), DataGridRulesEngineSource.GetField(cell));
                    return style.GetType().GetProperty(parameter as string).GetValue(style) ?? DependencyProperty.UnsetValue;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ScoreToIconConverter : IMultiValueConverter
    {
        public class ScoreSource : RulesEngineSource
        {
            public ScoreSource(List<Student> students)
            {
                Students = students;
            }

            public List<Student> Students { get; }
            public string Field => "Score";

            public override IEnumerable<(string Name, Type Type)> GetFields()
            {
                yield return (Field, typeof(double));
            }

            protected override IDataCollection<object> GetRangeValuesCollection<T>(RulesEngineRange range)
            {
                var students = Students;
                if (range.FirstItem.HasValue && range.LastItem.HasValue)
                {
                    students = students.Skip(range.FirstItem.Value).Take(range.LastItem.Value - range.FirstItem.Value + 1).ToList();
                }
                return new C1DataCollection<object>(students.Select(s => s.Score));
            }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Student item && values[1] is C1ListView list && values[2] is C1RulesEngine engine)
            {
                var students = list.ItemsSource as List<Student>;
                var index = students.IndexOf(item);
                var style = engine.GetStyle(new ScoreSource(students), index, "Score");
                if (style != null)
                {
                    if (values[4] is ContentControl textPresenter)
                    {
                        var textStyle = new Style(typeof(ContentControl));
                        if (style.Foreground is not null)
                        {
                            AddSetter(textStyle, new Setter { Property = Control.ForegroundProperty, Value = style.Foreground });
                        }
                        if (style.Background is not null)
                        {
                            AddSetter(textStyle, new Setter { Property = Control.BackgroundProperty, Value = style.Background });
                        }
                        textPresenter.Style = textStyle;
                    }
                    if (values[3] is ContentPresenter iconPresenter)
                    {
                        if (style.IconTemplate is not null)
                        {
                            var icon = style.IconTemplate;
                            if (style.IconBrush != null)
                            {
                                var iconStyle = new Style(typeof(ContentPresenter));
                                AddSetter(iconStyle, new Setter { Property = Control.ForegroundProperty, Value = style.IconBrush });
                                iconPresenter.Style = iconStyle;
                            }
                            return style.IconTemplate;
                        }
                    }
                }
            }
            return new DataTemplate();
        }

        private void AddSetter(Style style, SetterBase setterBase)
        {
            if (setterBase is Setter setter)
            {
                foreach (var s in style.Setters.OfType<Setter>().ToList())
                {
                    if (s.Property == setter.Property)
                        style.Setters.Remove(s);
                }
            }
            style.Setters.Add(setterBase);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class CountryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var countries = Customer.GetCountries();
            return countries[(int)value].Value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Customer.GetCountries().FirstOrDefault(pair => pair.Value == (value as string));
        }
    }
}
