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
    public sealed partial class SelectionModes : UserControl
    {
        List<string> _chartTypes;
        List<FruitDataItem> _fruits;
        List<string> _selectionMode;

        public SelectionModes()
        {
            this.InitializeComponent();
        }

        public List<string> ChartTypes
        {
            get
            {
                if (_chartTypes == null)
                {
                    _chartTypes = DataCreator.CreateSimpleChartTypes();
                }

                return _chartTypes;
            }
        }

        public List<string> Modes
        {
            get
            {
                if (_selectionMode == null)
                {
                    _selectionMode = Enum.GetNames(typeof(ChartSelectionMode)).ToList();
                }

                return _selectionMode;
            }
        }

        public List<FruitDataItem> Data
        {
            get
            {
                if (_fruits == null)
                {
                    _fruits = DataCreator.CreateFruit();
                }

                return _fruits;
            }
        }
    }
}
