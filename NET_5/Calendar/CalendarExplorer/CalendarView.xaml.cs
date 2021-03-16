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
        }
    }
}