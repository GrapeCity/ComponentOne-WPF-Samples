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
    <System.ComponentModel.Description("Shows plotting of simple Y(X)-function")> _
  Partial Public Class Functions
        Inherits UserControl
        Private ds As YFunctionSeries

        Public Sub New()
            InitializeComponent()

            chart.ChartType = ChartType.Line
            tbCode.Text = "Math.sin(4*val)*Math.cos(3*val)"
#If CLR40 Then
      chart.Data.Children.Add(New YFunctionSeries() With {.Function = AddressOf Sample0, .Min = -10, .Max = 10, .SampleCount = 300})
            btnSample1.Visibility = Visibility.Collapsed
            btnSample2.Visibility = Visibility.Collapsed
            btnApply.Visibility = Visibility.Collapsed
            tbCode.IsReadOnly = True
            tbText.Visibility = Visibility.Collapsed
#Else
            chart.Data.Children.Add(New YFunctionSeries() With {.FunctionCode = tbCode.Text, .Min = -10, .Max = 10, .SampleCount = 300})
#End If

            AddHandler tbCode.TextChanged, Function(s, e) AnonymousMethod1(s, e)
        End Sub

        Private Function AnonymousMethod1(ByVal s As Object, ByVal e As Object) As Object
            m_error.Visibility = Visibility.Collapsed
            Return Nothing
        End Function

    Private Function Sample0(ByVal val As Double) As Double
      Return Math.Sin(4 * val) * Math.Cos(3 * val)
    End Function


        Private Sub btnApply_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)

#If Not CLR40 Then
            chart.Data.Children.Clear()

            ds = New YFunctionSeries() With {.Min = -10, .Max = 10, .FunctionCode = tbCode.Text, .SampleCount = 300}

            If Not String.IsNullOrEmpty(ds.FunctionCodeError) Then
                m_error.Visibility = Visibility.Visible
                ToolTipService.SetToolTip(m_error, ds.FunctionCodeError)
            End If

            chart.Data.Children.Add(ds)
#End If

        End Sub

        Private Sub btnSample1_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            tbCode.Text = "Math.sin(val)"
        End Sub

        Private Sub btnSample2_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            tbCode.Text = "if (val > 0) return val * val; else return -val * val;"
        End Sub
    End Class
End Namespace
