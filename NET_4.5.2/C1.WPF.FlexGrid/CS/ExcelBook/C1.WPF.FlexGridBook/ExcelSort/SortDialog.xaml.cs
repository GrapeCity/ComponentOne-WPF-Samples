using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Dialog used to edit the grid's sort parameters.
    /// </summary>
    public partial class SortDialog : UserControl
    {
        //--------------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _owner;
        Popup _popup;
        ObservableCollection<UnboundSortDescription> _sdc = new ObservableCollection<UnboundSortDescription>();

        #endregion

        //--------------------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a <see cref="SortDialog"/>.
        /// </summary>
        public SortDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of a <see cref="SortDialog"/>.
        /// </summary>
        public SortDialog(C1FlexGridBook owner)
            : this()
        {
            // save reference to owner grid (the one being sorted)
            _owner = owner;

            // initialize sort descriptors
            _sdc.Clear();
            var sel = _owner.Selection;
            if (sel.IsValid)
            {
                for (int c = sel.LeftColumn; c <= sel.RightColumn; c++)
                {
                    var sd = new UnboundSortDescription(_owner.Columns[c], ListSortDirection.Ascending);
                    _sdc.Add(sd);
                }
            }
            else
            {
                _sdc.Add(new UnboundSortDescription());
            }

            // update UI when the collection/selection change
            _flex.SelectionChanged += _flex_SelectionChanged;

            // add data map to "Column" column
            var col = _flex.Columns["Column"];
            var dctColumn = new Dictionary<Column, string>();
            foreach (var c in _owner.Columns)
            {
                dctColumn[c] = GetColumnName(c);
            }
            col.ValueConverter = new ColumnValueConverter(dctColumn);

            // add data map to "Direction" column
            col = _flex.Columns["Direction"];
            var dctOrder = new Dictionary<ListSortDirection, string>();
            dctOrder[ListSortDirection.Ascending] = "Ascending";
            dctOrder[ListSortDirection.Descending] = "Descending";
            col.ValueConverter = new ColumnValueConverter(dctOrder);

            // show sort descriptors on grid
            _flex.ItemsSource = _sdc;
        }

        #endregion

        //--------------------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Shows the dialog to allow users to edit the sort parameters.
        /// </summary>
        public void Show()
        {
            // if we're already open, ignore
            if (_popup != null)
            {
                return;
            }

            // update font properties
            FontFamily = _owner.FontFamily;
            FontSize = _owner.FontSize;

            // create 'canvas' for popup
            var g = new Grid();
            var root = Window.GetWindow(_owner);
            g.Width = this.Width;
            g.Height = this.Height;
            g.Background = new SolidColorBrush(Color.FromArgb(0x20, 0, 0, 0));
            g.Children.Add(this);
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;

            // close dialog when root is resized
            root.SizeChanged += root_SizeChanged;

            // create and show popup
            _popup = new Popup();
            _popup.Placement = PlacementMode.Center;
            _popup.PlacementTarget = root;
            _popup.Child = g;
            _popup.IsOpen = true;

            // set focus to 'add' button
            _btnAddLevel.Focus();
        }
        /// <summary>
        /// Closes the dialog and raises the <see cref="DialogClosed"/> event.
        /// </summary>
        /// <param name="cancel">Whether the changes made by the user should be
        /// ignored or applied to the source collection.</param>
        public void Close(bool cancel)
        {
            if (_popup != null)
            {
                // check and apply sort criteria
                if (!cancel && !ApplySortCriteria())
                {
                    return;
                }

                // disconnect from root 
                var root = Window.GetWindow(_owner);
                root.SizeChanged -= root_SizeChanged;
                
                // close dialog
                _popup.IsOpen = false;
                _popup.Child = null;
                _popup = null;
                OnDialogClosed(new CancelEventArgs() { Cancel = cancel });
            }
        }
        /// <summary>
        /// Occurs when the user closes the dialog.
        /// </summary>
        public event EventHandler<CancelEventArgs> DialogClosed;
        /// <summary>
        /// Raises the <see cref="DialogClosed"/> event.
        /// </summary>
        /// <param name="e"><see cref="CancelEventArgs"/> that indicates whether
        /// the new sort parameters should be applied or discarded.</param>
        protected virtual void OnDialogClosed(CancelEventArgs e)
        {
            if (DialogClosed != null)
                DialogClosed(this, e);
        }
        #endregion

        //--------------------------------------------------------------------------------
        #region ** event handlers

        // add a sort level
        void _btnAddLevel_Click(object sender, RoutedEventArgs e)
        {
            _sdc.Add(new UnboundSortDescription());
            _flex.SelectedIndex = _sdc.Count - 1;
        }

        // delete a sort level
        void _btnDeleteLevel_Click(object sender, RoutedEventArgs e)
        {
            _sdc.RemoveAt(_flex.SelectedIndex);
            UpdateUI();
        }

        // copy a sort level
        void _btnCopyLevel_Click(object sender, RoutedEventArgs e)
        {
            var index = _flex.SelectedIndex;
            var item = _sdc[index];
            var sd = new UnboundSortDescription(item.Column, item.Direction);
            _sdc.Insert(index + 1, sd);
            _flex.SelectedIndex++;
        }

        // move current sort level up
        void _btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            _flex.FinishEditing();
            var index = _flex.SelectedIndex;
            if (index > -1)
            {
                var item = _sdc[index];
                _sdc.RemoveAt(index);
                _sdc.Insert(index - 1, item);
                _flex.SelectedIndex = index - 1;
            }
        }

        // move current sort level down
        void _btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            _flex.FinishEditing();
            var index = _flex.SelectedIndex;
            if (index > -1)
            {
                var item = _sdc[index];
                _sdc.RemoveAt(index);
                _sdc.Insert(index + 1, item);
                _flex.SelectedIndex = index + 1;
            }
        }

        // update UI when the selection changes
        void _flex_SelectionChanged(object sender, C1.WPF.FlexGrid.CellRangeEventArgs e)
        {
            UpdateUI();
        }

        // close dialog if the root size changes
        void root_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Close(true);
        }

        // close and apply changes
        void _btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        // close and cancel changes
        void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close(true);
        }

        // close and cancel changes
        void _btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        // close and cancel changes when user presses escape
        void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close(true);
            }
        }

        #endregion 
        
        //--------------------------------------------------------------------------------
        #region ** implementation

        // enable/disable buttons
        void UpdateUI()
        {
            // select an item if we can
            if (_flex.SelectedIndex < 0 && _sdc.Count > 0)
            {
                _flex.SelectedIndex = 0;
            }

            // enable/disable buttons to reflect state
            bool hasItem = _flex.SelectedIndex > -1;
            _btnCopyLevel.IsEnabled = hasItem;
            _btnDeleteLevel.IsEnabled = hasItem;
            _btnMoveUp.IsEnabled = hasItem && _flex.SelectedIndex > 0;
            _btnMoveDown.IsEnabled = hasItem && _flex.SelectedIndex < _sdc.Count - 1;
        }

        // check and apply sort criteria
        bool ApplySortCriteria()
        {
            // check sort descriptors
            foreach (var sortDesc in _sdc)
            {
                if (sortDesc.Column == null)
                {
                    // MessageBox appears behind the popup, so close the popup before showing it
                    this.Close(true);

                    // empty sort criteria
                    var msg = "All sort criteria must have a column specified.\r\nCheck the selected sort criteria and try again.";
                    MessageBox.Show(Application.Current.MainWindow, msg, "Sort", MessageBoxButton.OK);
                    return false;
                }
                else
                {
                    foreach (var sortDesc2 in _sdc)
                    {
                        if (sortDesc2 != sortDesc && sortDesc2.Column == sortDesc.Column)
                        {
                            // MessageBox appears behind the popup, so close the popup before showing it
                            this.Close(true);

                            // duplicate sort criteria
                            var msg = "Column '{0}' is being sorted more than once.\r\nDelete the duplicate sort criteria and try again.";
                            msg = string.Format(msg, sortDesc.Column);
                            MessageBox.Show(Application.Current.MainWindow, msg, "Sort", MessageBoxButton.OK);
                            return false;
                        }
                    }
                }
            }

            // apply undoable sort based on our sort descriptor collection
            var sa = new SortAction(_owner);
            UnboundSort.SortUnboundGrid(_owner, _sdc);
            sa.SaveNewState();
            _owner.UndoStack.AddAction(sa);

            // done
            return true;
        }

        // get a column name suitable for showing in the drop-down
        string GetColumnName(Column c)
        {
            var colName = C1FlexGridBook.GetAlphaColumnHeader(c.Index);
            return string.Format("Column {0}", colName);
        }
        #endregion
    }
}
