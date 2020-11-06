using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF;
using C1.WPF.FlexGrid;

namespace ExcelDragDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1DragDropManager _dd = new C1DragDropManager();
        Point _dragOffset;
        string _clip;
        const int DRAG_THRESHOLD = 4;
        string _flexname = "";
        
        public MainWindow()
        {
            InitializeComponent();
            _flex1.ItemsSource = Product.GetProducts(100);
            _flex2.ItemsSource = Product.GetProducts(100);
            // monitor mouse over the grids
            _flex1.MouseMove += _flex_MouseMove;
            _flex1.PreviewMouseLeftButtonDown += _flex_PreviewMouseLeftButtonDown;
            _flex2.MouseMove += _flex_MouseMove;
            _flex2.PreviewMouseLeftButtonDown += _flex_PreviewMouseLeftButtonDown;
           
           
            // initialize drag drop manager
            _dd.RegisterDropTarget(LayoutRoot, true);
            _dd.DragOver += _dd_DragOver;
            _dd.DragDrop += _dd_DragDrop;
        }
      
        // offer to drag the selection when the mouse hovers over its edges
        void _flex_MouseMove(object sender, MouseEventArgs e)
        {
            var flex = sender as C1FlexGrid;
            flex.Cursor = MouseInDragArea(flex, e)
                ? Cursors.Hand
                : null;
        }
  
        // start dragging when the user clicks the mouse in the drag zone
        void _flex_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var flex = sender as C1FlexGrid;
            _flexname = flex.Name;
            
            if (MouseInDragArea(flex, e))
            {
                // create image for dragging (optional)
                var flexImage = flex.GetGridImage(flex.Selection) as C1FlexGrid;
                flexImage.HeadersVisibility = HeadersVisibility.None;
                flexImage.Opacity = 0.5;
                _dd.TargetMarker.Child = flexImage;

                // copy data from source grid
                _clip = flex.GetClipString(ClipboardCopyMode.ExcludeHeader);

                // start dragging
                _dd.DoDragDrop(flex.Marquee, e, DragDropEffect.Copy);
                e.Handled = true;
            }
        }
        // provide feedback while dragging
        void _dd_DragOver(object source, DragDropEventArgs e)
        {

            // assume we can't drop here
            e.Effect = DragDropEffect.None;

            // check whether the mouse is over one of the grids
            var dst = GetDropTarget(e);
            if (dst != null)
            {
                // allow drop
                e.Effect = DragDropEffect.Copy;

                // select range to show drop location
                var dstRange = GetDropRange(e);
                dst.Select(dstRange, true);
            }
        }
        void _dd_DragDrop(object source, DragDropEventArgs e)
        {

            // get source grid
            var src = GetParentOfType<C1FlexGrid>(e.DragSource);

            // get target grid
            var dst = GetDropTarget(e);
            if (dst != null)
            {
                // no need for clipboard
                dst.SetClipString(_clip, dst.Selection, ClipboardCopyMode.ExcludeHeader);
            }
        }
        // get a valid drop target (mouse over a grid with dropping enabled)
        C1FlexGrid GetDropTarget(DragDropEventArgs e)
        {
            if (!string.IsNullOrEmpty(_flexname))
            {
                if (_flexname == "_flex1" && _chkDropRight.IsChecked.Value)
                {
                    return _flex2;
                }
                else if (_flexname == "_flex2" && _chkDropLeft.IsChecked.Value)
                {
                    return _flex1;
                }
            }
          
            return null;
         
          
        }
       
        // get a valid drop range
        CellRange GetDropRange(DragDropEventArgs e)
        {
            // get source, target
            var src = GetParentOfType<C1FlexGrid>(e.DragSource);
            var dst = GetDropTarget(e);

            // get target range from mouse position
            var pt = e.GetPosition(dst);
            pt.X -= _dragOffset.X;
            pt.Y -= _dragOffset.Y;
            var ht = dst.HitTest(pt);
            var rng = ht.CellRange;
            if (rng.Row < 0) rng.Row = 0;
            if (rng.Column < 0) rng.Column = 0;

            // expand to match size of source range
            var sel = src.Selection;
            rng.Row2 = Math.Min(rng.Row + sel.RowSpan - 1, dst.Rows.Count - 1);
            rng.Column2 = Math.Min(rng.Column + sel.ColumnSpan - 1, dst.Columns.Count - 1);

            // done
            return rng;
        }
        // check whether the mouse is in the drag zone
        bool MouseInDragArea(C1FlexGrid flex, MouseEventArgs e)
        {
            // check that this grid can be used as a drag source
            if ((flex == _flex1 && !_chkDragLeft.IsChecked.Value) ||
                (flex == _flex2 && !_chkDragRight.IsChecked.Value))
            {
                _flexname = "";
                return false;
            }
            
            // check that we are over a cell
            var ht = flex.HitTest(e);
            if (ht.CellType != CellType.Cell || !ht.CellRange.IsValid)
            {
                return false;
            }

            // get mouse position and selection rectangle
            var pt = e.GetPosition(flex.Cells);
            var rc = flex.Cells.GetCellRect(flex.Selection);
            _dragOffset = new Point(pt.X - rc.X - 20, pt.Y - rc.Y - 5);

            // check that the mouse is close to the edge of the selection
            return Math.Abs(pt.X - rc.Left) < DRAG_THRESHOLD ||
                Math.Abs(pt.X - rc.Right) < DRAG_THRESHOLD ||
                Math.Abs(pt.Y - rc.Top) < DRAG_THRESHOLD ||
                Math.Abs(pt.Y - rc.Bottom) < DRAG_THRESHOLD;
        }
        /// <summary>
        /// Gets an element's first ancestor of a given type.
        /// </summary>
        /// <typeparam name="T">Type to look for.</typeparam>
        /// <param name="e">Child element.</param>
        public static T GetParentOfType<T>(DependencyObject e) where T : DependencyObject
        {
            for (; e != null; )
            {
                e = VisualTreeHelper.GetParent(e);
              //  System.Diagnostics.Debug.WriteLine(e.GetType().ToString());
                if (e is T)
                {
                    return (T)e;
                }
            }
            return null;
        }

        public static T GetChildOfType<T>(DependencyObject e) where T : DependencyObject
        {
            for (; e != null; )
            {
                e = VisualTreeHelper.GetChild(e,0);
                if (e is T)
                {
                    return (T)e;
                }
            }
            return null;
        }
    }
}
