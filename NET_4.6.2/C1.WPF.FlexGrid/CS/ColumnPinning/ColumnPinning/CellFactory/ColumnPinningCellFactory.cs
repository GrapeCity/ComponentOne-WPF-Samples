using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using C1.WPF;
using C1.WPF.FlexGrid;

namespace ColumnPinning
{
    /// <summary>
    /// A customized Cell Factory that will add a pinning icon to each Column Header.
    /// </summary>
    class ColumnPinningCellFactory : CellFactory
    {
        public ObservableCollection<int> ColumnRecord;
        const string PIN_ICON = "M 20.53125 2.5625 L 19.84375 3.5 L 14.9375 10.1875 C 12.308594 9.730469 9.527344 10.472656 7.5 12.5 L 6.78125 13.1875 L 12.09375 18.5 L 4 26.59375 L 4 28 L 5.40625 28 L 13.5 19.90625 L 18.8125 25.21875 L 19.5 24.5 C 21.527344 22.472656 22.269531 19.691406 21.8125 17.0625 L 28.5 12.15625 L 29.4375 11.46875 Z M 20.78125 5.625 L 26.375 11.21875 L 20.15625 15.78125 L 19.59375 16.1875 L 19.78125 16.84375 C 20.261719 18.675781 19.738281 20.585938 18.59375 22.1875 L 9.8125 13.40625 C 11.414063 12.261719 13.324219 11.738281 15.15625 12.21875 L 15.8125 12.40625 L 16.21875 11.84375 Z "; // Pin icon
        const string UNPIN_ICON = "M 3.71875 2.28125 L 2.28125 3.71875 L 28.28125 29.71875 L 29.71875 28.28125 L 21.75 20.3125 C 21.988678 19.231627 22.072023 18.111911 21.875 17 L 29.4375 11.46875 L 20.53125 2.5625 L 15 10.125 C 13.889037 9.9274235 12.764228 10.013944 11.6875 10.25 L 3.71875 2.28125 z M 20.78125 5.625 L 26.375 11.21875 L 19.59375 16.1875 L 19.78125 16.84375 C 19.930164 17.410929 20.006357 17.989892 20 18.5625 L 13.4375 12 C 14.010039 11.993776 14.588137 12.069369 15.15625 12.21875 L 15.8125 12.40625 L 20.78125 5.625 z M 8.21875 11.84375 C 7.96575 12.04475 7.732 12.269 7.5 12.5 L 6.78125 13.1875 L 12.09375 18.5 L 4 26.59375 L 4 28 L 5.40625 28 L 13.5 19.90625 L 18.8125 25.21875 L 19.5 24.5 C 19.731 24.269 19.95625 24.03425 20.15625 23.78125 L 8.21875 11.84375 z"; // Unpin icon
        /// <summary>
        /// Define customized content for the column header.
        /// </summary>
        /// <param name="grid">The grid object which the Pin icon will be drawn on.</param>
        /// <param name="bdr">The border that wraps around the element.</param>
        /// <param name="rng"></param>
        public override void CreateColumnHeaderContent(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            base.CreateColumnHeaderContent(grid, bdr, rng);
            if (ColumnRecord == null)
            {
                // Populate ColumnRecord collection to stored the original index of each column before pinning them.
                ColumnRecord = new ObservableCollection<int>();
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    ColumnRecord.Add(i);
                }
            }
            TextBlock freeze = new TextBlock();
            freeze.Style = (Style)Application.Current.Resources["SymbolText"];
            freeze.Width = 17;
            freeze.Visibility = Visibility.Visible;
            //Use border to increase the hit test area
            Border border = new Border();
            border.Child = freeze;
            border.Background = Brushes.Transparent;
            border.MouseLeftButtonDown += (sender, e) => SetPinColumn(sender, e, grid, rng);
            border.Width = 20;

            // note, this method is called every time when column headers are re-drawn 
            // so it is a good place to set appearance depending on whether column is frozen or not
            int col = rng.Column;
            if (col >= grid.FrozenColumns)
            {
                // set icon appearance for not frozen column
                freeze.Text = "\ue718"; // Pin icon
            }
            else
            {
                // set icon appearance for pinned column
                freeze.Text = "\ue840"; // Pinned icon
            }

            Grid container = bdr.Child as Grid;
            ConcatenateElements(bdr, bdr.Child, border, 1);
        }
        /// <summary>
        /// Pin/Unpin column.
        /// </summary>
        /// <param name="sender">Event caller.</param>
        /// <param name="e">Event argument.</param>
        /// <param name="grid">The grid object.</param>
        /// <param name="rng">The cellrange to be operated on.</param>
        private void SetPinColumn(object sender, MouseButtonEventArgs e, C1FlexGrid grid, CellRange rng)
        {
            Border pinIcon = (Border)sender;
            int col = rng.Column;
            // When pinning column, we move that column to the left side and increment the FrozenCol number.
            if (col >= grid.FrozenColumns)
            {
                if (col != grid.FrozenColumns)
                {
                    grid.Columns.Move(col, grid.FrozenColumns);
                    ColumnRecord.Move(col, grid.FrozenColumns);
                }
                grid.FrozenColumns++;
            }
            // When unpinning column, we move that column back to its corresponding position based on the ColumnRecord.
            else
            {
                grid.FrozenColumns--;
                MoveColumn(col, grid);
            }
        }

        /// <summary>
        /// Move column to the right position upon unpinning.
        /// </summary>
        /// <param name="index">The position of the column prior to unpin event.</param>
        /// <param name="grid">The grid which contains the column.</param>
        private void MoveColumn(int index, C1FlexGrid grid)
        {
            int value = ColumnRecord[index];
            //Move the col outside of Frozen area first-hand
            if (index != grid.FrozenColumns)
            {
                grid.Columns.Move(index, grid.FrozenColumns);
                ColumnRecord.Move(index, grid.FrozenColumns);
            }

            if (grid.FrozenColumns == 0 && value == 0)
            {
                return;
            }

            int curIndex = grid.FrozenColumns;
            for (int i = grid.FrozenColumns; i < ColumnRecord.Count; i++)
            {
                if (ColumnRecord[curIndex] < ColumnRecord[i])
                {
                    grid.Columns.Move(curIndex, i - 1);
                    ColumnRecord.Move(curIndex, i - 1);
                    break;
                }
                if (i == ColumnRecord.Count - 1)
                {
                    grid.Columns.Move(curIndex, i);
                    ColumnRecord.Move(curIndex, i);
                    break;
                }
            }
        }

        /// <summary>
        /// Concatenate 2 UIelement in order from left to right.
        /// </summary>
        /// <param name="bdr"></param>
        /// <param name="e1">The left element.</param>
        /// <param name="e2">The right element.</param>
        /// <param name="autoCol"></param>
        void ConcatenateElements(Border bdr, UIElement e1, UIElement e2, int autoCol)
        {
            //Debug.Assert(e1 != null && e2 != null);
            if (e1 == null || e2 == null)
            {
                return;
            }
            var g = new Grid();
            g.Background = new SolidColorBrush(Colors.Transparent); // make sure it's transparent
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.ColumnDefinitions[autoCol].Width = new GridLength(1, GridUnitType.Auto);

            bdr.Child = null;
            g.Children.Add(e1);
            g.Children.Add(e2);
            e2.SetValue(Grid.ColumnProperty, 1);
            bdr.Child = g;
        }
    }
}
