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
using C1.WPF;

namespace C1TreeViewDragDropSample2010
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CustomDragDrop dragDrop;

        public MainWindow()
        {
            InitializeComponent();

            SampleTreeView.ItemsSource = BuildDepartmentTree();

            SampleTreeView.AllowDragDrop = true;
            dragDrop = new CustomDragDrop(SampleTreeView);
        }

        private ObservableCollection<object> BuildDepartmentTree()
        {
            ObservableCollection<object> itemsSource = new ObservableCollection<object>();
            IDictionary<int, Department> departmentDirectory = new Dictionary<int, Department>();

            // insert all departments as root nodes
            List<Department> departments = DataLoader.LoadDepartments();
            foreach (Department d in departments)
            {
                itemsSource.Add(d);
                departmentDirectory.Add(d.DepartmentID, d);
            }

            // insert all employees in their corresponding department
            List<Employee> employees = DataLoader.LoadEmployees();
            foreach (Employee e in employees)
            {
                Department d = departmentDirectory[e.DepartmentID];
                d.Employees.Add(e);
            }

            return itemsSource;
        }

        private void OnButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (dragDrop != null)
            {
                dragDrop.Effect = (rb.Name.CompareTo("Move") == 0) ? DragDropEffect.Move : DragDropEffect.Copy;
            }
        }
    }
}
