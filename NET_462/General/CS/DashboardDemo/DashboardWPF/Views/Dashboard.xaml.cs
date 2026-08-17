using C1.WPF.FlexGrid;
using C1.WPF.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DashboardModel;
using System.Globalization;

namespace DashboardWPF
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void OnDateRangeChanged(object sender, DateRangeChangedEventArgs e)
        {
            DataService.GetService().DateRange = e.NewValue;
            TopSaleProducts.ItemsSource = DataService.GetService().GetTopSaleProductList(3);
            TopSaleCustomer.ItemsSource = DataService.GetService().GetTopSaleCustomerList(7);
            RegionSales.ItemsSource = DataService.GetService().RegionSalesVsGoal;
            CategortySales.ItemsSource = DataService.GetService().CategorySalesVsGoal;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CurrentProfitVSPriorProfit.ItemsSource = DataService.GetService().BudgetItemList;
            SalesAndProfits.ItemsSource = DataService.GetService().BudgetItemList;
        }
    }
}
