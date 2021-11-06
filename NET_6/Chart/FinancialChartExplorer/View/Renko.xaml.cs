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
    /// Interaction logic for Renko.xaml
    /// </summary>
    public partial class Renko : UserControl
    {
        DataService dataService = DataService.GetService();

        public Renko()
        {
            InitializeComponent();
            this.Loaded += Renko_Loaded;
            Tag = AppResources.RenkoTag;
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

        void Renko_Loaded(object sender, RoutedEventArgs e)
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
    }
}
