using C1.Chart.Finance;
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

namespace StockAnalysis.Partial.UserControls
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    public partial class Display : UserControl
    {
        public Display()
        {
            InitializeComponent();
        }

        public FinancialChartType ChartType
        {
            get { return (FinancialChartType)this.GetValue(ChartTypeProperty); }
            set { this.SetValue(ChartTypeProperty, value); }
        }
        public static DependencyProperty ChartTypeProperty = DependencyProperty.Register(
            "ChartType",
            typeof(FinancialChartType),
            typeof(Display),
            new PropertyMetadata(FinancialChartType.Candlestick)
        );

        private void SettableRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var btn = sender as CustomControls.SettableRadioButton;
            if(btn.Tag != null)
                this.ChartType = (FinancialChartType)btn.Tag;

            if (this.ChartType != FinancialChartType.Renko &&
                this.ChartType != FinancialChartType.Kagi &&
                this.ChartType != FinancialChartType.PointAndFigure)
            {
                Command.CloseDropdownCommand cmd = new Command.CloseDropdownCommand();
                cmd.Execute(this);
            }
        }
    }
}
