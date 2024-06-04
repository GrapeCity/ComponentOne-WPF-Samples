using ListViewExplorer.Resources;
using System.Collections.ObjectModel;

namespace ListViewExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems =
            [
                new SampleItem(
                    AppResources.CustomTemplateTitle,
                    AppResources.CustomTemplateTitle,
                    new CustomTemplate()),
                new SampleItem(
                    AppResources.FlickrTitle,
                    AppResources.FlickrTitle,
                    new Flickr()),
                new SampleItem(
                    AppResources.GroupTitle,
                    AppResources.GroupTitle,
                    new Grouping()),
                new SampleItem(
                    AppResources.FilterTitle,
                    AppResources.FilterTitle,
                    new Filtering()),
                new SampleItem(
                    AppResources.SortTitle,
                    AppResources.SortTitle,
                    new Sorting()),
                new SampleItem(
                    AppResources.PagingTitle,
                    AppResources.PagingTitle,
                    new Paging()),
                new SampleItem(
                    AppResources.Unbound,
                    AppResources.Unbound,
                    new Unbound()),
                new SampleItem(
                    AppResources.TileListView,
                    AppResources.TileListView,
                    new TileListView()),
                new SampleItem(
                    AppResources.StylingTitle,
                    AppResources.StylingTitle,
                    new ItemContainerStyle()),
                new SampleItem(
                    AppResources.TemplateSelectorTitle,
                    AppResources.TemplateSelectorTitle,
                    new TemplateSelector()),
                new SampleItem(
                    AppResources.StyleSelectorTitle,
                    AppResources.StyleSelectorTitle,
                    new StyleSelectorSample()),
                new SampleItem(
                    AppResources.CustomAppearanceTitle,
                    AppResources.CustomAppearanceTitle,
                    new CustomAppearance()),
                new SampleItem(
                    AppResources.VirtualModeTitle,
                    AppResources.VirtualModeTitle,
                    new VirtualMode()),
            ];

        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
