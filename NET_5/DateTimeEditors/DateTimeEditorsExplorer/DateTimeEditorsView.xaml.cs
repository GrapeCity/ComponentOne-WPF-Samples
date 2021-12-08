using System.Windows.Controls;

namespace DateTimeEditorsExplorer
{
    /// <summary>
    /// Interaction logic for DateTimeEditorsView.xaml
    /// </summary>
    public partial class DateTimeEditorsView : UserControl
    {
        public DateTimeEditorsView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += DateTimeEditorsView_Loaded;
        }

        private void DateTimeEditorsView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
