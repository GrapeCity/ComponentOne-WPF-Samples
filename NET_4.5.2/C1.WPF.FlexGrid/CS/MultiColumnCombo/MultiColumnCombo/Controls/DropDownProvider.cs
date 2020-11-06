using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using C1.WPF.FlexGrid;

namespace MultiColumnCombo
{
    using C1.Util;

    /// <summary>
    /// Provides the dropdown items for a <see cref="TextBox"/> control.
    /// </summary>
    internal class DropDownProvider
    {
        //-----------------------------------------------------------------
        #region ** fields

        MultiColumnComboBox _cb;
        Popup _popup;
        C1FlexGrid _flex;
        bool _keyDown = true;
        bool _updatingText;
        bool _updatingSelection;

        static Brush _brWhite = new SolidColorBrush(Colors.White);
        static Brush _brGrey = new SolidColorBrush(Color.FromArgb(0xff, 0xc0, 0xc0, 0xc0));
        static Thickness _thkSingle = new Thickness(1);

        #endregion

        //-----------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a <see cref="DropDownProvider"/>.
        /// </summary>
        public DropDownProvider(MultiColumnComboBox cb)
        {
            // hook up to parent control
            _cb = cb;
            _cb.TextChanged += _cbTextChanged;
            _cb.LostFocus += _cbLostFocus;
            _cb.PreviewKeyDown += _cbKeyDown;

            // initialize dropdown
            _flex = new C1FlexGrid();
            _flex.LostFocus += _cbLostFocus;
            _flex.SelectionChanged += _flex_SelectionChanged;
            UpdateGridLayout();
        }
        #endregion

