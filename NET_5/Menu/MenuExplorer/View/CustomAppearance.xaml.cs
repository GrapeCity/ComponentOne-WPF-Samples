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
using C1.WPF.Core;
using C1.WPF.Menu;

namespace MenuExplorer
{
    /// <summary>
    /// Interaction logic for DemoMenu.xaml
    /// </summary>
    public partial class CustomAppearance : UserControl
    {
        public CustomAppearance()
        {
            InitializeComponent();
            Tag = "This demo shows how to apply a custom style to the C1Menu control.";
        }

        private void C1MenuItem_Click(object sender, SourcedEventArgs e)
        {
            C1MenuItem menu = sender as C1MenuItem;
        }
    }
}
