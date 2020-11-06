Imports C1.WPF

Namespace C1TreeViewDragDropSample2010
	''' <summary>
	''' Selects a data template depending on the business object.
	''' If the business object represents a Department it gets the DepartmentTemplate.
	''' If the business object represents an Employee, then it selects the MaleEmployeeTemplate
	''' or the FemaleEmployeeTemplate depending on the Gender property of each employee.
	''' </summary>
	Public Class CustomTemplateSelector
		Inherits C1DataTemplateSelector
		Public Overrides Function SelectTemplate(ByVal item As Object, ByVal container As DependencyObject) As DataTemplate
			If TypeOf item Is Department Then
				Return TryCast(Resources("DepartmentTemplate"), DataTemplate)
			Else
				Dim employee As Employee = CType(item, Employee)
				Dim templateKey As String = If(employee.Gender = "M"c, "MaleEmployeeTemplate", "FemaleEmployeeTemplate")
				Return TryCast(Resources(templateKey), DataTemplate)
			End If
		End Function
	End Class
End Namespace