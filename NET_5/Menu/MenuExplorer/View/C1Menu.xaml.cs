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
using C1.WPF.Core;
using C1.WPF.Menu;

namespace MenuExplorer
{
    /// <summary>
    /// Interaction logic for DemoMenu.xaml
    /// </summary>
    public partial class DemoMenu : UserControl
    {
        public DemoMenu()
        {
            InitializeComponent();
            Tag = "Use the C1Menu control to create menu and C1ContextMenu control to attach pop-up menu to your interface. Nest menus to any depth, use an icon for each menu item along with the menu label, and even make menu items checkable.";
        }

        #region ** ClearStyle

        #endregion

        private void C1MenuItem_Click(object sender, SourcedEventArgs e)
        {
            C1MenuItem menu = sender as C1MenuItem;
        }
    }
}
