using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using C1.Silverlight;
using C1.WPF.Excel;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Represents a worksheet within an ExcelBook.
    /// </summary>
    /// <remarks>
    /// This class extends the C1TabItem class and has an editable label on its header,
    /// which represents the sheet name, and a C1FlexGrid control that contains the sheet 
    /// content.
    /// </remarks>
    public class Sheet : C1TabItem
    {
        //--------------------------------------------------------------------------
        #region ** ctor

        public Sheet(XLSheet xlSheet)
        {
            // customize style
            FontSize = 14;
            FontFamily = new FontFamily("Arial");
            Padding = new Thickness(0);

            // create grid to hold the sheet data
            Grid = new C1FlexGrid();
            Grid.MergeManager = new ExcelMergeManager();

            // create editable label used as a header
            var label = new EditableLabel();
            Header = label;

            // create context menu for this sheet
            var ctxMenu = new SheetContextMenu(this);

            // if we have a sheet, get its name and load it into the grid
            if (xlSheet != null)
            {
                SheetName = xlSheet.Name;
                ExcelFilter.Load(xlSheet, this.Grid);
                Visibility = xlSheet.Visible ? Visibility.Visible : Visibility.Collapsed;
            }

            // keep track of changes to the sheet name
            label.TextChanged += label_TextChanged;
        }
        public Sheet()
            : this(null)
        {
        }

        #endregion 

        //--------------------------------------------------------------------------
        #region ** object model

        public string SheetName
        {
            get { return ((EditableLabel)Header).Text; }
            set { ((EditableLabel)Header).Text = value; }
        }
        public C1FlexGrid Grid { get; set; }
        public C1FlexGridBook Book { get; set; }
        public void EditName()
        {
            var label = (EditableLabel)Header;
            label.StartEditing();
        }

        #endregion

        //--------------------------------------------------------------------------
        #region ** event handlers

        void label_TextChanged(object sender, EventArgs e)
        {
            // assume all is fine
            string msg = null;

            // make sure name is not empty
            if (string.IsNullOrEmpty(this.SheetName))
            {
                msg = "Sheet names cannot be blank.";
            }

            // make sure new name is unique
            foreach (var sheet in Book.Sheets)
            {
                if (sheet != this && string.Compare(sheet.SheetName, this.SheetName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    msg = "Sheet names must be unique.";
                    break;
                }
            }

            // warn user and rename sheet if necessary
            if (msg != null)
            {
                MessageBox.Show(msg, "Sheet Name", MessageBoxButton.OK);
                SheetName = this.Book.Sheets.GetUniqueName();
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            var element = FocusManager.GetFocusedElement(Window.GetWindow(this));
            if (!(element is TextBox))
            {
                e.Handled = true;
            }
        }

        #endregion
    }
    /// <summary>
    /// Special item that doesn't have an associated sheet, it simply creates a new sheet
    /// when it is clicked.
    /// </summary>
    public class NewSheet : Sheet
    {
        public NewSheet()
        {
            var img = new Image();
            img.Stretch = Stretch.None;
            img.Source = C1FlexGridBook.LoadBitmap("NewSheet.png");
            Header = img;
        }      
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            Book.Sheets.InsertSheet(true);
        }
    }
    /// <summary>
    /// Class that managers the list of Sheet items associated with a grid and provides
    /// methods for adding, removing, deleting, and renaming sheets.
    /// </summary>
    public class SheetCollection : ObservableCollection<Sheet>
    {
        //--------------------------------------------------------------------------
        #region ** fields

        C1FlexGridBook _book;
        Sheet _currentSheet;
        bool _updating;

        #endregion

        //--------------------------------------------------------------------------
        #region ** ctor

        internal SheetCollection(C1FlexGridBook book)
        {
            _book = book;
            book.Tabs.ItemsChanged += Tabs_ItemsChanged;
            book.Tabs.SelectionChanged += Tabs_SelectionChanged;
        }

        #endregion

        //--------------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Gets or sets the index of the currently selected sheet in the collection.
        /// </summary>
        public int SelectedIndex
        {
            get { return _book.Tabs.SelectedIndex; }
            set
            {
                _book.Tabs.SelectedIndex = value;
                ShowCurrentSheet();
            }
        }
        /// <summary>
        /// Gets or sets the currently selected sheet.
        /// </summary>
        public Sheet SelectedSheet
        {
            get { return SelectedIndex > -1 && SelectedIndex < Count ? this[SelectedIndex] : null; }
            set { SelectedIndex = this.IndexOf(value); }
        }
        /// <summary>
        /// Gets the index of the sheet with a given name.
        /// </summary>
        public int IndexOf(string sheetName)
        {
            for (int index = 0; index < Count; index++)
            {
                if (string.Compare(this[index].SheetName, sheetName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return index;
                }
            }
            return -1;
        }
        /// <summary>
        /// Determines whether the collection contains a sheet with a given name.
        /// </summary>
        public bool Contains(string sheetName)
        {
            return IndexOf(sheetName) > -1;
        }
        /// <summary>
        /// Retrieves a sheet by name.
        /// </summary>
        public Sheet this[string sheetName]
        {
            get 
            { 
                var index = IndexOf(sheetName);
                return index > -1 ? this[index] : null;
            }
        }
        /// <summary>
        /// Inserts a new sheet at the currently selected position or appends it to the collection.
        /// </summary>
        /// <param name="append"></param>
        public void InsertSheet(bool append)
        {
            var newSheet = new Sheet();
            for (int r = 0; r < 50; r++)
            {
                newSheet.Grid.Rows.Add(new ExcelRow());
            }
            for (int c = 0; c < 10; c++)
            {
                newSheet.Grid.Columns.Add(new Column());
            }
            var index = append ? Count : SelectedIndex;
            this.Insert(index, newSheet);
            SelectedIndex = index;
        }
        /// <summary>
        /// Deletes the current sheet.
        /// </summary>
        public void DeleteSheet()
        {
            var newCurrentSheet = GetNewCurrentSheet();
            if (newCurrentSheet != null)
            {
                this.RemoveAt(SelectedIndex);
                SelectedSheet = newCurrentSheet;
            }
        }
        /// <summary>
        /// Puts the current sheet in edit mode so the user can rename it.
        /// </summary>
        public void RenameSheet()
        {
            var sheet = SelectedSheet;
            if (sheet != null)
            {
                sheet.EditName();
            }
        }
        /// <summary>
        /// Hides the current sheet from view.
        /// </summary>
        public void HideSheet() 
        {
            var newCurrentSheet = GetNewCurrentSheet();
            if (newCurrentSheet != null)
            {
                SelectedSheet.Visibility = Visibility.Collapsed;
                SelectedSheet = newCurrentSheet;
            }
        }
        /// <summary>
        /// Unhides all hidden sheets.
        /// </summary>
        public void UnhideSheets() 
        {
            foreach (var sheet in this)
            {
                sheet.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Selects the first visible sheet.
        /// </summary>
        public void SelectFirst()
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Visibility == Visibility.Visible)
                {
                    SelectedIndex = i;
                    break;
                }
            }
        }
        /// <summary>
        /// Selects the previous visible sheet.
        /// </summary>
        public void SelectPrev()
        {
            for (int i = SelectedIndex - 1; i >=0 ; i--)
            {
                if (this[i].Visibility == Visibility.Visible)
                {
                    SelectedIndex = i;
                    break;
                }
            }
        }
        /// <summary>
        /// Selects the next visible sheet.
        /// </summary>
        public void SelectNext()
        {
            for (int i = SelectedIndex + 1; i < Count; i++)
            {
                if (this[i].Visibility == Visibility.Visible)
                {
                    SelectedIndex = i;
                    break;
                }
            }
        }
        /// <summary>
        /// Selects the last visible sheet.
        /// </summary>
        public void SelectLast()
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (this[i].Visibility == Visibility.Visible)
                {
                    SelectedIndex = i;
                    break;
                }
            }
        }

        // gets a unique sheet name
        internal string GetUniqueName()
        {
            for (int i = 1; ; i++)
            {
                var name = string.Format("Sheet{0}", i);
                if (IndexOf(name) < 0)
                {
                    return name;
                }
            }
        }

        #endregion

        //--------------------------------------------------------------------------
        #region ** event handlers

        // show currently selected sheet on the main grid
        void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowCurrentSheet();
        }

        // synchronize tabs control with this collection
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_updating)
            {
                _updating = true;
                var items = _book.Tabs.Items;
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        int index = e.NewStartingIndex;
                        foreach (Sheet item in e.NewItems)
                        {
                            item.Book = _book;
                            if (string.IsNullOrEmpty(item.SheetName))
                            {
                                item.SheetName = GetUniqueName();
                            }
                            items.Insert(index++, item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (Sheet item in e.OldItems)
                        {
                            items.Remove(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        items.Clear();
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        Debug.Assert(false, "Replacing sheets?");
                        break;
                }

                // make sure the new item sheet is in the proper position
                EnsureNewSheet();

                // done
                _updating = false;
            }

            // fire event as usual
            base.OnCollectionChanged(e);
        }

        // synchronize this collection with tab control
        void Tabs_ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_updating)
            {
                _updating = true;
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Remove:
                        foreach (Sheet item in e.OldItems)
                        {
                            this.Remove(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Add:
                        int index = e.NewStartingIndex;
                        foreach (Sheet item in e.NewItems)
                        {
                            this.Insert(index++, item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Reset:
                        Debug.Assert(false, "Replacing/resetting tab items?");
                        break;
                }

                // make sure the new item sheet is in the proper position
                EnsureNewSheet();

                // done updating
                _updating = false;
            }
        }
        void EnsureNewSheet()
        {
            var items = _book.Tabs.Items;
            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var newSheet = items[i] as NewSheet;
                    if (newSheet != null && i < items.Count - 1)
                    {
                        items.RemoveAt(i);
                        i--;
                    }
                }
                if (!(items[items.Count - 1] is NewSheet))
                {
                    var newSheet = new NewSheet();
                    newSheet.Book = this._book;
                    items.Add(newSheet);
                }
            }
        }

        #endregion

        //--------------------------------------------------------------------------
        #region ** implementation

        // gets the sheet that will become current when the current one is deleted or hidden
        Sheet GetNewCurrentSheet()
        {
            // look for the next visible sheet
            for (int i = 1; i < this.Count; i++)
            {
                var index = (SelectedIndex + i) % this.Count;
                var sheet = this[index];
                if (sheet.Visibility == Visibility.Visible)
                {
                    return sheet;
                }
            }

            // no more visible sheets
            MessageBox.Show("A workbook must contain at least one visible worksheet.");
            return null;
        }

        // show currently selected sheet on the main grid
        void ShowCurrentSheet()
        {
            if (_currentSheet != SelectedSheet)
            {
                // clear undo stack when switching sheets
                _book.UndoStack.Clear();

                // save current selection
                if (_currentSheet != null)
                {
                    var grid = _currentSheet.Grid;
                    grid.Selection = _book.Selection;
                    grid.ScrollPosition = _book.ScrollPosition;
                    _currentSheet.FontWeight = FontWeights.Normal;
                }

                // show new item
                _currentSheet = SelectedSheet;
                if (_currentSheet != null)
                {
                    var grid = _currentSheet.Grid;
                    CopyGrid(grid, _book);
                    _currentSheet.FontWeight = FontWeights.Bold;
                }
            }
        }

        // copy the content of an invisible grid to this one
        static void CopyGrid(C1FlexGrid src, C1FlexGrid dst)
        {
            // copy global parameters
            dst.FontFamily = src.FontFamily;
            dst.FontSize = src.FontSize;
            dst.IsReadOnly = src.IsReadOnly;
            dst.GridLinesVisibility = src.GridLinesVisibility;
            dst.HeadersVisibility = src.HeadersVisibility;
            dst.GroupRowPosition = src.GroupRowPosition;

            // copy rows and columns
            CopyGridPanel(src.Cells, dst.Cells);
            CopyGridPanel(src.ColumnHeaders, dst.ColumnHeaders);
            CopyGridPanel(src.RowHeaders, dst.RowHeaders);

            // copy default sizes/frozen elements
            dst.Rows.DefaultSize = src.Rows.DefaultSize;
            dst.Rows.Frozen = src.Rows.Frozen;
            dst.Columns.DefaultSize = src.Columns.DefaultSize;
            dst.Columns.Frozen = src.Columns.Frozen;

            // copy merge manager
            dst.MergeManager = src.MergeManager;

            // copy selection (and bring it into view)
            dst.ScrollPosition = new Point(0, 0);
            dst.Selection = src.Selection;
        }
        static void CopyGridPanel(GridPanel src, GridPanel dst)
        {
            // defer notifications to avoid updates while populating the grid
            // this makes a huge performance difference when loading large grids
            using (dst.Rows.DeferNotifications())
            using (dst.Columns.DeferNotifications())
            {
                // clear destination
                dst.Rows.Clear();
                dst.Columns.Clear();

                // copy rows
                foreach (var row in src.Rows)
                {
                    dst.Rows.Add(row);
                }

                // copy columns
                foreach (var col in src.Columns)
                {
                    dst.Columns.Add(col);
                }
            }
        }

        #endregion
    }
}
