using System;
using System.Windows;
using System.Windows.Controls;

namespace ControlExplorer
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            DataContext = MainViewModel.Instance;
        }

        private void TileControl_Click(object sender, EventArgs e)
        {
            var link = ((sender as FrameworkElement).DataContext as ControlDescription).Link;
            var frame = Application.Current.MainWindow as ControlExplorer.Frame;
            if (frame != null)
            {
                frame.Navigate(link, frame.NavigationFrame.CurrentSource);
            }
            else
            {
                NavigationService.Navigate(link);
            }
        }
    }
}
