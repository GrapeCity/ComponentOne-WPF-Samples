Imports System.Text

Namespace CollectionViewFilter
	Public Class ViewFilter
		' ** fields
		Private _view As System.ComponentModel.ICollectionView
		Private _filterExpression As String
		Private _dt As DataTable

		' ** ctor
		Public Sub New(ByVal view As System.ComponentModel.ICollectionView)
			_view = view
		End Sub

		' ** object model
		Public Property FilterExpression() As String
			Get
				Return _filterExpression
			End Get
			Set(ByVal value As String)
				_filterExpression = value
				UpdateFilter()
				_view.Filter = Nothing
				If Not String.IsNullOrEmpty(_filterExpression) Then
					_view.Filter = AddressOf FilterPredicate
				End If
			End Set
		End Property

		' ** implementation
		Private Function FilterPredicate(ByVal obj As Object) As Boolean
			' populate the row
			Dim row = _dt.Rows(0)
			For Each pi In obj.GetType().GetProperties()
				row(pi.Name) = pi.GetValue(obj, Nothing)
			Next pi

			' compute the expression
			Return CBool(row("_filter"))
		End Function
		Private Sub UpdateFilter()
			_dt = Nothing
			If _view.CurrentItem IsNot Nothing AndAlso (Not String.IsNullOrEmpty(_filterExpression)) Then
				' build/rebuild data table
				Dim dt = New DataTable()
				For Each pi In _view.CurrentItem.GetType().GetProperties()
					dt.Columns.Add(pi.Name, pi.PropertyType)
				Next pi

				' add calculated column
				dt.Columns.Add("_filter", GetType(Boolean), _filterExpression)

				' create a single row for evaluating expressions
				If dt.Rows.Count = 0 Then
					dt.Rows.Add(dt.NewRow())
				End If

				' done, save table
				_dt = dt
			End If
		End Sub
	End Class
End Namespace
