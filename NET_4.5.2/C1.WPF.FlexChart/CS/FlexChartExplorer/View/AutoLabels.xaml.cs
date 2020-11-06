using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using C1.Chart;
using C1.WPF.Chart;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AutoLabels : UserControl
    {
        List<string> _dataShapes;
        DataShape _dataShape = DataShape.Line;
        List<string> _overlappings;

        public AutoLabels()
        {
            this.InitializeComponent();

            Data = DataHelper.Create(DataShape, 100);
        }

        public List<string> Overlappings
        {
            get
            {
                if (_overlappings == null)
                    _overlappings = Enum.GetNames(typeof(LabelOverlapping)).ToList();
                return _overlappings;
            }
        }

        public List<string> DataShapes
        {
            get
            {
                if (_dataShapes == null)
                    _dataShapes = Enum.GetNames(typeof(DataShape)).ToList();
                return _dataShapes;
            }
        }

        public DataShape DataShape
        {
            get { return _dataShape; }
            set
            {
                if (_dataShape != value)
                {
                    _dataShape = value;
                    Data = DataHelper.Create(DataShape, 100);
                }
            }
        }

        public Point[] Data
        {
            get { return (Point[])GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Point[]), typeof(AutoLabels), new PropertyMetadata(null));
    }

    public enum DataShape
    {
        Line,
        Sin,
        Random,
        Ellipse,
        Spiral,
        Grid
    }

    class DataHelper
    {
        static Random rnd = new Random();

        public static Point[] Create(DataShape shape, int npts)
        {
            var pts = new Point[npts];

            Func<int, Point> f = null;

            switch (shape)
            {
                case DataShape.Line:
                    f = (i) => new Point(i, i);
                    break;
                case DataShape.Sin:
                    f = (i) => new Point(i, (float)Math.Sin(0.1 * i));
                    break;
                case DataShape.Random:
                    f = (i) => new Point(i, (float)rnd.NextDouble() * 100);
                    break;
                case DataShape.Ellipse:
                    f = (i) => new Point((float)Math.Sin((2 * Math.PI * i) / npts), (float)Math.Cos((2 * Math.PI * i) / npts));
                    break;
                case DataShape.Spiral:
                    f = (i) => new Point(i * (float)Math.Sin((4 * Math.PI * i) / npts), i * (float)Math.Cos((4 * Math.PI * i) / npts));
                    break;
                case DataShape.Grid:
                    var l = (int)Math.Sqrt(npts);
                    f = (i) => new Point(i % l, (int)(i / l));
                    break;
            }

            for (var i = 0; i < npts; i++)
                pts[i] = f(i);

            return pts;
        }
    }
}
