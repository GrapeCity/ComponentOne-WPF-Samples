using System.Windows.Controls;
using C1.WPF.Accordion;
using ExpandDirection = C1.WPF.Accordion.ExpandDirection;

namespace AccordionExplorer
{
    /// <summary>
    /// Interaction logic for ExpanderSample.xaml
    /// </summary>
    public partial class ExpanderSample : UserControl
    {
        public ExpanderSample()
        {
            InitializeComponent();
            Tag = Properties.Resources.ExpanderDesc;

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
    }
}
