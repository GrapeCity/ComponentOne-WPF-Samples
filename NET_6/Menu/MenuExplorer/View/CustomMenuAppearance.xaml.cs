using C1.WPF.Core;
using C1.WPF.Menu;
using System.Windows.Controls;

namespace MenuExplorer
{
    /// <summary>
    /// Interaction logic for CustomMenuAppearance.xaml
    /// </summary>
    public partial class CustomMenuAppearance : UserControl
    {
        public CustomMenuAppearance()
        {
            InitializeComponent();
            Tag = Properties.Resources.CustomMenuAppearanceDesc;
        }

        private void C1MenuItem_Click(object sender, SourcedEventArgs e)
        {
            C1MenuItem menu = sender as C1MenuItem;
        }
    }
}
