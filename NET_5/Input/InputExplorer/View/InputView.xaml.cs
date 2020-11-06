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
            Tag = "This library includes several WPF input controls for any data entry scenario. Format and parse numbers, dates, colors, and masked text. Select multiple items from a checklist or enter items like a tag editor. Edit a numeric range visually using a slider. Design a custom drop-down UI with ease. Select files from the user's machine.";
        }
    }
}
