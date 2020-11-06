using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Text;
using C1.WPF.FlexGrid;

namespace ExcelGrid
{
    /// <summary>
    /// Custom cell factory that displays Excel-style row and column headers.
    /// </summary>
    public class ExcelCellFactory : CellFactory
    {
        public override void CreateTopLeftContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            var poly = CreatePolygon(0, 10, 10, 10, 10, 0);
            poly.Fill = new SolidColorBrush(Color.FromArgb(0xe0, 0xf5, 0xf5, 0xf5));
            poly.VerticalAlignment = VerticalAlignment.Center;
            poly.HorizontalAlignment = HorizontalAlignment.Right;
            poly.Margin = new Thickness(2);
            bdr.Child = poly;
            bdr.Tag = grid;
            bdr.MouseLeftButtonDown += new MouseButtonEventHandler(bdr_MouseLeftButtonDown);
        }
        void bdr_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // select the whole grid when user clicks the top left cell
            var g = ((FrameworkElement)sender).Tag as C1FlexGrid;
            g.SelectAll();
        }
        public override void CreateRowHeaderContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            var tb = new TextBlock();
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Text = string.Format("{0}", range.Row + 1);
            bdr.Child = tb;
        }
        public override void CreateColumnHeaderContent(C1FlexGrid grid, Border bdr, CellRange range)
        {
            var tb = new TextBlock();
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Text = GetAlphaColumnHeader(range.Column);
            bdr.Child = tb;
        }

        // create Excel style column headers ("A", "B", ... "AB", etc)
        string GetAlphaColumnHeader(int i)
        {
            return GetAlphaColumnHeader(i, string.Empty);
        }
        string GetAlphaColumnHeader(int i, string s)
        {
            var rem = i % 26;
            s = (char)((int)'A' + rem) + s;
            i = i / 26 - 1;
            return i < 0 ? s : GetAlphaColumnHeader(i, s);
        }

        // create a polygon based on a list of points
        static Polygon CreatePolygon(params double[] values)
        {
            var p = new Polygon();
            p.Points = new PointCollection();
            for (int i = 0; i < values.Length - 1; i += 2)
            {
                p.Points.Add(new Point(values[i], values[i + 1]));
            }
            return p;
        }
    }
}
