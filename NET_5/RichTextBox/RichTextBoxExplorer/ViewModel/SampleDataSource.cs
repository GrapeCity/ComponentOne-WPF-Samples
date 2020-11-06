using System;
using System.Collections.ObjectModel;

namespace RichTextBoxExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem("Overview",
                                "Overview",
                                new DemoRichTextBox()),

                new SampleItem("Formatting",
                                "Formatting",
                                new Formatting()),

                new SampleItem("Import & Export",
                                "Import & Export",
                                new DemoRichTextBoxFilter()),

                new SampleItem("ToolStrip",
                                "ToolStrip",
                                new ToolStrip()),

            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
