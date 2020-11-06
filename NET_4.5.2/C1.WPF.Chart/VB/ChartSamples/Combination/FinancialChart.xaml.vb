Imports System.ComponentModel
Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <Description("Financial chart with Two Y-axes")> _
  Partial Public Class FinancialChart
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
            chart.View.AxisX.Scale = 0.25
            NewData()
        End Sub

        Private Sub NewData()
            Dim ticks As Long = Date.Now.Ticks
            Dim len As Integer = 100
            Dim data As List(Of Quotation) = SampleFinancialData.Create(len)
            chart.BeginUpdate()
            chart.Data.ItemsSource = data
            chart.View.AxisX.Min = data(0).Time.ToOADate() - 0.5
            chart.View.AxisX.Max = data(len - 1).Time.ToOADate() + 0.5
            chart.EndUpdate()
        End Sub

        Private Sub btnNew_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            NewData()
        End Sub

        Private Sub Series_Loaded(ByVal sender As Object, ByVal e As EventArgs)
            Dim pe As PlotElement = CType(sender, PlotElement)
            If TypeOf pe Is HLOC Then
                Dim tb As TextBlock = CType(FindName("txtPrice"), TextBlock)
                tb.Foreground = pe.Stroke
            Else
                Dim tb As TextBlock = CType(FindName("txtVol"), TextBlock)
                tb.Foreground = pe.Stroke
            End If
        End Sub

        Private Sub btnPrice_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim btn As Button = CType(sender, Button)
            If CStr(btn.Content) = "Standard" Then
                btn.Content = "Candle"
                chart.ChartType = ChartType.HighLowOpenClose
            Else
                btn.Content = "Standard"
                chart.ChartType = ChartType.Candle
            End If
        End Sub

        Private Sub btnVol_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            chart.BeginUpdate()
            Dim btn As Button = CType(sender, Button)
            If CStr(btn.Content) = "Bar" Then
                btn.Content = "Area"
                chart.Data.Children(1).ChartType = ChartType.Bar
            Else
                btn.Content = "Bar"
                chart.Data.Children(1).ChartType = ChartType.Area
            End If
            chart.EndUpdate()
        End Sub
    End Class

  Public Class Quotation
        Public Property Time() As Date
            Get
                Return m_Time
            End Get
            Set(ByVal value As Date)
                m_Time = value
            End Set
        End Property
        Private m_Time As Date

        Public Property Open() As Double
            Get
                Return m_Open
            End Get
            Set(ByVal value As Double)
                m_Open = value
            End Set
        End Property
        Private m_Open As Double

        Public Property Close() As Double
            Get
                Return m_Close
            End Get
            Set(ByVal value As Double)
                m_Close = value
            End Set
        End Property
        Private m_Close As Double

        Public Property High() As Double
            Get
                Return m_High
            End Get
            Set(ByVal value As Double)
                m_High = value
            End Set
        End Property
        Private m_High As Double

        Public Property Low() As Double
            Get
                Return m_Low
            End Get
            Set(ByVal value As Double)
                m_Low = value
            End Set
        End Property
        Private m_Low As Double

        Public Property Volume() As Double
            Get
                Return m_Volume
            End Get
            Set(ByVal value As Double)
                m_Volume = value
            End Set
        End Property
        Private m_Volume As Double
    End Class

  Public Class SampleFinancialData
	Private Shared rnd As New Random()
	Public Shared Function Create(ByVal npts As Integer) As List(Of Quotation)
	  Dim data As New List(Of Quotation)()
	  Dim dt As Date = Date.Today.AddDays(0)

	  For i As Integer = 0 To npts - 1
		Dim q As New Quotation()

		q.Time = dt.AddDays(i)

		If i > 0 Then
		  q.Open = data(i - 1).Close
		Else
		  q.Open = 1000
		End If

		q.High = q.Open + rnd.Next(50)
		q.Low = q.Open - rnd.Next(50)

		q.Close = rnd.Next(CInt(Fix(q.Low)), CInt(Fix(q.High)))

		q.Volume = rnd.Next(0, 100)

		data.Add(q)
	  Next i

	  Return data
	End Function
  End Class
End Namespace
