using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using C1.C1Schedule;

namespace C1.WPF.Schedule
{
    /// <summary>
    /// Implements creation of multiple-page elements from the C1Scheduler control.
    /// </summary>
    public class C1SchedulerPaginator : DocumentPaginator
    {

        #region ** fields
        List<FrameworkElement> _pages = new List<FrameworkElement>(); // list of pages to print
        #endregion

        #region ** ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="C1SchedulerPaginator"/> class.
        /// </summary>
        /// <param name="schedule">The <see cref="C1Scheduler"/> control to print.</param>
        /// <param name="printSchedule">The hidden <see cref="C1Scheduler"/> control included into visual tree which can be used to create print pages.</param>
        /// <param name="pageSize">The page size to use.</param>
        /// <param name="margin">The page margins.</param>
        /// <param name="resolutionX">The horizontal printer resolution.</param>
        /// <param name="resolutionY">The vertical printer resolution.</param>
        public C1SchedulerPaginator(C1Scheduler schedule, C1Scheduler printSchedule, Size pageSize, Thickness margin, int resolutionX, int resolutionY)
        {
            PrintResolutionX = resolutionX;
            PrintResolutionY = resolutionY; 
            // calculate page parameters
            PageSize = pageSize;
            ContentSize = new Size(PageSize.Width - margin.Left - margin.Right, PageSize.Height - margin.Top - margin.Bottom);
            ContentLocation = new Point(margin.Left, margin.Top);

            // copy required settings from the C1Scheduler control into the hidden instance used for printing
            SetupPrintScheduler(schedule, printSchedule);

            // generate print pages for current view
            switch (printSchedule.ViewType)
            {
                case ViewType.Month:
                case ViewType.Custom:
                    // print current view as a single page, fit in page
                    // Notes:
                    // - if you use grouping, you might consider printing one group on page;
                    // - if you need to print arbitrary date range, you should split on pages 
                    //   by setting printSchedule.VisibleDates property and then using printSchedule.IncrementStartTimeLarge() for moving further;
                    // - if every day has a lot of appointments, you can split on pages to only print 2 or 3 weeks on page. 
                    GeneratePage(printSchedule, ContentSize.Width, ContentSize.Height, false, true); // only clip vertical scrollbar
                    break;
                case ViewType.TimeLine:
                    // print current view with paging, day-by-day
                    DateTime endDate = schedule.VisibleDates[schedule.VisibleDates.Count - 1];
                    VisualIntervalCollection dayIntervals = printSchedule.VisibleGroupItems[0].VisualIntervals;
                    if (dayIntervals.Count > 16)
                    {
                        // time line might not fit in page, so split 1 day to some pages
                        DateTime start = dayIntervals[0].StartTime;
                        DateTime end = dayIntervals[dayIntervals.Count - 1].EndTime;
                        DateTime middle = start.Add(TimeSpan.FromMilliseconds((end - start).TotalMilliseconds / 2));
                        while (printSchedule.VisibleDates[0] <= endDate)
                        {
                            // first half of day
                            printSchedule.BeginUpdate();
                            printSchedule.CalendarHelper.StartDayTime = start.TimeOfDay;
                            printSchedule.CalendarHelper.EndDayTime = middle.TimeOfDay;
                            printSchedule.EndUpdate();
                            GeneratePage(printSchedule, double.PositiveInfinity, ContentSize.Height, true, !string.IsNullOrEmpty(printSchedule.GroupBy)); // always clip horizontal scrollbar, clip vertical scrollbar if grouping is enabled
                            // second half of day
                            printSchedule.BeginUpdate();
                            printSchedule.CalendarHelper.StartDayTime = middle.TimeOfDay;
                            printSchedule.CalendarHelper.EndDayTime = end.TimeOfDay;
                            printSchedule.EndUpdate();
                            GeneratePage(printSchedule, double.PositiveInfinity, ContentSize.Height, true, !string.IsNullOrEmpty(printSchedule.GroupBy)); // always clip horizontal scrollbar, clip vertical scrollbar if grouping is enabled
                            if (printSchedule.VisibleDates[0] == printSchedule.End.Date)
                            {
                                // if we reached C1Scheduler.End date, IncrementStartTimeLarge won't change VisibleDates.
                                // So break here to avoid endless loop
                                break;
                            }
                            // move to next day
                            printSchedule.IncrementStartTimeLarge(); 
                        }
                    }
                    else
                    {
                        // 1 day - 1 page
                        while (printSchedule.VisibleDates[0] <= endDate)
                        {
                            GeneratePage(printSchedule, double.PositiveInfinity, ContentSize.Height, true, !string.IsNullOrEmpty(printSchedule.GroupBy)); // always clip horizontal scrollbar, clip vertical scrollbar if grouping is enabled
                            if (printSchedule.VisibleDates[0] == printSchedule.End.Date)
                            {
                                // if we reached C1Scheduler.End date, IncrementStartTimeLarge won't change VisibleDates.
                                // So break here to avoid endless loop
                                break;
                            }
                            // move to next day
                            printSchedule.IncrementStartTimeLarge(); 
                        }
                    }
                    break;
                default:
                    // Day, Working week, Week views. 
                    // If ShowWorkTimeOnly is true, in most cases the whole view will fit into page well enough, so don't break on pages.
                    // Print current view with scaling for page height
                    GeneratePage(printSchedule, ContentSize.Width, double.PositiveInfinity, false, true); // clip vertical scrollbar
                    break;
            }

            // done; release resources
            printSchedule.Clip = null;
            printSchedule.BeginUpdate();
            printSchedule.DataStorage.AppointmentStorage.Appointments.Clear();
            printSchedule.EndUpdate();
        }
        #endregion

