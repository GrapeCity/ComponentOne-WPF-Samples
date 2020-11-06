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

namespace TutorialsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void showSimpleBinding(object sender, RoutedEventArgs e)
        {
            new SimpleBinding().Show();
        }

        private void showMasterDetailBinding(object sender, RoutedEventArgs e)
        {
            new MasterDetailBinding().Show();
        }

        private void showServerSideFiltering(object sender, RoutedEventArgs e)
        {
            new ServerSideFiltering().Show();
        }

        private void showPaging(object sender, RoutedEventArgs e)
        {
            new Paging().Show();
        }

        private void showLargeDataSet(object sender, RoutedEventArgs e)
        {
            new LargeDataset().Show();
        }

        private void showCustomColumns(object sender, RoutedEventArgs e)
        {
            new CustomColumns().Show();
        }

        private void showDataSourcesInCode(object sender, RoutedEventArgs e)
        {
            new DataSourcesInCode().Show();
        }

        private void showClientSideQuerying(object sender, RoutedEventArgs e)
        {
            new ClientSideQuerying().Show();
        }

        private void showCategoryProductsView(object sender, RoutedEventArgs e)
        {
            new CategoryProductsView().Show();
        }
    }
}
