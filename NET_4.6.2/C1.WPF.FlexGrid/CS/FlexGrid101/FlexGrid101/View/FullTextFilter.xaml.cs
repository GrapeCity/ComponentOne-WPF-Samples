using System.Windows.Controls;

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for FullTextFilter.xaml
    /// </summary>
    public partial class FullTextFilter : Page
    {
        public FullTextFilter()
        {
            InitializeComponent();

            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.MinColumnWidth = 85;
        }
    }
}
