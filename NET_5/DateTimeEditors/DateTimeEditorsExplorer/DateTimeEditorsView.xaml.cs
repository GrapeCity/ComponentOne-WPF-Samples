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
        }
    }
}
