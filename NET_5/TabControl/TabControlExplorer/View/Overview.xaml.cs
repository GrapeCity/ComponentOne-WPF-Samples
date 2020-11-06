using C1.WPF.TabControl;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TabControlExplorer
{
    /// <summary>
    /// Interaction logic for C1TabControlPreInit.xaml
    /// </summary>
    public partial class Overview : UserControl
    {
        private bool _closing = false;

        public Overview()
        {
            InitializeComponent();
            Tag = "Arrange content in an efficient, organized manner using TabControl for WPF. Take advantage of tab scrolling, tabstrip placement options, pinning and closing tabs.";
        }

        private void tabs_TabClosing(object sender, C1.WPF.TabControl.CancelSourceEventArgs e)
        {
            if (!_closing)
            {
                _closing = true;
                C1TabItem tabItem = (C1TabItem)e.Source;
                e.Cancel = true;
                MessageBoxResult result = MessageBox.Show("Do you really want to close this tab?", "Close Tabs?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    c1Tab.Items.Remove(tabItem);
                }
                _closing = false;
            }
        }
    }
}
