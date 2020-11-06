Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Interactive line chart with 50K data points")> _
  Partial Public Class LargeData
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            LineAreaOptions.SetOptimizationRadius(chart, 2)
            chart.Actions.Add(New ZoomAction())
            CreateChart()
        End Sub

        Private Sub CreateChart()
            chart.BeginUpdate()
            Dim cnt As Integer = 50000
            Dim x(cnt - 1) As Double
            Dim y(cnt - 1) As Double

            For i As Integer = 0 To cnt - 1
                x(i) = i
                y(i) = 10 * Math.Sin(0.001 * i) + Math.Cos(0.03 * i)
            Next i

            Dim ds As New XYDataSeries() With {.XValuesSource = x, .ValuesSource = y, .ConnectionStrokeThickness = 1}

            chart.Data.Children.Add(ds)
            chart.ChartType = ChartType.Line

            chart.View.AxisX.ScrollBar = New AxisScrollBar()
            'chart.View.AxisX.Scale = 0.1;

            chart.View.AxisY.MinScale = 1

            SetScale(0.25)
            chart.EndUpdate()
        End Sub

        Private Sub OriginalSize_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            SetScale(1.0)
        End Sub

        Private Sub ZoomIn_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            SetScale(chart.View.AxisX.Scale * 0.5)
        End Sub

        Private Sub ZoomOut_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            SetScale(chart.View.AxisX.Scale * 2)
        End Sub

        Private Sub SetScale(ByVal scale As Double)
            chart.View.AxisX.Scale = scale
            btn1_1.IsEnabled = scale <> 1
            btnZoomIn.IsEnabled = scale > 0.002
            btnZoomOut.IsEnabled = scale < 1
        End Sub
    End Class
End Namespace
