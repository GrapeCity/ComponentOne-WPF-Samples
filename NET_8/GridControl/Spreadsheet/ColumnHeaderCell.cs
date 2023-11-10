using System.Windows;
using System.Windows.Controls;

namespace Spreadsheet
{
    public class ColumnHeaderCell : Control
    {
        static ColumnHeaderCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnHeaderCell), new FrameworkPropertyMetadata(typeof(ColumnHeaderCell)));
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ColumnHeaderCell), new PropertyMetadata(""));
    }
}
