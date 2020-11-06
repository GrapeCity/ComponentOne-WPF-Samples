using C1.WPF;
using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace C1TreeViewDragDropSample2010
{
    /// <summary>
    /// Interaction logic for Interaction.xaml
    /// </summary>
    public partial class Interaction : UserControl
    {
        private ObservableCollection<Person> _flexGridData = Person.Generate(5);

        public Interaction()
        {
            InitializeComponent();

            flexGrid.ItemsSource = _flexGridData;
            flexGrid.Drop += FlexGrid_Drop;
            if (flexGrid.Rows.Count > 0)
                flexGrid.Rows[0].GridPanel.MouseMove += FlexGrid_MouseMove;
        }

        private void FlexGrid_MouseMove(object sender, MouseEventArgs e)
        {
            // start drag operation from FlexGrid
            Person item = flexGrid.SelectedItem as Person;
            if (item != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(flexGrid, item.Name, DragDropEffects.Copy);
            }
        }

        private void FlexGrid_Drop(object sender, DragEventArgs e)
        {
            C1FlexGrid grid = sender as C1FlexGrid;
            if (grid != null)
            {
                if (e.Data.GetDataPresent(typeof(C1TreeViewItem)))
                {
                    C1TreeViewItem item = (C1TreeViewItem)e.Data.GetData(typeof(C1TreeViewItem));
                    Random Randomizer = new Random();

                    _flexGridData.Add(new Person()
                    {
                        Name = item.Header.ToString(),
                        Address = Person.Streets[Randomizer.Next(Person.Streets.Length)] + " " + (1000 + Randomizer.Next(8999)).ToString(),
                        Age = 30 + Randomizer.Next(10),
                        Residence = Person.Countries[Randomizer.Next(Person.Countries.Length)],
                        Born = Person.Countries[Randomizer.Next(Person.Countries.Length)],
                    });
                    treeView.Items.Remove(item);
                }
            }
        }
    }
}
