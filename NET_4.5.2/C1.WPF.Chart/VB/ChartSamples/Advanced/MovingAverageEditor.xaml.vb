Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart.Extended
#Else
Imports C1.WPF.C1Chart.Extended
#End If

Namespace ChartSamples
  Partial Public Class MovingAverageEditor
	  Inherits UserControl
	Public Sub New()
	  InitializeComponent()
            AddHandler DataContextChanged, Function(s, e) AnonymousMethod1(s, e)
		
        End Sub
        Private Function AnonymousMethod1(ByVal s As Object, ByVal e As Object) As Object
            Dim ms As MovingAverage = TryCast(DataContext, MovingAverage)
            If ms IsNot Nothing Then
                nbPeriod.Value = ms.Period
            End If
            Return Nothing
        End Function

	Private Sub nbPeriod_ValueChanged(ByVal sender As Object, ByVal e As C1.WPF.PropertyChangedEventArgs(Of Double))
	  Dim ms As MovingAverage = TryCast(DataContext, MovingAverage)
	  If ms IsNot Nothing Then
		ms.Period = CInt(Fix(nbPeriod.Value))
		ms.Label = String.Format("MovAvg {0}", ms.Period)
	  End If
	End Sub
  End Class
End Namespace
