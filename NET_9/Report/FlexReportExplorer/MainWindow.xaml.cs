using FlexReportExplorer.Resources;
using System.Windows;

namespace FlexReportExplorer
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
