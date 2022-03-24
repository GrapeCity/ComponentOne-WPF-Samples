Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
  Partial Public Class TickEditor
	  Inherits UserControl
	Public Sub New()
	  InitializeComponent()
	End Sub

	Private Sub nbTickHeight_ValueChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs(Of Double))
	  Dim ax As Axis = TryCast(DataContext, Axis)
	  If ax IsNot Nothing Then
		ax.MajorTickHeight = nbMajHeight.Value
		ax.MajorTickOverlap = nbMajOverlap.Value

		ax.MinorTickHeight = nbMinHeight.Value
		ax.MinorTickOverlap = nbMinOverlap.Value
	  End If
	End Sub
  End Class
End Namespace
