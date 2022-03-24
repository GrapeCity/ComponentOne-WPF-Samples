using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SparklineSamples
{
    /// <summary>
    /// Interaction logic for FlexGridIntegration.xaml
    /// </summary>
    public partial class FlexGridIntegration : UserControl
    {
        public ObservableCollection<RegionSalesData> Sales { get; private set; }

        public FlexGridIntegration()
        {
            Random rnd = new Random();
            string[] states = new string[] { "Alabama", "Alaska", "Arizona", "Idaho", "Illinois", "Indiana", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Vermont", "Virginia", "Washington" };
            this.Sales = new ObservableCollection<RegionSalesData>();
            for (int i = 0; i < states.Length; i++)
            {
                RegionSalesData rsd = new RegionSalesData();
                rsd.State = states[i];
                rsd.Data = new ObservableCollection<double>();
                for (int j = 0; j < 12; j++)
                {
                    double d = rnd.Next(-50, 50);
                    rsd.Data.Add(d);
                    rsd.NetSales += d;
                    rsd.TotalSales += Math.Abs(d);
                }
                this.Sales.Add(rsd);
            }

            this.DataContext = this;

            InitializeComponent();

        }
    }
}
