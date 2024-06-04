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

namespace TraditionalIndexing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // create some data
            var list = new List<Customer>();
            for (int i = 0; i < 200; i++)
                list.Add(new Customer(i));

            // create a flexgrid with traditional indexing
            var flex = new FlexGridTraditionalIndexing();
            LayoutRoot.Children.Add(flex);

            // set number of rows and columns
            flex.Rows.Fixed = 3;
            flex.Columns.Fixed = 3;

            // bind it to our data
            flex.ItemsSource = list;
        }
    }
}
