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
    /// Interaction logic for ConditionalFormat.xaml
    /// </summary>
    public partial class ConditionalFormat : UserControl
    {
        public ConditionalFormat()
        {
            InitializeComponent();

            // attach cell presenter loaded/unloaded events
            grid.LoadedCellPresenter += new EventHandler<DataGridCellEventArgs>(grid_LoadedCellPresenter);
            grid.UnloadedCellPresenter += new EventHandler<DataGridCellEventArgs>(grid_UnloadedCellPresenter);

            grid.ItemsSource = ProductDeliveryInfo.Generate(1000);
        }


        #region load/unload cells

        void grid_UnloadedCellPresenter(object sender, DataGridCellEventArgs e)
        {
            // set as defualt, for recycling
            e.Cell.Presenter.Background = null;
        }

        void grid_LoadedCellPresenter(object sender, DataGridCellEventArgs e)
        {
            if (e.Cell.Column.Name == "ExpectedDelivery")
            {
                ProductDeliveryInfo p = (ProductDeliveryInfo)e.Cell.Row.DataItem;
                DateTime realDelivery = p.ReadyForDelivery.AddDays(p.DeliveryDays);
                int daysDifference = p.ExpectedDelivery.Subtract(realDelivery).Days;
                if (daysDifference < -2)
                {
                    e.Cell.Presenter.Background = (Brush)Resources["ProblemBrush"];
                }
                else if (daysDifference < 0)
                {
                    e.Cell.Presenter.Background = (Brush)Resources["DelayBrush"];
                }
                else if (daysDifference < 1)
                {
                    e.Cell.Presenter.Background = (Brush)Resources["WarningBrush"];
                }
            }
        }

        #endregion
    }


    #region ** data

    public class ProductDeliveryInfo
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public DateTime ReadyForDelivery { get; set; }
        public int DeliveryDays { get; set; }
        public DateTime ExpectedDelivery { get; set; }

        public static List<ProductDeliveryInfo> Generate(int qty)
        {
            Random rnd = new Random();

            var list = new List<ProductDeliveryInfo>();
            for (int i = 0; i < qty; i++)
            {
                var baseDay = DateTime.Today.AddDays(rnd.Next(20));
                string id = "000" + i.ToString();
                list.Add(new ProductDeliveryInfo()
                {
                    ProductName = "Product " + id.Substring(id.Length - 3, 3),
                    Price = 100 + rnd.Next(900),
                    ReadyForDelivery = baseDay,
                    DeliveryDays = 1 + rnd.Next(5),
                    ExpectedDelivery = baseDay.AddDays(1 + rnd.Next(8))
                });
            }
            return list;
        }
    }

    #endregion
}
