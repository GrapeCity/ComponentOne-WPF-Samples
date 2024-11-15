using C1.WPF.Grid;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

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
            if (cellContent is RatingCell ratingCell)
            {
                var row = Grid.Rows[range.Row];
                if (row.DataItem is Song)
                    ratingCell.PropertyChanged -= RatingCellOnPropertyChanged;
            }
            base.UnbindCellContent(cellType, range, cellContent);
        }
    }
}
