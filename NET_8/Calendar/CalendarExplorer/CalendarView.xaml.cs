using System.Windows.Controls;

namespace CalendarExplorer
{
    /// <summary>
    ///     Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
        public CalendarView()
        {
            InitializeComponent();
            DataContext = new SampleDataSource();
            Loaded += CalendarView_Loaded;
        }

        private void CalendarView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}