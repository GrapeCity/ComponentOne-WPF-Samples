using C1.WPF.Core;
using C1.WPF.Input;
using C1.WPF.TabControl;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TabControlExplorer
{
    /// <summary>
    /// Interaction logic for DemoTabControl.xaml
    /// </summary>
    public partial class SimplifiedIndicator : UserControl
    {
        public SimplifiedIndicator()
        {
            InitializeComponent();
            Tag = Properties.Resources.Indicator;
        }

        void tabStripPlacement_SelectedValueChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            if (c1Tab != null)
            {
                try
                {
                    Dock val = (Dock)Enum.Parse(typeof(Dock), e.NewValue as string);
                    if (val == Dock.Left || val == Dock.Right)
                    {
                        (indicatorPlacement.Items[0] as C1ComboBoxItem).IsEnabled = false;
                        (indicatorPlacement.Items[1] as C1ComboBoxItem).IsEnabled = false;
                        (indicatorPlacement.Items[2] as C1ComboBoxItem).IsEnabled = true;
                        (indicatorPlacement.Items[3] as C1ComboBoxItem).IsEnabled = true;
                        indicatorPlacement.SelectedIndex = 3;
                    }
                    else
                    {
                        (indicatorPlacement.Items[0] as C1ComboBoxItem).IsEnabled = true;
                        (indicatorPlacement.Items[1] as C1ComboBoxItem).IsEnabled = true;
                        (indicatorPlacement.Items[2] as C1ComboBoxItem).IsEnabled = false;
                        (indicatorPlacement.Items[3] as C1ComboBoxItem).IsEnabled = false;
                        indicatorPlacement.SelectedIndex = 1;
                    }
                    c1Tab.TabStripPlacement = val;
                }
                catch { }
            }
        }

        void C1TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // create new item
            var item = new C1TabItem()
            {
                Header = string.Format("Item {0}", c1Tab.Items.Count),
                TabShape = C1TabItemShape.Rectangle,
                BorderBrush = Brushes.Transparent
            };

            c1Tab.Items.Insert(c1Tab.Items.Count - 1, item);
            c1Tab.SelectedItem = item;
        }

        void indicatorPlacement_SelectedValueChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            if (c1Tab != null)
            {
                try
                {
                    c1Tab.IndicatorPlacement = (Dock)Enum.Parse(typeof(Dock), e.NewValue as string);
                }
                catch { }
            }
        }

        private void indicatorVisible_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            if (c1Tab != null)
            {
                try
                {
                    c1Tab.IndicatorVisibility = (Visibility)Enum.Parse(typeof(Visibility), e.NewValue as string);
                }
                catch { }
            }
        }
    }
}
