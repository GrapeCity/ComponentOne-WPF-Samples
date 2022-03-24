using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for Unbound.xaml
    /// </summary>
    public partial class Unbound : Page
    {
        public Unbound()
        {
            InitializeComponent();

            grid.MinColumnWidth = 85;
            PopulateUnboundGrid();
        }

        /// <summary>
        /// Fill unbound grid with data
        /// </summary>
        private void PopulateUnboundGrid()
        {
            // allow merging
            grid.AllowMerging =  AllowMerging.All;

            // add rows/columns to the unbound grid
            for (int i = 0; i < 12; i++) // three years, four quarters per year
            {
                grid.Columns.Add(new Column() { HeaderHorizontalAlignment = HorizontalAlignment.Center });
            }
            for (int i = 0; i < 500; i++)
            {
                grid.Rows.Add(new Row());
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
            ch.Rows.Add(new Row()); // one header row for years, one for quarters
            ch.Rows.Add(new Row());
            for (int c = 0; c < ch.Columns.Count; c++)
            {
                ch[0, c] = 2017 + c / 4; // year
                ch[1, c] = string.Format("Q {0}", c % 4 + 1); // quarter
            }

            // allow merging the first fixed row
            ch.Rows[0].AllowMerging = true;

            // set unbound row headers
            var rh = grid.RowHeaders;
            rh.Columns.Add(new Column());
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

            grid.RowHeaders.AutoSizeColumn(0, 10, true);
            grid.RowHeaders.AutoSizeColumn(1, 10, true);
        }
    }
}
