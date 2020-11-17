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
using C1.WPF.Chart.Finance;
using FinancialChartExplorer.Resources;

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for ColumnVolume.xaml
    /// </summary>
    public partial class ColumnVolume : UserControl
    {
        DataService dataService = DataService.GetService();

        public ColumnVolume()
        {
            InitializeComponent();
            this.Loaded += ColumnVolume_Loaded;
            Tag = AppResources.ColumnVolumeTag;
        }

        void ColumnVolume_Loaded(object sender, RoutedEventArgs e)
        {
            financialChart.Series.Clear();
            financialChart.Series.Add(new FinancialSeries());
            cbSymbol.SelectedIndex = 0;
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

        public List<Company> Companies
        {
            get
            {
                return dataService.GetCompanies();
            }
        }
    }
}
