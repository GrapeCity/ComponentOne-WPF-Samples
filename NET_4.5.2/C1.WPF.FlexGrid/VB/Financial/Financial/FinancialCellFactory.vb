Imports System.Net
Imports System.Windows.Media.Animation
Imports C1.WPF.FlexGrid


Namespace Financial
	Public Class FinancialCellFactory
		Inherits CellFactory
		Private Shared _thicknessEmpty As New Thickness(0)

		' bind cell to ticker
		Public Overrides Sub CreateCellContent(ByVal grid As C1FlexGrid, ByVal bdr As Border, ByVal range As CellRange)
			' create binding for this cell
			Dim r = grid.Rows(range.Row)
			Dim c = grid.Columns(range.Column)
			Dim pi = c.PropertyInfo
			If TypeOf r.DataItem Is FinancialData AndAlso (pi.Name = "LastSale" OrElse pi.Name = "Bid" OrElse pi.Name = "Ask") Then
				' create stock ticker cell
				Dim ticker As New StockTicker()
				bdr.Child = ticker
				bdr.Padding = _thicknessEmpty

				' to show sparklines
				ticker.Tag = r.DataItem
				ticker.BindingSource = pi.Name

				' traditional binding
                Dim binding = New System.Windows.Data.Binding(pi.Name)
				binding.Source = r.DataItem
				binding.Mode = BindingMode.OneWay
				ticker.SetBinding(StockTicker.ValueProperty, binding)
			Else
				' use default implementation
				MyBase.CreateCellContent(grid, bdr, range)
			End If
		End Sub

		' override horizontal alignment to make ticker cell stretch and fill the column width
		Public Overrides Sub ApplyCellStyles(ByVal grid As C1FlexGrid, ByVal cellType As CellType, ByVal range As CellRange, ByVal bdr As Border)
			Dim ticker = TryCast(bdr.Child, StockTicker)
			If ticker IsNot Nothing Then
                ticker.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
			End If
		End Sub
	End Class
End Namespace
