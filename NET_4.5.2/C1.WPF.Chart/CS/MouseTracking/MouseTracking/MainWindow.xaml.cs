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

namespace Mouse_Tracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Vars
        Point ellipseDataPoint = new Point();
        Point test = new Point();
        #endregion
        public MainWindow()
        {
            InitializeComponent();

            #region DataCreation
            XYDataSeries Ds = new XYDataSeries();
            Ds.ChartType = ChartType.Line;

            List<int> xvalues = new List<int>();
            xvalues.Add(1);
            xvalues.Add(2);
            xvalues.Add(3);
            xvalues.Add(4);
            xvalues.Add(5);
            xvalues.Add(6);
            xvalues.Add(7);
            xvalues.Add(8);
            xvalues.Add(9);

            List<int> yvalues = new List<int>();
            yvalues.Add(200);
            yvalues.Add(130);
            yvalues.Add(303);
            yvalues.Add(442);
            yvalues.Add(190);
            yvalues.Add(239);
            yvalues.Add(400);
            yvalues.Add(350);
            yvalues.Add(9);

            Ds.XValuesSource = xvalues;
            Ds.ValuesSource = yvalues; 
            #endregion

            chart.Data.Children.Add(Ds);

            var pnl = new ChartPanel();
            pnl.MouseMove += new MouseEventHandler(pnl_MouseMove);

            var vmarker = CreateMarker(false, new Point());
            pnl.Children.Add(vmarker);
            vmarker.Action = ChartPanelAction.None;

            var hmarker = CreateMarker(true, new Point());
            pnl.Children.Add(hmarker);
            hmarker.Action = ChartPanelAction.None;
          
           chart.View.Layers.Add(pnl);
        }

        void pnl_MouseMove(object sender, MouseEventArgs e)
        {
            ChartPanel cnp = sender as ChartPanel;
            Point pnt = e.GetPosition(chart);
            test = pnt;
            
            var rect = chart.View.PlotRect;

            if (rect.Contains(pnt))
            {
                cnp.Children.Clear();

                var vmarker = CreateMarker(false, pnt);
                cnp.Children.Add(vmarker);

                var hmarker = CreateMarker(true, pnt);
                cnp.Children.Add(hmarker);
               
                var ellipse = CreateEllipse(pnt);
                cnp.Children.Add(ellipse);
                ellipse.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ellipse.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            }
           
        }

        
        ChartPanelObject CreateMarker(bool isHorizontal, Point pnt)
        {
            var obj = new ChartPanelObject();

            var bdr = new Border()
            {
                BorderBrush = Background = new SolidColorBrush(Colors.Red),
                Padding = new Thickness(2),
            };

            if (isHorizontal)
            {
                bdr.BorderThickness = new Thickness(0, 2, 0, 0);
                bdr.Margin = new Thickness(0, -1, 0, 0);
                obj.HorizontalContentAlignment = HorizontalAlignment.Stretch;

                if (pnt.X == 0 && pnt.Y == 0)
                    obj.DataPoint = new Point(double.NaN, 0.5);
                else
                {
                    //Data Point where Mouse cursor is present
                    Point dpt = chart.View.PointToData(pnt);

                    ///Markers are created based on the Horizontal movement
                    ///of mouse cursor. We have to track whether the mouse
                    ///cursor moved towards left or right from the last position.
                    ///For this we need to determine the two DataPoints plotted
                    ///between which the current mouse position is placed.


                    double val;
                    // Gets the index of the nearest data point
                    int ptIndex = chart.View.DataIndexFromPoint(pnt, 0, MeasureOption.X, out val);

                    // Use the above index to calculate the Coordinates of that nearest Data Point
                    Point closeScreenPoint = chart.View.DataIndexToPoint(0, ptIndex);

                    // Get the location of the next data point depending on whether mouse was
                    // was moved towards left or right
                    Point awayScreenPoint;
                    if (pnt.X > closeScreenPoint.X)
                        awayScreenPoint = chart.View.DataIndexToPoint(0, ptIndex + 1);
                    else
                        awayScreenPoint = chart.View.DataIndexToPoint(0, ptIndex - 1);

                    //Get coordinates where Vertical marker will intersect the chart line
                    double crossY = GetY(closeScreenPoint, awayScreenPoint, pnt);

                    //Get DataPoint for the above intersecting coordinates
                    Point closeDataPoint = chart.View.PointToData(new Point(pnt.X, crossY));

                    // set the DataPoint as the Attached data source for the Horizontal marker
                    obj.DataPoint = new Point(double.NaN, closeDataPoint.Y);

                    ellipseDataPoint.Y = closeDataPoint.Y;
                   // ellipseDataPoint.Y = obj.DataPoint.Y;
                }
            }
            else
            {
                bdr.BorderThickness = new Thickness(2, 0, 0, 0);
                bdr.Margin = new Thickness(-1, 0, 0, 0);
                obj.VerticalContentAlignment = VerticalAlignment.Stretch;

                Point dpt = chart.View.PointToData(pnt);
                obj.DataPoint = new Point(dpt.X, double.NaN);
                ellipseDataPoint.X = dpt.X;
            }

            bdr.IsHitTestVisible = false;
            obj.Content = bdr;

            return obj;
        }
        /// <summary>
        /// Draw Ellipse around the Intersection of 
        /// Horizontal and Vertical markers
        /// </summary>
        /// <param name="pnt"></param>        
        ChartPanelObject CreateEllipse(Point pnt)
        {
            Border sp = new Border();
            sp.Height = 50;
            sp.Width = 50;
            var obj = new ChartPanelObject();
            Ellipse el = new Ellipse();
            el.Height = 50;
            el.Width = 50;
            el.StrokeThickness = 2;
            el.Stroke = new SolidColorBrush(Colors.Red);

            sp.Child = el;
            obj.DataPoint = new Point(ellipseDataPoint.X, ellipseDataPoint.Y );
            obj.Content = sp;
            
            return obj;
        }

        /// <summary>
        /// Returns the Y value for the coordinate where Horizontal
        /// marker intersects the Chart line. 
        /// It uses Line theorem to calculate the point. It takes the
        /// coodinates for two datapoints as parameters. Line between these
        /// two datapoints covers the intersection line.
        /// </summary>
        /// <param name="p1">DataPoint nearest to the intersection coordinate</param>
        /// <param name="p2">DataPoint away from the intersection coordinate</param>
        /// <param name="pnt">Current DataPoint to get the X Value</param>
        /// <returns></returns>
        double GetY(Point p1, Point p2, Point pnt)
        {
            double m = (p1.Y - p2.Y) / (p1.X - p2.X);
            double c = (p1.Y) - (p1.X * m);

            double y = (m * pnt.X) + c;

            return y;
        }
    }
}
