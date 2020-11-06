Imports System.IO
Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation

Namespace ChartSamples
  Public Class WeatherData
	  Inherits Control
	Public Shared DateTimeProperty As DependencyProperty = DependencyProperty.Register("DateTime", GetType(Date), GetType(WeatherData), Nothing)

        Public Property [Date]() As Date
            Get
                Return CDate(GetValue(DateTimeProperty))
            End Get
            Set(ByVal value As Date)
                SetValue(DateTimeProperty, value)
            End Set
        End Property

	Public Shared TMinProperty As DependencyProperty = DependencyProperty.Register("TMin", GetType(Double), GetType(WeatherData), Nothing)

	Public Property TMin() As Double
	  Get
		  Return CDbl(GetValue(TMinProperty))
	  End Get
	  Set(ByVal value As Double)
		  SetValue(TMinProperty, value)
	  End Set
	End Property

	Public Shared TMaxProperty As DependencyProperty = DependencyProperty.Register("TMax", GetType(Double), GetType(WeatherData), Nothing)

	Public Property TMax() As Double
	  Get
		  Return CDbl(GetValue(TMaxProperty))
	  End Get
	  Set(ByVal value As Double)
		  SetValue(TMaxProperty, value)
	  End Set
	End Property

	Public Shared TAvgProperty As DependencyProperty = DependencyProperty.Register("TAvg", GetType(Double), GetType(WeatherData), Nothing)

	Public Property TAvg() As Double
	  Get
		  Return CDbl(GetValue(TAvgProperty))
	  End Get
	  Set(ByVal value As Double)
		  SetValue(TAvgProperty, value)
	  End Set
	End Property

	Public Sub New(ByVal dt As Date, ByVal tmax As Double, ByVal tavg As Double, ByVal tmin As Double)
            Me.[Date] = dt
	  Me.TMin = tmin
	  Me.TMax = tmax
	  Me.TAvg = tavg
	End Sub
  End Class

  Public Class CSVData
	Private _fields As New List(Of String)()
	Private _records As New List(Of String())()

	Private ReadOnly Property Fields() As List(Of String)
	  Get
		  Return _fields
	  End Get
	End Property

	Private ReadOnly Property Records() As List(Of String())
	  Get
		  Return _records
	  End Get
	End Property

	Public ReadOnly Property Length() As Integer
	  Get
		  Return _records.Count
	  End Get
	End Property

	Default Public ReadOnly Property Item(ByVal i As Integer, ByVal j As Integer) As String
	  Get
		Dim ss() As String = Records(i)
		If j < 0 OrElse j >= ss.Length Then
		  Throw New ArgumentException("j")
		End If

		Return ss(j)
	  End Get
	End Property

	Default Public ReadOnly Property Item(ByVal i As Integer, ByVal name As String) As String
	  Get
		Dim ss() As String = Records(i)
		Dim j As Integer = Fields.IndexOf(name)
		If j < 0 OrElse j >= ss.Length Then
		  Throw New ArgumentException("name")
		End If

		Return ss(j)
	  End Get
	End Property

	Public Sub Read(ByVal stream As Stream, ByVal readNames As Boolean)
	  If stream Is Nothing Then
		Throw New ArgumentNullException("stream")
	  End If

	  Fields.Clear()
	  Records.Clear()

	  If stream IsNot Nothing Then
		Using sr As New StreamReader(stream)
		  Dim i As Integer = 0
		  Do While sr.Peek() > -1
			Dim line As String = sr.ReadLine()

			If line.StartsWith("#") Then
			  Continue Do
			End If

			Dim ss() As String = line.Split(","c)

			If readNames AndAlso Fields.Count = 0 Then
			  Fields.AddRange(ss)
			Else
				Records.Add(ss)
			  i += 1
			End If
		  Loop
		  sr.Close()
		End Using
	  End If
	End Sub
  End Class
End Namespace
