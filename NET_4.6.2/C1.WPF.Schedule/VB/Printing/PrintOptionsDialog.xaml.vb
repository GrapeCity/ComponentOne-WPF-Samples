Imports System.Text

Namespace Printing
	''' <summary>
	''' Allows selecting printing style from the list of available styles and setting other printing options
	''' </summary>
	Partial Public Class PrintOptionsDialog
		Inherits Window
		#Region "** fields"
		' reference to the owning PrintInfo object
		Private _info As PrintInfo = Nothing
		' printing context
		Private _context As PrintContextType = PrintContextType.DateRange
		' initial value of the HidePrivateAppointments property
		Private _hidePrivateAppointmentsInitial As Boolean = False
		#End Region

		#Region "** ctor"
		''' <summary>
		''' Initializes a new instance of the <see cref="PrintOptionsForm"/> form.
		''' </summary>
		Public Sub New()
			InitializeComponent()
		End Sub

		''' <summary>
		''' Initializes a new instance of the <see cref="PrintOptionsForm"/> form.
		''' </summary>
		''' <param name="info">The <see cref="PrintInfo"/> object.</param>
		''' <param name="defaultStyle">The <see cref="PrintStyle"/> object which should be selected initially.</param>
		''' <param name="printContext">The <see cref="PrintContextType"/> value, determining the filter of styles which should be shown in a form.</param>
		Public Sub New(ByVal info As PrintInfo, ByVal defaultStyle As PrintStyle, ByVal printContext As PrintContextType)
			InitializeComponent()
			_context = printContext
			_info = info
			For Each ps As PrintStyle In _info.PrintStyles
				If (_context And ps.Context) <> 0 Then
					lstStyles.Items.Add(ps)
				End If
			Next ps
			If defaultStyle IsNot Nothing AndAlso lstStyles.Items.Count > 0 Then
				lstStyles.SelectedItem = defaultStyle
			End If
			HidePrivateAppointments = _info.HidePrivateAppointments
			_hidePrivateAppointmentsInitial = HidePrivateAppointments
		End Sub
		#End Region

		#Region "** public interface"
		''' <summary>
		''' Returns the currently selected <see cref="PrintStyle"/> object.
		''' </summary>
		Public ReadOnly Property SelectedStyle() As PrintStyle
			Get
				Return CType(lstStyles.SelectedItem, PrintStyle)
			End Get
		End Property
		''' <summary>
		''' Gets or sets the <see cref="DateTime"/> value determining beginning of the printing range.
		''' </summary>
		''' <remarks>This property does make sense only for styles with PrintContextType.DateRange.</remarks>
		Public Property StartDate() As Date
			Get
				If dtpStart.DateTime.HasValue Then
					Return dtpStart.DateTime.Value.Date
				End If
				Return Date.Today
			End Get
			Set(ByVal value As Date)
				dtpStart.DateTime = value
			End Set
		End Property
		''' <summary>
		''' Gets or sets the <see cref="DateTime"/> value determining the end of the printing range.
		''' </summary>
		''' <remarks>This property does make sense only for styles with PrintContextType.DateRange.</remarks>
		Public Property EndDate() As Date
			Get
				If dtpEnd.DateTime.HasValue Then
					Return dtpEnd.DateTime.Value.Date
				End If
				Return Date.Today
			End Get
			Set(ByVal value As Date)
				dtpEnd.DateTime = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets a <see cref="Boolean"/> value determining whether control should
		''' hide details of private appointments in the resulting document.
		''' </summary>
		Public Property HidePrivateAppointments() As Boolean
			Get
				Return chkHidePrivate.IsChecked.Value
			End Get
			Set(ByVal value As Boolean)
				chkHidePrivate.IsChecked = value
			End Set
		End Property

		#End Region

		#Region "** event handlers"
		' hide date range controls if currently selected style has PrintContextType.Appointment context
		Private Sub lstStyles_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
			If (SelectedStyle.Context And PrintContextType.DateRange) = 0 Then
				grpPrintRange.Visibility = Visibility.Collapsed
			Else
				grpPrintRange.Visibility = Visibility.Visible
			End If
		End Sub

		' preview currently selected style
		Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If Not CheckTimes() Then
				Return
			End If
			_info.HidePrivateAppointments = HidePrivateAppointments
			_info.Preview(SelectedStyle, StartDate, EndDate)
		End Sub

		' set HidePrivateAppointments property to the initial value
		Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			HidePrivateAppointments = _hidePrivateAppointmentsInitial
		End Sub

		' validate date range
		Private Sub btnOk_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If Not CheckTimes() Then
				DialogResult = False
			Else
				DialogResult = True
			End If
		End Sub
		#End Region

		#Region "** private stuff"
		Private Function CheckTimes() As Boolean
			If EndDate < StartDate Then
                System.Windows.MessageBox.Show("The end date you entered occurs before the start date.", "Printing", MessageBoxButton.OK, MessageBoxImage.Exclamation)
				Return False
			End If
			Return True
		End Function
		#End Region
	End Class
End Namespace
