using System.Windows;
using System.Windows.Controls;
using C1.DataCollection;
using ListViewExplorer.Resources;

namespace ListViewExplorer
{
    public partial class Paging : UserControl
    {
        public Paging()
        {
            InitializeComponent();
            Tag = AppResources.PagingDescription;
            var persons = new C1PagedDataCollection<Person>(new VirtualModeDataCollection() { PageSize = 10 }) { PageSize = 10 };
            pager.Source = persons;
            listView.ItemsSource = persons;
        }
    }    
}
