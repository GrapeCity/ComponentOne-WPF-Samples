Imports System.Text
Imports C1.WPF.FlexGrid

Namespace Invalidate
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()

			' populate grid
			Dim list_Renamed = New List(Of Rect)()
			For i As Integer = 0 To 9
				list_Renamed.Add(New Rect(i, i, i, i))
			Next i
			_flex.ItemsSource = list_Renamed

			' attach custom cell factory
			_flex.CellFactory = New CustomCellFactory(_flex)
		End Sub
	End Class
	Public Class CustomCellFactory
		Inherits CellFactory
		Private _flex As C1FlexGrid
		Private _hotRange As CellRange
		Private Shared _arrowBrush As Brush = New SolidColorBrush(Colors.Red)
		Private Shared _leftArrow As New PointCollection(New Point() { New Point(0, 0), New Point(10, 5), New Point(0, 10) })
		Private Shared _rightArrow As New PointCollection(New Point() { New Point(10, 0), New Point(0, 5), New Point(10, 10) })

		' ctor
		Public Sub New(ByVal flex As C1FlexGrid)
			_flex = flex
			AddHandler _flex.MouseMove, AddressOf _flex_MouseMove
		End Sub

		' update HotRange when the mouse moves over the grid
		Private Sub _flex_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim ht = _flex.HitTest(e)
			HotRange = ht.CellRange
		End Sub

		' get/set HotRange
		Private Property HotRange() As CellRange
			Get
				Return _hotRange
			End Get
			Set(ByVal value As CellRange)
				If value <> _hotRange Then
					' invalidate old, update, invalidate new
                    _flex.Invalidate(Not _hotRange.IsSingleCell)
					_hotRange = value
                    _flex.Invalidate(Not _hotRange.IsSingleCell)
				End If
			End Set
		End Property

		' highlight hot cell using two red arrows
        Public Overrides Sub CreateCellContent(ByVal fgrid As C1.WPF.FlexGrid.C1FlexGrid, ByVal bdr As Border, ByVal range As C1.WPF.FlexGrid.CellRange)
            MyBase.CreateCellContent(fgrid, bdr, range)
            If HotRange.Contains(range) Then
                ' save original content
                Dim child = bdr.Child
                bdr.Child = Nothing

                ' create grid to hold content and two red arrows
                Dim g = New Grid()
                g.ColumnDefinitions.Add(New ColumnDefinition() With {.Width = New GridLength(1, GridUnitType.Auto)})
                g.ColumnDefinitions.Add(New ColumnDefinition())
                g.ColumnDefinitions.Add(New ColumnDefinition() With {.Width = New GridLength(1, GridUnitType.Auto)})

                ' set content
                child.SetValue(grid.ColumnProperty, 1)
                g.Children.Add(child)

                ' set left arrow
                Dim p1 = New Polygon()
                p1.Points = _leftArrow
                p1.Fill = _arrowBrush
                p1.VerticalAlignment = VerticalAlignment.Center
                g.Children.Add(p1)

                ' set right arrow
                Dim p2 = New Polygon()
                p2.Points = _rightArrow
                p2.Fill = _arrowBrush
                p2.VerticalAlignment = VerticalAlignment.Center
                p2.SetValue(grid.ColumnProperty, 2)
                g.Children.Add(p2)

                ' assign new content to cell
                bdr.Child = g
            End If
        End Sub
	End Class
End Namespace
