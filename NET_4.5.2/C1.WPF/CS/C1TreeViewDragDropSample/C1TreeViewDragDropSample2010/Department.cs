using System.Collections.ObjectModel;

namespace C1TreeViewDragDropSample2010
{
    /// <summary>
    /// Business object used to represent a Department.
    /// </summary>
    public class Department
    {
        public Department()
        {
            Employees = new ObservableCollection<Employee>();
        }

        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
    }
}
