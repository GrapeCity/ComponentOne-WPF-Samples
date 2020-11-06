Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Animation when loading new data")> _
  Partial Public Class Animation
        Inherits UserControl
        Private _dict As New Dictionary(Of String, PlotElement)()

        Public Sub New()
            InitializeComponent()

            chart.View.AxisY.Min = 0
            chart.View.AxisY.Max = 100
            BarColumnOptions.SetRadiusX(chart, 2)
            BarColumnOptions.SetRadiusY(chart, 2)
            chart.ChartType = ChartType.Column

            NewData()
        End Sub

        Private rnd As New Random()

        Private Sub NewData()
            ' create new random data values
            Dim cnt As Integer = 12
            Dim vals(cnt - 1) As Double
            For i As Integer = 0 To cnt - 1
                vals(i) = rnd.Next(10, 100)
            Next i

            Dim ds As DataSeries
            If chart.Data.Children.Count = 0 Then
                ds = New DataSeries()
                AddHandler ds.PlotElementLoaded, AddressOf ds_PlotElementLoaded
                chart.Data.Children.Add(ds)
            End If
            chart.Data.Children(0).ValuesSource = vals
        End Sub

        Private Sub ds_PlotElementLoaded(ByVal sender As Object, ByVal e As EventArgs)
            Dim pe = CType(sender, PlotElement)
            Dim key As String = GetKey(pe)
            If _dict.ContainsKey(key) Then
                Dim pe0 = _dict(key)

#If (SILVERLIGHT) Then
		Dim rc0 As Rect = (CType(pe0.Shape, Path)).Data.Bounds
		Dim rc As Rect = (CType(pe.Shape, Path)).Data.Bounds
#Else
                Dim rc0 As Rect = pe0.RenderedGeometry.Bounds
                Dim rc As Rect = pe.RenderedGeometry.Bounds
#End If

                If rc0 <> rc Then ' different values
                    ' transform to fit into the previous value
                    Dim st As New ScaleTransform() With {.ScaleX = rc0.Width \ rc.Width, .ScaleY = rc0.Height \ rc.Height}
                    pe.RenderTransform = st
                    pe.RenderTransformOrigin = New Point(0, 1)

                    ' animate to the current value
                    Dim da1 As New DoubleAnimation() With {.To = 1.0}
                    Dim da2 As New DoubleAnimation() With {.To = 1.0}

#If (SILVERLIGHT) Then
		  Dim sb As New Storyboard() With {.Duration = New Duration(TimeSpan.FromSeconds(3))}
		  Storyboard.SetTarget(da1, st)
		  Storyboard.SetTargetProperty(da1, New PropertyPath("ScaleX"))
		  sb.Children.Add(da1)
		  Storyboard.SetTarget(da2, st)
		  Storyboard.SetTargetProperty(da2, New PropertyPath("ScaleY"))
		  sb.Children.Add(da2)
		  sb.Begin()
		  st.BeginAnimation(ScaleTransform.ScaleXProperty, da1)
		  st.BeginAnimation(ScaleTransform.ScaleYProperty, da2)
#Else
                    st.BeginAnimation(ScaleTransform.ScaleXProperty, da1)
                    st.BeginAnimation(ScaleTransform.ScaleYProperty, da2)
#End If
                End If
            End If
            ' store the previous element into the dictionary
            _dict(key) = pe
        End Sub

        Private Shared Function GetKey(ByVal pe As PlotElement) As String
            Return String.Format("s{0}p{1}", pe.DataPoint.SeriesIndex, pe.DataPoint.PointIndex)
        End Function

        Private Sub btnNew_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            NewData()
        End Sub
    End Class
End Namespace
