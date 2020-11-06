using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using C1.WPF;
using C1.WPF.DataGrid;
using C1.WPF.DataGrid.Filters;
using C1.WPF.Extended;

namespace DataGridSamples
{
    public class DataGridColorColumn : DataGridBoundColumn
    {
        public DataGridColorColumn()
            : base()
        {
        }

        public DataGridColorColumn(PropertyInfo property)
            : base(property)
        {
        }

        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        public override object GetCellContentRecyclingKey(DataGridRow row)
        {
            return typeof(C1CheckeredBorder);
        }

        public override FrameworkElement CreateCellContent(DataGridRow row)
        {
            C1CheckeredBorder border = new C1CheckeredBorder();
            border.BorderBrush = DataGrid.BorderBrush;
            border.BorderThickness = new Thickness(1);
            border.Margin = new Thickness(1);
            border.CornerRadius = new CornerRadius(2);
            border.SquareWidth = 7;
            return border;
        }

        public override void BindCellContent(FrameworkElement cellContent, DataGridRow row)
        {
            C1CheckeredBorder border = (C1CheckeredBorder)cellContent;
            Binding binding = CopyBinding(Binding);
            binding.Converter = new ColorConverter();
            binding.Source = row.DataItem;
            border.Style = CellContentStyle;
            border.SetBinding(System.Windows.Controls.Control.BackgroundProperty, binding);
        }

        public override FrameworkElement GetCellEditingContent(DataGridRow row)
        {
            C1ColorPicker colorPicker = new C1ColorPicker();
            colorPicker.Style = CellEditingContentStyle;
            colorPicker.IsDropDownOpen = true;
            Binding binding = CopyBinding(Binding);
            binding.Converter = new ColorConverter();
            colorPicker.SetBinding(C1ColorPicker.SelectedColorProperty, CopyBinding(binding));
            return colorPicker;
        }

        public override IDataGridFilter GetFilter()
        {
            var colorsFilter = new DataGridMultiValueFilter();
            colorsFilter.ItemTemplate = XamlReader.Parse(@"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns:local=""clr-namespace:DataGridSamples;assembly=" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + @""" xmlns:c1=""http://schemas.componentone.com/winfx/2006/xaml"">
            <StackPanel Orientation=""Horizontal"">
                <StackPanel.Resources>
                    <local:ColorConverter x:Key=""colorConverter""/>
                </StackPanel.Resources>
                <c1:C1CheckeredBorder BorderThickness=""1"" BorderBrush=""Black"" Background=""{Binding Converter={StaticResource colorConverter}}"" Width=""12"" Height=""12"" VerticalAlignment=""Center""/>
                <TextBlock Text=""{Binding}"" VerticalAlignment=""Center"" Margin=""2 0 0 0 ""/>
            </StackPanel>
        </DataTemplate>") as DataTemplate;
            colorsFilter.SetBinding(DataGridMultiValueFilter.ItemsSourceProperty, new Binding("ItemsSource")
            {
                Source = DataGrid,
                Converter = CustomConverter.Create((value, type, parameter, culture) =>
                {
                    if (value is IEnumerable)
                    {
                        var enumerable = ((IEnumerable)value).Cast<object>();
                        var colorConverter = new ColorConverter();
                        return enumerable.Select(o => C1DataGridFilterHelper.GetPropertyValue<object>(o, FilterMemberPath)).Distinct().Select(o => (Color)colorConverter.Convert(o, typeof(Color), null, CultureInfo.CurrentCulture)).OrderBy(c => new C1HslColor(c).Hue);
                    }
                    return value;
                })
            });
            colorsFilter.SetBinding(DataGridMultiValueFilter.BackgroundProperty, new Binding("HeaderBackground") { Source = DataGrid });
            colorsFilter.SetBinding(DataGridMultiValueFilter.ForegroundProperty, new Binding("HeaderForeground") { Source = DataGrid });
            colorsFilter.SetBinding(DataGridMultiValueFilter.BorderBrushProperty, new Binding("BorderBrush") { Source = DataGrid });
            colorsFilter.SetBinding(DataGridMultiValueFilter.MouseOverBrushProperty, new Binding("MouseOverBrush") { Source = DataGrid });
            colorsFilter.SetBinding(DataGridMultiValueFilter.PressedBrushProperty, new Binding("PressedBrush") { Source = DataGrid });
            colorsFilter.SetBinding(DataGridMultiValueFilter.FocusBrushProperty, new Binding("SelectedBackground") { Source = DataGrid });
            colorsFilter.SetBinding(DataGridMultiValueFilter.InputBackgroundProperty, new Binding("RowBackground") { Source = DataGrid });
            colorsFilter.SetBinding(DataGridMultiValueFilter.InputForegroundProperty, new Binding("RowForeground") { Source = DataGrid });
            var filter = new DataGridContentFilter
            {
                Content = new DataGridFilterList
                {
                    Items = new List<IDataGridFilterUnity>
                    {
                        new DataGridTextFilter(),
                        colorsFilter,
                    }
                }
            };
            return filter;
        }
    }

    public class ColorConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType.Name == "Brush" && value is Color)
            {
                return new SolidColorBrush((Color)value);
            }
            if (targetType.Name == "Color" && value is SolidColorBrush)
            {
                return ((SolidColorBrush)value).Color;
            }
            return value;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}

