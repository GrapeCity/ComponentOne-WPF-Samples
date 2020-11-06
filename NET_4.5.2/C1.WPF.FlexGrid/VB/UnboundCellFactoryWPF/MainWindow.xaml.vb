Imports C1.WPF.FlexGrid
Imports C1.WPF.Extended

Class MainWindow

    Private Sub MainWindow_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        ' populate grid
        Dim c As Column = New Column()
        c.ColumnName = "Row#"
        c.Header = c.ColumnName
        c.IsReadOnly = True
        _flex.Columns.Add(c)

        c = New Column()
        c.ColumnName = "Cell Type"
        c.Header = c.ColumnName
        c.IsReadOnly = True
        _flex.Columns.Add(c)

        c = New Column()
        c.ColumnName = "Data"
        c.Header = c.ColumnName
        c.Width = New GridLength(250, GridUnitType.Pixel)
        _flex.Columns.Add(c)

        For i As Integer = 0 To 49
            AddRow(GetType(TextBox))
            AddRow(GetType(CheckBox))
            AddRow(GetType(ComboBox))
            AddRow(GetType(Slider))
            AddRow(GetType(C1ColorPicker))
        Next

        ' initialize custom cell factory
        _flex.Rows.MinSize = 35
        _flex.CellFactory = New UnboundCellFactory(_flex)
        AddHandler _ccf.Click, AddressOf _ccf_Checked
    End Sub

    ' add a new row to the grid, with an editor of the specified type
    Private Sub AddRow(cellType As Type)
        Dim r As New Row()
        r.Tag = cellType
        _flex.Rows.Add(r)
        _flex(r.Index, "Row#") = r.Index
        _flex(r.Index, "Cell Type") = cellType.Name
    End Sub

    ' toggle custom cell factory
    Private Sub _ccf_Checked(sender As Object, e As RoutedEventArgs)
        _flex.CellFactory = If(_ccf.IsChecked.Value, New UnboundCellFactory(_flex), Nothing)
    End Sub
End Class

