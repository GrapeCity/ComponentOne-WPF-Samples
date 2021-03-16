using System;
using System.Collections.ObjectModel;

namespace AccordionExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(Properties.Resources.AccordionTitle,
                                Properties.Resources.AccordionTitle,
                                new AccordionSample()),
                new SampleItem(Properties.Resources.ExpanderTitle,
                                Properties.Resources.ExpanderTitle,
                                new ExpanderSample()),                
                           };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
