using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;

namespace TraditionalIndexing
{
    class FlexGridTraditionalIndexing : C1FlexGrid
    {
        // ** fields
        RowCollectionTraditionalIndexing _rows;
        ColumnCollectionTraditionalIndexing _cols;

        // ** ctor
        public FlexGridTraditionalIndexing()
        {
            // create custom row and column collections to provide
            // a 'Fixed' property and the traditional FlexGrid indexing methods
            _rows = new RowCollectionTraditionalIndexing(this);
            _cols = new ColumnCollectionTraditionalIndexing(this);
        }
        new public RowCollectionTraditionalIndexing Rows
        {
            // return custom row collection
            get { return _rows; }
        }
        new public ColumnCollectionTraditionalIndexing Columns
        {
            // return custom column collection
            get { return _cols; }
        }
        new public object this[int row, int col]
        {
            // override direct data access
            get { return this.Rows[row][this.Columns[col]]; }
            set { this.Rows[row][this.Columns[col]] = value; }
        }

        // Row collection that uses the traditional FlexGrid indexing method:
        // The "Fixed" property gets or sets the number of fixed rows.
        // Row indices from 0 to "Fixed" correspond to column header rows,
        // Row indices from "Fixed" on correspond to scrollable rows 0, 1, etc...
        public class RowCollectionTraditionalIndexing
        {
            // ** fields
            FlexGridTraditionalIndexing _grid;

            // ** ctor
            public RowCollectionTraditionalIndexing(FlexGridTraditionalIndexing grid)
            {
                _grid = grid;
            }

            // ** object model
            public int Fixed
            {
                get { return _grid.ColumnHeaders.Rows.Count; }
                set
                {
                    while (Fixed < value)
                    {
                        _grid.ColumnHeaders.Rows.Add(new Row());
                    }
                    while (Fixed > value)
                    {
                        _grid.ColumnHeaders.Rows.RemoveAt(_grid.ColumnHeaders.Rows.Count - 1);
                    }
                }
            }
            public Row this[int index]
            {
                get
                {
                    return index < Fixed
                        ? _grid.ColumnHeaders.Rows[index]
                        : _grid.Cells.Rows[index - Fixed];
                }
            }
        }

        // Column collection that uses the traditional FlexGrid indexing method:
        // The "Fixed" property gets or sets the number of fixed columns.
        // Column indices from 0 to "Fixed" correspond to row header columns,
        // Column indices from "Fixed" on correspond to scrollable columns 0, 1, etc...
        public class ColumnCollectionTraditionalIndexing
        {
            // ** fields 
            FlexGridTraditionalIndexing _grid;

            // ** ctor
            public ColumnCollectionTraditionalIndexing(FlexGridTraditionalIndexing grid)
            {
                _grid = grid;
            }

            // ** object model
            public int Fixed
            {
                get { return _grid.RowHeaders.Columns.Count; }
                set
                {
                    while (Fixed < value)
                    {
                        _grid.RowHeaders.Columns.Add(new Column());
                    }
                    while (Fixed > value)
                    {
                        _grid.RowHeaders.Columns.RemoveAt(_grid.RowHeaders.Columns.Count - 1);
                    }
                }
            }
            public Column this[int index]
            {
                get
                {
                    return index < Fixed
                        ? _grid.RowHeaders.Columns[index]
                        : _grid.Cells.Columns[index - Fixed];
                }
            }
        }
    }
}
