using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlotAreas : UserControl
    {
        PlotAreasSampleDataItem[] _data;


        public PlotAreas()
        {
            this.InitializeComponent();
        }

        public PlotAreasSampleDataItem[] Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateForPlotAreas();
                }

                return _data;
            }
        }
    }
}
