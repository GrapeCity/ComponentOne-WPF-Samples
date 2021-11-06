using C1.DataCollection;
using ListViewExplorer.Resources;
using System.Windows;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class Grouping : UserControl
    {
        public Grouping()
        {
            InitializeComponent();
            Tag = AppResources.GroupDescription;
            listView.ItemsSource = Person.Generate(100);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await listView.DataCollection.GroupAsync("Born");
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await listView.DataCollection.GroupAsync("Residence");
        }
    }
}
