using C1.WPF.Core;
using C1.WPF.Grid;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace FlexGridExplorer
{
    /// <summary>
    /// Cell factory used to create iTunes cells.
    /// </summary>
    public class MusicCellFactory : GridCellFactory
    {
        public MusicCellFactory()
        {
            AllowCustomCells = true;
        }

        public C1IconTemplate ArtistIcon { get; set; }
        public C1IconTemplate AlbumIcon { get; set; }
        public C1IconTemplate SongIcon { get; set; }

        public override object GetCellKind(GridCellType cellType, GridCellRange range)
        {
            var cellKind = base.GetCellKind(cellType, range);
            if (cellType == GridCellType.Cell)
            {
                var row = Grid.Rows[range.Row];
                var col = Grid.Columns[range.Column];

                if (row is GridGroupRow groupRow && range.Column == 0)
                {
                    return groupRow.Level == 0 ? (ArtistIcon, cellKind) : (AlbumIcon, cellKind);
                }

                var colName = col.ColumnName;
                if (colName == "Name")
                {
                    return (SongIcon, cellKind);
                }
            }
            return cellKind;
        }

        public override GridCellView CreateCell(GridCellType cellType, GridCellRange range, object cellKind)
        {
            if (cellKind is ValueTuple<C1IconTemplate, object> tuple)
            {
                var cell = base.CreateCell(cellType, range, tuple.Item2);
                cell.IconTemplate = tuple.Item1;
                return cell;
            }
            else
            {
                return base.CreateCell(cellType, range, cellKind);
            }
        }

        public override object GetCellContentType(GridCellType cellType, GridCellRange range)
        {
            if (cellType == GridCellType.Cell)
            {
                var col = Grid.Columns[range.Column];
                var colName = col.ColumnName;
                if (colName == "Rating")
                {
                    return typeof(RatingCell);
                }
            }
            return base.GetCellContentType(cellType, range);
        }

        public override FrameworkElement CreateCellContent(GridCellType cellType, GridCellRange range, object cellContentType)
        {
            if (cellContentType as Type == typeof(RatingCell))
                return new RatingCell();
            return base.CreateCellContent(cellType, range, cellContentType);
        }

        public override void BindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            if (cellContent is RatingCell ratingCell)
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

        public override void UnbindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            if (cellContent is RatingCell ratingCell)
            {
                var row = Grid.Rows[range.Row];
                if (row.DataItem is Song)
                    ratingCell.PropertyChanged -= RatingCellOnPropertyChanged;
            }
            base.UnbindCellContent(cellType, range, cellContent);
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
    }
}
