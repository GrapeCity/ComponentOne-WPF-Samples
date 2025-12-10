using Localization.ViewModel;
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

namespace Localization.Views.TabViews
{
    /// <summary>
    /// Interaction logic for SchedularTab.xaml
    /// </summary>
    public partial class SchedularTab : UserControl
    {
        public SchedularTab()
        {
            InitializeComponent();
        }

        #region Event Handler
        private void Grouping_Checked(object sender, RoutedEventArgs e)
        {
            sched1.GroupBy = "Category";
        }

        private void Grouping_Unchecked(object sender, RoutedEventArgs e)
        {
            sched1.GroupBy = string.Empty;
        }
        #endregion
    }
}
