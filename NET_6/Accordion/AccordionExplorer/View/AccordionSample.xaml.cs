using C1.WPF.Accordion;
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
        }

        void ExpandModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (accordion != null && e.AddedItems.Count > 0)
            {
                accordion.ExpandMode = (ExpandMode) Enum.Parse(typeof(ExpandMode), (string)e.AddedItems[0]);
            }
        }

        private void OnExpandDirectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                accordion.ExpandDirection = e.AddedItems.Cast<ExpandDirection>().First();
            }
        }

    }
}
