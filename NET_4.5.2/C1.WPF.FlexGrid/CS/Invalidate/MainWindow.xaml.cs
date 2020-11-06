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

namespace Invalidate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // populate grid
            var list = new List<Rect>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Rect(i, i, i, i));
            }
            _flex.ItemsSource = list;

            // attach custom cell factory
            _flex.CellFactory = new CustomCellFactory(_flex);
        }
    }
    public class CustomCellFactory : CellFactory
    {
        C1FlexGrid _flex;
        CellRange _hotRange;
        static Brush _arrowBrush = new SolidColorBrush(Colors.Red);
        static PointCollection _leftArrow = new PointCollection(new Point[] { new Point(0, 0), new Point(10, 5), new Point(0, 10) });
        static PointCollection _rightArrow = new PointCollection(new Point[] { new Point(10, 0), new Point(0, 5), new Point(10, 10) });

        // ctor
        public CustomCellFactory(C1FlexGrid flex)
        {
            _flex = flex;
            _flex.MouseMove += _flex_MouseMove;
        }

        // update HotRange when the mouse moves over the grid
        void _flex_MouseMove(object sender, MouseEventArgs e)
        {
            var ht = _flex.HitTest(e);
            HotRange = ht.CellRange;
        }

        // get/set HotRange
        CellRange HotRange
        {
            get { return _hotRange; }
            set
            {
                if (value != _hotRange)
                {
                    // invalidate old, update, invalidate new
                    _flex.Invalidate(_hotRange);
                    _hotRange = value;
                    _flex.Invalidate(_hotRange);
                }
            }
        }

        // highlight hot cell using two red arrows
        public override void CreateCellContent(C1.WPF.FlexGrid.C1FlexGrid grid, Border bdr, C1.WPF.FlexGrid.CellRange range)
        {
            base.CreateCellContent(grid, bdr, range);
            if (HotRange.Contains(range))
            {
                // save original content
                var child = bdr.Child;
                bdr.Child = null;
                
                // create grid to hold content and two red arrows
                var g = new Grid();
                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                g.ColumnDefinitions.Add(new ColumnDefinition());
                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

                // set content
                child.SetValue(Grid.ColumnProperty, 1);
                g.Children.Add(child);

                // set left arrow
                var p1 = new Polygon();
                p1.Points = _leftArrow;
                p1.Fill = _arrowBrush;
                p1.VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(p1);

                // set right arrow
                var p2 = new Polygon();
                p2.Points = _rightArrow;
                p2.Fill = _arrowBrush;
                p2.VerticalAlignment = VerticalAlignment.Center;
                p2.SetValue(Grid.ColumnProperty, 2);
                g.Children.Add(p2);

                // assign new content to cell
                bdr.Child = g;
            }
        }
    }
}
