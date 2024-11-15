using C1.WPF.Input;
using C1.WPF.TabControl;
using System;
using System.Windows.Controls;

namespace TabControlExplorer
{
    /// <summary>
    /// Interaction logic for DemoTabControl.xaml
    /// </summary>
    public partial class TabPositions : UserControl
    {
        public TabPositions()
        {
            InitializeComponent();
            Tag = Properties.Resources.TabPosition;
        }

        private void tabShape_SelectedItemChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<object> e)
        {
            try
            {
                if (e.NewValue == null) return;
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
                    c1Tab.TabItemRotation = Convert.ToInt32(e.NewValue);
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
                    c1Tab.TabStripPlacement = (Dock)Enum.Parse(typeof(Dock), e.NewValue as string);
                } 
                catch { }
            }
        }
    }
}