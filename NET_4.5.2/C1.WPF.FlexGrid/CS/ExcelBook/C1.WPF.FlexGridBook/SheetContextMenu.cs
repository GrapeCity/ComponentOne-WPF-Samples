using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using C1.Silverlight;

namespace C1.WPF.FlexGridBook
{
    public class SheetContextMenu : ContextMenu
    {
        // ** fields

        Sheet _sheet;
        MenuItem _miInsert, _miDelete, _miRename, _miHide, _miUnhide;
        
        // ** ctor

        public SheetContextMenu(Sheet sheet)
        {
            _sheet = sheet;
            sheet.MouseRightButtonDown += sheet_MouseRightButtonDown;
            _miInsert = AddMenuItem("Insert");
            _miDelete = AddMenuItem("Delete");
            _miRename = AddMenuItem("Rename");
            _miHide = AddMenuItem("Hide");
            _miUnhide = AddMenuItem("Unhide");
        }

        // ** overrides/event handlers

        void sheet_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            _sheet.IsSelected = true;
            var pt = e.GetPosition(_sheet);
            pt.Y = _sheet.RenderSize.Height - this.Items.Count * (this.FontSize + 10);
            this.IsOpen = true;
            //Show(_sheet, pt);
        }
        protected override void OnOpened(RoutedEventArgs e)
        {
            // check whether there are hidden sheets and count the visible ones
            bool hidden = false;
            int count = 0;
            foreach (var sheet in _sheet.Book.Sheets)
            {
                if (sheet.Visibility == System.Windows.Visibility.Collapsed)
                {
                    hidden = true;
                }
                else
                {
                    count++;
                }
            }

            // enable 'unhide' option only when there are hidden sheets
            _miUnhide.IsEnabled = hidden;

            // enable 'delete' and 'hide' only when there are more than one visible sheets
            _miDelete.IsEnabled = _miHide.IsEnabled = count > 1;

            // fire event as usual
            base.OnOpened(e);
        }

        void mi_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            var sheets = _sheet.Book.Sheets;
            if (item == _miInsert)
            {
                sheets.InsertSheet(false);
            }
            else if (item == _miDelete)
            {
                sheets.DeleteSheet();
            }
            else if (item == _miRename)
            {
                sheets.RenameSheet();
            }
            else if (item == _miHide)
            {
                sheets.HideSheet();
            }
            else if (item == _miUnhide)
            {
                sheets.UnhideSheets();
            }
        }

        // ** implementation

        MenuItem AddMenuItem(string text)
        {
            var mi = new MenuItem();
            mi.Header = text;
            Items.Add(mi);
            mi.Click += mi_Click;
            return mi;
        }
    }
}
