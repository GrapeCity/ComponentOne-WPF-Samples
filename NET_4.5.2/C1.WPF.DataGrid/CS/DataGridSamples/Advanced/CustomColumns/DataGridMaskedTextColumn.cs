using System.Reflection;
using System.Windows;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public class DataGridMaskedTextColumn : DataGridTextColumn
    {
        public DataGridMaskedTextColumn()
            : base()
        {
            Initialize();
        }

        public DataGridMaskedTextColumn(PropertyInfo property)
            : base(property)
        {
            Initialize();
        }

        private void Initialize()
        {
            TextMaskFormat = MaskFormat.IncludeLiterals;
        }

        public string Mask { get; set; }

        public MaskFormat TextMaskFormat { get; set; }

        public override FrameworkElement GetCellEditingContent(DataGridRow row)
        {
            var txt = new C1MaskedTextBox();
            txt.Mask = Mask;
            txt.TextMaskFormat = TextMaskFormat;
            txt.TextWrapping = TextWrapping;
            txt.Padding = new Thickness(0);
            if (Binding != null)
            {
                txt.SetBinding(C1MaskedTextBox.ValueProperty, CopyBinding(Binding));
            }
            txt.TextAlignment = HorizontalAlignment == HorizontalAlignment.Left ? TextAlignment.Left : HorizontalAlignment == HorizontalAlignment.Right ? TextAlignment.Right : TextAlignment.Center;
            txt.VerticalContentAlignment = VerticalAlignment;
            if (CellEditingContentStyle != null && txt.Style == null)
            {
                txt.Style = CellEditingContentStyle;
            }
            return txt;
        }
    }
}
