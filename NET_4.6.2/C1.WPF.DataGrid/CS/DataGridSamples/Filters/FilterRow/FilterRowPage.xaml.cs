using System.Windows.Controls;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for FilterRowPage.xaml
    /// </summary>
    public partial class FilterRow : UserControl
    {
        public FilterRow()
        {
            InitializeComponent();

            grid.ItemsSource = Data.GetProducts();
        }

        private void grid_AutoGeneratingColumn(object sender, C1.WPF.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            // invoke common auto-generation handling for all the samples
            Common.HandleColumnAutoGeneration(e);
            if (e.Property.Name == "ImageUrl")
            {
                e.Cancel = true;
            }
        }
    }
}
