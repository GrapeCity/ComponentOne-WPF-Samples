using C1.WPF.FlexGrid;
using FlexGrid101.Resources;
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
    /// Interaction logic for CustomMerging.xaml
    /// </summary>
    public partial class CustomMerging : Page
    {
        public CustomMerging()
        {
            InitializeComponent();

            grid.SelectionChanged += Grid_SelectionChanged;
            grid.MinColumnWidth = 85;
            grid.MergeManager = new MyMergeManager();

            PopulateGrid();
        }

        private void PopulateGrid()
        {
            grid.Columns.Add(new Column { Header = AppResources.Monday, Width = new GridLength(1, GridUnitType.Star), MinWidth = 120, AllowMerging = true, HeaderHorizontalAlignment = HorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
            grid.Columns.Add(new Column { Header = AppResources.Tuesday, Width = new GridLength(1, GridUnitType.Star), MinWidth = 120, AllowMerging = true, HeaderHorizontalAlignment = HorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
            grid.Columns.Add(new Column { Header = AppResources.Wednesday, Width = new GridLength(1, GridUnitType.Star), MinWidth = 120, AllowMerging = true, HeaderHorizontalAlignment = HorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
            grid.Columns.Add(new Column { Header = AppResources.Thursday, Width = new GridLength(1, GridUnitType.Star), MinWidth = 120, AllowMerging = true, HeaderHorizontalAlignment = HorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
            grid.Columns.Add(new Column { Header = AppResources.Friday, Width = new GridLength(1, GridUnitType.Star), MinWidth = 120, AllowMerging = true, HeaderHorizontalAlignment = HorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
            grid.Columns.Add(new Column { Header = AppResources.Saturday, Width = new GridLength(1, GridUnitType.Star), MinWidth = 120, AllowMerging = true, HeaderHorizontalAlignment = HorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
            grid.Columns.Add(new Column { Header = AppResources.Sunday, Width = new GridLength(1, GridUnitType.Star), MinWidth = 120, AllowMerging = true, HeaderHorizontalAlignment = HorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });

            grid.Rows.Add(new Row());
            grid.Rows.Add(new Row());
            grid.Rows.Add(new Row());
            grid.Rows.Add(new Row());
            grid.Rows.Add(new Row());
            grid.Rows.Add(new Row());
            grid.Rows.Add(new Row());

            grid.ColumnHeaders.Rows.Insert(0, new Row() { AllowMerging = true });
            grid.ColumnHeaders[0, 0] = AppResources.Weekday;
            grid.ColumnHeaders[0, 1] = AppResources.Weekday;
            grid.ColumnHeaders[0, 2] = AppResources.Weekday;
            grid.ColumnHeaders[0, 3] = AppResources.Weekday;
            grid.ColumnHeaders[0, 4] = AppResources.Weekday;
            grid.ColumnHeaders[0, 5] = AppResources.Weekend;
            grid.ColumnHeaders[0, 6] = AppResources.Weekend;

            grid.ColumnHeaders[1, 0] = AppResources.Monday;
            grid.ColumnHeaders[1, 1] = AppResources.Tuesday;
            grid.ColumnHeaders[1, 2] = AppResources.Wednesday;
            grid.ColumnHeaders[1, 3] = AppResources.Thursday;
            grid.ColumnHeaders[1, 4] = AppResources.Friday;
            grid.ColumnHeaders[1, 5] = AppResources.Saturday;
            grid.ColumnHeaders[1, 6] = AppResources.Sunday;

            grid.RowHeaders.Columns[0].Width = GridLength.Auto;
            grid.RowHeaders[0, 0] = "12:00";
            grid.RowHeaders[1, 0] = "13:00";
            grid.RowHeaders[2, 0] = "14:00";
            grid.RowHeaders[3, 0] = "15:00";
            grid.RowHeaders[4, 0] = "16:00";
            grid.RowHeaders[5, 0] = "17:00";
            grid.RowHeaders[6, 0] = "18:00";

            grid[0, 0] = "Walker";
            grid[0, 1] = "Morning Show";
            grid[0, 2] = "Morning Show";
            grid[0, 3] = "Sports";
            grid[0, 4] = "Weather";
            grid[0, 5] = "N/A";
            grid[0, 6] = "N/A";
            grid[1, 5] = "N/A";
            grid[1, 6] = "N/A";
            grid[2, 5] = "N/A";
            grid[2, 6] = "N/A";
            grid[3, 5] = "N/A";
            grid[3, 6] = "N/A";
            grid[4, 5] = "N/A";
            grid[4, 6] = "N/A";
            grid[1, 0] = "Today Show";
            grid[1, 1] = "Today Show";
            grid[2, 0] = "Today Show";
            grid[2, 1] = "Today Show";
            grid[1, 2] = "Sesame Street";
            grid[1, 3] = "Football";
            grid[2, 3] = "Football";
            grid[1, 4] = "Market Watch";
            grid[2, 2] = "Kids Zone";
            grid[2, 4] = "Soap Opera";
            grid[3, 0] = "News";
            grid[3, 1] = "News";
            grid[3, 2] = "News";
            grid[3, 3] = "News";
            grid[3, 4] = "News";
            grid[4, 0] = "News";
            grid[4, 1] = "News";
            grid[4, 2] = "News";
            grid[4, 3] = "News";
            grid[4, 4] = "News";
            grid[5, 0] = "Wheel of Fortune";
            grid[5, 1] = "Wheel of Fortune";
            grid[5, 2] = "Wheel of Fortune";
            grid[5, 3] = "Jeopardy";
            grid[5, 4] = "Jeopardy";
            grid[5, 5] = "Movie";
            grid[6, 5] = "Movie";
            grid[5, 6] = "Golf";
            grid[6, 6] = "Golf";
            grid[6, 0] = "Night Show";
            grid[6, 1] = "Night Show";
            grid[6, 2] = "Sports";
            grid[6, 3] = "Big Brother";
            grid[6, 4] = "Big Brother";
        }

        private void Grid_SelectionChanged(object sender, CellRangeEventArgs e)
        {

            string selectedText = grid[e.CellRange.Row, e.CellRange.Column].ToString();
            labelShowName.Content = selectedText;
            labelShowTimes.Content = "";
            for (int c = 0; c < grid.Columns.Count; c++)
            {
                string dayName = grid.ColumnHeaders[1,c].ToString();
                string startTime = "";
                for (int r = 0; r < grid.Rows.Count; r++)
                {
                    string cellValue = grid[r, c].ToString();

                    if (cellValue.Equals(selectedText))
                    {
                        if (startTime == "")
                        {
                            startTime = grid.RowHeaders[r,0].ToString();
                            labelShowTimes.Content = labelShowTimes.Content + dayName + " " + startTime + "-";
                        }
                    }
                    else if (startTime != "" && labelShowTimes.Content.ToString().EndsWith("-"))
                    {
                        string endTime = grid.RowHeaders[r, 0].ToString();
                        labelShowTimes.Content = labelShowTimes.Content + endTime + "\n";
                    }

                    // handle last row exception
                    if (r == grid.Rows.Count - 1 && startTime != "" && labelShowTimes.Content.ToString().EndsWith("-"))
                    {
                        labelShowTimes.Content = labelShowTimes.Content + "19:00\n";
                    }
                }
            }
        }
    }

    public class MyMergeManager : IMergeManager
    {
        public CellRange GetMergedRange(C1FlexGrid grid, CellType cellType, CellRange rg)
        {
            if (cellType == CellType.Cell)
            {
                // expand left/right
                for (int i = rg.Column; i < grid.Columns.Count - 1; i++)
                {
                    if (GetDataDisplay(grid, rg.Row, i) != GetDataDisplay(grid, rg.Row, i + 1)) break;
                    rg.Column2 = i + 1;
                }
                for (int i = rg.Column; i > 0; i--)
                {
                    if (GetDataDisplay(grid, rg.Row, i) != GetDataDisplay(grid, rg.Row, i - 1)) break;
                    rg.Column = i - 1;
                }

                // expand up/down
                for (int i = rg.Row; i < grid.Rows.Count - 1; i++)
                {
                    if (GetDataDisplay(grid, i, rg.Column) != GetDataDisplay(grid, i + 1, rg.Column)) break;
                    rg.Row2 = i + 1;
                }
                for (int i = rg.Row; i > 0; i--)
                {
                    if (GetDataDisplay(grid, i, rg.Column) != GetDataDisplay(grid, i - 1, rg.Column)) break;
                    rg.Row = i - 1;
                }
            }

            if (cellType == CellType.ColumnHeader)
            {
                for (int i = rg.Column; i < grid.Columns.Count - 1; i++)
                {
                    if (GetColumnHeaderDataDisplay(grid, rg.Row, i) != GetColumnHeaderDataDisplay(grid, rg.Row, i + 1)) break;
                    rg.Column2 = i + 1;
                }
                for (int i = rg.Column; i > 0; i--)
                {
                    if (GetColumnHeaderDataDisplay(grid, rg.Row, i) != GetColumnHeaderDataDisplay(grid, rg.Row, i - 1)) break;
                    rg.Column = i - 1;
                }
            }

            // done
            return rg;
        }
        string GetDataDisplay(C1FlexGrid grid, int r, int c)
        {
            return grid[r, c].ToString();
        }

        string GetColumnHeaderDataDisplay(C1FlexGrid grid, int r, int c)
        {
            return grid.ColumnHeaders[r, c].ToString();
        }
    }
}
