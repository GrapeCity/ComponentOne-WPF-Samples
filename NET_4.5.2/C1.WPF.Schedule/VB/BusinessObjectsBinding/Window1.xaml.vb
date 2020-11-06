Namespace BusinessObjectsBinding
	''' <summary>
	''' Interaction logic for Window1.xaml
	''' </summary>
	Partial Public Class Window1
		Inherits Window
		Private _list As AppointmentBOList = Nothing

		Public Sub New()
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name)
			' create collection of custom business objects
			_list = New AppointmentBOList()
			InitializeComponent()
			AddHandler scheduler1.StyleChanged, AddressOf scheduler1_StyleChanged
			scheduler1_StyleChanged(Nothing, Nothing)
		End Sub

		Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' add a new AppointmentBusinessObject (that creates a new appointment in a C1Scheduler control)
			Dim newObject As New AppointmentBusinessObject()
			newObject.Subject = "The test business object " & _list.Count.ToString()
			_list.Add(newObject)
		End Sub

		Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' clear the list of business objects (that clears all appointments from the C1Scheduler control)
			_list.Clear()
		End Sub

		''' <summary>
		''' Gets a collection of custom business objects, used as a data source for C1Scheduler's AppointmentStorage.
		''' </summary>
		Public ReadOnly Property Appointments() As AppointmentBOList
			Get
				Return _list
			End Get
		End Property

		Private Sub scheduler1_StyleChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' update toolbar buttons state according to the current C1Scheduler view
            If scheduler1.Style Is scheduler1.WorkingWeekStyle Then
                btnWorkWeek.IsChecked = True
            ElseIf scheduler1.Style Is scheduler1.WeekStyle Then
                btnWeek.IsChecked = True
            ElseIf scheduler1.Style Is scheduler1.MonthStyle Then
                btnMonth.IsChecked = True
            ElseIf scheduler1.Style Is scheduler1.OneDayStyle Then
                btnDay.IsChecked = True
            Else
                btnTimeLine.IsChecked = True
            End If

		End Sub
	End Class
End Namespace
