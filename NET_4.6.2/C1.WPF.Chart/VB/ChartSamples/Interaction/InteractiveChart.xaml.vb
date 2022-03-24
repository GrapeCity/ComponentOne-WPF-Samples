Imports System.Net
Imports System.Windows.Controls.Primitives
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Interactive chart which uses built-in zooming and panning features")> _
  Partial Public Class InteractiveChart
        Inherits UserControl
        Private _clrs() As Color = {Colors.Red, Colors.Green, Colors.Blue}

        Public Sub New()
            InitializeComponent()

            AddHandler Loaded, AddressOf InteractiveChart_Loaded

            c1chart.View.PlotBackground = New SolidColorBrush(Color.FromArgb(255, 80, 80, 80))

            AddHandler c1chart.ActionEnter, AddressOf Actions_Enter
            AddHandler c1chart.ActionLeave, AddressOf Actions_Leave

            CreataSampleData(3, 200)

            UpdateScrollbars()
        End Sub

        Private Sub InteractiveChart_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            rbX.Foreground = Foreground
            rbY.Foreground = Foreground
            rbXY.Foreground = Foreground
        End Sub

        Private Sub Actions_Leave(ByVal sender As Object, ByVal e As EventArgs)
            c1chart.Cursor = Cursors.Arrow

            UpdateScrollbars()
        End Sub

        Private Sub Actions_Enter(ByVal sender As Object, ByVal e As EventArgs)
            If TypeOf sender Is ScaleAction Then
                c1chart.Cursor = Cursors.SizeNS
            ElseIf TypeOf sender Is TranslateAction Then
#If (SILVERLIGHT) Then
	  c1chart.Cursor = Cursors.Stylus
	  End If
#Else
                c1chart.Cursor = Cursors.SizeAll
#End If
            Else
                c1chart.Cursor = Cursors.Hand
            End If
        End Sub

        Private rnd As New Random()

        Private Sub CreataSampleData(ByVal nser As Integer, ByVal npts As Integer)
            c1chart.BeginUpdate()
            Dim sc As DataSeriesCollection = c1chart.Data.Children
            sc.Clear()

            For iser As Integer = 0 To nser - 1
                Dim phase As Double = 0.05 + 0.1 * rnd.NextDouble()
                Dim amp As Double = 1 + 5 * rnd.NextDouble()

                Dim ds As New DataSeries()
                Dim vals(npts - 1) As Double
                For i As Integer = 0 To npts - 1
                    vals(i) = amp * Math.Sin(i * phase)
                Next i

                ds.ValuesSource = vals
                ds.Label = "S " & iser.ToString()

                ds.ConnectionStroke = New SolidColorBrush(_clrs(iser Mod 3))
                ds.ConnectionStrokeThickness = 2

                sc.Add(ds)
            Next iser
            c1chart.EndUpdate()
        End Sub

        Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            c1chart.BeginUpdate()
            c1chart.View.AxisX.Scale = 1
            c1chart.View.AxisX.Value = 0.5
            c1chart.View.AxisY.Scale = 1
            c1chart.View.AxisY.Value = 0.5

            UpdateScrollbars()
            c1chart.EndUpdate()
        End Sub

        Private Sub rb_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If c1chart Is Nothing Then
                Return
            End If

            c1chart.BeginUpdate()

            If sender Is rbX Then
                ' limit scale of Y-axis
                c1chart.View.AxisY.MinScale = 1
                c1chart.View.AxisY.Scale = 1
                c1chart.View.AxisY.Value = 0.5
                c1chart.View.AxisX.MinScale = 0.001
            ElseIf sender Is rbY Then
                ' limit scale of X-axis
                c1chart.View.AxisX.MinScale = 1
                c1chart.View.AxisX.Scale = 1
                c1chart.View.AxisX.Value = 0.5
                c1chart.View.AxisY.MinScale = 0.001
            Else
                ' allow smaller scales for both axes
                c1chart.View.AxisX.MinScale = 0.001
                c1chart.View.AxisY.MinScale = 0.001
            End If

            UpdateScrollbars()
            c1chart.EndUpdate()
        End Sub

        Private Sub UpdateScrollbars()
            Dim sx As Double = c1chart.View.AxisX.Scale
            Dim sbx As AxisScrollBar = CType(c1chart.View.AxisX.ScrollBar, AxisScrollBar)
            If sx >= 1.0 Then
                sbx.Visibility = Visibility.Collapsed
            Else
                sbx.Visibility = Visibility.Visible
            End If

            Dim sy As Double = c1chart.View.AxisY.Scale
            Dim sby As AxisScrollBar = CType(c1chart.View.AxisY.ScrollBar, AxisScrollBar)
            If sy >= 1.0 Then
                sby.Visibility = Visibility.Collapsed
            Else
                sby.Visibility = Visibility.Visible
            End If
        End Sub
    End Class
End Namespace