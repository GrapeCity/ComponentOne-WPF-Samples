Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Controls the axis position with its origin")> _
  Partial Public Class AxisOrigin
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            chart.View.AxisX.Position = AxisPosition.OverData
            chart.View.AxisY.Position = AxisPosition.OverData

            chart.View.AxisX.MajorGridFill = New SolidColorBrush(Colors.LightGray) With {.Opacity = 0.1}
            chart.View.AxisY.MajorGridFill = New SolidColorBrush(Colors.LightGray) With {.Opacity = 0.1}

            Dim ds As XYDataSeries = [Function].CreateDataSeries(0, Math.PI, 200, Function(val) Math.Cos(7 * val) * Math.Cos(val), Function(val) Math.Cos(7 * val) * Math.Sin(val), Nothing)
            ds.ConnectionStrokeThickness = 4
            ds.ConnectionStroke = New SolidColorBrush(Colors.Red)
            chart.Data.Children.Add(ds)
        End Sub

        Private Sub vs_ValueChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Double))
            If chart IsNot Nothing Then
                chart.View.AxisX.Origin = 1 - 2 * 0.01 * vs.Value
            End If
        End Sub

        Private Sub hs_ValueChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Double))
            If chart IsNot Nothing Then
                chart.View.AxisY.Origin = -1 + 2 * 0.01 * hs.Value
            End If
        End Sub
    End Class
End Namespace
