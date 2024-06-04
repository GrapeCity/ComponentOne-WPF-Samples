using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using C1.C1Preview;
using C1.C1Preview.DataBinding;
using C1.Schedule;


namespace PrintDocTemplates
{
    /// <summary>
    /// Contains static methods filling document for different views
    /// </summary>
    public static class Utils
    {
        private static string GetStringCultureInfo()
        {
            return string.Format(@"CType({0}.CultureInfo, System.Globalization.CultureInfo)",
                    Utils.GetStringCalendarInfo());
        }

        private static string GetStringCalendarInfo()
        {
            return string.Format(@"CType( CType( Document.Tags, TagCollection)({0}).Value, C1.Schedule.CalendarInfo)",
                    string.Format("\"{0}\"", "CalendarInfo"));
        }

        /// <summary>
        /// Adds header and footer tags (common for all documents)
        /// </summary>
        /// <param name="doc"></param>
        private static void AddHeadersFooters(C1PrintDocument doc)
        {
            RenderTable theader = new RenderTable();
            theader.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Left;
            theader.Cells[0, 1].Style.TextAlignHorz = AlignHorzEnum.Center;
            theader.Cells[0, 2].Style.TextAlignHorz = AlignHorzEnum.Right;
            theader.CellStyle.Padding.Bottom = "2mm";
            theader.Cells[0, 0].Text = "[Document.Tags!HeaderLeft.Value]";
            theader.Cells[0, 1].Text = "[Document.Tags!HeaderCenter.Value]";
            theader.Cells[0, 2].Text = "[Document.Tags!HeaderRight.Value]";

            doc.PageLayouts.Default.PageHeader = theader;

            theader = new RenderTable();
            theader.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Left;
            theader.Cells[0, 1].Style.TextAlignHorz = AlignHorzEnum.Center;
            theader.Cells[0, 2].Style.TextAlignHorz = AlignHorzEnum.Right;
            theader.CellStyle.Padding.Bottom = "2mm";
            theader.Cells[0, 0].Text =
                    @"[If(Not Document.Tags!ReverseOnEvenPages.Value Is Nothing, Document.Tags!HeaderRight.Value , Document.Tags!HeaderLeft.Value)]";

            theader.Cells[0, 1].Text = "[Document.Tags!HeaderCenter.Value]";
            theader.Cells[0, 2].Text =
                    @"[If(Not Document.Tags!ReverseOnEvenPages.Value Is Nothing, Document.Tags!HeaderRight.Value , Document.Tags!HeaderLeft.Value)]";

            doc.PageLayouts.EvenPages = new PageLayout();
            doc.PageLayouts.EvenPages.PageHeader = theader;

            RenderTable tfooter = new RenderTable();
            tfooter.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Left;
            tfooter.Cells[0, 1].Style.TextAlignHorz = AlignHorzEnum.Center;
            tfooter.Cells[0, 2].Style.TextAlignHorz = AlignHorzEnum.Right;
            tfooter.CellStyle.Padding.Top = "2mm";
            tfooter.Cells[0, 0].Text = "[Document.Tags!FooterLeft.Value]";
            tfooter.Cells[0, 1].Text = "[Document.Tags!FooterCenter.Value]";
            tfooter.Cells[0, 2].Text = "[Document.Tags!FooterRight.Value]";

            doc.PageLayouts.Default.PageFooter = tfooter;

            tfooter = new RenderTable();
            tfooter.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Left;
            tfooter.Cells[0, 1].Style.TextAlignHorz = AlignHorzEnum.Center;
            tfooter.Cells[0, 2].Style.TextAlignHorz = AlignHorzEnum.Right;
            tfooter.CellStyle.Padding.Top = "2mm";
            tfooter.Cells[0, 0].Text = "[If(Not Document.Tags!ReverseOnEvenPages.Value Is Nothing, Document.Tags!FooterRight.Value , Document.Tags!FooterLeft.Value)]";
            tfooter.Cells[0, 1].Text = "[Document.Tags!FooterCenter.Value]";
            tfooter.Cells[0, 2].Text = "[If(Not Document.Tags!ReverseOnEvenPages.Value Is Nothing,  Document.Tags!FooterLeft.Value , Document.Tags!FooterRight.Value)]";

            doc.PageLayouts.EvenPages.PageFooter = tfooter;

            // add page header/footer tags
            Tag newTag = new Tag("HeaderLeft", String.Empty, typeof(string));
            doc.Tags.Add(newTag);
            newTag = new Tag("HeaderCenter", String.Empty, typeof(string));
            doc.Tags.Add(newTag);
            newTag = new Tag("HeaderRight", String.Empty, typeof(string));
            doc.Tags.Add(newTag);
            newTag = new Tag("FooterLeft", String.Empty, typeof(string));
            doc.Tags.Add(newTag);
            newTag = new Tag("FooterCenter", "[PageNo]", typeof(string));
            doc.Tags.Add(newTag);
            newTag = new Tag("FooterRight", String.Empty, typeof(string));
            doc.Tags.Add(newTag);
            newTag = new Tag("ReverseOnEvenPages", false, typeof(bool));
            doc.Tags.Add(newTag);
            newTag = new Tag("HeaderColor", Colors.Black, typeof(System.Windows.Media.Color));
            doc.Tags.Add(newTag);
            newTag = new Tag("FooterColor", Colors.Black, typeof(System.Windows.Media.Color));
            doc.Tags.Add(newTag);
            newTag = new Tag("HeaderFont", new Font("Tahoma", 8), typeof(Font));
            doc.Tags.Add(newTag);
            newTag = new Tag("FooterFont", new Font("Tahoma", 8), typeof(Font));
            doc.Tags.Add(newTag);

            newTag = new Tag("DateHeadingsFont", new Font("Segoe UI", 12, FontStyle.Bold), typeof(Font));
            doc.Tags.Add(newTag);
            newTag = new Tag("AppointmentsFont", new Font("Segoe UI", 8), typeof(Font));
            doc.Tags.Add(newTag);

            doc.DocumentStartingScript =
                @"
		        Dim headerTags as TagCollection = DirectCast(Document.Tags, TagCollection)
		        Document.Style.Font = headerTags!AppointmentsFont.Value
		        Document.PageLayout.PageFooter.Style.TextColor = headerTags!FooterColor.Value
                Document.PageLayouts.EvenPages.PageFooter.Style.TextColor =headerTags !FooterColor.Value
                Document.PageLayout.PageHeader.Style.TextColor = headerTags!HeaderColor.Value
                Document.PageLayouts.EvenPages.PageHeader.Style.TextColor = headerTags!HeaderColor.Value
		        Document.PageLayout.PageFooter.Style.Font = headerTags!FooterFont.Value
		        Document.PageLayouts.EvenPages.PageFooter.Style.Font = headerTags!FooterFont.Value
		        Document.PageLayout.PageHeader.Style.Font = headerTags!HeaderFont.Value
		        Document.PageLayouts.EvenPages.PageHeader.Style.Font = headerTags!HeaderFont.Value
		    ";
        }

