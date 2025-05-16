using FinancialChartExplorer.Resources;
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
    /// Interaction logic for ArmsCandleVolume.xaml
    /// </summary>
    public partial class ArmsCandleVolume : UserControl
    {
        DataService dataService = DataService.GetService();

        public ArmsCandleVolume()
        {
            InitializeComponent();
            this.Loaded += ArmsCandleVolume_Loaded;
            Tag = AppResources.ArmsCandleTag;
        }

        void ArmsCandleVolume_Loaded(object sender, RoutedEventArgs e)
        {
            cbSymbol.SelectedIndex = 0;
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
