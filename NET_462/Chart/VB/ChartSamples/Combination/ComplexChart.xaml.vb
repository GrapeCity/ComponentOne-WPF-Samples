Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Complex chart with different data deries")> _
  Partial Public Class ComplexChart
        Inherits UserControl
        Private rnd As New Random()
        Private brushes() As Brush = {New SolidColorBrush(Color.FromArgb(255, 21, 72, 123)), CreateBrush(Color.FromArgb(255, 21, 72, 123)), New SolidColorBrush(Color.FromArgb(255, 239, 239, 231)), CreateBrush(Color.FromArgb(255, 239, 239, 231)), New SolidColorBrush(Color.FromArgb(255, 74, 130, 189)), CreateBrush(Color.FromArgb(255, 74, 130, 189)), New SolidColorBrush(Color.FromArgb(255, 198, 80, 74)), CreateBrush(Color.FromArgb(255, 198, 80, 74)), New SolidColorBrush(Color.FromArgb(255, 156, 186, 90)), CreateBrush(Color.FromArgb(255, 156, 186, 90))}

        Public Sub New()
            InitializeComponent()

            chart.Data.ItemNames = New String() {"Cat.1", "Cat.2", "Cat.3", "Cat.4", "Cat.5"}
            CreateData()
        End Sub

        Private Sub CreateData()
            chart.Data.Children.Clear()

            For i As Integer = 0 To 9
                Dim ds As New DataSeries() With {.ValuesSource = CreateRandomArray(5)}
                ds.SymbolFill = brushes(i)

                BarColumnOptions.SetStackGroup(ds, i Mod 2)
                chart.Data.Children.Add(ds)
            Next i

            chart.Data.Children.Add(New DataSeries() With {.ChartType = ChartType.LineSymbols, .ValuesSource = CreateRandomArray(5), .ConnectionStrokeThickness = 5, .ConnectionStroke = New SolidColorBrush(Colors.Red)})
        End Sub

        Private Function CreateRandomArray(ByVal cnt As Integer) As Double()
            Dim vals(cnt - 1) As Double

            For i As Integer = 0 To cnt - 1
                vals(i) = rnd.Next(10, 100)
            Next i

            Return vals
        End Function

        Private Shared Function CreateBrush(ByVal clr As Color) As LinearGradientBrush
            Dim lgb As New LinearGradientBrush() With {.EndPoint = New Point(2, 2), .MappingMode = BrushMappingMode.Absolute, .SpreadMethod = GradientSpreadMethod.Reflect}
            lgb.GradientStops = New GradientStopCollection(New GradientStop() {New GradientStop() With {.Color = clr, .Offset = 0}, New GradientStop() With {.Color = Color.FromArgb(64, 255, 255, 255), .Offset = 1}})

            Return lgb
        End Function
    End Class
End Namespace