        /// <summary>
        /// Fills specified control with a Week style.
        /// </summary>
        /// <param name="doc"></param>
        public static void MakeWeek(C1PrintDocument doc)
        {
            doc.Clear();


            doc.DocumentInfo.Title = "Weekly style";
            doc.DocumentInfo.Subject = "Week";
            AddNamespaces(doc);

            AddHeadersFooters(doc);
            doc.Tags["FooterRight"].Value = "[GeneratedDateTime]";
            doc.Tags["DateHeadingsFont"].Value = new Font("Segoe UI", 20, FontStyle.Bold);

            doc.DocumentStartingScript +=
                string.Format(
                    @"
					Dim indexTags as TagCollection = DirectCast(Document.Tags, TagCollection)
					Dim IncludeTasks = Convert.ToBoolean(indexTags!IncludeTasks.Value )
					Dim IncludeBlankNotes = Convert.ToBoolean(indexTags!IncludeBlankNotes.Value)
					Dim IncludeLinedNotes = Convert.ToBoolean(indexTags!IncludeLinedNotes.Value)

					Dim tasksNumber As Integer = 0
					If IncludeTasks = True Then
						tasksNumber = tasksNumber + 1
					End If

					If IncludeBlankNotes = True Then
						tasksNumber = tasksNumber + 1
					End If

					If IncludeLinedNotes = True Then
						tasksNumber = tasksNumber + 1
					End If 

					Dim unit as Unit
                    If tasksNumber = 1 Then
                        unit = New Unit({0})
					ElseIf tasksNumber = 2 Then
                        unit = New Unit({1}) 
					Else
                        unit = New Unit({2})
					End If 

					indexTags!TaskHeight.Value = unit

					Dim startT As DateTime = Convert.ToDateTime(indexTags!StartDate.Value)
					Dim endT As DateTime = Convert.ToDateTime(indexTags!EndDate.Value)
					Dim calendarInfo as C1.Schedule.CalendarInfo = DirectCast(indexTags!CalendarInfo.Value, C1.Schedule.CalendarInfo)
					Dim cultureInfo as System.Globalization.CultureInfo = DirectCast(calendarInfo.CultureInfo, System.Globalization.CultureInfo)

					While Convert.ToInt32(startT.DayOfWeek) <> Convert.ToInt32(calendarInfo.WeekStart)
						startT = startT.AddDays(-1)
					End While

					Dim days as Long = endT.Subtract(startT).TotalDays

					endT = endT.AddDays(7 - (days Mod 7))
					Dim Weeks As List(Of DateTime) = New List(Of DateTime)
					Dim s As DateTime = startT

					' Creating weeks
					While s < endT
						Weeks.Add(s)
						s = s.AddDays(7)
					End While 

                    ' Create weeks collection
					Dim weekIndex = indexTags.IndexByName({3})
					Dim tagWeeks as Tag = New Tag({3}, weeks)
					tagWeeks.SerializeValue = False

					If weekIndex >=0 Then
						indexTags!Weeks.Value = Weeks
					Else
						indexTags.Add(tagWeeks)
					End if

					Dim appointments = DirectCast(indexTags!Appointments.Value, C1.Schedule.AppointmentList)
					Dim dateAppointments = New DateAppointmentsCollection(startT, endT, appointments, calendarInfo, True, True)
					Dim indexAppointments = indexTags.IndexByName({4})

					Dim tagApps as Tag = New Tag({4}, dateAppointments)
					tagApps.SerializeValue = False 

                    ' Create appointment collection
                    If indexAppointments >= 0 Then
						indexTags!DateAppointments.Value = dateAppointments
                    Else
                        indexTags.Add(tagApps)
                    End If"
            ,
            string.Format("\"{0}\"", "100%"),                   // 0
            string.Format("\"{0}\"", "50%"),                    // 1
            string.Format("\"{0}\"", "33%"),                    // 2 
            string.Format("\"{0}\"", "Weeks"),                  // 3
            string.Format("\"{0}\"", "DateAppointments"));      // 4


            // RenderArea representing the single page
            RenderArea raPage = new RenderArea();
            raPage.DataBinding.DataSource = new Expression("Document.Tags!Weeks.Value");
            raPage.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            raPage.BreakBefore = BreakEnum.Page;
            raPage.Width = "100%";
            raPage.Height = "100%";
            raPage.Stacking = StackingRulesEnum.InlineLeftToRight;
            raPage.Style.Borders.All = LineDef.Default;

            #region ** week header

            // month header
            RenderArea raWeekHeader = new RenderArea();
            raWeekHeader.Style.Borders.All = LineDef.Default;
            raWeekHeader.Width = "100%";
            raWeekHeader.Height = "30mm";
            raWeekHeader.Stacking = StackingRulesEnum.InlineLeftToRight;
            raWeekHeader.Style.TextAlignVert = AlignVertEnum.Bottom;

            RenderText rt = new RenderText();
            rt.FormatDataBindingInstanceScript = string.Format(
                @"
					Dim documentTags as TagCollection = DirectCast(Document.Tags, TagCollection)
					RenderObject.Style.Font = documentTags!DateHeadingsFont.Value
					Dim rt As RenderText = RenderObject

					Dim startDate As DateTime = Convert.ToDateTime(RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value)
					Dim endDate As DateTime = startDate.AddDays(6)

					' Formatting text
					Dim cultureInfo = {6}

					Dim txt As String = startDate.ToString({0},cultureInfo)
					If startDate.Year <> endDate.Year Then
						txt = txt & {1} & startDate.ToString({2}, cultureInfo)
					End If

					txt = txt &  {3} & endDate.ToString({4}, cultureInfo)
					rt.Text = txt

					' Fill calendar
					Dim monthCalendars as List(of DateTime) = New List(of DateTime)
					Dim indexMonthCalendar = documentTags.IndexByName({5})

					startDate = New DateTime(startDate.Year, startDate.Month, 1)
					While startDate <= endDate
						monthCalendars.Add(startDate)
						startDate = startDate.AddMonths(1)
					End While 

					If indexMonthCalendar >= 0 Then
						documentTags!MonthCalendars.Value = monthCalendars
					End if

					",
                    "\"MMMM d\"",                                   // 0
                    "\" \"",                                        // 1
                    "\"yyy\"",                                      // 2
                    "\" - \"",                                      // 3
                    "\"MMMM d yyy\"",                               // 4
                    string.Format("\"{0}\"", "MonthCalendars"),     // 5
                    Utils.GetStringCultureInfo());                  // 6

            rt.Style.TextAlignVert = AlignVertEnum.Center;
            rt.Style.Spacing.Left = "2mm";
            rt.Height = "100%";
            rt.Width = "parent.Width - 70mm";

            raWeekHeader.Children.Add(rt);

            RenderArea monthCalendar = new RenderArea();
            monthCalendar.DataBinding.DataSource = new Expression("Document.Tags!MonthCalendars.Value");
            monthCalendar.Stacking = StackingRulesEnum.InlineLeftToRight;
            monthCalendar.Style.Spacing.Left = "1mm";
            monthCalendar.Style.Spacing.Right = "1mm";
            monthCalendar.Style.Spacing.Top = "0.5mm";
            monthCalendar.Width = "34mm";
            monthCalendar.Height = "100%";
            monthCalendar.Style.TextAlignVert = AlignVertEnum.Bottom;
            monthCalendar.Style.TextAlignHorz = AlignHorzEnum.Center;


            // Creating month name
            rt = new RenderText(
                string.Format(
                        @"[Convert.ToDateTime(Fields!Date.Value).ToString({0},{1})]",
                            "\"MMMM yyyy\"",
                            Utils.GetStringCultureInfo()
                        ));

            rt.FormatDataBindingInstanceScript =
                string.Format(
                    @"
					Dim documentTags as TagCollection = DirectCast(Document.Tags, TagCollection)
					Dim startDate As DateTime = Convert.ToDateTime(RenderObject.Original.Parent.DataBinding.Fields!Date.Value)
					Dim endDate As DateTime = startDate.AddMonths(1).AddDays(-1)

					Dim calendarInfo as C1.Schedule.CalendarInfo = DirectCast(documentTags!CalendarInfo.Value, C1.Schedule.CalendarInfo)

					While startDate.DayOfWeek <> calendarInfo.WeekStart 
						startDate = startDate.AddDays(-1)
					End While
				
					' Add week list
					Dim weekList as List(of DateTime) =  New List(of DateTime)
					Dim indexWeekNumber = documentTags.IndexByName({0})
					Dim tagWeekNumber as Tag = New Tag({0}, weekList)
					tagWeekNumber.SerializeValue = False 

					If indexWeekNumber = -1 then
						documentTags.Add(tagWeekNumber)
					End if

					' Fill week list
					While startDate <= endDate
						weekList.Add(startDate)
						startDate = startDate.AddDays(7)
					End While
					documentTags!WeekNumber.Value = weekList
					",
                    string.Format("\"{0}\"", "WeekNumber"));
            rt.Style.TextAlignHorz = AlignHorzEnum.Center;
            rt.Style.TextAlignVert = AlignVertEnum.Top;

            rt.Style.Font = new Font("Segoe UI", 7f);
            rt.Width = "100%";
            monthCalendar.Children.Add(rt);

            // Creating space at left
            rt = new RenderText(" ");
            rt.Style.Font = new Font("Arial", 7f);
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";

            monthCalendar.Children.Add(rt);

            // Creating days legend
            rt = new RenderText(
                string.Format(
                    @"[CType ({0}.DateTimeFormat, System.Globalization.DateTimeFormatInfo).GetShortestDayName(System.Convert.ToDateTime(Fields!Date.Value).DayOfWeek)]"
                , Utils.GetStringCultureInfo()
                ));
            rt.DataBinding.DataSource = new Expression(
                string.Format(
                    @"New DateAppointmentsCollection(Convert.ToDateTime( CType(Document.Tags!WeekNumber.Value, List(of DateTime))(0) ), Convert.ToDateTime( CType(Document.Tags!WeekNumber.Value, List(of DateTime))(0) ).AddDays(6), CType(Document.Tags, TagCollection)({0}).Value, True)"
                    ,
                    string.Format("\"{0}\"", "Appointments")));

            rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            rt.Style.Borders.Bottom = LineDef.Default;
            rt.Style.Font = new Font("Arial", 7f);
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";

            monthCalendar.Children.Add(rt);

            // Creating numbers of week
            RenderArea raWeek = new RenderArea();
            raWeek.DataBinding.DataSource = new Expression("DirectCast(Document.Tags, TagCollection)!WeekNumber.Value");
            raWeek.Style.Font = new Font("Arial", 7f);
            raWeek.Style.TextAlignVert = AlignVertEnum.Top;
            raWeek.Style.TextAlignHorz = AlignHorzEnum.Center;

            raWeek.Width = "100%";
            raWeek.Stacking = StackingRulesEnum.InlineLeftToRight;

            rt = new RenderText(
                string.Format(
                    @"[{0}.Calendar.GetWeekOfYear( Convert.ToDateTime(Fields!Date.Value), System.Globalization.CalendarWeekRule.FirstDay, {1}.WeekStart)]",
                    Utils.GetStringCultureInfo(),
                    Utils.GetStringCalendarInfo()));

            rt.Style.Borders.Right = LineDef.Default;
            rt.Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";
            raWeek.Children.Add(rt);

            // Creating grid of calendar
            rt = new RenderText(
                string.Format(
                    @"[Convert.ToDateTime(Fields!Date.Value).ToString({0}, {1})]",
                    "\"%d\"",
                    Utils.GetStringCultureInfo()));

            rt.DataBinding.DataSource = new Expression(string.Format(
                    @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value).AddDays(6), CType(Document.Tags, TagCollection)({0}).Value, True)"
                    ,
                    string.Format("\"{0}\"", "Appointments")));

            rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            rt.FormatDataBindingInstanceScript =
                    @"If System.Convert.ToDateTime(RenderObject.Original.DataBinding.Fields!Date.Value).Month <> System.Convert.ToDateTime(RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value).Month Then
						RenderObject.Visibility = VisibilityEnum.Hidden
					  End If

					  Dim HasAppointments = Convert.ToBoolean( 	RenderObject.Original.DataBinding.Fields!HasAppointments.Value)
					  If HasAppointments = True Then
							RenderObject.Style.FontBold = true
					  End If";

            rt.Style.Font = new Font("Arial", 6f);
            rt.Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Width = "12.5%";

            raWeek.Children.Add(rt);

            monthCalendar.Children.Add(raWeek);
            raWeekHeader.Children.Add(monthCalendar);

            raPage.Children.Add(raWeekHeader);
            #endregion

            #region ** week

            // month 
            RenderArea raWeekBody = new RenderArea();
            raWeekBody.FormatDataBindingInstanceScript =
                string.Format(
                    @"
					Dim documentTag = CType(Document.Tags, TagCollection)
					Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
					Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
					Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

					If Not (IncludeTasks = True Or IncludeBlankNotes = True Or IncludeLinedNotes = True) Then
						RenderObject.Width = {0}
					End If", " \"100%\"");

            raWeekBody.Style.Spacing.Top = "0.5mm";
            raWeekBody.Style.Borders.All = LineDef.Default;
            raWeekBody.Width = "75%";
            raWeekBody.Height = "parent.Height - 28mm";
            raWeekBody.Stacking = StackingRulesEnum.InlineLeftToRight;

