using FlexGridExplorer.Resources;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for Financial.xaml
    /// </summary>
    public partial class Financial : UserControl
    {
        FinancialDataList _financialData;

        public Financial()
        {
            InitializeComponent();
            Tag = AppResources.FinancialDescription;
            PopulateFinancialGrid();
            Loaded += Financial_Loaded;
        }

        private void Financial_Loaded(object sender, RoutedEventArgs e)
        {
            _cmbUpdateInterval.SelectedIndex = 0;
            _cmbBatchSize.SelectedIndex = 1;
        }

        void PopulateFinancialGrid()
        {
            _financialData = FinancialData.GetFinancialData();
            var view = new MyCollectionView(_financialData);
            _flexFinancial.ItemsSource = view;
            _flexFinancial.FrozenColumns = 1;
            _flexFinancial.Columns[0].AllowDragging = false;

            // show company info
            UpdateCompanyStatus();
            view.CollectionChanged += financial_CollectionChanged;

            UpdateCellFactory();
        }

        void financial_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCompanyStatus();
        }

        // update song status
        void UpdateCompanyStatus()
        {
            var view = _flexFinancial.ItemsSource as ICollectionView;
            var companies = view.OfType<FinancialData>();
            _txtCompanies.Text = string.Format("{0:n0} companies.",
                (from c in companies select c.Symbol).Distinct().Count());
        }

        // control update frequency
        void _chkAutoUpdate_Click(object sender, RoutedEventArgs e)
        {
            _financialData.AutoUpdate = ((CheckBox)sender).IsChecked.Value;
        }
        void _chkOwnerDrawFinancial_Click(object sender, RoutedEventArgs e)
        {
            UpdateCellFactory();
        }

        private void UpdateCellFactory()
        {
            _flexFinancial.CellFactory = _chkOwnerDrawFinancial.IsChecked.Value
                ? new FinancialCellFactory()
                : null;
        }

        private void OnClearFilter(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _srchCompanies.Text = "";
        }

        private void _cmbUpdateInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_financialData != null && e.AddedItems.Count > 0)
            {
                var val = e.AddedItems[0] as string;
                val = val.Split(' ')[0].Trim();
                _financialData.UpdateInterval = int.Parse(val);
            }
        }

        private void _cmbBatchSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_financialData != null && e.AddedItems.Count > 0)
            {
                var val = e.AddedItems[0] as string;
                val = val.Split(' ')[0].Trim();
                _financialData.BatchSize = int.Parse(val);
            }
        }
    }
}
