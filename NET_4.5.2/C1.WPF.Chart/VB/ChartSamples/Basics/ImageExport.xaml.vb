Imports System.Net
Imports System.Windows.Media.Animation


#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
Imports C1.Silverlight.Chart.Extended
#Else
Imports Microsoft.Win32
Imports C1.WPF.C1Chart
Imports C1.WPF.C1Chart.Extended
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Save chart as png/jpg image")> _
  Partial Public Class ImageExport
        Inherits UserControl
        Private coef() As String = {"AMTMNQQXUYGA", "CVQKGHQTPHTE", "FIRCDERRPVLD", "GIIETPIQRRUL", "GLXOESFTTPSV", "GXQSNSKEECTX"}
        Private rnd As New Random()

        Public Sub New()
            InitializeComponent()

            CreateChart()
        End Sub

        Private Sub CreateChart()
            chart.BeginUpdate()
            chart.Data.Children.Clear()
            Dim cnt As Integer = 400

            Dim x(cnt - 1) As Double
            Dim y(cnt - 1) As Double

            Dim c() As Double = StringToCoeff(coef(rnd.Next(coef.Length)))

            For i As Integer = 1 To cnt - 1
                Dim pt As Point = Iterate(x(i - 1), y(i - 1), c)
                x(i) = pt.X
                y(i) = pt.Y
            Next i

            chart.Data.Children.Add(New XYDataSeries() With {.XValuesSource = x, .ValuesSource = y, .SymbolSize = New Size(5, 5)})
            chart.ChartType = ChartType.XYPlot
            chart.EndUpdate()
        End Sub

        Private Function Iterate(ByVal x As Double, ByVal y As Double, ByVal c() As Double) As Point
            Dim x1 As Double = c(0) + c(1) * x + c(2) * x * x + c(3) * x * y + c(4) * y + c(5) * y * y
            Dim y1 As Double = c(6) + c(7) * x + c(8) * x * x + c(9) * x * y + c(10) * y + c(11) * y * y

            Return New Point(x1, y1)
        End Function

        Private Function StringToCoeff(ByVal s As String) As Double()
            Dim len As Integer = s.Length
            Dim c(len - 1) As Double

            For i As Integer = 0 To len - 1
                c(i) = -1.2 + 0.1 * (AscW(s.Chars(i)) - AscW("A"c))
            Next i

            Return c
        End Function

        Private Sub btnNew_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            CreateChart()
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim sfd As New SaveFileDialog()
            sfd.Filter = "Jpeg files (*.jpg)|*.jpg|Png files (*.png)|*.png"

            If sfd.ShowDialog() = True Then
                Using stream = sfd.OpenFile()
                    If sfd.SafeFileName.EndsWith(".jpg") Then
                        chart.SaveImage(stream, ImageFormat.Jpeg)
                    Else
                        chart.SaveImage(stream, ImageFormat.Png)
                    End If
                End Using
            End If
        End Sub
    End Class
End Namespace
