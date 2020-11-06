using System.Windows.Controls;

namespace CoreExplorer
{
    /// <summary>
    /// Interaction logic for CoreView.xaml
    /// </summary>
    public partial class CoreView : UserControl
    {
        public CoreView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
        }
    }
}
