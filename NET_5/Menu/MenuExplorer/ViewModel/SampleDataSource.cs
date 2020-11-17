using System;
using System.Collections.ObjectModel;

namespace MenuExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(Properties.Resources.MenuTitle,
                                Properties.Resources.MenuTitle,
                                new DemoMenu()),
                new SampleItem(Properties.Resources.AppearanceTitle,
                                Properties.Resources.AppearanceTitle,
                                new CustomAppearance()),
                new SampleItem(Properties.Resources.DropDownMenuTitle,
                                Properties.Resources.DropDownMenuTitle,
                                new DropDownMenu())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
