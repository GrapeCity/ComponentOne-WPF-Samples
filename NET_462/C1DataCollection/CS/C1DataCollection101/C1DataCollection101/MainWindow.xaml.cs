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

namespace C1DataCollection101
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }

        public void Navigate(Uri targetUri)
        {
            try
            {
                NavigationFrame.Navigate(targetUri);
                HomeButton.Visibility = Visibility.Visible;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("** " + e.ToString());
                System.Diagnostics.Debug.WriteLine("** '" + targetUri.OriginalString + "'");
                ShowNavigationError(targetUri);
            }
        }

        public void UpdateCaption (string s)
        {
            Caption.Text = s;
        }

        public void ShowNavigationError(Uri targetUri)
        {
            MessageBox.Show(targetUri.OriginalString + "\r\n" + new System.IO.FileNotFoundException().Message,
               "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new Uri("/View/DataCollectionSamples.xaml", UriKind.Relative));
            Caption.Text = "C1DataCollection101";
            HomeButton.Visibility = Visibility.Collapsed;
        }
    }
}
