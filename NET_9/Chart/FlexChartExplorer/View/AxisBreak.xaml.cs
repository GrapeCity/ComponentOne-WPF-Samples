using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using C1.Chart;
using C1.WPF.Chart;
using FlexChartExplorer.Resources;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace FlexChartExplorer
{
    public sealed partial class AxisBreak : UserControl
    {
        Point[] _data;
        ChartType[] _chartTypes;

        public AxisBreak()
        {
            this.InitializeComponent();
            Tag = AppResources.AxisBreakTag;

            cbAxisBreak.IsChecked = true;
        }

        public ChartType[] ChartTypes
        {
            get
            {
                if (_chartTypes == null)
                    _chartTypes = new[] { ChartType.Column, ChartType.Line, ChartType.LineSymbols, ChartType.Area };
                return _chartTypes;
            }
        }

        public Point[] Data
        {
            get
            {
                if (_data == null)
                    _data = CreateData();
                return _data;
            }
        }

        Point[] CreateData()
        {
            var rnd = new Random();
            var pts = new Point[20];

            for (var i = 0; i < pts.Length; i++)
            {
                if (rnd.NextDouble() < 0.85)
                    pts[i] = new Point(i, rnd.Next(0, 10));
                else
                    pts[i] = new Point(i, 50 + rnd.Next(0, 50));
            }

            return pts;
        }

        private void cbAxisBreak_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cbAxisBreak.IsChecked == true)
            {
                if (flexChart.Rotated)
                    View.AxisBreakHelper.CreateAxisBreak(flexChart.AxisX, 0, 10, 50, 100);
                else
                    View.AxisBreakHelper.CreateAxisBreak(flexChart.AxisY, 0, 10, 50, 100);
            }
            else
                View.AxisBreakHelper.Remove(flexChart);

        }

        private void cbRotated_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            flexChart.Rotated = cbRotated.IsChecked == true;
            flexChart.AxisX.MajorGrid = flexChart.Rotated;
            flexChart.AxisY.MajorGrid = !flexChart.Rotated;

            if (cbAxisBreak.IsChecked == true)
            {
                if (flexChart.Rotated)
                    View.AxisBreakHelper.CreateAxisBreak(flexChart.AxisX, 0, 10, 50, 100);
                else
                    View.AxisBreakHelper.CreateAxisBreak(flexChart.AxisY, 0, 10, 50, 100);
            }

        }
    }
}
