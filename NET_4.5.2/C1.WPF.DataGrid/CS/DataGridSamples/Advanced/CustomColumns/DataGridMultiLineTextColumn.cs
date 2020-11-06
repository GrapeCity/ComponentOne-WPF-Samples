using System.Reflection;
using System.Windows;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public class DataGridMultiLineTextColumn : DataGridTextColumn
    {
        public DataGridMultiLineTextColumn()
            : base()
        {
        }

        public DataGridMultiLineTextColumn(PropertyInfo property)
            : base(property)
        {
        }

        public override FrameworkElement GetCellEditingContent(DataGridRow row)
        {
            var textBox = (C1TextBoxBase)base.GetCellEditingContent(row);
            textBox.KeyDown += (s, e) =>
                {
                    if (e.Key == System.Windows.Input.Key.Enter)
                    {
                        string newText = textBox.Text;
                        var selectionStart = textBox.SelectionStart;
                        newText = newText.Remove(selectionStart, textBox.SelectionLength);
                        newText = newText.Insert(selectionStart, "\n");
                        textBox.Text = newText;
                        textBox.Select(selectionStart + 1, 0);
                        e.Handled = true;
                        row.Presenter.InvalidateMeasure();
                    }
                };
            return textBox;
        }
    }
}
