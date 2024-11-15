Imports System.IO
Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation
Imports System.ComponentModel
Imports System.Reflection

Namespace CustomTypeDescriptor
	''' <summary>
	''' Based on this: http://en.wikipedia.org/wiki/Market_data
	''' 
	''' This version implements ICustomTypeDescriptor to provide a 
	''' custom list of bindable properties, and can only be used
	''' in WPF (Silverlight does not support the ICustomTypeDescriptor
	''' interface).
	''' </summary>
	Public Class FinancialData
		Implements INotifyPropertyChanged, ICustomTypeDescriptor
		Private Const HISTORY_SIZE As Integer = 5

		Private _askHistory As New List(Of Decimal)()
		Private _bidHistory As New List(Of Decimal)()
		Private _saleHistory As New List(Of Decimal)()

		Private Shared _propertyDescriptorCollectionCache As PropertyDescriptorCollection
		Private Shared _zeroPropertyDescriptorCollection As New PropertyDescriptorCollection(Nothing)
		Friend _propBag As New Dictionary(Of String, Object)()

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
		Friend Sub AddHistory(ByVal list_Renamed As List(Of Decimal), ByVal value As Decimal)
			Do While list_Renamed.Count >= HISTORY_SIZE
				list_Renamed.RemoveAt(0)
			Loop
			Do While list_Renamed.Count < HISTORY_SIZE
				list_Renamed.Add(value)
			Loop
		End Sub

		Friend Function GetProp(ByVal propName As String) As Object
			Dim value As Object = Nothing
			_propBag.TryGetValue(propName, value)
			Return value
		End Function
		Friend Sub SetProp(ByVal propName As String, ByVal value As Object)
			If (Not _propBag.ContainsKey(propName)) OrElse GetProp(propName) IsNot value Then
				_propBag(propName) = value

				' save history (for sparklines)
				Select Case propName
					Case "Bid", "Ask", "LastSale"
						AddHistory(GetHistory(propName), CDec(Convert.ChangeType(value, GetType(Decimal))))
				End Select

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
                    Dim zip = New C1.Zip.C1ZipFile(asm.GetManifestResourceStream(resName))
                    Using sr = New StreamReader(zip.Entries("StockSymbols.txt").OpenReader())
						Do While Not sr.EndOfStream
							Dim sn = sr.ReadLine().Split(ControlChars.Tab)
							If sn.Length > 1 AndAlso sn(0).Trim().Length > 0 Then
								Dim data = New FinancialData()
								data.SetProp("Symbol", sn(0))
								data.SetProp("Name", sn(1))
								data.SetProp("Bid", rnd.Next(1, 1000))
								data.SetProp("Ask", CDec(rnd.NextDouble()))
								data.SetProp("LastSale", CDec(rnd.NextDouble()))
								data.SetProp("BidSize", rnd.Next(10, 500))
								data.SetProp("AskSize", rnd.Next(10, 500))
								data.SetProp("LastSize", rnd.Next(10, 500))
								data.SetProp("Volume", rnd.Next(10000, 20000))
								data.SetProp("QuoteTime", Date.Now)
								data.SetProp("TradeTime", Date.Now)
								list.Add(data)
								FinancialData.BuildPropertyDescriptorCollection(data._propBag, False)
							End If
						Loop
					End Using
					list.AutoUpdate = True
					Return list
				End If
			Next resName
			Throw New Exception("Can't find 'data.zip' embedded resource.")
		End Function

		''' <summary>
		''' Can now call this multiple times and append to existing collection, if desired,
		'''   or do minimal work if propertydescriptor set already exists.
		''' </summary>
		Public Shared Sub BuildPropertyDescriptorCollection(ByVal properties As Dictionary(Of String, Object), ByVal append As Boolean)
			' return if no source information from which to build propertydescriptor set...
			If (Nothing Is properties) OrElse (0 = properties.Count) Then
				Return
			End If

			' return if set already exists and we are not to append...
			If (Not append) AndAlso (Nothing IsNot _propertyDescriptorCollectionCache) Then
				Return
			End If

			Dim descriptors = New List(Of PropertyDescriptor)()

			' copy over any existing PropertyDescriptors (append)
			If Nothing IsNot _propertyDescriptorCollectionCache Then
				For Each pd As PropertyDescriptor In _propertyDescriptorCollectionCache
					descriptors.Add(pd)
				Next pd
			End If

			' add each supplied descriptor, so long as  
			'    no descriptor with same name already exists
			For Each kvp As KeyValuePair(Of String, Object) In properties
				If Not descriptors.Exists(Function(p) p.Name = kvp.Key) Then
					descriptors.Add(New ItemPropertyDescriptor(kvp.Key, kvp.Value))
				End If
			Next kvp

			' copy over to the READONLY collection used to supply descriptor for given property name
			_propertyDescriptorCollectionCache = New PropertyDescriptorCollection(descriptors.ToArray())
		End Sub

		#Region "ICustomTypeDescriptor Members"

		Public Function GetAttributes() As AttributeCollection Implements ICustomTypeDescriptor.GetAttributes
			Return New AttributeCollection(Nothing)
		End Function

		Public Function GetClassName() As String Implements ICustomTypeDescriptor.GetClassName
			Return Nothing
		End Function

		Public Function GetComponentName() As String Implements ICustomTypeDescriptor.GetComponentName
			Return Nothing
		End Function

		Public Function GetConverter() As TypeConverter Implements ICustomTypeDescriptor.GetConverter
			Return Nothing
		End Function

		Public Function GetDefaultEvent() As EventDescriptor Implements ICustomTypeDescriptor.GetDefaultEvent
			Return Nothing
		End Function

		Public Function GetDefaultProperty() As PropertyDescriptor Implements ICustomTypeDescriptor.GetDefaultProperty
			Return Nothing
		End Function

		Public Function GetEditor(ByVal editorBaseType As Type) As Object Implements ICustomTypeDescriptor.GetEditor
			Return Nothing
		End Function

		Public Function GetEvents(ByVal attributes() As Attribute) As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
			Return New EventDescriptorCollection(Nothing)
		End Function

		Public Function GetEvents() As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
			Return New EventDescriptorCollection(Nothing)
		End Function

		Public Function GetProperties(ByVal attributes() As Attribute) As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
			If _propertyDescriptorCollectionCache Is Nothing Then
				Return _zeroPropertyDescriptorCollection
			End If
			Return _propertyDescriptorCollectionCache
		End Function

		Public Function GetProperties() As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
			Return (CType(Me, ICustomTypeDescriptor)).GetProperties(Nothing)
		End Function

		Public Function GetPropertyOwner(ByVal pd As PropertyDescriptor) As Object Implements ICustomTypeDescriptor.GetPropertyOwner
			Return Me
		End Function

		#End Region
	End Class
	Public Class FinancialDataList
		Inherits List(Of FinancialData)
		' fields
        Private _timer As System.Windows.Threading.DispatcherTimer
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
		Private privateBatchSize As Integer
		Public Property BatchSize() As Integer
			Get
				Return privateBatchSize
			End Get
			Set(ByVal value As Integer)
				privateBatchSize = value
			End Set
		End Property
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
            _timer = New System.Windows.Threading.DispatcherTimer()
			AddHandler _timer.Tick, AddressOf _timer_Tick
			UpdateInterval = 100
			BatchSize = 100
		End Sub

		Private Sub _timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
			If Me.Count > 0 Then
				For i As Integer = 0 To BatchSize - 1
					Dim index As Integer = _rnd.Next() Mod Me.Count
					Dim data = Me(index)
					Dim bid As Decimal = 10 * CDec(1 + (_rnd.NextDouble() *.11 -.05))
					Dim ask As Decimal = 20 * CDec(1 + (_rnd.NextDouble() *.11 -.05))
					data.SetProp("Bid", bid)
					data.SetProp("Ask", ask)
					data.SetProp("BidSize", _rnd.Next(10, 1000))
					data.SetProp("AskSize", _rnd.Next(10, 1000))
					Dim sale = (ask + bid) / 2
					data.SetProp("LastSale", sale)
					data.SetProp("LastSize", (_rnd.Next(10, 1000) + _rnd.Next(10, 1000)) / 2)
					data.SetProp("QuoteTime", Date.Now)
					data.SetProp("TradeTime", Date.Now.AddSeconds(-_rnd.Next(0, 60)))
				Next i
			End If
		End Sub
	End Class

	Public Class ItemPropertyDescriptor
		Inherits PropertyDescriptor
		#Region "private instance data"

		Private mPropType As Type = GetType(Object)

		#End Region

		Private privateKey As String
		Public Property Key() As String
			Get
				Return privateKey
			End Get
			Set(ByVal value As String)
				privateKey = value
			End Set
		End Property


		Public Sub New(ByVal key As String)
			MyBase.New(key, Nothing)
			Me.Key = key
		End Sub

		Public Sub New(ByVal key As String, ByVal sampleValue As Object)
			Me.New(key)
			If Nothing IsNot sampleValue Then
				mPropType = sampleValue.GetType()
			End If
		End Sub

		Public Overrides Function CanResetValue(ByVal component As Object) As Boolean
			Return False
		End Function

		Public Overrides ReadOnly Property ComponentType() As Type
			Get
				Return GetType(FinancialData)
			End Get
		End Property

		Public Overrides Function GetValue(ByVal component As Object) As Object
			Dim retVal As Object = Nothing
			Dim trade As FinancialData = CType(component, FinancialData)
			If Nothing IsNot trade Then
				retVal = trade.GetProp(Key)
			End If
			Return retVal
		End Function

		Public Overrides Sub ResetValue(ByVal component As Object)

		End Sub

		Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
			Dim trade As FinancialData = CType(component, FinancialData)
			If Nothing IsNot trade Then
				trade.SetProp(Key, value)
			End If
			OnValueChanged(component, EventArgs.Empty)
		End Sub

		Public Overrides ReadOnly Property IsReadOnly() As Boolean
			Get
				Return False
			End Get
		End Property

		Public Overrides ReadOnly Property PropertyType() As Type
			Get
				Return mPropType
			End Get
		End Property

		Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
			Return False
		End Function
	End Class
End Namespace
