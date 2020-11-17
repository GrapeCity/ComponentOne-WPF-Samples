using System.Linq;
using System.Windows.Controls;
using C1.WPF.Accordion;
using ExpandDirection = C1.WPF.Accordion.ExpandDirection;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class Expander : UserControl
    {
        public Expander()
        {
            InitializeComponent();
            Tag = Properties.Resources.Expander;

            ExpandDirectionComboBox.ItemsSource = new ExpandDirection[]
            {
                ExpandDirection.Down,
                ExpandDirection.Up,
                ExpandDirection.Left,
                ExpandDirection.Right,
            };

            ExpandIconAlignmentComboBox.ItemsSource = new ExpanderIconAlignment[]
            {
                ExpanderIconAlignment.Left,
                ExpanderIconAlignment.Right,
            };
        }

        private void OnExpandDirectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpanderControl.ExpandDirection = e.AddedItems.Cast<ExpandDirection>().First();
        }

        private void OnExpandIconAlignmentChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpanderControl.ExpandIconAlignment = e.AddedItems.Cast<ExpanderIconAlignment>().First();
        }
    }
}