        #region ** implementation
        /// <summary>
        /// Gets or sets size which is used to layout page content.
        /// </summary>
        public Size ContentSize
        {
            get; private set;
        }
        /// <summary>
        ///  Gets or sets content location.
        /// </summary>
        public Point ContentLocation
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets horizontal resolution which should be used for printing.
        /// </summary>
        public int PrintResolutionX
        {
            get; private set;
        }
        /// <summary>
        /// Gets or sets vertical resolution which should be used for printing.
        /// </summary>
        public int PrintResolutionY
        {
            get; private set;
        }

        /// <summary>
        /// Copies settings from the source to target control.
        /// </summary>
        /// <param name="source">The <see cref="C1Scheduler"/> control to copy settings from.</param>
        /// <param name="target">The hidden <see cref="C1Scheduler"/> control used for printing.</param>
        private void SetupPrintScheduler(C1Scheduler source, C1Scheduler target)
        {
            // call BeginUpdate to supress events and layout during initialization
            target.BeginUpdate();
            // copy theme information
            target.Theme = source.Theme;
            // you can use customized theme with simpler appearance and better print contrast
            // To do so:
            // - copy default xaml into custom ResourceDictionary;
            // - edit xaml according your needs (for example, use the same appearance for all time slots, more contrast colors, etc.);
            // - set target.Theme to your ResourceDictionary.

            // copy start and end dates
            target.Start = source.Start;
            target.End = source.End;

            // copy view information
            target.Style = source.Style;
            target.ViewType = source.ViewType;
            target.CurrentTimeBrush = null;
            // copy time scale information
            target.VisualIntervalScale = source.VisualIntervalScale;
            if (source.VisualIntervalScale.TotalMinutes < 20 && source.ViewType != ViewType.TimeLine)
            {
                // limit number of intervals in day view
                target.VisualIntervalScale = TimeSpan.FromMinutes(20);
            }
            // copy calendar parameters
            target.CalendarHelper.WeekStart = source.CalendarHelper.WeekStart;
            target.CalendarHelper.StartDayTime = source.CalendarHelper.StartDayTime;
            target.CalendarHelper.EndDayTime = source.CalendarHelper.EndDayTime;
            // if you use custom working days, holidays or weekend exceptions, copy them as well
            
            // grouping
            target.GroupBy = source.GroupBy;
            target.GroupPageSize = source.GroupPageSize;

            // hide free time
            target.ShowWorkTimeOnly = true;

            // copied the most of settings, call EndUpdate to allow layout
            target.EndUpdate();
            
            // load appointments
            using (MemoryStream stream = new MemoryStream())
            {
                // exporty appointments from the displayed Scheduler
                source.DataStorage.Export(stream, FileFormatEnum.XML);
                stream.Position = 0;
                // import appointments into hidden Scheduler
                target.DataStorage.Import(stream, FileFormatEnum.XML);
            }
            if (!string.IsNullOrEmpty(target.GroupBy))
            {
                // copy visible groups
                target.VisibleGroupItems.BeginUpdate();
                target.VisibleGroupItems.Clear();
                foreach (SchedulerGroupItem group in source.VisibleGroupItems)
                {
                    target.VisibleGroupItems.Add(target.GroupItems[source.GroupItems.IndexOf(group)]);
                }
                target.VisibleGroupItems.EndUpdate();
            }
            // set the same visible dates
            target.VisibleDates.BeginUpdate();
            target.VisibleDates.Clear();
            if (target.ViewType == ViewType.TimeLine)
            {
                // only single day fits into page, so print day-by-day
                target.VisibleDates.Add(source.VisibleDates[0]);
            }
            else
            {
                foreach (DateTime day in source.VisibleDates)
                {
                    target.VisibleDates.Add(day);
                }
            }
            target.VisibleDates.EndUpdate();
        }

