using System;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using C1.Chart;

namespace FlexChart101
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LegendAndTitles : UserControl
    {
        List<GroupDataItem> _data;
        List<string> _positions;

        public LegendAndTitles()
        {
            this.InitializeComponent();
        }

        public List<GroupDataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateGroupData();
                }

                return _data;
            }
        }

        public List<string> Legends
        {
            get
            {
                if (_positions == null)
                {
                    _positions = Enum.GetNames(typeof(Position)).ToList<string>();
                }

                return _positions;
            }
        }

        private void cbGroup_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cbGroup.IsChecked.HasValue && flexChart != null)
            {
                foreach (ISeries ser in flexChart.Series)
                {
                    ser.LegendGroup = (cbGroup.IsChecked == false) ? null :
                        ((ser.Name.IndexOf("Sales") > -1) ? "Sales" : "Expenses");
                }
            }
        }
    }
}
