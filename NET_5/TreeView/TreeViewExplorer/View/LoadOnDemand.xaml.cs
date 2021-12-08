using C1.WPF.TreeView;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TreeViewExplorer
{
    /// <summary>
    /// Interaction logic for SimpleC1TreeViewPage.xaml
    /// </summary>
    public partial class LoadOnDemand : UserControl
    {
        public LoadOnDemand()
        {
            InitializeComponent();
            Tag = Properties.Resources.LoadOnDemand;
            LoadData();
        }

        private void LoadData()
        {
            tree.Items.Clear();
            // Scan every type in the assembly
            foreach (Type t in tree.GetType().Assembly.GetTypes())
            {
                if (t.IsPublic && !t.IsSpecialName && !t.IsAbstract)
                {
                    // Add node for this type
                    C1TreeViewItem node = new C1TreeViewItem();
                    node.Header = t.Name;
                    node.FontWeight = FontWeights.Bold;
                    tree.Items.Add(node);
                    node.Tag = t;
                    // Add subnodes for properties, events, and methods
                    node.Items.Add(CreateMemberNode("Properties", MemberTypes.Property));
                    node.Items.Add(CreateMemberNode("Events", MemberTypes.Event));
                    node.Items.Add(CreateMemberNode("Methods", MemberTypes.Method));
                }
            }
        }

        C1TreeViewItem CreateMemberNode(string header, MemberTypes memberTypes)
        {
            // Create the node
            C1TreeViewItem node = new C1TreeViewItem();
            node.Header = header;
            node.Foreground = new SolidColorBrush(Colors.DarkGray);
            // Hook up event hander to populate the node before expanding it
            node.Expanding += node_Expanding;
            // Save information needed to populate the node
            node.Tag = memberTypes;
            // Add a dummy node so this node can be expanded
            node.Items.Add(new C1TreeViewItem());
            node.IsExpanded = false;
            // Finish
            return node;
        }

        //populate the node
        async void node_Expanding(object sender, C1.WPF.Core.SourcedEventArgs e)
        {
            indicator.Visibility = Visibility.Visible;
            // Get the node that fired the event
            C1TreeViewItem node = sender as C1TreeViewItem;
            // Unhook event handler (we'll populate the node and be done with it)
            node.Expanding -= node_Expanding;
            await Task.Delay(500); //Simulate loading
            await LoadChildren(node);
            indicator.Visibility = Visibility.Collapsed;
        }

        async Task<bool> LoadChildren(C1TreeViewItem node)
        {
            // Remove dummy node
            node.Items.Clear();
            node.ClearValue(C1TreeViewItem.ForegroundProperty);

            // Populate the node
            var items = new List<string>();
            var type = (Type)node.Parent.Tag;
            MemberTypes memberTypes = (MemberTypes)node.Tag;
            BindingFlags bf = BindingFlags.Public | BindingFlags.Instance;
            foreach (MemberInfo mi in type.GetMembers(bf))
            {
                if (mi.MemberType == memberTypes)
                {
                    if (!mi.Name.StartsWith("get_") && !mi.Name.StartsWith("set_"))
                    {
                        //C1TreeViewItem item = new C1TreeViewItem();
                        //item.Header = mi.Name;
                        //item.FontSize = 12;
                        //node.Items.Add(item);
                        items.Add(mi.Name);
                    }
                }
            }
            node.ItemsSource = items;
            return await Task.FromResult(true);
        }

    }
}
