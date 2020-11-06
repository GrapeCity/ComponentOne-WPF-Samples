using C1.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for AxisGrouping.xaml
    /// </summary>
    public partial class AxisGrouping : UserControl
    {
        List<string> _groupSeparator;

        public AxisGrouping()
        {
            InitializeComponent();
            //SetToHierarchicalData();
            Tag = "This view shows grouping of axis labels.";
        }

        public List<string> GroupSeparator
        {
            get
            {
                if (_groupSeparator == null)
                {
                    _groupSeparator = Enum.GetNames(typeof(AxisGroupSeparator)).ToList();
                }

                return _groupSeparator;
            }
        }

        public List<CountryGDP> Data
        {
            get { return CountryGDP.Countries(); }
        }

        /// <summary>
        /// We can also group the axis based on the hierarchical data source.
        /// Please have a try by calling this method.
        /// </summary>
        public void SetToHierarchicalData()
        {
            flexChart.ItemsSource = HierarchicalGDP.Data();
            flexChart.AxisX.GroupNames = "Continent";
            flexChart.AxisX.GroupItemsPath = "Items";
        }
    }

    public class CountryGDP
    {
        public string Country { get; set; }
        public string Continent { get; set; }

        public double CurrentPrices { get; set; }
        public double PPPValuation { get; set; }

        public CountryGDP(string country, string continent, double currentPrices, double pppValuation)
        {
            Country = country;
            Continent = continent;
            CurrentPrices = currentPrices;
            PPPValuation = pppValuation;
        }

        public static List<CountryGDP> Countries()
        {
            List<CountryGDP> countries = new List<CountryGDP>();
            countries.Add(new CountryGDP("US", "America", 19362, 19362));
            countries.Add(new CountryGDP("China", "Asia", 11938, 23122));
            countries.Add(new CountryGDP("Japan", "Asia", 4884, 5405));
            countries.Add(new CountryGDP("Germany", "Europe", 3652, 4150));
            countries.Add(new CountryGDP("France", "Europe", 2575, 2826));
            countries.Add(new CountryGDP("UK", "Europe", 2565, 2880));
            countries.Add(new CountryGDP("India", "Asia", 2439, 9447));
            countries.Add(new CountryGDP("Brazil", "America", 2081, 3219));
            countries.Add(new CountryGDP("Italy", "Europe", 1921, 2307));
            countries.Add(new CountryGDP("Canada", "America", 1640, 1764));
            return countries;
        }
    }

    public class HierarchicalGDP
    {
        public string Country { get; set; }
        public string Continent { get; set; }

        public double CurrentPrices { get; set; }
        public double PPPValuation { get; set; }

        public HierarchicalGDP[] Items { get; set; }

        public static HierarchicalGDP[] Data()
        {
            return new HierarchicalGDP[] {
                new HierarchicalGDP { Continent = "Asia",
                    Items = new HierarchicalGDP[] {
                        new HierarchicalGDP{Country= "China", CurrentPrices=11938, PPPValuation=23122},
                        new HierarchicalGDP{Country= "Japan", CurrentPrices=4884, PPPValuation=5405},
                        new HierarchicalGDP{Country= "India", CurrentPrices=2439, PPPValuation=9447}
                    }
                },
                new HierarchicalGDP { Continent = "America",
                    Items = new HierarchicalGDP[] {
                        new HierarchicalGDP {Country= "US", CurrentPrices=19362, PPPValuation=19362},
                        new HierarchicalGDP{Country= "Brazil", CurrentPrices=2081, PPPValuation=3219},
                        new HierarchicalGDP{Country= "Canada", CurrentPrices=1640, PPPValuation=1764}
                    }
                },
                new HierarchicalGDP { Continent = "Europe",
                    Items = new HierarchicalGDP[] {
                        new HierarchicalGDP{Country= "Germany", CurrentPrices=3652, PPPValuation=4150},
                        new HierarchicalGDP{Country= "France", CurrentPrices=2575, PPPValuation=2826},
                        new HierarchicalGDP{Country= "UK", CurrentPrices=2565, PPPValuation=2880},
                        new HierarchicalGDP{Country= "Italy", CurrentPrices=1921, PPPValuation=2307}
                    }
                }
            };
        }
    }
}
