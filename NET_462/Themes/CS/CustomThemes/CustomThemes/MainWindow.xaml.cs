using C1.WPF.Theming;
using C1.WPF.Theming.ExpressionDark;
using MyCustomTheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomThemes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            C1Theme.LoadingError += new EventHandler<C1ThemeLoadingErrorEventArgs>(C1Theme_LoadingError);
        }

        void C1Theme_LoadingError(object sender, C1ThemeLoadingErrorEventArgs e)
        {
            errors.ItemsSource = e.Errors;
        }

        private void btnED_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme((ToggleButton)sender, new C1ThemeExpressionDark());
        }

        private void btnCusomTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme((ToggleButton)sender, new MyCustomTheme.MyCustomTheme());
        }

        private void btnExtendedED_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme((ToggleButton)sender, new MyCustomExpressionDark());
        }

        ToggleButton _lastSelected;
        private void ApplyTheme(ToggleButton button, C1Theme theme)
        {
            if (_lastSelected != null)
            {
                _lastSelected.IsChecked = false;
            }
            _lastSelected = button;

            theme.Apply(LayoutRoot);
            LayoutRoot.Background = (Brush)C1Theme.GetCurrentThemeResources(theme)["MouseOverGradientBrush"];
        }
    }
}
