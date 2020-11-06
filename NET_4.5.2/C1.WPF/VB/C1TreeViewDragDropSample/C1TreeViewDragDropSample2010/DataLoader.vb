Imports System.IO
Imports System.Reflection

Namespace C1TreeViewDragDropSample2010
	''' <summary>
	''' Reads Employees and Departments data from a XML file.
	''' </summary>
	Public Class DataLoader
		Public Shared Function LoadEmployees() As List(Of Employee)
            Dim doc As XDocument = XDocument.Load(New StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("employeesDepartment.xml")))
			Dim employess = From employee In doc.Descendants("Employee")
			                Select New Employee With {.EmployeeID = Integer.Parse(If(String.IsNullOrEmpty(employee.Element("EmployeeID").Value), "-1", employee.Element("EmployeeID").Value)), .Name = employee.Element("Name").Value, .Title = employee.Element("Title").Value, .Gender = employee.Element("Gender").Value.ToCharArray()(0), .ManagerID = Integer.Parse(If(String.IsNullOrEmpty(employee.Element("ManagerID").Value), "-1", employee.Element("ManagerID").Value)), .DepartmentID = Integer.Parse(If(String.IsNullOrEmpty(employee.Element("DepartmentID").Value), "-1", employee.Element("DepartmentID").Value))}

			Dim employeeList As New List(Of Employee)()
			For Each employee In employess
				employeeList.Add(employee)
			Next employee
			Return employeeList
		End Function

		Public Shared Function LoadDepartments() As List(Of Department)
            Dim doc As XDocument = XDocument.Load(New StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("departments.xml")))
			Dim departments = From department In doc.Descendants("Department")
			                  Select New Department With {.DepartmentID = Integer.Parse(If(String.IsNullOrEmpty(department.Element("DepartmentID").Value), "-1", department.Element("DepartmentID").Value)), .Name = department.Element("Name").Value, .GroupName = department.Element("GroupName").Value}

			Dim departmentList As New List(Of Department)()
			For Each department In departments
				departmentList.Add(department)
			Next department
			Return departmentList
		End Function
	End Class
End Namespace
