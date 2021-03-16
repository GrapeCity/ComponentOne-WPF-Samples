using C1.WPF;
using C1.WPF.Accordion;
using C1.WPF.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            if (accordion != null)
            {
                accordion.ExpandMode = (ExpandMode) Enum.Parse(typeof(ExpandMode), (string)((sender as ComboBox).SelectedItem as ComboBoxItem).Content);
            }
        }
    }
}
