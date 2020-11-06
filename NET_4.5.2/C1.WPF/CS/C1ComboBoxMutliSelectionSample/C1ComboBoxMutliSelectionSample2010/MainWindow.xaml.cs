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
using C1.WPF;

namespace C1ComboBoxMutliSelectionSample2010
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NormalStyleComboBox.ItemsSource = Model.Employee.Default;
            CheckBoxStyleComboBox.ItemsSource = Model.Employee.Default;
            NormalStyleComboBox.SelectedItems.Add(Model.Employee.Default[2]);
            NormalStyleComboBox.SelectedItems.Add(Model.Employee.Default[4]);
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Model.Employee item in e.RemovedItems)
            {
                OperationsInfoListBox.Items.Add("Remove item" + item.Name);
            }
            foreach (Model.Employee item in e.AddedItems)
            {
                OperationsInfoListBox.Items.Add("Add item" + item.Name);
            }
            OperationsInfoListBox.ScrollIntoView(OperationsInfoListBox.Items[OperationsInfoListBox.Items.Count - 1]);
        }
    }
}
