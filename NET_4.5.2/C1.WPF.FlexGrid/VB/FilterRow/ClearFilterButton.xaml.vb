Imports System.Text

Namespace FilterRow
	''' <summary>
	''' Interaction logic for ClearFilterButton.xaml
	''' </summary>
	Partial Public Class ClearFilterButton
		Inherits UserControl
		' ** fields
		Private _row As GridFilterRow

		' ** ctors
		Public Sub New()
			InitializeComponent()
		End Sub
		Public Sub New(ByVal row As GridFilterRow)
			InitializeComponent()
			_row = row
		End Sub

		' ** event handlers

		' clear all column filters when the user clicks the clear filter button
		Private Sub _btn_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			If _row IsNot Nothing AndAlso _row.Grid IsNot Nothing Then
				' clear filters
				For Each col In _row.Grid.Columns
					_row.SetFilterArgument(col, String.Empty)
				Next col

				' invalidate grid to show that the filters are empty
				_row.Grid.Invalidate()

				' done with the mouse
				e.Handled = True
			End If
		End Sub

		' update button opacity when mouse enters/leaves the button
		Private Sub _btn_MouseEnter(ByVal sender As Object, ByVal e As MouseEventArgs)
			CType(sender, Image).Opacity = 1
		End Sub
		Private Sub _btn_MouseLeave(ByVal sender As Object, ByVal e As MouseEventArgs)
			CType(sender, Image).Opacity =.4
		End Sub
	End Class
End Namespace
