Namespace Printing
	''' <summary>
	''' Interaction logic for Window1.xaml
	''' </summary>
	Partial Public Class Window1
		Inherits Window
		Private _printInfo As PrintInfo

		Public Sub New()
			Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name)
			InitializeComponent()

			Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30)

			minutesGroup.Visibility = Visibility.Visible

			Scheduler.CalendarHelper.StartDayTime = TimeSpan.Parse("09:00:00")
			Scheduler.Settings.FirstVisibleTime = TimeSpan.Parse("09:00:00")

			Scheduler.CalendarHelper.EndDayTime = TimeSpan.Parse("18:00:00")
			_printInfo = New PrintInfo(Me.Scheduler)
		End Sub

		Private Sub rbChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			ChangeStyle()
		End Sub

		Private Sub ChangeStyle()
			If Not IsLoaded Then
				Return
			End If
            Dim curCursor As System.Windows.Input.Cursor = Cursor
            Cursor = System.Windows.Input.Cursors.Wait
			Scheduler.BeginUpdate()
			Try
				If oneDayRb.IsChecked.Value Then
					Scheduler.ChangeStyle(Scheduler.OneDayStyle)
					minutesGroup.Visibility = Visibility.Visible
					minutesChecked(Nothing, Nothing)
				ElseIf workingWeekRb.IsChecked.Value Then
					Scheduler.ChangeStyle(Scheduler.WorkingWeekStyle)
					minutesGroup.Visibility = Visibility.Visible
					minutesChecked(Nothing, Nothing)
				ElseIf weekRb.IsChecked.Value Then
					minutesGroup.Visibility = Visibility.Visible
					Scheduler.ChangeStyle(Scheduler.WeekStyle)
					minutesChecked(Nothing, Nothing)
				Else
					minutesGroup.Visibility = Visibility.Collapsed
					Scheduler.ChangeStyle(Scheduler.MonthStyle)
				End If
			Finally
				Scheduler.EndUpdate()
				Cursor = curCursor
			End Try
		End Sub

		Private Sub minutesChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If Not IsLoaded Then
				Return
			End If
            Dim curCursor As System.Windows.Input.Cursor = Cursor
            Cursor = System.Windows.Input.Cursors.Wait
			Try
				If m60.IsChecked.Value Then
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(60)
				ElseIf m30.IsChecked.Value Then
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(30)
				ElseIf m15.IsChecked.Value Then
					Scheduler.VisualIntervalScale = TimeSpan.FromMinutes(15)
				End If
			Finally
				Cursor = curCursor
			End Try
		End Sub

		Private Sub btnToday_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim targetDate As Date = Date.Today
            If Scheduler.Style IsNot Scheduler.OneDayStyle Then
                ' find the first week day
                Do While targetDate.DayOfWeek <> Scheduler.CalendarHelper.WeekStart
                    targetDate = targetDate.AddDays(-1)
                Loop
            End If

			' navigate to the current week 
			Scheduler.VisualStartTime = targetDate
		End Sub

		Private Sub btnPrev_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Scheduler.Style Is Scheduler.WorkingWeekStyle OrElse Scheduler.Style Is Scheduler.WeekStyle Then
                Scheduler.VisualStartTime = Scheduler.VisualStartTime.AddDays(-7)
            Else
                Scheduler.VisualStartTime = Scheduler.VisualStartTime.Add(Scheduler.VisualTimeSpan.Negate())
            End If

		End Sub
		Private Sub btnNext_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Scheduler.Style Is Scheduler.WorkingWeekStyle OrElse Scheduler.Style Is Scheduler.WeekStyle Then
                Scheduler.VisualStartTime = Scheduler.VisualStartTime.AddDays(7)
            Else
                Scheduler.VisualStartTime = Scheduler.VisualStartTime.Add(Scheduler.VisualTimeSpan)
            End If
		End Sub

		Private Sub Scheduler_LayoutUpdated(ByVal sender As Object, ByVal e As EventArgs)
			' refresh buttons' state if Scheduler's layout has been changed
			' by Scheduler's commands defined in default templates.
            If Scheduler.Style Is Scheduler.OneDayStyle Then
                oneDayRb.IsChecked = True
                minutesGroup.Visibility = Visibility.Visible
            ElseIf Scheduler.Style Is Scheduler.WorkingWeekStyle Then
                workingWeekRb.IsChecked = True
                minutesGroup.Visibility = Visibility.Visible
            ElseIf Scheduler.Style Is Scheduler.WeekStyle Then
                weekRb.IsChecked = True
                minutesGroup.Visibility = Visibility.Visible
            Else
                minutesGroup.Visibility = Visibility.Collapsed
                monthRb.IsChecked = True
            End If
		End Sub

		Private Sub Print_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_printInfo.Print()
		End Sub
		Private Sub PrintPreview_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_printInfo.Preview()
		End Sub
	End Class
End Namespace
