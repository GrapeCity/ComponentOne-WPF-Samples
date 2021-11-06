using C1.WPF.Accordion;
using System;
using System.Windows.Controls;

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
        }

        void ExpandModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (accordion != null && e.AddedItems.Count > 0)
            {
                accordion.ExpandMode = (ExpandMode) Enum.Parse(typeof(ExpandMode), (string)e.AddedItems[0]);
            }
        }
    }
}
