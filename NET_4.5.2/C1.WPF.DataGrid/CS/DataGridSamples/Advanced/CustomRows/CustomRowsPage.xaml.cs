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
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for CustomRows.xaml
    /// </summary>
    public partial class CustomRows : UserControl
    {
        public CustomRows()
        {
            InitializeComponent();

            grid.ItemsSource = Data.GetProducts((product) => product.Element("Image") != null && product.Element("Image").Value != "no_image_available_small.jpg");
        }

        private void grid_AutoGeneratingColumn(object sender, C1.WPF.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            Common.HandleColumnAutoGeneration(e);
        }

        private void C1DataGrid_CreatingRow(object sender, DataGridCreatingRowEventArgs e)
        {
            if (e.Type == DataGridRowType.Item)
            {
                e.Row = new DataGridTemplateRow() { RowTemplate = (DataTemplate)Resources["TemplateRow"] };
            }
        }

        private void grid_BeginningNewRow(object sender, DataGridBeginningNewRowEventArgs e)
        {
            Product product = (e.Item as Product);
            product.ExpirationDate = DateTime.Now.AddDays(7);
            product.Available = true;
            product.ImageUrl = "no_image_available_small.jpg";
        }
    }
}
