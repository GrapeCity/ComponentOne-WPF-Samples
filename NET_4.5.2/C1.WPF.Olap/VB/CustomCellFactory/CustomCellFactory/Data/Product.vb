Imports System.ComponentModel
Imports System.Collections.ObjectModel

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
Public Class Product
    Implements INotifyPropertyChanged
    Implements IEditableObject

    Shared _lines As String() = "Computers|Washers|Stoves".Split("|"c)
    Shared _colors As String() = "Red|Green|Blue|White".Split("|"c)

    ' object model
    Public Property Line() As String
        Get
            Return DirectCast(GetValue("Line"), String)
        End Get
        Set(value As String)
            SetValue("Line", value)
        End Set
    End Property
    Public Property Color() As String
        Get
            Return DirectCast(GetValue("Color"), String)
        End Get
        Set(value As String)
            SetValue("Color", value)
        End Set
    End Property
    Public Property Name() As String
        Get
            Return DirectCast(GetValue("Name"), String)
        End Get
        Set(value As String)
            SetValue("Name", value)
        End Set
    End Property
    Public Property Price() As Double
        Get
            Return CDbl(GetValue("Price"))
        End Get
        Set(value As Double)
            SetValue("Price", value)
        End Set
    End Property
    Public Property Weight() As Double
        Get
            Return CDbl(GetValue("Weight"))
        End Get
        Set(value As Double)
            SetValue("Weight", value)
        End Set
    End Property
    Public Property Cost() As Double
        Get
            Return CDbl(GetValue("Cost"))
        End Get
        Set(value As Double)
            SetValue("Cost", value)
        End Set
    End Property
    Public Property Volume() As Double
        Get
            Return CDbl(GetValue("Volume"))
        End Get
        Set(value As Double)
            SetValue("Volume", value)
        End Set
    End Property
    Public Property Discontinued() As Boolean
        Get
            Return CBool(GetValue("Discontinued"))
        End Get
        Set(value As Boolean)
            SetValue("Discontinued", value)
        End Set
    End Property
    Public Property Rating() As Integer
        Get
            Return CInt(GetValue("Rating"))
        End Get
        Set(value As Integer)
            SetValue("Rating", value)
        End Set
    End Property

    ' get/set values
    Private _values As New Dictionary(Of String, Object)()
    Private Function GetValue(p As String) As Object
        Dim value As Object
        _values.TryGetValue(p, value)
        Return value
    End Function
    Private Sub SetValue(p As String, value As Object)
        If Not Object.Equals(value, GetValue(p)) Then
            _values(p) = value
            OnPropertyChanged(p)
        End If
    End Sub
    Protected Overridable Sub OnPropertyChanged(p As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(p))
    End Sub

    ' factory
    Public Shared Function GetProducts(count As Integer) As ObservableCollection(Of Product)
        Dim list = New ObservableCollection(Of Product)()
        Dim rnd = New Random(0)
        For i As Integer = 0 To count - 1
            Dim p = New Product()
            p.Line = _lines(rnd.[Next]() Mod _lines.Length)
            p.Color = _colors(rnd.[Next]() Mod _colors.Length)
            p.Name = String.Format("{0} {1}{2}", p.Line.Substring(0, p.Line.Length - 1), p.Line(0), i)
            p.Price = rnd.[Next](1, 1000)
            p.Weight = rnd.[Next](1, 100)
            p.Cost = rnd.[Next](1, 600)
            p.Volume = rnd.[Next](500, 5000)
            p.Discontinued = rnd.NextDouble() < 0.1
            p.Rating = rnd.[Next](0, 5)
            list.Add(p)
        Next
        Return list
    End Function
    Public Shared Function GetLines() As String()
        Return _lines
    End Function
    Public Shared Function GetColors() As String()
        Return _colors
    End Function

#Region "INotifyPropertyChanged Members"

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region "IEditableObject Members"

    Shared _clone As Dictionary(Of String, Object)
    Public Sub BeginEdit() Implements IEditableObject.BeginEdit
        If _clone Is Nothing Then
            _clone = New Dictionary(Of String, Object)()
        End If
        _clone.Clear()
        For Each key As Object In _values.Keys
            _clone(key) = _values(key)
        Next
    End Sub
    Public Sub CancelEdit() Implements IEditableObject.CancelEdit
        _values.Clear()
        For Each key As Object In _clone.Keys
            _values(key) = _clone(key)
        Next
        OnPropertyChanged(Nothing)
    End Sub
    Public Sub EndEdit() Implements IEditableObject.EndEdit
    End Sub

#End Region


End Class

