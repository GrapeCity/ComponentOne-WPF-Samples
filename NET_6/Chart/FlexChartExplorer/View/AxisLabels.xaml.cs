using C1.Chart;
using C1.WPF;
using C1.WPF.Chart;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlexChartExplorer.Resources;

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for TrendLine.xaml
    /// </summary>
    public partial class AxisLabels : UserControl
    {
        Random rnd = new Random();
        List<string> _overlappingLabels;

        public AxisLabels()
        {
            InitializeComponent();
            Tag = AppResources.AxisLabelsTag;
        }

        public List<object> Data
        {
            get{ return GDPData.GetData(); }
        }

        public List<string> OverlappingLabels
        {
            get
            {
                if (_overlappingLabels == null)
                {
                    _overlappingLabels = Enum.GetNames(typeof(OverlappingLabels)).ToList();
                }

                return _overlappingLabels;
            }
        }
    }

    class GDPData
    {
        static string csv_data = @"United States,18624450
European Union,16408364
China,11232108
Japan,4936543
Germany,3479232
United Kingdom,2629188
France,2466472
India,2263792
Italy,1850735
Brazil,1798622
Canada,1529760
South Korea,1411042
Russia,1283162
Australia,1261645
Spain,1232597
Mexico,1046925
Indonesia,932448
Turkey,863390
Netherlands,777548
Switzerland,669038
Saudi Arabia,646438";

        static List<object> _data;

        static List<object> ParseData()
        {
            var list = new List<object>();
            var data = csv_data;
            var ss = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ss.Length; i++)
            {
                var vals = ss[i].Split(',');
                list.Add(new { Country = vals[0], GDP = double.Parse(vals[1]) });
            }

            return list;
        }

        public static List<object> GetData()
        {
            if (_data == null)
                _data = ParseData();

            return _data;
        }
    }
}
