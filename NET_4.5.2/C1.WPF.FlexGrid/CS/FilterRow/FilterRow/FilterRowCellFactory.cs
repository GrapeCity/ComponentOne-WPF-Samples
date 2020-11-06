using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using C1.WPF.FlexGrid;

namespace FilterRow
{
    class FilterRowCellFactory : CellFactory
    {
        // ** fields
        static Thickness _editorPadding = new Thickness(0, 0, 2, 0);

        // ** properties

        /// <summary>
        /// Set this property to true to handle the '%' character as a wildcard
        /// when filtering.
        /// For example, 
        ///     StartsWith%
        ///     %Endswith
        ///     %Contains%
        /// </summary>
        public bool UseRegEx { get; set; }

        // ** overrides
        public override void CreateColumnHeaderContent(C1FlexGrid grid, System.Windows.Controls.Border bdr, CellRange range)
        {
            // create the filter row if it isn't there
            var rows = grid.ColumnHeaders.Rows;
            if (rows.Count == 0 || !(rows[rows.Count - 1] is GridFilterRow))
            {
                rows.Add(new GridFilterRow());
            }

            // add filter editors to filter row
            var fr = rows[range.Row] as GridFilterRow;
            if (fr != null)
            {
                bdr.Child = new FilterEditor(fr, grid.Columns[range.Column]);
                bdr.Padding = _editorPadding;
            }
            else
            {
                base.CreateColumnHeaderContent(grid, bdr, range);
            }
        }
        public override void CreateTopLeftContent(C1FlexGrid grid, System.Windows.Controls.Border bdr, CellRange range)
        {
            var rows = grid.ColumnHeaders.Rows;
            var fr = rows[range.Row] as GridFilterRow;
            if (fr != null && range.Column == grid.TopLeftCells.Columns.Count - 1)
            {
                bdr.Child = new ClearFilterButton(fr);
            }
            else
            {
                base.CreateTopLeftContent(grid, bdr, range);
            }
        }
        public override bool IsSortSymbolRow(C1FlexGrid grid, CellRange range)
        {
            // show sort symbols on the column header row above the filter row
            return range.TopRow == grid.ColumnHeaders.Rows.Count - 2;
        }

        public void RefreshFilter(C1FlexGrid grid)
        {
            var rows = grid.ColumnHeaders.Rows;
            var filterRow = rows[rows.Count-1] as GridFilterRow;

            filterRow?.ApplyFilter();
        }
    }
    public class GridFilterRow : Row
    {
        // ** fields
        Dictionary<Column, string> _filterArgs = new Dictionary<Column, string>();
        Dictionary<Column, string> _rxPattern = new Dictionary<Column, string>();
        bool _useRegex;

        // ** object model
        public string GetFilterArgument(Column col)
        {
            string text;
            return _filterArgs.TryGetValue(col, out text)
                ? text
                : string.Empty;
        }
        public void SetFilterArgument(Column col, string arg)
        {
            if (arg != GetFilterArgument(col))
            {
                // save argument
                _filterArgs[col] = arg;

                // save regex pattern to support wildcards
                arg = arg != null ? arg.Replace("%", ".*") : arg;
                _rxPattern[col] = string.Format("^{0}$", arg);

                // apply the filter
                ApplyFilter();
            }
        }

        // ** implementation
        internal void ApplyFilter()
        {
            // check whether to use wildcards when matching
            var cf = Grid.CellFactory as FilterRowCellFactory;
            _useRegex = cf != null && cf.UseRegEx;

            // apply filter to grid's ICollectionView
            var view = Grid.ItemsSource as ICollectionView;
            if (view == null || !view.CanFilter)
            {
                // apply the filter in unbound mode
                foreach (var row in Grid.Rows)
                {
                    row.Visible = FilterPredicate(row);
                }

                // done
                return;
            }

            // can't apply filters while editing or adding items
            var edt = view as IEditableCollectionView;
            if (edt != null)
            {
                if (edt.IsEditingItem)
                    edt.CommitEdit();

                if (edt.IsAddingNew)
                    edt.CommitNew();
            }

            // apply the filter in bound mode
            try
            {
                view.Filter = FilterPredicate;
            }
            catch (Exception x)
            {
                MessageBox.Show("Failed to apply the filter:\r\n", x.Message);
            }
        }
        bool FilterPredicate(object item)
        {
            // get item type to retrieve properties
            var itemType = item.GetType();

            // in unbound mode, the item is the row itself
            bool unbound = item is Row;

            // check filter values applied to each column
            foreach (var kv in _filterArgs)
            {
                // no filter condition on this column? continue
                if (string.IsNullOrEmpty(kv.Value))
                {
                    continue;
                }

                // get cell value
                var stringValue = string.Empty;
                var row = item as Row;
                if (row != null)
                {
                    // handle unbound mode
                    var value = row[kv.Key];
                    stringValue = value == null ? string.Empty : value.ToString();
                }
                else
                {
                    // get property name
                    var propName = kv.Key.BoundPropertyName;
                    if (string.IsNullOrEmpty(propName))
                    {
                        continue;
                    }

                    // get property info
                    var prop = itemType.GetProperty(propName);
                    if (prop == null)
                    {
                        continue;
                    }

                    // get property value
                    var value = prop.GetValue(item, null);
                    stringValue = value == null ? string.Empty : value.ToString();
                }

                if (_useRegex)
                {
                    // test using regular expressions (support wildcards)
                    if (!Regex.IsMatch(stringValue, _rxPattern[kv.Key], RegexOptions.IgnoreCase))
                    {
                        return false;
                    }
                }
                else
                {
                    // text using simple 'contains' 
                    if (stringValue.IndexOf(kv.Value, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        return false;
                    }
                }
            }

            // item passed all filter conditions
            return true;
        }
    }
}
