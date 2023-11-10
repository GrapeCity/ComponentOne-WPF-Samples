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
using StockAnalysis.Object;

namespace StockAnalysis.Partial.UserControls
{
    /// <summary>
    /// Interaction logic for Charts.xaml
    /// </summary>
    public partial class Charts : UserControl
    {
        public Charts()
        {
            InitializeComponent();

            EventHandler<C1.WPF.Chart.RenderEventArgs> handlerInitRange = null;
            handlerInitRange = (o, e) =>
            {
                var model = ViewModel.ViewModel.Instance;
                if ( !model.IsLoaded && model.CurrectQuote != null && model.CurrectQuote.Data != null)
                {
                    model.UpperValue = model.CurrectQuote.Data.Count() - 1;
                    model.LowerValue = model.UpperValue - 60;

                    model.IsLoaded = true;
                    this.rangeChart.Rendered -= handlerInitRange;
                }
            };
            this.rangeChart.Rendered += handlerInitRange;

            ViewModel.ViewModel.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "ChartType")
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (ViewModel.ViewModel.Instance.ChartType == FinancialChartType.Kagi ||
                            ViewModel.ViewModel.Instance.ChartType == FinancialChartType.Renko ||
                            ViewModel.ViewModel.Instance.ChartType == FinancialChartType.PointAndFigure)
                        {
                            this.krChart.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            this.krChart.Visibility = Visibility.Collapsed;
                        }
                    }));
                }
            };
        }




        
        public ViewModel.ViewModel Model
        {
            get { return ViewModel.ViewModel.Instance; }
        }





    }

}
