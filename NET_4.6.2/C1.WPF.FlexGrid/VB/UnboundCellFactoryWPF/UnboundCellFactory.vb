Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Controls
Imports C1.WPF.FlexGrid
Imports C1.WPF.Extended

Public Class UnboundCellFactory
	Inherits CellFactory
	Private _flex As C1FlexGrid

	' ctor
	Public Sub New(flex As C1FlexGrid)
		_flex = flex
		AddHandler _flex.ScrollPositionChanging, AddressOf flex_ScrollPositionChanging
		AddHandler _flex.BeginningEdit, AddressOf flex_BeginningEdit
	End Sub

	' finish edits before scrolling
	Private Sub flex_ScrollPositionChanging(sender As Object, e As EventArgs)
		_flex.FinishEditing()
	End Sub

	' cancel standard editing for our custom cells (they are editors already)
	Private Sub flex_BeginningEdit(sender As Object, e As CellEditEventArgs)
        Dim t As Type = GetCellType(e.CellRange)
		If t IsNot Nothing Then
			e.Cancel = True
		End If
	End Sub

	' create the custom cell editor
	Public Overrides Sub CreateCellContent(grid As C1FlexGrid, bdr As Border, rng As CellRange)
        Dim t As Type = GetCellType(rng)
		If t IsNot Nothing Then
			' cell has a custom type, create editor
			bdr.Child = CreateEditor(t, rng)
		Else
            ' default handling
			MyBase.CreateCellContent(grid, bdr, rng)
		End If
	End Sub

	' select the element type for a cell
	Private Function GetCellType(rng As CellRange) As Type
        Dim c As Column = _flex.Columns(rng.Column)
		If c.ColumnName = "Data" Then
			Dim r = _flex.Rows(rng.Row)
			Return TryCast(r.Tag, Type)
		End If
		Return Nothing
	End Function

	' initialize custom cell editor
	Private Function CreateEditor(t As Type, rng As CellRange) As UIElement
		' create cell element, save range in Tag property
        Dim edt As FrameworkElement = TryCast(Activator.CreateInstance(t), FrameworkElement)
		edt.Tag = rng
		edt.VerticalAlignment = VerticalAlignment.Center

		' get current value from grid
        Dim value As Object = _flex(rng.Row, rng.Column)

		' initialize each editor type
        Dim textBox As TextBox = TryCast(edt, TextBox)
		If textBox IsNot Nothing Then
			textBox.Text = If(TypeOf value Is String, DirectCast(value, String), String.Empty)
			AddHandler textBox.LostFocus, AddressOf StoreCellValue
		End If

        Dim checkBox As CheckBox = TryCast(edt, CheckBox)
		If checkBox IsNot Nothing Then
			checkBox.HorizontalAlignment = HorizontalAlignment.Center
			checkBox.VerticalAlignment = VerticalAlignment.Center
			checkBox.IsChecked = If(TypeOf value Is Boolean, CBool(value), False)
			AddHandler checkBox.LostFocus, AddressOf StoreCellValue
		End If

        Dim comboBox As ComboBox = TryCast(edt, ComboBox)
		If comboBox IsNot Nothing Then
			For i As Integer = 0 To 4
				comboBox.Items.Add("Item " & i.ToString())
			Next
			comboBox.SelectedValue = If(TypeOf value Is String, DirectCast(value, String), comboBox.Items(0))
			AddHandler comboBox.LostFocus, AddressOf StoreCellValue
		End If

        Dim slider As Slider = TryCast(edt, Slider)
		If slider IsNot Nothing Then
			slider.Minimum = 0
			slider.Maximum = 50
			slider.Value = If(TypeOf value Is Double, CDbl(value), 0)
			AddHandler slider.LostFocus, AddressOf StoreCellValue
		End If

        Dim colorPicker As C1ColorPicker = TryCast(edt, C1ColorPicker)
		If colorPicker IsNot Nothing Then
			colorPicker.SelectedColor = If(TypeOf value Is Color, CType(value, Color), Colors.White)
			AddHandler colorPicker.SelectedColorChanged, AddressOf StoreCellValue
		End If

		' done
		edt.VerticalAlignment = VerticalAlignment.Center
		Return edt
	End Function

	' finalize custom cell editor
	Public Overrides Sub DisposeCell(grid As C1FlexGrid, cellType As CellType, cell As FrameworkElement)
        Dim edt As FrameworkElement = TryCast(DirectCast(cell, Border).Child, FrameworkElement)
		If edt IsNot Nothing Then
            Dim colorPicker As C1ColorPicker = TryCast(edt, C1ColorPicker)
			If colorPicker IsNot Nothing Then
				RemoveHandler colorPicker.SelectedColorChanged, AddressOf StoreCellValue
			Else
				RemoveHandler edt.LostFocus, AddressOf StoreCellValue
			End If
		End If
		MyBase.DisposeCell(grid, cellType, cell)
	End Sub

    ' store edited value into grid cell when the editor loses focus/changes
	Private Sub StoreCellValue(sender As Object, e As EventArgs)
		' get editor and range
        Dim edt As FrameworkElement = TryCast(sender, FrameworkElement)
        Dim rng As CellRange = CType(edt.Tag, CellRange)

		' apply value
        Dim textBox As TextBox = TryCast(edt, TextBox)
		If textBox IsNot Nothing Then
			_flex(rng.Row, rng.Column) = textBox.Text
		End If
        Dim checkBox As CheckBox = TryCast(edt, CheckBox)
		If checkBox IsNot Nothing Then
			_flex(rng.Row, rng.Column) = checkBox.IsChecked
		End If

        Dim comboBox As ComboBox = TryCast(edt, ComboBox)
		If comboBox IsNot Nothing Then
			_flex(rng.Row, rng.Column) = comboBox.SelectedValue
		End If

        Dim slider As Slider = TryCast(edt, Slider)
		If slider IsNot Nothing Then
			_flex(rng.Row, rng.Column) = slider.Value
		End If

        Dim colorPicker As C1ColorPicker = TryCast(edt, C1ColorPicker)
		If colorPicker IsNot Nothing Then
			_flex(rng.Row, rng.Column) = colorPicker.SelectedColor
		End If
	End Sub
End Class
