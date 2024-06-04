Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart.Extended
#Else
Imports C1.WPF.C1Chart.Extended
#End If

Namespace ChartSamples
  Partial Public Class TrendLineEditor
	  Inherits UserControl
	Public Sub New()
	  InitializeComponent()

	  cbFit.ItemsSource = Utils.GetEnumValues(GetType(FitType))
	  cbFit.SelectedIndex = 0

            AddHandler DataContextChanged, Function(s, e) AnonymousMethod1(s, e)
		  
        End Sub
        Private Function AnonymousMethod1(ByVal s As Object, ByVal e As Object) As Object
            Dim tl As TrendLine = TryCast(DataContext, TrendLine)
            If tl IsNot Nothing Then
                cbFit.SelectedIndex = CInt(tl.FitType)
                nbOrder.Value = tl.Order
                If tl.Label Is Nothing Then
                    tl.Label = CreateLabel(tl)
                End If
            End If
            Return Nothing
        End Function

	Private Sub cbFit_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
	  Dim tl As TrendLine = TryCast(DataContext, TrendLine)
	  If tl IsNot Nothing Then
		tl.FitType = CType(cbFit.SelectedIndex, FitType)
		tl.Label = CreateLabel(tl)
		UpdateControls()
	  End If
	End Sub

	Private Sub nbOrder_ValueChanged(ByVal sender As Object, ByVal e As C1.WPF.PropertyChangedEventArgs(Of Double))
	  Dim tl As TrendLine = TryCast(DataContext, TrendLine)
	  If tl IsNot Nothing Then
		tl.Order = CInt(Fix(nbOrder.Value))
		tl.Label = CreateLabel(tl)
	  End If
	End Sub

	Private Function CreateLabel(ByVal tl As TrendLine) As String
	  If tl.FitType = FitType.Polynom OrElse tl.FitType = FitType.Fourier Then
		Return String.Format("{0} {1}", tl.FitType, tl.Order)
	  Else
		Return String.Format("{0}", tl.FitType)
	  End If
	End Function

	Private Sub UpdateControls()
	  Dim tl As TrendLine = TryCast(DataContext, TrendLine)
	  If tl IsNot Nothing Then
		If tl.FitType = FitType.Polynom OrElse tl.FitType = FitType.Fourier Then
		  hOrder.Visibility = Visibility.Visible
		Else
		  hOrder.Visibility = Visibility.Collapsed
		End If
	  End If
	End Sub
  End Class
End Namespace
