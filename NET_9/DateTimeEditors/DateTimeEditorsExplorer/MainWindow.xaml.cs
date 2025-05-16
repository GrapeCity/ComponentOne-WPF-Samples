using System.Windows;

namespace DateTimeEditorsExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = DateTimeEditorsExplorer.Resources.AppResources.Title;
        }
    }
}
