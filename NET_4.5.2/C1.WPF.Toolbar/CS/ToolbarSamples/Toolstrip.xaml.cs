using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using C1.WPF;
using C1.WPF.Toolbar;

namespace ToolbarSamples
{
    /// <summary>
    /// Interaction logic for Toolstrip.xaml
    /// </summary>
    public partial class Toolstrip : UserControl
    {
        ScaleTransform st = new ScaleTransform();

        public Toolstrip()
        {
            InitializeComponent();

            tb.OverflowItemAdded += (s, e) =>
            {
                if (s is C1Separator)
                {
                    var mi = new C1Separator();
                    tb.OverflowMenuItems.Add(mi);
                    ((C1Separator)s).Tag = mi;
                }
                else if (s is FrameworkElement)
                {
                    var mi = new MenuItem() { Header = ToolTipService.GetToolTip((FrameworkElement)s) };
                    tb.OverflowMenuItems.Add(mi);
                    ((FrameworkElement)s).Tag = mi;
                }
            };
            tb.OverflowItemRemoved += (s, e) =>
            {
                FrameworkElement fe = s as FrameworkElement;
                if (fe != null)
                    tb.OverflowMenuItems.Remove(fe.Tag);
            };

            img.RenderTransform = st;
            img.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tb == null)
                return;
            RadioButton rb = (RadioButton)sender;
            if ((rb.Content as string) == "None")
                tb.Overflow = ToolbarOverflow.None;
            else if ((rb.Content as string) == "Menu")
                tb.Overflow = ToolbarOverflow.Menu;
            else if ((rb.Content as string) == "Panel")
                tb.Overflow = ToolbarOverflow.Panel;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked == true)
                Orientation = Orientation.Vertical;
            else
                Orientation = Orientation.Horizontal;
        }

        public Orientation Orientation
        {
            get { return tb.Orientation; }
            set
            {
                tb.Orientation = value;
                if (tb.Orientation == Orientation.Vertical)
                {
                    Grid.SetRowSpan(tb, 2);
                    Grid.SetColumnSpan(tb, 1);

                    Grid.SetRow(cb, 0);
                }
                else
                {
                    Grid.SetRowSpan(tb, 1);
                    Grid.SetColumnSpan(tb, 2);

                    Grid.SetRow(cb, 1);
                }
            }
        }

        public ToolbarOverflow Overflow
        {
            get { return tb.Overflow; }
            set { tb.Overflow = value; }
        }

        private void OriginalSize_Click(object sender, RoutedEventArgs e)
        {
            SetImageScale(1);
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            SetImageScale(st.ScaleX * 2);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            SetImageScale(st.ScaleX * 0.5);
        }

        private void SetImageScale(double scale)
        {
            st.ScaleX = st.ScaleY = scale;
            //imgbdr.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, LayoutRoot.ActualWidth, LayoutRoot.ActualHeight) };

            btn1_1.IsEnabled = scale != 1;
            btnZoomIn.IsEnabled = scale < 8;
            btnZoomOut.IsEnabled = scale > 1.0 / 8;
        }

        #region Object model

        public bool IsEnabled
        {
            get
            {
                return tb.IsEnabled;
            }
            set
            {
                tb.IsEnabled = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return tb.Background;
            }
            set
            {
                tb.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return tb.Foreground;
            }
            set
            {
                tb.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return tb.BorderBrush;
            }
            set
            {
                tb.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return tb.MouseOverBrush;
            }
            set
            {
                tb.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return tb.PressedBrush;
            }
            set
            {
                tb.PressedBrush = value;
            }
        }

        public Brush ButtonBackground
        {
            get
            {
                return tb.ButtonBackground;
            }
            set
            {
                tb.ButtonBackground = value;
            }
        }

        public Brush ButtonForeground
        {
            get
            {
                return tb.ButtonForeground;
            }
            set
            {
                tb.ButtonForeground = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return tb.FocusBrush;
            }
            set
            {
                tb.FocusBrush = value;
            }
        }

        #endregion

        #endregion

    }
}
