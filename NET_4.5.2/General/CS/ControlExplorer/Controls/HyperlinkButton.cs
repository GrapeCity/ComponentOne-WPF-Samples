using C1.WPF;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace ControlExplorer
{
    public class HyperlinkButton : Button
    {
        private Hyperlink _elementHyperlink;
        internal const string HyperlinkElement = "Hyperlink";

        static HyperlinkButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HyperlinkButton),
                new FrameworkPropertyMetadata(typeof(HyperlinkButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _elementHyperlink = GetTemplateChild(HyperlinkElement) as Hyperlink;
            _elementHyperlink.SetBinding(Hyperlink.NavigateUriProperty, new Binding("NavigateUri") { Source = this });
            if (_elementHyperlink != null)
            {
                _elementHyperlink.RequestNavigate += _elementHyperlink_RequestNavigate;
                _elementHyperlink.Click += (s, e) => 
                {
                    base.OnClick();
                    e.Handled = true;
                };
            }
        }

        private void _elementHyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var frame = Application.Current.MainWindow as ControlExplorer.Frame;
            if (frame != null)
            {
                frame.Navigate(e.Uri, frame.NavigationFrame.CurrentSource);
            }
            e.Handled = true;
        }

        public Uri NavigateUri
        {
            get { return (Uri)GetValue(NavigateUriProperty); }
            set { SetValue(NavigateUriProperty, value); }
        }

        public static readonly DependencyProperty NavigateUriProperty =
            DependencyProperty.Register("NavigateUri", typeof(Uri), typeof(HyperlinkButton),
            new UIPropertyMetadata(null));
    }
}
