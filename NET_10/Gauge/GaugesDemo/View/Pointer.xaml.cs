using GaugesDemo.Resources;
using System.Windows.Controls;

namespace GaugesDemo
{
    public partial class Pointer : UserControl
    {
        public Pointer()
        {
            InitializeComponent();
            Tag = AppResources.PointerDescription;
        }
    }
}
