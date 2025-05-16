using C1.DataCollection;
using C1.WPF.RulesManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace RulesManagerExplorer
{
    public class DataGridRulesEngineSource : RulesEngineSource
    {
        private DataGrid _grid;

        public DataGridRulesEngineSource(DataGrid grid)
        {
            _grid = grid;
        }

        public static string GetField(DataGridCell cell)
        {
            var binding = (cell.Column as DataGridTextColumn)?.Binding as Binding;
            return binding?.Path?.Path ?? cell.Column.Header as string;
        }


        public static int GetIndex(DataGrid grid, DataGridCell cell)
        {
            return grid.Items.IndexOf(cell.DataContext);
        }

        public override IEnumerable<(string Name, Type Type)> GetFields()
        {
            return GetItemType().GetProperties().Select(p => (p.Name, p.PropertyType));
        }

        private Type GetItemType()
        {
            return _grid.ItemsSource.Cast<object>().First().GetType();
        }

        protected override IDataCollection<object> GetRangeValuesCollection<T>(RulesEngineRange range)
        {
            var itemType = GetItemType();
            var sequence = new C1SequenceDataCollection<object>();
            {
                foreach (var field in range.Fields)
                {
                    var pi = itemType.GetProperty(field);
                    var countItems = _grid.Items.Count;
                    var startingIndex = Math.Min(countItems - 1, Math.Max(0, range.FirstItem ?? 0));
                    var count = Math.Max(countItems, (range.LastItem ?? -1) + 1) - startingIndex;
                    sequence.Collections.Add(new C1DelegateList<object>(count, i =>
                    {
                        try
                        {
                            var item = _grid.Items[startingIndex + i];
                            if (pi != null && item.GetType() == itemType)
                            {
                                object value = pi.GetValue(item);
                                if (ConvertValue(value, typeof(T)) is T result)
                                    return result;
                            }
                            return default(T);
                        }
                        catch
                        {
                            return default(T);
                        }
                    }).AsDataCollection());
                }
            }
            return sequence;
        }

        public object ConvertValue(object value, Type type)
        {
            if (type.IsAssignableFrom(value?.GetType() ?? typeof(object)))
                return value;
            if (type == typeof(object))
            {
                return value;
            }
            if (type == typeof(double))
            {
                if (value is string)
                {
                    return double.Parse((string)value, NumberStyles.Any);
                }
                else
                {
                    return Convert.ToDouble(value);
                }
            }
            if (type == typeof(int))
            {
                if (value is string)
                {
                    return int.Parse((string)value, NumberStyles.Any);
                }
                else
                {
                    return Convert.ToInt32(value);
                }
            }
            // Add other types of processing if necessary...
            return default;
        }
    }
}