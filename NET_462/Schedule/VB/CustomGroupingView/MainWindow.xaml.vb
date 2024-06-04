Imports System.Text
Imports System.Windows.Markup
Imports C1.Schedule
Imports C1.WPF.Schedule
Imports C1.WPF
Imports System.Collections

Namespace CustomGroupingView
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name)
			InitializeComponent()
			' add some contacts
			Dim cnt As New Contact()
			cnt.Text = "Andy Garcia"
			c1Scheduler1.DataStorage.ContactStorage.Contacts.Add(cnt)
			cnt = New Contact()
			cnt.Text = "Nancy Drew"
			c1Scheduler1.DataStorage.ContactStorage.Contacts.Add(cnt)
			cnt = New Contact()
			cnt.Text = "Robert Clark"
			c1Scheduler1.DataStorage.ContactStorage.Contacts.Add(cnt)
			cnt = New Contact()
			cnt.Text = "James Doe"
			c1Scheduler1.DataStorage.ContactStorage.Contacts.Add(cnt)
			c1Scheduler1.GroupBy = "Contact"
		End Sub
	End Class

	''' <summary>
	''' Returns VisualIntervalCollection for the specified day and specified SchedulerGroup which can be used
	''' as an ItemsSource for VisualIntervalsPresenter control.
	''' </summary>
	''' <remarks>
	''' If converter parameter is "Self", return list of a single VisualIntervalGroup, to use it as ItemsSource for representing all-day area.
	''' In all other cases returns VisualItervalCollection containing time slots for the single day.
	''' </remarks>
	Public Class GroupItemToVisualIntervalsConverter
		Implements IValueConverter

		Public Shared [Default] As New GroupItemToVisualIntervalsConverter()

		#Region "IValueConverter Members"

		Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
			Dim el As FrameworkElement = TryCast(value, FrameworkElement)
			If el IsNot Nothing Then
				Dim group As SchedulerGroupItem = TryCast(el.DataContext, SchedulerGroupItem)
				Dim index As Integer = -1
				If group IsNot Nothing Then
					Dim itm As ItemsControl = TryCast(VTreeHelper.GetParentOfType(el, GetType(ItemsControl)), ItemsControl)
					If itm IsNot Nothing Then
						Dim data As Object = itm.DataContext

						Dim itmParent As ItemsControl = TryCast(VTreeHelper.GetParentOfType(itm, GetType(ItemsControl)), ItemsControl)
						If itmParent IsNot Nothing Then
							index = itmParent.Items.IndexOf(data)
							Dim visualIntervalGroup As VisualIntervalGroup = TryCast(group.VisualIntervalGroups(index), VisualIntervalGroup)
							Dim param As String = CStr(parameter)
							If param.ToLower() = "self" Then
								' create list of a single VisualIntervalGroup
								' (we need list to use it as ItemsSource)
								Dim list As New List(Of Object)()
								list.Add(visualIntervalGroup)
								Return list
							Else
								Return visualIntervalGroup.VisualIntervals
							End If
						End If
					End If
				End If
			End If
			Return Nothing
		End Function

		Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function

		#End Region
	End Class
End Namespace
