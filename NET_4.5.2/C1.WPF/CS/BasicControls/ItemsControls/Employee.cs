using System.Collections.ObjectModel;

namespace BasicControls
{
    /// <summary>
    /// Business object used to represent an Employee.
    /// </summary>
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public char Gender { get; set; }
        public int ManagerID { get; set; }
        public int DepartmentID { get; set; }
        public ObservableCollection<Employee> Subordinates { get; set; }

        public Employee()
        {
            Subordinates = new ObservableCollection<Employee>();
        }

        public override bool Equals(object obj)
        {
            Employee employee = obj as Employee;
            bool isEqual = false;

            if (employee != null)
            {
                isEqual = (employee.EmployeeID == this.EmployeeID);
            }
            return isEqual;
        }

        public Employee Clone()
        {
            return this.MemberwiseClone() as Employee;
        }
    }
}
