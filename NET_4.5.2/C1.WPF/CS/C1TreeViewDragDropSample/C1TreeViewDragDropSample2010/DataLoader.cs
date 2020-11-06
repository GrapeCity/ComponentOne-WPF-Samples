using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
using System.Xml.Linq;

namespace C1TreeViewDragDropSample2010
{
    /// <summary>
    /// Reads Employees and Departments data from a XML file.
    /// </summary>
    public class DataLoader
    {
        public static List<Employee> LoadEmployees()
        {
            XDocument doc = XDocument.Load(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("C1TreeViewDragDropSample2010.data.employeesDepartment.xml")));
            var employess = from employee in doc.Descendants("Employee")
                            select new Employee
                            {
                                EmployeeID = int.Parse(string.IsNullOrEmpty(employee.Element("EmployeeID").Value) ? "-1" : employee.Element("EmployeeID").Value),
                                Name = employee.Element("Name").Value,
                                Title = employee.Element("Title").Value,
                                Gender = employee.Element("Gender").Value.ToCharArray()[0],
                                ManagerID = int.Parse(string.IsNullOrEmpty(employee.Element("ManagerID").Value) ? "-1" : employee.Element("ManagerID").Value),
                                DepartmentID = int.Parse(string.IsNullOrEmpty(employee.Element("DepartmentID").Value) ? "-1" : employee.Element("DepartmentID").Value)
                            };

            List<Employee> employeeList = new List<Employee>();
            foreach (var employee in employess)
            {
                employeeList.Add(employee);
            }
            return employeeList;
        }

        public static List<Department> LoadDepartments()
        {
            XDocument doc = XDocument.Load(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("C1TreeViewDragDropSample2010.data.departments.xml")));
            var departments = from department in doc.Descendants("Department")
                            select new Department
                            {
                                DepartmentID = int.Parse(string.IsNullOrEmpty(department.Element("DepartmentID").Value) ? "-1" : department.Element("DepartmentID").Value),
                                Name = department.Element("Name").Value,
                                GroupName = department.Element("GroupName").Value
                            };

            List<Department> departmentList = new List<Department>();
            foreach (var department in departments)
            {
                departmentList.Add(department);
            }
            return departmentList;
        }
    }
}
