using C1.WPF.Sparkline;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SparklineExplorer
{
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
            this.Tag = Properties.Resources.GridDescription;
        }
    }
}
