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
            this.Loaded += VirtualMode_Loaded;
        }

        private async void VirtualMode_Loaded(object sender, RoutedEventArgs e)
        {
            var persons = new VirtualModeDataCollection();
            listView.ItemsSource = persons;
            await persons.LoadAsync(0, 0);
        }
    }
}
