using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Linq;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for SelectionModes.xaml
    /// </summary>
    public partial class SelectionModes : UserControl
    {
        public SelectionModes()
        {
            InitializeComponent();
            Tag = AppResources.SelectionModesDescription;
            foreach (var value in Enum.GetValues<GridSelectionMode>())
            {
                selectionMode.Items.Add(value.ToString());
            }
            selectionMode.SelectedItem = GridSelectionMode.MultiRange.ToString();
            lblShowMarquee.Text = AppResources.ShowMarquee;

            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.MinColumnWidth = 85;
        }

        private void grid_SelectionChanging(object sender, GridSelectionEventArgs e)
        {
            // e.Cancel = true;
        }

        private void grid_SelectionChanged(object sender, GridSelectionEventArgs e)
        {
            if (e.CellRange != null && e.CellRange.Row != -1)
            {
                var cells = string.Join(", ", e.Cells);
                lblSelection.Text = e.Cells.Count.ToString() + " " + AppResources.CellsSelectedText;
            }
            else
            {
                lblSelection.Text = "";
            }
        }

        private void selectionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectionMode.SelectedItem != null)
                grid.SelectionMode = (GridSelectionMode)Enum.Parse(typeof(GridSelectionMode), selectionMode.SelectedItem.ToString());
        }
    }
}
