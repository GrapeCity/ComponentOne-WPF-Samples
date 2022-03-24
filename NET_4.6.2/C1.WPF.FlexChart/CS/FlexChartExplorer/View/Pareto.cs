using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using System.Windows.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using C1.WPF.Chart;
using System.Windows;
using System.Windows.Media;

namespace FlexChartExplorer
{
    public sealed partial class Pareto : UserControl
    {
        C1.WPF.Chart.RangedHistogram histogramSeries;
        Series paretoLine;
        List<object> data;

        public Pareto()
        {
            this.InitializeComponent();
            Init();
        }

        private void Init()
        {
            flexChart1.BeginUpdate();
            flexChart1.Series.Clear();
            
            flexChart1.ChartType = ChartType.RangedHistogram;
            flexChart1.Header = "Factors Influencing Purchase Decisions";
            flexChart1.HeaderStyle = new ChartStyle() { FontSize = 16 };
            flexChart1.AxisY.Title = "No. of Customers";
            flexChart1.AxisY.AxisLine = true;
            flexChart1.AxisY.MajorTickMarks = TickMark.Outside;
            flexChart1.AxisX.Title = "Factor";
            flexChart1.DataLabel.Content = "{}{y:N4}";
            flexChart1.DataLabel.Position = LabelPosition.Top;
            flexChart1.LabelRendering += FlexChart1_LabelRendering;
            flexChart1.MouseMove += FlexChart1_MouseMove1;
            

            histogramSeries = new C1.WPF.Chart.RangedHistogram()
            {
                ItemsSource = data,
                Binding = "Value",
                BindingX = "Name",
                SeriesName = "Count",
                SortDescending = true,
            };
            flexChart1.Series.Add(histogramSeries);

            //Pareto Line
            var histogramYs = histogramSeries.GetValues(0);
            var sum = histogramYs.Sum();
            double accumulatedValue = 0;
            var lineData = new List<Point>();
            for (int i = 0; i < histogramYs.Count(); i++)
            {
                accumulatedValue += histogramYs[i];
                lineData.Add(new Point
                {
                    X = i,
                    Y = accumulatedValue / sum
                });
            }

            paretoLine = new Series()
            {
                SeriesName = "Cumulative %",
                ItemsSource = lineData,
                Binding = "Y",
                BindingX = "X",
                ChartType = ChartType.LineSymbols,
                AxisY = new Axis()
                {
                    Position = Position.Right,
                    Max = 1,
                    Min = 0,
                    Format = "P0",
                    Title = "Cumulative Percentage"
                }
            };
            paretoLine.Style = new ChartStyle();
            paretoLine.SymbolStyle = new ChartStyle();
            paretoLine.Style.StrokeThickness = paretoLine.SymbolStyle.StrokeThickness = 3;
            paretoLine.Style.Stroke = paretoLine.SymbolStyle.Stroke = new SolidColorBrush(Color.FromArgb(255, 192, 80, 77));
            paretoLine.SymbolStyle.Fill = new SolidColorBrush(Color.FromArgb(255, 192, 80, 77));
            flexChart1.Series.Add(paretoLine);
            flexChart1.ToolTip.Content = "{}{y:P}";
            flexChart1.MouseMove += FlexChart1_MouseMove;
            flexChart1.EndUpdate();
        }

        private void FlexChart1_MouseMove1(object sender, MouseEventArgs e)
        {
            var ht = flexChart1.HitTest(e.GetPosition(flexChart1));
            if (ht == null || ht.Series == null)
                return;
            flexChart1.ToolTipContent = ht.Series.Name == "Cumulative %" ? "{y:P2}" : "{y}";
        }

        private void FlexChart1_LabelRendering(object sender, RenderDataLabelEventArgs e)
        {
            var value = Convert.ToDouble(e.Text);
            if (value <= 1)
                e.Text = value.ToString("P2");
            else
                e.Text = value.ToString("N0");
        }

        private void FlexChart1_MouseMove(object sender, MouseEventArgs e)
        {
            var ht = flexChart1.HitTest(e.GetPosition(flexChart1));
            if (ht == null || ht.Series == null)
                return;
            //flexChart1.ToolTip.Content = ht.Series.Name == "Cumulative %" ? ht.Item.ToString("P2") : "{}{y}";
        }

        public List<object> Data
        {
            get
            {
                if (data == null)
                {
                    data = new List<object>();
                    data.AddRange(new[] {  new { Name="Brand",Value=144},new { Name="Quality",Value=136},new { Name="New Product",Value=54},
                                   new { Name="Service",Value=58},new { Name="Price",Value=100},new { Name="Easy Returns",Value=29},
                                   new { Name="Quality",Value=139},new { Name="Brand",Value=123},new { Name="Easy Returns",Value=27},
                                   new { Name="Price",Value=133},new { Name="Easy Returns",Value=24},new { Name="Quality",Value=114},
                                   new { Name="Reviews",Value=73},new { Name="Quality",Value=119},new { Name="Service",Value=62},
                                   new { Name="New Product",Value=67},new { Name="Brand",Value=116},new { Name="Reviews",Value=79},
                                   new { Name="Price",Value=128},new { Name="Price",Value=125},new { Name="New Product",Value=69},
                                   new { Name="New Product",Value=65},new { Name="Price",Value=140},new { Name="Easy Returns",Value=28},
                                   new { Name="Brand",Value=146},new { Name="Service",Value=54},new { Name="Price",Value=100},
                                   new { Name="Price",Value=145},new { Name="Service",Value=69},new { Name="Quality",Value=118},
                                   new { Name="Easy Returns",Value=22},new { Name="New Product",Value=59},new { Name="Quality",Value=103},
                                   new { Name="Service",Value=66},new { Name="Brand",Value=123},new { Name="Brand",Value=117},
                                   new { Name="Service",Value=59},new { Name="Easy Returns",Value=29},new { Name="Service",Value=58},
                                   new { Name="Quality",Value=133},new { Name="Reviews",Value=73},new { Name="Price",Value=132},
                                   new { Name="Price",Value=125},new { Name="New Product",Value=57},new { Name="Quality",Value=128},
                                   new { Name="Quality",Value=107},new { Name="Brand",Value=137},new { Name="Reviews",Value=74},
                                   new { Name="Quality",Value=117},new { Name="Reviews",Value=70},}
                    );
                }
                return data;
            }
        }
    }
}
