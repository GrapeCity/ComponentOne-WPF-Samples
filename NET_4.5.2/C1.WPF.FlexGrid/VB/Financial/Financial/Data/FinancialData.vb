Imports System.IO
Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation
Imports System.ComponentModel
Imports System.Reflection
Imports System.ComponentModel.DataAnnotations

Namespace Financial
    ''' <summary>
    ''' Based on this: http://en.wikipedia.org/wiki/Market_data
    ''' </summary>
    Public Class FinancialData
        Implements INotifyPropertyChanged
        Private Const HISTORY_SIZE As Integer = 5
        Private _askHistory As New List(Of Decimal)()
        Private _bidHistory As New List(Of Decimal)()
        Private _saleHistory As New List(Of Decimal)()

        <Display(Name:="Symbol")>
        Public Property Symbol() As String
            Get
                Return CStr(GetProp("Symbol"))
            End Get
            Set(ByVal value As String)
                SetProp("Symbol", value)
            End Set
        End Property

        <Display(Name:="Name")>
        Public Property Name() As String
            Get
                Return CStr(GetProp("Name"))
            End Get
            Set(ByVal value As String)
                SetProp("Name", value)
            End Set
        End Property

        <Display(Name:="Bid")>
        Public Property Bid() As Decimal
            Get
                Return CDec(GetProp("Bid"))
            End Get
            Set(ByVal value As Decimal)
                AddHistory(_bidHistory, value)
                SetProp("Bid", value)
            End Set
        End Property

        <Display(Name:="Ask")>
        Public Property Ask() As Decimal
            Get
                Return CDec(GetProp("Ask"))
            End Get
            Set(ByVal value As Decimal)
                AddHistory(_askHistory, value)
                SetProp("Ask", value)
            End Set
        End Property

        <Display(Name:="LastSale")>
        Public Property LastSale() As Decimal
            Get
                Return CDec(GetProp("LastSale"))
            End Get
            Set(ByVal value As Decimal)
                AddHistory(_saleHistory, value)
                SetProp("LastSale", value)
            End Set
        End Property

        <Display(Name:="BidSize")>
        Public Property BidSize() As Integer
            Get
                Return CInt(Fix(GetProp("BidSize")))
            End Get
            Set(ByVal value As Integer)
                SetProp("BidSize", value)
            End Set
        End Property

        <Display(Name:="AskSize")>
        Public Property AskSize() As Integer
            Get
                Return CInt(Fix(GetProp("AskSize")))
            End Get
            Set(ByVal value As Integer)
                SetProp("AskSize", value)
            End Set
        End Property

        <Display(Name:="LastSize")>
        Public Property LastSize() As Integer
            Get
                Return CInt(Fix(GetProp("LastSize")))
            End Get
            Set(ByVal value As Integer)
                SetProp("LastSize", value)
            End Set
        End Property

        <Display(Name:="Volume")>
        Public Property Volume() As Integer
            Get
                Return CInt(Fix(GetProp("Volume")))
            End Get
            Set(ByVal value As Integer)
                SetProp("Volume", value)
            End Set
        End Property

        <Display(Name:="QuoteTime")>
        Public Property QuoteTime() As Date
            Get
                Return CDate(GetProp("QuoteTime"))
            End Get
            Set(ByVal value As Date)
                SetProp("QuoteTime", value)
            End Set
        End Property

        <Display(Name:="TradeTime")>
        Public Property TradeTime() As Date
            Get
                Return CDate(GetProp("TradeTime"))
            End Get
            Set(ByVal value As Date)
                SetProp("TradeTime", value)
            End Set
        End Property
        Public Function GetHistory(ByVal propName As String) As List(Of Decimal)
            Select Case propName
                Case "Ask"
                    Return _askHistory
                Case "Bid"
                    Return _bidHistory
                Case "LastSale"
                    Return _saleHistory
            End Select
            Return Nothing
        End Function
        Private Sub AddHistory(ByVal list As List(Of Decimal), ByVal value As Decimal)
            Do While list.Count >= HISTORY_SIZE
                list.RemoveAt(0)
            Loop
            Do While list.Count < HISTORY_SIZE
                list.Add(value)
            Loop
        End Sub

        Private _propBag As New Dictionary(Of String, Object)()
        Private Function GetProp(ByVal propName As String) As Object
            Dim value As Object = Nothing
            _propBag.TryGetValue(propName, value)
            Return value
        End Function
        Private Sub SetProp(ByVal propName As String, ByVal value As Object)
            If (Not _propBag.ContainsKey(propName)) OrElse GetProp(propName) IsNot value Then
                _propBag(propName) = value
                OnPropertyChanged(New PropertyChangedEventArgs(propName))
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Overridable Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub

        ' get a default list of financial items
        Public Shared Function GetFinancialData() As FinancialDataList
            Dim list = New FinancialDataList()
            Dim rnd = New Random(0)
            Dim asm = System.Reflection.Assembly.GetExecutingAssembly()
            For Each resName As String In asm.GetManifestResourceNames()
                If resName.EndsWith("data.zip") Then
                    Dim zip = New C1.C1Zip.C1ZipFile(asm.GetManifestResourceStream(resName))
                    Using sr = New StreamReader(zip.Entries("StockSymbols.txt").OpenReader())
                        Do While Not sr.EndOfStream
                            Dim sn = sr.ReadLine().Split(ControlChars.Tab)
                            If sn.Length > 1 AndAlso sn(0).Trim().Length > 0 Then
                                Dim data = New FinancialData()
                                data.Symbol = sn(0)
                                data.Name = sn(1)
                                data.Bid = rnd.Next(1, 1000)
                                data.Ask = data.Bid + CDec(rnd.NextDouble())
                                data.LastSale = data.Bid
                                data.BidSize = rnd.Next(10, 500)
                                data.AskSize = rnd.Next(10, 500)
                                data.LastSize = rnd.Next(10, 500)
                                data.Volume = rnd.Next(10000, 20000)
                                data.QuoteTime = Date.Now
                                data.TradeTime = Date.Now
                                list.Add(data)
                            End If
                        Loop
                    End Using
                    list.AutoUpdate = True
                    Return list
                End If
            Next resName
            Throw New Exception("Can't find 'data.zip' embedded resource.")
        End Function
    End Class
    Public Class FinancialDataList
        Inherits List(Of FinancialData)
        ' fields
        Private _timer As C1.Util.Timer
        Private _rnd As New Random(0)

        ' object model
        Public Property UpdateInterval() As Integer
            Get
                Return CInt(Fix(_timer.Interval.TotalMilliseconds))
            End Get
            Set(ByVal value As Integer)
                _timer.Interval = TimeSpan.FromMilliseconds(value)
            End Set
        End Property
        Public Property BatchSize() As Integer
        Public Property AutoUpdate() As Boolean
            Get
                Return _timer.IsEnabled
            End Get
            Set(ByVal value As Boolean)
                _timer.IsEnabled = value
            End Set
        End Property

        ' ctor
        Public Sub New()
            _timer = New C1.Util.Timer()
            AddHandler _timer.Tick, AddressOf _timer_Tick
            UpdateInterval = 100
            BatchSize = 100
        End Sub

        Private Sub _timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
            If Me.Count > 0 Then
                For i As Integer = 0 To BatchSize - 1
                    Dim index As Integer = _rnd.Next() Mod Me.Count
                    Dim data = Me(index)
                    data.Bid = data.Bid * CDec(1 + (_rnd.NextDouble() * 0.11 - 0.05))
                    data.Ask = data.Ask * CDec(1 + (_rnd.NextDouble() * 0.11 - 0.05))
                    data.BidSize = _rnd.Next(10, 1000)
                    data.AskSize = _rnd.Next(10, 1000)
                    Dim sale = (data.Ask + data.Bid) / 2
                    data.LastSale = sale
                    data.LastSize = (data.AskSize + data.BidSize) / 2
                    data.QuoteTime = Date.Now
                    data.TradeTime = Date.Now.AddSeconds(-_rnd.Next(0, 60))
                Next i
            End If
        End Sub
    End Class
End Namespace
