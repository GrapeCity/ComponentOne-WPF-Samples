Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Reflection
Imports C1.C1Preview
Imports C1.Schedule
Imports C1.WPF.Schedule

Namespace Printing
    ''' <summary>
    ''' The <see cref="PrintContextType"/> defines the printing context for the <see cref="PrintStyle"/> object.
    ''' </summary>
    <Flags()>
    Public Enum PrintContextType
        ''' <summary>
        ''' Document displays content of the <see cref="C1.Schedule.Appointment"/> object.
        ''' </summary>
        Appointment = &H1
        ''' <summary>
        ''' Document displayes appointments for the specified date range.
        ''' </summary>
        DateRange = &H2
    End Enum

	''' <summary>
	''' The object used to manage schedule printing.
	''' </summary>
	Public Class PrintInfo
		#Region "** fields"
		Private _schedule As C1Scheduler
		Private _preview As PrintPreviewWindow = Nothing
		Private _currentAppointments As List(Of Appointment) = Nothing
		#End Region

		#Region "** ctor"
		''' <summary>
		''' Initializes a ne instance of the <see cref="PrintInfo"/> class.
		''' </summary>
		''' <param name="schedule"></param>
		Public Sub New(ByVal schedule As C1Scheduler)
			HidePrivateAppointments = False
			_schedule = schedule
			PrintDocument = New C1PrintDocument()
			PrintStyles = New PrintStyleCollection()
			PrintStyles.LoadDefaults()
			AddHandler PrintDocument.DocumentStarting, AddressOf _printDoc_DocumentStarting
		End Sub
		#End Region

		#Region "** public properties"
		''' <summary>
		''' Gets the <see cref="PrintStyleCollection"/> collection, containing all available styles of printing.
		''' </summary>
		Private privatePrintStyles As PrintStyleCollection
		Public Property PrintStyles() As PrintStyleCollection
			Get
				Return privatePrintStyles
			End Get
			Private Set(ByVal value As PrintStyleCollection)
				privatePrintStyles = value
			End Set
		End Property

		''' <summary>
		''' Gets the <see cref="C1PrintDocument"/> object.
		''' </summary>
		Private privatePrintDocument As C1PrintDocument
		Public Property PrintDocument() As C1PrintDocument
			Get
				Return privatePrintDocument
			End Get
			Private Set(ByVal value As C1PrintDocument)
				privatePrintDocument = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the <see cref="PrintPreviewWindow"/> object, used for previewing. 
		''' </summary>
		Public Property PrintPreviewDialog() As PrintPreviewWindow
			Get
				If _preview Is Nothing Then
					_preview = New PrintPreviewWindow()
				End If
				Return _preview
			End Get
			Set(ByVal value As PrintPreviewWindow)
				_preview = value
			End Set
		End Property

		''' <summary>
		''' Gets a <see cref="PrintStyle"/> object which is currently selected for printing.
		''' </summary>
		Private privateCurrentStyle As PrintStyle
        <Browsable(False)> _
  Public Property CurrentStyle() As PrintStyle
            Get
                Return privateCurrentStyle
            End Get
            Friend Set(ByVal value As PrintStyle)
                privateCurrentStyle = value
            End Set
        End Property

		''' <summary>
		''' Gets or sets a <see cref="Boolean"/> value determining whether control should
		''' hide details of private appointments in resulting document.
		''' </summary>
        <DefaultValue(False)> _
  Public Property HidePrivateAppointments() As Boolean
            Get
                Return m_HidePrivateAppointments
            End Get
            Set(ByVal value As Boolean)
                m_HidePrivateAppointments = Value
            End Set
        End Property
        Private m_HidePrivateAppointments As Boolean

