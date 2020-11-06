using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for Financial.xaml
    /// </summary>
    public partial class Financial : Page
    {
        FinancialDataList _financialData;

        public Financial()
        {
            InitializeComponent();
            PopulateFinancialGrid();
        }

        void PopulateFinancialGrid()
        {
            _financialData = FinancialData.GetFinancialData();
            var view = new MyCollectionView(_financialData);
            _flexFinancial.ItemsSource = view;
            _flexFinancial.Columns.Frozen = 1;
            _flexFinancial.Columns[0].AllowDragging = false;

            // configure search box
            _srchCompanies.View = view;
            var props = _srchCompanies.FilterProperties;
            props.Add(typeof(FinancialData).GetProperty("Name"));
            props.Add(typeof(FinancialData).GetProperty("Symbol"));

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
        void _cmbUpdateInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_financialData != null)
            {
                var cmb = sender as ComboBox;
                var val = ((ComboBoxItem)cmb.SelectedItem).Content as string;
                val = val.Split(' ')[0].Trim();
                _financialData.UpdateInterval = int.Parse(val);
            }
        }
        void _cmbBatchSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_financialData != null)
            {
                var cmb = sender as ComboBox;
                var val = ((ComboBoxItem)cmb.SelectedItem).Content as string;
                val = val.Split(' ')[0].Trim();
                _financialData.BatchSize = int.Parse(val);
            }
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
    }
}
