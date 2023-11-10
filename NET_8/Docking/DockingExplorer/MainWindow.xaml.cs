using C1.WPF.Docking;
using System.Collections.Generic;
using System.Windows;

namespace DockingExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = DockingExplorer.Resources.AppResources.Title;
        }

        static List<C1DockTabControl> floatingList = new List<C1DockTabControl>();

        internal static void dockControl_ItemDockModeChanged(object sender, ItemDockModeChangedEventArgs e)
        {
            if (e.NewValue == DockMode.Floating)
            {
                if (!floatingList.Contains(e.TabControl))
                {
                    floatingList.Add(e.TabControl);
                }
            }
        }

        internal static void RemoveFloatingWindow()
        {
            foreach (var tabControl in floatingList)
            {
                tabControl.DockMode = DockMode.Hidden;
            }
            floatingList.Clear();
        }

    }
}
