Imports System.Collections
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace CollectionViewFilter
    ''' <summary>
    ''' Simple data class that implements two important binding interfaces:
    ''' 
    ''' INotifyPropertyChanged:
    '''   Notifies bound controls of changes to the object properties so they can synchronize
    '''   the UI with the current object state.
    '''   
    ''' IEditableObject:
    '''   Provides transacted edits by saving the object state when editing starts and 
    '''   committing or rolling back when editing ends.
    '''   
    ''' </summary>
    Public Class Customer
        Implements INotifyPropertyChanged, IEditableObject
        ' ** fields
        Private _id, _countryID As Integer
        Private _first, _last As String
        Private _father, _brother, _cousin As String
        Private _active As Boolean
        Private _hired As Date
        Private _weight As Double

        ' ** data generators
        Private Shared _rnd As New Random()
        Private Shared _firstNames() As String = "Andy|Ben|Charlie|Dan|Ed|Fred|Gil|Herb|Jack|Karl|Larry|Mark|Noah|Oprah|Paul|Quince|Rich|Steve|Ted|Ulrich|Vic|Xavier|Zeb".Split("|"c)
        Private Shared _lastNames() As String = "Ambers|Bishop|Cole|Danson|Evers|Frommer|Griswold|Heath|Jammers|Krause|Lehman|Myers|Neiman|Orsted|Paulson|Quaid|Richards|Stevens|Trask|Ulam".Split("|"c)
        Private Shared _countries() As String = "China|India|United States|Indonesia|Brazil|Pakistan|Bangladesh|Nigeria|Russia|Japan|Mexico|Philippines|Vietnam|Germany|Ethiopia|Egypt|Iran|Turkey|Congo|France|Thailand|United Kingdom|Italy|Myanmar".Split("|"c)

        ' ** ctors
        Public Sub New()
            Me.New(_rnd.Next())
        End Sub
        Public Sub New(ByVal id As Integer)
            Me.ID = id

            First = GetString(_firstNames)
            Last = GetString(_lastNames)
            CountryID = _rnd.Next() Mod _countries.Length
            Active = _rnd.NextDouble() >= 0.5
            Hired = Date.Today.AddDays(-_rnd.Next(1, 365))
            Weight = 50 + _rnd.NextDouble() * 50
            _father = String.Format("{0} {1}", GetString(_firstNames), Last)
            _brother = String.Format("{0} {1}", GetString(_firstNames), Last)
            _cousin = GetName()
        End Sub

        ' ** object model
        <Display(Name:="ID")>
        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                If value <> _id Then
                    _id = value
                    RaisePropertyChanged("ID")
                End If
            End Set
        End Property

        <Display(Name:="Name")>
        Public ReadOnly Property Name() As String
            Get
                Return String.Format("{0} {1}", First, Last)
            End Get
        End Property

        <Display(Name:="Country")>
        Public ReadOnly Property Country() As String
            Get
                Return _countries(_countryID)
            End Get
        End Property

        <Display(Name:="CountryID")>
        Public Property CountryID() As Integer
            Get
                Return _countryID
            End Get
            Set(ByVal value As Integer)
                If value <> _countryID AndAlso value > -1 AndAlso value < _countries.Length Then
                    _countryID = value

                    ' call OnPropertyChanged with null parameter since setting this property
                    ' modifies the value of "CountryID" and also the value of "Country".
                    RaisePropertyChanged(Nothing)
                End If
            End Set
        End Property

        <Display(Name:="Active")>
        Public Property Active() As Boolean
            Get
                Return _active
            End Get
            Set(ByVal value As Boolean)
                If value <> _active Then
                    _active = value
                    RaisePropertyChanged("Active")
                End If
            End Set
        End Property

        <Display(Name:="First")>
        Public Property First() As String
            Get
                Return _first
            End Get
            Set(ByVal value As String)
                If value <> _first Then
                    _first = value

                    ' call OnPropertyChanged with null parameter since setting this property
                    ' modifies the value of "First" and also the value of "Name".
                    RaisePropertyChanged(Nothing)
                End If
            End Set
        End Property

        <Display(Name:="Last")>
        Public Property Last() As String
            Get
                Return _last
            End Get
            Set(ByVal value As String)
                If value <> _last Then
                    _last = value

                    ' call OnPropertyChanged with null parameter since setting this property
                    ' modifies the value of "First" and also the value of "Name".
                    RaisePropertyChanged(Nothing)
                End If
            End Set
        End Property

        <Display(Name:="Hired")>
        Public Property Hired() As Date
            Get
                Return _hired
            End Get
            Set(ByVal value As Date)
                If value <> _hired Then
                    _hired = value
                    RaisePropertyChanged("Hired")
                End If
            End Set
        End Property

        <Display(Name:="Weight")>
        Public Property Weight() As Double
            Get
                Return _weight
            End Get
            Set(ByVal value As Double)
                If value <> _weight Then
                    _weight = value
                    RaisePropertyChanged("Weight")
                End If
            End Set
        End Property

        ' some read-only stuff
        <Display(Name:="Father")>
        Public ReadOnly Property Father() As String
            Get
                Return _father
            End Get
        End Property

        <Display(Name:="Brother")>
        Public ReadOnly Property Brother() As String
            Get
                Return _brother
            End Get
        End Property

        <Display(Name:="Cousin")>
        Public ReadOnly Property Cousin() As String
            Get
                Return _cousin
            End Get
        End Property

        ' ** utilities
        Private Shared Function GetString(ByVal arr() As String) As String
            Return arr(_rnd.Next(arr.Length))
        End Function
        Private Shared Function GetName() As String
            Return String.Format("{0} {1}", GetString(_firstNames), GetString(_lastNames))
        End Function

        ' ** static list provider
        Public Shared Function GetCustomerList(ByVal count As Integer) As ObservableCollection(Of Customer)
            Dim list = New ObservableCollection(Of Customer)()
            For i As Integer = 0 To count - 1
                list.Add(New Customer(i))
            Next i
            Return list
        End Function

        ' ** static value providers
        Public Shared Function GetCountries() As String()
            Return _countries
        End Function
        Public Shared Function GetFirstNames() As String()
            Return _firstNames
        End Function
        Public Shared Function GetLastNames() As String()
            Return _lastNames
        End Function

        '-----------------------------------------------------------------------------------
#Region "** INotifyPropertyChanged Members"

        ' this interface allows bounds controls to react to changes in the data objects.

        Private Sub RaisePropertyChanged(ByVal propertyName As String)
            OnPropertyChanged(New PropertyChangedEventArgs(propertyName))
        End Sub
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub

#End Region

        '-----------------------------------------------------------------------------------
#Region "IEditableObject Members"

        ' this interface allows transacted edits (user can press escape to restore previous values).

        Private _clone As Customer
        Public Sub BeginEdit() Implements IEditableObject.BeginEdit
            _clone = CType(Me.MemberwiseClone(), Customer)
        End Sub
        Public Sub EndEdit() Implements IEditableObject.EndEdit
            _clone = Nothing
        End Sub
        Public Sub CancelEdit() Implements IEditableObject.CancelEdit
            If _clone IsNot Nothing Then
                For Each p In Me.GetType().GetProperties()
                    If p.CanRead AndAlso p.CanWrite Then
                        p.SetValue(Me, p.GetValue(_clone, Nothing), Nothing)
                    End If
                Next p
            End If
        End Sub
#End Region
    End Class
End Namespace
