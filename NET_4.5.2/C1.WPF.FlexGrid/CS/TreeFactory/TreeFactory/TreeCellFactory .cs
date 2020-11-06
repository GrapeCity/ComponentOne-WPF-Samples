using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.IO;

namespace TreeFactory
{
    public class TreeCellFactory : CellFactory
    {
        static Thickness _tEmpty = new Thickness(0);
        static BitmapImage _bmpCollapsed, _bmpExpanded, _bmpNoChildren;
        static Brush _connBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xc0, 0xc0, 0xc0));
        const double _connThickness = 1.5;

        public TreeCellFactory()
        {
            // load bitmaps (once)
            if (_bmpCollapsed == null)
            {
                _bmpCollapsed = LoadBitmap("collapsed.png");
                _bmpExpanded = LoadBitmap("expanded.png");
                _bmpNoChildren = null;// LoadBitmap("nochildren.png");
            }
        }

        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            base.CreateCellContent(grid, bdr, rng);

            var row = grid.Rows[rng.Row] as GroupRow;
            if (rng.Column == 0 && row != null)
            {
                bdr.Padding = _tEmpty;
                var cellGrid = bdr.Child as Grid;
                cellGrid.Children.RemoveAt(0);
                var treeNode = CreateTreeNode(grid, row);
                cellGrid.Children.Add(treeNode);
            }
        }

        // creates the element that represents a tree node
        FrameworkElement CreateTreeNode(C1FlexGrid flex, GroupRow row)
        {
            var g = new Grid();

            for (int i = 0; i <= row.Level; i++)
            {
                var cd = new ColumnDefinition();
                cd.Width = new GridLength(flex.TreeIndent);
                g.ColumnDefinitions.Add(cd);
            }

            // icon connectors (left and up)
            if (row.Level > 0)
            {
                var rc = CreateConnector(flex.TreeIndent, _connThickness);
                rc.HorizontalAlignment = HorizontalAlignment.Left;
                rc.VerticalAlignment = VerticalAlignment.Center;
                rc.Margin = new Thickness(flex.TreeIndent / 2, 0, 0, 0);
                rc.SetValue(Grid.ColumnProperty, row.Level - 1);
                rc.SetValue(Grid.ColumnSpanProperty, 2);
                g.Children.Add(rc);

                rc = CreateConnector(_connThickness, row.ActualHeight / 2);
                rc.HorizontalAlignment = HorizontalAlignment.Center;
                rc.VerticalAlignment = VerticalAlignment.Top;
                rc.SetValue(Grid.ColumnProperty, row.Level - 1);
                g.Children.Add(rc);
            }

            // icon connector (down)
            if (row.HasChildren && !row.IsCollapsed)
            {
                var rc = CreateConnector(_connThickness, row.ActualHeight / 2);
                rc.HorizontalAlignment = HorizontalAlignment.Center;
                rc.VerticalAlignment = VerticalAlignment.Bottom;
                rc.SetValue(Grid.ColumnProperty, row.Level);
                g.Children.Add(rc);
            }

            // inter-node connectors
            for (int i = row.Level; i > 0; i--)
            {
                if (CheckNodeAfter(row, i))
                {
                    var rc = CreateConnector(_connThickness, row.ActualHeight);
                    rc.HorizontalAlignment = HorizontalAlignment.Center;
                    rc.VerticalAlignment = VerticalAlignment.Top;
                    rc.SetValue(Grid.ColumnProperty, i - 1);
                    g.Children.Add(rc);
                }
            }

            // icon
            var nodeIcon = new Image();
            nodeIcon.Source =
                row.HasChildren == false ? _bmpNoChildren :
                row.IsCollapsed ? _bmpCollapsed : _bmpExpanded;
            nodeIcon.Stretch = Stretch.None;
            nodeIcon.SetValue(Grid.ColumnProperty, row.Level + 1);
            g.Children.Add(nodeIcon);
            if (row.HasChildren)
            {
                nodeIcon.MouseLeftButtonDown += (s, e) => { row.IsCollapsed = !row.IsCollapsed; };
            }

            // done
            g.Margin = new Thickness(0, 0, 4, 0);
            return g;
        }

        // creates a Rectangle element to use as a tree connector
        static Rectangle CreateConnector(double width, double height)
        {
            var rc = new Rectangle();
            rc.Width = width;
            rc.Height = height;
            rc.Fill = _connBrush;
            return rc;
        }

        // checks whether there is a row after this one with a given level 
        static bool CheckNodeAfter(GroupRow row, int level)
        {
            var rows = row.Grid.Rows;
            for (int i = row.Index + 1; i < rows.Count; i++)
            {
                var r = rows[i] as GroupRow;
                if (r.Level < level)
                {
                    return false;
                }
                if (r.Level == level)
                {
                    return true;
                }
            }
            return false;
        }

        // loads a bitmap from an embedded resource
        static BitmapImage LoadBitmap(string bmpName)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = GetResourceStream(bmpName);
            bmp.EndInit();
            return bmp;
        }

        // gets an embedded resource stream
        static Stream GetResourceStream(string resName)
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (var rn in asm.GetManifestResourceNames())
            {
                if (rn.EndsWith(resName))
                {
                    return asm.GetManifestResourceStream(rn);
                }
            }
            throw new Exception("Embedded resource not found: " + resName);
        }
    }
}
