Imports C1.WPF.FlexGrid

Public Class MyCellFactory
    Inherits C1.WPF.FlexGrid.CellFactory

    Public Overrides Sub CreateCellContent(grid As C1.WPF.FlexGrid.C1FlexGrid, bdr As System.Windows.Controls.Border, rng As C1.WPF.FlexGrid.CellRange)

        ' check if the cell contains a boolean value
        If TypeOf grid(rng.Row, rng.Column) Is Boolean Then

            ' it does, so create a checkbox to show/edit the value
            Dim chk As New CheckBox
            chk.IsChecked = grid(rng.Row, rng.Column)
            chk.VerticalAlignment = VerticalAlignment.Center
            chk.HorizontalAlignment = HorizontalAlignment.Center
            ToolTipService.SetToolTip(chk, "This CheckBox represents a boolean value stored in a grid cell.")

            ' assign the checkbox to the cell element (a Border)
            bdr.Child = chk

            ' connect the checkbox so it updates the content of the grid cell
            chk.Tag = grid
            chk.AddHandler(CheckBox.ClickEvent, New RoutedEventHandler(AddressOf chk_Click))

        ElseIf TypeOf grid(rng.Row, rng.Column) Is Control Then

            ' add the control to the cell (if it doesn't have a parent)
            Dim ctl As Control = DirectCast(grid(rng.Row, rng.Column), Control)
            If ctl.Parent Is Nothing Then
                bdr.Child = grid(rng.Row, rng.Column)
            End If

        Else

            ' not a boolean, allow default behavior
            MyBase.CreateCellContent(grid, bdr, rng)

        End If

    End Sub

    ' when our checkbox is clicked, update the underlying value
    Private Sub chk_Click(sender As Object, e As EventArgs)

        ' get the checkbox that was clicked
        Dim chk As CheckBox = DirectCast(sender, CheckBox)

        ' get the grid that owns the checkbox
        Dim flex As C1FlexGrid = DirectCast(chk.Tag, C1FlexGrid)

        ' get the cell that contains the checkbox
        Dim bdr As Border = DirectCast(chk.Parent, Border)
        Dim row As Integer = bdr.GetValue(Grid.RowProperty)
        Dim col As Integer = bdr.GetValue(Grid.ColumnProperty)

        ' assign new value to the cell
        flex(row, col) = chk.IsChecked

    End Sub

    ' when cells are destroyed, remove content from borders so it can be re-used
    Public Overrides Sub DisposeCell(grid As C1.WPF.FlexGrid.C1FlexGrid, cellType As C1.WPF.FlexGrid.CellType, cell As System.Windows.FrameworkElement)
        Dim bdr As Border = DirectCast(cell, Border)
        bdr.Child = Nothing
    End Sub

End Class
