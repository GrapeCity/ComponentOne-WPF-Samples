using System.Windows;
using System.Windows.Controls;

namespace Spreadsheet
{
    public class TopLeftCell : Control
    {
        static TopLeftCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TopLeftCell), new FrameworkPropertyMetadata(typeof(TopLeftCell)));
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(TopLeftCell), new PropertyMetadata(""));
    }
}
