using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace StockAnalysis.Partial.UserControls
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }
        
        public ViewModel.ViewModel Model
        {
            get { return ViewModel.ViewModel.Instance; }
        }
        private void indicatorsDropdown_DropDownOpened(object sender, EventArgs e)
        {
            TryShowDisableMessage(String.Message.DisableIndicatorMessage);
        }

        private void overlaysDropdown_DropDownOpened(object sender, EventArgs e)
        {
            TryShowDisableMessage(String.Message.DisableOverlayMessage);
        }

        private void TryShowDisableMessage(string msg)
        {
            if (ViewModel.ViewModel.Instance.ChartType == C1.Chart.Finance.FinancialChartType.Kagi ||
                ViewModel.ViewModel.Instance.ChartType == C1.Chart.Finance.FinancialChartType.Renko ||
                ViewModel.ViewModel.Instance.ChartType == C1.Chart.Finance.FinancialChartType.PointAndFigure)
            {
                MessageBox.Show(msg);
            }
        }
    }
}
