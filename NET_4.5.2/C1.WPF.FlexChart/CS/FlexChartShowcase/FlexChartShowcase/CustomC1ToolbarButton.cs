using C1.WPF.Toolbar;
using System.Windows;
using System.Windows.Media;

namespace FlexChartShowcase
{
    public class CustomC1ToolbarButton : C1ToolbarButton
    {
        public CustomC1ToolbarButton()
        {
            Background = Brushes.Transparent;
            Padding = new Thickness(0);
            ShowHighlight = Visibility.Collapsed;
        }

        public static readonly DependencyProperty ShowHighlightProperty = DependencyProperty.Register(
        "ShowHighlight",
        typeof(Visibility),
        typeof(CustomC1ToolbarButton),
        new FrameworkPropertyMetadata(null)
        );
        public Visibility ShowHighlight
        {
            get { return (Visibility)GetValue(ShowHighlightProperty); }
            set
            {
                SetValue(ShowHighlightProperty, value);
                if (Visibility == Visibility.Visible)
                {
                    //Background = Brushes.LightSkyBlue;
                }
                else
                {
                    Background = Brushes.Transparent;
                }
            }
        }
    }
}
