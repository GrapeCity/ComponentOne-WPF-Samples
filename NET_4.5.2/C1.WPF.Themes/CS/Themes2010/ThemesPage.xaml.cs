using C1.WPF;
using C1.WPF.Theming;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Themes
{
    /// <summary>
    /// Interaction logic for DemoThemes.xaml
    /// </summary>
    public partial class DemoThemes : UserControl
    {
        public DemoThemes()
        {
            InitializeComponent();

            cmbTheme.ItemsSource = typeof(C1AvailableThemes).GetEnumValues<C1AvailableThemes>();
            cmbTheme.SelectedItem = C1AvailableThemes.Material;
        }

        private void cmbTheme_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            Theme theme = null;
            if(cmbTheme.SelectedItem != null)
            {
                theme = C1ThemeFactory.GetTheme((C1AvailableThemes)cmbTheme.SelectedItem);
            }
            
            C1Theme.ApplyTheme(LayoutRoot, theme);

            var adornerLayer = AdornerLayer.GetAdornerLayer(LayoutRoot);
            if (adornerLayer != null)
            {
                // this will apply theme to everything displayed in adorner, including any C1Window instances
                C1Theme.ApplyTheme(adornerLayer, theme); 
            }

            gallery.CurrentTheme = theme;
        }
    }
}
