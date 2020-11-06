using C1.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for FibonacciTool.xaml
    /// </summary>
    public partial class FibonacciTool : UserControl
    {
        DataService dataService = DataService.GetService();

        public FibonacciTool()
        {
            InitializeComponent();
            Tag = "Fibonacci tool is used for trend analysis in financial charts. With the help of range selector, you can choose data range for calculation.";
        }

        public List<Quote> Data
        {
            get
            {
                return dataService.GetSymbolData("box");
            }
        }

        public List<string> FibonacciTypes
        {
            get
            {
                return "Arcs,Fans,Retracements,Time Zones".Split(',').ToList();
            }
        }

        public List<string> Uptrends
        {
            get
            {
                return new List<string>() { bool.TrueString, bool.FalseString};
            }
        }

        public List<string> Positions
        {
            get
            {
                return "Left,Center,Right".Split(',').ToList();
            }
        }

        public List<string> VerticalPositions
        {
            get
            {
                return "None,Top,Center,Bottom".Split(',').ToList();
            }
        }

        public List<string> TimeZonesPositions
        {
            get
            {
                return "None,Left,Center,Right".Split(',').ToList();
            }
        }

        private void C1RangeSelector_ValueChanged(object sender, EventArgs e)
        {
            if (fibonacci != null)
            {
                financialChart.BeginUpdate();
                fibonacci.MinX = rangeSelector.LowerValue;
                fibonacci.MaxX = rangeSelector.UpperValue;
                financialChart.EndUpdate();
            }
        }
    }
}
