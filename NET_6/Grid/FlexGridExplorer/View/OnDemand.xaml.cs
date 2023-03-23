using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public partial class OnDemand : UserControl
    {
        YouTubeCollectionView _dataCollection;

        public OnDemand()
        {
            InitializeComponent();
            Tag = AppResources.OnDemandDescription;
            //search.Watermark = AppResources.SearchPlaceholderText;
            emptyListLabel.Text = AppResources.EmptyListText;

            _dataCollection = new YouTubeCollectionView() { PageSize = 50 };
            grid.ItemsSource = _dataCollection;
            search.Text = ".Net7";
            var task = PerformSearch();
        }

        private async Task PerformSearch()
        {
            try
            {
                activityIndicator.Visibility = Visibility.Visible;
                await _dataCollection.SearchAsync(search.Text);
            }
            finally
            {
                activityIndicator.Visibility = Visibility.Collapsed;
            }
        }

        private async void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            await PerformSearch();
        }
    }
}
