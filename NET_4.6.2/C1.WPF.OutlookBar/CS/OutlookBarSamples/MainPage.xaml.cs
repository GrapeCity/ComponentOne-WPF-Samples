using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using C1.WPF.OutlookBar;
using C1.WPF;

namespace OutlookBarSamples
{
    public partial class DemoOutlookBar : UserControl
    {
        Popup window = new Popup();
        Border border = new Border() { BorderThickness = new Thickness(2), BorderBrush = new SolidColorBrush(Colors.LightGray) };
        public DemoOutlookBar()
        {
            InitializeComponent();
            transformerBorder.MouseEnter += new MouseEventHandler(transformerBorder_MouseEnter);
            window.Placement = PlacementMode.Relative;
            window.PlacementTarget = c1OutlookBar1;
            window.Child = border;
            border.MouseLeave += new MouseEventHandler(window_MouseLeave);
            c1OutlookBar1.SelectedItemChanged += new EventHandler<PropertyChangedEventArgs<object>>(c1OutlookBar1_SelectedItemChanged);
            c1OutlookBar1.SizeChanged += new SizeChangedEventHandler(c1OutlookBar1_SizeChanged);
        }

        void c1OutlookBar1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (window.IsOpen)
            {
                var item = c1OutlookBar1.SelectedItem as C1OutlookItem;
                FrameworkElement fe = border.Child as FrameworkElement;
                if (border.Child != null)
                {
                    border.Child = null;
                    item.Content = fe;
                }
                window.IsOpen = false;
                c1OutlookBar1.SelectedIndex = -1;
                c1OutlookBar1.SelectedItem = item;
            }
        }

        void c1OutlookBar1_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            if (window.IsOpen)
            {
                var oldItem = e.OldValue as C1OutlookItem;
                var newItem = e.NewValue as C1OutlookItem;
                FrameworkElement fe = border.Child as FrameworkElement;
                border.Child = null;
                oldItem.Content = fe;
                fe = newItem.Content as FrameworkElement;
                newItem.Content = null;
                border.Child = fe;
            }
        }

        void window_MouseLeave(object sender, MouseEventArgs e)
        {
            var item = c1OutlookBar1.SelectedItem as C1OutlookItem;
            FrameworkElement fe = border.Child as FrameworkElement;
            if (border.Child != null)
            {
                border.Child = null;
                item.Content = fe;
            }
            window.IsOpen = false;
        }

        void transformerBorder_MouseEnter(object sender, RoutedEventArgs e)
        {
            var item = c1OutlookBar1.SelectedItem as C1OutlookItem;
            FrameworkElement fe = item.Content as FrameworkElement;
            var point = transformerBorder.TranslatePoint(new Point(0, 0),c1OutlookBar1);
            if (border.Child == null)
            {
                item.Content = null;
                border.Child = fe;
            }
            border.Height = transformerBorder.ActualWidth;
            border.Width = c1OutlookBar1.ExpandedWidth;
            if (SystemParameters.MenuDropAlignment)
            {
                window.HorizontalOffset = 0;
            }
            else
            {
                window.HorizontalOffset = c1OutlookBar1.ActualWidth;
            }
            window.VerticalOffset = point.Y - transformerBorder.ActualWidth;
            window.IsOpen = true;
        }
    }
}
