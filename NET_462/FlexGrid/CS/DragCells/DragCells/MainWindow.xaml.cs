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
using C1.WPF.FlexGrid;
using C1.WPF;

namespace DragCells
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyViewObject _dragViewObject;

        public MainWindow()
        {
            InitializeComponent();

            // ensure flex template has been loaded
            _flex.ApplyTemplate();

            // make rows tall enough to show our custom objects
            _flex.Rows.DefaultSize = 50;

            // no editing
            _flex.IsReadOnly = true;

            // create unbound rows and columns
            for (int r = 0; r < 50; r++)
            {
                _flex.Rows.Add(new Row());
            }
            for (int c = 0; c < 10; c++)
            {
                _flex.Columns.Add(new Column());
            }

            // create objects (model) and assign them to grid cells
            for (int r = 0; r < _flex.Rows.Count; r++)
            {
                for (int c = 0; c < _flex.Columns.Count; c++)
                {
                    if ((r + c) % 2 == 0)
                    {
                        _flex[r, c] = new MyDataObject(r, c);
                    }
                }
            }

            // register flex as drop target
            var ddMgr = new C1DragDropManager();
            ddMgr.RegisterDropTarget(_flex, true);
            ddMgr.DragStart += DdMgr_DragStart;
            ddMgr.DragDrop += _ddMgr_DragDrop;

            // activate custom cell factory
            _flex.CellFactory = new MyCellFactory(ddMgr);
        }

        private void DdMgr_DragStart(object source, DragDropEventArgs e)
        {
            var bdr = e.DragSource as Border;
            _dragViewObject = bdr.Child as MyViewObject;
        }

        // handle user dragging a cell to a new position
        void _ddMgr_DragDrop(object source, DragDropEventArgs e)
        {            
            if (_dragViewObject != null)
            {
                // get target cell
                var ht = _flex.HitTest(e.GetPosition(_flex));
                if (ht.CellType == CellType.Cell && ht.Row > -1 && ht.Column > -1)
                {
                    // move object to target cell 
                    var oldRange = new CellRange(e.DragSource);
                    _flex[oldRange.Row, oldRange.Column] = null;
                    _flex[ht.Row, ht.Column] = _dragViewObject.DataObject;

                    // and move the selection
                    _flex.Select(ht.Row, ht.Column);
                    _dragViewObject = null;
                }
            }
        }
    }
}
