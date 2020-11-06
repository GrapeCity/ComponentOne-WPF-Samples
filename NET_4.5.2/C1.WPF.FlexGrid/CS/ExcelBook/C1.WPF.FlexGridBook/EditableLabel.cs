using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    public class EditableLabel : Grid
    {
        TextBlock _tb = new TextBlock();
        TextBox _txt = new TextBox();
        DateTime _lastMouseDown;
        static EditableLabel _focused;
        C1FlexGrid _grid;
        bool _isReadOnly;

        public EditableLabel()
        {
            // configure TextBox
            _txt.Visibility = System.Windows.Visibility.Collapsed;
            _txt.Padding = _txt.Margin = new Thickness(0);
            _txt.Background = null;
            _txt.BorderThickness = _txt.Padding;
            _txt.GotFocus += _txt_GotFocus;
            _txt.LostFocus += _txt_LostFocus;
            _txt.KeyDown += _txt_KeyDown;
            _txt.TextChanged += _txt_TextChanged;
            _txt.MaxLength = 31;

            // configure TextBlock
            _tb.MouseLeftButtonDown += _tb_MouseLeftButtonDown;

            // add children
            Children.Add(_tb);
            Children.Add(_txt);

            // start out non-editable
            SetEditable(false);
        }

        // gets or sets the text in the label
        public string Text
        {
            get { return _tb.Text; }
            set 
            {
                if (value != _tb.Text)
                {
                    _tb.Text = _txt.Text = value;
                    OnTextChanged(EventArgs.Empty);
                }
            }
        }

        // start editing this label
        public void StartEditing()
        {
            SetEditable(true);
        }

        // fire event when editable label text is committed
        public event EventHandler TextChanged;
        protected virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }

        // prevent user from entering invalid characters into textbox
        void _txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = _txt.Text;
            foreach (var ch in @":\/?*[]")
            {
                text = text.Replace(ch.ToString(), string.Empty);
            }
            if (text != _txt.Text)
            {
                var start = _txt.SelectionStart;
                _txt.Text = text;
                _txt.SelectionStart = Math.Max(0, start - 1);
            }
        }

        // keep track of focused element
        void _txt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_focused != null && _focused != this)
            {
                _focused.SetEditable(false);
            }
            _focused = _txt.Parent as EditableLabel;
        }

        // stop editing when textbox loses focus
        void _txt_LostFocus(object sender, RoutedEventArgs e)
        {
            SetEditable(false);
        }

        // stop editing when user presses Enter or Escape
        void _txt_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    e.Handled = true;
                    SetEditable(false);
                    break;
                case Key.Escape:
                    e.Handled = true;
                    _txt.Text = _tb.Text;
                    SetEditable(false);
                    break;
            }
        }

        // handle clicks and double-clicks
        void _tb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // remove focus from any other editable labels
            if (_focused != null && _focused != this)
            {
                _focused.SetEditable(false);
                _focused = null;
            }

            // start editing on double-click
            var now = DateTime.Now;
            if (now.Subtract(_lastMouseDown).TotalMilliseconds < 250)
            {
                e.Handled = true;
                SetEditable(true);
            }
            _lastMouseDown = now;
        }
        void SetEditable(bool editable)
        {
            // prevent the grid from getting the keyboard
            var grid = GetParentGrid();
            if (grid != null)
            {
                grid.IsReadOnly = editable ? true : _isReadOnly;
            }

            // select textbox or textblock
            if (editable)
            {
                _txt.Text = _tb.Text;
                _txt.Visibility = Visibility.Visible;
                _tb.Visibility = Visibility.Collapsed;
                _txt.Focus();
                _txt.SelectAll();
            }
            else
            {
                _tb.Visibility = Visibility.Visible;
                _txt.Visibility = Visibility.Collapsed;
                Text = _txt.Text;
            }
        }
        C1FlexGrid GetParentGrid()
        {
            if (_grid == null)
            {
                for (var p = VisualTreeHelper.GetParent(this); p != null; p = VisualTreeHelper.GetParent(p))
                {
                    var g = p as C1FlexGrid;
                    if (g != null)
                    {
                        _grid = g;
                        _isReadOnly = g.IsReadOnly;
                    }
                }
            }
            return _grid;
        }
    }
}
