using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using FlexChartExplorer.Resources;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PieSelection : UserControl
    {
        List<FruitDataItem> _data;
        List<string> _position;

        public PieSelection()
        {
            this.InitializeComponent();
            Tag = AppResources.PieSelectionTag;
        }

        public List<string> Positions
        {
            get
            {
                if (_position == null)
                {
                    _position = Enum.GetNames(typeof(Position)).ToList();
                }

                return _position;
            }
        }

        public List<double> Offsets
        {
            get
            {
                return new List<double>(){ 0, 0.1, 0.2 };
            }
        }

        public List<FruitDataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateFruit();
                }

                return _data;
            }
        }
    }
}