            // RenderArea representing the single day
            RenderArea raDay = new RenderArea();
            raDay.DataBinding.DataSource = new Expression(string.Format(
                @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value).AddDays(6), CType(Document.Tags, TagCollection)({0}).Value, True)"
                ,
                string.Format("\"{0}\"", "Appointments")));
            raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            raDay.Style.Borders.Right = LineDef.Default;
            raDay.Style.Borders.Bottom = LineDef.Default;
            raDay.Stacking = StackingRulesEnum.InlineLeftToRight;
            raDay.Width = "50%";
            raDay.Height = "25%";

            rt = new RenderText(
                string.Format(
                    @"[Convert.ToDateTime(Fields!Date.Value).ToString({0},{1})]"
                    , "\"d  dddd\"",
                    Utils.GetStringCultureInfo()));

            rt.Style.Borders.Bottom = LineDef.Default;
            rt.Style.Padding.Left = "1mm";
            rt.Style.FontBold = true;
            rt.Style.BackColor = Colors.White;
            rt.Style.TextAlignHorz = AlignHorzEnum.Left;
            rt.Style.WordWrap = false;
            raDay.Children.Add(rt);

            RenderText status = new RenderText(" ");
            status.FormatDataBindingInstanceScript =
                    @"
					If RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value Is Nothing Then
						RenderObject.Visibility = VisibilityEnum.Collapse
                    Else
                        Dim status = CType(RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value, Status)
					    If Not status Is Nothing Then
						    Dim c1Brush = Ctype(status.Brush, C1Brush)
						    Dim brush = c1Brush.Brush
						    RenderObject.Style.Brush = brush
					    End if
					End If";

            status.Width = "100%";
            status.Height = "1.5mm";
            raDay.Children.Add(status);

            RenderArea appointments = new RenderArea();
            appointments.Height = "parent.Height - Y - 2.5mm";

            // RenderArea representing the single appointment
            RenderArea raApp = new RenderArea();
            raApp.Style.Spacing.All = "0.5mm";
            raApp.FormatDataBindingInstanceScript =
                    @"
                    Dim documentTab = CType(Document.Tags, TagCollection)
					Dim AllDayEvent = Convert.ToBoolean(RenderObject.Original.DataBinding.Fields!AllDayEvent.Value)
					If AllDayEvent = True Then
						RenderObject.Style.Borders.All = LineDef.Default
					End If 

                    If Not RenderObject.Original.DataBinding.Fields!Label Is Nothing Then
						Dim label = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label)
						If Not label Is Nothing Then
							Dim c1Brush = Ctype(label.Brush, C1Brush)
							Dim brush = c1Brush.Brush
							RenderObject.Style.Brush = brush
						End if
					End If
                    ";

            raApp.Stacking = StackingRulesEnum.InlineLeftToRight;
            // Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
            raApp.DataBinding.DataSource = new Expression("Parent.Fields!Appointments.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value");

            rt = new RenderText();
            rt.Text =
                string.Format(
                    @"[If(Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract( Convert.ToDateTime(Fields!Start.Value)).TotalSeconds > 0, {0}, 
						String.Empty) + {1} + If( Convert.ToBoolean(Fields!AllDayEvent.Value) = True, {1}, String.Format({3}, {2}, 
						If( Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract( Convert.ToDateTime(Fields!Start.Value)).TotalSeconds > 0, 
						DataBinding.Parent.Fields!Date.Value, Fields!Start.Value), 
						If( Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1).Subtract( Convert.ToDateTime(Fields!End.Value)).TotalSeconds < 0, Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1), Fields!End.Value) )	)] [Fields!Subject.Value]",
                    "\"> \"",                                       // 0
                    "\"\"",                                         // 1
                    string.Format("{0}", "\"{0:t} {1:t}\""),        // 2
                    Utils.GetStringCultureInfo(),                   // 3
                    "\" - \""                                       // 4
                    );

            rt.Width = "Auto";
            raApp.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                @"
					Dim location = Convert.ToString(RenderObject.Original.DataBinding.Parent.Fields!Location.Value)
					if( String.IsNullOrEmpty(location)) Then
						RenderObject.Visibility = VisibilityEnum.Collapse
					else
						RenderObject.Visibility = VisibilityEnum.Visible
					end if ";

            rt.Text = "[\" (\" + System.Convert.ToString(Fields!Location.Value) + \")\"]";
            rt.Width = "Auto";
            raApp.Children.Add(rt);

            appointments.Children.Add(raApp);
            raDay.Children.Add(appointments);

            RenderArea overflowArea = new RenderArea();
            overflowArea.Name = "overflow";
            RenderText arrow = new RenderText(new string((char)0x36, 1), new Font("Webdings", 10));
            arrow.Style.Padding.Top = "-1.5mm";
            arrow.X = "parent.Width - 4mm";
            overflowArea.Children.Add(arrow);
            overflowArea.ZOrder = 100;
            overflowArea.Height = "2.5mm";
            overflowArea.FragmentResolvedScript =
                @"
				If (C1PrintDocument.FormatVersion.AssemblyVersion.MinorRevision <> 100) Then
					Dim fragment As RenderFragment = RenderFragment.Parent.Children(2)
					If (fragment.HasChildren) Then
					If (RenderFragment.Parent.BoundsOnPage.Contains(fragment.Children(fragment.Children.Count - 1).BoundsOnPage)) Then
						RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden
					Else
						RenderFragment.RenderObject.Visibility = VisibilityEnum.Visible
					end if
					Else
					RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden
					end if
				Else
					RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden
				End if
				";

            raDay.Children.Add(overflowArea);

            raWeekBody.Children.Add(raDay);

            raPage.Children.Add(raWeekBody);
            #endregion

            #region ** tasks

            // tasks
            RenderArea include = new RenderArea();
            include.FormatDataBindingInstanceScript =
                @"
				Dim documentTag = DirectCast(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If  ( IncludeTasks = True Or IncludeBlankNotes = True Or IncludeLinedNotes = True) Then
				    RenderObject.Visibility = VisibilityEnum.Visible
				Else
				    RenderObject.Visibility = VisibilityEnum.Collapse
				End If";

            include.Width = "25%";
            include.Height = "parent.Height - 28mm";

            RenderArea raTasks = new RenderArea();
            raTasks.FormatDataBindingInstanceScript =
                @"
				Dim documentTag = DirectCast(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeTasks = False Then
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If

				RenderObject.Height = documentTag!TaskHeight.Value
				";


            raTasks.Style.Borders.All = LineDef.Default;
            raTasks.Style.Spacing.Top = "0.5mm";
            raTasks.Style.Spacing.Left = "0.5mm";
            raTasks.Width = "100%";

            rt = new RenderText();
            rt.Text = "Tasks";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raTasks.Children.Add(rt);
            include.Children.Add(raTasks);

            RenderArea raNotes = new RenderArea();
            raNotes.FormatDataBindingInstanceScript =
                @"
				Dim documentTag = DirectCast(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeBlankNotes = False Then
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If

				RenderObject.Height = documentTag!TaskHeight.Value
				";

            raNotes.Style.Borders.All = LineDef.Default;
            raNotes.Style.Spacing.Top = "0.5mm";
            raNotes.Style.Spacing.Left = "0.5mm";
            raNotes.Width = "100%";

            rt = new RenderText();
            rt.Text = "Notes";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raNotes.Children.Add(rt);
            include.Children.Add(raNotes);

            raNotes = new RenderArea();
            raNotes.FormatDataBindingInstanceScript =
                @"
				Dim documentTag = DirectCast(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeLinedNotes = False Then
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If

				RenderObject.Height = Document.Tags!TaskHeight.Value 
				";

            raNotes.Style.Borders.All = LineDef.Default;
            raNotes.Style.Spacing.Top = "0.5mm";
            raNotes.Style.Spacing.Left = "0.5mm";
            raNotes.Width = "100%";

            rt = new RenderText();
            rt.Text = "Notes";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raNotes.Children.Add(rt);

            RenderTable lines = new RenderTable();
            lines.Rows.Insert(0, 1);
            lines.Rows[0].Height = "0.5cm";
            TableVectorGroup gr = lines.RowGroups[0, 1];
            gr.Header = TableHeaderEnum.None;
            List<int> lst = new List<int>(60);
            for (int i = 0; i < 60; i++)
            {
                lst.Add(i);
            }
            gr.DataBinding.DataSource = lst;
            lines.Style.GridLines.Horz = LineDef.Default;
            lines.RowSizingMode = TableSizingModeEnum.Fixed;

            raNotes.Children.Add(lines);

            include.Children.Add(raNotes);

            raPage.Children.Add(include);
            #endregion

            doc.Body.Children.Add(raPage);

            AddCommonTags(doc);

            Tag newTag = new Tag("Appointments", null, typeof(IList<Appointment>));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("WeekNumber", null, typeof(List<DateTime>));
            newTag.SerializeValue = false;
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("MonthCalendars", null, typeof(List<DateTime>));
            newTag.SerializeValue = false;
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("StartDate", DateTime.Today, typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            doc.Tags.Add(newTag);
            newTag = new Tag("EndDate", DateTime.Today.AddDays(1), typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            doc.Tags.Add(newTag);
            newTag = new Tag("HidePrivateAppointments", false, typeof(bool));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("TaskHeight", new Unit("33.3%"), typeof(Unit));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("IncludeTasks", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Tasks";
            doc.Tags.Add(newTag);
            newTag = new Tag("IncludeBlankNotes", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Notes (blank)";
            doc.Tags.Add(newTag);
            newTag = new Tag("IncludeLinedNotes", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Notes (lined)";
            doc.Tags.Add(newTag);
        }

        /// <summary>
        /// Fills specified control with a Memo style.
        /// </summary>
        /// <param name="doc"></param>
        public static void MakeMemo(C1PrintDocument doc)
        {
            doc.Clear();

            doc.DocumentInfo.Title = "Memo style";
            doc.DocumentInfo.Subject = "Memo";

            AddNamespaces(doc);

            AddHeadersFooters(doc);
            doc.Tags["AppointmentsFont"].Value = new Font("Arial", 10);

            // add string tags (localizable)

            Tag newTag = new Tag("Subject", "Subject:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("Location", "Location:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("Start", "Start:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("End", "End:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("ShowTimeAs", "Show Time As:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("Recurrence", "Recurrence:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("None", "none", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("Categories", "Categories:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("Resources", "Resources:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("Contacts", "Contacts:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("Importance", "Importance:", typeof(string));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            RenderArea ra = new RenderArea();
            ra.Width = "100%";
            ra.BreakBefore = BreakEnum.Page;
            ra.DataBinding.DataSource = new Expression("Document.Tags!Appointments.Value");
            ra.DataBinding.Sorting.Expressions.Add("Fields!Start.Value");
            ra.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value");
            ra.Style.Borders.Top = LineDef.DefaultBold;
            ra.Stacking = StackingRulesEnum.InlineLeftToRight;
            ra.Style.Padding.Top = "1mm";
            ra.Style.Spacing.All = "1mm";

            RenderText rt = new RenderText();
            rt.FormatDataBindingInstanceScript = //VisibilityEnum
                "RenderObject.Visibility = If(Not RenderObject.Parent.Original.DataBinding.Fields!Subject.Value Is Nothing, VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Document.Tags!Subject.Value]";
            rt.Width = "25%";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If(Not RenderObject.Parent.Original.DataBinding.Fields!Subject.Value Is Nothing, VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Fields!Subject.Value]";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If(Not RenderObject.Parent.Original.DataBinding.Fields!Location.Value Is Nothing, VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Document.Tags!Location.Value]";
            rt.Width = "25%";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If(Not RenderObject.Parent.Original.DataBinding.Fields!Location.Value Is Nothing, VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Fields!Location.Value]";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.Text = "[Document.Tags!Start.Value]";
            rt.Width = "25%";
            rt.Style.Padding.Top = "2mm";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.Text = string.Format(@"[String.Format({0}, {1} , System.Convert.ToDateTime(Fields!Start.Value), System.Convert.ToDateTime(Fields!Start.Value) )]"
                        , Utils.GetStringCultureInfo(), "\"{0:ddd} {1:g}\"");
            rt.Width = "75%";
            rt.Style.Padding.Top = "2mm";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.Text = "[Document.Tags!End.Value]";
            rt.Width = "25%";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.Text = string.Format(@"[String.Format({0}, {1} , System.Convert.ToDateTime(Fields!End.Value), System.Convert.ToDateTime(Fields!End.Value) )]"
            , Utils.GetStringCultureInfo(), "\"{0:ddd} {1:g}\"");

            //rt.Text = "[String.Format(Document.Tags!CalendarInfo.Value.CultureInfo, \"{0:ddd} {1:g}\", System.Convert.ToDateTime(Fields!End.Value), System.Convert.ToDateTime(Fields!End.Value) )]";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.Text = "[Document.Tags!ShowTimeAs.Value]";
            rt.Width = "25%";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.Text = "[CType( Fields!BusyStatus.Value, C1.Schedule.Status).Text]";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.Text = "[Document.Tags!Recurrence.Value]";
            rt.Width = "25%";
            rt.Style.Padding.Top = "2mm";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                 @"Dim index as Integer = RenderObject.Parent.Original.DataBinding.RowNumber - 1
                 Dim documentTags = CType(Document.Tags, TagCollection)
                 Dim appointments = TryCast(documentTags!Appointments.Value, IList(Of C1.Schedule.Appointment))

                 If appointments Is Nothing Then
                     appointments = DirectCast(documentTags!Appointments.Value, C1.Schedule.AppointmentList)
                 End If

                 Dim app = appointments(index)

                 Dim text As RenderText = RenderObject
                 If app.RecurrenceState <> RecurrenceStateEnum.NotRecurring Then
                     text.Text = app.GetRecurrencePattern().Description
                 End If";

            rt.Style.Padding.Top = "2mm";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( Not RenderObject.Parent.Original.DataBinding.Fields!Links.Value Is Nothing And Not RenderObject.Parent.Original.DataBinding.Fields!Links.Value Is Nothing , VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Document.Tags!Contacts.Value]";
            rt.Style.Padding.Top = "2mm";
            rt.Width = "25%";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( Not RenderObject.Parent.Original.DataBinding.Fields!Links.Value Is Nothing And Not RenderObject.Parent.Original.DataBinding.Fields!Links.Value Is Nothing , VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Fields!Links.Value.ToString()]";
            rt.Style.Padding.Top = "2mm";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( Not RenderObject.Parent.Original.DataBinding.Fields!Categories.Value Is Nothing , VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Document.Tags!Categories.Value]";
            rt.Style.Padding.Top = "2mm";
            rt.Width = "25%";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( Not RenderObject.Parent.Original.DataBinding.Fields!Categories.Value Is Nothing, VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Fields!Categories.Value.ToString()]";
            rt.Style.Padding.Top = "2mm";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( Not RenderObject.Parent.Original.DataBinding.Fields!Resources.Value Is Nothing , VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Document.Tags!Resources.Value]";
            rt.Width = "25%";
            rt.Style.Padding.Top = "2mm";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( Not RenderObject.Parent.Original.DataBinding.Fields!Resources.Value Is Nothing , VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Fields!Resources.Value.ToString()]";
            rt.Style.Padding.Top = "2mm";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( Not RenderObject.Parent.Original.DataBinding.Fields!Importance.Value Is Nothing , VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Text = "[Document.Tags!Importance.Value]";
            rt.Style.Padding.Top = "2mm";
            rt.Width = "25%";
            rt.Style.FontBold = true;
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( Not RenderObject.Parent.Original.DataBinding.Fields!Importance.Value Is Nothing , VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt.Style.Padding.Top = "2mm";
            rt.Text = "[Fields!Importance.Value.ToString()]";
            rt.Width = "75%";
            ra.Children.Add(rt);

            rt = new RenderText();
            rt.Text = "[Fields!Body.Value.ToString()]";
            rt.Style.Padding.Top = "3mm";
            rt.Style.Padding.Right = "2mm";
            rt.Width = "100%";
            ra.Children.Add(rt);

            doc.Body.Children.Add(ra);

            AddCommonTags(doc);

            newTag = new Tag("Appointments", null, typeof(IList<Appointment>));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
        }

        /// <summary>
        /// Fills specified control with a Details style.
        /// </summary>
        /// <param name="doc"></param>
        public static void MakeDetails(C1PrintDocument doc)
        {
            doc.Clear();

            doc.DocumentInfo.Title = "Details style";
            doc.DocumentInfo.Subject = "Details";
            AddNamespaces(doc);

            AddHeadersFooters(doc);
            doc.Tags["FooterRight"].Value = "[GeneratedDateTime]";

            doc.DocumentStartingScript += 
                string.Format(
                @"
				Dim documentTags = CType(Document.Tags, TagCollection)
				Dim PageBreakString as String = System.Convert.ToString( documentTags!PageBreak.Value)
				Dim InsertPageBreaks = System.Convert.ToBoolean(documentTags!InsertPageBreaks.Value)

				If InsertPageBreaks = True And not String.Equals(PageBreakString,{0}) Then
					If documentTags.IndexByName({1}) = -1 Then
					    Dim tag As Tag
				       tag = New Tag({1}, System.Convert.ToDateTime(documentTags!StartDate.Value))
				       Document.Tags.Add(tag)
				   Else
				       documentTags!LastDate.Value = System.Convert.ToDateTime(documentTags!StartDate.Value) 
				   End If
				End If

				Dim dateAppointments As DateAppointmentsCollection = New DateAppointmentsCollection( System.Convert.ToDateTime(documentTags!StartDate.Value), 
								System.Convert.ToDateTime(documentTags!EndDate.Value), documentTags!Appointments.Value, false)

				If Tags.IndexByName({2}) = -1 Then
				   Dim tagApps As Tag
				   tagApps = New Tag({2}, dateAppointments)
				   tagApps.SerializeValue = False
				   documentTags.Add(tagApps)
				Else
				   documentTags!DateAppointments.Value = dateAppointments
				End If",
                            "\"Day\"",                                      // 0
                            "\"LastDate\"",                                 // 1
                            "\"DateAppointments\""                          // 2
                );

            // RenderArea representing the single day
            RenderArea ra1 = new RenderArea();
            ra1.DataBinding.DataSource = new Expression("Document.Tags!DateAppointments.Value");
            ra1.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            ra1.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim InsertPageBreaks = System.Convert.ToBoolean( documentTag!InsertPageBreaks.Value )

				If InsertPageBreaks = True Then
					Dim newDate as DateTime
					newDate = System.Convert.ToDateTime(RenderObject.Original.DataBinding.Fields!Date.Value)
					Dim mode = System.Convert.ToString(documentTag!PageBreak.Value)

					If mode.Equals({0}) Then
						Dim tmp as DateTime
						tmp = System.Convert.ToDateTime(Tags!LastDate.Value).AddDays(1)
						While tmp <= newDate
							If System.Convert.ToInt32(tmp.DayOfWeek) = System.Convert.ToInt32({2}.WeekStart) Then
								RenderObject.BreakBefore = BreakEnum.Page
								Exit While
							End If

							tmp = tmp.AddDays(1)
						End While
					End If
						
					If mode.Equals({1}) Then
						If System.Convert.ToDateTime(documentTag!LastDate.Value).Month <> newDate.Month Then
							RenderObject.BreakBefore = BreakEnum.Page
						End If
	 				End If

					If not mode.Equals({1}) and Not  mode.Equals({0}) Then
						RenderObject.BreakBefore = BreakEnum.Page
					End If

				    documentTag!LastDate.Value = newDate
				End If",
                "\"Week\"",                                     // 0
                "\"Month\"",                                    // 1
                Utils.GetStringCalendarInfo()                   // 2
                );

            ra1.Style.Spacing.All = "1mm";

            // day header
            RenderText rt = new RenderText(string.Format(
                    "[String.Format({0}, {1}, System.Convert.ToDateTime(Fields!Date.Value))]",
                    Utils.GetStringCultureInfo(), "\"{0:D}\""));

            rt.FormatDataBindingInstanceScript =
                "RenderObject.Style.Font = Document.Tags!DateHeadingsFont.Value";
            rt.Style.Borders.All = LineDef.Default;
            rt.Style.Padding.All = "2mm";
            rt.Style.BackColor = Colors.LightGray;

            ra1.Children.Add(rt);

            // RenderArea representing the single appointment
            RenderArea raApp = new RenderArea();
            raApp.Style.Spacing.All = "2mm";
            raApp.Stacking = StackingRulesEnum.InlineLeftToRight;

            // Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
            raApp.DataBinding.DataSource = new Expression("Parent.Fields!Appointments.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value");

            rt = new RenderText();
            var d1 = new DateTime();
            rt.Text = 
                string.Format(
                @"[	If( Convert.ToBoolean(Fields!AllDayEvent.Value) = True, {0}, String.Format	({1}, {2},	
						If(System.Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract(  System.Convert.ToDateTime(Fields!Start.Value) ).TotalSeconds > 0,	Convert.ToDateTime( DataBinding.Parent.Fields!Date.Value), Convert.ToDateTime( Fields!Start.Value)), If(System.Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1).Subtract(  System.Convert.ToDateTime(Fields!End.Value) ).TotalSeconds < 0, Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1), Convert.ToDateTime(Fields!End.Value))	))]",
                "\"All Day\"",
                Utils.GetStringCultureInfo(),
                "\"{0:t} - {1:t}\""
                );

            rt.Width = "25%";
            rt.Style.FontBold = true;
            raApp.Children.Add(rt);

            RenderArea appDetails = new RenderArea();
            appDetails.Width = "75%";
            appDetails.Stacking = StackingRulesEnum.InlineLeftToRight;

            RenderText rt1 = new RenderText();
            rt1.Text = "[Fields!Subject.Value] ";
            rt1.Style.FontBold = true;
            rt1.Width = "Auto";
            appDetails.Children.Add(rt1);

            rt1 = new RenderText();
            rt1.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim location = Convert.ToString(RenderObject.Original.DataBinding.Parent.Fields!Location.Value)
				RenderObject.Visibility = If(Not String.IsNUllOrEmpty(location), VisibilityEnum.Visible, VisibilityEnum.Collapse)");

            rt1.Text = "[\"-- \" + System.Convert.ToString(Fields!Location.Value)]";
            rt1.Style.FontBold = true;
            rt1.Width = "Auto";
            appDetails.Children.Add(rt1);

            rt1 = new RenderText();
            rt1.FormatDataBindingInstanceScript =
                @"
				Dim location = Convert.ToString(RenderObject.Original.DataBinding.Parent.Fields!Location.Value)
				RenderObject.Visibility =  If(Not RenderObject.Original.DataBinding.Parent.Fields!Body.Value Is Nothing, VisibilityEnum.Visible, VisibilityEnum.Collapse)
				RenderObject.BreakBefore = If(Not (RenderObject.Original.DataBinding.Parent.Fields!Subject.Value Is Nothing and String.IsNullOrEmpty(location)), BreakEnum.Line, BreakEnum.None)";
            rt1.Text = "[Fields!Body.Value]";
            appDetails.Children.Add(rt1);

            rt1 = new RenderText();
            rt1.FormatDataBindingInstanceScript =
                "RenderObject.Visibility = If( System.Convert.ToDateTime(RenderObject.Original.DataBinding.Parent.Parent.Fields!Date.Value).Subtract(   System.Convert.ToDateTime(RenderObject.Original.DataBinding.Parent.Fields!Start.Value )).TotalSeconds > 0, VisibilityEnum.Visible, VisibilityEnum.Collapse)";
            rt1.Text = "[\"Please See Above\"]";
            rt1.Style.FontBold = true;
            rt1.BreakBefore = BreakEnum.Line;
            appDetails.Children.Add(rt1);

            raApp.Children.Add(appDetails);

            ra1.Children.Add(raApp);
            doc.Body.Children.Add(ra1);

            AddCommonTags(doc);

            Tag newTag = new Tag("Appointments", null, typeof(IList<Appointment>));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("StartDate", DateTime.Today, typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            doc.Tags.Add(newTag);
            newTag = new Tag("EndDate", DateTime.Today.AddDays(1), typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            doc.Tags.Add(newTag);
            newTag = new Tag("InsertPageBreaks", false, typeof(Boolean));
            newTag.Description = "Start a new page each";
            doc.Tags.Add(newTag);
            newTag = new Tag("PageBreak", "Day", typeof(string));
            newTag.Description = "";
            newTag.InputParams = new TagListInputParams(TagListInputParamsTypeEnum.ComboBox, new TagListInputParamsItem("Day", "Day"), new TagListInputParamsItem("Week", "Week"), new TagListInputParamsItem("Month", "Month"));
            doc.Tags.Add(newTag);
            newTag = new Tag("HidePrivateAppointments", false, typeof(bool));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
        }

        /// <summary>
        /// Fills specified control with a Month style.
        /// </summary>
        /// <param name="doc"></param>
        public static void MakeMonth(C1PrintDocument doc)
        {
            doc.Clear();

            doc.DocumentInfo.Title = "Monthly style";
            doc.DocumentInfo.Subject = "Month";
            AddNamespaces(doc);

            AddHeadersFooters(doc);
            doc.PageLayout.PageSettings.Landscape = true;
            doc.Tags["FooterRight"].Value = "[GeneratedDateTime]";
            doc.Tags["DateHeadingsFont"].Value = new Font("Segoe UI", 20, FontStyle.Bold);

            doc.DocumentStartingScript +=
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				Dim daysNumber As Long = 7.0
				Dim WorkDaysOnly = Convert.ToBoolean(documentTag!WorkDaysOnly.Value)

				If WorkDaysOnly = True Then
					daysNumber = {0}.WorkDays.Count 
				End If

				documentTag!DayWidth.Value = New Unit((97.3/daysNumber - 0.005).ToString({1}, System.Globalization.CultureInfo.InvariantCulture) + {3})
				Dim tasksNumber As Integer = 0

				If IncludeTasks = True Then
					tasksNumber = tasksNumber + 1
				End If

				If IncludeBlankNotes = True Then
					tasksNumber = tasksNumber + 1
				End If

				If IncludeLinedNotes = True Then
					tasksNumber = tasksNumber + 1
				End If

				If tasksNumber = 1 Then
						documentTag!TaskHeight.Value = New Unit({4})
				ElseIf tasksNumber = 2 Then
						documentTag!TaskHeight.Value = New Unit({5})
				Else
					    documentTag!TaskHeight.Value = New Unit({6})
				End If

				Dim startT As DateTime = System.Convert.ToDateTime(documentTag!StartDate.Value)
				Dim endT As DateTime = System.Convert.ToDateTime( documentTag!EndDate.Value )

				While System.Convert.ToInt32( startT.DayOfWeek) <> System.Convert.ToInt32( {0}.WeekStart)
					startT = startT.AddDays(-1)
				End While

				Dim days As Long = endT.Subtract(startT).TotalDays
				endT = endT.AddDays(35 - (days Mod 35))
				Dim months As New List(Of Date)
				Dim s As DateTime = startT

				While s < endT
					months.Add(s)
					s = s.AddDays(35)
				End While

				If Document.Tags.IndexByName({7}) = -1 Then
					Dim tagMonths As Tag
					tagMonths = New Tag({7}, months)
					tagMonths.SerializeValue = False
					documentTag.Add(tagMonths)
				Else
					documentTag!Months.Value = months
				End If

				Dim calendarInfo = {0}
				Dim appointments = DirectCast(documentTag!Appointments.Value, C1.Schedule.AppointmentList)
				Dim dateAppointments = New DateAppointmentsCollection(startT, endT, appointments, calendarInfo, True,  Not WorkDaysOnly)

				If Document.Tags.IndexByName({8}) = -1 Then
					Dim tagApps As Tag
					tagApps = New Tag({8}, dateAppointments)
					tagApps.SerializeValue = False
					documentTag.Add(tagApps)
				Else
					documentTag!DateAppointments.Value = dateAppointments
				End If",
                Utils.GetStringCalendarInfo(),                              // 0
                "\"#.###\"",                                                // 1
                Utils.GetStringCultureInfo(),                               // 2
                "\"%\"",                                                    // 3
                "\"100%\"",                                                 // 4	
                "\"50%\"",                                                  // 5
                "\"33.3 %\"",                                               // 6
                "\"Months\"",                                               // 7
                "\"DateAppointments\""                                      // 8
            );

            // RenderArea representing the single page
            RenderArea raPage = new RenderArea();
            raPage.DataBinding.DataSource = new Expression("Document.Tags!Months.Value");
            raPage.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            raPage.BreakBefore = BreakEnum.Page;
            raPage.Width = "100%";
            raPage.Height = "100%";
            raPage.Stacking = StackingRulesEnum.InlineLeftToRight;

            #region ** month header

            // month header
            RenderArea raMonthHeader = new RenderArea();
            raMonthHeader.Style.Borders.All = LineDef.Default;
            raMonthHeader.Width = "100%";
            raMonthHeader.Height = "28mm";
            raMonthHeader.Stacking = StackingRulesEnum.InlineLeftToRight;

            RenderText rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				RenderObject.Style.Font = documentTag!DateHeadingsFont.Value
				Dim rt As RenderText = RenderObject
				Dim startDate As DateTime = System.Convert.ToDateTime( RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value)
				Dim endDate As DateTime = startDate.AddDays(34)
				Dim cultureInfo = {0}

				Dim txt As String = startDate.ToString({1}, cultureInfo)
				If startDate.Year <> endDate.Year Then
					txt = txt + {2} + startDate.ToString({3}, cultureInfo)
				End If

				txt = txt + {4} + endDate.ToString({5}, cultureInfo)
				rt.Text = txt

				Dim weeks = New List(of DateTime)
				documentTag!WeekTabs.Value = weeks

				weeks.Add(startDate)
				weeks.Add(startDate.AddDays(7))
				weeks.Add(startDate.AddDays(14))
				weeks.Add(startDate.AddDays(21))
				weeks.Add(startDate.AddDays(28))

				Dim monthCalenar =  New List(of DateTime)
				documentTag!MonthCalendars.Value = monthCalenar

				startDate = New DateTime(startDate.Year, startDate.Month, 1)
				While startDate <= endDate
					monthCalenar.Add(startDate)
					startDate = startDate.AddMonths(1)
				End While",
                Utils.GetStringCultureInfo(),
                "\"MMMM\"",
                "\" \"",
                "\"yyy\"",
                "\" - \"",
                "\"MMMM yyy\""
                );

            rt.Style.TextAlignVert = AlignVertEnum.Center;
            rt.Style.Spacing.Left = "2mm";
            rt.Height = "100%";
            rt.Width = "parent.Width - 102mm";

            raMonthHeader.Children.Add(rt);

            RenderArea monthCalendar = new RenderArea();
            monthCalendar.DataBinding.DataSource = new Expression("Document.Tags!MonthCalendars.Value");
            monthCalendar.Stacking = StackingRulesEnum.InlineLeftToRight;
            monthCalendar.Style.Spacing.Left = "1mm";
            monthCalendar.Style.Spacing.Right = "1mm";
            monthCalendar.Style.Spacing.Top = "0.5mm";
            monthCalendar.Width = "34mm";

            rt = new RenderText(string.Format("[System.Convert.ToDateTime(Fields!Date.Value).ToString({0}, {1})]", "\"MMMM yyy\"", Utils.GetStringCultureInfo()));

            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim startDate As Date = System.Convert.ToDateTime(RenderObject.Original.Parent.DataBinding.Fields!Date.Value)
                Dim endDate As Date = startDate.AddMonths(1).AddDays(-1)
				Dim documentTags = CType(Document.Tags, TagCollection)

                While startDate.DayOfWeek <> {0}.WeekStart
                    startDate = startDate.AddDays(-1)
                End While

				Dim weeks = New List(of DateTime)
                documentTags!WeekNumber.Value = weeks

                While startDate <= endDate
                    weeks.Add(startDate)
                    startDate = startDate.AddDays(7)
                End While",
                Utils.GetStringCalendarInfo());

            rt.Style.TextAlignHorz = AlignHorzEnum.Center;
            rt.Style.Font = new Font("Segoe UI", 8);
            rt.Width = "100%";
            monthCalendar.Children.Add(rt);

            rt = new RenderText(" ");
            rt.Style.Font = new Font("Arial", 7f);
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";
            monthCalendar.Children.Add(rt);

            rt = new RenderText(
            string.Format(
                @"[CType ({0}.DateTimeFormat, System.Globalization.DateTimeFormatInfo).GetShortestDayName(System.Convert.ToDateTime(Fields!Date.Value).DayOfWeek)]"
            , Utils.GetStringCultureInfo()
            ));
            rt.DataBinding.DataSource = new Expression(
                string.Format(
                    @"New DateAppointmentsCollection(Convert.ToDateTime( CType(Document.Tags!WeekNumber.Value, List(of DateTime))(0) ), Convert.ToDateTime( CType(Document.Tags!WeekNumber.Value, List(of DateTime))(0) ).AddDays(6), CType(Document.Tags, TagCollection)({0}).Value, True)"
                    ,
                    string.Format("\"{0}\"", "Appointments")));

            rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            rt.Style.Borders.Bottom = LineDef.Default;
            rt.Style.Font = new Font("Arial", 7f);
            rt.Style.TextAlignHorz = AlignHorzEnum.Center;
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";
            monthCalendar.Children.Add(rt);

            RenderArea raWeek = new RenderArea();
            raWeek.DataBinding.DataSource = new Expression("Document.Tags!WeekNumber.Value");
            raWeek.Style.Font = new Font("Arial", 7f);
            raWeek.Width = "100%";
            raWeek.Stacking = StackingRulesEnum.InlineLeftToRight;

            // Creating month calendar
            rt = new RenderText(
                string.Format(
                    @"[{0}.Calendar.GetWeekOfYear( Convert.ToDateTime(Fields!Date.Value), System.Globalization.CalendarWeekRule.FirstDay, {1}.WeekStart)]",
                    Utils.GetStringCultureInfo(),
                    Utils.GetStringCalendarInfo()));

            rt.Style.Borders.Right = LineDef.Default;
            rt.Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";
            raWeek.Children.Add(rt);

            rt = new RenderText(string.Format(@"[System.Convert.ToDateTime(Fields!Date.Value).ToString({0},{1})]",
                "\"%d\"", Utils.GetStringCultureInfo()));
            rt.DataBinding.DataSource = new Expression(
                string.Format(
                    @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value).AddDays(6), CType(Document.Tags, TagCollection)({0}).Value, True)"
                    ,
                    string.Format("\"{0}\"", "Appointments")));

            rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"If System.Convert.ToDateTime(RenderObject.Original.DataBinding.Fields!Date.Value).Month <> System.Convert.ToDateTime(RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value).Month Then
					RenderObject.Visibility = VisibilityEnum.Hidden
					End If
					Dim HasAppointments = Convert.ToBoolean(RenderObject.Original.DataBinding.Fields!HasAppointments.Value)
					If  HasAppointments = True Then
						RenderObject.Style.FontBold = true
					End if
				");

            rt.Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Width = "12.5%";
            raWeek.Children.Add(rt);

            monthCalendar.Children.Add(raWeek);
            raMonthHeader.Children.Add(monthCalendar);

            raPage.Children.Add(raMonthHeader);
            #endregion

            #region ** month

            // month 
            RenderArea raMonth = new RenderArea();
            raMonth.FormatDataBindingInstanceScript =
                string.Format(
                @"Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If Not (IncludeTasks = True Or IncludeBlankNotes = True Or IncludeLinedNotes = True) Then
						RenderObject.Width = {0}
				End If",
                "\"100%\"");

            raMonth.Style.Spacing.Top = "0.5mm";
            raMonth.Style.Borders.Top = LineDef.Default;
            raMonth.Style.Borders.Left = LineDef.Default;
            raMonth.Style.Borders.Bottom = LineDef.Default;
            raMonth.Width = "80%";
            raMonth.Height = "parent.Height - 28mm";
            raMonth.Stacking = StackingRulesEnum.InlineLeftToRight;

            rt = new RenderText(" ");
            rt.Style.WordWrap = false;
            rt.Height = "4%";
            rt.Style.TextAngle = 90;
            rt.Width = "2.7%";
            raMonth.Children.Add(rt);

            rt = new RenderText(
                string.Format(
                @"[System.Convert.ToDateTime(Fields!Date.Value).ToString({0},{1})]",
                "\"dddd\"",
                Utils.GetStringCultureInfo()));

            rt.DataBinding.DataSource = new Expression(
                string.Format(
                @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value).AddDays( If( Convert.ToBoolean(CType(Document.Tags, TagCollection)!WorkDaysOnly.Value)  = True , 4, 6) ), CType(Document.Tags, TagCollection)({0}).Value,  True )"
                ,
                string.Format("\"{0}\"", "Appointments")));

            rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				RenderObject.Width = documentTag!DayWidth.Value");

            rt.Style.Borders.Bottom = LineDef.Default;
            rt.Style.Borders.Right = LineDef.Default;
            rt.Style.TextAlignHorz = AlignHorzEnum.Center;
            rt.Style.WordWrap = false;
            rt.Height = "4%";
            raMonth.Children.Add(rt);

            RenderArea raWeekTab = new RenderArea();
            raWeekTab.DataBinding.DataSource = new Expression("Document.Tags!WeekTabs.Value");
            raWeekTab.Height = "19.2%";
            raWeekTab.Width = "100%";
            raWeekTab.Stacking = StackingRulesEnum.InlineLeftToRight;

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"Dim txt As RenderText = RenderObject
				Dim start As DateTime = System.Convert.ToDateTime(RenderObject.Parent.Original.DataBinding.Fields!Date.Value)
				Dim endT As DateTime = start.AddDays(6)
				If start.Month = endT.Month Then
					txt.Text = start.ToString({0},{1}) + {2} + endT.ToString({3},{1})
				Else
					txt.Text = start.ToString({3},{1}) + {2} + endT.ToString({3},{1})
				End If",
                "\"%d\"",                                               // 0
                Utils.GetStringCultureInfo(),                           // 1
                "\" - \"",                                              // 2
                "\"d.MM\"");                                            // 3

            rt.Style.Borders.Right = LineDef.Default;
            rt.Style.FontBold = true;
            rt.Style.TextAngle = 90;
            rt.Style.TextAlignHorz = AlignHorzEnum.Center;
            rt.Style.WordWrap = false;
            rt.Height = "100%";
            rt.Width = "2.7%";
            raWeekTab.Children.Add(rt);

            // RenderArea representing the single day
            RenderArea raDay = new RenderArea();
            raDay.DataBinding.DataSource = new Expression(
                string.Format(
                @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value).AddDays(6), CType(Document.Tags, TagCollection)({0}).Value, True)"
                ,
                string.Format("\"{0}\"", "Appointments")));

            raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            raDay.Style.Borders.Right = LineDef.Default;
            raDay.Style.Borders.Bottom = LineDef.Default;
            raDay.FormatDataBindingInstanceScript =
                "RenderObject.Width = Document.Tags!DayWidth.Value";
            raDay.Height = "100%";

            // day header
            rt = new RenderText(
                string.Format(
                @"[IF( System.Convert.ToDateTime(Fields!Date.Value).Day = 1 Or System.Convert.ToDateTime(Fields!Date.Value) = System.Convert.ToDateTime( CType( CType(Document.Tags, TagCollection)!WeekTabs.Value, List(Of DateTime))(0).Date), 
								String.Format({1}, {0}, System.Convert.ToDateTime(Fields!Date.Value)) , System.Convert.ToDateTime(Fields!Date.Value).Day)]",
                "\"{0:m}\"",
                Utils.GetStringCultureInfo()));

            rt.Style.Borders.Bottom = LineDef.Default;
            rt.Style.Padding.Left = "1mm";
            rt.Style.FontBold = true;
            rt.Style.BackColor = Colors.White;
            rt.Style.TextAlignHorz = AlignHorzEnum.Left;
            rt.Style.WordWrap = false;
            raDay.Children.Add(rt);

            RenderText status = new RenderText(" ");
            status.FormatDataBindingInstanceScript =
                string.Format(
                    @"
					If RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value Is Nothing Then
							RenderObject.Visibility = VisibilityEnum.Collapse
                    Else
					    Dim status = CType(RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value, Status)
					    If Not status Is Nothing Then
						    Dim c1Brush = Ctype(status.Brush, C1Brush)
						    Dim brush = c1Brush.Brush
						    RenderObject.Style.Brush = brush
					    End if
					End If");

            status.Width = "100%";
            status.Height = "1.5mm";
            raDay.Children.Add(status);

            RenderArea appointments = new RenderArea();
            appointments.Height = "parent.Height - Y - 2.5mm";

            // RenderArea representing the single appointment
            RenderArea raApp = new RenderArea();
            raApp.Style.Spacing.All = "0.2mm";
            raApp.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim AllDayEvent = Convert.ToBoolean(RenderObject.Original.DataBinding.Fields!AllDayEvent.Value)
				If AllDayEvent = True Then
					RenderObject.Style.Borders.All = LineDef.Default
				End If

                If Not RenderObject.Original.DataBinding.Fields!Label Is Nothing Then
					Dim label = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label)
					If Not label Is Nothing Then
						Dim c1Brush = Ctype(label.Brush, C1Brush)
						Dim brush = c1Brush.Brush
						RenderObject.Style.Brush = brush
					End if
				End If
                ");
            raApp.Stacking = StackingRulesEnum.InlineLeftToRight;
            raApp.Height = "14pt";

            // Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
            raApp.DataBinding.DataSource = new Expression("Parent.Fields!Appointments.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value");

            rt = new RenderText();
            rt.Style.WordWrap = false;
            rt.Text =
                string.Format(
                @"[If(Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract( Convert.ToDateTime(Fields!Start.Value)).TotalSeconds > 0, {0}, 
					String.Empty) + {1} + If( Convert.ToBoolean(Fields!AllDayEvent.Value) = True, {1}, String.Format({3}, {2}, 
					If( Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract( Convert.ToDateTime(Fields!Start.Value)).TotalSeconds > 0, 
					DataBinding.Parent.Fields!Date.Value, Fields!Start.Value), 
					If( Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1).Subtract( Convert.ToDateTime(Fields!End.Value)).TotalSeconds < 0, Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1), Fields!End.Value) )	)] [Fields!Subject.Value]",
                "\"> \"",                                       // 0
                "\"\"",                                         // 1
                string.Format("{0}", "\"{0:t} {1:t}\""),        // 2
                Utils.GetStringCultureInfo(),                   // 3
                "\" - \""                                       // 4
                );

            rt.Width = "Auto";
            raApp.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                @"
				Dim location = Convert.ToString(RenderObject.Original.DataBinding.Parent.Fields!Location.Value)
				if( String.IsNullOrEmpty(location)) Then
						RenderObject.Visibility = VisibilityEnum.Collapse
					else
						RenderObject.Visibility = VisibilityEnum.Visible
					end if ";

            rt.Text = "[\" (\" + System.Convert.ToString(Fields!Location.Value) + \")\"]";
            rt.Style.WordWrap = false;
            rt.Width = "Auto";
            raApp.Children.Add(rt);

            appointments.Children.Add(raApp);
            raDay.Children.Add(appointments);

            RenderArea overflowArea = new RenderArea();
            overflowArea.Name = "overflow";
            RenderText arrow = new RenderText(new string((char)0x36, 1), new Font("Webdings", 10));
            arrow.Style.Padding.Top = "-1.5mm";
            arrow.X = "parent.Width - 4mm";
            overflowArea.Children.Add(arrow);
            overflowArea.ZOrder = 100;
            overflowArea.Height = "2.5mm";
            overflowArea.FragmentResolvedScript =
                @"
				If (C1PrintDocument.FormatVersion.AssemblyVersion.MinorRevision <> 100) Then
					Dim fragment As RenderFragment = RenderFragment.Parent.Children(2)
					If (fragment.HasChildren) Then
					If (RenderFragment.Parent.BoundsOnPage.Contains(fragment.Children(fragment.Children.Count - 1).BoundsOnPage)) Then
						RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden
					else
						RenderFragment.RenderObject.Visibility = VisibilityEnum.Visible
					end if
					else
					RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden
					end if
				Else
					RenderFragment.RenderObject.Visibility = VisibilityEnum.Hidden
				End if
				";
            raDay.Children.Add(overflowArea);

            raWeekTab.Children.Add(raDay);
            raMonth.Children.Add(raWeekTab);

            raPage.Children.Add(raMonth);
            #endregion

            #region ** tasks
            // tasks
            RenderArea include = new RenderArea();
            include.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If (IncludeTasks = True Or IncludeBlankNotes = True Or IncludeLinedNotes = True) Then
					RenderObject.Visibility = VisibilityEnum.Visible
				Else
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If");

            include.Width = "20%";
            include.Height = "parent.Height - 28mm";

            RenderArea raTasks = new RenderArea();
            raTasks.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeTasks = False Then
						RenderObject.Visibility = VisibilityEnum.Collapse
				End If
				RenderObject.Height = Document.Tags!TaskHeight.Value");

            raTasks.Style.Borders.All = LineDef.Default;
            raTasks.Style.Spacing.Top = "0.5mm";
            raTasks.Style.Spacing.Left = "0.5mm";
            raTasks.Width = "100%";

            rt = new RenderText();
            rt.Text = "Tasks";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raTasks.Children.Add(rt);
            include.Children.Add(raTasks);

            RenderArea raNotes = new RenderArea();
            raNotes.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeBlankNotes = False Then
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If
				RenderObject.Height = Document.Tags!TaskHeight.Value");

            raNotes.Style.Borders.All = LineDef.Default;
            raNotes.Style.Spacing.Top = "0.5mm";
            raNotes.Style.Spacing.Left = "0.5mm";
            raNotes.Width = "100%";

            rt = new RenderText();
            rt.Text = "Notes";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raNotes.Children.Add(rt);
            include.Children.Add(raNotes);

            raNotes = new RenderArea();
            raNotes.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeLinedNotes = False Then
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If
				RenderObject.Height = Document.Tags!TaskHeight.Value");

            raNotes.Style.Borders.All = LineDef.Default;
            raNotes.Style.Spacing.Top = "0.5mm";
            raNotes.Style.Spacing.Left = "0.5mm";
            raNotes.Width = "100%";

            rt = new RenderText();
            rt.Text = "Notes";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raNotes.Children.Add(rt);

            RenderTable lines = new RenderTable();
            lines.Rows.Insert(0, 1);
            lines.Rows[0].Height = "0.5cm";
            TableVectorGroup gr = lines.RowGroups[0, 1];
            gr.Header = TableHeaderEnum.None;
            List<int> lst = new List<int>(60);
            for (int i = 0; i < 60; i++)
            {
                lst.Add(i);
            }
            gr.DataBinding.DataSource = lst;
            lines.Style.GridLines.Horz = LineDef.Default;
            lines.RowSizingMode = TableSizingModeEnum.Fixed;

            raNotes.Children.Add(lines);

            include.Children.Add(raNotes);

            raPage.Children.Add(include);
            #endregion

            doc.Body.Children.Add(raPage);

            AddCommonTags(doc);

            Tag newTag = new Tag("Appointments", null, typeof(IList<Appointment>));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("WeekTabs", null, typeof(List<DateTime>));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("WeekNumber", null, typeof(List<DateTime>));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("MonthCalendars", null, typeof(List<DateTime>));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("StartDate", DateTime.Today, typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            doc.Tags.Add(newTag);
            newTag = new Tag("EndDate", DateTime.Today.AddDays(1), typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            doc.Tags.Add(newTag);
            newTag = new Tag("HidePrivateAppointments", false, typeof(bool));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("TaskHeight", new Unit("33.3%"), typeof(Unit));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("DayWidth", new Unit("13.9%"), typeof(Unit));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("IncludeTasks", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Tasks";
            doc.Tags.Add(newTag);
            newTag = new Tag("IncludeBlankNotes", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Notes (blank)";
            doc.Tags.Add(newTag);
            newTag = new Tag("IncludeLinedNotes", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Notes (lined)";
            doc.Tags.Add(newTag);
            newTag = new Tag("WorkDaysOnly", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Only Print Workdays";
            doc.Tags.Add(newTag);
        }

        /// <summary>
        /// Fills specified control with a Day style.
        /// </summary>
        /// <param name="doc"></param>
        public static void MakeDay(C1PrintDocument doc)
        {
            doc.Clear();

            doc.DocumentInfo.Title = "Daily style";
            doc.DocumentInfo.Subject = "Day";
            AddNamespaces(doc);

            AddHeadersFooters(doc);
            doc.Tags["FooterRight"].Value = "[GeneratedDateTime]";
            doc.Tags["DateHeadingsFont"].Value = new Font("Segoe UI", 24, FontStyle.Bold);

            doc.DocumentStartingScript +=
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim tasksNumber As Integer = 0
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeTasks = True Then
					tasksNumber = tasksNumber + 1
				End If

				If IncludeBlankNotes = True Then
					tasksNumber = tasksNumber + 1
				End If

				If IncludeLinedNotes = True Then
					tasksNumber = tasksNumber + 1
				End If

				If tasksNumber = 1 Then
					documentTag!TaskHeight.Value = New Unit({0})
				ElseIf tasksNumber = 2 Then
					documentTag!TaskHeight.Value = New Unit({1})
				Else
					documentTag!TaskHeight.Value = New Unit({2})
				End If

				Dim startT As DateTime = Convert.ToDateTime(documentTag!StartTime.Value)
				Dim endT As DateTime = Convert.ToDateTime( documentTag!EndTime.Value)

				Dim startDate as DateTime = Convert.ToDateTime(documentTag!StartDate.Value)
				Dim endDate as DateTime = Convert.ToDateTime(documentTag!EndDate.Value)

				Dim calendarInfo = {4}
				Dim appointments = DirectCast(documentTag!Appointments.Value, C1.Schedule.AppointmentList)
				Dim dateAppointments = New DateAppointmentsCollection(startDate, endDate, appointments, calendarInfo, True, True)
				Dim indexAppointments = documentTag.IndexByName({3})

				If indexAppointments = -1 Then
					Dim tagApps As Tag
					tagApps = New Tag({3}, dateAppointments)
					tagApps.SerializeValue = False
					documentTag.Add(tagApps)
				Else
					documentTag!DateAppointments.Value = dateAppointments
				End If

				If startT > endT Then
					documentTag!StartTime.Value = endT
					documentTag!EndTime.Value = startT
				End If",
                    "\"100%\"",                                                                     // 0
                    "\"50%\"",                                                                      // 1
                    "\"33.3%\"",                                                                    // 2
                    "\"DateAppointments\"",                                                         // 3
                    Utils.GetStringCalendarInfo()
                );


            // RenderArea representing the single page
            RenderArea raPage = new RenderArea();
            raPage.DataBinding.DataSource = new Expression("Document.Tags!DateAppointments.Value");
            raPage.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            raPage.BreakBefore = BreakEnum.Page;
            raPage.Width = "100%";
            raPage.Height = "100%";
            raPage.Stacking = StackingRulesEnum.InlineLeftToRight;

            #region ** day header
            // day header
            RenderArea raDayHeader = new RenderArea();
            raDayHeader.Style.Borders.All = LineDef.Default;
            raDayHeader.Width = "100%";
            raDayHeader.Height = "28mm";
            raDayHeader.Stacking = StackingRulesEnum.InlineLeftToRight;

            RenderText rt = new RenderText(
                string.Format(
                    @"[Convert.ToDateTime(Fields!Date.Value).ToString({0},{1})]"
                    ,
                    "\"D\"",
                    Utils.GetStringCultureInfo()));

            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				RenderObject.Style.Font = documentTag!DateHeadingsFont.Value

				Dim bindStartDate As DateTime =  Convert.ToDateTime(RenderObject.Original.Parent.Parent.DataBinding.Fields!Date.Value)

				Dim startDateTime as DateTime =  bindStartDate.Add(  Convert.ToDateTime(documentTag!StartTime.Value).TimeOfDay) 
				Dim endDateTime as DateTime =  bindStartDate.Add(  Convert.ToDateTime(documentTag!EndTime.Value).TimeOfDay).AddMinutes(-30) 
				documentTag!StartTime.Value = startDateTime
				documentTag!EndTime.Value = endDateTime.AddMinutes(30)
				documentTag!MonthCalendar.Value = New DateTime(bindStartDate.Year, bindStartDate.Month, 1)

				Dim days = New Dictionary(of DateTime, DateTime)
				documentTag!DayHours.Value = days

				Dim startPeriod = Convert.ToDateTime( documentTag!StartTime.Value)

				While startDateTime <= endDateTime
					days.Add(startDateTime, startDateTime)
					startDateTime = startDateTime.AddMinutes(30)
				End While

					");
            rt.Style.TextAlignVert = AlignVertEnum.Center;
            rt.Style.Spacing.Left = "2mm";
            rt.Height = "100%";
            rt.Width = "parent.Width - 38mm";

            raDayHeader.Children.Add(rt);

            RenderArea monthCalendar = new RenderArea();
            monthCalendar.Stacking = StackingRulesEnum.InlineLeftToRight;
            monthCalendar.Style.Spacing.Left = "1mm";
            monthCalendar.Style.Spacing.Right = "3mm";
            monthCalendar.Style.Spacing.Top = "0.5mm";
            monthCalendar.Width = "36mm";

            rt = new RenderText(
                string.Format(
                @"[Convert.ToDateTime(Document.Tags!MonthCalendar.Value).ToString({0},{1})]", "\"MMMM yyy\"", Utils.GetStringCultureInfo()));
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim startDate As DateTime = Convert.ToDateTime(documentTag!MonthCalendar.Value)
				Dim endDate As DateTime = Convert.ToDateTime( startDate.AddMonths(1).AddDays(-1))
				While startDate.DayOfWeek <> {0}.WeekStart
						startDate = startDate.AddDays(-1)
				End While

				Dim weeks = New List(of DateTime)
				documentTag!WeekNumber.Value = weeks

				While startDate < endDate
					weeks.Add(startDate)
					startDate = startDate.AddDays(7)
				End While",
                Utils.GetStringCalendarInfo());

            rt.Style.TextAlignHorz = AlignHorzEnum.Center;
            rt.Style.Font = new Font("Segoe UI", 8);
            rt.Width = "100%";
            monthCalendar.Children.Add(rt);

            rt = new RenderText(" ");
            rt.Style.Font = new Font("Arial", 7f);
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";
            monthCalendar.Children.Add(rt);

            rt = new RenderText(
            string.Format(
                @"[CType ({0}.DateTimeFormat, System.Globalization.DateTimeFormatInfo).GetShortestDayName(System.Convert.ToDateTime(Fields!Date.Value).DayOfWeek)]"
            , Utils.GetStringCultureInfo()
            ));
            rt.DataBinding.DataSource = new Expression(
                string.Format(
                @"New DateAppointmentsCollection(Convert.ToDateTime( CType(Document.Tags!WeekNumber.Value, List(of DateTime))(0) ), Convert.ToDateTime( CType(Document.Tags!WeekNumber.Value, List(of DateTime))(0) ).AddDays(6), CType(Document.Tags, TagCollection)({0}).Value, True)"
                ,
                string.Format("\"{0}\"", "Appointments")));

            rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            rt.Style.Borders.Bottom = LineDef.Default;
            rt.Style.Font = new Font("Arial", 7f);
            rt.Style.TextAlignHorz = AlignHorzEnum.Center;
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";
            monthCalendar.Children.Add(rt);

            RenderArea raWeek = new RenderArea();
            raWeek.DataBinding.DataSource = new Expression("Document.Tags!WeekNumber.Value");
            raWeek.Style.Font = new Font("Arial", 7f);
            raWeek.Width = "100%";
            raWeek.Stacking = StackingRulesEnum.InlineLeftToRight;

            rt = new RenderText(
            string.Format(
                @"[{0}.Calendar.GetWeekOfYear( Convert.ToDateTime(Fields!Date.Value), System.Globalization.CalendarWeekRule.FirstDay, {1}.WeekStart)]",
                Utils.GetStringCultureInfo(),
                Utils.GetStringCalendarInfo()));

            rt.Style.Borders.Right = LineDef.Default;
            rt.Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Style.WordWrap = false;
            rt.Width = "12.5%";
            raWeek.Children.Add(rt);

            rt = new RenderText(
                string.Format(
                @"[System.Convert.ToDateTime(Fields!Date.Value).ToString({0},{1})]",
                "\"%d\"",
                Utils.GetStringCultureInfo()));

            rt.DataBinding.DataSource = new Expression(string.Format(
                @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value).AddDays(6), CType(Document.Tags, TagCollection)({0}).Value, True)"
                ,
                string.Format("\"{0}\"", "Appointments")));

            rt.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"If Convert.ToDateTime(RenderObject.Original.DataBinding.Fields!Date.Value).Month <> Convert.TodateTime( CType(Document.Tags, TagCollection)!MonthCalendar.Value).Month Then
					RenderObject.Visibility = VisibilityEnum.Hidden
					End if

                    Dim HasAppointments = Convert.ToBoolean(RenderObject.Original.DataBinding.Fields!HasAppointments.Value)
    				If HasAppointments = True Then
						RenderObject.Style.FontBold = true
					End If
				");

            rt.Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Width = "12.5%";
            raWeek.Children.Add(rt);
            monthCalendar.Children.Add(raWeek);

            raDayHeader.Children.Add(monthCalendar);

            raPage.Children.Add(raDayHeader);
            #endregion

            #region ** day
            // day 
            RenderArea raDayBody = new RenderArea();
            raDayBody.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If  Not (IncludeTasks = True Or IncludeBlankNotes = True Or IncludeLinedNotes = True ) Then
					RenderObject.Width = {0}
				End If", "\"100%\"");

            raDayBody.Style.Spacing.Top = "0.5mm";
            raDayBody.Style.Borders.All = LineDef.Default;
            raDayBody.Width = "75%";
            raDayBody.Height = "parent.Height - 28mm";
            raDayBody.Stacking = StackingRulesEnum.BlockTopToBottom;

            #region ** all-day events

            // RenderArea representing the single day
            RenderArea raDay = new RenderArea();
            raDay.DataBinding.DataSource = new Expression(
                string.Format(
                    @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value), CType(Document.Tags, TagCollection)({0}).Value, True)"
                    ,
                    string.Format("\"{0}\"", "Appointments")));
            raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            raDay.Stacking = StackingRulesEnum.InlineLeftToRight;
            raDay.Height = "Auto";

            RenderText status = new RenderText(" ");
            status.FormatDataBindingInstanceScript =
                string.Format(
                @"
				If RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value Is Nothing Then
					RenderObject.Visibility = VisibilityEnum.Collapse
				Else
                    Dim status = CType(RenderObject.Original.DataBinding.Parent.Fields!BusyStatus.Value, Status)
					If Not status Is Nothing Then
						Dim c1Brush = Ctype(status.Brush, C1Brush)
						Dim brush = c1Brush.Brush
						RenderObject.Style.Brush = brush
					End If
				End If
				");

            status.Width = "100%";
            status.Height = "1.5mm";
            raDay.Children.Add(status);

            // RenderArea representing the single appointment
            RenderArea raApp = new RenderArea();
            raApp.Style.Spacing.All = "0.5mm";
            raApp.Style.Spacing.Left = "1.25cm";

            raApp.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim AllDayEvent = Convert.ToBoolean(RenderObject.Original.DataBinding.Fields!AllDayEvent.Value)
				If AllDayEvent = True Then
					RenderObject.Style.Borders.All = LineDef.Default
				End if

				' Show or hide the panel
				If Convert.ToDateTime(Document.Tags!StartTime.Value).Subtract( Convert.ToDateTime( RenderObject.Original.DataBinding.Fields!Start.Value )).TotalSeconds <= 0 Then
						RenderObject.Visibility = VisibilityEnum.Collapse
				End if

				If Not RenderObject.Original.DataBinding.Fields!Label Is Nothing Then
					Dim label = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label)
					If Not label Is Nothing Then
						Dim c1Brush = Ctype(label.Brush, C1Brush)
						Dim brush = c1Brush.Brush
						RenderObject.Style.Brush = brush
					End if
				End If
				");

            raApp.Stacking = StackingRulesEnum.InlineLeftToRight;
            // Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
            raApp.DataBinding.DataSource = new Expression("Parent.Fields!Appointments.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value");

            rt = new RenderText();
            rt.Text =
                string.Format(
                @"[If(Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract( Convert.ToDateTime(Fields!Start.Value)).TotalSeconds > 0, {0}, 
							String.Empty) + {1} + If( Convert.ToBoolean(Fields!AllDayEvent.Value) = True, {1}, String.Format({3}, {2}, 
							If( Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract( Convert.ToDateTime(Fields!Start.Value)).TotalSeconds > 0, 
							DataBinding.Parent.Fields!Date.Value, Fields!Start.Value), 
							If( Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1).Subtract( Convert.ToDateTime(Fields!End.Value)).TotalSeconds < 0, Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1), Fields!End.Value) )	)] [Fields!Subject.Value]",
                "\"> \"",                                       // 0
                "\"\"",                                         // 1
                string.Format("{0}", "\"{0:t} {1:t}\""),        // 2
                Utils.GetStringCultureInfo(),                   // 3
                "\" - \""                                       // 4
                );

            raApp.Children.Add(rt);

            rt = new RenderText();
            rt.Text = " [Fields!Subject.Value] ";
            rt.Width = "Auto";
            raApp.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim location = Convert.ToString(RenderObject.Original.DataBinding.Parent.Fields!Location.Value)
				RenderObject.Visibility = If( Not String.IsNullOrEmpty(location), VisibilityEnum.Visible, VisibilityEnum.Collapse)");

            rt.Text = "[\" (\" + System.Convert.ToString(Fields!Location.Value) + \")\"]";
            rt.Width = "Auto";
            raApp.Children.Add(rt);

            raDay.Children.Add(raApp);

            raDayBody.Children.Add(raDay);
            #endregion

            #region ** slots
            raDay = new RenderArea();
            raDay.DataBinding.DataSource = new Expression(
                string.Format(
                @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value), CType(Document.Tags, TagCollection)({0}).Value, True)"
                ,
                string.Format("\"{0}\"", "Appointments")));

            raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            raDay.Stacking = StackingRulesEnum.InlineLeftToRight;
            raDay.Height = "parent - prev.height - next.height";

            RenderArea raTimeSlot = new RenderArea();
            raTimeSlot.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim dayHours = CType(documentTag!DayHours.Value, Dictionary(Of DateTime, DateTime))

				RenderObject.Height = New Unit((100/Convert.ToInt32(dayHours.Keys.Count() ) - 0.005).ToString({0}, System.Globalization.CultureInfo.InvariantCulture) + {1})",
                "\"#.###\"",                                            // 0
                "\"%\""                                                 // 1
                );

            raTimeSlot.DataBinding.DataSource = new Expression("Document.Tags!DayHours.Value");
            raTimeSlot.Width = "100%";
            raTimeSlot.Height = "0.5cm";
            raTimeSlot.Stacking = StackingRulesEnum.InlineLeftToRight;

            RenderParagraph rp = new RenderParagraph();
            rp.FormatDataBindingInstanceScript =
            string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim startPeriod = Convert.ToDateTime( RenderObject.Original.DataBinding.Parent.Fields!Key.Value)
				Dim yesTopPanel =  If( startPeriod.Minute = 0, False, True)

				Dim endPeriod as DateTime = startPeriod.AddMinutes(30)
				Dim dateAppointments = CType(documentTag!DateAppointments.Value, DateAppointmentsCollection)
				Dim data = dateAppointments.GetIntervalAppointments(startPeriod, endPeriod, False)
				documentTag!SlotAppointments.Value = data

				RenderObject.Visibility = If(yesTopPanel = False, VisibilityEnum.Visible, VisibilityEnum.Hidden)

				Dim headerFont As Font = documentTag!DateHeadingsFont.Value
				RenderObject.Style.Font = New Font(headerFont.FontFamily, headerFont.Size * 0.5) 
				", "\"Appointments\"");

            rp.Width = "1.65cm";
            rp.Style.TextAlignHorz = AlignHorzEnum.Right;
            rp.Style.Borders.Top = LineDef.Default;
            rp.Style.Padding.Right = "1mm";
            rp.Height = "100%";

            ParagraphText pt = new ParagraphText(
                string.Format(
                @"[If(  String.IsNullOrEmpty({0}.DateTimeFormat.AMDesignator), Convert.ToDateTime(Fields!Key.Value).ToString({1}), Convert.ToDateTime(Fields!Key.Value).ToString({2}))]"
                , Utils.GetStringCultureInfo(),                                         // 0
                "\"%H\"",                                                               // 1
                "\"%h\""                                                                // 2	
                ));
            pt.Style.FontBold = true;
            pt.Style.Padding.Right = "1mm";
            rp.Content.Add(pt);

            rp.Content.Add(
                new ParagraphText(
                string.Format(
                @"[If( string.IsNullOrEmpty({0}.DateTimeFormat.AMDesignator), {1}, Convert.ToDateTime(Fields!Key.Value).ToString({2}, {0}).ToLower())]",
                Utils.GetStringCultureInfo(),
                "\"00\"",
                "\"tt\""),
                TextPositionEnum.Superscript));
            raTimeSlot.Children.Add(rp);

            RenderArea slot = new RenderArea();
            slot.Width = "parent.width - prev.width - prev.left";
            slot.Height = "100%";
            slot.Style.Borders.Top = LineDef.Default;
            slot.Style.Borders.Left = LineDef.Default;
            slot.Stacking = StackingRulesEnum.InlineLeftToRight;

            // RenderArea representing the single appointment
            raApp = new RenderArea();
            // Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
            raApp.DataBinding.DataSource = new Expression("Document.Tags!SlotAppointments.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value");
            raApp.Style.Spacing.All = "0.5mm";
            raApp.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags , TagCollection)
				Dim slotAppointments = CType( documentTag!SlotAppointments.Value , List(Of Appointment))
				RenderObject.Width = New Unit( (100 / Convert.ToInt32(slotAppointments.Count() ) - 0.005).ToString({0}, System.Globalization.CultureInfo.InvariantCulture) + {2})

				If Not RenderObject.Original.DataBinding.Fields!Label Is Nothing Then
					Dim label = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label)
					If Not label Is Nothing Then
						Dim c1Brush = Ctype(label.Brush, C1Brush)
						Dim brush = c1Brush.Brush
						RenderObject.Style.Brush = brush
					End if
				End If",
                "\"#.###\"",
                Utils.GetStringCultureInfo(),
                "\"%\"");

            raApp.Width = "10%";
            raApp.Height = "100%";
            raApp.Stacking = StackingRulesEnum.InlineLeftToRight;

            rt = new RenderText();
            rt.Text = 
                string.Format(
                @"[String.Format({0}, {1}, Convert.ToDateTime(Fields!Start.Value), Convert.ToDateTime(Fields!End.Value)).ToLower()]"
                ,
                Utils.GetStringCultureInfo(),
                "\"{0:t}-{1:t}\""
                );

            rt.Width = "Auto";
            raApp.Children.Add(rt);

            rt = new RenderText();
            rt.Text = " [Fields!Subject.Value] ";
            rt.Width = "Auto";
            raApp.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim location = Convert.ToString(RenderObject.Original.DataBinding.Parent.Fields!Location.Value)
				RenderObject.Visibility = If( Not String.IsNullOrEmpty(location), VisibilityEnum.Visible, VisibilityEnum.Collapse)");

            rt.Text = string.Format(
                @"[{0} + {1} + System.Convert.ToString(Fields!Location.Value) + {2}]",
                    "\" \"",
                    "\"(\"",
                    "\")\"");

            rt.Width = "Auto";
            raApp.Children.Add(rt);

            slot.Children.Add(raApp);

            raTimeSlot.Children.Add(slot);

            raDay.Children.Add(raTimeSlot);
            raDayBody.Children.Add(raDay);
            #endregion

            #region ** late appointments

            raDay = new RenderArea();
            raDay.DataBinding.DataSource =
                string.Format(
                        @"New DateAppointmentsCollection(Convert.ToDateTime(Parent.Fields!Date.Value), Convert.ToDateTime(Parent.Fields!Date.Value), CType(Document.Tags, TagCollection)({0}).Value, True)"
                        ,
                        string.Format("\"{0}\"", "Appointments"));

            raDay.DataBinding.Sorting.Expressions.Add("Fields!Date.Value");
            raDay.Style.Borders.Top = LineDef.Default;
            raDay.Stacking = StackingRulesEnum.InlineLeftToRight;
            raDay.Height = "Auto";

            // RenderArea representing the single appointment
            raApp = new RenderArea();
            raApp.Style.Spacing.All = "0.5mm";
            raApp.Style.Spacing.Left = "1.25cm";

            raApp.FormatDataBindingInstanceScript =
            string.Format(
                @"
                Dim documentTag = CType(Document.Tags, TagCollection)
				Dim startPeriod = Convert.ToDateTime(documentTag!StartTime.Value)
				Dim keyPeriod = Convert.ToDateTime(RenderObject.Original.DataBinding.Parent.Fields!Key.Value)
				Dim yesTopPanel = If(keyPeriod.Minute = 0, False, True)

				RenderObject.Visibility = If(yesTopPanel = False, VisibilityEnum.Visible, VisibilityEnum.Hidden)

				If Convert.ToDateTime(Document.Tags!EndTime.Value).Subtract( Convert.ToDateTime(Document.Tags!EndTime.Value)).TotalSeconds >= 0 Then
					RenderObject.Visibility = VisibilityEnum.Visible
				Else
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If

				If Not RenderObject.Original.DataBinding.Fields!Label Is Nothing Then
					Dim label = CType(RenderObject.Original.DataBinding.Fields!Label.Value, Label)
					If Not label Is Nothing Then
						Dim c1Brush = Ctype(label.Brush, C1Brush)
						Dim brush = c1Brush.Brush
						RenderObject.Style.Brush = brush
					End if
				End If");

            raApp.Stacking = StackingRulesEnum.InlineLeftToRight;
            // Set the text's data source to the data source of the containing  RenderArea - this indicates that the render object is bound to the current group in the specified object:
            raApp.DataBinding.DataSource = new Expression("Parent.Fields!Appointments.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Start.Value");
            raApp.DataBinding.Sorting.Expressions.Add("Fields!Subject.Value");

            rt = new RenderText();
            rt.Text =
                string.Format(
                @"[If(Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract( Convert.ToDateTime(Fields!Start.Value)).TotalSeconds > 0, {0}, 
					String.Empty) + {1} + If( Convert.ToBoolean(Fields!AllDayEvent.Value) = True, {1}, String.Format({3}, {2}, 
					If( Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).Subtract( Convert.ToDateTime(Fields!Start.Value)).TotalSeconds > 0, 
					DataBinding.Parent.Fields!Date.Value, Fields!Start.Value), 
					If( Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1).Subtract( Convert.ToDateTime(Fields!End.Value)).TotalSeconds < 0, Convert.ToDateTime(DataBinding.Parent.Fields!Date.Value).AddDays(1), Fields!End.Value) )	)] ",
                "\"> \"",                                       // 0
                "\"\"",                                         // 1
                string.Format("{0}", "\"{0:t} {1:t}\""),        // 2
                Utils.GetStringCultureInfo(),                   // 3
                "\" - \""                                       // 4
                );

            rt.Width = "Auto";
            raApp.Children.Add(rt);

            rt = new RenderText();
            rt.Text = " [Fields!Subject.Value] ";
            rt.Width = "Auto";
            raApp.Children.Add(rt);

            rt = new RenderText();
            rt.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim startPeriod = Convert.ToDateTime(documentTag!StartTime.Value)
				Dim keyPeriod = Convert.ToDateTime(RenderObject.Original.DataBinding.Parent.Fields!Key.Value)
				Dim yesTopPanel = If(keyPeriod.Minute = 0, False, True)

				RenderObject.Visibility = If(yesTopPanel = False, VisibilityEnum.Visible, VisibilityEnum.Hidden)

				Dim location = Convert.ToString(RenderObject.Original.DataBinding.Parent.Fields!Location.Value)
				RenderObject.Visibility =  If(Not String.IsNullOrEmpty(location), VisibilityEnum.Visible, VisibilityEnum.Collapse)");

            rt.Text = "[\" (\" + System.Convert.ToString(Fields!Location.Value) + \")\"]";
            rt.Width = "Auto";
            raApp.Children.Add(rt);

            raDay.Children.Add(raApp);

            raDayBody.Children.Add(raDay);
            #endregion

            raPage.Children.Add(raDayBody);
            #endregion

            #region ** tasks
            // tasks
            RenderArea include = new RenderArea();
            include.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If (IncludeTasks = True OR IncludeBlankNotes = True OR IncludeLinedNotes = True) Then
					RenderObject.Visibility = VisibilityEnum.Visible
				Else
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If");

            include.Width = "25%";
            include.Height = "parent.Height - 28mm";

            RenderArea raTasks = new RenderArea();
            raTasks.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeTasks = False Then
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If

				RenderObject.Height = documentTag!TaskHeight.Value");

            raTasks.Style.Borders.All = LineDef.Default;
            raTasks.Style.Spacing.Top = "0.5mm";
            raTasks.Style.Spacing.Left = "0.5mm";
            raTasks.Width = "100%";

            rt = new RenderText();
            rt.Text = "Tasks";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raTasks.Children.Add(rt);
            include.Children.Add(raTasks);

            RenderArea raNotes = new RenderArea();
            raNotes.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeBlankNotes = False Then
					RenderObject.Visibility = VisibilityEnum.Collapse
					End If

					RenderObject.Height = Document.Tags!TaskHeight.Value"
                );

            raNotes.Style.Borders.All = LineDef.Default;
            raNotes.Style.Spacing.Top = "0.5mm";
            raNotes.Style.Spacing.Left = "0.5mm";
            raNotes.Width = "100%";

            rt = new RenderText();
            rt.Text = "Notes";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raNotes.Children.Add(rt);
            include.Children.Add(raNotes);

            raNotes = new RenderArea();
            raNotes.FormatDataBindingInstanceScript =
                string.Format(
                @"
				Dim documentTag = CType(Document.Tags, TagCollection)
				Dim IncludeTasks = Convert.ToBoolean(documentTag!IncludeTasks.Value )
				Dim IncludeBlankNotes = Convert.ToBoolean(documentTag!IncludeBlankNotes.Value)
				Dim IncludeLinedNotes = Convert.ToBoolean(documentTag!IncludeLinedNotes.Value)

				If IncludeLinedNotes = False Then
					RenderObject.Visibility = VisibilityEnum.Collapse
				End If

				RenderObject.Height = Document.Tags!TaskHeight.Value");

            raNotes.Style.Borders.All = LineDef.Default;
            raNotes.Style.Spacing.Top = "0.5mm";
            raNotes.Style.Spacing.Left = "0.5mm";
            raNotes.Width = "100%";

            rt = new RenderText();
            rt.Text = "Notes";
            rt.Style.Padding.Left = "2mm";
            rt.Style.Borders.Bottom = LineDef.Default;
            raNotes.Children.Add(rt);

            RenderTable lines = new RenderTable();
            lines.Rows.Insert(0, 1);
            lines.Rows[0].Height = "0.5cm";
            TableVectorGroup gr = lines.RowGroups[0, 1];
            gr.Header = TableHeaderEnum.None;
            List<int> lst = new List<int>(60);
            for (int i = 0; i < 60; i++)
            {
                lst.Add(i);
            }
            gr.DataBinding.DataSource = lst;
            lines.Style.GridLines.Horz = LineDef.Default;
            lines.RowSizingMode = TableSizingModeEnum.Fixed;

            raNotes.Children.Add(lines);

            include.Children.Add(raNotes);

            raPage.Children.Add(include);
            #endregion

            doc.Body.Children.Add(raPage);

            AddCommonTags(doc);

            Tag newTag = new Tag("Appointments", null, typeof(IList<Appointment>));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("WeekNumber", null, typeof(List<DateTime>));
            newTag.SerializeValue = false;
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("DayHours", null, typeof(Dictionary<DateTime, DateTime>));
            newTag.SerializeValue = false;
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("MonthCalendar", null, typeof(DateTime));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("StartDate", DateTime.Today, typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            doc.Tags.Add(newTag);
            newTag = new Tag("EndDate", DateTime.Today.AddDays(1), typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            doc.Tags.Add(newTag);

            newTag = new Tag("StartTime", DateTime.Today.AddHours(7), typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            ((TagDateTimeInputParams)newTag.InputParams).Format = DateTimePickerFormat.Time;
            doc.Tags.Add(newTag);
            newTag = new Tag("EndTime", DateTime.Today.AddHours(19), typeof(DateTime));
            newTag.InputParams = new TagDateTimeInputParams();
            ((TagDateTimeInputParams)newTag.InputParams).Format = DateTimePickerFormat.Time;
            doc.Tags.Add(newTag);

            newTag = new Tag("HidePrivateAppointments", false, typeof(bool));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("TaskHeight", new Unit("33.3%"), typeof(Unit));
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);
            newTag = new Tag("SlotAppointments", null, typeof(List<Appointment>));
            newTag.SerializeValue = false;
            newTag.ShowInDialog = false;
            doc.Tags.Add(newTag);

            newTag = new Tag("IncludeTasks", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Tasks";
            doc.Tags.Add(newTag);
            newTag = new Tag("IncludeBlankNotes", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Notes (blank)";
            doc.Tags.Add(newTag);
            newTag = new Tag("IncludeLinedNotes", false, typeof(bool));
            newTag.ShowInDialog = true;
            newTag.InputParams = new TagBoolInputParams();
            newTag.Description = "Include Notes (lined)";
            doc.Tags.Add(newTag);
        }

        /// <summary>
        /// Adds namespaces (common for all documents)
        /// </summary>
        /// <param name="doc"></param>
        private static void AddNamespaces(C1PrintDocument doc)
        {
            doc.TagEscapeString = "\x01";
            doc.ScriptingOptions.Namespaces.Add("C1.Schedule");
            doc.ScriptingOptions.Namespaces.Add("C1.Schedule.Printing");
        }

        /// <summary>
        /// Adds common tags
        /// </summary>
        /// <param name="doc"></param>
        private static void AddCommonTags(C1PrintDocument doc)
        {
            // CalendarInfo object contains all calendar-cspecific information
            // (culture, working days, week start, etc..)
            Tag newTag = new Tag("CalendarInfo", null, typeof(CalendarInfo));
            newTag.ShowInDialog = false;
            newTag.SerializeValue = false;
            doc.Tags.Add(newTag);
        }
    }
}