Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports C1.C1Preview
Imports C1.Schedule
Imports C1.WPF.Schedule

Namespace Printing
	''' <summary>
	''' The <see cref="PrintStyle"/> class represents the single printing style for a schedule control.
	''' </summary>
	Public Class PrintStyle
		#Region "** fields"
		Private _owner As PrintStyleCollection = Nothing

		Private _description As String = String.Empty
		Private _styleSource As String = String.Empty
		#End Region

		#Region "** ctor"
		''' <summary>
		''' Initializes the new <see cref="PrintStyle"/> object.
		''' </summary>
		Public Sub New()
			StyleName = String.Empty
			DocumentFormat = C1DocumentFormatEnum.C1d
			Context = PrintContextType.DateRange
		End Sub
		#End Region

		#Region "** public properties"
		''' <summary>
		''' Gets or sets the <see cref="String"/> value, determining the unique style name for using in
		''' <see cref="PrintStyleCollection"/>. 
		''' </summary>
        <DefaultValue("")> _
  Public Property StyleName() As String

            ''' <summary>
            ''' Gets or sets the <see cref="String"/> value, determining style description.
            ''' </summary>
            Get
                Return m_StyleName
            End Get
            Set(ByVal value As String)
                m_StyleName = value
            End Set
        End Property
        Private m_StyleName As String
        <DefaultValue("")> _
  Public Property Description() As String
            Get
                If String.IsNullOrEmpty(_description) Then
                    _description = StyleName
                End If
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

		''' <summary>
		''' Gets an <see cref="int"/> value determining the format of source document.
		''' Returns 0 for .c1d and 1 for .c1dx documents.
		''' </summary>
		Private privateDocumentFormat As C1DocumentFormatEnum
		Public Property DocumentFormat() As C1DocumentFormatEnum
			Get
				Return privateDocumentFormat
			End Get
			Private Set(ByVal value As C1DocumentFormatEnum)
				privateDocumentFormat = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets a <see cref="PrintContextType"/> value, specifying whether the current
		''' <see cref="PrintStyle"/> objects displays the single appointment or appointments
		''' of the specified date range.
		''' </summary>
        <DefaultValue(PrintContextType.DateRange)> _
  Public Property Context() As PrintContextType

            ''' <summary>
            ''' Gets or sets the <see cref="String"/> value determining the source of
            ''' C1PrintDocument template. It might be the name of .c1d or .c1dx file or the name
            ''' of resource.
            ''' </summary>
            Get
                Return m_Context
            End Get
            Set(ByVal value As PrintContextType)
                m_Context = value
            End Set
        End Property
        Private m_Context As PrintContextType
        <DefaultValue("")> _
  Public Property StyleSource() As String
            Get
                Return _styleSource
            End Get
            Set(ByVal value As String)
                _styleSource = value
                If _styleSource.EndsWith(".c1dx", StringComparison.OrdinalIgnoreCase) Then
                    DocumentFormat = C1DocumentFormatEnum.C1dx
                Else
                    DocumentFormat = C1DocumentFormatEnum.C1d
                End If
            End Set
        End Property

		''' <summary>
		''' Gets or sets the <see cref="Uri"/> of the current <see cref="PrintStyle"/> preview image.
		''' </summary>
        Public Property PreviewImage() As BitmapImage
            Get
                Return m_PreviewImage
            End Get
            Set(ByVal value As BitmapImage)
                m_PreviewImage = value
            End Set
        End Property
        Private m_PreviewImage As BitmapImage
