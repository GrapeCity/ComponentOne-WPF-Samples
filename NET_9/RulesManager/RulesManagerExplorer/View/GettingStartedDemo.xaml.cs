using System.Windows.Controls;

namespace RulesManagerExplorer
{
    public partial class GettingStartedDemo : UserControl
    {
        public GettingStartedDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.GettingStartedDescription;
        }
    }
}
