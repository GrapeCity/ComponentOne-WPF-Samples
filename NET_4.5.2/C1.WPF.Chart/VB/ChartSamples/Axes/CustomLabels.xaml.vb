Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Windows.Controls.Primitives
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Custom axis labels")> _
  Partial Public Class CustomLabels
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            If Not DesignerProperties.GetIsInDesignMode(Me) Then
                AddHandler chart.Loaded, AddressOf chart_Loaded
            End If
        End Sub

        Private Sub chart_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' read data from resource
            Dim data As New CSVData()

            Using stream As Stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("weather.csv")
                data.Read(stream, True)
            End Using

            Dim len As Integer = data.Length
            Dim dts(len - 1) As Date
            Dim t(len - 1) As Double
            Dim red As Brush = New SolidColorBrush(Colors.Red)
            Dim blue As Brush = New SolidColorBrush(Colors.Blue)

            Dim day As New Date(), dtmin As New Date(), dtmax As New Date()
            Dim tmax As Double = Double.MinValue, tmin As Double = Double.MaxValue

            ' collection for min/max values axis source
            Dim kvals As New List(Of KeyValuePair(Of Object, Double))()

            ' fill up the time and temperature arrays
            ' and calculate daily min/max
            For i As Integer = 0 To len - 1
                dts(i) = Date.Parse(data(i, "date") & " " & data(i, "time"), CultureInfo.InvariantCulture)
                t(i) = Double.Parse(data(i, "T"), CultureInfo.InvariantCulture)

                If i = 0 Then
                    day = dts(i).Date
                    dtmin = dts(i)
                    dtmax = dtmin
                    tmin = t(i)
                    tmax = tmin
                End If

                If dts(i).Date = day Then
                    If t(i) > tmax Then
                        tmax = t(i)
                        dtmax = dts(i)
                    End If
                    If t(i) < tmin Then
                        tmin = t(i)
                        dtmin = dts(i)
                    End If
                Else
                    kvals.Add(New KeyValuePair(Of Object, Double)(New TextBlock() With {.Text = "low", .Foreground = blue}, dtmin.ToOADate()))
                    kvals.Add(New KeyValuePair(Of Object, Double)(New TextBlock() With {.Text = "high", .Foreground = red}, dtmax.ToOADate()))
                    day = dts(i).Date
                    dtmin = dts(i)
                    dtmax = dtmin
                    tmin = t(i)
                    tmax = tmin
                End If
            Next i

            chart.BeginUpdate()

            ' create data series
            Dim ds As New XYDataSeries() With {.XValuesSource = dts, .ValuesSource = t, .ConnectionStrokeThickness = 2, .ConnectionStroke = New SolidColorBrush(Colors.Green)}
            chart.Data.Children.Add(ds)
            chart.ChartType = ChartType.Line

            Dim xsc As Double = 0.05

            ' main x-axis for time
            Dim axx As Axis = chart.View.AxisX
            axx.Min = dts(0).ToOADate()
            axx.Max = dts(len - 1).ToOADate()
            axx.Value = 0
            axx.Scale = xsc
            axx.IsTime = True
            axx.MajorUnit = 0.25
            axx.AnnoFormat = "HH"
            axx.MajorGridStroke = Nothing
            axx.AnnoTemplate = New TextBlock() With {.FontSize = 8, .Foreground = Foreground}
            axx.Foreground = Foreground
            axx.ScrollBar = New AxisScrollBar()

            ' auxiliary x-axis for dates
            Dim ax As New Axis() With {.AxisType = AxisType.X, .AnnoFormat = "d MMM", .AnnoPosition = AnnoPosition.Near, .IsDependent = True, .MajorUnit = 1, .IsTime = True, .MajorGridStroke = Foreground, .MajorGridStrokeThickness = 1.5}
            chart.View.Axes.Add(ax)

            ' create auxiliary x-axis for daily min/max
            ax = New Axis() With {.AxisType = AxisType.X, .Position = AxisPosition.Far, .IsDependent = True, .ItemsSource = kvals, .MajorGridStroke = New SolidColorBrush(Colors.LightGray), .MajorGridStrokeThickness = 1, .MajorGridStrokeDashes = New DoubleCollection(New Double() {1.0, 2.0})}
            'AnnoAngle = -90,
            chart.View.Axes.Add(ax)

            chart.EndUpdate()

            RemoveHandler chart.Loaded, AddressOf chart_Loaded
        End Sub
    End Class
End Namespace
