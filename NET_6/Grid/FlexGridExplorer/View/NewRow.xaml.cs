using C1.DataCollection;
using C1.WPF.Grid;
using CommunityToolkit.Mvvm.ComponentModel;
using FlexGridExplorer.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for NewRow.xaml
    /// </summary>
    public partial class NewRow : UserControl
    {
        public NewRow()
        {
            InitializeComponent();
            Tag = AppResources.NewRowDescription;
            grid.ItemsSource = new CustomDataCollection<Customer>(Customer.GetCustomerList(100));
            grid.MinColumnWidth = 85;
        }

        private void OnRowEditEnding(object sender, GridCellEditEventArgs e)
        {
            var row = grid.Rows[e.CellRange.Row];
            if(!(row.DataItem as Customer).Validate())
                e.Cancel = true;
        }
    }
}
