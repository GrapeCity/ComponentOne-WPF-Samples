using System.Windows;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();
            this.Title = InputExplorer.Properties.Resources.Title;
        }
    }
}
