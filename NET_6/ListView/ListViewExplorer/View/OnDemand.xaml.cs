using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.DataCollection;
using C1.WPF.Core;
using C1.WPF.DataCollection;
using C1.WPF.ListView;
using ListViewCoreExplorer;
using ListViewExplorer.Resources;

namespace ListViewExplorer
{
    public partial class OnDemand : UserControl
    {
        string filterValue;
        YouTubeDataCollection videos = new YouTubeDataCollection();
        SemaphoreSlim filterSemaphore = new System.Threading.SemaphoreSlim(1);

        public OnDemand()
        {
            InitializeComponent();
            Tag = AppResources.OnDemandDescription;
            listView.ItemsSource = videos;
        }

        public string Filter
        {
            get
            {
                return filterValue;
            }
            set
            {
                filterValue = value;
                OnFilterChanged(value);
            }
        }
        private async void OnFilterChanged(string filter)
        {
            Console.WriteLine($"OnTextChange({filterValue})");
            try
            {
                filterValue = filter;
                await Task.Delay(400);
                await filterSemaphore.WaitAsync();
                if (filterValue == filter)
                {
                    listView.ScrollToVerticalOffset(0);
                    await videos.SearchAsync(filterValue);
                }
            }
            finally
            {
                filterSemaphore.Release();
            }
        }

        private void C1TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnFilterChanged(txtFilter.Text);
        }
    }


}
