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
    <System.ComponentModel.Description("Shows trend lines with its options")> _
  Partial Public Class TrendLines
        Inherits UserControl
        Private rnd As New Random()
        Private x(), y() As Double

        Public Sub New()
            InitializeComponent()

            Dim cnt As Integer = 10
            x = New Double(cnt - 1) {}
            y = New Double(cnt - 1) {}

            For i As Integer = 0 To cnt - 1
                x(i) = i + 1
                y(i) = rnd.Next(1, 100)
            Next i

            chart.Data.Children.Add(New XYDataSeries() With {.XValuesSource = x, .ValuesSource = y, .Label = "points"})

            chart.View.AxisX.Min = 0
            chart.View.AxisX.Max = cnt + 1
            chart.View.AxisY.Min = 0
            chart.View.AxisY.Max = 100
            chart.ChartType = ChartType.XYPlot

            AddTrendLine()
        End Sub

        Private Sub AddTrendLine()
            Dim tl As New TrendLine() With {.XValuesSource = x, .ValuesSource = y, .Order = 4}

            chart.Data.Children.Add(tl)

            Dim tle As New TrendLineEditor() With {.Margin = New Thickness(4)}
            tle.DataContext = tl
            sp.Children.Add(tle)
        End Sub
    End Class
End Namespace
