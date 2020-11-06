using System.Windows.Controls;
using System.Collections.Generic;

namespace FlexChart101
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GettingStarted : UserControl
    {
        private List<DataItem> _data;

        public GettingStarted()
        {
            this.InitializeComponent();
        }

        public List<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateData();
                }

                return _data;
            }
        }
    }
}
