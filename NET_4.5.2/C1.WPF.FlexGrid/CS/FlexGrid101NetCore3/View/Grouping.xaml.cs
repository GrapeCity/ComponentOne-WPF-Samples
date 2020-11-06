using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for Groupingxaml.xaml
    /// </summary>
    public partial class Groupingxaml : Page
    {
        ObservableCollection<Customer> _collectionView;
        public Groupingxaml()
        {
            InitializeComponent();

            UpdateVideos();
        }

        private void UpdateVideos()
        {
            var data = Customer.GetCustomerList(100);
            _collectionView = new ObservableCollection<Customer>(data);
            var listCollectionView = new ListCollectionView(_collectionView);
            listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Country"));
            grid.ItemsSource = listCollectionView;
            grid.MinColumnWidth = 85;
        }
    }
}
