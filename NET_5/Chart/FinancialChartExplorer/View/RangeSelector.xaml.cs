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
    /// Interaction logic for RangeSelector.xaml
    /// </summary>
    public partial class RangeSelector : UserControl
    {
        DataService dataService = DataService.GetService();

        public RangeSelector()
        {
            InitializeComponent();
            Tag = "The RangeSelector allows end users to adjust the FinancialChart's visible range of data at runtime. The example below demonstrates the typical use case for the RangeSelector and how to apply a custom style.";
        }

        public List<Quote> Data
        {
            get
            {
                return dataService.GetSymbolData("fb");
            }
        }
    }
}
