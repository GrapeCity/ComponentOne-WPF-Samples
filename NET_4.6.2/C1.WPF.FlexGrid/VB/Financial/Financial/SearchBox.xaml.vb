Imports System.Reflection
Imports System.ComponentModel
Imports System.Net
Imports System.Windows.Media.Animation

Namespace Financial
	Partial Public Class SearchBox
		Inherits UserControl
		Private _propertyInfo As New List(Of PropertyInfo)()
		Private _timer As New C1.Util.Timer()

		Public Sub New()
			InitializeComponent()
			_timer.Interval = TimeSpan.FromMilliseconds(800)
			AddHandler _timer.Tick, AddressOf _timer_Tick
		End Sub
		Public Property View() As ICollectionView
		Public ReadOnly Property FilterProperties() As List(Of PropertyInfo)
			Get
				Return _propertyInfo
			End Get
		End Property
		Private Sub _txtSearch_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			If _imgClear IsNot Nothing Then
				' update clear button visibility
				_imgClear.Visibility = If(String.IsNullOrEmpty(_txtSearch.Text), Visibility.Collapsed, Visibility.Visible)

				' start ticking
				_timer.Stop()
				_timer.Start()
			End If
		End Sub
		Private Sub _imgClear_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			_txtSearch.Text = String.Empty
		End Sub
		Private Sub _timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
			_timer.Stop()
			If View IsNot Nothing AndAlso _propertyInfo.Count > 0 Then
				View.Filter = Nothing
				View.Filter = Function(item As Object)
					' get search text
					Dim srch = _txtSearch.Text

					' no text? show all items
					If String.IsNullOrEmpty(srch) Then
						Return True
					End If

					' show items that contain the text in any of the specified properties
					For Each pi As PropertyInfo In _propertyInfo
						Dim value = TryCast(pi.GetValue(item, Nothing), String)
						If value IsNot Nothing AndAlso value.IndexOf(srch, StringComparison.OrdinalIgnoreCase) > -1 Then
							Return True
						End If
					Next pi

					' exclude this item...
					Return False
				End Function
			End If
		End Sub
	End Class
End Namespace