#End Region

		#Region "** public methods"

		''' <summary>
		''' Sends default printing style for the currently selected view to the printer.
		''' </summary>
		Public Sub Print()
			If SetupPrintContext(Nothing) Then
				InternalPrint()
			End If
		End Sub

		''' <summary>
		''' Sends the specified printing style to the printer.
		''' </summary>
		''' <param name="style">The <see cref="PrintStyle"/> object to print.</param>
		Public Sub Print(ByVal style As PrintStyle)
			Print(style, _schedule.VisibleDates(0), _schedule.VisibleDates(_schedule.VisibleDates.Count - 1))
		End Sub

		''' <summary>
		''' Sends the specified <see cref="Appointment"/> object to the printer.
		''' </summary>
		''' <param name="appointment">The <see cref="Appointment"/> object to print.</param>
		Public Sub Print(ByVal appointment As Appointment)
			If SetupPrintContext(appointment) Then
				InternalPrint()
			End If
		End Sub

		''' <summary>
		''' Sends the specified printing style to the printer.
		''' </summary>
		''' <param name="style">The <see cref="PrintStyle"/> object to print.</param>
		''' <param name="start">The <see cref="DateTime"/> value specifying the beginning of the print range.</param>
		''' <param name="end">The <see cref="DateTime"/> value specifying the end of the print range.</param>
		Public Sub Print(ByVal style As PrintStyle, ByVal start As Date, ByVal [end] As Date)
			If style IsNot Nothing Then
				SetupPrintContext(style, start, [end])
				InternalPrint()
			End If
		End Sub

		''' <summary>
		''' Opens a separate application window in which end users can preview 
		''' the output that would be generated by the print operation.
		''' </summary>
		Public Sub Preview()
			If SetupPrintContext(Nothing) Then
				PreviewCurrentStyle()
			End If
		End Sub

		''' <summary>
		''' Opens a separate application window in which end users can preview 
		''' the output that would be generated by the print operation.
		''' </summary>
		''' <param name="style">The <see cref="PrintStyle"/> object to preview.</param>
		Public Sub Preview(ByVal style As PrintStyle)
			Preview(style, _schedule.VisibleDates(0), _schedule.VisibleDates(_schedule.VisibleDates.Count - 1))
		End Sub

		''' <summary>
		''' Opens a separate application window in which end users can preview 
		''' the output that would be generated by the print operation.
		''' </summary>
		''' <param name="appointment">The <see cref="Appointment"/> object to preview.</param>
		Public Sub Preview(ByVal appointment As Appointment)
			If SetupPrintContext(appointment) Then
				PreviewCurrentStyle()
			End If
		End Sub

		''' <summary>
		''' Opens a separate application window in which end users can preview 
		''' the output that would be generated by the print operation.
		''' </summary>
		''' <param name="style">The <see cref="PrintStyle"/> object to preview.</param>
		''' <param name="start">The <see cref="DateTime"/> value specifying the beginning of the print range.</param>
		''' <param name="end">The <see cref="DateTime"/> value specifying the end of the print range.</param>
		Public Sub Preview(ByVal style As PrintStyle, ByVal start As Date, ByVal [end] As Date)
			If style IsNot Nothing Then
				SetupPrintContext(style, start, [end])
				PreviewCurrentStyle()
			End If
		End Sub

		''' <summary>
		''' Loads style definition to C1PrintDocument control.
		''' </summary>
		''' <param name="style">The <see cref="PrintStyle"/> for loading into print document object.</param>
		Public Sub LoadStyle(ByVal style As PrintStyle)
			If style.Load(PrintDocument) Then
				CurrentStyle = style
				AddScheduleReferences()
			End If
		End Sub
		#End Region

		#Region "** private stuff"
		' adds references to additional assemblies needed for document generation
		Private Sub AddScheduleReferences()
			AddExternalAssembly(Assembly.GetAssembly(GetType(C1.WPF.Schedule.C1Scheduler)).Location)
            AddExternalAssembly(Assembly.GetAssembly(GetType(System.Windows.Media.Color)).Location)
            AddExternalAssembly(Assembly.GetAssembly(GetType(C1.Schedule.Appointment)).Location)
            AddExternalAssembly(Assembly.GetAssembly(GetType(C1.WPF.Schedule.C1ScheduleStorage)).Location)
            AddExternalAssembly("WindowsBase.dll")
            AddExternalAssembly("netstandard.dll")

            AddExternalAssembly(Assembly.GetAssembly(GetType(INotifyCollectionChanged)).Location)
        End Sub
		''' <summary>
		''' Adds specified external assembly reference to the currently loaded document.
		''' </summary>
		''' <param name="AssemblyName">The <see cref="String"/> value specifying assembly name for adding.</param>
		Private Sub AddExternalAssembly(ByVal assemblyName As String)
			Dim extAssemblies As StringCollection = PrintDocument.ScriptingOptions.ExternalAssemblies
			If extAssemblies IsNot Nothing AndAlso (Not extAssemblies.Contains(assemblyName)) Then
				extAssemblies.Add(assemblyName)
			End If
		End Sub

		' shows PrintOptionsDialog and prints selected style if user clicks Ok button
		Private Sub InternalPrint()
			' setup print context
			Dim context As PrintContextType = PrintContextType.DateRange
			If _schedule.SelectedAppointment IsNot Nothing Then
				context = context Or PrintContextType.Appointment
			End If
			' show PrintOptionsDialog
			Dim form As New PrintOptionsDialog(Me, CurrentStyle, context)
			form.StartDate = _schedule.VisibleDates(0)
			form.EndDate = _schedule.VisibleDates(_schedule.VisibleDates.Count - 1)
			Dim result? As Boolean = form.ShowDialog()
			If result.HasValue AndAlso result.Value Then
				' user clicks Ok
				' set PrintStyle properties according to user settings in a dialog
				CurrentStyle = form.SelectedStyle
				HidePrivateAppointments = form.HidePrivateAppointments
				SetupPrintContext(CurrentStyle, form.StartDate, form.EndDate)
				' print the document
				PrintDocument.Print(New System.Drawing.Printing.PrinterSettings())
				_currentAppointments = Nothing
			End If
		End Sub

		' initialize printing context for the specified appointment
		Private Function SetupPrintContext(ByVal appointment As Appointment) As Boolean
			If appointment IsNot Nothing Then
				_currentAppointments = New List(Of Appointment)()
				_currentAppointments.Add(appointment)
			ElseIf _schedule.SelectedAppointment IsNot Nothing Then
				_currentAppointments = New List(Of Appointment)()
				_currentAppointments.Add(_schedule.SelectedAppointment)
			End If
			If PrintStyles.Contains("Memo") AndAlso (_currentAppointments IsNot Nothing AndAlso _currentAppointments.Count > 0) Then
				CurrentStyle = PrintStyles("Memo")
			Else
				Dim daysNumber As Integer = _schedule.VisibleDates.Count
				If daysNumber > 14 AndAlso PrintStyles.Contains("Month") Then
					CurrentStyle = PrintStyles("Month")
				ElseIf daysNumber = 7 AndAlso PrintStyles.Contains("Week") Then
					CurrentStyle = PrintStyles("Week")
				ElseIf PrintStyles.Contains("Daily") Then
					CurrentStyle = PrintStyles("Daily")
				ElseIf PrintStyles.Contains("Details") Then
					CurrentStyle = PrintStyles("Details")
				Else
					CurrentStyle = PrintStyles(0)
				End If
			End If
			If CurrentStyle Is Nothing Then
				Return False
			End If
			SetupPrintContext(CurrentStyle, _schedule.VisibleDates(0), _schedule.VisibleDates(_schedule.VisibleDates.Count - 1))
			Return True
		End Function

		' on document starting show tag input form to the user and initialize document tags
		Private Sub _printDoc_DocumentStarting(ByVal sender As Object, ByVal e As EventArgs)
			Dim start As Date = _schedule.VisibleDates(0).Date
			Dim [end] As Date = _schedule.VisibleDates(_schedule.VisibleDates.Count - 1).Date
			Dim calendarInfo As CalendarInfo = _schedule.CalendarHelper.Info

			' Show tag input form to end-user.
			' This is a default form shown by the C1PrintDocument control. 
			' You can create your own form for this purpose, show it, and set C1PrintDocumentTags
			' according to the user's input.
			PrintDocument.EditTags()

			CurrentStyle.SetupTags(PrintDocument, _schedule.DataStorage.AppointmentStorage.Appointments, _currentAppointments, start, [end], HidePrivateAppointments, calendarInfo)
		End Sub

		' load specified PrintStyle into the PrintDocument control and initialize date range
		Friend Sub SetupPrintContext(ByVal style As PrintStyle, ByVal start As Date, ByVal [end] As Date)
			LoadStyle(style)
			style.SetDateRangeTags(PrintDocument, start, [end])
		End Sub

		''' <summary>
		''' Shows current style in a print preview dialog.
		''' </summary>
		Private Sub PreviewCurrentStyle()
			' generate the document
			If PrintPreviewDialog IsNot Nothing Then
				_preview.Document = PrintDocument.FixedDocumentSequence
				_preview.ShowDialog()
				_preview.Activate()
				_preview = Nothing
			End If
			_currentAppointments = Nothing
		End Sub
		#End Region
	End Class
End Namespace
