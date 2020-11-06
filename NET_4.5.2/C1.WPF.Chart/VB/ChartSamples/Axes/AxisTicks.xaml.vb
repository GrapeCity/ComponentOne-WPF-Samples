Imports System.ComponentModel
Imports System.Collections
Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Axis ticks customization")> _
  Partial Public Class AxisTicks
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            Dim ds As XYDataSeries = [Function].CreateDataSeries(0, 5, 200, Function(val) Math.Cos(4 * val) * Math.Sin(val), Nothing)

            ds.ConnectionStrokeThickness = 3
            ds.ConnectionStroke = New SolidColorBrush(Colors.Green)
            chart.Data.Children.Add(ds)

            chart.View.AxisX.MajorTickHeight = 6
            chart.View.AxisX.MinorTickHeight = 4
            chart.View.AxisY.MajorTickHeight = 6
            chart.View.AxisY.MinorTickHeight = 4

            tex.DataContext = chart.View.AxisX
            tey.DataContext = chart.View.AxisY
        End Sub
    End Class
End Namespace
