using C1.WPF.Chart;
using System.Windows.Input;
using System.Windows.Controls;

namespace DrillDown
{
    /// <summary>
    /// Interaction logic for TreemapDemo.xaml
    /// </summary>
    public partial class TreemapDemo : UserControl
    {
        private DrillDownViewModel _vm;

        public TreemapDemo()
        {
            InitializeComponent();
            _vm = new DrillDownViewModel();
            this.DataContext = _vm;
        }
    }
}
