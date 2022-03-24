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

namespace InputPanelSample
{
    /// <summary>
    /// Interaction logic for IntegrationC1DataGrid.xaml
    /// </summary>
    public partial class IntegrationC1DataGrid : UserControl
    {
        public IntegrationC1DataGrid()
        {
            InitializeComponent();
            DataContext = new Data();
        }
    }
}
