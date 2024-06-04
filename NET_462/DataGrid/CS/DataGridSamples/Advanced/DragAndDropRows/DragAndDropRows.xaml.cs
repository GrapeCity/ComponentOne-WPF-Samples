﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media.Animation;
using System;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public partial class DragAndDropRows : System.Windows.Controls.UserControl
    {
        private static ObservableCollection<Player> _players = new ObservableCollection<Player>();

        static DragAndDropRows()
        {
            // initialize data
            foreach (var player in Data.GetPlayers(null))
            {
                _players.Add(player);
            }
        }

        public DragAndDropRows()
        {
            InitializeComponent();

            DataContext = _players;
        }

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Property.Name == "Photo")
            {
                var imageCol = new DataGridImageColumn(e.Property);
                imageCol.CanUserMove = false;
                imageCol.Header = "";
                imageCol.Binding.Converter = new ImageSourceConverter();
                imageCol.Binding.ConverterParameter = "/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/tennis/";
                e.Column = imageCol;
            }
            if (e.Property.Name == "Birthday" && e.Column is DataGridBoundColumn)
            {
                ((DataGridBoundColumn)e.Column).Format = "d";
            }
            if (e.Property.Name == "Website")
            {
                var hyperCol = new DataGridHyperlinkColumn(e.Property);
                //hyperCol.CellContentStyle = Resources["HyperlinkColumnStyle"] as Style;
                e.Column = hyperCol;
            }
        }

        private void grid_AutoGeneratedColumns(object sender, System.EventArgs e)
        {
            grid.Columns["Photo"].DisplayIndex = 0;
        }

        private Point _draggingStartPoint;
        private bool _draggingSelection = false;
        private int _rowsNumberOffset = 0;

        private void grid_SelectionDragStarted(object sender, DataGridSelectionDragStartedEventArgs e)
        {
            _draggingStartPoint = e.GetPosition(null);
            var cell = grid.GetCellFromPoint(_draggingStartPoint);
            if (cell != null && grid.Selection.SelectedCells.Contains(cell))
            {
                _draggingSelection = true;
                e.Cancel = true;
            }
        }

        private void grid_SelectionDragDelta(object sender, DataGridSelectionDragEventArgs e)
        {
            if (_draggingSelection)
            {
                var currentPoint = e.GetPosition(null);
                double offset = currentPoint.Y - _draggingStartPoint.Y;
                double rowsOffset = 0;
                int from, to, increment = 1;
                if (offset > 0)
                {
                    from = grid.Selection.SelectedRows.Max(r => r.Index) + 1;
                    to = grid.Rows.Count - 1;
                }
                else
                {
                    from = grid.Selection.SelectedRows.Min(r => r.Index) - 1;
                    to = 0;
                    increment = -1;
                }

                _rowsNumberOffset = 0;
                for (int i = from; increment > 0 ? i <= to : i >= to; i += increment)
                {
                    var rowHeight = grid.Rows[i].ActualHeight * increment;
                    if (increment > 0 ? rowHeight < offset : rowHeight > offset)
                    {
                        rowsOffset += rowHeight;
                        offset -= rowHeight;
                        _rowsNumberOffset += increment;
                        continue;
                    }
                    break;
                }

                double selectionHeight = 0;
                foreach (var selectedRow in grid.Selection.SelectedRows)
                {
                    selectionHeight += selectedRow.ActualHeight;
                }

                foreach (var row in grid.Viewport.Rows)
                {
                    double y = 0;
                    if (grid.Selection.SelectedRows.Contains(row))
                    {
                        y = rowsOffset;
                        System.Windows.Controls.Canvas.SetZIndex(row.Presenter, 1);
                        System.Windows.Controls.Canvas.SetZIndex(row.HeaderPresenter, 1);
                    }
                    else
                    {
                        if (increment > 0 ?
                            row.Index >= from && row.Index < from + _rowsNumberOffset :
                            row.Index <= from && row.Index > from + _rowsNumberOffset)
                        {
                            y = -selectionHeight * increment;
                        }
                        System.Windows.Controls.Canvas.SetZIndex(row.Presenter, 0);
                        System.Windows.Controls.Canvas.SetZIndex(row.HeaderPresenter, 0);
                    }

                    var storyboard = row.Presenter.Tag as Storyboard;
                    var animation = storyboard.Children[0] as DoubleAnimation;
                    if (animation.To != y)
                    {
                        animation.To = y;
                        storyboard.Begin();
                    }

                    var storyboard2 = row.HeaderPresenter.Tag as Storyboard;
                    var animation2 = storyboard2.Children[0] as DoubleAnimation;
                    if (animation2.To != y)
                    {
                        animation2.To = y;
                        storyboard2.Begin();
                    }
                }
            }
        }

        private void grid_SelectionDragCompleted(object sender, DataGridSelectionDragEventArgs e)
        {
            if (_draggingSelection)
            {
                foreach (var row in grid.Viewport.Rows)
                {
                    var storyboard = row.Presenter.Tag as Storyboard;
                    storyboard.Stop();
                    (row.Presenter.RenderTransform as TranslateTransform).Y = 0;

                    var storyboard2 = row.HeaderPresenter.Tag as Storyboard;
                    storyboard2.Stop();
                    (row.HeaderPresenter.RenderTransform as TranslateTransform).Y = 0;
                }

                _draggingSelection = false;
                var selectedItems = grid.Selection.SelectedRows.Select(r => r.DataItem);
                /*             var selectionStart = grid.Selection.SelectedRows.Min(r => r.Index);
                             var selectionEnd = grid.Selection.SelectedRows.Max(r => r.Index);*/
                var selectionStart = _players.IndexOf((Player)grid.Rows[grid.Selection.SelectedRows.Min(r => r.Index)].DataItem); // get index of DataItem in underlying collection
                var selectionEnd = _players.IndexOf((Player)grid.Rows[grid.Selection.SelectedRows.Max(r => r.Index)].DataItem); // get index of DataItem in underlying collection
                for (int i = 0; i < Math.Abs(_rowsNumberOffset); i++)
                {
                    if (_rowsNumberOffset > 0)
                    {
                        if (selectionStart + _rowsNumberOffset > _players.Count - 1)
                        {
                            // drag after add new row in the end
                            return;
                        }
                        var player = _players[selectionEnd + _rowsNumberOffset];
                        _players.Remove(player);
                        _players.Insert(selectionStart, player);
                    }
                    else
                    {
                        if (selectionStart + _rowsNumberOffset < 0)
                        {
                            // drag before add new row in the beginning
                            return;
                        }
                        var player = _players[selectionStart + _rowsNumberOffset];
                        _players.Remove(player);
                        _players.Insert(selectionEnd, player);
                    }
                }

                grid.Selection.BeginUpdate();
                grid.Selection.Clear();
                foreach (var item in selectedItems)
                {
                    var row = grid.Rows[item];
                    grid.Selection.Add(row, row);
                }
                grid.Selection.EndUpdate();
            }
        }

        private void grid_LoadedRowPresenter(object sender, DataGridRowEventArgs e)
        {
            var storyBoard = new Storyboard();
            var animation = new DoubleAnimation() { Duration = TimeSpan.FromMilliseconds(200) };
            Storyboard.SetTarget(animation, e.Row.Presenter);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(animation);
            e.Row.Presenter.Tag = storyBoard;
            e.Row.Presenter.RenderTransform = new TranslateTransform();
        }

        private void grid_LoadedRowHeaderPresenter(object sender, DataGridRowEventArgs e)
        {
            var storyBoard = new Storyboard();
            var animation = new DoubleAnimation() { Duration = TimeSpan.FromMilliseconds(200) };
            Storyboard.SetTarget(animation, e.Row.HeaderPresenter);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(animation);
            e.Row.HeaderPresenter.Tag = storyBoard;
            e.Row.HeaderPresenter.RenderTransform = new TranslateTransform();
        }
    }
}