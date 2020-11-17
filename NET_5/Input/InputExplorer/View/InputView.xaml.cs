using System.Windows.Controls;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class InputView : UserControl
    {
        public InputView()
        {
            InitializeComponent();
            Tag = Properties.Resources.Input;
        }
    }
}
