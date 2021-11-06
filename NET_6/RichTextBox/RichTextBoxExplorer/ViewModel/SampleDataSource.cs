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
                new SampleItem(Properties.Resources.DemoTitle,
                                Properties.Resources.DemoTitle,
                                new DemoRichTextBox()),

                new SampleItem(Properties.Resources.FormatTitle,
                                Properties.Resources.FormatTitle,
                                new Formatting()),

                new SampleItem(Properties.Resources.DemoFilterTitle,
                                Properties.Resources.DemoFilterTitle,
                                new DemoRichTextBoxFilter()),

                new SampleItem(Properties.Resources.ToolStripTitle,
                                Properties.Resources.ToolStripTitle,
                                new ToolStrip()),

            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
