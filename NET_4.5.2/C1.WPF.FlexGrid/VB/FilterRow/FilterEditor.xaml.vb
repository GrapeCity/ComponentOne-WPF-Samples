Imports System.Text
Imports System.ComponentModel
Imports C1.WPF.FlexGrid

Namespace FilterRow
	''' <summary>
	''' Interaction logic for FilterEditor.xaml
	''' </summary>
	Partial Public Class FilterEditor
		Inherits UserControl
		' ** fields
		Private _grid As C1FlexGrid
		Private _row As GridFilterRow
		Private _col As Column
		Private _readOnly As Boolean
		Private _kaTab As KeyAction

		' ** ctor
		Public Sub New()
			InitializeComponent()
		End Sub
		Public Sub New(ByVal row As GridFilterRow, ByVal col As Column)
			InitializeComponent()

			' store grid parameters
			_grid = row.Grid
			_row = row
			_col = col

			' initialize editor content from values stored in the row
			' (the editor is transient and can't be used for storage)
			Dim filterArgument = _row.GetFilterArgument(_col)

			If _col.DataType Is GetType(Boolean) Then
				' show checkbox for Boolean values
				_cbValue.Visibility = Visibility.Visible
				_tbValue.Visibility = Visibility.Hidden

				' initialize editors
				Dim cb As Boolean
                _cbValue.IsChecked = If(Boolean.TryParse(filterArgument, cb), CType(cb, System.Nullable(Of Boolean)), Nothing)
                _tbValue.Text = Nothing
			Else
				' show TextBox for non-Boolean values
				_cbValue.Visibility = Visibility.Hidden
				_tbValue.Visibility = Visibility.Visible

				' initialize editors
				_tbValue.Text = _row.GetFilterArgument(_col)
				_cbValue.IsChecked = Nothing
			End If

			' show filter image if the filter is active
			UpdateFilterImage()
		End Sub

		' ** event handlers

		' text box
		Private Sub _tbValue_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' make grid read-only while editing the filter
			_readOnly = _grid.IsReadOnly
			_grid.IsReadOnly = True

			' make tabs cycle through filter controls
			_kaTab = _grid.KeyActionTab
			_grid.KeyActionTab = KeyAction.Default
		End Sub
		Private Sub _tbValue_LostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' restore read-only/KeyActionTab state
			_grid.IsReadOnly = _readOnly
			_grid.KeyActionTab = _kaTab

			' apply the new filter condition
			_row.SetFilterArgument(_col, _tbValue.Text)
		End Sub
		Private Sub _tbValue_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.Key = Key.Enter Then
				_col.Grid.Focus()
			End If
		End Sub
		Private Sub _tbValue_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			UpdateFilterImage()
		End Sub

		' check box
		Private Sub _cbValue_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' make grid read-only while editing the filter
			_readOnly = _grid.IsReadOnly
			_grid.IsReadOnly = True
		End Sub
		Private Sub _cbValue_LostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' restore read-only state
			_grid.IsReadOnly = _readOnly
		End Sub
		Private Sub _cbValue_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' apply the new filter condition
			_row.SetFilterArgument(_col, _cbValue.IsChecked.ToString())

			' update filter visibility
			UpdateFilterImage()
		End Sub

		' filter image
		Private Sub _imgClear_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			' clear filter value
			_tbValue.Text = String.Empty
			_cbValue.IsChecked = Nothing

			' clear the filter condition
			_row.SetFilterArgument(_col, String.Empty)

			' done with the mouse
			e.Handled = True
		End Sub
		Private Sub _imgClear_MouseEnter(ByVal sender As Object, ByVal e As MouseEventArgs)
			CType(sender, Image).Opacity = 1
		End Sub
		Private Sub _imgClear_MouseLeave(ByVal sender As Object, ByVal e As MouseEventArgs)
			CType(sender, Image).Opacity = 0.4
		End Sub

		' ** implementation
		Private Sub UpdateFilter()
			' store filter value in row
			' (the editor is transient and can't be used for storage)
			_row.SetFilterArgument(_col, _tbValue.Text)
		End Sub
		Private Sub UpdateFilterImage()
			' show filter image if we are currently active
			_imgClear.Visibility = If(String.IsNullOrEmpty(_tbValue.Text) AndAlso (Not _cbValue.IsChecked.HasValue), Visibility.Collapsed, Visibility.Visible)
		End Sub
	End Class
End Namespace
