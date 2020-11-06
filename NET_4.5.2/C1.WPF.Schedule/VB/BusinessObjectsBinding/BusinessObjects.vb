Imports System.ComponentModel

Namespace BusinessObjectsBinding
	''' <summary>
	''' Represents custom business objects.
	''' Implementation of the INotifyPropertyChanged interface allows 
	''' to notify binding clients, that a property value has changed
	''' </summary>
	Public Class AppointmentBusinessObject
		Implements INotifyPropertyChanged
		#Region "** fields"
		Private _subject As String = ""
		Private _body As String = ""
		Private _location As String = ""
		Private _properties As String = ""
		Private _start As Date = Date.Today
		Private _end As Date = Date.Today + TimeSpan.FromDays(1)
		Private _id As Guid = Guid.NewGuid()
		Private _isDeleted As Boolean = False
		#End Region

		#Region "** ctor"
		''' <summary>
		''' Initializes a new instance of the <see cref="AppointmentBusinessObject"/> class.
		''' </summary>
		Public Sub New()
		End Sub
		#End Region

		#Region "** object model"
		Public Property Subject() As String
			Get
				Return _subject
			End Get
			Set(ByVal value As String)
				If _subject <> value Then
					_subject = value
					OnPropertyChanged("Subject")
				End If
			End Set
		End Property

		Public Property Start() As Date
			Get
				Return _start
			End Get
			Set(ByVal value As Date)
                If _start <> value Then
                    _start = value
                    OnPropertyChanged("Start")
                End If
			End Set
		End Property

		Public Property [End]() As Date
			Get
				Return _end
			End Get
			Set(ByVal value As Date)
                If _end <> value Then
                    _end = value
                    OnPropertyChanged("End")
                End If
			End Set
		End Property

		Public Property Body() As String
			Get
				Return _body
			End Get
			Set(ByVal value As String)
				If _body <> value Then
					_body = value
					OnPropertyChanged("Body")
				End If
			End Set
		End Property

		Public Property Location() As String
			Get
				Return _location
			End Get
			Set(ByVal value As String)
				If _location <> value Then
					_location = value
					OnPropertyChanged("Location")
				End If
			End Set
		End Property

		Public Property Properties() As String
			Get
				Return _properties
			End Get
			Set(ByVal value As String)
				If _properties <> value Then
					_properties = value
					OnPropertyChanged("Properties")
				End If
			End Set
		End Property

		Public Property Id() As Guid
			Get
				Return _id
			End Get
			Set(ByVal value As Guid)
				If _id <> value Then
					_id = value
					OnPropertyChanged("Id")
				End If
			End Set
		End Property

		Public Property IsDeleted() As Boolean
			Get
				Return _isDeleted
			End Get
			Set(ByVal value As Boolean)
				If _isDeleted <> value Then
					_isDeleted = value
					OnPropertyChanged("IsDeleted")
				End If
			End Set
		End Property
		#End Region

		#Region "** INotifyPropertyChanged implementation"
		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Protected Overridable Sub OnPropertyChanged(ByVal propName As String)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propName))
		End Sub
		#End Region
	End Class

	''' <summary>
	''' The <see cref="AppointmentBOList"/> class is a collection of the <see cref="AppointmentBusinessObject"/>
	''' objects that supports data binding.
	''' Note: data binding support is inherited from the BindingList class. 
	''' </summary>
	Public Class AppointmentBOList
		Inherits BindingList(Of AppointmentBusinessObject)
	End Class
End Namespace
