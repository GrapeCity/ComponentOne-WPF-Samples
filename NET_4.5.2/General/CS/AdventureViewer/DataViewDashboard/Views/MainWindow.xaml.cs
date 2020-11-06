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
using C1.WPF.Theming;

namespace DataViewDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var theme = new C1.WPF.Theming.Cosmopolitan.C1ThemeCosmopolitan();
            var ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonCosmopolitan();
            
            ribbonTheme.Apply(this);
            root.Style = TryFindResource(typeof(Window)) as Style;
            this.Resources.MergedDictionaries.Add(C1Theme.GetCurrentThemeResources(theme));
            Application.Current.Resources.MergedDictionaries.Add(theme.GetNewResourceDictionary());
            Application.Current.Resources.MergedDictionaries.Add(ribbonTheme.GetNewResourceDictionary());
        }
    }
}
