using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.SpellChecker;
using C1.WPF.DataGrid;
using System.Collections;
using System.Windows.Controls;
using System.Reflection;

namespace SpellCheckerSamples
{
    public class DataGridSpellWrapper : ISpellCheckableEditor
    {
        C1DataGrid _grid;                 // DataGrid being checked
        List<C1.WPF.DataGrid.DataGridTextColumn> _cols; // columns to be spell-checked
        IList _items;                   // items being checked
        int _row, _col;                 // cell being checked
        int _selStart, _selLength;      // selection within cell

        public DataGridSpellWrapper(C1DataGrid grid)
        {
            _grid = grid;
            _cols = new List<C1.WPF.DataGrid.DataGridTextColumn>();
            Initialize();
        }

        //-----------------------------------------------------------------------------
        #region ** ISpellCheckableEditor

        public Control Control
        {
            get { return _grid; }
        }
        public string Text
        {
            get
            {
                // get value from data object
                var item = _items[_row];
                var pi = GetPropertyInfo(item, _cols[_col]);

                return (string)pi.GetValue(item, null);
            }
            set
            {
                // set value in data object
                var item = _items[_row];
                var pi = GetPropertyInfo(item, _cols[_col]);
                pi.SetValue(item, value, null);

            }
        }
        public int SelectionStart
        {
            get { return _selStart; }
            set { _selStart = value; }
        }
        public int SelectionLength
        {
            get { return _selLength; }
            set { _selLength = value; }
        }
        public string SelectedText
        {
            get { return Text.Substring(_selStart, _selLength); }
            set
            {
                string text = Text;
                text = string.Format("{0}{1}{2}",
                    text.Substring(0, _selStart),
                    value,
                    text.Substring(_selStart + _selLength));
                Text = text;
            }
        }
        public void Select(int start, int length)
        {
            // keep track of selection within the cell
            _selStart = start;
            _selLength = length;

            // check that the cell being checked is selected
            _grid.SelectedIndex = _row;
            _grid.CurrentColumn = _cols[_col];
            _grid.ScrollIntoView(_grid.SelectedItem, _grid.CurrentColumn);
        }
        public bool HasMoreText()
        {
            return MoveNext();
        }
        public void BeginSpell()
        {
            Initialize();
        }
        public void EndSpell()
        {
        }

        #endregion

        //-----------------------------------------------------------------------------
        #region ** private stuff

        // initialize list of items to check
        void Initialize()
        {
            //build list of items to check
            _items = _grid.ItemsSource as IList;
            if (_items == null)
            {
                _items = new List<object>();
                foreach (object item in _grid.ItemsSource)
                {
                    _items.Add(item);
                }
            }

            // build list of text columns
            _cols.Clear();
            foreach (C1.WPF.DataGrid.DataGridColumn col in _grid.Columns)
            {
                var textCol = col as C1.WPF.DataGrid.DataGridTextColumn;
                if (textCol != null && !textCol.IsReadOnly)
                {
                    _cols.Add(textCol);
                }
            }

            // move to first cell
            _row = -1;
            MoveNext();
        }

        // move on to the next cell
        bool MoveNext()
        {
            // initialize or increment row/col position
            if (_row < 0)
            {
                // initialize
                _row = 0;
                _col = 0;
            }
            else if (_col < _cols.Count - 1)
            {
                // next column
                _col++;
            }
            else
            {
                // next row
                _row++;
                _col = 0;
            }

            // return true if we still have valid cells
            return _row < _items.Count && _col < _cols.Count;
        }

        // get PropertyInfo for getting/setting value of the current cell
        PropertyInfo GetPropertyInfo(object item, C1.WPF.DataGrid.DataGridBoundColumn column)
        {
            string propName = column.Binding.Path.Path;
            PropertyInfo pi = item.GetType().GetProperty(propName);
            return pi;
        }

        #endregion
    }
}
