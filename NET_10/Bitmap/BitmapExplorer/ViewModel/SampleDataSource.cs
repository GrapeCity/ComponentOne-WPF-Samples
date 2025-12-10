using System;
using System.Collections.ObjectModel;

namespace BitmapExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(Properties.Resources.CropDemoTitle,
                                Properties.Resources.CropDemoTitle,
                                new CropDemo()),
                new SampleItem(Properties.Resources.FaceWrapDemoTitle,
                                Properties.Resources.FaceWrapDemoTitle,
                                new FaceWarpDemo()),
                new SampleItem(Properties.Resources.TransformDemoTitle,
                                Properties.Resources.TransformDemoTitle,
                                new TransformDemo()),
                           };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
