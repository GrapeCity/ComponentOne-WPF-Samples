Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
Imports C1.Silverlight.Chart.Extended
#Else
Imports C1.WPF.C1Chart
Imports C1.WPF.C1Chart.Extended
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Shows plotting of parametric function")> _
  Partial Public Class Parametric
        Inherits UserControl
        Private _pfs As ParametricFunctionSeries

        Public Sub New()
            InitializeComponent()

            _pfs = New ParametricFunctionSeries() With {.SampleCount = 1000, .Max = 2 * Math.PI, .XFunction = Function(t) Math.Cos(xPar.Value * t), .YFunction = Function(t) Math.Sin(yPar.Value * t)}
            chart.Data.Children.Add(_pfs)

            chart.ChartType = ChartType.Line
        End Sub

        Private Sub Par_ValueChanged(ByVal sender As Object, ByVal e As C1.WPF.PropertyChangedEventArgs(Of Double))
            If _pfs IsNot Nothing Then
                _pfs.UpdateData()
            End If
        End Sub
    End Class
End Namespace