#End Region

		#Region "** public methods"
		''' <summary>
		''' Loads style definition to the specified C1PrintDocument control.
		''' </summary>
		''' <param name="printDoc">The <see cref="PrintDocumentWrapper"/> object.</param>
		''' <returns>Returns true at successful loading; false - otherwise.</returns>
		Public Function Load(ByVal printDoc As C1PrintDocument) As Boolean
			If String.IsNullOrEmpty(StyleSource) Then
				Return False
			End If
			printDoc.Clear()

			' load document
			Dim sr As Stream = ResourceLoader.GetStream(StyleSource)
			If sr IsNot Nothing Then
				printDoc.Load(sr, DocumentFormat)
				sr.Dispose()
			Else
				printDoc.Load(StyleSource, DocumentFormat)
			End If

			If String.IsNullOrEmpty(StyleName) Then
				' fill name and description from document
				StyleName = CStr(printDoc.DocumentInfo.Title)
				If String.IsNullOrEmpty(StyleName) Then
					StyleName = _owner.GetUniqueStyleName()
				End If
			End If
			If String.IsNullOrEmpty(_description) Then
				_description = CStr(printDoc.DocumentInfo.Subject)
			End If

			Return True
		End Function
		#End Region

		#Region "private stuff"
		Friend Property Owner() As PrintStyleCollection
			Get
				Return _owner
			End Get
			Set(ByVal value As PrintStyleCollection)
				_owner = value
			End Set
		End Property

		' Set date range tags in a separate method, as it might
		' be useful to set them before editing style in print setup forms.
		Friend Sub SetDateRangeTags(ByVal printDoc As C1PrintDocument, ByVal start As Date, ByVal [end] As Date)
			Dim tag As Tag = Nothing
			Dim Tags As TagCollection = printDoc.Tags
			If (Context And PrintContextType.DateRange) <> 0 Then
				If Tags.IndexOfName("StartDate") >= 0 Then
					tag = Tags("StartDate")
					If tag IsNot Nothing AndAlso tag.Type Is GetType(Date) Then
						tag.Value = start
					End If
				End If
				If Tags.IndexOfName("EndDate") >= 0 Then
					tag = Tags("EndDate")
					If tag IsNot Nothing AndAlso tag.Type Is GetType(Date) Then
						tag.Value = [end]
					End If
				End If
			End If
		End Sub

		' Use this method to set all tags at document starting.
		' Note: start and end parameters are just default values. 
		' If document contains StartDate and EndDate tags, 
		' actual start and end values go from the tags.
		Friend Sub SetupTags(ByVal printDoc As C1PrintDocument, ByVal appointmentCollection As AppointmentCollection, ByVal appointmentList As List(Of Appointment), ByVal start As Date, ByVal [end] As Date, ByVal hidePrivateAppointments As Boolean, ByVal calendarInfo As CalendarInfo)
			Dim tag As Tag = Nothing
			Dim Tags As TagCollection = printDoc.Tags
			If Tags.IndexOfName("Appointment") >= 0 Then
				tag = Tags("Appointment")
				If tag IsNot Nothing AndAlso tag.Type Is GetType(Appointment) AndAlso appointmentList IsNot Nothing AndAlso appointmentList.Count > 0 Then
					tag.Value = appointmentList(0)
				End If
			End If
			If Tags.IndexOfName("Appointments") >= 0 Then
				tag = Tags("Appointments")
				If tag IsNot Nothing Then
					If tag.Type Is GetType(AppointmentCollection) Then
						tag.Value = appointmentCollection
					ElseIf tag.Type Is GetType(IList(Of Appointment)) Then
						If (Context And PrintContextType.Appointment) <> 0 AndAlso appointmentList IsNot Nothing Then
							appointmentList.Sort(AppointmentComparer.Default)
							tag.Value = appointmentList
						Else
							Dim tag1 As Tag = Nothing
							If Tags.IndexOfName("StartDate") >= 0 Then
								tag1 = Tags("StartDate")
								If tag1 IsNot Nothing AndAlso tag1.Type Is GetType(Date) Then
									start = CDate(tag1.Value)
								End If
							End If
							If Tags.IndexOfName("EndDate") >= 0 Then
								tag1 = Tags("EndDate")
								If tag1 IsNot Nothing AndAlso tag1.Type Is GetType(Date) Then
									[end] = CDate(tag1.Value)
								End If
							End If
                            Dim scheduler As C1Scheduler = CType(appointmentCollection.ParentStorage.ScheduleStorage, C1.WPF.Schedule.C1ScheduleStorage).Scheduler
                            ' get appointments for the currently selected SchedulerGroupItem if any, 
                            ' or all appointments otherwise.
                            Dim list As AppointmentList = appointmentCollection.GetOccurrences(If(scheduler.SelectedGroupItem Is Nothing, Nothing, scheduler.SelectedGroupItem.Owner), scheduler.GroupBy, start, [end].AddDays(1), (Not hidePrivateAppointments))
							list.Sort()
							tag.Value = list
						End If
					End If
				End If
			End If
			If Tags.IndexOfName("CalendarInfo") >= 0 Then
				tag = Tags("CalendarInfo")
				If tag IsNot Nothing AndAlso tag.Type Is GetType(CalendarInfo) Then
					tag.Value = calendarInfo
				End If
			End If
			If Tags.IndexOfName("HidePrivateAppointments") >= 0 Then
				tag = Tags("HidePrivateAppointments")
				If tag IsNot Nothing AndAlso tag.Type Is GetType(Boolean) Then
					tag.Value = hidePrivateAppointments
				End If
			End If

			' update string tags
			For Each t As Tag In Tags
				If t.Type Is GetType(String) Then
					Dim key As String = t.Name.ToLower()
					Select Case key
						Case "start"
							key = "startTime"
						Case "end"
							key = "endTime"
						Case "showtimeas"
							key = "showTimeAs"
						Case "contacts"
							key = "contactsButton"
						Case "resources"
							key = "resourcesButton"
						Case "categories"
							key = "categoriesButton"
					End Select
					If Not String.IsNullOrEmpty(key) Then
						' try find localized value from the Scheduler resources
                        Dim str As String = C1.WPF.Localization.C1Localizer.GetString("EditAppointment", key, CStr(t.Value)).Trim()
						If Not String.IsNullOrEmpty(str) Then
							While (str.EndsWith("."))
								str = str.Substring(0, str.Length - 1)
							End While
							If Not str.EndsWith(":") Then
								str += ":"
							End If
							t.Value = str
						End If
					End If
				End If
			Next

		End Sub
		#End Region
	End Class

	''' <summary>
	''' The <see cref="PrintStyleCollection"/> class represents the keyed collection of scheduler printing styles.
	''' The <see cref="PrintStyle.StyleName"/> property is used as a collection key.
	''' </summary>
	Public Class PrintStyleCollection
		Inherits KeyedCollection(Of String, PrintStyle)
		#Region "** fields"
		Private Shared _defaultStyleName As String = "Unknown Style"
		#End Region

		#Region "** public interface"
		''' <summary>
		''' Fills collection with default printing styles.
		''' </summary>
		Public Sub LoadDefaults()
			Clear()

			Dim st As New PrintStyle()
			st.StyleName = "Daily"
			st.Description = "Daily Style"
			st.StyleSource = "day.c1d" ' the name of C1Schedule resource containing the style
			st.Owner = Me
			Dim bi As New BitmapImage()
			bi.BeginInit()
			bi.UriSource = New Uri("/Images/daily.png", UriKind.RelativeOrAbsolute)
			bi.EndInit()
			st.PreviewImage = bi
			Add(st)

			st = New PrintStyle()
			st.StyleName = "Week"
			st.Description = "Weekly Style"
			st.StyleSource = "week.c1d"
			st.Owner = Me
			bi = New BitmapImage()
			bi.BeginInit()
			bi.UriSource = New Uri("/Images/week.png", UriKind.RelativeOrAbsolute)
			bi.EndInit()
			st.PreviewImage = bi
			Add(st)

			st = New PrintStyle()
			st.StyleName = "Month"
			st.Description = "Monthly Style"
			st.StyleSource = "month.c1d"
			st.Owner = Me
			bi = New BitmapImage()
			bi.BeginInit()
			bi.UriSource = New Uri("/Images/month.png", UriKind.RelativeOrAbsolute)
			bi.EndInit()
			st.PreviewImage = bi
			Add(st)

			st = New PrintStyle()
			st.StyleName = "Details"
			st.Description = "Details Style"
			st.StyleSource = "details.c1d"
			st.Owner = Me
			bi = New BitmapImage()
			bi.BeginInit()
			bi.UriSource = New Uri("/Images/details.png", UriKind.RelativeOrAbsolute)
			bi.EndInit()
			st.PreviewImage = bi
			Add(st)

			st = New PrintStyle()
			st.StyleName = "Memo"
			st.Context = PrintContextType.Appointment
			st.Description = "Memo Style"
			st.StyleSource = "memo.c1d"
			st.Owner = Me
			bi = New BitmapImage()
			bi.BeginInit()
			bi.UriSource = New Uri("/Images/memo.png", UriKind.RelativeOrAbsolute)
			bi.EndInit()
			st.PreviewImage = bi
			Add(st)
		End Sub

		''' <summary>
		''' Adds a set of <see cref="PrintStyle"/> objects to the collection.
		''' </summary>
		''' <param name="items">An array of type <see cref="PrintStyle"/>
		''' that contains the print styles to add. </param>
		Public Sub AddRange(ByVal items() As PrintStyle)
			Clear()
			For Each item As PrintStyle In items
				Add(item)
			Next item
		End Sub

		''' <summary>
		''' Returns the <see cref="String"/> value which can be used
		''' as unique style name.
		''' </summary>
		Public Function GetUniqueStyleName() As String
			Dim name As String = _defaultStyleName
			Dim index As Integer = 0
			Do While Contains(name)
				index += 1
				name = _defaultStyleName & " " & index.ToString()
			Loop
			Return name
		End Function
		#End Region

		#Region "** private stuff"
		''' <summary>
		''' Extracts the key from the specified item.
		''' </summary>
		''' <param name="item">The <see cref="PrintStyle"/> object from which to extract the key.</param>
		''' <returns>The key for the specified item.</returns>
		Protected Overrides Function GetKeyForItem(ByVal item As PrintStyle) As String
			Return item.StyleName
		End Function

		''' <summary>
		''' Inserts the specified item in the collection at the specified index.
		''' </summary>
		''' <param name="index">The zero-based index where the item is to be inserted.</param>
		''' <param name="item">The item to insert in the collection.</param>
		Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As PrintStyle)
			item.Owner = Me
			MyBase.InsertItem(index, item)
		End Sub
		#End Region
	End Class
End Namespace