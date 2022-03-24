using C1DataCollection101.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C1.DataCollection;
using System.Windows.Controls;
using C1.WPF.DataCollection;

namespace C1DataCollection101
{
    public partial class Grouping : Page
    {
        C1DataCollection<YouTubeVideo> _dataCollection;

        public Grouping()
        {
            InitializeComponent();
            Title = AppResources.FilteringTitle;
            UpdateVideos();
        }

        private async Task UpdateVideos()
        {
            try
            {
                emptyListLabel.Visibility = System.Windows.Visibility.Collapsed;
                activityIndicator.Visibility = System.Windows.Visibility.Visible;
                var _videos = new ObservableCollection<YouTubeVideo>((await YouTubeCollectionView.LoadVideosAsync("WPF", "relevance", null, 50)).Item2);
                _dataCollection = new C1DataCollection<YouTubeVideo>(_videos);
                await _dataCollection.GroupAsync(v => v.ChannelTitle);
                grid.ItemsSource = new C1CollectionView(_dataCollection);
            }
            catch
            {
                emptyListLabel.Text = AppResources.InternetConnectionError;
                emptyListLabel.Visibility = System.Windows.Visibility.Visible;
            }
            finally
            {
                activityIndicator.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
