Imports System.Text
Imports C1.WPF.FlexGrid

Namespace ExcelGrid
	''' <summary>
	''' Custom cell factory that displays Excel-style row and column headers.
	''' </summary>
	Public Class ExcelCellFactory
		Inherits CellFactory
		Public Overrides Sub CreateTopLeftContent(ByVal grid As C1FlexGrid, ByVal bdr As Border, ByVal range As CellRange)
			Dim poly = CreatePolygon(0, 10, 10, 10, 10, 0)
			poly.Fill = New SolidColorBrush(Color.FromArgb(&He0, &Hf5, &Hf5, &Hf5))
			poly.VerticalAlignment = VerticalAlignment.Center
			poly.HorizontalAlignment = HorizontalAlignment.Right
			poly.Margin = New Thickness(2)
			bdr.Child = poly
			bdr.Tag = grid
			AddHandler bdr.MouseLeftButtonDown, AddressOf bdr_MouseLeftButtonDown
		End Sub
		Private Sub bdr_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			' select the whole grid when user clicks the top left cell
			Dim g = TryCast((CType(sender, FrameworkElement)).Tag, C1FlexGrid)
			g.SelectAll()
		End Sub
		Public Overrides Sub CreateRowHeaderContent(ByVal grid As C1FlexGrid, ByVal bdr As Border, ByVal range As CellRange)
			Dim tb = New TextBlock()
			tb.HorizontalAlignment = HorizontalAlignment.Center
			tb.VerticalAlignment = VerticalAlignment.Center
			tb.Text = String.Format("{0}", range.Row + 1)
			bdr.Child = tb
		End Sub
		Public Overrides Sub CreateColumnHeaderContent(ByVal grid As C1FlexGrid, ByVal bdr As Border, ByVal range As CellRange)
			Dim tb = New TextBlock()
			tb.HorizontalAlignment = HorizontalAlignment.Center
			tb.VerticalAlignment = VerticalAlignment.Center
			tb.Text = GetAlphaColumnHeader(range.Column)
			bdr.Child = tb
		End Sub

		' create Excel style column headers ("A", "B", ... "AB", etc)
		Private Function GetAlphaColumnHeader(ByVal i As Integer) As String
			Return GetAlphaColumnHeader(i, String.Empty)
		End Function
		Private Function GetAlphaColumnHeader(ByVal i As Integer, ByVal s As String) As String
			Dim [rem] = i Mod 26
            s = CChar(ChrW(AscW("A"c) + [rem])) & s
			i = i \ 26 - 1
			Return If(i < 0, s, GetAlphaColumnHeader(i, s))
		End Function

		' create a polygon based on a list of points
		Private Shared Function CreatePolygon(ParamArray ByVal values() As Double) As Polygon
			Dim p = New Polygon()
			p.Points = New PointCollection()
			For i As Integer = 0 To values.Length - 2 Step 2
				p.Points.Add(New Point(values(i), values(i + 1)))
			Next i
			Return p
		End Function
	End Class
End Namespace
