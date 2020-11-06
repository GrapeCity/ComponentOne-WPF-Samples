Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Shows various options of logarithmic chart axes")> _
  Partial Public Class LogAxes
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            templ.Visibility = Visibility.Collapsed

            AddHandler templ.Checked, Function(s, a) AnonymousMethod1(s, a)
            AddHandler templ.Unchecked, Function(s, a) AnonymousMethod2(s, a)
            
            xbase.ItemsSource = New String() {"None", "e", "10"}
            xbase.Tag = chart.View.AxisX
            AddHandler xbase.SelectionChanged, AddressOf base_SelectionChanged
            xbase.SelectedIndex = 2

            ybase.ItemsSource = New String() {"None", "e", "10"}
            ybase.Tag = chart.View.AxisY
            AddHandler ybase.SelectionChanged, AddressOf base_SelectionChanged
            ybase.SelectedIndex = 2

            templ.IsChecked = True

            chart.ChartType = ChartType.LineSmoothed

            ' create data series
            chart.Data.Children.Add([Function].CreateDataSeries(0.01, 10, 100, Function(val) Math.Log10(val), "y(x)=log(x)"))
            chart.Data.Children.Add([Function].CreateDataSeries(0.01, 10, 100, Function(val) val, "y(x)=x"))
            chart.Data.Children.Add([Function].CreateDataSeries(0.01, 1, 100, Function(val) Math.Pow(10, val), "y(x)=10^x"))

            ' minor grid
            chart.View.AxisX.MinorUnit = 1
            chart.View.AxisX.MinorGridStroke = New SolidColorBrush(Colors.LightGray)
            chart.View.AxisX.MinorGridStrokeThickness = 0.5

            ' major grid
            chart.View.AxisX.MajorGridStroke = New SolidColorBrush(Colors.DarkGray)
            chart.View.AxisX.MajorGridStrokeDashes = Nothing

            chart.View.AxisY.MajorGridStroke = New SolidColorBrush(Colors.DarkGray)
            chart.View.AxisY.MajorGridStrokeDashes = Nothing
        End Sub

        Private Function AnonymousMethod1(ByVal s As Object, ByVal a As Object) As Object
            UpdateTemplate(chart.View.AxisX)
            UpdateTemplate(chart.View.AxisY)
            Return Nothing
        End Function

        Private Function AnonymousMethod2(ByVal s As Object, ByVal a As Object) As Object
            UpdateTemplate(chart.View.AxisX)
            UpdateTemplate(chart.View.AxisY)
            Return Nothing
        End Function

        Private Sub base_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            Dim cb As ComboBox = CType(sender, ComboBox)
            Dim ax As Axis = TryCast(cb.Tag, Axis)
            If ax IsNot Nothing Then
                Select Case cb.SelectedIndex
                    Case 0
                        ax.LogBase = Double.NaN
                    Case 1
                        ax.LogBase = Math.E
                    Case 2
                        ax.LogBase = 10
                End Select

                UpdateTemplate(ax)
            End If
        End Sub

        Private Sub UpdateTemplate(ByVal ax As Axis)
            If (Not Double.IsNaN(ax.LogBase)) AndAlso templ.IsChecked = True Then
                ax.AnnoTemplate = Resources("ax")
            Else
                ax.AnnoTemplate = Nothing
            End If

            If Double.IsNaN(chart.View.AxisX.LogBase) AndAlso Double.IsNaN(chart.View.AxisY.LogBase) Then
                templ.Visibility = Visibility.Collapsed
            Else
                templ.Visibility = Visibility.Visible
            End If
        End Sub
    End Class

  Public NotInheritable Class [Function]
	Private Sub New()
	End Sub
	Public Shared Function CreateDataSeries(ByVal xmin As Double, ByVal xmax As Double, ByVal npts As Integer, ByVal func As Func(Of Double, Double), ByVal label As String) As XYDataSeries
	  Dim x(npts - 1) As Double
	  Dim y(npts - 1) As Double

	  For i As Integer = 0 To npts - 1
		x(i) = xmin + (xmax - xmin) * i / (npts - 1)
		y(i) = func(x(i))
	  Next i

	  Return New XYDataSeries() With {.ValuesSource = y, .XValuesSource = x, .Label = label}
	End Function

	Public Shared Function CreateDataSeries(ByVal tmin As Double, ByVal tmax As Double, ByVal npts As Integer, ByVal xfunc As Func(Of Double, Double), ByVal yfunc As Func(Of Double, Double), ByVal label As String) As XYDataSeries
	  Dim x(npts - 1) As Double
	  Dim y(npts - 1) As Double

	  For i As Integer = 0 To npts - 1
		Dim t As Double = tmin + (tmax - tmin) * i / (npts - 1)
		x(i) = xfunc(t)
		y(i) = yfunc(t)
	  Next i

	  Return New XYDataSeries() With {.ValuesSource = y, .XValuesSource = x, .Label = label}
	End Function
  End Class
End Namespace
