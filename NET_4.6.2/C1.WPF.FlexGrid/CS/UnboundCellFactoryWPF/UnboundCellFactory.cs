using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using C1.WPF.FlexGrid;
using C1.WPF.Extended;

namespace UnboundCellFactoryWPF
{
    public class UnboundCellFactory : CellFactory
    {
        C1FlexGrid _flex;

        // ctor
        public UnboundCellFactory(C1FlexGrid flex)
        {
            _flex = flex;
            _flex.ScrollPositionChanging += flex_ScrollPositionChanging;
            _flex.BeginningEdit += flex_BeginningEdit;
        }

        // finish edits before scrolling
        void flex_ScrollPositionChanging(object sender, EventArgs e)
        {
            _flex.FinishEditing();
        }

        // cancel standard editing for our custom cells (they are editors already)
        void flex_BeginningEdit(object sender, CellEditEventArgs e)
        {
            var t = GetCellType(e.CellRange);
            if (t != null)
            {
                e.Cancel = true;
            }
        }

        // create the custom cell editor
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            var t = GetCellType(rng);
            if (t != null)
            {
                // cell has a custom type, create editor
                bdr.Child = CreateEditor(t, rng);
            }
            else
            {
                // default handling
                base.CreateCellContent(grid, bdr, rng);
            }
        }

        // select the element type for a cell
        Type GetCellType(CellRange rng)
        {
            var c = _flex.Columns[rng.Column];
            if (c.ColumnName == "Data")
            {
                var r = _flex.Rows[rng.Row];
                return r.Tag as Type;
            }
            return null;
        }

        // initialize custom cell editor
        UIElement CreateEditor(Type t, CellRange rng)
        {
            // create cell element, save range in Tag property
            var edt = Activator.CreateInstance(t) as FrameworkElement;
            edt.Tag = rng;
            edt.VerticalAlignment = VerticalAlignment.Center;

            // get current value from grid
            var value = _flex[rng.Row, rng.Column];

            // initialize each editor type
            var textBox = edt as TextBox;
            if (textBox != null)
            {
                textBox.Text = value is string ? (string)value : string.Empty;
                textBox.LostFocus += StoreCellValue;
            }

            var checkBox = edt as CheckBox;
            if (checkBox != null)
            {
                checkBox.HorizontalAlignment = HorizontalAlignment.Center;
                checkBox.VerticalAlignment = VerticalAlignment.Center;
                checkBox.IsChecked = value is bool ? (bool)value : false;
                checkBox.LostFocus += StoreCellValue;
            }

            var comboBox = edt as ComboBox;
            if (comboBox != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    comboBox.Items.Add("Item " + i.ToString());
                }
                comboBox.SelectedValue = value is string ? (string)value : comboBox.Items[0];
                comboBox.LostFocus += StoreCellValue;
            }

            var slider = edt as Slider;
            if (slider != null)
            {
                slider.Minimum = 0;
                slider.Maximum = 50;
                slider.Value = value is double ? (double)value : 0;
                slider.LostFocus += StoreCellValue;
            }

            var colorPicker = edt as C1ColorPicker;
            if (colorPicker != null)
            {
                colorPicker.SelectedColor = value is Color ? (Color)value : Colors.White;
                colorPicker.SelectedColorChanged += StoreCellValue;
            }

            // done
            edt.VerticalAlignment = VerticalAlignment.Center;
            return edt;
        }

        // finalize custom cell editor
        public override void DisposeCell(C1FlexGrid grid, CellType cellType, FrameworkElement cell)
        {
            var edt = ((Border)cell).Child as FrameworkElement;
            if (edt != null)
            {
                var colorPicker = edt as C1ColorPicker;
                if (colorPicker != null)
                {
                    colorPicker.SelectedColorChanged -= StoreCellValue;
                }
                else
                {
                    edt.LostFocus -= StoreCellValue;
                }
            }
            base.DisposeCell(grid, cellType, cell);
        }

        // store edited value into grid cell when the editor loses focus/changes
        void StoreCellValue(object sender, EventArgs e)
        {
            // get editor and range
            var edt = sender as FrameworkElement;
            var rng = (CellRange)edt.Tag;

            // apply value
            var textBox = edt as TextBox;
            if (textBox != null)
            {
                _flex[rng.Row, rng.Column] = textBox.Text;
            }
            var checkBox = edt as CheckBox;
            if (checkBox != null)
            {
                _flex[rng.Row, rng.Column] = checkBox.IsChecked;
            }

            var comboBox = edt as ComboBox;
            if (comboBox != null)
            {
                _flex[rng.Row, rng.Column] = comboBox.SelectedValue;
            }

            var slider = edt as Slider;
            if (slider != null)
            {
                _flex[rng.Row, rng.Column] = slider.Value;
            }

            var colorPicker = edt as C1ColorPicker;
            if (colorPicker != null)
            {
                _flex[rng.Row, rng.Column] = colorPicker.SelectedColor;
            }
        }
    }
}