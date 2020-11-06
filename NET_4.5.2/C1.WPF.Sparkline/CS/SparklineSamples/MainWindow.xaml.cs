using C1.WPF;
using C1.WPF.Sparkline;
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

namespace SparklineSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            C1TabItem item1 = new C1TabItem();
            item1.Header = "See it in action";
            item1.Content = new SparklineSamples.SparklineDemo();

            C1TabItem item2 = new C1TabItem();
            item2.Header = "Appearance";
            item2.Content = new SparklineSamples.AppearanceSample();

            C1TabItem item3 = new C1TabItem();
            item3.Header = "Region Sales";
            item3.Content = new SparklineSamples.RegionSales();

            C1TabItem item4 = new C1TabItem();
            item4.Header = "FlexGrid Integration";
            item4.Content = new SparklineSamples.FlexGridIntegration();

            tab.Items.Add(item1);
            tab.Items.Add(item2);
            tab.Items.Add(item3);
            tab.Items.Add(item4);
        }
    }
}
