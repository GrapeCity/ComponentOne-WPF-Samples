## Printing
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.Schedule/VB/Printing)
____
#### Demonstrates printing C1Scheduler's views via the C1.C1Preview.C1PrintDocument component.
____
The sample is based on using C1.C1Preview.C1PrintDocument component and System.Windows.Controls.DocumentViewer.
The sample uses C1PrintDocument templates included into C1Schedule assembly resources. 

By default, when user click "Print" button, he gets Print Options dialog, where he can select 
the style for printing. Furthermore, he can edit some document properties in a Tags input form.

All printing options, the set of printing styles and printing styles properties are defined as a part of
the Printing.PrintInfo class.
  
Sample classes:
  
public enum PrintContextType
  Defines the printing context for the PrintStyle object.
	
public class PrintPreviewWindow 
  Shows the DocumentViewer control.	

public class PrintInfo  
  The object used to manage schedule printing.	

public class PrintStyle 
  Represents the single printing style for a schedule control.

public class PrintStyleCollection 
  Represents the keyed collection of scheduler printing styles. 
  The PrintStyle.StyleName property is used as a collection key.

public class PrintOptionsDialog  
  Allows selecting printing style and setting other printing options.


The sample uses default C1PrintDocument form for editing document tags. You can create own Print Style options 
form for editing tags according your need. Analyze tag properties to determine tag value type, label and 
other input options to use in Print Style options form.

Tags used in default templates:


* Page header tags:
	  HeaderLeft, HeaderCenter, HeaderRight - string values used for printing in a page header;
	  HeaderColor - System.Windows.Media.Color value used for printing a page header;
	  HeaderFont - System.Drawing.Font used for printing a page header.



* Page footer tags:
	  FooterLeft, FooterCenter, FooterRight - string values used for printing in a page footer;
	  FooterColor - System.Windows.Media.Color value used for printing a page footer;
	  FooterFont - System.Drawing.Font used for printing a page footer.

	

* ReverseOnEvenPages - a boolean value determining whether to reverse page footers and headers on even pages;


* DateHeadingsFont - System.Drawing.Font use for printing date headings;
  

* AppointmentsFont - System.Drawing.Font use for printing appointments;


* CalendarInfo - C1.C1Schedule.CalendarInfo object keeping calendar-related information;


* Appointments - appointments to print. This tag may be type of C1.C1Schedule.AppointmentCollection or generic List of C1.C1Schedule.Appointment objects;
	

* Appointment - C1.C1Schedule.Appointment object to print;


* StartDate - System.DateTime value determining the beginning of the print range;


* EndDate - System.DateTime value determining the end of the print range;


* StartTime - System.DateTime value determining the beginning of the working day (used in a daily style);


* EndTime - System.DateTime value determining the end of the working day (used in a daily style);


* IncludeTasks - a boolean value determining whether resulting document should contain place for tasks;
  

* IncludeBlankNotes - a boolean value determining whether resulting document should contain blank place for notes;
  

* IncludeLinedNotes - a boolean value determining whether resulting document should contain lined place for notes;
  

* WorkDaysOnly - a boolean value determining whether weekends should be omitted at printing month view style;


* HidePrivateAppointments - a boolean value determining whether private appointments should be hidden in resulting document;


* InsertPageBreaks - a boolean value determining whether page breaks should be inserter in details style;
  

* PageBreak - determines whether page break should be inserted every Day, Week or Month in details style.


You can use the sample code as it is or edit it in any convenient way.	
