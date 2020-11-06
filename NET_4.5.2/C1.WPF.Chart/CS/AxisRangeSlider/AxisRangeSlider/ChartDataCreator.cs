using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.C1Chart;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace AxisRangeSlider
{
    /// <summary>
    /// This class is meant to create the data to be shown in the charts
    /// </summary>
   internal  class ChartDataCreator
    {
       /// <summary>
       /// Populates C1WPFChart with created data
       /// </summary>
       /// <param name="chart">C1WPFChart</param>
       internal void DrawChart(C1Chart chart, bool inverted = false)
       {
            
           chart.ChartType = ChartType.Line;
           PopulateDataInChart(chart, inverted);
         
           
       }
      /// <summary>
      /// Populates the chart data and the axes
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="template">Empty template for chart</param>
      /// <param name="inverted"></param>
       internal void DrawAxisChart(C1Chart chart,DataTemplate template, bool inverted = false)
       {
           chart.ChartType = ChartType.Line;
           PopulateDataInChart(chart, inverted);
           chart.View.AxisX.AnnoTemplate = template;
           chart.View.AxisY.AnnoTemplate = template;
           chart.View.AxisX.MajorTickHeight = chart.View.AxisX.MinorTickHeight = 0;
           chart.View.AxisY.MajorTickHeight = chart.View.AxisY.MinorTickHeight = 0;

       }
       /// <summary>
       /// Populates the data for the chart
       /// </summary>
       /// <param name="chart"></param>
       /// <param name="inverted"></param>
       /// <returns></returns>
       C1Chart PopulateDataInChart(C1Chart chart,bool inverted)
       {
           double[] x = new double[1000];
           double[] y = new double[1000];
           for (int i = 0; i < 1000; i++)
           {
               x[i] = i; y[i] = 100 * Math.Sin(0.1 * i);
           }
           if (inverted)
               chart.Data.Children.Add(
                 new XYDataSeries()
                 {
                     ConnectionStroke = new SolidColorBrush(Colors.Red),
                     XValuesSource = y,
                     ValuesSource = x
                 });
           else
               chart.Data.Children.Add(
                 new XYDataSeries()
                 {
                     ConnectionStroke = new SolidColorBrush(Colors.Blue),
                     XValuesSource = x,
                     ValuesSource = y
                 });

           return chart;
       }
       

    }
}
