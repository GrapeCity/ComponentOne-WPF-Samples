Imports System.Text
Imports System.Collections.ObjectModel
Imports C1.WPF

Namespace C1TreeViewDragDropSample2010
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private dragDrop As CustomDragDrop

		Public Sub New()
			InitializeComponent()

			SampleTreeView.ItemsSource = BuildDepartmentTree()

			SampleTreeView.AllowDragDrop = True
			dragDrop = New CustomDragDrop(SampleTreeView)
            AddHandler SampleTreeView.DragOver, AddressOf dragDrop.HandleDragOver
            AddHandler SampleTreeView.DragDrop, AddressOf dragDrop.HandleDragDrop
		End Sub

		Private Function BuildDepartmentTree() As ObservableCollection(Of Object)
			Dim itemsSource As New ObservableCollection(Of Object)()
			Dim departmentDirectory As IDictionary(Of Integer, Department) = New Dictionary(Of Integer, Department)()

			' insert all departments as root nodes
			Dim departments As List(Of Department) = DataLoader.LoadDepartments()
			For Each d As Department In departments
				itemsSource.Add(d)
				departmentDirectory.Add(d.DepartmentID, d)
			Next d

			' insert all employees in their corresponding department
			Dim employees As List(Of Employee) = DataLoader.LoadEmployees()
			For Each e As Employee In employees
				Dim d As Department = departmentDirectory(e.DepartmentID)
				d.Employees.Add(e)
			Next e

			Return itemsSource
		End Function

		Private Sub OnButtonChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim rb As System.Windows.Controls.RadioButton = TryCast(sender, System.Windows.Controls.RadioButton)
			If dragDrop IsNot Nothing Then
				dragDrop.Effect = If(rb.Name.CompareTo("Move") = 0, DragDropEffect.Move, DragDropEffect.Copy)
			End If
		End Sub
	End Class
End Namespace
