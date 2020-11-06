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
using System.Collections.ObjectModel;
using C1.WPF.FlexGrid;

namespace GridTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Person _person;
        Random _rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();

            // number of children for each person (4 levels)
            // item count is 1 + count + count^2 + count^3 + count^4
            // (e.g. count = 10 => ~10,000 people)
            int count = 10;

            // build person tree
            _person = new Person();
            #region CreatingPersons
            using (new Benchmark("Building person tree", _txtPerson))
            {
                for (int i = 0; i < count; i++)
                {
                    var pi = new Person();
                    _person.Children.Add(pi);

                    for (int j = 0; j < count; j++)
                    {
                        var pj = new Person();
                        pi.Children.Add(pj);

                        for (int k = 0; k < count; k++)
                        {
                            var pk = new Person();
                            pj.Children.Add(pk);

                            for (int l = 0; l < count; l++)
                            {
                                var pl = new Person();
                                pk.Children.Add(pl);
                            }
                        }
                    }
                }
            } 
            #endregion

            #region Populating
            // put person in a list so we can bind to it
            var list = new ObservableCollection<Person>();
            list.Add(_person);

            // show items in bound controls
            using (new Benchmark("Bound TreeView", _txtTree))
            {
                _tree.ItemsSource = list;
            }
            using (new Benchmark("Bound FlexGrid", _txtFlex))
            {
                _flex.ItemsSource = list;
            }

            // show items in unbound controls
            using (new Benchmark("Unbound TreeView", _txtTreeUnbound))
            {
                ShowPersonOnTree(_person, _treeUnbound);
            }

            using (new Benchmark("Unbound FlexGrid", _txtFlexUnbound))
            {
                ShowPersonOnGrid(_person, _flexUnbound);
            }
            #endregion

            // try doing this with a TreeView ;-)
            _flex.CollapseGroupsToLevel(5);
            _flexUnbound.CollapseGroupsToLevel(5);

            // hide row headers
            _flex.HeadersVisibility = HeadersVisibility.Column;
            _flexUnbound.HeadersVisibility = HeadersVisibility.Column;
        }

        #region ** populate unbound TreeView

        void ShowPersonOnTree(Person p, TreeView t)
        {
            t.Items.Clear();
            AddPersonToTree(p, t.Items);
        }
        void AddPersonToTree(Person p, ItemCollection items)
        {
            // create an item for this person
            var item = new TreeViewItem();
            item.Tag = p;
            item.Header = p.Name;

            // add this person to the tree
            items.Add(item);

            // and add any children
            foreach (var child in p.Children)
            {
                AddPersonToTree(child, item.Items);
            }
        }

        #endregion

        #region ** populate unbound FlexGrid

        // populate unbound FlexGrid
        void ShowPersonOnGrid(Person p, C1FlexGrid flex)
        {
            // initialize grid
            flex.Rows.Clear();
            flex.Columns.Clear();

            // add columns we want to show
            var c = new Column();
            c.Header = "Name";
            c.Binding = new System.Windows.Data.Binding("Name");
            c.Width = new GridLength(1, GridUnitType.Star);
            flex.Columns.Add(c);

            c = new Column();
            c.Header = "Children";
            c.Binding = new System.Windows.Data.Binding("Children.Count");
            c.HorizontalAlignment = HorizontalAlignment.Right;
            flex.Columns.Add(c);

            using (flex.Rows.DeferNotifications())
            {
                AddPersonToGrid(p, flex, 0);
            }
        }
        void AddPersonToGrid(Person p, C1FlexGrid flex, int level)
        {
            // create a row for this person
            Row row;
            if (p.Children.Count > 0 || true)
            {
                var gr = new GroupRow();
                gr.Level = level;
                row = gr;
            }
            else
            {
                row = new Row();
            }
            row.DataItem = p;

            // add this person to the grid
            flex.Rows.Add(row);

            // and add any children
            foreach (var child in p.Children)
            {
                AddPersonToGrid(child, flex, level + 1);
            }
        }

        #endregion
        #region EventHandlers

        private void _btnAddRoot_Click(object sender, RoutedEventArgs e)
        {
            var list = _flex.CollectionView.SourceCollection as ObservableCollection<Person>;
            var root = new Person();
            list.Insert(0, root);
        }

        private void _btnAddChild_Click(object sender, RoutedEventArgs e)
        {
            var p = GetSelectedPerson();
            p.Children.Insert(0, new Person());
        }

        private void _btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var p = GetSelectedPerson();
            var parent = p.Parent;
            if (parent != null)
            {
                parent.Children.Remove(p);
            }
        }

        private void _btnChange_Click(object sender, RoutedEventArgs e)
        {
            var p = GetSelectedPerson();
            p.Name = _rnd.Next(0, 1000).ToString();
        } 
        #endregion

        Person GetSelectedPerson()
        {
            var p = _flex.SelectedItem as Person;
            return p != null ? p : _person;
        }
    }
}
