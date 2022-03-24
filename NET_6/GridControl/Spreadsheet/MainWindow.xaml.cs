using System.Windows;

namespace Spreadsheet
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = Properties.Resources.SampleTitle;
            Tag = Properties.Resources.Tag;
        }
    }
}
