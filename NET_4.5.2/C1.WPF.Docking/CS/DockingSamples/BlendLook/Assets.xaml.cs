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
using System.Windows.Markup;
using C1.WPF;
using System.Collections.ObjectModel;

namespace DockingSamples.BlendLook
{
    /// <summary>
    /// Interaction logic for Assets.xaml
    /// </summary>
    public partial class Assets : UserControl
    {
        public Assets()
        {
            InitializeComponent();
        }

        private void TreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var s = TreeView.SelectedItem as AssetTreeViewItem;
            if (s == null)
                return;
            ListBox.ItemsSource = s.Controls;
        }
    }

    [ContentProperty("Controls")]
    public class AssetTreeViewItem : C1TreeViewItem
    {
        public Collection<String> Controls { get { return _controls; } }
        Collection<String> _controls = new Collection<string>();
    }
}
