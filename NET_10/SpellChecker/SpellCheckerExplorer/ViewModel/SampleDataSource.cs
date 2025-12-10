using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SpellCheckerExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>();

            AllItems.Add(new SampleItem(
                Properties.Resources.SpellCheckerTextDemoTitle,
                Properties.Resources.SpellCheckerTextDemoTitle,
                new SpellCheckerTextDemo()));
            AllItems.Add(new SampleItem(
                Properties.Resources.SpellCheckerRtbDemoTitle,
                Properties.Resources.SpellCheckerRtbDemoTitle,
                new SpellCheckerRichTextBoxDemo()));
            AllItems.Add(new SampleItem(
                Properties.Resources.MultiLanguageDemoTitle,
                Properties.Resources.MultiLanguageDemoTitle,
                new MultiLanguageDemo()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }

    public class SampleItem
    {
        public SampleItem(string name, string title, Control sample)
        {
            Name = name;
            Title = title;
            Sample = sample;
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description => Sample.Tag?.ToString();
        public Control Sample { get; set; }
    }
}
