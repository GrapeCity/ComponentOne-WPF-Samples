Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
  ''' <summary>
  ''' Sample data for different chart types
  ''' </summary>
  Friend Class SampleData
	Private _dataSeriesLoaded As EventHandler
	Private _defaultData, _finData, _bubbleData, _ganttData, _polarData As ChartData

	Public Function GetData(ByVal ct As ChartType) As ChartData
            If ct = ChartType.Bubble Then
                Return BubbleData
            ElseIf ct = ChartType.Candle OrElse ct = ChartType.HighLowOpenClose Then
                Return FinancialData
            ElseIf ct = ChartType.Gantt Then
                Return GanttData
            ElseIf ct.ToString().StartsWith("Polar") Then
                Return PolarData
            Else
                Return DefaultData
            End If
	End Function

	Public Sub New()
	End Sub

	Public Sub New(ByVal dataSeriesLoaded As EventHandler)
	  _dataSeriesLoaded = dataSeriesLoaded
	End Sub

	''' <summary>
	''' Default data is appropriate for the most chart types.
	''' </summary>
	Public ReadOnly Property DefaultData() As ChartData
	  Get
		If _defaultData Is Nothing Then
		  _defaultData = New ChartData()

		  Dim ds1 As New DataSeries() With {.Label = "s1"}
		  ds1.ValuesSource = New Double() { 3, 2, 7, 4, 8 }
		  _defaultData.Children.Add(ds1)

		  Dim ds2 As New DataSeries() With {.Label = "s2"}
		  ds2.ValuesSource = New Double() { 1, 2, 3, 4, 6 }
		  _defaultData.Children.Add(ds2)

		  Dim ds3 As New DataSeries() With {.Label = "s3"}
		  ds3.ValuesSource = New Double() { 0, 1, 6, 2, 3 }
		  _defaultData.Children.Add(ds3)

		  If _dataSeriesLoaded IsNot Nothing Then
			For Each ds As DataSeries In _defaultData.Children
			  AddHandler ds.PlotElementLoaded, _dataSeriesLoaded
			Next ds
		  End If
		End If

		Return _defaultData
	  End Get
	End Property

	''' <summary>
	''' Data for financial charts(HighLowOpenClose and Candle).
	''' </summary>
	Public ReadOnly Property FinancialData() As ChartData
	  Get
		If _finData Is Nothing Then
		  _finData = New ChartData()

		  Dim ds As New HighLowOpenCloseSeries() With {.Label = "s1"}
		  ds.SymbolStrokeThickness = 3
		  ds.SymbolSize = New Size(20, 20)
		  ds.XValuesSource = New Date() { New Date(2008,10,1), New Date(2008,10,2), New Date(2008,10,3), New Date(2008,10,6), New Date(2008,10,7), New Date(2008,10,8) }
		  ds.OpenValuesSource = New Double() { 100, 102, 104, 100, 107, 102 }
		  ds.CloseValuesSource = New Double() { 102, 104, 100, 107, 102, 100 }
		  ds.HighValuesSource = New Double() { 102, 105, 105, 108, 109, 105 }
		  ds.LowValuesSource = New Double() { 99, 95, 95, 100, 96, 99, 98 }
		  If _dataSeriesLoaded IsNot Nothing Then
			AddHandler ds.PlotElementLoaded, _dataSeriesLoaded
		  End If
		  _finData.Children.Add(ds)
		End If
		Return _finData
	  End Get
	End Property

	''' <summary>
	''' Data for Bubble chart.
	''' </summary>
	Public ReadOnly Property BubbleData() As ChartData
	  Get
		If _bubbleData Is Nothing Then
		  _bubbleData = New ChartData()

		  Dim ds1 As New BubbleSeries() With {.Label = "s1"}
		  ds1.ValuesSource = New Double() { 3, 4, 7, 5, 8 }
		  ds1.SizeValuesSource = New Double() { 1, 5, 7, 4, 9 }
		  _bubbleData.Children.Add(ds1)

		  Dim ds2 As New BubbleSeries() With {.Label = "s2"}
		  ds2.ValuesSource = New Double() { 1, 2, 3, 4, 6 }
		  ds2.SizeValuesSource = New Double() { 10, 3, 6, 8, 9 }
		  _bubbleData.Children.Add(ds2)

		  If _dataSeriesLoaded IsNot Nothing Then
			For Each ds As DataSeries In _bubbleData.Children
			  AddHandler ds.PlotElementLoaded, _dataSeriesLoaded
			Next ds
		  End If
		End If

		Return _bubbleData
	  End Get
	End Property

	''' <summary>
	''' Data for Gantt chart.
	''' </summary>
	Public ReadOnly Property GanttData() As ChartData
	  Get
		If _ganttData Is Nothing Then
		  _ganttData = New ChartData()
		  _ganttData.ItemNames = New String() { "Task1", "Task2", "Task3", "Task4" }

		  Dim ds1 As New HighLowSeries() With {.Label = "Task1"}
		  ds1.XValuesSource = New Double() { 0 }
		  ds1.LowValuesSource = New Date() { New Date(2008, 10, 1) }
		  ds1.HighValuesSource = New Date() { New Date(2008, 10, 5) }
		  _ganttData.Children.Add(ds1)

		  Dim ds2 As New HighLowSeries() With {.Label = "Task2"}
		  ds2.XValuesSource = New Double() { 1 }
		  ds2.LowValuesSource = New Date() { New Date(2008, 10, 3) }
		  ds2.HighValuesSource = New Date() { New Date(2008, 10, 4) }
		  _ganttData.Children.Add(ds2)

		  Dim ds3 As New HighLowSeries() With {.Label = "Task3"}
		  ds3.XValuesSource = New Double() { 2 }
		  ds3.LowValuesSource = New Date() { New Date(2008, 10, 2) }
		  ds3.HighValuesSource = New Date() { New Date(2008, 10, 8) }
		  _ganttData.Children.Add(ds3)

		  Dim ds4 As New HighLowSeries() With {.Label = "Task4"}
		  ds4.XValuesSource = New Double() { 3 }
		  ds4.LowValuesSource = New Date() { New Date(2008, 10, 4) }
		  ds4.HighValuesSource = New Date() { New Date(2008, 10, 7) }
		  _ganttData.Children.Add(ds4)

		  For Each ds As DataSeries In _ganttData.Children
			ds.SymbolSize = New Size(30, 30)
			If _dataSeriesLoaded IsNot Nothing Then
			  AddHandler ds.PlotElementLoaded, _dataSeriesLoaded
			End If
		  Next ds
		End If
		Return _ganttData
	  End Get
	End Property

		''' <summary>
	''' Data for polar chart.
	''' </summary>
	Public ReadOnly Property PolarData() As ChartData
	  Get
		If _polarData Is Nothing Then
		  Dim cnt As Integer = 61
		  Dim x(cnt - 1) As Double
		  Dim y(cnt - 1) As Double
		  For i As Integer = 0 To cnt - 1
			x(i) = i*6
			y(i) = 100 * Math.Abs(Math.Cos(9 * i * Math.PI / 180))
		  Next i
		  Dim ds As New XYDataSeries() With {.XValuesSource = x, .ValuesSource = y}
		  _polarData = New ChartData()
		  _polarData.Children.Add(ds)
		End If

		Return _polarData
	  End Get
	End Property
  End Class
End Namespace
