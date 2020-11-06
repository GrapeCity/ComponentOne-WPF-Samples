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
using C1.WPF.C1Chart;

namespace ChartSamples
{
    /// <summary>
    /// Interaction logic for LabelMarker.xaml
    /// </summary>
    [System.ComponentModel.Description("This sample shows how to create a simple marker using the ChartPanelObject class. Hover over the chart to see the marker.")]
    public partial class SimpleMarker : UserControl
    {
        public SimpleMarker()
        {
            InitializeComponent();

            cmbChartTypes.Items.Add(ChartType.Area);
            cmbChartTypes.Items.Add(ChartType.Column);
            cmbChartTypes.Items.Add(ChartType.LineSymbols);
            cmbChartTypes.SelectedIndex = 0;
 
        }

        private void ChartPanelObject_DataPointChanged(object sender, EventArgs e)
        {
            // update marker in code if you choose

            //var obj = (ChartPanelObject)sender;
            //if (obj != null)
            //{
            //    TextBlock lbl = FindName("label") as TextBlock;
            //    if (lbl != null)
            //    {
            //        lbl.Text = obj.DataPoint.Y.ToString("c2");
            //    }
            //}
        }

        private void cmbChartTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            c1Chart.ChartType = (ChartType)cmbChartTypes.SelectedItem;

            if (c1Chart.ChartType == ChartType.Area)
            {
                symbols.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                symbols.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
