using C1.WPF.Theming;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF;
using C1.WPF.Theming;

namespace Themes
{
    /// <summary>
    /// Interaction logic for SchedulerThemes.xaml
    /// </summary>
    public partial class SchedulerThemes : UserControl
    {
        private Theme _theme;
        public SchedulerThemes()
        {
            InitializeComponent();
            cmbTheme.ItemsSource = typeof(C1AvailableThemes).GetEnumValues<C1AvailableThemes>();
            cmbTheme.SelectedItem = C1AvailableThemes.Office2016Black;
            ViewType.ItemsSource = Enum.GetValues(typeof(C1.WPF.Schedule.ViewType));
            ViewType.SelectedValue = C1.WPF.Schedule.ViewType.Month;
        }

        public Theme CurrentTheme
        {
            get { return _theme; }
            set
            {
                if (_theme != value)
                {
                    _theme = value;
                    OnThemeChanged();
                }
            }
        }

        private void OnThemeChanged()
        {
            if (CurrentTheme != null)
            {
                var theme = CurrentTheme.GetNewResourceDictionary();
                cal1.Theme = theme;
                sched1.Theme = theme;
            }
        }

        private void Grouping_Checked(object sender, RoutedEventArgs e)
        {
            if (sched1 != null)
            {
                sched1.GroupBy = "Category";
            }
        }

        private void Grouping_Unchecked(object sender, RoutedEventArgs e)
        {
            sched1.GroupBy = "";
        }

        private void cmbTheme_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            var theme = C1ThemeFactory.GetTheme((C1AvailableThemes)cmbTheme.SelectedItem);
            C1Theme.ApplyTheme(LayoutRoot, theme);

            var adornerLayer = AdornerLayer.GetAdornerLayer(LayoutRoot);
            if (adornerLayer != null)
            {
                // this will apply theme to everything displayed in adorner, including any C1Window instances
                C1Theme.ApplyTheme(adornerLayer, theme);
            }

            CurrentTheme = theme;
        }
    }
}
