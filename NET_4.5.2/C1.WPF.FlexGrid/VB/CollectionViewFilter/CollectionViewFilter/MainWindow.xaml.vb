Imports System.Text

Namespace CollectionViewFilter
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private _filter As ViewFilter

		Public Sub New()
			InitializeComponent()

			' create regular view, attach ViewFilter
			Dim view = New ListCollectionView(Customer.GetCustomerList(200))
			_filter = New ViewFilter(view)

			' show the data
			_flex.ItemsSource = view
		End Sub

		' apply filter on button click or enter key
		Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_filter.FilterExpression = _txtFilter.Text
		End Sub
		Private Sub _txtFilter_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.Key = Key.Enter Then
				_filter.FilterExpression = _txtFilter.Text
			End If
		End Sub
	End Class
End Namespace
