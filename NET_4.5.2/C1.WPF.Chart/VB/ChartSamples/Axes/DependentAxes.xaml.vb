Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Multiple Y-axes with different units of measurement")> _
  Partial Public Class DependentAxes
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            chart.Data.Children.Add(CreateTestData(New Date(2007, 3, 1), 50))
            chart.View.AxisX.IsTime = True

            CreateTitle(chart.View.AxisY, "°C", Nothing)

            Dim axf As New Axis() With {.AxisType = AxisType.Y, .IsDependent = True, .Foreground = New SolidColorBrush(Colors.Red), .DependentAxisConverter = Function(x) x * 9 \ 5 + 32}
            CreateTitle(axf, "°F", New SolidColorBrush(Colors.Red))
            chart.View.Axes.Add(axf)

            Dim axk As New Axis() With {.IsDependent = True, .Foreground = New SolidColorBrush(Colors.Purple), .DependentAxisConverter = Function(x) x + 273.15}
            CreateTitle(axk, "K", New SolidColorBrush(Colors.Purple))
            chart.View.Axes.Add(axk)
        End Sub

        Private rnd As New Random()

        Private Function CreateTestData(ByVal start As Date, ByVal cnt As Integer) As XYDataSeries
            Dim x(cnt - 1) As Date
            Dim y(cnt - 1) As Double

            For i As Integer = 0 To cnt - 1
                x(i) = start.AddDays(i)
                y(i) = -10 + 20 * rnd.NextDouble()
            Next i

            Return New XYDataSeries() With {.XValuesSource = x, .ValuesSource = y}
        End Function

        Private Sub CreateTitle(ByVal ax As Axis, ByVal text As String, ByVal fg As SolidColorBrush)
            Dim tb As New TextBlock() With {.TextAlignment = TextAlignment.Right, .Text = text, .HorizontalAlignment = HorizontalAlignment.Center, .VerticalAlignment = VerticalAlignment.Bottom, .Width = 20, .RenderTransform = New RotateTransform() With {.Angle = 90}, .RenderTransformOrigin = New Point(0.5, 0.5)}

            If fg IsNot Nothing Then
                tb.Foreground = fg
            End If

            ax.Title = New Border() With {.HorizontalAlignment = HorizontalAlignment.Left, .VerticalAlignment = VerticalAlignment.Top, .Child = tb}
        End Sub
    End Class
End Namespace
