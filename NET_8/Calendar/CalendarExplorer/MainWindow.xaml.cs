using System.Windows;

namespace CalendarExplorer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = CalendarExplorer.Resources.AppResources.Title;
        }
    }
}