        /// <summary>
        /// Generates single page image using specified size and clipping options.
        /// </summary>
        /// <param name="printSchedule">The <see cref="C1Scheduler"/> control to print.</param>
        /// <param name="contentWidth">The content width.</param>
        /// <param name="contentHeight">The content height.</param>
        /// <param name="clipHScrollBar">True to clip horizontal scrollbar area.</param>
        /// <param name="clipVScrollBar">True to clip vertical scrollbar area.</param>
        private void GeneratePage(C1Scheduler printSchedule, double contentWidth, double contentHeight, bool clipHScrollBar, bool clipVScrollBar)
        {
            // Measure control with specified size.
            printSchedule.Measure(new Size(contentWidth, contentHeight));
            // Update content size if it was undefined according to the control's desired size.
            if (double.IsPositiveInfinity(contentWidth))
            {
                contentWidth = Math.Max(printSchedule.DesiredSize.Width, ContentSize.Width);
            }
            if (double.IsPositiveInfinity(contentHeight))
            {
                contentHeight = Math.Max(printSchedule.DesiredSize.Height, ContentSize.Height);
            }
            // Set up clip to exclude scrollbars.
            printSchedule.Clip = new RectangleGeometry(new Rect(0, 0, contentWidth, contentHeight));
            // Enlarge size, so that scrollbars go out of printable area.
            if (clipVScrollBar)
            {
                contentWidth += SystemParameters.VerticalScrollBarWidth;
            }
            if (clipHScrollBar)
            {
                contentHeight += SystemParameters.HorizontalScrollBarHeight;
            }
            // Perform layout according to the desired page size. 
            Size contentSize = new Size(contentWidth, contentHeight);
            printSchedule.Measure(contentSize);
            printSchedule.Arrange(new Rect(contentSize));
            // In some C1Scheduler views not all bindings got updated during Arrange call, so force the whole visual tree update.
            printSchedule.InvalidateVisualTree();
            printSchedule.UpdateLayout();
            // Generate page image.
            _pages.Add(GetImageFromElement(printSchedule));
        }

        /// <summary>
        /// Generates image of the specified FrameworkElement using RenderTargetBitmap.
        /// </summary>
        /// <param name="e">The source <see cref="FrameworkElement"/>.</param>
        /// <returns>The <see cref="System.Windows.Controls.Image"/> control representing content of the single page.</returns>
        public System.Windows.Controls.Image GetImageFromElement(FrameworkElement e)
        {
            double scaleX = PrintResolutionX / 96;
            double scaleY = PrintResolutionY / 96;
            // render element to image which will be used for printing
            RenderTargetBitmap targetBitmap = 
              new RenderTargetBitmap((int)(e.ActualWidth * scaleX), (int)(e.ActualHeight * scaleY), 96 * scaleX, 96 * scaleY, System.Windows.Media.PixelFormats.Pbgra32);
            
            targetBitmap.Render(e);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(stream.ToArray());
                bmp.EndInit();
                return new System.Windows.Controls.Image()
                {
                    Source = bmp
                };
            }
        }
        #endregion

        #region ** DocumentPaginator overrides

        /// <summary>
        ///  Gets the System.Windows.Documents.DocumentPage  for the specified page number.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns>The <see cref="System.Windows.Documents.DocumentPage"/> object.</returns>
        public override DocumentPage GetPage(int pageNumber)
        {
            if (pageNumber < 0)
            {
                throw new ArgumentOutOfRangeException("Page number can't be negative.");
            }
            if (pageNumber > _pages.Count - 1)
            {
                return DocumentPage.Missing;
            }
            var page = _pages[pageNumber];

            // add page title (page number and DateTime stamp)
            TextBlock title = new TextBlock();
            title.FontFamily = new FontFamily("Courier New");
            title.Text = string.Format("Page {0} of {1}", pageNumber + 1, _pages.Count);

            TextBlock date = new TextBlock();
            date.FontFamily = new FontFamily("Courier New");
            date.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
            date.HorizontalAlignment = HorizontalAlignment.Right;

            double y = Math.Max(20, ContentLocation.Y - 30);
            page.Margin = new Thickness(0, ContentLocation.Y - y, 0, 0);
            Point location = new Point(ContentLocation.X, y);
            Size size = new Size(ContentSize.Width, ContentSize.Height + y);

            // perform page layout
            var container = new Grid();
            container.Children.Add(page);
            container.Children.Add(title);
            container.Children.Add(date);
            container.Measure(size);
            container.Arrange(new Rect(location, size));

            // return DocumentPage
            return new DocumentPage(container, PageSize, new Rect(location.X, y, ContentSize.Width, ContentSize.Height + 30), new Rect (ContentLocation, ContentSize));
        }

        public override bool IsPageCountValid
        {
            get
            {
                return true;
            }
        }

        public override int PageCount
        {
            get
            {
                return _pages.Count;
            }
        }

        public override Size PageSize
        {
            get; set;
        }


        public override IDocumentPaginatorSource Source
        {
            get
            {
                return null;
            }
        }
        #endregion
    }

    internal static class Extensions
    {
        internal static void InvalidateVisualTree(this FrameworkElement target)
        {
            target.InvalidateMeasure();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(target); i++)
            {
                var child = VisualTreeHelper.GetChild(target, i) as FrameworkElement;
                if (child != null)
                {
                    child.InvalidateVisualTree();
                }
            }
        }
    }
}
 