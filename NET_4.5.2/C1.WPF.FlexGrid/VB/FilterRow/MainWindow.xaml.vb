Imports System.Text
Imports System.Reflection


Namespace FilterRow
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
        Inherits Window

		' set this to true to show filtering in unbound mode
        Private Const UNBOUND As Boolean = True

		Public Sub New()
			InitializeComponent()

			' populate the grid
			If UNBOUND Then
				' add columns
				For Each pi As PropertyInfo In GetType(Product).GetProperties()
					Dim c = New C1.WPF.FlexGrid.Column()
					c.ColumnName = pi.Name
					c.DataType = pi.PropertyType
					_flex.Columns.Add(c)
				Next pi

				' add rows
				For Each item In Product.GetProducts(100)
					Dim r = New C1.WPF.FlexGrid.Row()
					_flex.Rows.Add(r)
					For Each pi As PropertyInfo In GetType(Product).GetProperties()
						r(pi.Name) = pi.GetValue(item, Nothing)
					Next pi
				Next item
			Else
				_flex.ItemsSource = Product.GetProducts(100)
			End If

			' create filter row
			_flex.CellFactory = New FilterRowCellFactory()
		End Sub

		Private Sub CheckBox_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim cf = TryCast(_flex.CellFactory, FilterRowCellFactory)
			cf.UseRegEx = (CType(sender, CheckBox)).IsChecked.Value
		End Sub
	End Class
End Namespace
