using ListViewExplorer.Resources;
using System.Windows;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class VirtualMode : UserControl
    {
        public VirtualMode()
        {
            InitializeComponent();
            Tag = AppResources.VirtualModeDescription;

            var persons = new VirtualModeDataCollection();
            listView.ItemsSource = persons;
        }
    }
}
