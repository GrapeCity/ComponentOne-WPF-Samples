Imports System.Net
Imports System.Windows.Media.Animation
Imports C1.WPF.FlexGrid

Namespace CustomTypeDescriptor
    Public Class FinancialCellFactory
        Inherits CellFactory
        Private Shared _thicknessEmpty As New Thickness(0)

        ' bind cell to ticker
        Public Overrides Sub CreateCellContent(ByVal grid As C1FlexGrid, ByVal bdr As Border, ByVal range As CellRange)
            ' create binding for this cell
            Dim r = grid.Rows(range.Row)
            Dim c = grid.Columns(range.Column)
            Dim name = c.BoundPropertyName
            If TypeOf r.DataItem Is FinancialData AndAlso (name = "LastSale" OrElse name = "Bid" OrElse name = "Ask") Then
                Dim ticker As New StockTicker()
                bdr.Child = ticker
                bdr.Padding = _thicknessEmpty
                ' to show sparklines
                ticker.Tag = r.DataItem
                ticker.BindingSource = name

                ' traditional binding
                Dim binding_Renamed = New Binding(name)
                binding_Renamed.Source = r.DataItem
                binding_Renamed.Mode = BindingMode.OneWay
                ticker.SetBinding(StockTicker.ValueProperty, binding_Renamed)
            Else
                ' use default implementation
                MyBase.CreateCellContent(grid, bdr, range)
            End If
        End Sub
    End Class
End Namespace
