Imports System.Text
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace ImageColumnTemplate
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window
        Public Sub New()
            InitializeComponent()

            ' create some data items
            Dim list_Renamed = New List(Of Status)()
            For i As Integer = 0 To 49
                list_Renamed.Add(New Status() With {.Alert = i Mod 3, .Message = "item " & i.ToString()})
            Next i

            ' bind data to grid
            _flex.ItemsSource = list_Renamed

            ' limit edit options in the Alert column to 0, 1, and 2
            _flex.Columns(0).ValueConverter = New C1.WPF.FlexGrid.ColumnValueConverter(New Integer() {0, 1, 2}, True)
        End Sub
    End Class

    ''' <summary>
    ''' Custom cell factory that creates cells with images based on the 
    ''' value of the 'Alert' property in the underlying data item.
    ''' </summary>
    Public Class AlertImageConverter
        Implements IValueConverter
        ' load static images to show on the grid from application resources
        Private Shared _bmpRed, _bmpYellow, _bmpGreen As BitmapImage
        Shared Sub New()
            _bmpRed = New BitmapImage(New Uri("/Resources/redBell.png", UriKind.Relative))
            _bmpYellow = New BitmapImage(New Uri("/Resources/yellowBell.png", UriKind.Relative))
            _bmpGreen = New BitmapImage(New Uri("/Resources/greenBell.png", UriKind.Relative))
        End Sub

        ' convert 'Alert' int value into corresponding image
        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Select Case CInt(Fix(value))
                Case 1
                    Return _bmpRed
                Case 2
                    Return _bmpYellow
            End Select
            Return _bmpGreen
        End Function

        ' one-way conversion only (ConvertBack is not used)
        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

    ''' <summary>
    ''' Our data item.
    ''' </summary>
    Public Class Status
        Implements INotifyPropertyChanged
        Private _alert As Integer
        Private _msg As String

        <Display(Name:="Alert")>
        Public Property Alert() As Integer
            Get
                Return _alert
            End Get
            Set(ByVal value As Integer)
                _alert = value
                OnPropertyChanged("Alert")
            End Set
        End Property

        <Display(Name:="Message")>
        Public Property Message() As String
            Get
                Return _msg
            End Get
            Set(ByVal value As String)
                _msg = value
                OnPropertyChanged("Message")
            End Set
        End Property
        Private Sub OnPropertyChanged(ByVal propName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propName))
        End Sub
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
