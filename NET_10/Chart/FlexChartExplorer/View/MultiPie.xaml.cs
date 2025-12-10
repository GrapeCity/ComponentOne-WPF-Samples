using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MultiPie : UserControl
    {
        List<SeasonSale> _data;
        string[] bindings = new string[] {
                "WinterOnline,WinterOffline,SummerOnline,SummerOffline",
                "OfflineTotal,OnlineTotal",
                "WinterTotal,SummerTotal",
                "TotalSales"
            };

        public MultiPie()
        {
            this.InitializeComponent();
        }

        public string[] ShowOptions
        {
            get
            {
                return new string[] { "Season & Channel", "Offline vs Online", "Winter vs Summer", "Total" }; 
            }
        }

        public List<SeasonSale> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = SeasonSale.GetSeasonSales(5);
                }

                return _data;
            }
        }

        string[] AdjustTitles(string[] titles)
        {
            if (titles.Length > 1)
            {
                for (var i = 0; i < titles.Length; i++)
                {
                    titles[i] = titles[i].Replace("Total", "");
                    // add space
                    titles[i] = string.Concat(titles[i].Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
                }
            }
            else
                titles = null;

            return titles;
        }

        private void cbShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbShow.SelectedItem != null)
            {
                pieChart.Binding = bindings[cbShow.SelectedIndex];
                // adjust titles
                pieChart.Titles = AdjustTitles(pieChart.Binding.Split(','));
            }
        }
    }

    public class SeasonSale
    {
        public string Name { get; set; }
        public double WinterOnline { get; set; }
        public double WinterOffline { get; set; }
        public double SummerOnline { get; set; }
        public double SummerOffline { get; set; }
        public double TotalSales
        {
            get
            {
                return WinterOnline + WinterOffline + SummerOffline + SummerOnline;
            }
        }
        public double WinterTotal { get { return WinterOnline + WinterOffline; } }
        public double SummerTotal { get { return SummerOnline + SummerOffline; } }
        public double OnlineTotal { get { return WinterOnline + SummerOnline; } }
        public double OfflineTotal { get { return WinterOffline + SummerOffline; } }


        static string[] salesItems = "Computers|Software|Cell Phones|Video Games|Musical Instruments|Household Electronic Items|Sports & Fitness|Jewellery & Accessories|Furniture|Clothing".Split('|');

        public static List<SeasonSale> GetSeasonSales(int numOfCategories)
        {
            var rnd = new Random();
            var data = new List<SeasonSale>();
            for (int i = 0; i < numOfCategories; i++)
            {
                var item = salesItems[i];
                data.Add(new SeasonSale
                {
                    Name = item,
                    WinterOnline = rnd.Next(10, 100),
                    SummerOnline = rnd.Next(10, 100),
                    WinterOffline = rnd.Next(10, 100),
                    SummerOffline = rnd.Next(10, 100),
                });
            }
            return data;
        }
    }
}
