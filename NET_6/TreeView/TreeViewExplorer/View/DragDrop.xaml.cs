using C1.WPF.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TreeViewExplorer
{
    /// <summary>
    /// Interaction logic for DemoTreeViewDragDrop.xaml
    /// </summary>
    public partial class DemoTreeViewDragDrop : UserControl
    {
        CustomDragDrop dragDrop;

        public DemoTreeViewDragDrop()
        {
            InitializeComponent();
            Tag = Properties.Resources.DragDrop;

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
