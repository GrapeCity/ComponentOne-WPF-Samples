using C1.WPF.Grid;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
#if SILVERLIGHT
using C1.Silverlight.FlexGrid;
#else
#endif

namespace FlexGridExplorer
{
    // cell factory used to create iTunes cells
    public class MusicCellFactory : GridCellFactory
    {
        public override object GetCellContentType(GridCellType cellType, GridCellRange range)
        {
            if (cellType == GridCellType.Cell)
            {
                var row = Grid.Rows[range.Row];
                var col = Grid.Columns[range.Column];

                if (row is GridGroupRow groupRow && range.Column == 0)
                {
                    return groupRow.Level == 0 ? typeof(ArtistCell) : typeof(AlbumCell);
                }

                var colName = col.ColumnName;
                if (colName == "Name")
                {
                    return typeof(SongCell);
                }
                if (colName == "Rating")
                {
                    return typeof(RatingCell);
                }
            }
            return base.GetCellContentType(cellType, range);
        }

        public override FrameworkElement CreateCellContent(GridCellType cellType, GridCellRange range, object cellContentType)
        {
            if (cellContentType as Type == typeof(ArtistCell))
                return new ArtistCell();
            else if (cellContentType as Type == typeof(AlbumCell))
                return new AlbumCell();
            else if (cellContentType as Type == typeof(SongCell))
                return new SongCell();
            else if (cellContentType as Type == typeof(RatingCell))
                return new RatingCell();
            return base.CreateCellContent(cellType, range, cellContentType);
        }

        public override void BindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            if (cellContent is ImageCell imageCell)
            {
                var cellText = Grid.GetCellText(range);
                imageCell.TextBlock.Text = cellText;
                if (cellContent is NodeCell nodeCell)
                {
                    var groupRow = Grid.Rows[range.Row] as GridGroupRow;
                    nodeCell.Tag = groupRow;
                    nodeCell.IsCollapsed = groupRow?.IsCollapsed ?? false;
                    nodeCell.IsCollapsedChanged += OnIsCollapsedChanged;
                }
            }
            else if (cellContent is RatingCell ratingCell)
            {
                var col = Grid.Columns[range.Column];
                var row = Grid.Rows[range.Row];
                var cellValue = Grid.GetCellValue(range);
                ratingCell.Rating = Convert.ToInt32(cellValue);
                ratingCell.Range = range;
                if (row.DataItem is Song)
                    ratingCell.PropertyChanged += RatingCellOnPropertyChanged;
            }
            else
            {
                base.BindCellContent(cellType, range, cellContent);
            }
        }

        private void RatingCellOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Rating")
            {
                var ratingCell = sender as RatingCell;

                if (ratingCell != null)
                {
                    Grid.SetCellValue(ratingCell.Range, ratingCell.Rating);
                    var task = Grid.StartEditingAsync(ratingCell.Range.Row, ratingCell.Range.Column);
                    var res = task.Status == TaskStatus.RanToCompletion && task.Result;
                    if (!res)
                        return;
                }

                Grid.FinishEditing();
            }
        }

        public override void UnbindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            if (cellContent is NodeCell nodeCell)
            {
                nodeCell.IsCollapsedChanged -= OnIsCollapsedChanged;
            }
            else 
            {
                if (cellContent is RatingCell ratingCell)
                {
                    var row = Grid.Rows[range.Row];
                    if (row.DataItem is Song)
                        ratingCell.PropertyChanged -= RatingCellOnPropertyChanged;
                }
                base.UnbindCellContent(cellType, range, cellContent);
            }
        }
        private void OnIsCollapsedChanged(object sender, EventArgs e)
        {
            var nodeCell = sender as NodeCell;
            var groupRow = nodeCell.Tag as GridGroupRow;
            groupRow.IsCollapsed = nodeCell.IsCollapsed;
        }

        //static Thickness _emptyThickness = new Thickness(0);
        //public List<RatingCell> _ratings = new List<RatingCell>();

        //// bind a cell to a range
        //public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange range)
        //{
        //    // get row/col
        //    var row = grid.Rows[range.Row];
        //    var col = grid.Columns[range.Column];
        //    var gr = row as GroupRow;

        //    // no border for group rows
        //    if (gr != null)
        //    {
        //        bdr.BorderThickness = _emptyThickness;
        //    }

        //    // bind the tree cells
        //    if (gr != null && range.Column == 0)
        //    {
        //        BindGroupRowCell(grid, bdr, range);
        //        return;
        //    }

        //    // bind cells in regular data rows
        //    var colName = col.ColumnName;
        //    if (colName == "Name")
        //    {
        //        bdr.Child = new SongCell(row);
        //        return;
        //    }
        //    if (colName == "Rating")
        //    {
        //        var song = row.DataItem as Song;
        //        if (song != null)
        //        {
        //            // create rating control to represent this cell
        //            // notes:
        //            // - the data context is provided by the Border element, and
        //            // - the binding is provided by the column
        //            var cell = new RatingCell();
        //            cell.SetBinding(RatingCell.RatingProperty, col.Binding);
        //            bdr.Child = cell;
        //            return;
        //        }
        //    }

        //    // default binding
        //    base.CreateCellContent(grid, bdr, range);
        //}

        //// bind a cell to a group row
        //void BindGroupRowCell(C1FlexGrid grid, Border bdr, CellRange range)
        //{
        //    // get row, group row
        //    var row = grid.Rows[range.Row];
        //    var gr = row as GroupRow;

        //    // show group caption/image on first column
        //    if (range.Column == 0)
        //    {
        //        // build a custom data item if necessary
        //        if (gr.DataItem == null)
        //        {
        //            gr.DataItem = BuildGroupDataItem(gr);
        //        }

        //        // get type of cell we need
        //        Type cellType = gr.Level == 0 ? typeof(ArtistCell) : typeof(AlbumCell);

        //        // create the cell
        //        bdr.Child = gr.Level == 0 
        //            ? (ImageCell)new ArtistCell(row) 
        //            : (ImageCell)new AlbumCell(row);
        //    }
        //}

        //// build a song to represent a group
        //// the GetChildDataItems method returns all songs that belong
        //// to this node, and the LINQ statement below calculates the total 
        //// size, length, and average rating for the album/artist.
        //Song BuildGroupDataItem(GroupRow gr)
        //{
        //    var gs = gr.GetDataItems().OfType<Song>();
        //    return new Song()
        //        {
        //            Name = gr.Group.Name.ToString(),
        //            Size = (long)gs.Sum(s => s.Size),
        //            Duration = (long)gs.Sum(s => s.Duration),
        //            Rating = (int)(gs.Average(s => s.Rating) + 0.5)
        //        };
        //}
    }

}
