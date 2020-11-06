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
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for DefaultFilterPage.xaml
    /// </summary>
    public partial class DefaultFilter : UserControl
    {
        public DefaultFilter()
        {
            InitializeComponent();

            grid.ItemsSource = Data.GetProducts();
        }

        private void grid_AutoGeneratingColumn(object sender, C1.WPF.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            // invoke common autogeneration handling for all the samples
            Common.HandleColumnAutoGeneration(e);
            if (e.Property.Name == "ImageUrl")
            {
                e.Cancel = true;
            }
        }
    }
}
