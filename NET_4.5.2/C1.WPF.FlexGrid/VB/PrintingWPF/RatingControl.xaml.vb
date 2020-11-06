Imports System.Net
Imports System.Windows.Media.Animation

Namespace Printing
	Partial Public Class RatingControl
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
			UpdateStars()
		End Sub

		Public Property Rating() As Integer
			Get
				Return CInt(Fix(GetValue(RatingProperty)))
			End Get
			Set(ByVal value As Integer)
				SetValue(RatingProperty, value)
			End Set
		End Property
		Private Sub UpdateStars()
			For i As Integer = 0 To _sp.Children.Count - 1
				_sp.Children(i).Opacity = If(Rating > i, 1, 0.1)
			Next i
		End Sub
		Public Shared ReadOnly RatingProperty As DependencyProperty = DependencyProperty.Register("Rating", GetType(Integer), GetType(RatingControl), New PropertyMetadata(0, AddressOf RatingControl.OnPropertyChanged))

		Private Shared Overloads Sub OnPropertyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			If e.Property Is RatingProperty Then
				Dim ctl As RatingControl = CType(d, RatingControl)
				ctl.UpdateStars()
			End If
		End Sub
	End Class
End Namespace
