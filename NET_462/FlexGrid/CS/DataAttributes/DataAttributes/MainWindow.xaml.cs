using System;
using System.Collections.ObjectModel;
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

namespace DataAttributes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // create a list of customers
            // NOTE: the properties on the Customer object have data attributes.
            var list = new ObservableCollection<Customer>();
            for (int i = 0; i < 20; i++)
            {
                var c = new Customer();
                list.Add(c);
            }

            // show customers on both grids
            _grid.ItemsSource = list;
            _flex.ItemsSource = new ListCollectionView(list);

            _grid.RowEditEnding += _grid_RowEditEnding;
        }

        private void _grid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            _flex.Invalidate();
        }
    }
}
