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
                new SampleItem(Resources.AppResources.PropertyGridDemoTitle,
                                Resources.AppResources.PropertyGridDemoTitle,
                                new Demo()),
                new SampleItem(Resources.AppResources.PropertyGridAutoPropsTitle,
                                Resources.AppResources.PropertyGridAutoPropsTitle,
                                new AutoProperties()),
                new SampleItem(Resources.AppResources.CustomEditorsTitle,
                                Resources.AppResources.CustomEditorsTitle,
                                new CustomEditors()),
                new SampleItem(Resources.AppResources.DataObjectPropertiesTitle,
                                Resources.AppResources.DataObjectPropertiesTitle,
                                new DataObjectProperties()),
                new SampleItem(Resources.AppResources.DynamicBindingTitle,
                                Resources.AppResources.DynamicBindingTitle,
                                new DynamicBinding())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
