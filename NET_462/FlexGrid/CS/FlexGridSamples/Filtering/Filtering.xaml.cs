using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for Filtering.xaml
    /// </summary>
    public partial class Filtering : UserControl
    {
        const int ROW_COUNT = 500;

        public Filtering()
        {
            InitializeComponent();
            BindGrid();
        }

        void BindGrid()
        {
            // create PagedCollectionView used as a data source for the
            // FlexGrid and for the MS DataGrid
            var data = new ObservableCollection<Customer>();
            for (int i = 0; i < ROW_COUNT; i++)
            {
                data.Add(new Customer(i));
            }
            var view = new MyCollectionView(data);
            using (view.DeferRefresh())
            {
                view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(new PropertyGroupDescription("Country"));
                view.GroupDescriptions.Add(new PropertyGroupDescription("Active"));
                var gd = view.GroupDescriptions[0] as PropertyGroupDescription;
                gd.Converter = new CountryInitialConverter();
            }

            // bind grids to ListCollectionView
            _flexFilter.ItemsSource = view;
        }
    }
}
