using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Printing
{
	/// <summary>
	/// Allows selecting printing style from the list of available styles and setting other printing options
	/// </summary>
	public partial class PrintOptionsDialog : Window
	{
		#region ** fields
		// reference to the owning PrintInfo object
		private PrintInfo _info = null;
		// printing context
		private PrintContextType _context = PrintContextType.DateRange;
		// initial value of the HidePrivateAppointments property
		private bool _hidePrivateAppointmentsInitial = false;
		#endregion

		#region ** ctor
		/// <summary>
		/// Initializes a new instance of the <see cref="PrintOptionsForm"/> form.
		/// </summary>
		public PrintOptionsDialog()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="PrintOptionsForm"/> form.
		/// </summary>
		/// <param name="info">The <see cref="PrintInfo"/> object.</param>
		/// <param name="defaultStyle">The <see cref="PrintStyle"/> object which should be selected initially.</param>
		/// <param name="printContext">The <see cref="PrintContextType"/> value, determining the filter of styles which should be shown in a form.</param>
		public PrintOptionsDialog(PrintInfo info, PrintStyle defaultStyle, PrintContextType printContext)
		{
			InitializeComponent();
			_context = printContext;
			_info = info;
			foreach (PrintStyle ps in _info.PrintStyles)
			{
				if ((_context & ps.Context) != 0)
				{
					lstStyles.Items.Add(ps);
				}
			}
			if (defaultStyle != null && lstStyles.Items.Count > 0)
			{
				lstStyles.SelectedItem = defaultStyle;
			}
			_hidePrivateAppointmentsInitial = HidePrivateAppointments = _info.HidePrivateAppointments;
		}
		#endregion

		#region ** public interface
		/// <summary>
		/// Returns the currently selected <see cref="PrintStyle"/> object.
		/// </summary>
		public PrintStyle SelectedStyle
		{
			get
			{
				return (PrintStyle)lstStyles.SelectedItem;
			}
		}
		/// <summary>
		/// Gets or sets the <see cref="DateTime"/> value determining beginning of the printing range.
		/// </summary>
		/// <remarks>This property does make sense only for styles with PrintContextType.DateRange.</remarks>
		public DateTime StartDate
		{
			get
			{
				if (dtpStart.DateTime.HasValue)
				{
					return dtpStart.DateTime.Value.Date;
				}
				return DateTime.Today;
			}
			set
			{
				dtpStart.DateTime = value;
			}
		}
		/// <summary>
		/// Gets or sets the <see cref="DateTime"/> value determining the end of the printing range.
		/// </summary>
		/// <remarks>This property does make sense only for styles with PrintContextType.DateRange.</remarks>
		public DateTime EndDate
		{
			get
			{
				if (dtpEnd.DateTime.HasValue)
				{
					return dtpEnd.DateTime.Value.Date;
				}
				return DateTime.Today;
			}
			set
			{
				dtpEnd.DateTime = value;
			}
		}

		/// <summary>
		/// Gets or sets a <see cref="Boolean"/> value determining whether control should
		/// hide details of private appointments in the resulting document.
		/// </summary>
		public bool HidePrivateAppointments
		{
			get
			{
				return chkHidePrivate.IsChecked.Value;
			}
			set
			{
				chkHidePrivate.IsChecked = value;
			}
		}

		#endregion

		#region ** event handlers
		// hide date range controls if currently selected style has PrintContextType.Appointment context
		private void lstStyles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((SelectedStyle.Context & PrintContextType.DateRange) == 0)
			{
				grpPrintRange.Visibility = Visibility.Collapsed;
			}
			else
			{
				grpPrintRange.Visibility = Visibility.Visible;
			}
		}

		// preview currently selected style
		private void btnPreview_Click(object sender, RoutedEventArgs e)
		{
			if (!CheckTimes())
			{
				return;
			}
			_info.HidePrivateAppointments = HidePrivateAppointments;
			_info.Preview(SelectedStyle, StartDate, EndDate);
		}

		// set HidePrivateAppointments property to the initial value
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			HidePrivateAppointments = _hidePrivateAppointmentsInitial;
		}

		// validate date range
		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			if (!CheckTimes())
			{
				DialogResult = false;
			}
			else
			{
				DialogResult = true;
			}
		}
		#endregion

		#region ** private stuff
		private bool CheckTimes()
		{
			if (EndDate < StartDate)
			{
				MessageBox.Show("The end date you entered occurs before the start date.",
					"Printing", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return false;
			}
			return true;
		}
		#endregion
	}
}
