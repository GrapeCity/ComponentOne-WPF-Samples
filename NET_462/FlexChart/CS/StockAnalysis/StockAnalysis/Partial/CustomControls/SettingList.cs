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

namespace StockAnalysis.Partial.CustomControls
{
    public class SettingList : ItemsControl
    {
        static SettingList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SettingList), new FrameworkPropertyMetadata(typeof(SettingList)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var container = new SettingListItem();
            return container;
        }
        

    }
}
