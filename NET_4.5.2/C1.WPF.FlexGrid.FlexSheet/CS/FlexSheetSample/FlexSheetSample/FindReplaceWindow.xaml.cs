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
using System.Windows.Shapes;

namespace FlexSheetSample
{
    /// <summary>
    /// Interaction logic for FindReplaceWindow.xaml
    /// </summary>
    public partial class FindReplaceWindow : Window
    {
        //--------------------------------------------------------------------------------
        #region ** fields

        C1FlexSheet _owner;
        FindOption _option = new FindOption(FindRange.Sheet, FindPriority.ByRows, false, false);

        #endregion

        //--------------------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a <see cref="FindReplaceWindow"/>.
        /// </summary>
        public FindReplaceWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="FindReplaceWindow"/>.
        /// </summary>
        public FindReplaceWindow(C1FlexSheet owner)
            : this()
        {
            _owner = owner;
           
        }

        #endregion

        //--------------------------------------------------------------------------------
        #region ** event handlers

        private void _tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;

            if (tabControl != null)
            {
                if (tabControl.SelectedIndex != 0)
                {
                    _btnReplace.Visibility = Visibility.Visible;
                    _btnReplaceAll.Visibility = Visibility.Visible;
                    _lbReplace.Visibility = Visibility.Visible;
                    _comboReplace.Visibility = Visibility.Visible;
                }
                else
                {
                    _btnReplace.Visibility = Visibility.Hidden;
                    _btnReplaceAll.Visibility = Visibility.Hidden;
                    _lbReplace.Visibility = Visibility.Hidden;
                    _comboReplace.Visibility = Visibility.Hidden;
                }
            }
        }

        private void _btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void _btnFindNext_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_comboFind.Text))
            {
                return;
            }
            if (!_comboFind.Items.Contains(_comboFind.Text))
                _comboFind.Items.Add(_comboFind.Text);

            _owner.FindNext(_comboFind.Text, _option);
        }

        private void _chkMatchCase_Click(object sender, RoutedEventArgs e)
        {
            _option.IsMatchCase = _chkMatchCase.IsChecked == true;
        }

        private void _chkMatchAll_Click(object sender, RoutedEventArgs e)
        {
            _option.IsMatchEntireContent = _chkMatchAll.IsChecked == true;
        }

        private void _comboWithin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _option.FindRange = _comboWithin.SelectedIndex != 0 ? FindRange.Workbook : FindRange.Sheet;
        }

        private void _comboSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _option.FindPriority = _comboSearch.SelectedIndex != 0 ? FindPriority.ByColumns : FindPriority.ByRows;
        }

        private void _btnReplace_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_comboFind.Text))
            {
                return;
            }
            if (!_comboReplace.Items.Contains(_comboReplace.Text))
                _comboReplace.Items.Add(_comboReplace.Text);

            Replace(_owner.FindNext(_comboFind.Text, _option));
        }

        private void _btnFindAll_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_comboFind.Text))
            {
                return;
            }
            if (!_comboFind.Items.Contains(_comboFind.Text))
                _comboFind.Items.Add(_comboFind.Text);

            _dataGrid.Items.Clear();
  
            List<FindResult> results = _owner.FindAll(_comboFind.Text, _option);

            foreach (FindResult result in results)
            {
                var grid = result.Sheet.Grid;
                if (result.Sheet == _owner.Sheets.SelectedSheet)
                    grid = _owner;
                foreach (CellRange cellRange in result.Cells)
                {
                    string original = null;

                    if (grid[cellRange.Row, cellRange.Column] != null)
                    {
                        original = grid[cellRange.Row, cellRange.Column].ToString();
                    }
                    if (!String.IsNullOrEmpty(original))
                    {
                        CustomFindResult customResult = new CustomFindResult(result.Sheet.SheetName, _owner.GetAddress(cellRange, false), original, cellRange);
                        _dataGrid.Items.Add(customResult);
                    }
                }
            }

            // To show the datagrid.
            if (_dataGrid.Items.Count > 0)
                Height = Height < 350 ? 350 : Height;
        }

        private void _btnReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_comboFind.Text))
            {
                return;
            }
            if (!_comboReplace.Items.Contains(_comboReplace.Text))
                _comboReplace.Items.Add(_comboReplace.Text);

            _dataGrid.Items.Clear();

            List<FindResult> results = _owner.FindAll(_comboFind.Text, _option);

            foreach (FindResult result in results)
            {
                if (result.Sheet.IsProtected)
                {
                    continue;
                }
                var grid = result.Sheet.Grid;
                if (result.Sheet != _owner.Sheets.SelectedSheet)
                    _owner.Sheets.SelectedSheet = result.Sheet;
                    grid = _owner;

                foreach (CellRange cellRange in result.Cells)
                {
                    Replace(cellRange);
                    string original = null;
                    
                    if (grid[cellRange.Row, cellRange.Column] != null)
                    {
                        original = grid[cellRange.Row, cellRange.Column].ToString();
                    }
                    if (!String.IsNullOrEmpty(original))
                    {
                        CustomFindResult customResult = new CustomFindResult(result.Sheet.SheetName, _owner.GetAddress(cellRange, false), original, cellRange);
                        _dataGrid.Items.Add(customResult);
                    }
                    
                }
            }
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var clickResult = _dataGrid.SelectedItem as CustomFindResult;
            var selectedSheet = _owner.Sheets.FirstOrDefault(x => x.SheetName == clickResult.Sheet);
            if (selectedSheet != null)
            {
                _owner.Sheets.SelectedSheet = selectedSheet;
            }
            _owner.Selection = clickResult.CellRange;
        }

        #endregion

        //--------------------------------------------------------------------------------
        #region ** implementation

        private void Replace(CellRange cellRange)
        {
            
            if (cellRange.IsValid)
            {
                string original = null;
                if (_owner[cellRange.Row, cellRange.Column] != null) 
                {
                    original = _owner[cellRange.Row, cellRange.Column].ToString();
                }
                if (!String.IsNullOrEmpty(original))
                {
                    _owner[cellRange.Row, cellRange.Column] = original.Replace(_comboFind.Text, _comboReplace.Text);
                }
            }
        }

        #endregion
    }

    public class CustomFindResult
    {
        public CustomFindResult(string sheet, string cell, string value, CellRange cellRange)
        {
            Sheet = sheet;
            Cell = cell;
            Value = value;
            CellRange = cellRange;
        }

        public string Sheet { get; set; }
        public string Cell { get; set; }
        public string Value { get; set; }
        public CellRange CellRange { get; set; }
    }
}
