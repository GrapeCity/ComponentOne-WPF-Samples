using C1.WPF.Core;
using C1.WPF.Menu;
using System.Windows.Controls;

namespace MenuExplorer
{
    public partial class Tiles : UserControl
    {
        public Tiles()
        {
            InitializeComponent();
            Tag = Properties.Resources.TilesDesc;
        }

        private void C1MenuItem_Click(object sender, SourcedEventArgs e)
        {
            C1MenuItem menu = sender as C1MenuItem;
        }

        private void OnMenuItemClick(object sender, SourcedEventArgs e)
        {
            Message.Text = e.Source.ToString();
        }
    }
}
