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

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for GroupingPanel.xaml
    /// </summary>
    public partial class GroupingPanel : UserControl
    {
        public GroupingPanel()
        {
            InitializeComponent();

            // create a data source
            var list = new List<Product>();
            for (int i = 0; i < 200; i++)
            {
                list.Add(new Product());
            }

            // assign the data source to grid
            var cvs = new CollectionViewSource() { Source = list };
            _flex.ItemsSource = cvs.View;
        }
    }
}
