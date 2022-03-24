Imports System.Text
Imports System.Windows.Media.Animation

Namespace CustomTypeDescriptor
	''' <summary>
	''' Interaction logic for StockTicker.xaml
	''' </summary>
	Partial Public Class StockTicker
		Inherits UserControl
		Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Double), GetType(StockTicker), New PropertyMetadata(0.0, AddressOf ValueChanged))

		Private _format As String = "n2"
		Private _flash As Storyboard
		Private _bindingSource As String
		Private _firstTime As Boolean = True

		Private Shared _brNegative As Brush = New SolidColorBrush(Colors.Red)
		Private Shared _brPositive As Brush = New SolidColorBrush(Colors.Green)
		Private Shared _clrNegative As Color = Color.FromArgb(100, &Hff, 0, 0)
		Private Shared _clrPositive As Color = Color.FromArgb(100, 0, &Hff, 0)

		Public Sub New()
			InitializeComponent()
			_arrow.Fill = Nothing
			_flash = CType(Resources("_sbFlash"), Storyboard)
		End Sub
		Public Property Value() As Double
			Get
				Return CDbl(GetValue(ValueProperty))
			End Get
			Set(ByVal value As Double)
				SetValue(ValueProperty, value)
			End Set
		End Property
		Private Shared Sub ValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			Dim ticker = TryCast(d, StockTicker)
			Dim value = CDbl(e.NewValue)
			Dim oldValue = CDbl(e.OldValue)

			' resetting
			If Double.IsNaN(value) Then
				ticker._flash.Stop()
				ticker._root.Background = New SolidColorBrush(Colors.Transparent)
				ticker._arrow.Fill = Nothing
				ticker._txtChange.Foreground = ticker._txtValue.Foreground
				Return
			End If

			' get historical data
			Dim data = TryCast(ticker.Tag, FinancialData)
			Dim list = data.GetHistory(ticker.BindingSource)
			If list IsNot Nothing AndAlso list.Count > 1 Then
				oldValue = CDbl(list(list.Count - 2))
			End If

			' calculate percentage change
			Dim change = If(oldValue = 0 OrElse Double.IsNaN(oldValue), 0, (value - oldValue) / oldValue)

			' update text
			ticker._txtValue.Text = value.ToString(ticker._format)
			ticker._txtChange.Text = String.Format("({0:0.0}%)", change * 100)

			' update flash color
			Dim ca = TryCast(ticker._flash.Children(0), ColorAnimation)

			' update symbol
			If change = 0 Then
				ticker._arrow.Fill = Nothing
				ticker._txtChange.Foreground = ticker._txtValue.Foreground
			ElseIf change < 0 Then
				ticker._stArrow.ScaleY = -1
				ticker._arrow.Fill = _brNegative
				ticker._txtChange.Foreground = ticker._arrow.Fill
				ca.From = _clrNegative
			Else
				ticker._stArrow.ScaleY = +1
				ticker._arrow.Fill = _brPositive
				ticker._txtChange.Foreground = ticker._arrow.Fill
				ca.From = _clrPositive
			End If

			' update sparkline
			If list IsNot Nothing Then
				Dim points = ticker._sparkLine.Points
				points.Clear()
				For x As Integer = 0 To list.Count - 1
					points.Add(New Point(x, CDbl(list(x))))
				Next x
			End If

			' flash new value (but not right after the control was created)
			If (Not ticker._firstTime) AndAlso change <> 0 Then
				ticker._flash.Begin()
			End If
			ticker._firstTime = False
		End Sub
		Public Property BindingSource() As String
			Get
				Return _bindingSource
			End Get
			Set(ByVal value As String)
				_bindingSource = value
			End Set
		End Property
		Public Property Format() As String
			Get
				Return _format
			End Get
			Set(ByVal value As String)
				_format = value
				_txtValue.Text = Me.Value.ToString(_format)
			End Set
		End Property
	End Class
End Namespace
