using C1.WPF.Chart;
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

namespace DrillDown
{
    /// <summary>
    /// Interaction logic for PieColumnDrillDown2.xaml
    /// </summary>
    public partial class BasicDrillDownDemo : UserControl
    {
        private DrillDownViewModel _vm;
        private bool isDrillDownEnabled = true;

        public BasicDrillDownDemo()
        {
            InitializeComponent();
            _vm = new DrillDownViewModel();
            this.DataContext = _vm;
            _vm.Manager.BeforeDrill += Manager_BeforeDrill;
            _vm.Manager.AfterDrill += Manager_AfterDrill;
            _vm.Manager.Refresh();
        }

        private void Manager_AfterDrill(object sender, DrillDownEventArgs e)
        {
            if (flexChart.Visibility == Visibility.Visible)
            {
                switch (e.DrillDownPath)
                {
                    case "Country":
                    case "City":
                    default:
                        flexChart.ChartType = C1.Chart.ChartType.Column;
                        break;
                    case "Year":
                        flexChart.ChartType = C1.Chart.ChartType.LineSymbols;
                        break;
                }
            }
            flexChart.Footer = pieChart.Footer = string.Format("{0}-wise sales", e.DrillDownPath);
        }

        private void Manager_BeforeDrill(object sender, DrillDownEventArgs e)
        {
            e.Cancel = !isDrillDownEnabled;
        }

        private void HandleNormalChartDrillDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                var flexChart = sender as FlexChartBase;
                var hitInfo = flexChart.HitTest(e.GetPosition(flexChart));
                if (hitInfo != null && hitInfo.Distance < 2)
                {
                    _vm.Manager.DrillDown(hitInfo.PointIndex);
                }
                return;
            }
        }

        private void OnCheckChanged(object sender, RoutedEventArgs e)
        {
            isDrillDownEnabled = (bool)chkEnableDrillDown.IsChecked;
        }
    }
}
