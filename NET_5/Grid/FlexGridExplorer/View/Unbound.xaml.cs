using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for Unbound.xaml
    /// </summary>
    public partial class Unbound : UserControl
    {
        public Unbound()
        {
            InitializeComponent();
            Tag = AppResources.UnboundDescription;
            grid.CellFactory = new CustomCellFactory();
            grid.MinColumnWidth = 85;
            PopulateUnboundGrid();
        }

        /// <summary>
        /// Fill unbound grid with data
        /// </summary>
        private void PopulateUnboundGrid()
        {
            // allow merging
            grid.AllowMerging = GridAllowMerging.All;

            // add rows/columns to the unbound grid
            for (int i = 0; i < 12; i++) // three years, four quarters per year
            {
                grid.Columns.Add(new GridColumn() { HeaderHorizontalAlignment = HorizontalAlignment.Center });
            }
            for (int i = 0; i < 500; i++)
            {
                grid.Rows.Add(new GridRow());
            }

            // populate the unbound grid with some stuff
            for (int r = 0; r < grid.Rows.Count; r++)
            {
                for (int c = 0; c < grid.Columns.Count; c++)
                {
                    grid[r, c] = string.Format("cell [{0},{1}]", r, c);
                }
            }

            // set unbound column headers
            var ch = grid.ColumnHeaders;
            ch.Rows.Clear();
            ch.Rows.Add(new GridRow()); // one header row for years, one for quarters
            ch.Rows.Add(new GridRow());
            for (int c = 0; c < ch.Columns.Count; c++)
            {
                ch[0, c] = 2017 + c / 4; // year
                ch[1, c] = string.Format("Q {0}", c % 4 + 1); // quarter
            }

            // allow merging the first fixed row
            ch.Rows[0].AllowMerging = true;

            // set unbound row headers
            var rh = grid.RowHeaders;
            rh.Columns.Add(new GridColumn());
            for (int c = 0; c < rh.Columns.Count; c++)
            {
                rh.Columns[c].Width = new GridLength(60);
                for (int r = 0; r < rh.Rows.Count; r++)
                {
                    rh[r, c] = string.Format("hdr {0},{1}", c == 0 ? r / 2 : r, c);
                }
            }

            // allow merging the first fixed column
            rh.Columns[0].AllowMerging = true;

            grid.RowHeaders.Columns[0].Width = GridLength.Auto;
            grid.RowHeaders.Columns[1].Width = GridLength.Auto;
        }
    }

    public class CustomCellFactory : GridCellFactory
    {
        public override void PrepareCell(GridCellType cellType, GridCellRange range, GridCellView cell, Thickness internalBorders)
        {
            base.PrepareCell(cellType, range, cell, internalBorders);
            if (cellType == GridCellType.Cell && range.Column % 4 == 0)
                cell.BorderThickness = new Thickness(1, 0, 0, 0); //Custom vertical grid line that separate years
        }
    }

}
