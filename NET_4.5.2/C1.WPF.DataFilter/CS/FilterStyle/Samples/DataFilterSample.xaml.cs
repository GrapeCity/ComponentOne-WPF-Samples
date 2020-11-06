using C1.DataCollection;
using C1.WPF.DataFilter;
using System.Linq;
using System.Windows.Controls;

namespace FilterStyle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DataFilterSample : UserControl
    {
        private DataProvider _dataProvider = new DataProvider();

        public DataFilterSample()
        {
            InitializeComponent();
            var data = new C1DataCollection<Employee>(_dataProvider.GetEmployees());
            c1DataFilter1.ItemsSource = data;
            flexGrid.ItemsSource = data;

            foreach (ChecklistFilter filter in c1DataFilter1.Filters.Where(f => f is ChecklistFilter))
                filter.SelectAll();
        }
    }
}
