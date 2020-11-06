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

namespace C1.WPF.FlexGridBook
{
    public class GridSplitter : Control
    {
        Grid _parentGrid;
        int _gridColumn;
        double _widthLeft, _widthRight;
        Point _ptDown;

        public GridSplitter()
        {
            this.DefaultStyleKey = typeof(GridSplitter);
            this.MouseLeftButtonDown += GridSplitter_MouseLeftButtonDown;
            this.MouseMove += GridSplitter_MouseMove;
            this.MouseLeftButtonUp += GridSplitter_MouseLeftButtonUp;
            this.LostMouseCapture += GridSplitter_LostMouseCapture;
        }

        void GridSplitter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _gridColumn = -1;
            _parentGrid = this.Parent as Grid;
            if (_parentGrid != null && _parentGrid.ColumnDefinitions.Count > 2)
            {
                var c = (int)GetValue(Grid.ColumnProperty);
                if (c > 0 && c < _parentGrid.ColumnDefinitions.Count - 1)
                {
                    if (CaptureMouse())
                    {
                        _widthLeft = _parentGrid.ColumnDefinitions[c - 1].ActualWidth;
                        _widthRight = _parentGrid.ColumnDefinitions[c + 1].ActualWidth;
                        _ptDown = e.GetPosition(_parentGrid);
                        _gridColumn = c;
                    }
                }
            }
        }
        void GridSplitter_MouseMove(object sender, MouseEventArgs e)
        {
            if (_parentGrid != null && _gridColumn > 0)
            {
                var pt = e.GetPosition(_parentGrid);
                ApplyDelta(pt.X - _ptDown.X);
            }
        }
        void GridSplitter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
        }
        void GridSplitter_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _gridColumn = -1;
            _parentGrid = null;
        }
        void ApplyDelta(double dx)
        {
            var cdLeft = _parentGrid.ColumnDefinitions[_gridColumn - 1];
            var cdRight = _parentGrid.ColumnDefinitions[_gridColumn + 1];

            if (cdRight.Width.IsStar)
            {
                cdLeft.Width = new GridLength(_widthLeft + dx, GridUnitType.Star);
                cdRight.Width = new GridLength(_widthRight - dx, GridUnitType.Star);
            }
            else
            {
                cdLeft.Width = new GridLength(_widthLeft + dx);
                cdRight.Width = new GridLength(_widthRight - dx);
            }
        }
    }
}
