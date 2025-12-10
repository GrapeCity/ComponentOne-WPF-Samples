using System.Windows;

namespace SchedulerExplorer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = SchedulerExplorer.Resources.AppResources.Title;
        }
    }
}