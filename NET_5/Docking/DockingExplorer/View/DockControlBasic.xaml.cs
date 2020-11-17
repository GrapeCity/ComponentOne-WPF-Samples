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
using DockingExplorer.Resources;

namespace DockingExplorer
{
    /// <summary>
    /// Interaction logic for DockControlSaveLayout.xaml
    /// </summary>
    public partial class DockControlBasic : UserControl
    {
        public DockControlBasic()
        {
            InitializeComponent();
            Tag = AppResources.OverviewTag;
            dockControl.ItemDockModeChanged += new EventHandler<C1.WPF.Docking.ItemDockModeChangedEventArgs>(MainWindow.dockControl_ItemDockModeChanged);

            Unloaded += delegate
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is WPFWindow)
                        w.Close();
                }
            };
        }
    }
}