        //-----------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Indicates whether the dropdown is currently being displayed.
        /// </summary>
        public bool IsDroppedDown
        {
            get { return _popup != null && _popup.IsOpen; }
            set
            {
                var oldValue = IsDroppedDown;
                if (IsDroppedDown && !value)
                {
                    try
                    {
                        _cb.SelectAll();
                        CloseDropDown();
                    }
                    catch 
                    { 
                        /* occasional weird WPF exception */  
                    }
                }
                else if (value)
                {
                    ShowDropDown();
                }
                if (oldValue != IsDroppedDown)
                {
                    _cb.OnIsDroppedDownChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Gets or sets the maximum height of the dropdown in pixels.
        /// </summary>
        public double MaxDropDownHeight { get; set; }
        /// <summary>
        /// Gets or sets a value that determines whether the textbox is editable 
        /// or restricted to the items in the <see cref="DropDownItems"/> 
        /// collection.
        /// </summary>
        public bool IsEditable { get; set; }
        /// <summary>
        /// Gets a reference to the <see cref="C1FlexGrid"/> control in the dropdown.
        /// </summary>
        public C1FlexGrid DropDownGrid
        {
            get { return _flex; }
        }
        /// <summary>
        /// Selects an item in the DropDown by index.
        /// </summary>
        /// <param name="index">Index of the item to select.</param>
        public void SetSelectedIndex(int index)
        {
            // update index
            if (_flex.Rows.Count > 0 && index != _flex.SelectedIndex)
            {
                _updatingSelection = true;
                _flex.SelectedIndex = index;
                _updatingSelection = false;
            }

            // update text
            if (_flex.SelectedItem != null)
            {
                UpdateComboText();
                _cb.SelectAll();
            }

            // update SelectedIndex, SelectedValue properties
            _cb.SelectedIndex = index;
            _cb.SelectedValue = GetItemValue(index);
        }
        /// <summary>
        /// Selects an item in the DropDown by value.
        /// </summary>
        /// <param name="value">Value of the item to select.</param>
        public void SetSelectedValue(object value)
        {
            var index = SearchByValue(value);
            if (index != _cb.SelectedIndex)
            {
                SetSelectedIndex(index);
            }
        }
        #endregion

        //-----------------------------------------------------------------
        #region ** event handlers

        // handle user selection on the dropdown
        void _flex_SelectionChanged(object sender, CellRangeEventArgs e)
        {
            // not if we're updating
            if (_updatingSelection)
            {
                _updatingSelection = false;
                return;
            }

            // update textbox to match drop-down selection
            UpdateComboText();

            // close dropdown
            IsDroppedDown = false;

            // update selected index
            SetSelectedIndex(_flex.SelectedIndex);
        }

        // update the dropdown with the new content in the textbox.
        void _cbTextChanged(object sender, TextChangedEventArgs e)
        {
            // not if we're updating
            var updatingText = _updatingText;
            _updatingText = false;
            if (updatingText && !_keyDown)
            {
                return;
            }

            // not if we have no items
            if (_flex.Rows.Count == 0)
            {
                return;
            }

            // allow user to type partial matches when editable
            bool keyDown = _keyDown;
            _keyDown = false;
            if (IsEditable && !keyDown)
            {
                return;
            }

            // find the item that best matches the text
            var len = _cb.Text.Length;
            var index = SearchByText(_cb.Text, true);

            // handle not found cases
            if (index < 0 && !IsEditable)
            {
                len = 0;
                if (_flex.Rows.Count > 0)
                {
                    index = Math.Max(0, _flex.SelectedIndex);
                }
                else
                {
                    SetComboText(string.Empty);
                }
            }

            // update list index
            SetSelectedIndex(index);

            // update selection to show the piece that matched
            if (index > -1)
            {
                _cb.Select(len, _cb.Text.Length - len);
            }
        }

        // handle cursor keys to update dropdown selection and to close the dropdown
        void _cbKeyDown(object sender, KeyEventArgs e)
        {
            // remember we pressed a key when handling the TextChanged event
            if (e.Key != Key.Back && e.Key != Key.Delete)
            {
                _keyDown = true;
            }

            // not if we have no items
            if (_flex.Rows.Count == 0)
            {
                return;
            }

            // handle key
            int index;
            switch (e.Key)
            {
                // select previous item (or wrap back to the last)
                case Key.Up:
                    index =
                        _flex.SelectedIndex < 0 ? 0 :
                        _flex.SelectedIndex > 0 ? _flex.SelectedIndex - 1 :
                        _flex.Rows.Count - 1;
                    SetSelectedIndex(index);
                    e.Handled = true;
                    break;

                // select next item (or wrap back to the first, or show dropdown)
                case Key.Down:
                    var ctrl = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
                    var shft = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
                    if (!IsDroppedDown && (ctrl || shft))
                    {
                        // show drop down
                        IsDroppedDown = true;
                    }
                    else
                    {
                        // select next item (with wrap-around)
                        index = _flex.SelectedIndex;
                        SetSelectedIndex(index < _flex.Rows.Count - 1 ? index + 1 : 0);
                    }
                    e.Handled = true;
                    break;

                // close dropdown
                case Key.Escape:
                case Key.Enter:
                    _cb.Select(int.MaxValue, 0);
                    if (IsDroppedDown)
                    {
                        IsDroppedDown = false;
                        e.Handled = true;
                    }
                    break;

                // close dropdown
                case Key.Tab:
                    IsDroppedDown = false;
                    break;
            }
        }

        // close the dropdown when losing focus
        void _cbLostFocus(object sender, EventArgs e)
        {
            if (IsDroppedDown)
            {
                var f = FocusManager.GetFocusedElement(Application.Current.MainWindow);
                if (f != _cb && f != _flex)
                {
                    IsDroppedDown = false;
                }
            }
        }

        // close the dropdown when the parent form is resized
        void DropDownProvider_SizeChanged(object sender, EventArgs e)
        {
            CloseDropDown();
        }

        #endregion

        //-----------------------------------------------------------------
        #region ** implementation

        // show the drop-down
        void ShowDropDown()
        {
            // can't show if there are no items
            if (_flex.Rows.Count == 0)
            {
                return;
            }

            // get root to show popup
            var root = Util.GetParentOfType<FrameworkElement>(_cb);
            if (root != null)
            {
                // create popup
                if (_popup == null)
                {
                    var bdr = new Border();
                    bdr.Background = _brWhite;
                    bdr.BorderThickness = _thkSingle;
                    bdr.BorderBrush = _brGrey;
                    bdr.Child = _flex;
                    bdr.Effect = new System.Windows.Media.Effects.DropShadowEffect();
                    _popup = new Popup();
                    _popup.Child = bdr;
                    root.SizeChanged += DropDownProvider_SizeChanged;
#if !SILVERLIGHT
                    // close popup when the main window changes or is deactivated
                    var w = (Application.Current != null) ? Application.Current.MainWindow : root as Window;
                    if (w != null)
                    {
                        w.SizeChanged += DropDownProvider_SizeChanged;
                        w.LocationChanged += DropDownProvider_SizeChanged;
                    }
#endif
                }

                // configure grid
                UpdateGridLayout();

                // position popup
                try
                {
                    Util.PositionPopup(_popup, _cb);
                }
                catch
                {
                    return; // TransformToVisual failed, return without opening it...
                }

                // show popup
                _popup.IsOpen = true;

                // freeze drop-down width
                _flex.UpdateLayout();
                _flex.Width = Math.Max(_cb.ActualWidth, _flex.ActualWidth);

                // reposition popup above the control if necessary (after layout)
                Util.PositionPopup(_popup, _cb);

                // scroll selection into view
                if (_cb.SelectedIndex > -1)
                {
                    _flex.ScrollIntoView(_cb.SelectedIndex, 0);
                }
            }
        }

        // close the dropdown without making a selection
        void CloseDropDown()
        {
            if (_popup != null && _popup.IsOpen)
            {
                _popup.IsOpen = false;
                _cb.Focus();
            }
        }

        // update the drop down grid's layout
        void UpdateGridLayout()
        {
            // set default properties
            _flex.IsSynchronizedWithCurrentItem = false;
            _flex.SelectionMode = C1.WPF.FlexGrid.SelectionMode.Row;
            _flex.AutoGenerateColumns = false;
            _flex.RowBackground = null;
            _flex.AlternatingRowBackground = null;
            _flex.KeepCurrentVisible = true;
            _flex.IsTabStop = false;
            _flex.IsReadOnly = true;
            _flex.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

            // copy properties from parent combo
            _flex.SelectedIndex = _cb.SelectedIndex;
            _flex.FontFamily = _cb.FontFamily;
            _flex.FontSize = _cb.FontSize;
            _flex.MinWidth = _cb.ActualWidth;
            _flex.MaxHeight = MaxDropDownHeight;
            _flex.Width = double.NaN;
            _flex.Foreground = _cb.Foreground;
            if (_cb.Background != null)
            {
                _flex.Background = _cb.Background;
            }

            // allow user to customize
            var ddg = _cb.DropDownGrid;
            if (ddg != null)
            {
                // copy columns from parent
                _flex.Columns.Clear();
                foreach (var c in ddg.Columns)
                {
                    _flex.Columns.Add(c);
                }

                var bf = BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public;
                foreach (var dppi in typeof(C1FlexGrid).GetFields(bf))
                {
                    var dp = dppi.GetValue(null) as DependencyProperty;
                    if (dp != null)
                    {
                        var value = ddg.ReadLocalValue(dp);
                        if (value != DependencyProperty.UnsetValue)
                        {
                            _flex.SetValue(dp, value);
                        }
                    }
                }
            }
            else if (_flex.ItemsSource != null && !string.IsNullOrEmpty(_cb.DisplayMemberPath))
            {
                // show a single column
                _flex.Columns.Clear();
                var c = new Column();
                c.Width = new GridLength(1, GridUnitType.Star);
                c.Header = _cb.DisplayMemberPath;
                var b = new System.Windows.Data.Binding(_cb.DisplayMemberPath);
                c.Binding = b;
                _flex.Columns.Add(c);
                _flex.HeadersVisibility = HeadersVisibility.None;
            }
        }

        // search for an item by text
        int SearchByText(string text, bool partial)
        {
            // wrap-around lookup
            int start = Math.Max(0, _flex.SelectedIndex);
            for (int i = 0; i < _flex.Rows.Count; i++)
            {
                int index = (start + i) % _flex.Rows.Count;
                var item = GetItemDisplayText(index);
                if (partial)
                {
                    if (item.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                    {
                        return index;
                    }
                }
                else
                {
                    if (item.Equals(text, StringComparison.OrdinalIgnoreCase))
                    {
                        return index;
                    }
                }
            }

            // not found
            return -1;
        }

        // search for an item by value
        int SearchByValue(object value)
        {
            int start = Math.Max(0, _flex.SelectedIndex);
            for (int i = 0; i < _flex.Rows.Count; i++)
            {
                // get item value
                int index = (start + i) % _flex.Rows.Count;
                var itemValue = GetItemValue(index);

                // change search value type if necessary
                if (itemValue != null && value != null && itemValue.GetType() != value.GetType())
                {
                    try
                    {
                        value = Convert.ChangeType(value, itemValue.GetType(), null);
                    }
                    catch
                    {
                        return -1;
                    }
                }

                // check for match
                if (object.Equals(itemValue, value))
                {
                    return index;
                }
            }

            // not found
            return -1;
        }

        // gets the value for an item on the list
        object GetItemValue(int index)
        {
            object item = null;
            if (index > -1 && index < _flex.Rows.Count)
            {
                item = _flex.Rows[index].DataItem;
                if (!string.IsNullOrEmpty(_cb.SelectedValuePath))
                {
                    var pi = item.GetType().GetProperty(_cb.SelectedValuePath);
                    if (pi != null)
                    {
                        item = pi.GetValue(item, null);
                    }
                }
            }
            return item;
        }

        // gets the text for an item on the list
        string GetItemDisplayText(int index)
        {
            object item = null;
            if (index > -1)
            {
                item = _flex.Rows[index].DataItem;
                if (!string.IsNullOrEmpty(_cb.DisplayMemberPath))
                {
                    var pi = item.GetType().GetProperty(_cb.DisplayMemberPath);
                    if (pi != null)
                    {
                        item = pi.GetValue(item, null);
                    }
                }
            }
            return item != null ? item.ToString() : null;
        }

        // update text in main control to match list selection
        void UpdateComboText()
        {
            var text = GetItemDisplayText(_flex.SelectedIndex);
            if (text != null)
            {
                SetComboText(text);
            }
        }

        // update text in main control
        void SetComboText(string text)
        {
            if (_cb.Text != text)
            {
                _updatingText = true;
                _cb.Text = text;
                _cb.SelectAll();
            }
        }

        #endregion
    }
}
