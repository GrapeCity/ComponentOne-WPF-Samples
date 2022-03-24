using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for FullTextFilter.xaml
    /// </summary>
    public partial class FullTextFilter : UserControl
    {
        const int ROW_COUNT = 500;
        public FullTextFilter()
        {
            InitializeComponent();

            var data = new ObservableCollection<Customer>();
            for (int i = 0; i < ROW_COUNT; i++)
            {
                data.Add(new Customer(i));
            }
            var view = new MyCollectionView(data);

            // bind grids to ListCollectionView
            grid.ItemsSource = view;

            grid.MinColumnWidth = 85;
        }
    }
}
