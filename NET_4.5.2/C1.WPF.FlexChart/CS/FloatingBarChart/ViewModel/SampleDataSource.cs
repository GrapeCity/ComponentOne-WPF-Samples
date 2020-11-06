using System;
using System.Collections.ObjectModel;

namespace FloatingBarChart
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Floating Bar",
                "Floating Bar",
                "Floating Bar chart is normally used to display a series of horizontal or vertical bars, with each bar representing a range of values. The sample below shows two series of vertical bars which represent the test score range of different subjects.",
                new FloatBar()));
            _allItems.Add(new SampleItem("Gantt Charts",
                "Gantt Charts",
                "Gantt chart is a series of horizontal floating bars primarily used for scheduling activities and tasks in a project. The sample below shows a Gantt chart for planning the phases of a release.",
                new Gantt()));
            _allItems.Add(new SampleItem("Customized Gantt Chart",
                "Customized Gantt Chart",
                "The sample below shows how to enhance the basic Gantt Chart by using Rendered event and SymbolRendering event. ",
                new Customized()));
            _allItems.Add(new SampleItem("Ranged Area",
               "Ranged Area",
               "Ranged Area chart is normally used to display a series of areas which representing a range of values. The sample below shows two series of areas which represent the test score range of different subjects.",
               new RangedArea()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
