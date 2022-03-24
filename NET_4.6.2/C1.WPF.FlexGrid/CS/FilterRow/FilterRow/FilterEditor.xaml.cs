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
using System.ComponentModel;
using C1.WPF.FlexGrid;

namespace FilterRow
{
    /// <summary>
    /// Interaction logic for FilterEditor.xaml
    /// </summary>
    public partial class FilterEditor : UserControl
    {
        // ** fields
        C1FlexGrid _grid;
        GridFilterRow _row;
        Column _col;
        bool _readOnly;
        KeyAction _kaTab;

        // ** ctor
        public FilterEditor()
        {
            InitializeComponent();
        }
        public FilterEditor(GridFilterRow row, Column col)
        {
            InitializeComponent();

            // store grid parameters
            _grid = row.Grid;
            _row = row;
            _col = col;

            // initialize editor content from values stored in the row
            // (the editor is transient and can't be used for storage)
            var filterArgument = _row.GetFilterArgument(_col);

            if (_col.DataType == typeof(bool))
            {
                // show checkbox for Boolean values
                _cbValue.Visibility = System.Windows.Visibility.Visible;
                _tbValue.Visibility = System.Windows.Visibility.Hidden;

                // initialize editors
                bool cb;
                _cbValue.IsChecked = bool.TryParse(filterArgument, out cb) ? (bool?)cb : null;
                _tbValue.Text = null;
            }
            else
            {
                // show TextBox for non-Boolean values
                _cbValue.Visibility = System.Windows.Visibility.Hidden;
                _tbValue.Visibility = System.Windows.Visibility.Visible;

                // initialize editors
                _tbValue.Text = _row.GetFilterArgument(_col);
                _cbValue.IsChecked = null;
            }

            // show filter image if the filter is active
            UpdateFilterImage();
        }

        // ** event handlers

        // text box
        void _tbValue_GotFocus(object sender, RoutedEventArgs e)
        {
            // make grid read-only while editing the filter
            _readOnly = _grid.IsReadOnly;
            _grid.IsReadOnly = true;

            // make tabs cycle through filter controls
            _kaTab = _grid.KeyActionTab;
            _grid.KeyActionTab = KeyAction.Default;
        }
        void _tbValue_LostFocus(object sender, RoutedEventArgs e)
        {
            // restore read-only/KeyActionTab state
            _grid.IsReadOnly = _readOnly;
            _grid.KeyActionTab = _kaTab;

            // apply the new filter condition
            _row.SetFilterArgument(_col, _tbValue.Text);
        }
        void _tbValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _col.Grid.Focus();
            }
        }
        void _tbValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilterImage();
        }

        // check box
        void _cbValue_GotFocus(object sender, RoutedEventArgs e)
        {
            // make grid read-only while editing the filter
            _readOnly = _grid.IsReadOnly;
            _grid.IsReadOnly = true;
        }
        void _cbValue_LostFocus(object sender, RoutedEventArgs e)
        {
            // restore read-only state
            _grid.IsReadOnly = _readOnly;
        }
        void _cbValue_Click(object sender, RoutedEventArgs e)
        {
            // apply the new filter condition
            _row.SetFilterArgument(_col, _cbValue.IsChecked.ToString());

            // update filter visibility
            UpdateFilterImage();
        }

        // filter image
        void _imgClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // clear filter value
            _tbValue.Text = string.Empty;
            _cbValue.IsChecked = null;

            // clear the filter condition
            _row.SetFilterArgument(_col, string.Empty);

            // done with the mouse
            e.Handled = true;
        }
        void _imgClear_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = 1;
        }
        void _imgClear_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = 0.4;
        }

        // ** implementation
        void UpdateFilter()
        {
            // store filter value in row
            // (the editor is transient and can't be used for storage)
            _row.SetFilterArgument(_col, _tbValue.Text);
        }
        void UpdateFilterImage()
        {
            // show filter image if we are currently active
            _imgClear.Visibility = string.IsNullOrEmpty(_tbValue.Text) && !_cbValue.IsChecked.HasValue
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}
