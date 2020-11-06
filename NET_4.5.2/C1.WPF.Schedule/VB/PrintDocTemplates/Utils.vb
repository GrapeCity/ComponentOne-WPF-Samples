Imports C1.C1Preview
Imports C1.C1Preview.DataBinding
Imports C1.C1Schedule

Namespace PrintDocTemplates
	''' <summary>
	''' Contains static methods filling document for different views
	''' </summary>
	Public NotInheritable Class Utils
		''' <summary>
		''' Fills specified control with a Memo style.
		''' </summary>
		''' <param name="doc"></param>
		Private Sub New()
		End Sub
		Public Shared Sub MakeMemo(ByVal doc As C1PrintDocument)
			doc.Clear()

			doc.DocumentInfo.Title = "Memo style"
			doc.DocumentInfo.Subject = "Memo"

			AddNamespaces(doc)

			AddHeadersFooters(doc)
			doc.Tags("AppointmentsFont").Value = New Font("Arial", 10)

			' add string tags (localizable)

			Dim newTag As New Tag("Subject", "Subject:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("Location", "Location:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("Start", "Start:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("End", "End:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("ShowTimeAs", "Show Time As:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("Recurrence", "Recurrence:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("None", "none", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("Categories", "Categories:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("Resources", "Resources:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("Contacts", "Contacts:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("Importance", "Importance:", GetType(String))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			Dim ra As New RenderArea()
			ra.Width = "100%"
			ra.BreakBefore = BreakEnum.Page
			ra.DataBinding.DataSource = New Expression("Document.Tags!Appointments.Value")
			ra.DataBinding.Sorting.Expressions.Add("Fields!Start.Value")
			ra.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value")
			ra.Style.Borders.Top = LineDef.DefaultBold
			ra.Stacking = StackingRulesEnum.InlineLeftToRight
			ra.Style.Padding.Top = "1mm"
			ra.Style.Spacing.All = "1mm"

			Dim rt As New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Parent.Original.DataBinding.Fields!Subject.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)" 'VisibilityEnum
			rt.Text = "[Document.Tags!Subject.Value]"
			rt.Width = "25%"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Parent.Original.DataBinding.Fields!Subject.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Fields!Subject.Value]"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Parent.Original.DataBinding.Fields!Location.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Document.Tags!Location.Value]"
			rt.Width = "25%"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Parent.Original.DataBinding.Fields!Location.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Fields!Location.Value]"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.Text = "[Document.Tags!Start.Value]"
			rt.Width = "25%"
			rt.Style.Padding.Top = "2mm"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.Text = "[String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:ddd} {1:g}"", Fields!Start.Value, Fields!Start.Value)]"
			rt.Width = "75%"
			rt.Style.Padding.Top = "2mm"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.Text = "[Document.Tags!End.Value]"
			rt.Width = "25%"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.Text = "[String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:ddd} {1:g}"", Fields!End.Value, Fields!End.Value)]"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.Text = "[Document.Tags!ShowTimeAs.Value]"
			rt.Width = "25%"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.Text = "[Fields!BusyStatus.Value.Text]"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.Text = "[Document.Tags!Recurrence.Value]"
			rt.Width = "25%"
			rt.Style.Padding.Top = "2mm"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "Dim index as Integer = RenderObject.Parent.Original.DataBinding.RowNumber - 1 " & vbCrLf & "Dim app As Appointment = Document.Tags!Appointments.Value(index)" & vbCrLf & "Dim text As RenderText = RenderObject" & vbCrLf & "If app.RecurrenceState <> RecurrenceStateEnum.NotRecurring Then" & vbCrLf & "    text.Text = app.GetRecurrencePattern().Description" & vbCrLf & "Else" & vbCrLf & "    text.Text = Document.Tags!None.Value" & vbCrLf & "End If"
			rt.Style.Padding.Top = "2mm"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf((Not (RenderObject.Parent.Original.DataBinding.Fields!Links.Value Is Nothing)) And RenderObject.Parent.Original.DataBinding.Fields!Links.Value.Count > 0, VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Document.Tags!Contacts.Value]"
			rt.Style.Padding.Top = "2mm"
			rt.Width = "25%"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf((Not (RenderObject.Parent.Original.DataBinding.Fields!Links.Value Is Nothing)) And RenderObject.Parent.Original.DataBinding.Fields!Links.Value.Count > 0, VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Fields!Links.Value.ToString()]"
			rt.Style.Padding.Top = "2mm"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf((Not (RenderObject.Parent.Original.DataBinding.Fields!Categories.Value Is Nothing)) And RenderObject.Parent.Original.DataBinding.Fields!Categories.Value.Count > 0, VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Document.Tags!Categories.Value]"
			rt.Style.Padding.Top = "2mm"
			rt.Width = "25%"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf((Not (RenderObject.Parent.Original.DataBinding.Fields!Categories.Value Is Nothing)) And RenderObject.Parent.Original.DataBinding.Fields!Categories.Value.Count > 0, VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Fields!Categories.Value.ToString()]"
			rt.Style.Padding.Top = "2mm"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf((Not (RenderObject.Parent.Original.DataBinding.Fields!Resources.Value Is Nothing)) And RenderObject.Parent.Original.DataBinding.Fields!Resources.Value.Count > 0, VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Document.Tags!Resources.Value]"
			rt.Width = "25%"
			rt.Style.Padding.Top = "2mm"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf((Not (RenderObject.Parent.Original.DataBinding.Fields!Resources.Value Is Nothing)) And RenderObject.Parent.Original.DataBinding.Fields!Resources.Value.Count > 0, VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Fields!Resources.Value.ToString()]"
			rt.Style.Padding.Top = "2mm"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not (RenderObject.Parent.Original.DataBinding.Fields!Importance.Value = 1), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "[Document.Tags!Importance.Value]"
			rt.Style.Padding.Top = "2mm"
			rt.Width = "25%"
			rt.Style.FontBold = True
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not (RenderObject.Parent.Original.DataBinding.Fields!Importance.Value = 1), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Style.Padding.Top = "2mm"
			rt.Text = "[Fields!Importance.Value.ToString()]"
			rt.Width = "75%"
			ra.Children.Add(rt)

			rt = New RenderText()
			rt.Text = "[Fields!Body.Value.ToString()]"
			rt.Style.Padding.Top = "3mm"
			rt.Style.Padding.Right = "2mm"
			rt.Width = "100%"
			ra.Children.Add(rt)

			doc.Body.Children.Add(ra)

			AddCommonTags(doc)

			newTag = New Tag("Appointments", Nothing, GetType(IList(Of Appointment)))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
		End Sub

		''' <summary>
		''' Fills specified control with a Details style.
		''' </summary>
		''' <param name="doc"></param>
		Public Shared Sub MakeDetails(ByVal doc As C1PrintDocument)
			doc.Clear()

			doc.DocumentInfo.Title = "Details style"
			doc.DocumentInfo.Subject = "Details"
			AddNamespaces(doc)

			AddHeadersFooters(doc)
            doc.Tags("FooterRight").Value = "[GeneratedDateTime]"

			doc.DocumentStartingScript &= "If Tags!InsertPageBreaks.Value And Tags!PageBreak.Value <> ""Day"" Then " & vbCrLf & "	If Tags.IndexByName(""LastDate"") = -1 Then" & vbCrLf & "	    Dim tag As Tag" & vbCrLf & "       tag = New Tag(""LastDate"", Tags!StartDate.Value.Date)" & vbCrLf & "       Tags.Add(tag)" & vbCrLf & "   Else" & vbCrLf & "       Tags!LastDate.Value = Tags!StartDate.Value.Date" & vbCrLf & "   End If" & vbCrLf & "End If" & vbCrLf & "Dim dateAppointments As New DateAppointmentsCollection(Tags!StartDate.Value, Tags!EndDate.Value, Tags!Appointments.Value, false)" & vbCrLf & "If Tags.IndexByName(""DateAppointments"") = -1 Then" & vbCrLf & "   Dim tagApps As Tag" & vbCrLf & "   tagApps = New Tag(""DateAppointments"", dateAppointments)" & vbCrLf & "   tagApps.SerializeValue = False" & vbCrLf & "   Tags.Add(tagApps)" & vbCrLf & "Else" & vbCrLf & "   Tags!DateAppointments.Value = dateAppointments" & vbCrLf & "End If"

			' RenderArea representing the single day
			Dim ra1 As New RenderArea()
			ra1.DataBinding.DataSource = New Expression("Document.Tags!DateAppointments.Value")
			ra1.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			ra1.FormatDataBindingInstanceScript = "If Tags!InsertPageBreaks.Value Then" & vbCrLf & "	 Dim newDate as DateTime" & vbCrLf & "	 newDate = RenderObject.Original.DataBinding.Fields!Date.Value" & vbCrLf & "    Select Case Tags!PageBreak.Value" & vbCrLf & "		Case ""Week""" & vbCrLf & "			Dim tmp as DateTime" & vbCrLf & "			tmp = Tags!LastDate.Value.Date.AddDays(1)" & vbCrLf & "			While tmp <= newDate" & vbCrLf & "				If tmp.DayOfWeek = Tags!CalendarInfo.Value.WeekStart Then" & vbCrLf & "					RenderObject.BreakBefore = BreakEnum.Page" & vbCrLf & "					Exit While" & vbCrLf & "				End If" & vbCrLf & "				tmp = tmp.AddDays(1)" & vbCrLf & "			End While" & vbCrLf & "		Case ""Month""" & vbCrLf & "			If Tags!LastDate.Value.Month <> newDate.Month Then" & vbCrLf & "				RenderObject.BreakBefore = BreakEnum.Page" & vbCrLf & "			End If" & vbCrLf & "		Case Else" & vbCrLf & "			RenderObject.BreakBefore = BreakEnum.Page" & vbCrLf & "    End Select" & vbCrLf & "    Tags!LastDate.Value = newDate" & vbCrLf & "End If"

			ra1.Style.Spacing.All = "1mm"

			' day header
			Dim rt As New RenderText("[String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:D}"", Fields!Date.Value)]")
			rt.FormatDataBindingInstanceScript = "RenderObject.Style.Font = Document.Tags!DateHeadingsFont.Value"
			rt.Style.Borders.All = LineDef.Default
			rt.Style.Padding.All = "2mm"
			rt.Style.BackColor = Colors.LightGray

			ra1.Children.Add(rt)

			' RenderArea tepresenting the single appointment
			Dim raApp As New RenderArea()
			raApp.Style.Spacing.All = "2mm"
			raApp.Stacking = StackingRulesEnum.InlineLeftToRight

			' Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
			raApp.DataBinding.DataSource = New Expression("Parent.Fields!Appointments.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value")

			rt = New RenderText()
			rt.Text = "[IIf(Fields!AllDayEvent.Value, ""All Day"", String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:t} - {1:t}"", " & "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, DataBinding.Parent.Fields!Date.Value, Fields!Start.Value), " & "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value.AddDays(1)), CDate(Fields!End.Value)) > 0, DataBinding.Parent.Fields!Date.Value.AddDays(1), Fields!End.Value)))]"
			rt.Width = "25%"
			rt.Style.FontBold = True
			raApp.Children.Add(rt)

			Dim appDetails As New RenderArea()
			appDetails.Width = "75%"
			appDetails.Stacking = StackingRulesEnum.InlineLeftToRight

			Dim rt1 As New RenderText()
			rt1.Text = "[Fields!Subject.Value] "
			rt1.Style.FontBold = True
			rt1.Width = "Auto"
			appDetails.Children.Add(rt1)

			rt1 = New RenderText()
			rt1.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Original.DataBinding.Parent.Fields!Location.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt1.Text = "[""-- "" & Fields!Location.Value]"
			rt1.Style.FontBold = True
			rt1.Width = "Auto"
			appDetails.Children.Add(rt1)

			rt1 = New RenderText()
			rt1.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Original.DataBinding.Parent.Fields!Body.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse) " & vbCrLf & "RenderObject.BreakBefore = IIf(Not String.IsNullOrEmpty(RenderObject.Original.DataBinding.Parent.Fields!Subject.Value & RenderObject.Original.DataBinding.Parent.Fields!Location.Value), BreakEnum.Line, BreakEnum.None)"
			rt1.Text = "[Fields!Body.Value]"
			appDetails.Children.Add(rt1)

			rt1 = New RenderText()
			rt1.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(DateDiff(DateInterval.Second, CDate(RenderObject.Original.DataBinding.Parent.Parent.Fields!Date.Value), CDate(RenderObject.Original.DataBinding.Parent.Fields!Start.Value)) < 0, VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt1.Text = "[""Please See Above""]"
			rt1.Style.FontBold = True
			rt1.BreakBefore = BreakEnum.Line
			appDetails.Children.Add(rt1)

			raApp.Children.Add(appDetails)

			ra1.Children.Add(raApp)
			doc.Body.Children.Add(ra1)

			AddCommonTags(doc)

			Dim newTag As New Tag("Appointments", Nothing, GetType(IList(Of Appointment)))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
			newTag = New Tag("StartDate", Date.Today, GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			doc.Tags.Add(newTag)
			newTag = New Tag("EndDate", Date.Today.AddDays(1), GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			doc.Tags.Add(newTag)
			newTag = New Tag("InsertPageBreaks", False, GetType(Boolean))
			newTag.Description = "Start a new page each"
			doc.Tags.Add(newTag)
			newTag = New Tag("PageBreak", "Day", GetType(String))
			newTag.Description = ""
			newTag.InputParams = New TagListInputParams(TagListInputParamsTypeEnum.ComboBox, New TagListInputParamsItem("Day", "Day"), New TagListInputParamsItem("Week", "Week"), New TagListInputParamsItem("Month", "Month"))
			doc.Tags.Add(newTag)
			newTag = New Tag("HidePrivateAppointments", False, GetType(Boolean))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
		End Sub

		''' <summary>
		''' Fills specified control with a Month style.
		''' </summary>
		''' <param name="doc"></param>
		Public Shared Sub MakeMonth(ByVal doc As C1PrintDocument)
			doc.Clear()

			doc.DocumentInfo.Title = "Monthly style"
			doc.DocumentInfo.Subject = "Month"
			AddNamespaces(doc)

			AddHeadersFooters(doc)
			doc.PageLayout.PageSettings.Landscape = True
            doc.Tags("FooterRight").Value = "[GeneratedDateTime]"
			doc.Tags("DateHeadingsFont").Value = New Font("Segoe UI", 20, System.Drawing.FontStyle.Bold)

			doc.DocumentStartingScript &= "Dim daysNumber As Long = 7.0 " & vbCrLf & "If Document.Tags!WorkDaysOnly.Value Then" & vbCrLf & "   daysNumber = Document.Tags!CalendarInfo.Value.WorkDays.Count " & vbCrLf & "End If" & vbCrLf & "Document.Tags!DayWidth.Value = New Unit((97.3/daysNumber - 0.005).ToString(""#.###"", System.Globalization.CultureInfo.InvariantCulture) & ""%"") " & vbCrLf & "Dim tasksNumber As Integer = 0 " & vbCrLf & "If Document.Tags!IncludeTasks.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If Document.Tags!IncludeBlankNotes.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If tasksNumber = 1 Then" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""100%"")" & vbCrLf & "ElseIf tasksNumber = 2 Then" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""50%"")" & vbCrLf & "Else" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""33.3%"")" & vbCrLf & "End If" & vbCrLf & "Dim startT As DateTime = Tags!StartDate.Value  " & vbCrLf & "Dim endT As DateTime = Tags!EndDate.Value  " & vbCrLf & "While startT.DayOfWeek <> Tags!CalendarInfo.Value.WeekStart " & vbCrLf & "    startT = startT.AddDays(-1)" & vbCrLf & "End While" & vbCrLf & "Dim days As Long = DateDiff(DateInterval.Day, startT, endT )" & vbCrLf & "endT = endT.AddDays(35 - (days Mod 35))" & vbCrLf & "Dim months As New List(Of Date)" & vbCrLf & "Dim s As DateTime = startT" & vbCrLf & "While s < endT" & vbCrLf & "	months.Add(s)" & vbCrLf & "   s = s.AddDays(35)" & vbCrLf & "End While" & vbCrLf & "If Tags.IndexByName(""Months"") = -1 Then" & vbCrLf & "   Dim tagMonths As Tag" & vbCrLf & "   tagMonths = New Tag(""Months"", months)" & vbCrLf & "   tagMonths.SerializeValue = False" & vbCrLf & "   Tags.Add(tagMonths)" & vbCrLf & "Else" & vbCrLf & "   Tags!Months.Value = months" & vbCrLf & "End If" & vbCrLf & vbCrLf & "Dim dateAppointments As New DateAppointmentsCollection(startT, endT, Tags!Appointments.Value, Tags!CalendarInfo.Value, True, Not Tags!WorkDaysOnly.Value)" & vbCrLf & "If Tags.IndexByName(""DateAppointments"") = -1 Then" & vbCrLf & "   Dim tagApps As Tag" & vbCrLf & "   tagApps = New Tag(""DateAppointments"", dateAppointments)" & vbCrLf & "   tagApps.SerializeValue = False" & vbCrLf & "   Tags.Add(tagApps)" & vbCrLf & "Else" & vbCrLf & "   Tags!DateAppointments.Value = dateAppointments" & vbCrLf & "End If" & vbCrLf

			' RenderArea representing the single page
			Dim raPage As New RenderArea()
			raPage.DataBinding.DataSource = New Expression("Document.Tags!Months.Value")
			raPage.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			raPage.BreakBefore = BreakEnum.Page
			raPage.Width = "100%"
			raPage.Height = "100%"
			raPage.Stacking = StackingRulesEnum.InlineLeftToRight

'			#Region "** month header"
			' month header
			Dim raMonthHeader As New RenderArea()
			raMonthHeader.Style.Borders.All = LineDef.Default
			raMonthHeader.Width = "100%"
			raMonthHeader.Height = "28mm"
			raMonthHeader.Stacking = StackingRulesEnum.InlineLeftToRight

			Dim rt As New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Style.Font = Document.Tags!DateHeadingsFont.Value" & vbCrLf & "Dim rt As RenderText = RenderObject" & vbCrLf & "Dim startDate As Date = RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value" & vbCrLf & "Dim endDate As Date = startDate.AddDays(34)" & vbCrLf & "Dim txt As String = startDate.ToString(""MMMM"", Document.Tags!CalendarInfo.Value.CultureInfo)" & vbCrLf & "If startDate.Year <> endDate.Year Then" & vbCrLf & "    txt = txt & "" "" & startDate.ToString(""yyy"", Document.Tags!CalendarInfo.Value.CultureInfo)" & vbCrLf & "End If" & vbCrLf & "txt = txt & "" - "" & endDate.ToString(""MMMM yyy"", Document.Tags!CalendarInfo.Value.CultureInfo)" & vbCrLf & "rt.Text = txt" & vbCrLf & "Document.Tags!WeekTabs.Value = New List(of Date)" & vbCrLf & "Document.Tags!WeekTabs.Value.Add(startDate)" & vbCrLf & "Document.Tags!WeekTabs.Value.Add(startDate.AddDays(7))" & vbCrLf & "Document.Tags!WeekTabs.Value.Add(startDate.AddDays(14))" & vbCrLf & "Document.Tags!WeekTabs.Value.Add(startDate.AddDays(21))" & vbCrLf & "Document.Tags!WeekTabs.Value.Add(startDate.AddDays(28))" & vbCrLf & "Document.Tags!MonthCalendars.Value = New List(of Date)" & vbCrLf & "startDate = New Date(startDate.Year, startDate.Month, 1)" & vbCrLf & "While startDate <= endDate" & vbCrLf & "	Document.Tags!MonthCalendars.Value.Add(startDate)" & vbCrLf & "	startDate = startDate.AddMonths(1)" & vbCrLf & "End While"
			rt.Style.TextAlignVert = AlignVertEnum.Center
			rt.Style.Spacing.Left = "2mm"
			rt.Height = "100%"
			rt.Width = "parent.Width - 102mm"

			raMonthHeader.Children.Add(rt)

			Dim monthCalendar As New RenderArea()
			monthCalendar.DataBinding.DataSource = New Expression("Document.Tags!MonthCalendars.Value")
			monthCalendar.Stacking = StackingRulesEnum.InlineLeftToRight
			monthCalendar.Style.Spacing.Left = "1mm"
			monthCalendar.Style.Spacing.Right = "1mm"
			monthCalendar.Style.Spacing.Top = "0.5mm"
			monthCalendar.Width = "34mm"

			rt = New RenderText("[CDate(Fields!Date.Value).ToString(""MMMM yyy"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.FormatDataBindingInstanceScript = "Dim startDate As Date = RenderObject.Original.Parent.DataBinding.Fields!Date.Value" & vbCrLf & "Dim endDate As Date = startDate.AddMonths(1).AddDays(-1)" & vbCrLf & "While startDate.DayOfWeek <> Document.Tags!CalendarInfo.Value.WeekStart " & vbCrLf & "    startDate = startDate.AddDays(-1)" & vbCrLf & "End While" & vbCrLf & "Document.Tags!WeekNumber.Value = New List(of Date)" & vbCrLf & "While startDate <= endDate" & vbCrLf & "	Document.Tags!WeekNumber.Value.Add(startDate)" & vbCrLf & "	startDate = startDate.AddDays(7)" & vbCrLf & "End While"
			rt.Style.TextAlignHorz = AlignHorzEnum.Center
			rt.Style.Font = New Font("Segoe UI", 8)
			rt.Width = "100%"
			monthCalendar.Children.Add(rt)

			rt = New RenderText(" ")
			rt.Style.Font = New Font("Arial", 7f)
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			monthCalendar.Children.Add(rt)

			rt = New RenderText("[Document.Tags!CalendarInfo.Value.CultureInfo.DateTimeFormat.GetShortestDayName(CDate(Fields!Date.Value).DayOfWeek)]")
			rt.DataBinding.DataSource = New Expression("New DateAppointmentsCollection(Document.Tags!WeekNumber.Value(0), Document.Tags!WeekNumber.Value(0).AddDays(6), Document.Tags!Appointments.Value, True)")
			rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			rt.Style.Borders.Bottom = LineDef.Default
			rt.Style.Font = New Font("Arial", 7f)
			rt.Style.TextAlignHorz = AlignHorzEnum.Center
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			monthCalendar.Children.Add(rt)

			Dim raWeek As New RenderArea()
			raWeek.DataBinding.DataSource = New Expression("Document.Tags!WeekNumber.Value")
			raWeek.Style.Font = New Font("Arial", 7f)
			raWeek.Width = "100%"
			raWeek.Stacking = StackingRulesEnum.InlineLeftToRight

			rt = New RenderText("[Document.Tags!CalendarInfo.Value.CultureInfo.Calendar.GetWeekOfYear(CDate(Fields!Date.Value), System.Globalization.CalendarWeekRule.FirstDay, Document.Tags!CalendarInfo.Value.WeekStart)]")
			rt.Style.Borders.Right = LineDef.Default
			rt.Style.TextAlignHorz = AlignHorzEnum.Right
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			raWeek.Children.Add(rt)

			rt = New RenderText("[CDate(Fields!Date.Value).ToString(""%d"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.DataBinding.DataSource = New Expression("New DateAppointmentsCollection(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value).AddDays(6), Document.Tags!Appointments.Value, True)")
			rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			rt.FormatDataBindingInstanceScript = "If RenderObject.Original.DataBinding.Fields!Date.Value.Month <> RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value.Month Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & "Else If RenderObject.Original.DataBinding.Fields!HasAppointments.Value Then" & vbCrLf & "    RenderObject.Style.FontBold = true" & vbCrLf & "End If"
			rt.Style.TextAlignHorz = AlignHorzEnum.Right
			rt.Width = "12.5%"
			raWeek.Children.Add(rt)
			monthCalendar.Children.Add(raWeek)

			raMonthHeader.Children.Add(monthCalendar)

			raPage.Children.Add(raMonthHeader)
'			#End Region

'			#Region "** month"
			' month 
			Dim raMonth As New RenderArea()
			raMonth.FormatDataBindingInstanceScript = "If Not (Document.Tags!IncludeTasks.Value Or Document.Tags!IncludeBlankNotes.Value Or Document.Tags!IncludeLinedNotes.Value) Then" & vbCrLf & "    RenderObject.Width = ""100%"" " & vbCrLf & "End If"
			raMonth.Style.Spacing.Top = "0.5mm"
			raMonth.Style.Borders.Top = LineDef.Default
			raMonth.Style.Borders.Left = LineDef.Default
			raMonth.Style.Borders.Bottom = LineDef.Default
			raMonth.Width = "80%"
			raMonth.Height = "parent.Height - 28mm"
			raMonth.Stacking = StackingRulesEnum.InlineLeftToRight

			rt = New RenderText(" ")
			rt.Style.WordWrap = False
			rt.Height = "4%"
			rt.Style.TextAngle = 90
			rt.Width = "2.7%"
			raMonth.Children.Add(rt)

			rt = New RenderText("[CDate(Fields!Date.Value).ToString(""dddd"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.DataBinding.DataSource = New Expression("Document.Tags!DateAppointments.Value(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value).AddDays(6))")
			rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			rt.FormatDataBindingInstanceScript = "RenderObject.Width = Document.Tags!DayWidth.Value"
			rt.Style.Borders.Bottom = LineDef.Default
			rt.Style.Borders.Right = LineDef.Default
			rt.Style.TextAlignHorz = AlignHorzEnum.Center
			rt.Style.WordWrap = False
			rt.Height = "4%"
			raMonth.Children.Add(rt)

			Dim raWeekTab As New RenderArea()
			raWeekTab.DataBinding.DataSource = New Expression("Document.Tags!WeekTabs.Value")
			raWeekTab.Height = "19.2%"
			raWeekTab.Width = "100%"
			raWeekTab.Stacking = StackingRulesEnum.InlineLeftToRight

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "Dim txt As RenderText = RenderObject" & vbCrLf & "Dim start As Date = RenderObject.Parent.Original.DataBinding.Fields!Date.Value" & vbCrLf & "Dim endT As Date = start.AddDays(6)" & vbCrLf & "If start.Month = endT.Month Then " & vbCrLf & "	txt.Text = start.ToString(""%d"", Document.Tags!CalendarInfo.Value.CultureInfo) & "" - "" & endT.ToString(""d.MM"", Document.Tags!CalendarInfo.Value.CultureInfo)" & vbCrLf & "Else" & vbCrLf & "	txt.Text = start.ToString(""d.MM"", Document.Tags!CalendarInfo.Value.CultureInfo) & "" - "" & endT.ToString(""d.MM"", Document.Tags!CalendarInfo.Value.CultureInfo)" & vbCrLf & "End If"
			rt.Style.Borders.Right = LineDef.Default
			rt.Style.FontBold = True
			rt.Style.TextAngle = 90
			rt.Style.TextAlignHorz = AlignHorzEnum.Center
			rt.Style.WordWrap = False
			rt.Height = "100%"
			rt.Width = "2.7%"
			raWeekTab.Children.Add(rt)

			' RenderArea representing the single day
			Dim raDay As New RenderArea()
			raDay.DataBinding.DataSource = New Expression("Document.Tags!DateAppointments.Value(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value).AddDays(6))")
			raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			raDay.Style.Borders.Right = LineDef.Default
			raDay.Style.Borders.Bottom = LineDef.Default
			raDay.FormatDataBindingInstanceScript = "RenderObject.Width = Document.Tags!DayWidth.Value"
			raDay.Height = "100%"

			' day header
			rt = New RenderText("[IIF(Fields!Date.Value.Day = 1 Or CDate(Fields!Date.Value) = CDate(Document.Tags!WeekTabs.Value(0).Date), String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:m}"", Fields!Date.Value) ,Fields!Date.Value.Day)]")
			rt.Style.Borders.Bottom = LineDef.Default
			rt.Style.Padding.Left = "1mm"
			rt.Style.FontBold = True
			rt.Style.BackColor = Colors.White
			rt.Style.TextAlignHorz = AlignHorzEnum.Left
			rt.Style.WordWrap = False

			raDay.Children.Add(rt)

			Dim status As New RenderText(" ")
			status.FormatDataBindingInstanceScript = "If IsNothing(RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value) Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse" & vbCrLf & "Else " & vbCrLf & "    RenderObject.Style.Brush = RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value.Brush.Brush " & vbCrLf & "End If"
			status.Width = "100%"
			status.Height = "1.5mm"
			raDay.Children.Add(status)

			Dim appointments As RenderArea = New RenderArea()
			appointments.Height = "parent.Height - Y - 2.5mm"

			' RenderArea representing the single appointment
			Dim raApp As New RenderArea()
			raApp.Style.Spacing.All = "0.2mm"
			raApp.FormatDataBindingInstanceScript = "If RenderObject.Original.DataBinding.Fields!AllDayEvent.Value Then" & vbCrLf & "    RenderObject.Style.Borders.All = LineDef.Default" & vbCrLf & "End If" & vbCrLf & "If Not IsNothing(RenderObject.Original.DataBinding.Fields!Label.Value) Then" & vbCrLf & "    RenderObject.Style.Brush = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label).Brush.Brush" & vbCrLf & "End If"
			raApp.Stacking = StackingRulesEnum.InlineLeftToRight
			raApp.Height = "14pt"

			' Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
			raApp.DataBinding.DataSource = New Expression("Parent.Fields!Appointments.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value")

			rt = New RenderText()
			rt.Style.WordWrap = False
			rt.Text = "[IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, ""> "", String.Empty) & " _
			 & "IIf(Fields!AllDayEvent.Value, """", String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:t}"", " _
			 & "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, DataBinding.Parent.Fields!Date.Value, Fields!Start.Value)))] [Fields!Subject.Value]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Original.DataBinding.Parent.Fields!Location.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "["" ("" & Fields!Location.Value & "")""]"
			rt.Style.WordWrap = False
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			appointments.Children.Add(raApp)
			raDay.Children.Add(appointments)

			Dim overflowArea As RenderArea = New RenderArea()
			overflowArea.Name = "overflow"
			Dim arrow As RenderText = New RenderText(New String(ChrW(&H36), 1), New Font("Webdings", 10))
			arrow.Style.Padding.Top = "-1.5mm"
			arrow.X = "parent.Width - 4mm"
			overflowArea.Children.Add(arrow)
			overflowArea.ZOrder = 100
			overflowArea.Height = "2.5mm"
			overflowArea.FragmentResolvedScript =
			 "if (C1PrintDocument.FormatVersion.AssemblyVersion.MinorRevision <> 100) Then" & vbCrLf & _
			 "  Dim fragment As RenderFragment = RenderFragment.Parent.Children(2)" & vbCrLf & _
			 "  If (fragment.HasChildren) Then" & vbCrLf & _
			 "    If (RenderFragment.Parent.BoundsOnPage.Contains(fragment.Children(fragment.Children.Count - 1).BoundsOnPage)) Then" & vbCrLf & _
			 "      RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & _
			 "    else" & vbCrLf & _
			 "      RenderFragment.RenderObject.Visibility = VisibilityEnum.Visible" & vbCrLf & _
			 "    end if" & vbCrLf & _
			 "  else" & vbCrLf & _
			 "    RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & _
			 "  end if" & vbCrLf & _
			 "else" & vbCrLf & _
			 "  RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & _
			 "end if"
			raDay.Children.Add(overflowArea)

			raWeekTab.Children.Add(raDay)
			raMonth.Children.Add(raWeekTab)

			raPage.Children.Add(raMonth)
'			#End Region

'			#Region "** tasks"
			' tasks
			Dim include As New RenderArea()
			include.FormatDataBindingInstanceScript = "If Document.Tags!IncludeTasks.Value Or Document.Tags!IncludeBlankNotes.Value Or Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Visible " & vbCrLf & "Else" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If"
			include.Width = "20%"
			include.Height = "parent.Height - 28mm"

			Dim raTasks As New RenderArea()
			raTasks.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeTasks.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raTasks.Style.Borders.All = LineDef.Default
			raTasks.Style.Spacing.Top = "0.5mm"
			raTasks.Style.Spacing.Left = "0.5mm"
			raTasks.Width = "100%"

			rt = New RenderText()
			rt.Text = "Tasks"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raTasks.Children.Add(rt)
			include.Children.Add(raTasks)

			Dim raNotes As New RenderArea()
			raNotes.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeBlankNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raNotes.Style.Borders.All = LineDef.Default
			raNotes.Style.Spacing.Top = "0.5mm"
			raNotes.Style.Spacing.Left = "0.5mm"
			raNotes.Width = "100%"

			rt = New RenderText()
			rt.Text = "Notes"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raNotes.Children.Add(rt)
			include.Children.Add(raNotes)

			raNotes = New RenderArea()
			raNotes.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raNotes.Style.Borders.All = LineDef.Default
			raNotes.Style.Spacing.Top = "0.5mm"
			raNotes.Style.Spacing.Left = "0.5mm"
			raNotes.Width = "100%"

			rt = New RenderText()
			rt.Text = "Notes"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raNotes.Children.Add(rt)

			Dim lines As New RenderTable()
			lines.Rows.Insert(0, 1)
			lines.Rows(0).Height = "0.5cm"
			Dim gr As TableVectorGroup = lines.RowGroups(0, 1)
			gr.Header = TableHeaderEnum.None
			Dim lst As New List(Of Integer)(60)
			For i As Integer = 0 To 59
				lst.Add(i)
			Next i
			gr.DataBinding.DataSource = lst
			lines.Style.GridLines.Horz = LineDef.Default
			lines.RowSizingMode = TableSizingModeEnum.Fixed

			raNotes.Children.Add(lines)

			include.Children.Add(raNotes)

			raPage.Children.Add(include)
'			#End Region

			doc.Body.Children.Add(raPage)

			AddCommonTags(doc)

			Dim newTag As New Tag("Appointments", Nothing, GetType(IList(Of Appointment)))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
			newTag = New Tag("WeekTabs", Nothing, GetType(List(Of Date)))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
			newTag = New Tag("WeekNumber", Nothing, GetType(List(Of Date)))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
			newTag = New Tag("MonthCalendars", Nothing, GetType(List(Of Date)))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
			newTag = New Tag("StartDate", Date.Today, GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			doc.Tags.Add(newTag)
			newTag = New Tag("EndDate", Date.Today.AddDays(1), GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			doc.Tags.Add(newTag)
			newTag = New Tag("HidePrivateAppointments", False, GetType(Boolean))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("TaskHeight", New Unit("33.3%"), GetType(Unit))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("DayWidth", New Unit("13.9%"), GetType(Unit))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("IncludeTasks", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Tasks"
			doc.Tags.Add(newTag)
			newTag = New Tag("IncludeBlankNotes", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Notes (blank)"
			doc.Tags.Add(newTag)
			newTag = New Tag("IncludeLinedNotes", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Notes (lined)"
			doc.Tags.Add(newTag)
			newTag = New Tag("WorkDaysOnly", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Only Print Workdays"
			doc.Tags.Add(newTag)
		End Sub

		''' <summary>
		''' Fills specified control with a Week style.
		''' </summary>
		''' <param name="doc"></param>
		Public Shared Sub MakeWeek(ByVal doc As C1PrintDocument)
			doc.Clear()

			doc.DocumentInfo.Title = "Weekly style"
			doc.DocumentInfo.Subject = "Week"
			AddNamespaces(doc)

			AddHeadersFooters(doc)
            doc.Tags("FooterRight").Value = "[GeneratedDateTime]"
			doc.Tags("DateHeadingsFont").Value = New Font("Segoe UI", 20, System.Drawing.FontStyle.Bold)

			doc.DocumentStartingScript &= "Dim tasksNumber As Integer = 0 " & vbCrLf & "If Document.Tags!IncludeTasks.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If Document.Tags!IncludeBlankNotes.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If tasksNumber = 1 Then" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""100%"")" & vbCrLf & "ElseIf tasksNumber = 2 Then" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""50%"")" & vbCrLf & "Else" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""33.3%"")" & vbCrLf & "End If" & vbCrLf & "Dim startT As DateTime = Tags!StartDate.Value  " & vbCrLf & "Dim endT As DateTime = Tags!EndDate.Value  " & vbCrLf & "While startT.DayOfWeek <> Tags!CalendarInfo.Value.WeekStart " & vbCrLf & "    startT = startT.AddDays(-1)" & vbCrLf & "End While" & vbCrLf & "Dim days As Long = DateDiff(DateInterval.Day, startT, endT )" & vbCrLf & "endT = endT.AddDays(7 - (days Mod 7))" & vbCrLf & "Dim weeks As New List(Of Date)" & vbCrLf & "Dim s As DateTime = startT" & vbCrLf & "While s < endT" & vbCrLf & "	weeks.Add(s)" & vbCrLf & "   s = s.AddDays(7)" & vbCrLf & "End While" & vbCrLf & "If Tags.IndexByName(""Weeks"") = -1 Then" & vbCrLf & "   Dim tagWeeks As Tag" & vbCrLf & "   tagWeeks = New Tag(""Weeks"", weeks)" & vbCrLf & "   tagWeeks.SerializeValue = False" & vbCrLf & "   Tags.Add(tagWeeks)" & vbCrLf & "Else" & vbCrLf & "   Tags!Weeks.Value = weeks" & vbCrLf & "End If" & vbCrLf & vbCrLf & "Dim dateAppointments As New DateAppointmentsCollection(startT, endT, Tags!Appointments.Value, Tags!CalendarInfo.Value, True, True)" & vbCrLf & "If Tags.IndexByName(""DateAppointments"") = -1 Then" & vbCrLf & "   Dim tagApps As Tag" & vbCrLf & "   tagApps = New Tag(""DateAppointments"", dateAppointments)" & vbCrLf & "   tagApps.SerializeValue = False" & vbCrLf & "   Tags.Add(tagApps)" & vbCrLf & "Else" & vbCrLf & "   Tags!DateAppointments.Value = dateAppointments" & vbCrLf & "End If"

			' RenderArea representing the single page
			Dim raPage As New RenderArea()
			raPage.DataBinding.DataSource = New Expression("Document.Tags!Weeks.Value")
			raPage.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			raPage.BreakBefore = BreakEnum.Page
			raPage.Width = "100%"
			raPage.Height = "100%"
			raPage.Stacking = StackingRulesEnum.InlineLeftToRight

'			#Region "** week header"
			' month header
			Dim raWeekHeader As New RenderArea()
			raWeekHeader.Style.Borders.All = LineDef.Default
			raWeekHeader.Width = "100%"
			raWeekHeader.Height = "28mm"
			raWeekHeader.Stacking = StackingRulesEnum.InlineLeftToRight

			Dim rt As New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Style.Font = Document.Tags!DateHeadingsFont.Value" & vbCrLf & "Dim rt As RenderText = RenderObject" & vbCrLf & "Dim startDate As Date = RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value" & vbCrLf & "Dim endDate As Date = startDate.AddDays(6)" & vbCrLf & "Dim txt As String = startDate.ToString(""MMMM d"", Document.Tags!CalendarInfo.Value.CultureInfo)" & vbCrLf & "If startDate.Year <> endDate.Year Then" & vbCrLf & "    txt = txt & "" "" & startDate.ToString(""yyy"", Document.Tags!CalendarInfo.Value.CultureInfo)" & vbCrLf & "End If" & vbCrLf & "txt = txt & "" - "" & endDate.ToString(""MMMM d yyy"", Document.Tags!CalendarInfo.Value.CultureInfo)" & vbCrLf & "rt.Text = txt" & vbCrLf & "Document.Tags!MonthCalendars.Value = New List(of Date)" & vbCrLf & "startDate = New Date(startDate.Year, startDate.Month, 1)" & vbCrLf & "While startDate <= endDate" & vbCrLf & "	Document.Tags!MonthCalendars.Value.Add(startDate)" & vbCrLf & "	startDate = startDate.AddMonths(1)" & vbCrLf & "End While"
			rt.Style.TextAlignVert = AlignVertEnum.Center
			rt.Style.Spacing.Left = "2mm"
			rt.Height = "100%"
			rt.Width = "parent.Width - 70mm"

			raWeekHeader.Children.Add(rt)

			Dim monthCalendar As New RenderArea()
			monthCalendar.DataBinding.DataSource = New Expression("Document.Tags!MonthCalendars.Value")
			monthCalendar.Stacking = StackingRulesEnum.InlineLeftToRight
			monthCalendar.Style.Spacing.Left = "1mm"
			monthCalendar.Style.Spacing.Right = "1mm"
			monthCalendar.Style.Spacing.Top = "0.5mm"
			monthCalendar.Width = "34mm"

			rt = New RenderText("[CDate(Fields!Date.Value).ToString(""MMMM yyy"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.FormatDataBindingInstanceScript = "Dim startDate As Date = RenderObject.Original.Parent.DataBinding.Fields!Date.Value" & vbCrLf & "Dim endDate As Date = startDate.AddMonths(1).AddDays(-1)" & vbCrLf & "While startDate.DayOfWeek <> Document.Tags!CalendarInfo.Value.WeekStart " & vbCrLf & "    startDate = startDate.AddDays(-1)" & vbCrLf & "End While" & vbCrLf & "Document.Tags!WeekNumber.Value = New List(of Date)" & vbCrLf & "While startDate <= endDate" & vbCrLf & "	Document.Tags!WeekNumber.Value.Add(startDate)" & vbCrLf & "	startDate = startDate.AddDays(7)" & vbCrLf & "End While"
			rt.Style.TextAlignHorz = AlignHorzEnum.Center
			rt.Style.Font = New Font("Segoe UI", 8)
			rt.Width = "100%"
			monthCalendar.Children.Add(rt)

			rt = New RenderText(" ")
			rt.Style.Font = New Font("Arial", 7f)
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			monthCalendar.Children.Add(rt)

			rt = New RenderText("[Document.Tags!CalendarInfo.Value.CultureInfo.DateTimeFormat.GetShortestDayName(CDate(Fields!Date.Value).DayOfWeek)]")
			rt.DataBinding.DataSource = New Expression("New DateAppointmentsCollection(Document.Tags!WeekNumber.Value(0), Document.Tags!WeekNumber.Value(0).AddDays(6), Document.Tags!Appointments.Value, True)")
			rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			rt.Style.Borders.Bottom = LineDef.Default
			rt.Style.Font = New Font("Arial", 7f)
			rt.Style.TextAlignHorz = AlignHorzEnum.Center
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			monthCalendar.Children.Add(rt)

			Dim raWeek As New RenderArea()
			raWeek.DataBinding.DataSource = New Expression("Document.Tags!WeekNumber.Value")
			raWeek.Style.Font = New Font("Arial", 7f)
			raWeek.Width = "100%"
			raWeek.Stacking = StackingRulesEnum.InlineLeftToRight

			rt = New RenderText("[Document.Tags!CalendarInfo.Value.CultureInfo.Calendar.GetWeekOfYear(CDate(Fields!Date.Value), System.Globalization.CalendarWeekRule.FirstDay, Document.Tags!CalendarInfo.Value.WeekStart)]")
			rt.Style.Borders.Right = LineDef.Default
			rt.Style.TextAlignHorz = AlignHorzEnum.Right
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			raWeek.Children.Add(rt)

			rt = New RenderText("[CDate(Fields!Date.Value).ToString(""%d"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.DataBinding.DataSource = New Expression("New DateAppointmentsCollection(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value).AddDays(6), Document.Tags!Appointments.Value, True)")
			rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			rt.FormatDataBindingInstanceScript = "If RenderObject.Original.DataBinding.Fields!Date.Value.Month <> RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value.Month Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & "Else If RenderObject.Original.DataBinding.Fields!HasAppointments.Value Then" & vbCrLf & "    RenderObject.Style.FontBold = true" & vbCrLf & "End If"
			rt.Style.TextAlignHorz = AlignHorzEnum.Right
			rt.Width = "12.5%"
			raWeek.Children.Add(rt)
			monthCalendar.Children.Add(raWeek)

			raWeekHeader.Children.Add(monthCalendar)

			raPage.Children.Add(raWeekHeader)
'			#End Region

'			#Region "** week"
			' month 
			Dim raWeekBody As New RenderArea()
			raWeekBody.FormatDataBindingInstanceScript = "If Not (Document.Tags!IncludeTasks.Value Or Document.Tags!IncludeBlankNotes.Value Or Document.Tags!IncludeLinedNotes.Value) Then" & vbCrLf & "    RenderObject.Width = ""100%"" " & vbCrLf & "End If"
			raWeekBody.Style.Spacing.Top = "0.5mm"
			raWeekBody.Style.Borders.All = LineDef.Default
			raWeekBody.Width = "75%"
			raWeekBody.Height = "parent.Height - 28mm"
			raWeekBody.Stacking = StackingRulesEnum.InlineLeftToRight

			' RenderArea representing the single day
			Dim raDay As New RenderArea()
			raDay.DataBinding.DataSource = New Expression("Document.Tags!DateAppointments.Value(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value).AddDays(6))")
			raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			raDay.Style.Borders.Right = LineDef.Default
			raDay.Style.Borders.Bottom = LineDef.Default
			raDay.Stacking = StackingRulesEnum.InlineLeftToRight
			raDay.Width = "50%"
			raDay.Height = "25%"

			rt = New RenderText("[CDate(Fields!Date.Value).ToString(""d  dddd"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.Style.Borders.Bottom = LineDef.Default
			rt.Style.Padding.Left = "1mm"
			rt.Style.FontBold = True
			rt.Style.BackColor = Colors.White
			rt.Style.TextAlignHorz = AlignHorzEnum.Left
			rt.Style.WordWrap = False
			raDay.Children.Add(rt)

			Dim status As New RenderText(" ")
			status.FormatDataBindingInstanceScript = "If IsNothing(RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value) Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse" & vbCrLf & "Else " & vbCrLf & "    RenderObject.Style.Brush = RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value.Brush.Brush " & vbCrLf & "End If"
			status.Width = "100%"
			status.Height = "1.5mm"
			raDay.Children.Add(status)

			Dim appointments As RenderArea = New RenderArea()
			appointments.Height = "parent.Height - Y - 2.5mm"

			' RenderArea representing the single appointment
			Dim raApp As New RenderArea()
			raApp.Style.Spacing.All = "0.5mm"
			raApp.FormatDataBindingInstanceScript = "If RenderObject.Original.DataBinding.Fields!AllDayEvent.Value Then" & vbCrLf & "    RenderObject.Style.Borders.All = LineDef.Default" & vbCrLf & "End If" & vbCrLf & "If Not IsNothing(RenderObject.Original.DataBinding.Fields!Label.Value) Then" & vbCrLf & "    RenderObject.Style.Brush = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label).Brush.Brush" & vbCrLf & "End If"
			raApp.Stacking = StackingRulesEnum.InlineLeftToRight
			' Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
			raApp.DataBinding.DataSource = New Expression("Parent.Fields!Appointments.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value")

			rt = New RenderText()
			rt.Text = "[IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, ""> "", String.Empty) & " & _
			 "IIf(Fields!AllDayEvent.Value, """", String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:t} {1:t}"", " & _
			 "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, DataBinding.Parent.Fields!Date.Value, Fields!Start.Value), " & _
			 "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value.AddDays(1)), CDate(Fields!End.Value)) > 0, DataBinding.Parent.Fields!Date.Value.AddDays(1), Fields!End.Value)))] [Fields!Subject.Value]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Original.DataBinding.Parent.Fields!Location.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "["" ("" & Fields!Location.Value & "")""]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			appointments.Children.Add(raApp)
			raDay.Children.Add(appointments)

			Dim overflowArea As RenderArea = New RenderArea()
			overflowArea.Name = "overflow"
			Dim arrow As RenderText = New RenderText(New String(ChrW(&H36), 1), New Font("Webdings", 10))
			arrow.Style.Padding.Top = "-1.5mm"
			arrow.X = "parent.Width - 4mm"
			overflowArea.Children.Add(arrow)
			overflowArea.ZOrder = 100
			overflowArea.Height = "2.5mm"
			overflowArea.FragmentResolvedScript =
			 "If (C1PrintDocument.FormatVersion.AssemblyVersion.MinorRevision <> 100) Then" & vbCrLf & _
			 "  Dim fragment As RenderFragment = RenderFragment.Parent.Children(2)" & vbCrLf & _
			 "  If (fragment.HasChildren) Then" & vbCrLf & _
			 "    If (RenderFragment.Parent.BoundsOnPage.Contains(fragment.Children(fragment.Children.Count - 1).BoundsOnPage)) Then" & vbCrLf & _
			 "      RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & _
			 "    else" & vbCrLf & _
			 "      RenderFragment.RenderObject.Visibility = VisibilityEnum.Visible" & vbCrLf & _
			 "    end if" & vbCrLf & _
			 "  else" & vbCrLf & _
			 "    RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & _
			 "  end if" & vbCrLf & _
			 "else" & vbCrLf & _
			 "  RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & _
			 "end if"
			raDay.Children.Add(overflowArea)

			raWeekBody.Children.Add(raDay)

			raPage.Children.Add(raWeekBody)
'			#End Region

'			#Region "** tasks"
			' tasks
			Dim include As New RenderArea()
			include.FormatDataBindingInstanceScript = "If Document.Tags!IncludeTasks.Value Or Document.Tags!IncludeBlankNotes.Value Or Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Visible " & vbCrLf & "Else" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If"
			include.Width = "25%"
			include.Height = "parent.Height - 28mm"

			Dim raTasks As New RenderArea()
			raTasks.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeTasks.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raTasks.Style.Borders.All = LineDef.Default
			raTasks.Style.Spacing.Top = "0.5mm"
			raTasks.Style.Spacing.Left = "0.5mm"
			raTasks.Width = "100%"

			rt = New RenderText()
			rt.Text = "Tasks"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raTasks.Children.Add(rt)
			include.Children.Add(raTasks)

			Dim raNotes As New RenderArea()
			raNotes.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeBlankNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raNotes.Style.Borders.All = LineDef.Default
			raNotes.Style.Spacing.Top = "0.5mm"
			raNotes.Style.Spacing.Left = "0.5mm"
			raNotes.Width = "100%"

			rt = New RenderText()
			rt.Text = "Notes"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raNotes.Children.Add(rt)
			include.Children.Add(raNotes)

			raNotes = New RenderArea()
			raNotes.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raNotes.Style.Borders.All = LineDef.Default
			raNotes.Style.Spacing.Top = "0.5mm"
			raNotes.Style.Spacing.Left = "0.5mm"
			raNotes.Width = "100%"

			rt = New RenderText()
			rt.Text = "Notes"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raNotes.Children.Add(rt)

			Dim lines As New RenderTable()
			lines.Rows.Insert(0, 1)
			lines.Rows(0).Height = "0.5cm"
			Dim gr As TableVectorGroup = lines.RowGroups(0, 1)
			gr.Header = TableHeaderEnum.None
			Dim lst As New List(Of Integer)(60)
			For i As Integer = 0 To 59
				lst.Add(i)
			Next i
			gr.DataBinding.DataSource = lst
			lines.Style.GridLines.Horz = LineDef.Default
			lines.RowSizingMode = TableSizingModeEnum.Fixed

			raNotes.Children.Add(lines)

			include.Children.Add(raNotes)

			raPage.Children.Add(include)
'			#End Region

			doc.Body.Children.Add(raPage)

			AddCommonTags(doc)

			Dim newTag As New Tag("Appointments", Nothing, GetType(IList(Of Appointment)))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
			newTag = New Tag("WeekNumber", Nothing, GetType(List(Of Date)))
			newTag.SerializeValue = False
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("MonthCalendars", Nothing, GetType(List(Of Date)))
			newTag.SerializeValue = False
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("StartDate", Date.Today, GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			doc.Tags.Add(newTag)
			newTag = New Tag("EndDate", Date.Today.AddDays(1), GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			doc.Tags.Add(newTag)
			newTag = New Tag("HidePrivateAppointments", False, GetType(Boolean))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("TaskHeight", New Unit("33.3%"), GetType(Unit))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("IncludeTasks", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Tasks"
			doc.Tags.Add(newTag)
			newTag = New Tag("IncludeBlankNotes", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Notes (blank)"
			doc.Tags.Add(newTag)
			newTag = New Tag("IncludeLinedNotes", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Notes (lined)"
			doc.Tags.Add(newTag)
		End Sub

		''' <summary>
		''' Fills specified control with a Day style.
		''' </summary>
		''' <param name="doc"></param>
		Public Shared Sub MakeDay(ByVal doc As C1PrintDocument)
			doc.Clear()

			doc.DocumentInfo.Title = "Daily style"
			doc.DocumentInfo.Subject = "Day"
			AddNamespaces(doc)

			AddHeadersFooters(doc)
            doc.Tags("FooterRight").Value = "[GeneratedDateTime]"
            doc.Tags("DateHeadingsFont").Value = New Font("Segoe UI", 24, System.Drawing.FontStyle.Bold)

			doc.DocumentStartingScript &= "Dim tasksNumber As Integer = 0 " & vbCrLf & "If Document.Tags!IncludeTasks.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If Document.Tags!IncludeBlankNotes.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "   tasksNumber = tasksNumber + 1 " & vbCrLf & "End If" & vbCrLf & "If tasksNumber = 1 Then" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""100%"")" & vbCrLf & "ElseIf tasksNumber = 2 Then" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""50%"")" & vbCrLf & "Else" & vbCrLf & "   Document.Tags!TaskHeight.Value = New Unit(""33.3%"")" & vbCrLf & "End If" & vbCrLf & "Dim dateAppointments As New DateAppointmentsCollection(Tags!StartDate.Value, Tags!EndDate.Value, Tags!Appointments.Value, Tags!CalendarInfo.Value, True, True)" & vbCrLf & "If Tags.IndexByName(""DateAppointments"") = -1 Then" & vbCrLf & "   Dim tagApps As Tag" & vbCrLf & "   tagApps = New Tag(""DateAppointments"", dateAppointments)" & vbCrLf & "   tagApps.SerializeValue = False" & vbCrLf & "   Tags.Add(tagApps)" & vbCrLf & "Else" & vbCrLf & "   Tags!DateAppointments.Value = dateAppointments" & vbCrLf & "End If" & vbCrLf & "Dim startT As Date = Tags!StartTime.Value  " & vbCrLf & "Dim endT As Date = Tags!EndTime.Value  " & vbCrLf & "If startT > endT Then" & vbCrLf & "   Tags!StartTime.Value = endT " & vbCrLf & "   Tags!EndTime.Value = startT " & vbCrLf & "End If"

			' RenderArea representing the single page
			Dim raPage As New RenderArea()
			raPage.DataBinding.DataSource = New Expression("Document.Tags!DateAppointments.Value")
			raPage.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			raPage.BreakBefore = BreakEnum.Page
			raPage.Width = "100%"
			raPage.Height = "100%"
			raPage.Stacking = StackingRulesEnum.InlineLeftToRight

'			#Region "** day header"
			' day header
			Dim raDayHeader As New RenderArea()
			raDayHeader.Style.Borders.All = LineDef.Default
			raDayHeader.Width = "100%"
			raDayHeader.Height = "28mm"
			raDayHeader.Stacking = StackingRulesEnum.InlineLeftToRight

			Dim rt As New RenderText("[CDate(Fields!Date.Value).ToString(""D"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.FormatDataBindingInstanceScript = "RenderObject.Style.Font = Document.Tags!DateHeadingsFont.Value" & vbCrLf & "Dim startDate As Date = RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value" & vbCrLf & "Document.Tags!StartTime.Value = startDate.Add(Document.Tags!StartTime.Value.TimeOfDay)" & vbCrLf & "Document.Tags!EndTime.Value = startDate.Add(Document.Tags!EndTime.Value.TimeOfDay)" & vbCrLf & "Document.Tags!MonthCalendar.Value = New Date(startDate.Year, startDate.Month, 1) " & vbCrLf & "startDate = Document.Tags!StartTime.Value" & vbCrLf & "Document.Tags!DayHours.Value = New Dictionary(of Date, Date)" & vbCrLf & "While startDate < Document.Tags!EndTime.Value" & vbCrLf & "	Document.Tags!DayHours.Value.Add(startDate, startDate)" & vbCrLf & "	startDate = startDate.AddMinutes(30)" & vbCrLf & "End While"
			rt.Style.TextAlignVert = AlignVertEnum.Center
			rt.Style.Spacing.Left = "2mm"
			rt.Height = "100%"
			rt.Width = "parent.Width - 38mm"

			raDayHeader.Children.Add(rt)

			Dim monthCalendar As New RenderArea()
			monthCalendar.Stacking = StackingRulesEnum.InlineLeftToRight
			monthCalendar.Style.Spacing.Left = "1mm"
			monthCalendar.Style.Spacing.Right = "3mm"
			monthCalendar.Style.Spacing.Top = "0.5mm"
			monthCalendar.Width = "36mm"

			rt = New RenderText("[CDate(Document.Tags!MonthCalendar.Value).ToString(""MMMM yyy"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.FormatDataBindingInstanceScript = "Dim startDate As Date = Document.Tags!MonthCalendar.Value" & vbCrLf & "Dim endDate As Date = startDate.AddMonths(1).AddDays(-1)" & vbCrLf & "While startDate.DayOfWeek <> Document.Tags!CalendarInfo.Value.WeekStart " & vbCrLf & "    startDate = startDate.AddDays(-1)" & vbCrLf & "End While" & vbCrLf & "Document.Tags!WeekNumber.Value = New List(of Date)" & vbCrLf & "While startDate <= endDate" & vbCrLf & "	Document.Tags!WeekNumber.Value.Add(startDate)" & vbCrLf & "	startDate = startDate.AddDays(7)" & vbCrLf & "End While"
			rt.Style.TextAlignHorz = AlignHorzEnum.Center
			rt.Style.Font = New Font("Segoe UI", 8)
			rt.Width = "100%"
			monthCalendar.Children.Add(rt)

			rt = New RenderText(" ")
			rt.Style.Font = New Font("Arial", 7f)
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			monthCalendar.Children.Add(rt)

			rt = New RenderText("[Document.Tags!CalendarInfo.Value.CultureInfo.DateTimeFormat.GetShortestDayName(CDate(Fields!Date.Value).DayOfWeek)]")
			rt.DataBinding.DataSource = New Expression("New DateAppointmentsCollection(Document.Tags!WeekNumber.Value(0), Document.Tags!WeekNumber.Value(0).AddDays(6), Document.Tags!Appointments.Value, True)")
			rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			rt.Style.Borders.Bottom = LineDef.Default
			rt.Style.Font = New Font("Arial", 7f)
			rt.Style.TextAlignHorz = AlignHorzEnum.Center
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			monthCalendar.Children.Add(rt)

			Dim raWeek As New RenderArea()
			raWeek.DataBinding.DataSource = New Expression("Document.Tags!WeekNumber.Value")
			raWeek.Style.Font = New Font("Arial", 7f)
			raWeek.Width = "100%"
			raWeek.Stacking = StackingRulesEnum.InlineLeftToRight

			rt = New RenderText("[Document.Tags!CalendarInfo.Value.CultureInfo.Calendar.GetWeekOfYear(CDate(Fields!Date.Value), System.Globalization.CalendarWeekRule.FirstDay, Document.Tags!CalendarInfo.Value.WeekStart)]")
			rt.Style.Borders.Right = LineDef.Default
			rt.Style.TextAlignHorz = AlignHorzEnum.Right
			rt.Style.WordWrap = False
			rt.Width = "12.5%"
			raWeek.Children.Add(rt)

			rt = New RenderText("[CDate(Fields!Date.Value).ToString(""%d"", Document.Tags!CalendarInfo.Value.CultureInfo)]")
			rt.DataBinding.DataSource = New Expression("New DateAppointmentsCollection(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value).AddDays(6), Document.Tags!Appointments.Value, True)")
			rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			rt.FormatDataBindingInstanceScript = "If RenderObject.Original.DataBinding.Fields!Date.Value.Month <> Document.Tags!MonthCalendar.Value.Month Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Hidden" & vbCrLf & "Else If RenderObject.Original.DataBinding.Fields!HasAppointments.Value Then" & vbCrLf & "    RenderObject.Style.FontBold = true" & vbCrLf & "End If"
			rt.Style.TextAlignHorz = AlignHorzEnum.Right
			rt.Width = "12.5%"
			raWeek.Children.Add(rt)
			monthCalendar.Children.Add(raWeek)

			raDayHeader.Children.Add(monthCalendar)

			raPage.Children.Add(raDayHeader)
'			#End Region

'			#Region "** day"
			' day 
			Dim raDayBody As New RenderArea()
			raDayBody.FormatDataBindingInstanceScript = "If Not (Document.Tags!IncludeTasks.Value Or Document.Tags!IncludeBlankNotes.Value Or Document.Tags!IncludeLinedNotes.Value) Then" & vbCrLf & "    RenderObject.Width = ""100%"" " & vbCrLf & "End If"
			raDayBody.Style.Spacing.Top = "0.5mm"
			raDayBody.Style.Borders.All = LineDef.Default
			raDayBody.Width = "75%"
			raDayBody.Height = "parent.Height - 28mm"
			raDayBody.Stacking = StackingRulesEnum.BlockTopToBottom

'			#Region "** all-day events"
			' RenderArea representing the single day
			Dim raDay As New RenderArea()
			raDay.DataBinding.DataSource = New Expression("Document.Tags!DateAppointments.Value(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value))")
			raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			raDay.Stacking = StackingRulesEnum.InlineLeftToRight
			raDay.Height = "Auto"

			Dim status As New RenderText(" ")
			status.FormatDataBindingInstanceScript = "If IsNothing(RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value) Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse" & vbCrLf & "Else " & vbCrLf & "    RenderObject.Style.Brush = RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value.Brush.Brush " & vbCrLf & "End If"
			status.Width = "100%"
			status.Height = "1.5mm"
			raDay.Children.Add(status)

			' RenderArea representing the single appointment
			Dim raApp As New RenderArea()
			raApp.Style.Spacing.All = "0.5mm"
			raApp.Style.Spacing.Left = "1.25cm"
			raApp.FormatDataBindingInstanceScript = "If RenderObject.Original.DataBinding.Fields!AllDayEvent.Value Then" & vbCrLf & "    RenderObject.Style.Borders.All = LineDef.Default" & vbCrLf & "ElseIf DateDiff(DateInterval.Second, CDate(Document.Tags!StartTime.Value), CDate(RenderObject.Original.DataBinding.Fields!Start.Value)) >= 0 Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse" & vbCrLf & "End If" & vbCrLf & "If Not IsNothing(RenderObject.Original.DataBinding.Fields!Label.Value) Then" & vbCrLf & "    RenderObject.Style.Brush = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label).Brush.Brush" & vbCrLf & "End If"
			raApp.Stacking = StackingRulesEnum.InlineLeftToRight
			' Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
			raApp.DataBinding.DataSource = New Expression("Parent.Fields!Appointments.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value")

			rt = New RenderText()
			rt.Text = "[IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, ""> "", String.Empty) & " & "IIf(Fields!AllDayEvent.Value, """", String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:t} {1:t}"", " & "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, DataBinding.Parent.Fields!Date.Value, Fields!Start.Value), " & "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value.AddDays(1)), CDate(Fields!End.Value)) > 0, DataBinding.Parent.Fields!Date.Value.AddDays(1), Fields!End.Value)))]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			rt = New RenderText()
			rt.Text = " [Fields!Subject.Value] "
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Original.DataBinding.Parent.Fields!Location.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "["" ("" & Fields!Location.Value & "")""]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			raDay.Children.Add(raApp)

			raDayBody.Children.Add(raDay)
'			#End Region

'			#Region "** slots"
			raDay = New RenderArea()
			raDay.DataBinding.DataSource = New Expression("Document.Tags!DateAppointments.Value(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value))")
			raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			raDay.Stacking = StackingRulesEnum.InlineLeftToRight
			raDay.Height = "parent - prev.height - next.height"

			Dim raTimeSlot As New RenderArea()
			raTimeSlot.FormatDataBindingInstanceScript = "RenderObject.Height = New Unit((100/CLng(Document.Tags!DayHours.Value.Count) - 0.005).ToString(""#.###"", System.Globalization.CultureInfo.InvariantCulture) & ""%"")"
			raTimeSlot.DataBinding.DataSource = New Expression("Document.Tags!DayHours.Value")
			raTimeSlot.Width = "100%"
			raTimeSlot.Height = "0.5cm"
			raTimeSlot.Stacking = StackingRulesEnum.InlineLeftToRight

			Dim rp As New RenderParagraph()
			rp.FormatDataBindingInstanceScript = "Document.Tags!SlotAppointments.Value = Document.Tags!DateAppointments.Value.GetIntervalAppointments(RenderObject.Original.DataBinding.Parent.Fields!Key.Value, RenderObject.Original.DataBinding.PArent.Fields!Key.Value.AddMinutes(30), False)" & vbCrLf & "RenderObject.Visibility = IIf(RenderObject.Original.DataBinding.Parent.Fields!Key.Value.Minute = 0, VisibilityEnum.Visible, VisibilityEnum.Hidden)" & vbCrLf & "Dim headerFont As Font = Document.Tags!DateHeadingsFont.Value" & vbCrLf & "RenderObject.Style.Font = New Font(headerFont.FontFamily, headerFont.Size * 2 / 3)"
			rp.Width = "1.65cm"
			rp.Style.TextAlignHorz = AlignHorzEnum.Right
			rp.Style.Borders.Top = LineDef.Default
			rp.Style.Padding.Right = "1mm"
			rp.Height = "100%"
			Dim pt As New ParagraphText("[IIf(String.IsNullOrEmpty(Document.Tags!CalendarInfo.Value.CultureInfo.DateTimeFormat.AMDesignator), CDate(Fields!Key.Value).ToString(""%H""), CDate(Fields!Key.Value).ToString(""%h""))]")
			pt.Style.FontBold = True
			pt.Style.Padding.Right = "1mm"
			rp.Content.Add(pt)
			rp.Content.Add(New ParagraphText("[IIf(String.IsNullOrEmpty(Document.Tags!CalendarInfo.Value.CultureInfo.DateTimeFormat.AMDesignator), ""00"", CDate(Fields!Key.Value).ToString(""tt"", Document.Tags!CalendarInfo.Value.CultureInfo).ToLower())]", TextPositionEnum.Superscript))
			raTimeSlot.Children.Add(rp)

			Dim slot As New RenderArea()
			slot.Width = "parent.width - prev.width - prev.left"
			slot.Height = "100%"
			slot.Style.Borders.Top = LineDef.Default
			slot.Style.Borders.Left = LineDef.Default
			slot.Stacking = StackingRulesEnum.InlineLeftToRight

			' RenderArea representing the single appointment
			raApp = New RenderArea()
			' Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
			raApp.DataBinding.DataSource = New Expression("Document.Tags!SlotAppointments.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value")
			raApp.Style.Spacing.All = "0.5mm"
			raApp.FormatDataBindingInstanceScript = "RenderObject.Width = New Unit((100/CLng(Document.Tags!SlotAppointments.Value.Count) - 0.005).ToString(""#.###"", System.Globalization.CultureInfo.InvariantCulture) & ""%"")" & vbCrLf & "If Not IsNothing(RenderObject.Original.DataBinding.Fields!Label.Value) Then" & vbCrLf & "    RenderObject.Style.Brush = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label).Brush.Brush" & vbCrLf & "End If"
			raApp.Width = "10%"
			raApp.Height = "100%"
			raApp.Stacking = StackingRulesEnum.InlineLeftToRight

			rt = New RenderText()
			rt.Text = "[String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:t}-{1:t}"", Fields!Start.Value, Fields!End.Value).ToLower()]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			rt = New RenderText()
			rt.Text = " [Fields!Subject.Value] "
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Original.DataBinding.Parent.Fields!Location.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "["" ("" & Fields!Location.Value & "")""]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			slot.Children.Add(raApp)

			raTimeSlot.Children.Add(slot)

			raDay.Children.Add(raTimeSlot)
			raDayBody.Children.Add(raDay)
'			#End Region

'			#Region "** late appointments"
			raDay = New RenderArea()
			raDay.DataBinding.DataSource = New Expression("Document.Tags!DateAppointments.Value(Parent.Fields!Date.Value, CDate(Parent.Fields!Date.Value))")
			raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value")
			raDay.Style.Borders.Top = LineDef.Default
			raDay.Stacking = StackingRulesEnum.InlineLeftToRight
			raDay.Height = "Auto"

			' RenderArea representing the single appointment
			raApp = New RenderArea()
			raApp.Style.Spacing.All = "0.5mm"
			raApp.Style.Spacing.Left = "1.25cm"
			raApp.FormatDataBindingInstanceScript = "If DateDiff(DateInterval.Second, CDate(Document.Tags!EndTime.Value), CDate(RenderObject.Original.DataBinding.Fields!Start.Value)) >= 0 Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Visible" & vbCrLf & "Else" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse" & vbCrLf & "End If" & vbCrLf & "If Not IsNothing(RenderObject.Original.DataBinding.Fields!Label.Value) Then" & vbCrLf & "    RenderObject.Style.Brush = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label).Brush.Brush" & vbCrLf & "End If"
			raApp.Stacking = StackingRulesEnum.InlineLeftToRight
			' Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
			raApp.DataBinding.DataSource = New Expression("Parent.Fields!Appointments.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value")
			raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value")

			rt = New RenderText()
			rt.Text = "[IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, ""> "", String.Empty) & " & "IIf(Fields!AllDayEvent.Value, """", String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, ""{0:t} {1:t}"", " & "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value), CDate(Fields!Start.Value)) < 0, DataBinding.Parent.Fields!Date.Value, Fields!Start.Value), " & "IIf(DateDiff(DateInterval.Second, CDate(DataBinding.Parent.Fields!Date.Value.AddDays(1)), CDate(Fields!End.Value)) > 0, DataBinding.Parent.Fields!Date.Value.AddDays(1), Fields!End.Value)))]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			rt = New RenderText()
			rt.Text = " [Fields!Subject.Value] "
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			rt = New RenderText()
			rt.FormatDataBindingInstanceScript = "RenderObject.Visibility = IIf(Not String.IsNullOrEmpty(RenderObject.Original.DataBinding.Parent.Fields!Location.Value), VisibilityEnum.Visible, VisibilityEnum.Collapse)"
			rt.Text = "["" ("" & Fields!Location.Value & "")""]"
			rt.Width = "Auto"
			raApp.Children.Add(rt)

			raDay.Children.Add(raApp)

			raDayBody.Children.Add(raDay)
'			#End Region

			raPage.Children.Add(raDayBody)
'			#End Region

'			#Region "** tasks"
			' tasks
			Dim include As New RenderArea()
			include.FormatDataBindingInstanceScript = "If Document.Tags!IncludeTasks.Value Or Document.Tags!IncludeBlankNotes.Value Or Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Visible " & vbCrLf & "Else" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If"
			include.Width = "25%"
			include.Height = "parent.Height - 28mm"

			Dim raTasks As New RenderArea()
			raTasks.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeTasks.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raTasks.Style.Borders.All = LineDef.Default
			raTasks.Style.Spacing.Top = "0.5mm"
			raTasks.Style.Spacing.Left = "0.5mm"
			raTasks.Width = "100%"

			rt = New RenderText()
			rt.Text = "Tasks"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raTasks.Children.Add(rt)
			include.Children.Add(raTasks)

			Dim raNotes As New RenderArea()
			raNotes.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeBlankNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raNotes.Style.Borders.All = LineDef.Default
			raNotes.Style.Spacing.Top = "0.5mm"
			raNotes.Style.Spacing.Left = "0.5mm"
			raNotes.Width = "100%"

			rt = New RenderText()
			rt.Text = "Notes"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raNotes.Children.Add(rt)
			include.Children.Add(raNotes)

			raNotes = New RenderArea()
			raNotes.FormatDataBindingInstanceScript = "If Not Document.Tags!IncludeLinedNotes.Value Then" & vbCrLf & "    RenderObject.Visibility = VisibilityEnum.Collapse " & vbCrLf & "End If" & vbCrLf & "RenderObject.Height = Document.Tags!TaskHeight.Value"
			raNotes.Style.Borders.All = LineDef.Default
			raNotes.Style.Spacing.Top = "0.5mm"
			raNotes.Style.Spacing.Left = "0.5mm"
			raNotes.Width = "100%"

			rt = New RenderText()
			rt.Text = "Notes"
			rt.Style.Padding.Left = "2mm"
			rt.Style.Borders.Bottom = LineDef.Default
			raNotes.Children.Add(rt)

			Dim lines As New RenderTable()
			lines.Rows.Insert(0, 1)
			lines.Rows(0).Height = "0.5cm"
			Dim gr As TableVectorGroup = lines.RowGroups(0, 1)
			gr.Header = TableHeaderEnum.None
			Dim lst As New List(Of Integer)(60)
			For i As Integer = 0 To 59
				lst.Add(i)
			Next i
			gr.DataBinding.DataSource = lst
			lines.Style.GridLines.Horz = LineDef.Default
			lines.RowSizingMode = TableSizingModeEnum.Fixed

			raNotes.Children.Add(lines)

			include.Children.Add(raNotes)

			raPage.Children.Add(include)
'			#End Region

			doc.Body.Children.Add(raPage)

			AddCommonTags(doc)

			Dim newTag As New Tag("Appointments", Nothing, GetType(IList(Of Appointment)))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
			newTag = New Tag("WeekNumber", Nothing, GetType(List(Of Date)))
			newTag.SerializeValue = False
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("DayHours", Nothing, GetType(Dictionary(Of Date, Date)))
			newTag.SerializeValue = False
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("MonthCalendar", Nothing, GetType(Date))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("StartDate", Date.Today, GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			doc.Tags.Add(newTag)
			newTag = New Tag("EndDate", Date.Today.AddDays(1), GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			doc.Tags.Add(newTag)

			newTag = New Tag("StartTime", Date.Today.AddHours(7), GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			CType(newTag.InputParams, TagDateTimeInputParams).Format = DateTimePickerFormat.Time
			doc.Tags.Add(newTag)
			newTag = New Tag("EndTime", Date.Today.AddHours(19), GetType(Date))
			newTag.InputParams = New TagDateTimeInputParams()
			CType(newTag.InputParams, TagDateTimeInputParams).Format = DateTimePickerFormat.Time
			doc.Tags.Add(newTag)

			newTag = New Tag("HidePrivateAppointments", False, GetType(Boolean))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("TaskHeight", New Unit("33.3%"), GetType(Unit))
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)
			newTag = New Tag("SlotAppointments", Nothing, GetType(List(Of Appointment)))
			newTag.SerializeValue = False
			newTag.ShowInDialog = False
			doc.Tags.Add(newTag)

			newTag = New Tag("IncludeTasks", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Tasks"
			doc.Tags.Add(newTag)
			newTag = New Tag("IncludeBlankNotes", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Notes (blank)"
			doc.Tags.Add(newTag)
			newTag = New Tag("IncludeLinedNotes", False, GetType(Boolean))
			newTag.ShowInDialog = True
			newTag.InputParams = New TagBoolInputParams()
			newTag.Description = "Include Notes (lined)"
			doc.Tags.Add(newTag)
		End Sub

		''' <summary>
		''' Adds header and footer tags (common for all documents)
		''' </summary>
		''' <param name="doc"></param>
		Private Shared Sub AddHeadersFooters(ByVal doc As C1PrintDocument)
			Dim theader As New RenderTable()
			theader.Cells(0, 0).Style.TextAlignHorz = AlignHorzEnum.Left
			theader.Cells(0, 1).Style.TextAlignHorz = AlignHorzEnum.Center
			theader.Cells(0, 2).Style.TextAlignHorz = AlignHorzEnum.Right
			theader.CellStyle.Padding.Bottom = "2mm"
			theader.Cells(0, 0).Text = "[Document.Tags!HeaderLeft.Value]"
			theader.Cells(0, 1).Text = "[Document.Tags!HeaderCenter.Value]"
			theader.Cells(0, 2).Text = "[Document.Tags!HeaderRight.Value]"

			doc.PageLayouts.Default.PageHeader = theader

			theader = New RenderTable()
			theader.Cells(0, 0).Style.TextAlignHorz = AlignHorzEnum.Left
			theader.Cells(0, 1).Style.TextAlignHorz = AlignHorzEnum.Center
			theader.Cells(0, 2).Style.TextAlignHorz = AlignHorzEnum.Right
			theader.CellStyle.Padding.Bottom = "2mm"
			theader.Cells(0, 0).Text = "[IIf(Document.Tags!ReverseOnEvenPages.Value, Document.Tags!HeaderRight.Value, Document.Tags!HeaderLeft.Value)]"
			theader.Cells(0, 1).Text = "[Document.Tags!HeaderCenter.Value]"
			theader.Cells(0, 2).Text = "[IIf(Document.Tags!ReverseOnEvenPages.Value, Document.Tags!HeaderLeft.Value, Document.Tags!HeaderRight.Value)]"

			doc.PageLayouts.EvenPages = New PageLayout()
			doc.PageLayouts.EvenPages.PageHeader = theader

			Dim tfooter As New RenderTable()
			tfooter.Cells(0, 0).Style.TextAlignHorz = AlignHorzEnum.Left
			tfooter.Cells(0, 1).Style.TextAlignHorz = AlignHorzEnum.Center
			tfooter.Cells(0, 2).Style.TextAlignHorz = AlignHorzEnum.Right
			tfooter.CellStyle.Padding.Top = "2mm"
			tfooter.Cells(0, 0).Text = "[Document.Tags!FooterLeft.Value]"
			tfooter.Cells(0, 1).Text = "[Document.Tags!FooterCenter.Value]"
			tfooter.Cells(0, 2).Text = "[Document.Tags!FooterRight.Value]"

			doc.PageLayouts.Default.PageFooter = tfooter

			tfooter = New RenderTable()
			tfooter.Cells(0, 0).Style.TextAlignHorz = AlignHorzEnum.Left
			tfooter.Cells(0, 1).Style.TextAlignHorz = AlignHorzEnum.Center
			tfooter.Cells(0, 2).Style.TextAlignHorz = AlignHorzEnum.Right
			tfooter.CellStyle.Padding.Top = "2mm"
			tfooter.Cells(0, 0).Text = "[IIf(Document.Tags!ReverseOnEvenPages.Value, Document.Tags!FooterRight.Value, Document.Tags!FooterLeft.Value)]"
			tfooter.Cells(0, 1).Text = "[Document.Tags!FooterCenter.Value]"
			tfooter.Cells(0, 2).Text = "[IIf(Document.Tags!ReverseOnEvenPages.Value, Document.Tags!FooterLeft.Value, Document.Tags!FooterRight.Value)]"

			doc.PageLayouts.EvenPages.PageFooter = tfooter

			' add page header/footer tags
			Dim newTag As New Tag("HeaderLeft", String.Empty, GetType(String))
			doc.Tags.Add(newTag)
			newTag = New Tag("HeaderCenter", String.Empty, GetType(String))
			doc.Tags.Add(newTag)
			newTag = New Tag("HeaderRight", String.Empty, GetType(String))
			doc.Tags.Add(newTag)
			newTag = New Tag("FooterLeft", String.Empty, GetType(String))
			doc.Tags.Add(newTag)
			newTag = New Tag("FooterCenter", "[PageNo]", GetType(String))
			doc.Tags.Add(newTag)
			newTag = New Tag("FooterRight", String.Empty, GetType(String))
			doc.Tags.Add(newTag)
			newTag = New Tag("ReverseOnEvenPages", False, GetType(Boolean))
			doc.Tags.Add(newTag)
            newTag = New Tag("HeaderColor", Colors.Black, GetType(System.Windows.Media.Color))
			doc.Tags.Add(newTag)
            newTag = New Tag("FooterColor", Colors.Black, GetType(System.Windows.Media.Color))
			doc.Tags.Add(newTag)
			newTag = New Tag("HeaderFont", New Font("Tahoma", 8), GetType(Font))
			doc.Tags.Add(newTag)
			newTag = New Tag("FooterFont", New Font("Tahoma", 8), GetType(Font))
			doc.Tags.Add(newTag)

            newTag = New Tag("DateHeadingsFont", New Font("Segoe UI", 12, System.Drawing.FontStyle.Bold), GetType(Font))
			doc.Tags.Add(newTag)
			newTag = New Tag("AppointmentsFont", New Font("Segoe UI", 8), GetType(Font))
			doc.Tags.Add(newTag)

			doc.DocumentStartingScript = "Document.Style.Font = Document.Tags!AppointmentsFont.Value" & vbCrLf & "Document.PageLayout.PageFooter.Style.TextColor = Document.Tags!FooterColor.Value" & vbCrLf & "Document.PageLayouts.EvenPages.PageFooter.Style.TextColor = Document.Tags!FooterColor.Value" & vbCrLf & "Document.PageLayout.PageHeader.Style.TextColor = Document.Tags!HeaderColor.Value" & vbCrLf & "Document.PageLayouts.EvenPages.PageHeader.Style.TextColor = Document.Tags!HeaderColor.Value" & vbCrLf & "Document.PageLayout.PageFooter.Style.Font = Document.Tags!FooterFont.Value" & vbCrLf & "Document.PageLayouts.EvenPages.PageFooter.Style.Font = Document.Tags!FooterFont.Value" & vbCrLf & "Document.PageLayout.PageHeader.Style.Font = Document.Tags!HeaderFont.Value" & vbCrLf & "Document.PageLayouts.EvenPages.PageHeader.Style.Font = Document.Tags!HeaderFont.Value" & vbCrLf
		End Sub

		''' <summary>
		''' Adds namespaces (common for all documents)
		''' </summary>
		''' <param name="doc"></param>
		Private Shared Sub AddNamespaces(ByVal doc As C1PrintDocument)
			doc.TagEscapeString = ChrW(&H01).ToString()
			doc.ScriptingOptions.Namespaces.Add("C1.C1Schedule")
			doc.ScriptingOptions.Namespaces.Add("C1.C1Schedule.Printing")
		End Sub

		''' <summary>
		''' Adds common tags
		''' </summary>
		''' <param name="doc"></param>
		Private Shared Sub AddCommonTags(ByVal doc As C1PrintDocument)
			' CalendarInfo object contains all calendar-cspecific information
			' (culture, working days, week start, etc..)
			Dim newTag As New Tag("CalendarInfo", Nothing, GetType(CalendarInfo))
			newTag.ShowInDialog = False
			newTag.SerializeValue = False
			doc.Tags.Add(newTag)
		End Sub
	End Class
End Namespace