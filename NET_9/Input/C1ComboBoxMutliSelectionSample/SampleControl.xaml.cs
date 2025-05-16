using C1ComboBoxMutliSelectionSample.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace C1ComboBoxMutliSelectionSample
{
    /// <summary>
    /// Interaction logic for SampleControl.xaml
    /// </summary>
    public partial class SampleControl : UserControl, INotifyPropertyChanged
    {
        public SampleControl()
        {
            InitializeComponent();
            Tag = Properties.Resources.Sample;
            Loaded += Window_Loaded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Infos { get; set; } = new ObservableCollection<string>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NormalStyleComboBox.ItemsSource = Employee.Default;
            CheckBoxStyleComboBox.ItemsSource = Employee.Default;

            NormalStyleComboBox.SelectedItems.Add(Employee.Default[2]);
            NormalStyleComboBox.SelectedItems.Add(Employee.Default[4]);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Employee item in e.RemovedItems)
            {
                Infos.Add(string.Format(Properties.Resources.RemoveOperationFormat, item.Name));
            }
            foreach (Employee item in e.AddedItems)
            {
                Infos.Add(string.Format(Properties.Resources.AddOperationFormat, item.Name));
            }

            OnPropertyChanged(nameof(Infos));
            if(OperationsInfoListBox != null)
                OperationsInfoListBox.BringIndexIntoView(Infos.Count - 1);

            if (SelectedItemsListView != null)
                SelectedItemsListView.BringIndexIntoView(SelectedItemsListView.Items.Count - 1);
        }

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
