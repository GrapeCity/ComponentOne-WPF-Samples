using System;
using System.Collections.ObjectModel;

namespace PropertyGridExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(Properties.Resources.PropertyGridDemoTitle,
                                Properties.Resources.PropertyGridDemoTitle,
                                new Demo()),
                new SampleItem(Properties.Resources.PropertyGridAutoPropsTitle,
                                Properties.Resources.PropertyGridAutoPropsTitle,
                                new AutoProperties()),
                new SampleItem(Properties.Resources.CustomEditorsTitle,
                                Properties.Resources.CustomEditorsTitle,
                                new CustomEditors()),
                new SampleItem(Properties.Resources.DataObjectPropertiesTitle,
                                Properties.Resources.DataObjectPropertiesTitle,
                                new DataObjectProperties())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
