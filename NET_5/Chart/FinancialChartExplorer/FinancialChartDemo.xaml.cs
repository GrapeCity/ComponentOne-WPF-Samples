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

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for FinancialChartDemo.xaml
    /// </summary>
    public partial class FinancialChartDemo : UserControl
    {
        public FinancialChartDemo()
        {
            InitializeComponent();
        }

        void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var group = e.NewValue as SampleGroup;
            if (group != null)
            {
                var sample = group.Samples.First();
                var item = lbSamples.ItemContainerGenerator.ContainerFromItem(group) as TreeViewItem;
                if (item != null)
                {
                    var child = item.ItemContainerGenerator.ContainerFromItem(sample) as TreeViewItem;
                    if (child != null)
                        child.IsSelected = true;
                }
            }
        }
    }
}
