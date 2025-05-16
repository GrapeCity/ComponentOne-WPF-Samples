using C1.Chart;
using C1.WPF.Chart;
using FlexChartExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for Waterfall.xaml
    /// </summary>
    public partial class ColumnHeatmap : UserControl
    {
        public ColumnHeatmap()
        {
            InitializeComponent();
            Tag = AppResources.ColumnHeatmapTag;
        }

        private void ChartLoaded(object sender, RoutedEventArgs e)
        {
            if (heatmap.ItemsSource == null)
            {
                // init data
                heatmap.ItemsSource = HeatmapData;
                heatmap.StartX = Data[0].Date.ToOADate();

                chart.AxisX.Min = Data[0].Date.ToOADate() - 0.5;
                chart.AxisX.Max = Data[Data.Count - 1].Date.ToOADate() + 0.5;
            }
        }

        #region Data
        double[,] heatmapData = null;
        public List<TemperatureDiff> Data { get; set; } = DataCreator.GetTemperatureDifferenceData();

        public double[,] HeatmapData
        {
            get 
            {
                if (heatmapData == null)
                {
                    heatmapData = new double[Data.Count, 1];
                    for (int i = 0; i < Data.Count; i++)
                        heatmapData[i, 0] = Data[i].Diff >= 0 ? 0 : 1;
                }
                return heatmapData;
            }
        }
        #endregion

        private void SymbolRendering(object sender, RenderSymbolEventArgs e)
        {
            var clr = ((TemperatureDiff)e.Item).Diff >= 0 ? Resources["clr1"] : Resources["clr2"];
            e.Engine.SetFill(clr);
            e.Engine.SetStroke(clr);
        }
    }

    public class ColorList : List<Color>
    { 
    }
}
