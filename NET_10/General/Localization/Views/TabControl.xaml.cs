using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Localization.Views
{
    /// <summary>
    /// Interaction logic for TabControl.xaml
    /// </summary>
    public partial class TabControl : UserControl
    {
        public TabControl()
        {
            InitializeComponent();
        }
        private void C1TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl == null)
                return;
        }
    }
}
