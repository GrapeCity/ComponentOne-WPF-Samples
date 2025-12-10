using C1.WPF.Accordion;
using C1.WPF.Input;
using System;
using System.Linq;
using System.Windows.Controls;
using ExpandDirection = C1.WPF.Accordion.ExpandDirection;

namespace AccordionExplorer
{
    /// <summary>
    /// Interaction logic for AccordionSample.xaml
    /// </summary>
    public partial class AccordionSample : UserControl
    {
        public AccordionSample()
        {
            InitializeComponent();
            Tag = Properties.Resources.AccordionDesc;
            ExpandDirectionComboBox.ItemsSource = new ExpandDirection[]
            {
                ExpandDirection.Down,
                ExpandDirection.Up,
                ExpandDirection.Left,
                ExpandDirection.Right,
            };

            ExpandModeComboBox.ItemsSource = new ExpandMode[]
            {
                ExpandMode.One,
                ExpandMode.Collapsible,
                ExpandMode.Any,
            };

        }

    }
}
