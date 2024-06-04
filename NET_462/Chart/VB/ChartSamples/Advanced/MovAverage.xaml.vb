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
    <System.ComponentModel.Description("Shows moving average trend")> _
  Partial Public Class MovAverage
        Inherits UserControl
        Private rnd As New Random()

        Public Sub New()
            InitializeComponent()

            Dim cnt As Integer = 200

            Dim x(cnt - 1) As Double
            Dim y(cnt - 1) As Double

            For i As Integer = 0 To cnt - 1
                x(i) = i
                y(i) = rnd.Next(100)
            Next i

            chart.Data.Children.Add(New XYDataSeries() With {.XValuesSource = x, .ValuesSource = y, .Label = "RawData", .ConnectionStrokeThickness = 1, .ConnectionStroke = New SolidColorBrush(Colors.LightGray)})

            chart.Data.Children.Add(New MovingAverage() With {.XValuesSource = x, .ValuesSource = y, .Label = "MovAvg 10", .Period = 10, .ConnectionStroke = New SolidColorBrush(Colors.Green)})

            chart.Data.Children.Add(New MovingAverage() With {.XValuesSource = x, .ValuesSource = y, .Label = "MovAvg 40", .Period = 40, .ConnectionStroke = New SolidColorBrush(Colors.Red)})

            chart.ChartType = ChartType.Line

            maEditor1.DataContext = chart.Data.Children(1)
            maEditor2.DataContext = chart.Data.Children(2)
        End Sub
    End Class
End Namespace
