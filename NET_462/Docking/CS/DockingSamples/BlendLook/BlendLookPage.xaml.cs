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
using C1.WPF.Docking;
using C1.WPF;
using DockingSamples.BlendLook;

namespace DockingSamples
{
    /// <summary>
    /// Interaction logic for BlendLookPage.xaml
    /// </summary>
    public partial class BlendLookPage : UserControl
    {
        public BlendLookPage()
        {
            InitializeComponent();
            UpdateWindowMenuItem();

            
            AddImplictStyle(typeof(BlendDockControl), typeof(C1DockControl));
            AddImplictStyle(typeof(BlendTabControl), typeof(C1DockTabControl));
            dockControl.ItemDockModeChanged += new EventHandler<C1.WPF.Docking.ItemDockModeChangedEventArgs>(MainWindow.dockControl_ItemDockModeChanged);
        }

        void AddImplictStyle(Type type, Type baseType)
        {
            theme.Resources.Add(type, new Style { TargetType = type, BasedOn = theme.Resources[baseType] as Style });
        }

        private void UpdateWindowMenuItem()
        {
            if (WindowMenuItem == null | MainArea == null)
                return;
            WindowMenuItem.Items.Clear();
            foreach (var item in MainArea.Items)
            {
                var tabItem = (C1DockTabItem)item; // must be fresh each loop iteration because of the delegate
                var menuItem = new C1MenuItem();
                menuItem.Header = tabItem.Header;
                menuItem.Click += delegate
                {
                    tabItem.IsSelected = true;
                };
                WindowMenuItem.Items.Add(menuItem);
            }
        }

        private void dockControl_ViewChanged(object sender, EventArgs e)
        {
            var list = new List<C1MenuItem>();
            foreach (var nestedTabControl in dockControl.NestedItems)
            {
                var tabControl = nestedTabControl; // must be fresh each loop iteration because of the delegate
                if (tabControl == MainArea)
                    continue;
                foreach (var item in tabControl.Items)
                {
                    var tabItem = (C1DockTabItem)item; // must be fresh each loop iteration because of the delegate
                    var menuItem = new C1MenuItem();
                    menuItem.Header = tabItem.Header;
                    menuItem.Click += delegate
                    {
                        switch (tabControl.DockMode)
                        {
                            case DockMode.Hidden:
                                tabControl.DockMode = DockMode.Docked;
                                break;
                            case DockMode.Sliding:
                                tabControl.SlideOpen();
                                break;
                        }
                        tabItem.IsSelected = true;
                    };
                    list.Add(menuItem);
                }
            }

            ViewMenuItem.Items.Clear();
            foreach (var menuItem in list.OrderBy(mi => mi.Header))
            {
                ViewMenuItem.Items.Add(menuItem);
            }
        }

        private void C1DockControl_PickerLoading(object sender, PickerLoadingEventArgs e)
        {
            if (e.Target == MainArea)
            {
                e.ShowOverInnerPart = false;
            }
            if (MainArea.Items.Contains(e.Source))
            {
                e.ShowLeftInnerPart = false;
                e.ShowLeftOuterPart = false;
                e.ShowTopInnerPart = false;
                e.ShowTopOuterPart = false;
                e.ShowRightInnerPart = false;
                e.ShowRightOuterPart = false;
                e.ShowBottomInnerPart = false;
                e.ShowBottomOuterPart = false;
                e.ShowOverInnerPart = false;
            }
        }
    }
}
