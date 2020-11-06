using System.Windows.Controls;

namespace FlexChart101
{
    public class SampleItem
    {
        private SourceCodesModel _sourceCodes;

        public SampleItem(string name, string title, string desc, Control sample)
        {
            Name = name;
            Title = title;
            Description = desc;
            Sample = sample;
            _sourceCodes = new SourceCodesModel()
            {
                XamlFileName = string.Format("{0}.xaml", sample.GetType().Name),
                CodeFileName = string.Format("{0}.xaml.cs", sample.GetType().Name)
            };
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Control Sample { get; set; }
        public SourceCodesModel SourceCodes
        {
            get { return _sourceCodes; }
            set { _sourceCodes = value; }
        }

    }
}
