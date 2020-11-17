using C1.WPF.Core;
using C1.WPF.Menu;
using System.Windows.Controls;

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
            Tag = Properties.Resources.Menu;
        }

        #region ** ClearStyle

        #endregion

        private void C1MenuItem_Click(object sender, SourcedEventArgs e)
        {
            C1MenuItem menu = sender as C1MenuItem;
        }
    }
}
