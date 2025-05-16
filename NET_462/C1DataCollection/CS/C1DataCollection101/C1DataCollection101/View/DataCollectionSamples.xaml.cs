using C1DataCollection101.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace C1DataCollection101

{
    public partial class DataCollectionSamples : Page
    {
        public DataCollectionSamples()
        {
            InitializeComponent();
            Title = "C1DataCollection101";
            DataContext = GetSamples();
        }

        private List<Sample> GetSamples()
        {
            return new List<Sample>
            {
                new Sample() { Name = nameof(Sorting), Title = AppResources.SortingTitle, Description= AppResources.SortingDescription, SampleViewType = 0, Thumbnail="sort.png" },
                new Sample() { Name = nameof(Filtering), Title = AppResources.FilteringTitle, Description= AppResources.FilteringDescription, SampleViewType = 1, Thumbnail="filter.png" },
                new Sample() { Name = nameof(Grouping), Title = AppResources.GroupingTitle, Description= AppResources.GroupingDescription, SampleViewType = 2, Thumbnail="flexgrid_grouping.png"},
                new Sample() { Name = nameof(VirtualMode), Title = AppResources.VirtualModeTitle, Description= AppResources.VirtualModeDescription, SampleViewType = 4, Thumbnail="flexgrid_loading.png" },
            };
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sample selectedItem = e.AddedItems[0] as Sample;
            if (selectedItem != null)
            {
                MainWindow frame = Application.Current.MainWindow as MainWindow;
                if (frame != null)
                {
                    Uri link;
                    if (selectedItem.Name.Contains("&"))
                    {
                        link = new Uri("/View/" + selectedItem.Name.Substring(0, selectedItem.Name.IndexOf("&")).Replace(" ", string.Empty) + ".xaml", UriKind.Relative);
                    }
                    else
                        link = new Uri("/View/" + selectedItem.Name.Replace(" ", string.Empty) + ".xaml", UriKind.Relative);
                    frame.Navigate(link);
                    frame.UpdateCaption(selectedItem.Title);
                }

            }
        }
    }
}
