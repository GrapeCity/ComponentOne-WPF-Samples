using System;
using System.Collections.ObjectModel;

namespace DateTimeEditorsExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem("DatePicker",
                "DatePicker",
                new DemoDatePicker()),

                new SampleItem("DateTimePicker",
                "DateTimePicker",
                new DemoDateTimePicker()),

                new SampleItem("TimeEditor",
                "TimeEditor",
                new DemoTimeEditor()),

                new SampleItem("DateTimePicker Advanced",
                "DateTimePicker Advanced",
                new DateTimePickerAdvanced())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
