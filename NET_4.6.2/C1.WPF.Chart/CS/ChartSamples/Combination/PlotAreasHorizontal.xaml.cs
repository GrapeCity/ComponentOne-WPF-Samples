using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;


namespace ChartSamples
{
    [System.ComponentModel.Description("This sample shows several plot areas stacked horizontally.")]
    public partial class PlotAreasHorizontal : UserControl
    {
        public PlotAreasHorizontal()
        {
            InitializeComponent();
            NewData();

            chart.MouseLeftButtonUp += (s, e) => NewData();
        }

        void NewData()
        {
            chart.BeginUpdate();
            chart.Reset(true);

            chart.ChartType = ChartType.Line;

            var ax = chart.View.AxisX;
            ax.Title = "X0";
            ax.Position = AxisPosition.Far | AxisPosition.DisableLastLabelOverflow;
            ax.Min = 0;
            ax.Max = 1;

            for (int i = 0; i < 6; i++)
            {

                if (i > 0)
                {
                    var axisname = "X" + i;
                    chart.View.Axes.Add(new Axis()
                    {
                        AxisType = AxisType.X,
                        Position = AxisPosition.Far | AxisPosition.DisableLastLabelOverflow,
                        Name = axisname,
                        PlotAreaIndex = i,
                        Title = axisname,
                        MajorGridStroke = chart.View.AxisX.MajorGridStroke,
                        Min = 0,
                        Max = 1
                    });
                    var ds = SampleData.CreateDataSeries(100, true);
                    ds.AxisX = axisname;
                    chart.Data.Children.Add(ds);
                }
                else
                {
                    var ds = SampleData.CreateDataSeries(100, true);
                    chart.Data.Children.Add(ds);
                }

                chart.View.PlotAreas.Add(
                  new PlotArea() { Column = i, Stroke = new SolidColorBrush(Colors.DarkGray) });
            }

            var ay = chart.View.AxisY;
            ay.Reversed = true;
            ay.Title = "Depth, meters";

            chart.EndUpdate();
        }
    }
}
