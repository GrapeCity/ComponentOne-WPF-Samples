Imports System.Collections.ObjectModel
Imports System.Net
Imports System.Windows.Media.Animation
Imports System.Windows.Threading

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
Imports C1.Silverlight.Chart.Extended
#Else
Imports C1.WPF.C1Chart
Imports C1.WPF.C1Chart.Extended
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Live line chart with automatic calculation of min/max/avarage values")> _
  Partial Public Class Dynamic
        Inherits UserControl
        Private _pts As New ObservableCollection(Of Point)()
        Private counter As Integer = 0
        Private rnd As New Random()
        Private dt As DispatcherTimer
        Private nMaxPoints As Integer = 60
        Private nAddPoints As Integer = 1
        Private _tlmin, _tlmax, _tlavg As TrendLine

        Public Sub New()
            InitializeComponent()

            chart.Data.ItemsSource = _pts
            chart.ChartType = ChartType.Line

            Dim ds As New XYDataSeries() With {.XValueBinding = New Binding("X"), .ValueBinding = New Binding("Y"), .ConnectionStrokeThickness = 1, .Label = "raw", .ConnectionStroke = New SolidColorBrush(Colors.DarkGray)}

            chart.Data.Children.Add(ds)

            _tlmin = New TrendLine() With {.FitType = FitType.MinY, .XValueBinding = New Binding("X"), .ValueBinding = New Binding("Y"), .Label = "min", .ConnectionStroke = New SolidColorBrush(Colors.Blue)}

            _tlmax = New TrendLine() With {.FitType = FitType.MaxY, .XValueBinding = New Binding("X"), .ValueBinding = New Binding("Y"), .Label = "max", .ConnectionStroke = New SolidColorBrush(Colors.Red)}

            _tlavg = New TrendLine() With {.FitType = FitType.AverageY, .XValueBinding = New Binding("X"), .ValueBinding = New Binding("Y"), .Label = "avg", .ConnectionStroke = New SolidColorBrush(Colors.Green)}

            chart.View.AxisY.Min = -1000
            chart.View.AxisY.Max = 1000

            dt = New DispatcherTimer() With {.Interval = TimeSpan.FromSeconds(0.2)}
            AddHandler dt.Tick, AddressOf Update
            dt.Start()
        End Sub

        Private Sub Update()
            chart.BeginUpdate()

            Dim cnt As Integer = nAddPoints
            For i As Integer = 0 To cnt - 1
                Dim r As Double = rnd.NextDouble()
                Dim y As Double = (10 * r * Math.Sin(0.1 * counter) * Math.Sin(0.6 * rnd.NextDouble() * counter))
                _pts.Add(New Point(counter, y * 100))
                counter += 1
            Next i

            Dim ndel As Integer = _pts.Count - nMaxPoints
            If ndel > 0 Then
                For i As Integer = 0 To ndel - 1
                    _pts.RemoveAt(0)
                Next i
            End If

            chart.EndUpdate()
        End Sub

        Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim btn As Button = CType(sender, Button)

            If dt.IsEnabled Then
                dt.Stop()
                btn.Content = "Start"
            Else
                dt.Start()
                btn.Content = "Stop"
            End If
        End Sub

        Friend Sub StopTimer()
            If dt.IsEnabled Then
                dt.Stop()
                btnTimer.Content = "Start"
            End If
        End Sub

        Private Sub CheckBox_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim cb As CheckBox = CType(sender, CheckBox)
            If TryCast(cb.Content, String) = "Minimum" Then
                chart.Data.Children.Add(_tlmin)
            ElseIf TryCast(cb.Content, String) = "Maximum" Then
                chart.Data.Children.Add(_tlmax)
            ElseIf TryCast(cb.Content, String) = "Average" Then
                chart.Data.Children.Add(_tlavg)
            End If
        End Sub

        Private Sub CheckBox_Unchecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim cb As CheckBox = CType(sender, CheckBox)
            If TryCast(cb.Content, String) = "Minimum" Then
                chart.Data.Children.Remove(_tlmin)
            ElseIf TryCast(cb.Content, String) = "Maximum" Then
                chart.Data.Children.Remove(_tlmax)
            ElseIf TryCast(cb.Content, String) = "Average" Then
                chart.Data.Children.Remove(_tlavg)
            End If
        End Sub
    End Class
End Namespace
