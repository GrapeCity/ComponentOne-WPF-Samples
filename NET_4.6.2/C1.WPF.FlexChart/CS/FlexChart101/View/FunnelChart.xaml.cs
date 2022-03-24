using C1.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlexChart101
{
    /// <summary>
    /// Interaction logic for FunnelChart.xaml
    /// </summary>
    public partial class FunnelChart : UserControl
    {
        private List<DataItem> _data;

        public FunnelChart()
        {
            InitializeComponent();
        }

        public List<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateFunnelData();
                }

                return _data;
            }
        }

        public List<string> FunnelTypes
        {
            get
            {
                return Enum.GetNames(typeof(FunnelChartType)).ToList();
            }
        }
    }
}
