using ListViewExplorer.Resources;
using System.Collections.ObjectModel;

namespace ListViewExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>();

            AllItems.Add(new SampleItem(
                AppResources.CustomTemplateTitle,
                AppResources.CustomTemplateTitle,
                new CustomTemplate()));
            AllItems.Add(new SampleItem(
                AppResources.FlickrTitle,
                AppResources.FlickrTitle,
                new Flickr()));
            AllItems.Add(new SampleItem(
                AppResources.GroupTitle,
                AppResources.GroupTitle,
                new Grouping()));
            AllItems.Add(new SampleItem(
                AppResources.FilterTitle,
                AppResources.FilterTitle,
                new Filtering()));
            AllItems.Add(new SampleItem(
                AppResources.SortTitle,
                AppResources.SortTitle,
                new Sorting()));
            AllItems.Add(new SampleItem(
                AppResources.PagingTitle,
                AppResources.PagingTitle,
                new Paging()));

            AllItems.Add(new SampleItem(
                AppResources.Unbound,
                AppResources.Unbound,
                new Unbound()));

            AllItems.Add(new SampleItem(
                AppResources.TileListView,
                AppResources.TileListView,
                new TileListView()));

            AllItems.Add(new SampleItem(
                AppResources.StylingTitle,
                AppResources.StylingTitle,
                new ItemContainerStyle()));

            AllItems.Add(new SampleItem(
                AppResources.TemplateSelectorTitle,
                AppResources.TemplateSelectorTitle,
                new TemplateSelector()));
            AllItems.Add(new SampleItem(
                AppResources.StyleSelectorTitle,
                AppResources.StyleSelectorTitle,
                new StyleSelectorSample()));

            AllItems.Add(new SampleItem(
                AppResources.CustomAppearanceTitle,
                AppResources.CustomAppearanceTitle,
                new CustomAppearance()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
