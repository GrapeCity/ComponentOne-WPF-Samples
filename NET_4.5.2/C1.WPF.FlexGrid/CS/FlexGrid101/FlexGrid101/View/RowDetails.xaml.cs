using FlexGrid101.Resources;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RowDetails.xaml
    /// </summary>
    public partial class RowDetails : Page
    {
        public RowDetails()
        {
            InitializeComponent();

            Title = AppResources.RowDetailsTitle;
            lblMode.Content = AppResources.DetailsVisibiltyMode;

            showItemsPicker.Items.Add(DataGridRowDetailsVisibilityMode.Collapsed.ToString());
            showItemsPicker.Items.Add(DataGridRowDetailsVisibilityMode.Visible.ToString());
            showItemsPicker.Items.Add(DataGridRowDetailsVisibilityMode.VisibleWhenSelected.ToString());
            showItemsPicker.SelectionChanged += (s, e) =>
            {
                switch (showItemsPicker.SelectedIndex)
                {
                    case 0:
                        grid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
                        grid.Invalidate();
                        break;
                    case 1:
                        grid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;
                        grid.Invalidate();
                        break;
                    case 2:
                        grid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                        grid.Invalidate();
                        break;
                }
            };
            showItemsPicker.SelectedIndex = 1;

            var data = Customer.GetCustomerList(1000);
            var view = new ListCollectionView(data);
            grid.ItemsSource = view;
            grid.MinColumnWidth = 85;
        }
    }
}
