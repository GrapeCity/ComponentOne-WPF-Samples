using C1.Chart.Finance;
using FinancialChartExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            Tag = AppResources.KagiTag;
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
            if (financialChart != null && e.AddedItems.Count > 0)
            {
                var c = e.AddedItems[0] as Company;
                var data = dataService.GetSymbolData(c.Symbol);
                financialChart.BeginUpdate();
                financialChart.ItemsSource = data;
                financialChart.EndUpdate();
            }
        }

        private void OnRangeModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0) return;
            financialChart.BeginUpdate();
            var rmode = e.AddedItems[0] as string;
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
