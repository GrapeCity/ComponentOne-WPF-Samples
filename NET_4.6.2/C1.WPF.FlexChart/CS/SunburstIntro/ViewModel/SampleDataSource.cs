using System;
using System.Collections.ObjectModel;

namespace SunburstIntro
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Getting Started",
                "Getting Started",
                "Steps for getting started with Sunburst control in WPF applications:\r" + Environment.NewLine+
                "1. Add reference to C1.WPF.FlexChart assembly.\r"+
                "2. Bind the data to Sunburst.",
                new GettingStarted()));
            _allItems.Add(new SampleItem("Group Collection",
                "Group Collection",
                "This sample shows how to use the Sunburst chart with Grouped CollectionView",
                new Group()));
            _allItems.Add(new SampleItem("Basic Features",
                "Basic Features",
                "The Sunburst control has five basic properties that allow you to customize its layout and appearance." + Environment.NewLine +
                "1. innerRadius: Specifies the control's inner radius to support donut charts.\r"+
                "2. offset: Specifies the offset of the sunburst slices from the center of the control.\r"+
                "3. startAngle: Starting at the nine o'clock position, specifies the angle to start drawing sunburst slices. \r" +
                "4. palette: Specifies an array of default colors to be used when rendering sunburst slices.\r" +
                "5. reversed: Determines whether the control should draw sunburst slices clockwise(false) or counterclockwise(true)" + Environment.NewLine +
                "The example allows you to see what hanppens when you change these properties. Also, clicking on a sunburst slice will display a tooltip for the data point.",
                new BasicFeatures()));
            _allItems.Add(new SampleItem("Legend And Titles",
                "Legend And Titles",
                "The legend properties can be used to customize the appearance of the chart's legend. The header and footer properties can be used to add titles to the Sunburst control as well.\r"+
                "This example alows you to change the Sunburst's LegendPosition, Header and Footer properties",
                new LegendTitles()));
            _allItems.Add(new SampleItem("Selection",
                "Selection",
                "The Sunburst control allows you to select data points by clicking or touching a sunburst slice.Use the SelectionMode property to specify whether you want to allow selection by data point or no selection at all(default).\r" +
                "Setting the SelctionMode property to Point causes the Sunburst to update the selection property when the user clicks or touch on a sunburst slice. " + Environment.NewLine +
                "The Sunburst offers two additional properties to customize the selection: \r" +
                "SelectedItemOffset: Specifies the offset of the selected sunburst slice from the center of the control.\r" +
                "SelectedItemPosition: Specifies the position of the selected sunburst slice. The available options are Top, Bottom, Left, Right, and None (default).",
                new SunburstSelection()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
