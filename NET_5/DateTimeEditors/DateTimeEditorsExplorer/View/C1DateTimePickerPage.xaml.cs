using DateTimeEditorsExplorer.Resources;
using System.Windows.Controls;

namespace DateTimeEditorsExplorer
{
    public partial class DemoDateTimePicker : UserControl
    {
        public DemoDateTimePicker()
        {
            InitializeComponent();
            Tag = AppResources.DateTimePickerTag;
        }
    }
}
