using C1.Chart;
using C1.WPF.Chart;
using C1.WPF.Chart.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for Markers.xaml
    /// </summary>
    public partial class Markers : UserControl
    {
        private DataService dataService = DataService.GetService();

        public Markers()
        {
            InitializeComponent();
        }

        public List<Quote> Data
        {
            get
            {
                return dataService.GetSymbolData("box").Take(20).ToList();
            }
        }

        public List<string> Alignments
        {
            get
            {
                return Enum.GetNames(typeof(LineMarkerAlignment)).ToList();
            }
        }

        public List<string> Interactions
        {
            get
            {
                return Enum.GetNames(typeof(LineMarkerInteraction)).ToList();
            }
        }

        public List<string> MarkerLines
        {
            get
            {
                return Enum.GetNames(typeof(LineMarkerLines)).ToList();
            }
        }

        private TextBlock CreateContent(Quote item)
        {
            var textblock = new TextBlock()
            {
                Text = "Date: " + item.Date + "\u000A"
                    + "Open: " + item.Open + "\u000A"
                    + "High: " + item.High + "\u000A"
                    + "Low: " + item.Low + "\u000A"
                    + "Close: " + item.Close,
                Background = new SolidColorBrush(Colors.LightGray)
            };

            return textblock;
        }

        private void positionChanged(object sender, PositionChangedArgs e)
        {
            if (!cbSnpping.IsChecked.Value)
            {
                var pt = e.Position;
                var hti = financialChart.HitTest(pt);
                if (hti != null && hti.Item != null)
                {
                    var item = hti.Item as Quote;
                    var textblock = CreateContent(item);
                    marker.Content = textblock;
                }
            }
        }

        private void cbSnpping_Checked(object sender, RoutedEventArgs e)
        {
            if (marker != null)
            {
                marker.Interaction = LineMarkerInteraction.None;
                marker.Lines = LineMarkerLines.Both;
                marker.Alignment = LineMarkerAlignment.Auto;
                financialChart.MouseMove += FinancialChart_MouseMove;
            }
        }

        private void cbSnpping_Unchecked(object sender, RoutedEventArgs e)
        {
            if (marker != null)
            {
                marker.Interaction = LineMarkerInteraction.Move;
                marker.Lines = LineMarkerLines.Both;
                marker.Alignment = LineMarkerAlignment.Auto;
                marker.VerticalPosition = double.NaN;
                marker.HorizontalPosition = double.NaN;
                financialChart.MouseMove -= FinancialChart_MouseMove;
            }
        }

        private void FinancialChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (marker != null)
            {
                var pt = e.GetPosition(financialChart);
                var dp = financialChart.PointToData(pt);
                var idx = Math.Min(Data.Count - 1, Math.Max(0, (int)Math.Round(dp.X)));
                var item = Data[idx];
                var ax = financialChart.AxisX as IAxis;
                var ay = financialChart.AxisY as IAxis;
                var xmin = ax.GetMin();
                var xmax = ax.GetMax();
                var ymin = ay.GetMin();
                var ymax = ay.GetMax();
                var x = Math.Min(1, Math.Max(0, (idx - xmin) / (xmax - xmin)));
                var y = Math.Min(1, Math.Max(0, (ymax - dp.Y) / (ymax - ymin)));
                marker.Content = CreateContent(item);
                marker.HorizontalPosition = x;
                marker.VerticalPosition = y;
            }
        }
    }
}
