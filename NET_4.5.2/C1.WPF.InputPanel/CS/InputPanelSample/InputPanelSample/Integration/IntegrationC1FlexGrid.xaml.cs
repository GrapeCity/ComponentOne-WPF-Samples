using C1.WPF;
using C1.WPF.FlexGrid;
using C1.WPF.InputPanel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InputPanelSample
{
    /// <summary>
    /// Interaction logic for IntegrationC1FlexGrid.xaml
    /// </summary>
    public partial class IntegrationC1FlexGrid : UserControl
    {       
        public IntegrationC1FlexGrid()
        {
            InitializeComponent();
            DataContext = new Data();
        }

    }
}
