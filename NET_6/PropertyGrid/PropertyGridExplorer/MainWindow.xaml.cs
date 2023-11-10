using PropertyGridExplorer.Resources;
using System.Windows;

namespace PropertyGridExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = AppResources.Title;
        }
    }
}
