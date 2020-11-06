using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using C1.WPF;

namespace ControlExplorer
{
    public class SearchControl : Control
    {
        private C1DropDown _elementDropDown;
        private C1TreeView _elementTreeView;

        static SearchControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchControl),
                new FrameworkPropertyMetadata(typeof(SearchControl)));
        }

        public SearchControl()
        {
            DataContext = SearchViewModel.Instance;
        }

        public Style ScrollViewerStyle
        {
            get { return (Style)GetValue(ScrollViewerStyleProperty); }
            set { SetValue(ScrollViewerStyleProperty, value); }
        }

        public static readonly DependencyProperty ScrollViewerStyleProperty =
            DependencyProperty.Register("ScrollViewerStyle", typeof(Style), typeof(SearchControl),
            new PropertyMetadata(null));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(SearchControl),
            new PropertyMetadata(string.Empty));

        private C1DropDown DropDown
        {
            get
            {
                if (_elementDropDown == null)
                {
                    _elementDropDown = GetTemplateChild("DropDown") as C1DropDown;
                }
                return _elementDropDown;
            }
        }

        private C1TreeView TreeView
        {
            get
            {
                if (_elementTreeView == null)
                {
                    _elementTreeView = GetTemplateChild("TreeView") as C1TreeView;
                }
                return _elementTreeView;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DropDown != null)
            {
                _elementDropDown.KeyDown += new KeyEventHandler(_elementDropDown_KeyDown);
            }

            if (TreeView != null)
            {
                _elementTreeView.ApplyTemplate();
                if (ScrollViewerStyle != null)
                {
                    IList<DependencyObject> list = new List<DependencyObject>();
                    VTreeHelper.GetChildrenOfType(_elementTreeView, typeof(ScrollViewer), ref list);
                    foreach (ScrollViewer sv in list)
                    {
                        sv.Style = ScrollViewerStyle;
                    }
                }
            }
        }

        void _elementDropDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                SearchViewModel.Instance.ShowSearchList = true;
                if (TreeView != null)
                {
                    _elementTreeView.Focus();
                    bool hasFocus = false;
                    foreach (var item in _elementTreeView.Items)
                    {
                        var treeItem = _elementTreeView.ItemContainerGenerator.ContainerFromItem(item) as C1TreeViewItem;
                        if (treeItem.Visibility == Visibility.Visible )
                        {
                            if ( !hasFocus )
                            {
                                treeItem.Focus();
                                hasFocus = true;
                            }
                            if (treeItem.IsSelected)
                            {
                                treeItem.Focus();
                                break;
                            }
                        }
                    }
                    e.Handled = true;
                }
            }
        }
    }
}
