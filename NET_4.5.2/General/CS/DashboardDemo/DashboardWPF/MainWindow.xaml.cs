using DashboardModel;
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
using System.IO;
using C1.WPF;
using DashboardWPF.Strings;

namespace DashboardWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isOpen = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DataService.GetService().InitializeDataService();
            Navigate(new Uri("Views/Dashboard.xaml", UriKind.Relative));
        }

        private void HamburgerMenu_Click(object sender, RoutedEventArgs e)
        {
            SplitPane.Width = isOpen ? 54 : 200;
            MenuList.ItemTemplate = isOpen ? FindResource("NormalListView") as DataTemplate : FindResource("OpenListView") as DataTemplate;
            MenuList.UpdateLayout();

            isOpen = !isOpen;
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Navigate(new Uri("Views/"+ (e.AddedItems[0] as HamburgerItem).Display + ".xaml", UriKind.Relative));
        }

        void Navigate(Uri targetUri)
        {
            try
            {
                contentFrame.Navigate(targetUri);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("** " + e.ToString());
                System.Diagnostics.Debug.WriteLine("** '" + targetUri.OriginalString + "'");
                ShowNavigationError(targetUri);
            }
        }

        void ShowNavigationError(Uri targetUri)
        {
            C1MessageBox.Show(targetUri.OriginalString + "\r\n" + new System.IO.FileNotFoundException().Message,
               "Navigation Error", C1MessageBoxIcon.Warning,
               new Action<MessageBoxResult>((r) =>
               {
               }));
        }
    }

    class HamburgerItems : List<HamburgerItem>
    {
        public HamburgerItems()
        {
            Add(new HamburgerItem() { Display = Resource.Dashboard_Title, Source = new BitmapImage(new Uri("pack://application:,,,/DashboardWPF;component/Assets/24X24_Dashboard.png")) });
            Add(new HamburgerItem() { Display = Resource.AnalysisPage_Title, Source = new BitmapImage(new Uri("pack://application:,,,/DashboardWPF;component/Assets/24X24_Analysis.png")) });
            Add(new HamburgerItem() { Display = Resource.Reporting_Title, Source = new BitmapImage(new Uri("pack://application:,,,/DashboardWPF;component/Assets/24X24_Reporting.png")) });
            Add(new HamburgerItem() { Display = Resource.TaskPage_Title, Source = new BitmapImage(new Uri("pack://application:,,,/DashboardWPF;component/Assets/24X24_Tasks.png")) });
            Add(new HamburgerItem() { Display = Resource.ProductsPage_Title, Source = new BitmapImage(new Uri("pack://application:,,,/DashboardWPF;component/Assets/24X24_Products.png")) });
        }
    }

    class HamburgerItem
    {
        public ImageSource Source { get; set; }

        public string Display { get; set; }
    }
}
