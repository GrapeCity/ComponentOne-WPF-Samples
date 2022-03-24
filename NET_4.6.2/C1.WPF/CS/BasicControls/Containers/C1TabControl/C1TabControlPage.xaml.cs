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

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoTabControl.xaml
    /// </summary>
    public partial class DemoTabControl : UserControl
    {
        private bool _closing = false;

        public DemoTabControl()
        {
            InitializeComponent();
        }

        #region Object model

        public bool CanUserReorder
        {
            get { return c1Tab.CanUserReorder; }
            set { c1Tab.CanUserReorder = value; }
        }

        public C1TabItemCloseOptions TabItemClose
        {
            get { return c1Tab.TabItemClose; }
            set { c1Tab.TabItemClose = value; }
        }
        public C1TabItemShape TabItemShape
        {
            get { return c1Tab.TabItemShape; }
            set { c1Tab.TabItemShape = value; }
        }
        public Visibility TabStripMenuVisibility
        {
            get { return c1Tab.TabStripMenuVisibility; }
            set { c1Tab.TabStripMenuVisibility = value; }
        }
        public int TabStripOverlap
        {
            get { return c1Tab.TabStripOverlap; }
            set { c1Tab.TabStripOverlap = value; }
        }
        public C1TabPanelOverlapDirection TabStripOverlapDirection
        {
            get { return c1Tab.TabStripOverlapDirection; }
            set { c1Tab.TabStripOverlapDirection = value; }
        }
        public C1.WPF.Dock TabStripPlacement
        {
            get { return c1Tab.TabStripPlacement; }
            set { c1Tab.TabStripPlacement = value; }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return c1Tab.Background;
            }
            set
            {
                c1Tab.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return c1Tab.Foreground;
            }
            set
            {
                c1Tab.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return c1Tab.BorderBrush;
            }
            set
            {
                c1Tab.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return c1Tab.MouseOverBrush;
            }
            set
            {
                c1Tab.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return c1Tab.PressedBrush;
            }
            set
            {
                c1Tab.PressedBrush = value;
            }
        }

        public Brush SelectedBackground
        {
            get
            {
                return c1Tab.SelectedBackground;
            }
            set
            {
                c1Tab.SelectedBackground = value;
            }
        }

        public Brush TabStripBackground
        {
            get
            {
                return c1Tab.TabStripBackground;
            }
            set
            {
                c1Tab.TabStripBackground = value;
            }
        }

        public Brush TabStripForeground
        {
            get
            {
                return c1Tab.TabStripForeground;
            }
            set
            {
                c1Tab.TabStripForeground = value;
            }
        }
        
        #endregion

        #endregion

        private void tabs_TabClosing(object sender, CancelSourceEventArgs e)
        {
            if (!_closing)
            {
                _closing = true;
                C1TabItem tabItem = (C1TabItem)e.Source;
                e.Cancel = true;
                MessageBoxResult result = MessageBox.Show("Do you really want to close this tab?", "Close Tabs?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    c1Tab.Items.Remove(tabItem);
                }
                _closing = false;
            }
        }

        private void C1TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // create new item
            var item = new C1TabItem()
            {
                Header = string.Format("Item {0}", c1Tab.Items.Count)
            };

            c1Tab.Items.Insert(c1Tab.Items.Count - 1, item);
            c1Tab.SelectedItem = item;
        }
    }
}
