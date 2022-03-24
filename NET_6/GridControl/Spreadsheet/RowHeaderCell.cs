using System.Windows;
using System.Windows.Controls;

namespace Spreadsheet
{
    public class RowHeaderCell : Control
    {
        static RowHeaderCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RowHeaderCell), new FrameworkPropertyMetadata(typeof(RowHeaderCell)));
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(RowHeaderCell), new PropertyMetadata(""));
    }
}
