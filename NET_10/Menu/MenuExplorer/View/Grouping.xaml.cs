using System.Windows.Controls;

namespace MenuExplorer
{
    public partial class Grouping : UserControl
    {
        public Grouping()
        {
            InitializeComponent();
            Tag = Properties.Resources.GroupingDesc;
        }
    }
}
