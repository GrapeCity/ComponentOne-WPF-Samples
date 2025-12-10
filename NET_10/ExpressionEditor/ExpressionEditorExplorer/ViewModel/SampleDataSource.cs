using ExpressionEditorExplorer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEditorExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(Properties.Resources.FlexGridDemoTitle,
                                Properties.Resources.FlexGridDemoTitle,
                                new ExpressionEditorDemo()),
                new SampleItem(Properties.Resources.DataGridDemoTitle,
                                Properties.Resources.DataGridDemoTitle,
                                new ExpressionEditorDataGridDemo()),
                new SampleItem(Properties.Resources.CustomizationSampleTitle,
                                Properties.Resources.CustomizationSampleTitle,
                                new ExpressionEditorCustomizationDemo()),
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
