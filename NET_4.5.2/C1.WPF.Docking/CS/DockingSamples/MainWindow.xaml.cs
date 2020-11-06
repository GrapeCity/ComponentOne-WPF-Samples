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
using System.Windows.Shapes;
using C1.WPF;
using C1.WPF.Docking;

namespace DockingSamples
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tab.SelectionChanged += new SelectionChangedEventHandler(tab_SelectionChanged);
            C1TabItem item1 = new C1TabItem();
            item1.Header = "See it in action";
            item1.Content = new DockingSamples.DockControlBasic();

            C1TabItem item2 = new C1TabItem();
            item2.Header = "Visual Studio";
            item2.Content = new DockingSamples.VisualStudioLookPage();
            tab.Items.Add(item1);
            tab.Items.Add(item2);

            C1TabItem item3 = new C1TabItem();
            item3.Header = "Blend Look";
            item3.Content = new DockingSamples.BlendLookPage();
            tab.Items.Add(item3);
        }

        void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RemoveFloatingWindow();
            if (e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0] as C1TabItem;
                if (item.Header.ToString() == "See it in action")
                {
                    item.Content = new DockingSamples.DockControlBasic();
                }
                else if (item.Header.ToString() == "Visual Studio")
                {
                    item.Content = new DockingSamples.VisualStudioLookPage();
                }
                else
                {
                    item.Content = new DockingSamples.BlendLookPage();
                }
            }
        }

        static List<C1DockTabControl> floatingList = new List<C1DockTabControl>();

        internal static void dockControl_ItemDockModeChanged(object sender, C1.WPF.Docking.ItemDockModeChangedEventArgs e)
        {
            if (e.NewValue == C1.WPF.Docking.DockMode.Floating)
            {
                if (!floatingList.Contains(e.TabControl))
                {
                    floatingList.Add(e.TabControl);
                }
            }
        }

        internal void RemoveFloatingWindow()
        {
            foreach (var tabControl in floatingList)
            {
                tabControl.DockMode = DockMode.Hidden;
            }
            floatingList.Clear();
        }

    }
}
