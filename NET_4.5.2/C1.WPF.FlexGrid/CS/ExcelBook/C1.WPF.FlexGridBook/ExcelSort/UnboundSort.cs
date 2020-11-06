using System;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Utility for sorting unbound grids.
    /// </summary>
    public static class UnboundSort
    {
        /// <summary>
        /// Sorts the rows in an unbound grid.
        /// </summary>
        /// <param name="flex">Grid to be sorted.</param>
        /// <param name="sortDescriptions">List of <see cref="UnboundSortDescription"/> objects that specify the columns to sort on.</param>
        /// <param name="startRow">First row in the range to be sorted.</param>
        /// <param name="endRow">Last row in the range to be sorted.</param>
        public static void SortUnboundGrid(C1FlexGrid flex, IList<UnboundSortDescription> sortDescriptions, int startRow, int endRow)
        {
            // build row list
            var list = new List<Row>();
            for (int r = startRow; r <= endRow; r++)
            {
                list.Add(flex.Rows[r]);
            }

            // sort the list
            var comparer = new UnboundRowComparer(sortDescriptions);
            list.Sort(comparer);

            // copy sorted rows back to grid
            using (flex.Rows.DeferNotifications())
            {
                for (int r = startRow; r <= endRow; r++)
                {
                    flex.Rows[r] = list[r - startRow];
                }
            }
        }
        /// <summary>
        /// Sorts the selected rows in an unbound grid.
        /// </summary>
        /// <remarks>
        /// If only one row is selected, this method sorts the whole grid.
        /// </remarks>
        /// <param name="flex">Grid to be sorted.</param>
        /// <param name="sortDescriptions">List of <see cref="UnboundSortDescription"/> objects that specify the columns to sort on.</param>
        public static void SortUnboundGrid(C1FlexGrid flex, IList<UnboundSortDescription> sortDescriptions)
        {
            var rng = flex.Selection;
            if (rng.RowSpan <= 1)
            {
                rng.Row = 0;
                rng.Row2 = flex.Rows.Count - 1;
            }
            SortUnboundGrid(flex, sortDescriptions, rng.TopRow, rng.BottomRow);
        }
        /// <summary>
        /// Comparer used for sorting the unbound rows.
        /// </summary>
        class UnboundRowComparer : IComparer<Row>
        {
            IList<UnboundSortDescription> _sdc;
            public UnboundRowComparer(IList<UnboundSortDescription> sdc)
            {
                _sdc = sdc;
            }
            public int Compare(Row x, Row y)
            {
                var cmp = 0;
                foreach (var sd in _sdc)
                {
                    // get values to compare
                    var v1 = x.GetDataRaw(sd.Column) as IComparable;
                    var v2 = y.GetDataRaw(sd.Column) as IComparable;

                    // compare them
                    if (v1 != null && v2 != null)
                    {
                        try
                        {
                            cmp = v1.CompareTo(v2);
                        }
                        catch
                        {
                            // types are not compatible? use strings
                            cmp = string.Compare(v1.ToString(), v2.ToString(), StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    else if (v1 != null)
                    {
                        cmp = +1;
                    }
                    else if (v2 != null)
                    {
                        cmp = -1;
                    }

                    // account for direction
                    if (sd.Direction == ListSortDirection.Descending)
                    {
                        cmp = -cmp;
                    }

                    // break when different
                    if (cmp != 0)
                    {
                        break;
                    }
                }

                // done
                return cmp;
            }
        }
    }
    /// <summary>
    /// Specifies a column to be sorted and the sort direction.
    /// </summary>
    public class UnboundSortDescription : INotifyPropertyChanged
    {
        // ** fields
        Column _col;
        ListSortDirection _direction;

        // ** ctors

        /// <summary>
        /// Initializes a new instance of a <see cref="UnboundSortDescription"/>.
        /// </summary>
        public UnboundSortDescription()
        {
        }
        /// <summary>
        /// Initializes a new instance of a <see cref="UnboundSortDescription"/>.
        /// </summary>
        /// <param name="column">The column to sort the list by.</param>
        /// <param name="direction">The sort order.</param>
        public UnboundSortDescription(Column column, ListSortDirection direction)
        {
            _col = column;
            _direction = direction;
        }

        // ** object model

        /// <summary>
        /// Gets or sets the column to sort the list by.
        /// </summary>
        public Column Column
        {
            get { return _col; }
            set
            {
                _col = value;
                OnPropertyChanged("Column");
            }
        }
        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        public ListSortDirection Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                OnPropertyChanged("Direction");
            }
        }
        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propName">Name of the property whose value changed.</param>
        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        /// <summary>
        /// Occurs when the value of a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
