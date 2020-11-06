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
    public sealed partial class StylingSeries : UserControl
    {
        List<FruitDataItem> _data;
        List<string> _palettes;

        public StylingSeries()
        {
            this.InitializeComponent();
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

        public List<string> Palettes
        {
            get
            {
                if (_palettes == null)
                {
                    _palettes = Enum.GetNames(typeof(Palette)).ToList<string>();
                }

                return _palettes;
            }
        }
    }
}
