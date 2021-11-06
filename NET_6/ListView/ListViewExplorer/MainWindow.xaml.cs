using ListViewExplorer.Resources;
using System.Windows;

namespace ListViewExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = AppResources.MainTitle;
        }
    }
}
