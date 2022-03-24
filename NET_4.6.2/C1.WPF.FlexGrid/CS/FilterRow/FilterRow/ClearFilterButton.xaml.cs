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

namespace FilterRow
{
    /// <summary>
    /// Interaction logic for ClearFilterButton.xaml
    /// </summary>
    public partial class ClearFilterButton : UserControl
    {
        // ** fields
        GridFilterRow _row;

        // ** ctors
        public ClearFilterButton()
        {
            InitializeComponent();
        }
        public ClearFilterButton(GridFilterRow row)
        {
            InitializeComponent();
            _row = row;
        }

        // ** event handlers

        // clear all column filters when the user clicks the clear filter button
        void _btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_row != null && _row.Grid != null)
            {
                // clear filters
                foreach (var col in _row.Grid.Columns)
                {
                    _row.SetFilterArgument(col, string.Empty);
                }

                // invalidate grid to show that the filters are empty
                _row.Grid.Invalidate();

                // done with the mouse
                e.Handled = true;
            }
        }

        // update button opacity when mouse enters/leaves the button
        void _btn_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = 1;
        }
        void _btn_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = .4;
        }
    }
}
