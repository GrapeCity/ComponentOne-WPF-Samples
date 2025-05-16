using DateTimeEditorsExplorer.Resources;
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
                new SampleItem(
                AppResources.DatePickerTitle,
                AppResources.DatePickerDescription,
                new DemoDatePicker()),

                new SampleItem(
                AppResources.DateTimePickerTitle,
                AppResources.DateTimePickerDescription,
                new DemoDateTimePicker()),

                new SampleItem(
                AppResources.TimeEditorTitle,
                AppResources.TimeEditorDescription,
                new DemoTimeEditor()),

                new SampleItem(
                AppResources.DateTimePickerAdvancedTitle,
                AppResources.DateTimePickerAdvancedDescription,
                new DateTimePickerAdvanced())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
