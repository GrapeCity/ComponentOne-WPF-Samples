using System.Collections.ObjectModel;

namespace RibbonExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(Properties.Resources.RibbonTitle,
                                Properties.Resources.RibbonTitle,
                                new Ribbon()),

                new SampleItem(Properties.Resources.ToolStripTitle,
                                Properties.Resources.ToolStripTitle,
                                new Toolstrip())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
