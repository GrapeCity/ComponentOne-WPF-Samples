using C1.Chart.Finance;
using System;
using System.Collections.Generic;
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

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for Kagi.xaml
    /// </summary>
    public partial class Kagi : UserControl
    {
        DataService dataService = DataService.GetService();

        public Kagi()
        {
            InitializeComponent();
            this.Loaded += Kagi_Loaded;
            Tag = "A Kagi chart displays supply and demand trends using a sequence of linked vertical lines. The thickness and direction of the lines vary depending on the price movement. If closing prices go in the direction of the previous Kagi line, then that Kagi line is extended. However, if the closing price reverses by the preset reversal amount, a new Kagi line is charted in the next column in the opposite direction. Thin lines indicate that the price breaks the previous low (supply) while thick lines indicate that the price breaks the previous high (demand).";
        }

        public List<Company> Companies
        {
            get
            {
                return dataService.GetCompanies();
            }
        }

        public List<string> RangeMode
        {
            get
            {
                return Enum.GetNames(typeof(RangeMode)).ToList();
            }
        }

        public List<string> DataFields
        {
            get
            {
                return Enum.GetNames(typeof(DataFields)).ToList();
            }
        }

        void Kagi_Loaded(object sender, RoutedEventArgs e)
        {
            cbSymbol.SelectedIndex = 0;
            rangeMode.SelectedIndex = 0;
        }

        void OnSymbolSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (financialChart != null)
            {
                Company c = cbSymbol.SelectedValue as Company;
                var data = dataService.GetSymbolData(c.Symbol);
                financialChart.BeginUpdate();
                financialChart.ItemsSource = data;
                financialChart.EndUpdate();
            }
        }

        private void OnRangeModeChanged(object sender, SelectionChangedEventArgs e)
        {
            financialChart.BeginUpdate();
            var rmode = rangeMode.SelectedValue as string;
            if (rmode == "Fixed")
            {
                reversalAmount.Minimum = 1;
                reversalAmount.Maximum = 14;
                reversalAmount.Increment = 1;
                reversalAmount.Format = "0";
                financialChart.Options.RangeMode = C1.Chart.Finance.RangeMode.Fixed;
            }
            else if ( rmode == "ATR")
            {
                reversalAmount.Minimum = 2;
                reversalAmount.Maximum = 14;
                reversalAmount.Increment = 1;
                reversalAmount.Format = "0";
                if (financialChart.Options.ReversalAmount < 2)
                    financialChart.Options.ReversalAmount = 2;
                financialChart.Options.RangeMode = C1.Chart.Finance.RangeMode.ATR;
            }
            else
            {
                reversalAmount.Minimum = 0;
                reversalAmount.Maximum = 1;
                reversalAmount.Format = "0.00";
                reversalAmount.Increment = 0.05;
                if (reversalAmount.Value >= 1)
                    reversalAmount.Value = 0.05;
                financialChart.Options.RangeMode = C1.Chart.Finance.RangeMode.Percentage;
            }
            financialChart.EndUpdate();
        }
    }
}
