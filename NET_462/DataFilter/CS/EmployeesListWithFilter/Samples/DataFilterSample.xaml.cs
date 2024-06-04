using C1.DataCollection;
using C1.WPF;
using C1.WPF.DataFilter;
using C1.WPF.Theming;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EmployeesListWithFilter
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

            cmbTheme.ItemsSource = typeof(C1AvailableThemes).GetEnumValues<C1AvailableThemes>();
            cmbTheme.SelectedItem = C1AvailableThemes.Office2016White;
        }

        private void cmbTheme_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            var theme = C1ThemeFactory.GetTheme((C1AvailableThemes)cmbTheme.SelectedItem);
            C1Theme.ApplyTheme(LayoutRoot, theme);

            var adornerLayer = AdornerLayer.GetAdornerLayer(LayoutRoot);
            if (adornerLayer != null)
            {
                C1Theme.ApplyTheme(adornerLayer, theme);
            }
        }

    }
}
