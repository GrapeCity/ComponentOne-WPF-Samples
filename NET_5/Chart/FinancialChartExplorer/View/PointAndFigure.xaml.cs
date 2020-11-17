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

using C1.Chart.Finance;
using C1.WPF.Chart.Finance;
using FinancialChartExplorer.Resources;

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for Renko.xaml
    /// </summary>
    public partial class PointAndFigure : UserControl
    {
        DataService dataService = DataService.GetService();

        public PointAndFigure()
        {
            InitializeComponent();
            this.Loaded += Renko_Loaded;
            Tag = AppResources.PointFigureTag;
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

        public List<string> Scalings
        {
            get
            {
                return Enum.GetNames(typeof(PointAndFigureScaling)).ToList();
            }
        }

        public List<string> DataFields
        {
            get
            {
                return new List<string>() { "Close", "HighLow" };
            }
        }

        void Renko_Loaded(object sender, RoutedEventArgs e)
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (financialChart != null)
            {
                if(boxSize!=null)
                    boxSize.IsEnabled = ((PointAndFigureOptions)financialChart.Options).Scaling == PointAndFigureScaling.Fixed;
                if(period!=null)
                    period.IsEnabled = ((PointAndFigureOptions)financialChart.Options).Scaling == PointAndFigureScaling.Dynamic;
            }
        }
    }
}
