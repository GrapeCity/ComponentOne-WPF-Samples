using DashboardModel;
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

namespace DashboardWPF
{
    /// <summary>
    /// Interaction logic for RangeSelector.xaml
    /// </summary>
    public partial class RangeSelector : UserControl
    {
        List<SaleItem> items;

        public RangeSelector()
        {
            InitializeComponent();
        }

        public event DateRangeChangedEvent DateRangeChanged;

        void RasisedDateRangeChangedEvent(DateRange dateRange)
        {
            if (DateRangeChanged != null)
                DateRangeChanged(this, new DateRangeChangedEventArgs(dateRange));
        }

        public List<SaleItem> DateSaleItemSource { get { return items; } }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            items = DataService.GetService().DateSaleList;
            Chart.ItemsSource = items;
        }

        private void OnSelectorValueChanged(object sender, EventArgs e)
        {
            DateRange dateRange = new DateRange();
            dateRange.Start = items[(int)Selector.LowerValue].Date;
            dateRange.End = items[(int)Selector.UpperValue].Date;
            RasisedDateRangeChangedEvent(dateRange);
        }
    }
}
