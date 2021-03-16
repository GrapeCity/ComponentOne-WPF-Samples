using System.Windows.Controls;

namespace MenuExplorer
{
    public partial class DropDownMenu : UserControl
    {
        public DropDownMenu()
        {
            InitializeComponent();
            Tag = Properties.Resources.DropDownMenuDesc;
        }
    }
}
