using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Excel-style value filter editor.
    /// </summary>
    /// <remarks>
    /// <para>This control inherits from the standard <see cref="ColumnFilterEditor"/>
    /// to modify the control appearance and to add the following functionality:</para>
    /// <list>
    /// <item>Filtered value list (users may show only a subset of the values present
    /// on the columns to filter on).</item>
    /// <item>Column sorting commands (ascending, descending, clear).</item>
    /// <item>Resizing (users can drag the bottom right corner of the editor to adjust
    /// its size.</item>
    /// </list>
    /// </remarks>
    public class ExcelFilterEditor : ColumnFilterEditor
    {
        //---------------------------------------------------------------------------
        #region ** fields

        ListBox _lbValues;
        TextBox _txtValueFilter;
        Image _imgFilter;
        Image _imgClearFilter;
        Storyboard _sbFilter;
        Column _currentColumn;

        Point _ptDown;
        Size _szDown;
        bool _resizing;

        #endregion

        //---------------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of an <see cref="ExcelFilterEditor"/>.
        /// </summary>
        public ExcelFilterEditor()
        {
            this.DefaultStyleKey = typeof(ExcelFilterEditor);
            Background = new SolidColorBrush(Colors.White);

            // filterbox storyboard
            _sbFilter = new Storyboard();
            _sbFilter.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            _sbFilter.Completed += (s, e) => { FilterListBox(); };
        }

        #endregion

        //---------------------------------------------------------------------------
        #region ** overrides

        /// <summary>
        /// Build the control layout.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // connect sort button handlers
            var btn = GetTemplateChild("_btnSortAsc") as /*Hyperlink*/Button;
            btn.Click += btnSortAsc_Click;
            btn = GetTemplateChild("_btnSortDesc") as /*Hyperlink*/Button;
            btn.Click += btnSortDesc_Click;

            // connect value filter
            _lbValues = GetTemplateChild("_lbValues") as ListBox;
            _txtValueFilter = GetTemplateChild("_txtValueFilter") as TextBox;
            _imgFilter = GetTemplateChild("_imgFilter") as Image;
            _imgClearFilter = GetTemplateChild("_imgClearFilter") as Image;
            _txtValueFilter.TextChanged += txtValueFilter_TextChanged;
            _imgClearFilter.MouseLeftButtonDown += _imgClearFilter_MouseLeftButtonDown;

            // connect resizing handle
            var imgSz = GetTemplateChild("_imgResizer") as Image;
            imgSz.MouseLeftButtonDown += sz_MouseLeftButtonDown;
            imgSz.MouseLeftButtonUp += sz_MouseLeftButtonUp;
            imgSz.LostMouseCapture += imgSz_LostMouseCapture;
            imgSz.MouseMove += imgSz_MouseMove;
        }
        /// <summary>
        /// Overridden to initialize value list filter.
        /// </summary>
        protected override void InitializeUI()
        {
            if (Filter.Column != _currentColumn)
            {
                _currentColumn = Filter.Column;
                if (_txtValueFilter != null)
                    _txtValueFilter.Text = string.Empty;
            }

            base.InitializeUI();
        }
        /// <summary>
        /// Overridden to honor the value list filter.
        /// </summary>
        protected override void AddValueCheckBox(object content, bool isChecked)
        {
            var filter = _txtValueFilter != null ? _txtValueFilter.Text : null;
            var value = content != null ? content.ToString() : string.Empty;
            if (string.IsNullOrEmpty(filter) || value.IndexOf(filter, StringComparison.OrdinalIgnoreCase) > -1)
            {
                base.AddValueCheckBox(content, isChecked);
            }
        }
      
        #endregion

        //---------------------------------------------------------------------------
        #region ** value filter

        // update value list filter
        void txtValueFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            // stop timer if it is on
            _sbFilter.Stop();
            _sbFilter.Seek(TimeSpan.Zero);

            // update search icon visibility
            if (string.IsNullOrEmpty(_txtValueFilter.Text))
            {
                _imgFilter.Visibility = Visibility.Visible;
                _imgClearFilter.Visibility = Visibility.Collapsed;
                FilterListBox();
            }
            else
            {
                _imgFilter.Visibility = Visibility.Collapsed;
                _imgClearFilter.Visibility = Visibility.Visible;
                _sbFilter.Begin();
            }
        }
        void FilterListBox()
        {
            UpdateValueList();
        }
        void _imgClearFilter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _txtValueFilter.Text = string.Empty;
        }

        #endregion

        //---------------------------------------------------------------------------
        #region ** resizing editor

        void sz_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _ptDown = e.GetPosition(null);
            _szDown = this.RenderSize;
            var img = sender as Image;
            img.CaptureMouse();
            _resizing = true;
        }
        void sz_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var img = sender as Image;
            img.ReleaseMouseCapture();
            _resizing = false;
        }
        void imgSz_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _resizing = false;
        }
        void imgSz_MouseMove(object sender, MouseEventArgs e)
        {
            if (_resizing)
            {
                var pt = e.GetPosition(null);
                Width = _szDown.Width + (pt.X - _ptDown.X);
                Height = _szDown.Height + (pt.Y - _ptDown.Y);
            }
        }

        #endregion

        //---------------------------------------------------------------------------
        #region ** sort commands

        void btnSortAsc_Click(object sender, EventArgs e)
        {
            OnCloseEditor(false);
            SortGrid(ListSortDirection.Ascending);
        }
        void btnSortDesc_Click(object sender, EventArgs e)
        {
            OnCloseEditor(false);
            SortGrid(ListSortDirection.Descending);
        }
        void SortGrid(ListSortDirection dir)
        {
            // get parameters
            var flex = Filter.Column.Grid as C1FlexGridBook;
            var sdc = new List<UnboundSortDescription>();
            sdc.Add(new UnboundSortDescription(Filter.Column, dir));

            // apply sort
            var sa = new SortAction(flex);
            UnboundSort.SortUnboundGrid(flex, sdc);
            sa.SaveNewState();
            flex.UndoStack.AddAction(sa);
        }

        #endregion
    }
}
