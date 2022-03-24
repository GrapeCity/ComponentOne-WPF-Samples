using System;
using System.Windows;
using System.Windows.Data;
using C1.WPF;
using C1.WPF.DataGrid;

namespace C1DataGrid_ComboCols2010
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1.WPF.DataGrid.DataGridComboBoxColumn _colCountry = new C1.WPF.DataGrid.DataGridComboBoxColumn();
        C1.WPF.DataGrid.DataGridComboBoxColumn _colRegion = new C1.WPF.DataGrid.DataGridComboBoxColumn();

        public MainWindow()
        {
            InitializeComponent();

            // handle auto generation to set the combo columns
            grid.AutoGeneratingColumn += grid_AutoGeneratingColumn;

            // update combo collections while the values are edited
            grid.BeganEdit += new EventHandler<DataGridBeganEditEventArgs>(grid_BeganEdit);

            // load grid
            grid.ItemsSource = Data.GenerateData();
        }

        void grid_BeganEdit(object sender, DataGridBeganEditEventArgs e)
        {
            if (e.Column == _colRegion)
            {
                var combo = e.EditingElement as C1ComboBox;
                var userInfo = e.Row.DataItem as UserInfo;
                var country = Country.GetCountryById(userInfo.CountryId);

                // restrict combo collection
                combo.ItemsSource = country.Regions;
            }
        }


        private void grid_AutoGeneratingColumn(object sender, C1.WPF.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Property.Name == "CountryId")
            {
                _colCountry.DisplayMemberPath = "Name";
                _colCountry.SelectedValuePath = "Id";
                _colCountry.SortMemberPath = "CountryId";
                _colCountry.FilterMemberPath = "CountryId";
                _colCountry.Header = "Country";
                _colCountry.Binding = new Binding() { Path = new PropertyPath("CountryId"), Mode = BindingMode.TwoWay };
                _colCountry.ItemsSource = Country.AllCountries;
                e.Column = _colCountry;
            }
            else if (e.Property.Name == "RegionId")
            {
                _colRegion.DisplayMemberPath = "Name";
                _colRegion.SelectedValuePath = "Id";
                _colRegion.SortMemberPath = "RegionId";
                _colRegion.FilterMemberPath = "RegionId";
                _colRegion.Header = "Region";
                _colRegion.Binding = new Binding() { Path = new PropertyPath("RegionId"), Mode = BindingMode.TwoWay };
                _colRegion.ItemsSource = Region.AllRegions;
                e.Column = _colRegion;
            }
        }
    }
}
