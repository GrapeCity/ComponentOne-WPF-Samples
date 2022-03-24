Imports System.Text
Imports C1.WPF.FlexGrid

Namespace ExcelGrid
	''' <summary>
	''' Interaction logic for Window1.xaml
	''' </summary>
	Partial Public Class Window1
		Inherits Window
		Public Sub New()
			InitializeComponent()

			' show Excel-style marquee
			_flex.ShowMarquee = True
			_flex.CursorBackground = Nothing
			_flex.GridLinesVisibility = GridLinesVisibility.All

			' use custom cell factory to generate Excel-style row and column headers
			' (columns labeled A, B, C, etc, rows labeled 1, 2, 3, etc)
			_flex.CellFactory = New ExcelCellFactory()

			' start with blue color scheme
			ColorSchemeManager.ApplyColorScheme(_flex, ColorScheme.Blue)

			' start in bound mode
			PopulateGrid(True)

			' give grid the focus when the app loads
			AddHandler Loaded, AddressOf MainPage_Loaded
		End Sub

		' give grid the focus when the app loads
		Private Sub MainPage_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_flex.Focus()
		End Sub

		' pass focus to grid when user hits enter on the header TextBox
		Private Sub TextBox_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.Key = Key.Enter Then
				_flex.Focus()
			End If
		End Sub

		' assign cell content from header TextBox
		Private Sub TextBox_LostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Try
				Dim sel = _flex.Selection
				_flex(sel.Row, sel.Column) = (CType(sender, TextBox)).Text
			Catch
			End Try
		End Sub

		' change color scheme (blue, silver, black)
		Private Sub ComboBox_ColorSchemeChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			If _flex IsNot Nothing Then
				Dim cs = CType((CType(sender, ComboBox)).SelectedIndex, ColorScheme)
				ColorSchemeManager.ApplyColorScheme(_flex, cs)
				_flex.Select(0, 0)
				_flex.Focus()
			End If
		End Sub

		' change data mode (bound, unbound)
		Private Sub ComboBox_DataModeChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			If _flex IsNot Nothing Then
				PopulateGrid((CType(sender, ComboBox)).SelectedIndex = 0)
				_flex.Select(0, 0)
				_flex.Focus()
			End If
		End Sub

		' populate the grid (bound or unbound)
		Private Sub PopulateGrid(ByVal bound As Boolean)
			If bound Then
				' set item source
				_flex.Columns.Clear()
				_flex.ItemsSource = Customer.GetCustomerList(250)

				' hide read-only "Country" column 
				Dim col = _flex.Columns("Country")
				col.Visible = False

				' map countryID column so it shows country names instead of their IDs
				Dim dct As New Dictionary(Of Integer, String)()
				For Each country In Customer.GetCountries()
					dct(dct.Count) = country
				Next country
				col = _flex.Columns("CountryID")
				col.ValueConverter = New ColumnValueConverter(dct)
				col.HorizontalAlignment = HorizontalAlignment.Left
				col.Width = New GridLength(120)

				' provide auto-complete lists for first and last name columns
				col = _flex.Columns("First")
				col.ValueConverter = New ColumnValueConverter(Customer.GetFirstNames(), False)
				col = _flex.Columns("Last")
				col.ValueConverter = New ColumnValueConverter(Customer.GetLastNames(), False)

				' make read-only columns look disabled
				Dim readOnlyBrush = New SolidColorBrush(Color.FromArgb(&He0, &He0, &He0, &He0))
				For Each c In _flex.Columns
					If c.PropertyInfo IsNot Nothing AndAlso (Not c.PropertyInfo.CanWrite) Then
						c.Background = readOnlyBrush
					End If
				Next c
			Else
				' remove all rows and columns
				_flex.ItemsSource = Nothing
				_flex.Rows.Clear()
				_flex.Columns.Clear()

				' create 256 columns and 1500 rows (unbound)
				Using _flex.Rows.DeferNotifications()
				Using _flex.Columns.DeferNotifications()
					Do While _flex.Columns.Count < 256
						_flex.Columns.Add(New Column())
					Loop
					Do While _flex.Rows.Count < 1500
						_flex.Rows.Add(New Row())
					Loop
				End Using
				End Using
				Dim col = _flex.RowHeaders.Columns(0)
				col.Width = New GridLength(35)
				col.AllowResizing = False
			End If
		End Sub
	End Class
End Namespace
