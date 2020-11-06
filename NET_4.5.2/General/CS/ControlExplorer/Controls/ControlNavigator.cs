using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System;
using System.Windows.Documents;

namespace ControlExplorer
{
    public class ControlNavigator : Control
    {
        private Popup _popupElement = null;
        internal const string PopupElementName = "DropDown";
        private ScrollViewer _scrollViewerElement = null;
        internal const string ScrollViewerName = "ScrollViewer";

        static ControlNavigator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlNavigator),
                new FrameworkPropertyMetadata(typeof(ControlNavigator)));
        }

        public ControlNavigator()
        {
            this.Groups = MainViewModel.Instance.Groups.ToList();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ControlNavigator),
            new PropertyMetadata(string.Empty));

        public IList<GroupDescription> Groups
        {
            get { return (IList<GroupDescription>)GetValue(GroupsProperty); }
            set { SetValue(GroupsProperty, value); }
        }

        public static readonly DependencyProperty GroupsProperty =
            DependencyProperty.Register("Groups", typeof(IList<GroupDescription>), typeof(ControlNavigator),
            new PropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popupElement = GetTemplateChild(PopupElementName) as Popup;
            _popupElement.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(_popupElement_MouseLeftButtonUp), true);
            _scrollViewerElement = GetTemplateChild(ScrollViewerName) as ScrollViewer;
        }

        private void _popupElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_popupElement != null)
            {
                if (e.OriginalSource is Hyperlink)
                {
                    _popupElement.IsOpen = false;
                }
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (!SystemParameters.IsMenuDropRightAligned)
            {
                _popupElement.HorizontalOffset = 0;
            }
            else
            {
                _scrollViewerElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var scrollWidth = _scrollViewerElement.DesiredSize.Width;
                _popupElement.HorizontalOffset = scrollWidth;
            }
            if (!_popupElement.IsOpen)
                _popupElement.IsOpen = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (_popupElement != null)
            {
                _popupElement.IsOpen = false;
            }
            base.OnMouseLeave(e);
        }
    }
}
