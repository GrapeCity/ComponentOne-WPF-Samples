using C1.WPF.Ribbon;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RibbonExplorer
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
            Tag = Properties.Resources.ToolStrip;

            img.RenderTransform = st;
            img.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tb == null)
                return;
            RadioButton rb = (RadioButton)sender;
            if ((rb.Content as string) == "None")
                tb.Overflow = ToolStripOverflow.None;
            else if ((rb.Content as string) == "Menu")
                tb.Overflow = ToolStripOverflow.Menu;
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

        public ToolStripOverflow Overflow
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

        private void OnProjectClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
