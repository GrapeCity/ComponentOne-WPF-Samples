Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Aggregate chart")> _
  Partial Public Class AggregateChart
        Inherits UserControl
        Private _dict As New Dictionary(Of String, Brush)()

        Public Sub New()
            InitializeComponent()

            _dict("red") = CreateBrush(Colors.Red)
            _dict("blue") = CreateBrush(Color.FromArgb(255, 2, 150, 252))
            _dict("yellow") = CreateBrush(Colors.Yellow)
            _dict("white") = CreateBrush(Colors.White)


            cb.ItemsSource = Utils.GetEnumValues(GetType(Aggregate))
            cb.SelectedIndex = 0
            AddHandler cb.SelectionChanged, Function(s, e) AnonymousMethod2(s, e)

            Dim cnt As Integer = 30
            chart.Data.ItemNames = CreateRandomStrings(cnt, New String() {"blue", "red", "white", "yellow"})


            Dim vals = CreateRandomValues(cnt)

            Dim ds = New DataSeries() With {.ValuesSource = vals}

            AddHandler ds.PlotElementLoaded, Function(s, e) AnonymousMethod1(s, e)

            chart.Data.Children.Add(ds)
            chart.View.AxisX.AnnoTemplate = chart.Resources("al_tmpl")
            BarColumnOptions.SetRadiusX(chart, 4)
            BarColumnOptions.SetRadiusY(chart, 4)
        End Sub
        Private Function AnonymousMethod1(ByVal s As Object, ByVal e As Object) As Object
            Dim pe As PlotElement = CType(s, PlotElement)
            If _dict.ContainsKey(pe.DataPoint.Name) Then
                pe.Fill = _dict(pe.DataPoint.Name)
            End If
            pe.StrokeThickness = 0
            Return Nothing
        End Function

        Private Function AnonymousMethod2(ByVal s As Object, ByVal e As Object) As Object
            chart.Aggregate = CType(cb.SelectedIndex, Aggregate)
            Return Nothing
        End Function

        Private rnd As New Random()

        Private Function CreateRandomValues(ByVal cnt As Integer) As Double()
            Dim vals(cnt - 1) As Double
            For i As Integer = 0 To cnt - 1
                vals(i) = rnd.NextDouble() * 100
            Next i
            Return vals
        End Function

        Private Function CreateRandomStrings(ByVal cnt As Integer, ByVal list() As String) As String()
            Dim vals(cnt - 1) As String
            For i As Integer = 0 To cnt - 1
                vals(i) = list(rnd.Next(0, list.Length))
            Next i
            Return vals
        End Function

        Private Function CreateBrush(ByVal clr As Color) As Brush

            Return New LinearGradientBrush() With {.GradientStops = New GradientStopCollection(New GradientStop() {New GradientStop() With {.Color = Color.FromArgb(128, clr.R, clr.G, clr.B), .Offset = 0}, New GradientStop() With {.Color = clr, .Offset = 0.2}, New GradientStop() With {.Color = clr, .Offset = 0.7}, New GradientStop() With {.Color = Color.FromArgb(128, clr.R, clr.G, clr.B), .Offset = 1}}), .StartPoint = New Point(0, 0), .EndPoint = New Point(1, 1)}

        End Function
    End Class

  Public Class AxisLabelConverter
	  Implements IValueConverter
	#Region "IValueConverter Members"

	Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
	  If value IsNot Nothing Then
		Return String.Format("../../Resources/flower_{0}.png", value.ToString())
	  End If

	  Return value
	End Function

	Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
	  Throw New NotImplementedException()
	End Function

	#End Region
  End Class

End Namespace
