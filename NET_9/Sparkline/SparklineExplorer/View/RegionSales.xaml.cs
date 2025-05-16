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
    public partial class RegionSales : UserControl
    {
        public ObservableCollection<RegionSalesData> Sales { get; private set; }

        public RegionSales()
        {
            InitializeComponent();
            this.Tag = Properties.Resources.RegionSalesDescription;

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
        }
    }

    public class RegionSalesData
    {
        public ObservableCollection<double> Data { get; set; }
        public string State { get; set; }
        public double TotalSales { get; set; }
        public string TotalSalesFormatted
        {
            get
            {
                return String.Format("{0:c2}", this.TotalSales);
            }
        }
        public double NetSales { get; set; }
    }
}
