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
    /// Interaction logic for LineBreak.xaml
    /// </summary>
    public partial class LineBreak : UserControl
    {
        DataService dataService = DataService.GetService();

        public LineBreak()
        {
            InitializeComponent();
            Loaded += LineBreak_Loaded;
            Tag = "A Line Break or Three Line Break chart uses vertical boxes or lines to illustrate the price changes of an asset or market. Movements are depicted with box colors and styles; movements that continue the trend of the previous box are colored similarly while movements that trend oppositely are indicated with a different color and/or style. The opposite trend is only drawn if its value exceeds the extreme value of the previous n number of boxes or lines, which is determined by the newLineBreaks option.";
        }

        public List<Company> Companies
        {
            get
            {
                return dataService.GetCompanies();
            }
        }

        void LineBreak_Loaded(object sender, RoutedEventArgs e)
        {
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
    }
}
