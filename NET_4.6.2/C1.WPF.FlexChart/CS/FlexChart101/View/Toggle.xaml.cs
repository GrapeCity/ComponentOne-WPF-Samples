using System.Windows.Controls;
using System.Collections.Generic;

namespace FlexChart101
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Toggle : UserControl
    {
        List<FruitDataItem> _fruits;

        public Toggle()
        {
            this.InitializeComponent();
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
