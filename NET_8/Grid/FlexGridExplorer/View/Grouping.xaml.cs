using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for Groupingxaml.xaml
    /// </summary>
    public partial class Grouping : UserControl
    {
        ObservableCollection<Customer> _collectionView;
        public Grouping()
        {
            InitializeComponent();
            Tag = AppResources.GroupingDescription;

            groupRowPosition.ItemsSource = new GridGroupRowPosition[] { GridGroupRowPosition.AboveData, GridGroupRowPosition.BelowData, GridGroupRowPosition.None };
            groupSummariesPosition.ItemsSource = new GridGroupSummariesPosition[] { GridGroupSummariesPosition.Group, GridGroupSummariesPosition.Bottom, GridGroupSummariesPosition.None };

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
