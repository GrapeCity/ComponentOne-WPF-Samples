using C1.DataCollection;
using ListViewExplorer.Resources;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class Filtering : UserControl
    {
        string currentFilterValue;
        System.Threading.SemaphoreSlim filterSemaphore;

        public Filtering()
        {
            InitializeComponent();
            Tag = AppResources.FilterDescription;

            filterSemaphore = new System.Threading.SemaphoreSlim(1);
            txtFilter.Text = "";
            listView.ItemsSource = Person.Generate(100);
        }

        private async void C1TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var filter = txtFilter.Text;
                currentFilterValue = filter;
                await Task.Delay(400);
                await filterSemaphore.WaitAsync();
                if (currentFilterValue == filter)
                {
                    await listView.DataCollection.FilterAsync("Name", FilterOperation.Contains, filter);
                }
            }
            finally
            {
                filterSemaphore.Release();
            }
        }
    }
}
