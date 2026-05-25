using GaugesDemo.Resources;
using System.Windows.Controls;

namespace GaugesDemo
{
    public partial class MarksAndLabels : UserControl
    {
        public MarksAndLabels()
        {
            InitializeComponent();
            Tag = AppResources.MarksAndLabelsDescription;
        }
    }
}
