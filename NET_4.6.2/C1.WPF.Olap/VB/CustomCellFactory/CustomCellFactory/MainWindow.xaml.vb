Imports C1.WPF.FlexGrid

Class MainWindow

    Private Sub _c1OlapPage_Loaded(sender As Object, e As RoutedEventArgs)
        _c1OlapPage.DataSource = Product.GetProducts(500)

        ' show product prices by color and line
        Dim olap = _c1OlapPage.OlapPanel.OlapEngine
        olap.BeginUpdate()
        olap.RowFields.Add("Color")
        olap.ColumnFields.Add("Line")
        Dim value = olap.Fields("Price")
        olap.ValueFields.Add(value)
        value.Format = "n0"
        value.Subtotal = C1.Olap.Subtotal.Average
        olap.EndUpdate()

        ' apply conditional formatting to grid cells
        _c1OlapPage.OlapGrid.CellFactory = New ConditionalCellFactory()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ' refresh olap output
        _c1OlapPage.OlapPanel.OlapEngine.Update()
    End Sub
End Class

''' <summary>
''' Custom cell factory class for the C1FlexGrid. This class applies a custom
''' green background to cells with values over 500.
''' </summary>
Public Class ConditionalCellFactory
    Inherits CellFactory
    Public Overrides Function CreateCell(grid As C1FlexGrid, cellType__1 As CellType, range As CellRange) As FrameworkElement
        ' let base class to most of the work
        Dim cell = MyBase.CreateCell(grid, cellType__1, range)

        ' keep the selectionbackground
        If grid.GetSelectedState(range) <> SelectedState.None Then
            Return cell
        End If

        ' apply green background if necessary
        If cellType__1 = CellType.Cell Then
            Dim cellValue = grid(range.Row, range.Column)
            If TypeOf cellValue Is Double AndAlso CDbl(cellValue) > 500 Then
                Dim border = TryCast(cell, Border)
                border.Background = _greenBrush
            End If
        End If

        ' done
        Return cell
    End Function
    Shared _greenBrush As Brush = New SolidColorBrush(Color.FromArgb(&HFF, 150, 255, 150))
End Class

