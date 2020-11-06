using C1.WPF.Chart;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DrillDown
{
    /// <summary>
    /// Interaction logic for SunburstDemo.xaml
    /// </summary>
    public partial class SunburstDemo : UserControl
    {
        private DrillDownViewModel _vm;

        public SunburstDemo()
        {
            InitializeComponent();
            _vm = new DrillDownViewModel();
            this.DataContext = _vm;
            
        }
        
    }
}
