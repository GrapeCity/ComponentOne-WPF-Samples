using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace GapAnalysis
{
    public class FormCellFactory : CellFactory
    {
        C1FlexGrid _grid;
        CellStyle _editStyle;
        CellStyle _calculatedStyle;
        CellStyle _numberStyle;
        CellStyle _groupRowStyle;

        public FormCellFactory()
        {
            _editStyle = new CellStyle();
            _editStyle.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xe0));

            _calculatedStyle = new CellStyle();
            _calculatedStyle.FontWeight = FontWeights.Bold;
            _calculatedStyle.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xe0, 0xe0, 0xff));
            _calculatedStyle.HorizontalAlignment = HorizontalAlignment.Right;

            _numberStyle = new CellStyle();
            _numberStyle.HorizontalAlignment = HorizontalAlignment.Right;

            _groupRowStyle = new CellStyle();
            _groupRowStyle.FontWeight = FontWeights.Bold;
        }
        void SetGrid(C1FlexGrid grid)
        {
            if (_grid == null)
            {
                _grid = grid;
                _grid.BeginningEdit += _grid_BeginningEdit;
                _grid.PreviewKeyDown += _grid_PreviewKeyDown;
                _grid.LoadedRows += _grid_LoadedRows;
                _grid_LoadedRows(_grid, EventArgs.Empty);
            }
            else if (grid != _grid)
            {
                throw new Exception("this factory can only be used with a single grid.");
            }
        }

        // customize display for group rows (bold/bigger font)
        void _grid_LoadedRows(object sender, EventArgs e)
        {
            _groupRowStyle.FontSize = _grid.FontSize + 4;
            foreach (var row in _grid.Rows)
            {
                var gr = row as GroupRow;
                if (gr != null)
                {
                    gr.Height = 40;
                    gr.CellStyle = _groupRowStyle;
                }
            }
        }

        // use tab key to navigate editable fields
        void _grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                var sel = _grid.Selection;
                var colIndex = _grid.Columns.IndexOf("Value");
                var offset = sel.Column == colIndex ? 1 : 0;
                for (; offset < _grid.Rows.Count && colIndex > 0; offset++)
                {
                    var rowIndex = (sel.Row + offset) % _grid.Rows.Count;
                    var field = _grid.Rows[rowIndex].DataItem as FormField;
                    if (field != null && string.IsNullOrEmpty(field.Formula))
                    {
                        _grid.Select(rowIndex, colIndex);
                        e.Handled = true;
                        return;
                    }
                }
            }
        }

        // values can be edited, formulas cannot
        void _grid_BeginningEdit(object sender, CellEditEventArgs e)
        {
            // get the row/column being edited
            var row = _grid.Rows[e.CellRange.Row];
            var field = row.DataItem as FormField;

            if (row is GroupRow || !string.IsNullOrEmpty(field.Formula))
            {
                e.Cancel = true;
            }
        }

        // customize cells
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            // attach grid event handlers
            SetGrid(grid);

            // get the row/column being created
            var col = grid.Columns[rng.Column];
            var row = grid.Rows[rng.Row];
            var field = row.DataItem as FormField;
            var gr = row as GroupRow;

            // create the cell
            base.CreateCellContent(grid, bdr, rng);
            if (col.BoundPropertyName == "Value" && gr == null)
            {
                if (!string.IsNullOrEmpty(field.Formula))
                {
                    _calculatedStyle.Apply(bdr, SelectedState.None);
                }
                else
                {
                    _editStyle.Apply(bdr, SelectedState.None);
                    if (!field.IsString)
                    {
                        _numberStyle.Apply(bdr, SelectedState.None);
                    }
                }
            }
        }
    }
}
