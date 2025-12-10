using System.Windows.Controls;

namespace SchedulerExplorer
{
    /// <summary>
    ///     Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class SchedulerView : UserControl
    {
        public SchedulerView()
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