Imports System.Collections.ObjectModel

Namespace C1TreeViewDragDropSample2010
	''' <summary>
	''' Business object used to represent an Employee.
	''' </summary>
	Public Class Employee
		Public Property EmployeeID() As Integer
		Public Property Name() As String
		Public Property Title() As String
		Public Property Gender() As Char
		Public Property ManagerID() As Integer
		Public Property DepartmentID() As Integer
		Public Property Subordinates() As ObservableCollection(Of Employee)

		Public Sub New()
			Subordinates = New ObservableCollection(Of Employee)()
		End Sub

		Public Overrides Overloads Function Equals(ByVal obj As Object) As Boolean
			Dim employee As Employee = TryCast(obj, Employee)
			Dim isEqual As Boolean = False

			If employee IsNot Nothing Then
				isEqual = (employee.EmployeeID = Me.EmployeeID)
			End If
			Return isEqual
		End Function

		Public Function Clone() As Employee
			Return TryCast(Me.MemberwiseClone(), Employee)
		End Function
	End Class
End Namespace
