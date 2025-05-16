using FlexViewerExplorer.Resources;
using System;
using System.Collections.ObjectModel;

namespace FlexViewerExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem(AppResources.PdfViewerTitle,
                AppResources.PdfViewerTitle,
                new PdfViewer()));
            _allItems.Add(new SampleItem(AppResources.CustomAppearanceTitle,
                AppResources.CustomAppearanceTitle,
                new CustomAppearance()));
            _allItems.Add(new SampleItem(AppResources.FlexReportViewerTitle,
                AppResources.FlexReportViewerTitle,
                new FlexReportViewer()));
            _allItems.Add(new SampleItem(AppResources.SsrsViewerTitle,
                AppResources.SsrsViewerTitle,
                new SsrsViewer()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
