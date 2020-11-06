using C1.Chart;
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
    /// Interaction logic for MovingAverages.xaml
    /// </summary>
    public partial class MovingAverages : UserControl
    {
        DataService dataSerivice = DataService.GetService();

        public MovingAverages()
        {
            InitializeComponent();
        }

        public List<Quote> Data
        {
            get
            {
                return dataSerivice.GetSymbolData("box");
            }
        }

        public List<string> MovingAverageType
        {
            get
            {
                return Enum.GetNames(typeof(MovingAverageType)).ToList();
            }
        }
    }
}
