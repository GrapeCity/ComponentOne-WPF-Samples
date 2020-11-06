WYSIWYGPrinting
----------------------------------------
Demonstrates printing C1Scheduler's views via System.Windows.Controls.PrintDialog.PrintDocument method.

The C1SchedulerPaginator class implements System.Windows.Documents.DocumentPaginator abstract interface to provide paginated output.
The sample uses hidden C1Scheduler control instance included into application visual tree to print areas which are not currently visible in UI.

The C1SchedulerPaginator performs the next actions:

- copies all the settings of the currently displayed C1Scheduler view into the hidden C1Scheduler instance, 

- hides non-working time and adjusts VisualIntervalScale to get best possible fit,

- hides some visual elements such as current time indicator and navigation panels,

- performs pagination by changing C1Scheduler.VisibleDates collection and C1Scheduler.CalendarHelper.StartDayTime and C1Scheduler.CalendarHelper.EndDayTime properties,

- clips vertical and horizontal scrollbars so that they are not appear in the printed document,

- performs layout using selected paper size, 

- renders individual pages as images,

- adds page number and current date to the page header.
