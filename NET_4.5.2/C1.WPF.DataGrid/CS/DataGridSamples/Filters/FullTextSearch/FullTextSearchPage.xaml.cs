using System.Linq;
using System.Windows.Controls;
using C1.WPF.DataGrid;
using C1.WPF.DataGrid.Filters;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for FullTextSearchPage.xaml
    /// </summary>
    public partial class FullTextSearch : UserControl
    {
        public FullTextSearch()
        {
            InitializeComponent();

            grid.ItemsSource = Data.GetProducts().ToList<Product>();
        }

        private void grid_AutoGeneratingColumn(object sender, C1.WPF.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            Common.HandleColumnAutoGeneration(e);
            if (e.Property.Name == "ImageUrl")
            {
                e.Cancel = true;
            }
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            C1FullTextSearchBehavior.GetFullTextSearchBehavior(grid).Filter = filter.Text;
        }
    }
}
