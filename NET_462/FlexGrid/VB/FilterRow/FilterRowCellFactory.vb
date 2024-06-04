Imports System.Text
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports C1.WPF.FlexGrid

Namespace FilterRow
	Friend Class FilterRowCellFactory
		Inherits CellFactory
		' ** fields
		Private Shared _editorPadding As New Thickness(0, 0, 2, 0)

		' ** properties

		''' <summary>
		''' Set this property to true to handle the '%' character as a wildcard
		''' when filtering.
		''' For example, 
		'''     StartsWith%
		'''     %Endswith
		'''     %Contains%
		''' </summary>
		Public Property UseRegEx() As Boolean

		' ** overrides
		Public Overrides Sub CreateColumnHeaderContent(ByVal grid As C1FlexGrid, ByVal bdr As Border, ByVal range As CellRange)
			' create the filter row if it isn't there
			Dim rows = grid.ColumnHeaders.Rows
			If rows.Count = 0 OrElse Not(TypeOf rows(rows.Count - 1) Is GridFilterRow) Then
				rows.Add(New GridFilterRow())
			End If

			' add filter editors to filter row
			Dim fr = TryCast(rows(range.Row), GridFilterRow)
			If fr IsNot Nothing Then
				bdr.Child = New FilterEditor(fr, grid.Columns(range.Column))
				bdr.Padding = _editorPadding
			Else
				MyBase.CreateColumnHeaderContent(grid, bdr, range)
			End If
		End Sub
		Public Overrides Sub CreateTopLeftContent(ByVal grid As C1FlexGrid, ByVal bdr As Border, ByVal range As CellRange)
			Dim rows = grid.ColumnHeaders.Rows
			Dim fr = TryCast(rows(range.Row), GridFilterRow)
			If fr IsNot Nothing AndAlso range.Column = grid.TopLeftCells.Columns.Count - 1 Then
				bdr.Child = New ClearFilterButton(fr)
			Else
				MyBase.CreateTopLeftContent(grid, bdr, range)
			End If
		End Sub
		Public Overrides Function IsSortSymbolRow(ByVal grid As C1FlexGrid, ByVal range As CellRange) As Boolean
			' show sort symbols on the column header row above the filter row
			Return range.TopRow = grid.ColumnHeaders.Rows.Count - 2
		End Function
	End Class
	Public Class GridFilterRow
		Inherits Row
		' ** fields
		Private _filterArgs As New Dictionary(Of Column, String)()
		Private _rxPattern As New Dictionary(Of Column, String)()
		Private _useRegex As Boolean

		' ** object model
		Public Function GetFilterArgument(ByVal col As Column) As String
			Dim text As String
			Return If(_filterArgs.TryGetValue(col, text), text, String.Empty)
		End Function
		Public Sub SetFilterArgument(ByVal col As Column, ByVal arg As String)
			If arg <> GetFilterArgument(col) Then
				' save argument
				_filterArgs(col) = arg

				' save regex pattern to support wildcards
				arg = If(arg IsNot Nothing, arg.Replace("%", ".*"), arg)
				_rxPattern(col) = String.Format("^{0}$", arg)

				' apply the filter
				ApplyFilter()
			End If
		End Sub

		' ** implementation
		Private Sub ApplyFilter()
			' check whether to use wildcards when matching
			Dim cf = TryCast(Grid.CellFactory, FilterRowCellFactory)
			_useRegex = cf IsNot Nothing AndAlso cf.UseRegEx

			' apply filter to grid's ICollectionView
			Dim view = TryCast(Grid.ItemsSource, ICollectionView)
			If view Is Nothing OrElse (Not view.CanFilter) Then
				' apply the filter in unbound mode
				For Each row In Grid.Rows
					row.Visible = FilterPredicate(row)
				Next row

				' done
				Return
			End If

			' can't apply filters while editing or adding items
			Dim edt = TryCast(view, IEditableCollectionView)
			If edt IsNot Nothing Then
				If edt.IsEditingItem Then
					edt.CommitEdit()
				End If

				If edt.IsAddingNew Then
					edt.CommitNew()
				End If
			End If

			' apply the filter in bound mode
			Try
				view.Filter = AddressOf FilterPredicate
			Catch x As Exception
				MessageBox.Show("Failed to apply the filter:" & vbCrLf, x.Message)
			End Try
		End Sub
		Private Function FilterPredicate(ByVal item As Object) As Boolean
			' get item type to retrieve properties
			Dim itemType = item.GetType()

			' in unbound mode, the item is the row itself
			Dim unbound As Boolean = TypeOf item Is Row

			' check filter values applied to each column
			For Each kv In _filterArgs
				' no filter condition on this column? continue
				If String.IsNullOrEmpty(kv.Value) Then
					Continue For
				End If

				' get cell value
				Dim stringValue = String.Empty
				Dim row = TryCast(item, Row)
				If row IsNot Nothing Then
					' handle unbound mode
					Dim value = row(kv.Key)
					stringValue = If(value Is Nothing, String.Empty, value.ToString())
				Else
					' get property name
					Dim propName = kv.Key.BoundPropertyName
					If String.IsNullOrEmpty(propName) Then
						Continue For
					End If

					' get property info
					Dim prop = itemType.GetProperty(propName)
					If prop Is Nothing Then
						Continue For
					End If

					' get property value
					Dim value = prop.GetValue(item, Nothing)
					stringValue = If(value Is Nothing, String.Empty, value.ToString())
				End If

				If _useRegex Then
					' test using regular expressions (support wildcards)
					If Not Regex.IsMatch(stringValue, _rxPattern(kv.Key), RegexOptions.IgnoreCase) Then
						Return False
					End If
				Else
					' text using simple 'contains' 
					If stringValue.IndexOf(kv.Value, StringComparison.OrdinalIgnoreCase) < 0 Then
						Return False
					End If
				End If
			Next kv

			' item passed all filter conditions
			Return True
		End Function
	End Class
End Namespace
