using FinancialChartExplorer.Resources;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for HeikinAshi.xaml
    /// </summary>
    public partial class HeikinAshi : UserControl
    {
        DataService dataService = DataService.GetService();

        public HeikinAshi()
        {
            InitializeComponent();

            Tag = AppResources.HeikinAshiTag;
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

        public List<Company> Companies
        {
            get
            {
                return dataService.GetCompanies();
            }
        }
    }
}
