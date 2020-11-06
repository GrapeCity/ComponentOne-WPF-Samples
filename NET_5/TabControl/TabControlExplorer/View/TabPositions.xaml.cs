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
using C1.WPF.Input;
using C1.WPF.TabControl;

namespace TabControlExplorer
{
    /// <summary>
    /// Interaction logic for DemoTabControl.xaml
    /// </summary>
    public partial class TabPositions : UserControl
    {
        private bool _closing = false;

        public TabPositions()
        {
            InitializeComponent();
            Tag = "The tabs can be placed at the top, bottom, left, or right of the control. Rotate the tab content and customize the tab shape.";
        }

        private void tabShape_SelectedItemChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<object> e)
        {
            try
            {
                C1TabItemShape val = (C1TabItemShape)Enum.Parse(typeof(C1TabItemShape), (e.NewValue as C1ComboBoxItem).Content as string);
                if (c1Tab != null)
                {
                    foreach (C1TabItem item in c1Tab.Items)
                    {
                        item.TabShape = val;
                    }
                }
            } catch { }
        }

        void tabItemRotation_SelectedValueChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<object> e)
        {
            if (c1Tab != null)
            {
                try
                {
                    c1Tab.TabItemRotation = Convert.ToInt32((e.NewValue as C1ComboBoxItem).Content);
                }
                catch { }
            }
        }

        void tabStripPlacement_SelectedValueChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<object> e)
        {
            if (c1Tab != null)
            {
                try
                {
                    c1Tab.TabStripPlacement = (Dock)Enum.Parse(typeof(Dock), (e.NewValue as C1ComboBoxItem).Content as string);
                } 
                catch { }
            }
        }
    }
}