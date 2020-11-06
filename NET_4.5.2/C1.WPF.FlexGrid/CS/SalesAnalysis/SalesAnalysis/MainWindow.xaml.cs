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

namespace SalesAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Home hmpage;
        About abtpage;
        Invoice invpage;
        Chart chart;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                hmpage = new Home();
                abtpage = new About();
                chart = new Chart();
                invpage = new Invoice();
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();
            var cntent = (sender as Button).Content.ToString();
            switch (cntent)
            {
                case "Home":
                    {


                        ContentGrid.Children.Add(hmpage);
                        break;
                    }
                case "About":
                    {


                        ContentGrid.Children.Add(abtpage);
                        break;
                    }
                case "Invoices":
                    {

                        ContentGrid.Children.Add(invpage);
                        break;
                    }
                case "Chart":
                    {
                        ContentGrid.Children.Add(chart);
                        break;
                    }
            }
        }
    }
}
