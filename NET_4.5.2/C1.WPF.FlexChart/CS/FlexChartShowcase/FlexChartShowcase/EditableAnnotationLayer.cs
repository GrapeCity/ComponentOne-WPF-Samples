using C1.Chart;
using C1.Chart.Annotation;
using C1.WPF.Chart;
using C1.WPF.Chart.Annotation;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace FlexChartShowcase
{
    /// <summary>
    /// Creates an AnnotationLayer that provides for 
    /// Adding, Editing and  Moving Annotations at runtime
    /// </summary>
    public class EditableAnnotationLayer : AnnotationLayer
    {
        #region Object Model
        /// <summary>
        /// Allow moving Annotations at runtime
        /// </summary>
        public bool AllowMove { get; set; }

        /// <summary>
        /// Allow adding new Annotations at runtime
        /// </summary>
        public bool AllowAdd { get; set; }

        /// <summary>
        /// FlexChart object to which the Annotations are added
        /// </summary>
        public C1FlexChart FlexChart { get { return _flexChart; } }

        /// <summary>
        /// Selection style for the SelectedAnnotation
        /// </summary>
        public ChartStyle SelectionStyle { get; set; }

        /// <summary>
        /// Annotation that has been Selected on the chart
        /// </summary>
        public AnnotationBase SelectedAnnotation
        {
            get { return _selectedAnnotation; }
            private set
            {
                if (_selectedAnnotation != value)
                {
                    _selectedAnnotation.SetStyle(_originalStyle); //Reset the Style
                    _selectedAnnotation = value;
                    _originalStyle = _selectedAnnotation.CloneStyle(); //Store the style of new selected Annotation
                    _selectedAnnotation.SetStyle(SelectionStyle);
                    SendToFront();
                }
            }
        }
        /// <summary>
        /// Specifies the Type for the new Annotaions that are to be added by click & drag operation
        /// </summary>
        public Type NewAnnotationType
        {
            get { return _newAnnoType; }
            set
            {
                if (!typeof(AnnotationBase).IsAssignableFrom(value))
                {
                    throw new FormatException("Not Supported Annotation Type!!");
                }
                _newAnnoType = value;
            }
        }
        /// <summary>
        /// Specifies the AnnotationAttachment for new Annotations that will be added to the chart by click & drag operation
        /// </summary>
        public AnnotationAttachment Attachment { get; set; }

        /// <summary>
        /// Defines a delegate that determines the Polygon shape that will be added to the chart by click & drag operation
        /// </summary>
        public Func<Point, Polygon> PolygonAddFunc { get; set; }

        /// <summary>
        ///Defines a delegate that determines the resizing of the Polygon shape while adding 
        /// </summary>
        public Action<Polygon, Rect> PolygonReSizeFunc { get; set; }

        public TextEditor ContentEditor { get; set; }
        #endregion

        #region fields
        private AnnotationBase _selectedAnnotation;
        private AnnotationBase _annotationToAdd;
        private C1FlexChart _flexChart;
        private Point _oldPoint, _newPoint;  //For Moving Annotations
        private Point _start, _end;          //For Adding Annotations
        private ChartStyle _originalStyle;
        private Type _newAnnoType;
        private bool _isDragging, _drawing;
        private List<AnnotationEx> _rectCache;
        private Popup EditorPopup;
        #endregion

        #region C'tor
        public EditableAnnotationLayer(C1FlexChart chart)
        {
            AllowMove = true;
            _flexChart = chart;

            _flexChart.PreviewMouseDown += OnPreviewMouseDown;
            _flexChart.MouseDown += OnMouseDown;
            _flexChart.PreviewMouseLeftButtonUp += _flexChart_PreviewMouseLeftButtonUp;
            _flexChart.MouseUp += OnMouseUp;
            _flexChart.MouseMove += OnMouseMove;
            _flexChart.MouseDoubleClick += OnMouseDoubleClick;
            //_flexChart.Rendered += OnChartRendered;
            Annotations.CollectionChanged += OnCollectionChanged;

            _rectCache = new List<AnnotationEx>();

            _originalStyle = new ChartStyle
            {
                Fill = Brushes.Red
            };

            SelectionStyle = new ChartStyle
            {
                Fill = Brushes.White,
                Stroke = Brushes.Red,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 1, 2 }
            };

            Attachment = AnnotationAttachment.Absolute;

            //Popup to show Annotation editor
            EditorPopup = new Popup();
            EditorPopup.Placement = PlacementMode.AbsolutePoint;
            EditorPopup.PlacementTarget = FlexChart;

            FlexChart.KeyDown += (s1, e1) =>
            {
                if (e1.Key == Key.Escape)
                    HideContentEditor();
            };

            EditorPopup.Closed += (s1, e1) =>
            {
                _flexChart.Invalidate();
            };
        }

        private void _flexChart_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _drawing = false;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the Annotation at the specified point
        /// </summary>
        /// <param name="pt">Point in control coordiantes</param>
        /// <returns>The Annotation at the specified point, null otherwise</returns>
        public AnnotationBase HitTest(Point pt)
        {
            AnnotationBase selectedAnnotation = null;
            //Need to start hit-testing from the TopMost annotation
            foreach (var anno in this.Annotations.Reverse())
            {
                var annoEx = _rectCache.First(a => a.annotation == anno);
                if (annoEx.HitTest(pt, 0))
                {
                    selectedAnnotation = annoEx.annotation;
                    break;
                }
            }
            return selectedAnnotation;
        }

        #endregion

        #region implementation

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HideContentEditor();
            _drawing = false;
        }
        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(FlexChart);
            SelectedAnnotation = HitTest(pos);

            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            if (SelectedAnnotation != null && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                _oldPoint = pos;
                _isDragging = true; //Start Dragging                
                FlexChart.Cursor = Cursors.SizeAll;
            }
            else
            {
                if (AllowAdd && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    this._start = pos;
                    if (this.NewAnnotationType == typeof(Text))
                    {
                        AddAnnotation(_start);
                    }
                }
                else if (e.RightButton != MouseButtonState.Pressed)
                {
                    SelectedAnnotation = null;
                    _flexChart.Invalidate();
                }
            }
            //}));
        }
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed))
            {
                FlexChart.Cursor = Cursors.Arrow;
                return;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (AllowMove && SelectedAnnotation != null && _isDragging && _flexChart.PlotRect.Contains(e.GetPosition(FlexChart)))
                {
                    FlexChart.Cursor = Cursors.SizeAll;
                    _newPoint = e.GetPosition(FlexChart);
                    var diff = new Point
                    {
                        X = _newPoint.X - _oldPoint.X,
                        Y = _newPoint.Y - _oldPoint.Y
                    };

                    if (SelectedAnnotation is Line)
                    {
                        var line = SelectedAnnotation as Line;
                        var start = Helpers.AnnoPointToCoords(_flexChart, line, line.Start);
                        var end = Helpers.AnnoPointToCoords(_flexChart, line, line.End);

                        start = start.OffSet(diff);
                        end = end.OffSet(diff);

                        line.Start = Helpers.CoordsToAnnoPoint(_flexChart, line, start);
                        line.End = Helpers.CoordsToAnnoPoint(_flexChart, line, end);
                        _flexChart.Invalidate();
                    }
                    else if (SelectedAnnotation is Polygon)
                    {
                        var polygon = SelectedAnnotation as Polygon;
                        for (int i = 0; i < polygon.Points.Count; i++)
                        {
                            var pt = Helpers.AnnoPointToCoords(_flexChart, polygon, polygon.Points[i]);
                            pt = pt.OffSet(diff);
                            polygon.Points[i] = Helpers.CoordsToAnnoPoint(_flexChart, polygon, pt);
                        }
                        _flexChart.Invalidate();
                    }
                    else
                    {
                        var location = Helpers.AnnoPointToCoords(_flexChart, SelectedAnnotation, SelectedAnnotation.Location);
                        location = location.OffSet(diff);
                        SelectedAnnotation.Location = Helpers.CoordsToAnnoPoint(_flexChart, SelectedAnnotation, location);
                        _flexChart.Invalidate();
                    }
                    _oldPoint = _newPoint;                    
                    return;
                }

                if (this.AllowAdd && this._start != new Point(-1, -1))
                {
                    if (_drawing)
                    {
                        this.UpdateAnnotation(e.GetPosition(FlexChart));
                        _flexChart.Invalidate();
                    }
                    else
                    {
                        Size sz = new Size
                        {
                            Height = Math.Abs(this._start.Y - (float)e.GetPosition(FlexChart).Y),
                            Width = Math.Abs(this._start.X - (float)e.GetPosition(FlexChart).X)
                        };
                        Size threshold = new Size(2, 2);
                        if (sz.Width > (float)threshold.Width && sz.Height > (float)threshold.Height)
                        {
                            this.AddAnnotation(this._start);
                            _flexChart.Invalidate();
                        }
                    }
                }
            }));
        }
        private void OnMouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AllowAdd)
            {
                _start = _end = new Point(-1, -1);
                _drawing = false;
            }
            if (_isDragging)
            {
                _isDragging = false;
                //UpdateRectCache(SelectedAnnotation);
                FlexChart.Cursor = Cursors.Arrow;
            }
            
            foreach (var anno in _rectCache)
            {
                UpdateRectCache(anno.annotation);
            }
            _flexChart.Invalidate();
        }
        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ContentEditor == null || SelectedAnnotation == null)
                return;

            //Show Annotation Editor
            if (this.HitTest(e.GetPosition(FlexChart)) == this.SelectedAnnotation && SelectedAnnotation.IsEditable())
            {
                var mainWindow = ((System.Windows.Window)(((System.Windows.FrameworkElement)FlexChart.Parent).Parent));
                ShowContentEditor(this.SelectedAnnotation, e.GetPosition(Application.Current.MainWindow));
            }
            else
                HideContentEditor();
        }
        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!IsAddOrDelete(e.Action))
                return;
            if (e.NewItems != null)
            {
                foreach (AnnotationBase newAnnotation in e.NewItems)
                {
                    _rectCache.Add(new AnnotationEx(FlexChart, newAnnotation, Helpers.GetRect(FlexChart, newAnnotation)));
                }
            }
            if (e.OldItems != null)
            {
                HideContentEditor();
                foreach (AnnotationBase oldAnnotation in e.OldItems)
                {
                    var anno = _rectCache.First(a => a.annotation == oldAnnotation);
                    if (anno != null)
                        _rectCache.Remove(anno);
                }
            }
        }
        //private void OnChartRendered(object sender, RenderEventArgs e)
        //{
        //}
        private bool IsAddOrDelete(NotifyCollectionChangedAction action)
        {
            return action == NotifyCollectionChangedAction.Add || action == NotifyCollectionChangedAction.Remove;
        }
        private void UpdateRectCache(AnnotationBase annoToUpdate)
        {
            if (annoToUpdate != null)
            {
                try
                {
                    var anno = _rectCache.First(a => a.annotation == annoToUpdate);
                    anno.BoundingRectangle = Helpers.GetRect(FlexChart, anno.annotation);
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void ShowContentEditor(AnnotationBase anno, Point location)
        {
            ContentEditor.Annotation = anno;
            ContentEditor.UpdateEditorContent();

            EditorPopup.Child = ContentEditor;
            EditorPopup.Width = double.NaN;
            EditorPopup.Height = double.NaN;
            Point pt = Application.Current.MainWindow.PointToScreen(location);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                EditorPopup.HorizontalOffset = pt.X + 10;
                EditorPopup.VerticalOffset = pt.Y + 10;
                EditorPopup.IsOpen = true;
                EditorPopup.Focus();
            }));

        }
        private void HideContentEditor()
        {
            EditorPopup.IsOpen = false;
            ContentEditor.Accept = ContentEditor.Reject = null;
            _flexChart.Invalidate();
            _flexChart.Focus();
        }
        private void AddAnnotation(Point p)
        {
            _start = p;
            try
            {
                _annotationToAdd = (AnnotationBase)Activator.CreateInstance(NewAnnotationType);
                _annotationToAdd.Attachment = Attachment;
                _annotationToAdd.Location = Helpers.CoordsToAnnoPoint(FlexChart, _annotationToAdd, _start);

                if (_annotationToAdd is Text)
                {
                    var pt = Mouse.GetPosition(FlexChart);
                    var anno = this.HitTest(pt);
                    if (anno == null)
                    {
                        ((Text)_annotationToAdd).Content = "New Text Annotation";
                        ShowContentEditor(_annotationToAdd, Mouse.GetPosition(Application.Current.MainWindow));
                        ContentEditor.Accept = () =>
                        {
                            this.Annotations.Add(_annotationToAdd);
                            SelectedAnnotation = _annotationToAdd;
                            HideContentEditor();
                        };
                        ContentEditor.Reject = () =>
                        {
                            _annotationToAdd = null;
                            HideContentEditor();
                        };
                    }
                    return;
                }
                if (_annotationToAdd is Polygon)
                {
                    var polygon = PolygonAddFunc(_start);
                    polygon.Attachment = Attachment;
                    for (int i = 0; i < polygon.Points.Count; i++)
                    {
                        polygon.Points[i] = Helpers.CoordsToAnnoPoint(FlexChart, polygon, polygon.Points[i]);
                    }
                    _annotationToAdd = polygon;
                }

                _annotationToAdd.Style = new ChartStyle { Fill = Brushes.Red };
                this.Annotations.Add(_annotationToAdd);
                SelectedAnnotation = _annotationToAdd;
                _drawing = true;   //Start drawing on MouseMove
                _flexChart.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void UpdateAnnotation(Point p)
        {
            _end = p;
            var rect = new Rect
            {
                X = _start.X,
                Y = _start.Y,
                Width = Math.Abs(_end.X - _start.X),
                Height = Math.Abs(_end.Y - _start.Y)
            };

            var location = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);

            string annoToAdd = _annotationToAdd.GetType().Name;
            switch (annoToAdd)
            {
                case "Text": //Do Nothing
                    break;
                case "Line":
                    var line = _annotationToAdd as Line;
                    line.Start = Helpers.CoordsToAnnoPoint(FlexChart, line, _start);
                    line.End = Helpers.CoordsToAnnoPoint(FlexChart, line, _end);
                    break;
                case "Circle":
                    var circle = _annotationToAdd as Circle;
                    circle.Location = Helpers.CoordsToAnnoPoint(FlexChart, circle, _start);
                    circle.Radius = Helpers.Distance(_start.X, _start.Y, _end.X, _end.Y);
                    break;
                case "Ellipse":
                    var ellipse = _annotationToAdd as Ellipse;
                    ellipse.Location = Helpers.CoordsToAnnoPoint(FlexChart, ellipse, _start);
                    ellipse.Height = rect.Height * 2;
                    ellipse.Width = rect.Width * 2;
                    break;
                case "Rectangle":
                    var rectangle = _annotationToAdd as C1.WPF.Chart.Annotation.Rectangle;
                    rectangle.Location = Helpers.CoordsToAnnoPoint(FlexChart, rectangle, location);
                    rectangle.Height = rect.Height;
                    rectangle.Width = rect.Width;
                    break;
                case "Square":
                    var square = _annotationToAdd as Square;
                    square.Location = Helpers.CoordsToAnnoPoint(FlexChart, square, location);
                    square.Length = Math.Max(rect.Height, rect.Width);
                    break;
                case "Polygon":
                    var polygon = _annotationToAdd as Polygon;
                    PolygonReSizeFunc(polygon, rect);
                    break;
            }
            _flexChart.Invalidate();
        }
        private void SendToFront()
        {
            if (SelectedAnnotation == null)
                return;
            FlexChart.BeginUpdate();
            var oldIndex = Annotations.IndexOf(SelectedAnnotation);
            var newIndex = Annotations.Count - 1;
            this.Annotations.Move(oldIndex, newIndex);
            FlexChart.EndUpdate();
        }
        #endregion
    }

    class AnnotationEx
    {
        internal C1FlexChart flexChart;
        internal AnnotationBase annotation;
        internal Rect BoundingRectangle;

        public AnnotationEx(C1FlexChart chart, AnnotationBase anno, Rect rect)
        {
            this.flexChart = chart;
            this.annotation = anno;
            this.BoundingRectangle = rect;
        }

        public bool HitTest(Point pt, int minDist)
        {
            if (!BoundingRectangle.Contains(pt))
                return false;

            if (annotation is IEllipse)
                return Math.Pow((pt.X - BoundingRectangle.Left - BoundingRectangle.Width / 2), 2) / Math.Pow((annotation as IEllipse).Width / 2 + minDist, 2) + Math.Pow((pt.Y - BoundingRectangle.Top - BoundingRectangle.Height / 2), 2) / Math.Pow((annotation as IEllipse).Height / 2 + minDist, 2) <= 1;
            else if (annotation is ICircle)
                return Math.Pow((pt.X - BoundingRectangle.Left - BoundingRectangle.Width / 2), 2) + Math.Pow((pt.Y - BoundingRectangle.Top - BoundingRectangle.Width / 2), 2) <= Math.Pow((annotation as ICircle).Radius + minDist, 2);
            else if (annotation is ILine)
                return LineHitTest(pt, 5);
            else if (annotation is Polygon)
                return PolygonHitTest(pt);
            else
                return IncreasedRectangleContains(pt, minDist);
        }

        private bool IncreasedRectangleContains(Point pt, int minDist)
        {
            return BoundingRectangle.Left - minDist <= pt.X &&
                   BoundingRectangle.Left + BoundingRectangle.Width + minDist >= pt.X &&
                   BoundingRectangle.Top - minDist <= pt.Y &&
                   BoundingRectangle.Top + BoundingRectangle.Height + minDist >= pt.Y;
        }

        private bool LineHitTest(Point pt, int minDist = 0)
        {
            var line = annotation as Line;
            var start = Helpers.AnnoPointToCoords(flexChart, line, line.Start);
            var end = Helpers.AnnoPointToCoords(flexChart, line, line.End);
            //Horizontal or Vertical lines
            if (start.X == end.X || start.Y == end.Y)
                return BoundingRectangle.Contains(pt);
            else
            {
                var lineWidth = line.Style.StrokeThickness;
                double textHeight = 0;
                if (!string.IsNullOrEmpty(line.Content))
                {
                    TextBlock txtBlock = new TextBlock();
                    txtBlock.FontStyle = line.Style.FontStyle;
                    txtBlock.Text = line.Content;
                    txtBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                    textHeight = txtBlock.DesiredSize.Height;
                }

                return (-pt.X * (start.Y - end.Y) - (start.X * end.Y - end.X * start.Y)) / (end.X - start.X) - (minDist + lineWidth / 2 + textHeight) < pt.Y
    && pt.Y < (-pt.X * (start.Y - end.Y) - (start.X * end.Y - end.X * start.Y)) / (end.X - start.X) + (minDist + lineWidth / 2);
            }
        }

        private bool PolygonHitTest(Point pt, int minDist = 0)
        {
            var polygon = annotation as Polygon;
            var convertedPoints = polygon.Points.Select(x =>
                Helpers.AnnoPointToCoords(flexChart, polygon, x)).ToArray();
            if (IsPointInPolygon(convertedPoints, pt))
                return true;
            var arrayCount = convertedPoints.Count();
            for (var i = 0; i <= arrayCount - 1; i++)
            {
                var firstLinePoint = convertedPoints[i];
                var secondLinePoint = i < arrayCount - 1 ? convertedPoints[i + 1] : convertedPoints[0];
                if (IsPointOnLine(firstLinePoint, secondLinePoint, pt, minDist, polygon.Style.StrokeThickness))
                    return true;
            }
            return false;
        }

        private bool IsPointOnLine(Point start, Point end, Point pt, int minDist = 0, double lineWidth = 1)
        {
            var halfWidth = lineWidth / 2;
            var dist = Helpers.DistanceToSegement(pt.X, pt.Y, start.X, start.Y, end.X, end.Y) - (halfWidth + minDist);
            return dist <= minDist;
        }

        private bool IsPointInPolygon(Point[] points, Point testPoint, int minDist = 0)
        {
            bool result = false;
            int len = points.Length;
            int j = len - 1;
            for (int i = 0; i < len; i++)
            {
                if (points[i].Y < testPoint.Y && points[j].Y >= testPoint.Y || points[j].Y < testPoint.Y && points[i].Y >= testPoint.Y)
                {
                    if (points[i].X + (testPoint.Y - points[i].Y) / (points[j].Y - points[i].Y) * (points[j].X - points[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }

    public class Helpers
    {
        public static Point CoordsToAnnoPoint(C1FlexChart flexChart, IAnnotationBase annotation, Point point)
        {
            Point pt = new Point();
            switch (annotation.Attachment)
            {
                case AnnotationAttachment.Absolute:
                    pt = point;
                    break;
                case AnnotationAttachment.Relative:
                    pt = new Point
                    {
                        X = point.X / (float)flexChart.PlotRect.Width,
                        Y = point.Y / (float)flexChart.PlotRect.Height
                    };
                    break;
                case AnnotationAttachment.DataCoordinate:
                    double x = point.X;
                    double y = point.Y;
                    var xValue = (float)flexChart.AxisX.ConvertBack(x);
                    var yValue = (float)flexChart.AxisY.ConvertBack(y);
                    pt = new Point(xValue, yValue);
                    break;
            }
            return pt;
        }

        public static Point AnnoPointToCoords(C1FlexChart flexChart, IAnnotationBase annotation, Point point)
        {
            Point pt = new Point();
            switch (annotation.Attachment)
            {
                case AnnotationAttachment.Absolute:
                    pt = point;
                    break;
                case AnnotationAttachment.Relative:
                    pt = new Point
                    {
                        X = (float)flexChart.PlotRect.Width * point.X,
                        Y = (float)flexChart.PlotRect.Height * point.Y,
                    };
                    break;
                case AnnotationAttachment.DataCoordinate:
                    pt = new Point
                    {
                        X = (float)flexChart.AxisX.Convert(point.X),
                        Y = (float)flexChart.AxisY.Convert(point.Y),
                    };
                    break;
            }
            return pt;
        }

        internal static Rect GetRect(C1FlexChart flexChart, IAnnotationBase anno)
        {
            var annotaion = anno as AnnotationBase;
            Point point = new Point();
            _Size size;
            Size sz;
            if (annotaion is Line)
                return GetRect(flexChart, annotaion as Line);
            else if (annotaion is Polygon)
                return GetRect(flexChart, annotaion as Polygon);
            else
            {
                size = anno.GetSize();
                sz = new Size((float)size.Width, (float)size.Height);
                if (annotaion is Text)
                {
                    TextBlock txtBlock = new TextBlock();
                    txtBlock.FontStyle = (((Text)annotaion).Style.FontStyle);
                    txtBlock.Text = ((Text)annotaion).Content;
                    txtBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                    sz = txtBlock.DesiredSize;
                }
                Point loc = AnnoPointToCoords(flexChart, annotaion, annotaion.Location);
                point.X = (float)(loc.X - size.Width / 2);
                point.Y = (float)(loc.Y - size.Height / 2);

            }
            return new Rect(point.X, point.Y, sz.Width, sz.Height);
        }

        private static Rect GetRect(C1FlexChart flexChart, Line line)
        {
            var start = AnnoPointToCoords(flexChart, line, line.Start);
            var end = AnnoPointToCoords(flexChart, line, line.End);
            var x = Math.Min(start.X, end.X);
            var y = Math.Min(start.Y, end.Y);
            var width = Math.Abs(start.X - end.X);
            var height = Math.Abs(start.Y - end.Y);

            Size contentSize = new Size();
            if (!string.IsNullOrEmpty(line.Content))
            {
                TextBlock txtBlock = new TextBlock();
                txtBlock.FontStyle = line.Style.FontStyle;
                txtBlock.Text = line.Content;
                txtBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                contentSize = txtBlock.DesiredSize;
            }

            if (height == 0)             //Horizontal line
            {
                height = (float)line.Style.StrokeThickness + contentSize.Height;
                y = y - height;
            }
            else if (width == 0)           //Vertical line
            {
                width = (float)line.Style.StrokeThickness + contentSize.Height;
            }
            return new Rect(x, y, width, height);
        }

        private static Rect GetRect(C1FlexChart flexChart, Polygon polygon)
        {
            Point point = new Point();
            Size sz;
            var convertedPoints = polygon.Points.Select(x =>
                AnnoPointToCoords(flexChart, polygon, x));
            var arrayX = convertedPoints.Select(x => x.X).ToArray();
            var arrayY = convertedPoints.Select(y => y.Y).ToArray();
            point.X = arrayX.Min();
            point.Y = arrayY.Min();
            sz = new Size(arrayX.Max() - arrayX.Min(), arrayY.Max() - arrayY.Min());
            return new Rect(point.X, point.Y, sz.Width, sz.Height);
        }

        internal static double Distance(double x1, double y1, double x2, double y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // distance from point(x,y) to line segment (x1,y1) - (x2,y2)
        internal static double DistanceToSegement(double x, double y, double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(DistanceToSegement2(x, y, x1, y1, x2, y2));
        }

        private static double DistanceToSegement2(double x, double y, double x1, double y1, double x2, double y2)
        {
            var l2 = Distance(x1, y1, x2, y2);
            if (l2 == 0)
                return Distance(x, y, x1, y1);
            var t = ((x - x1) * (x2 - x1) + (y - y1) * (y2 - y1)) / l2;
            if (t < 0)
                return Distance(x, y, x1, y1);
            if (t > 1)
                return Distance(x, y, x2, y2);
            return Distance(x, y, x1 + t * (x2 - x1), y1 + t * (y2 - y1));
        }
    }

    public static class Extensions
    {
        public static Point OffSet(this Point source, Point offSet)
        {
            source.X += offSet.X;
            source.Y += offSet.Y;
            return source;
        }

        public static void SetStyle(this AnnotationBase annotation, ChartStyle style)
        {
            if (annotation == null || annotation.Style == null)
                return;
            annotation.Style.Fill = style.Fill;
            annotation.Style.FontStyle = style.FontStyle;
            annotation.Style.Stroke = style.Stroke;
            annotation.Style.StrokeThickness = style.StrokeThickness;
        }

        public static ChartStyle CloneStyle(this AnnotationBase annotation)
        {
            if (annotation == null || annotation.Style == null)
                return null;

            return new ChartStyle
            {
                Fill = annotation.Style.Fill,
                FontStyle = annotation.Style.FontStyle,
                Stroke = annotation.Style.Stroke,
                StrokeThickness = annotation.Style.StrokeThickness
            };
        }
        public static bool IsEditable(this AnnotationBase anno)
        {
            return anno is Shape || anno is Text || anno is Line;
        }
    }

    public abstract class TextEditor : UserControl
    {
        internal Action Reject, Accept;

        public AnnotationBase Annotation { get; set; }

        public void RejectChanges()
        {
            if (Reject != null)
                Reject();
        }

        public void AcceptChanges(string content)
        {
            if (Accept != null)
                Accept();

            var shape = Annotation as C1.WPF.Chart.Annotation.Shape;
            if (shape != null)
            {
                shape.Content = content;
                return;
            }
            var text = Annotation as Text;
            if (text != null)
            {
                text.Content = content;
                return;
            }

            var line = Annotation as C1.WPF.Chart.Annotation.Line;
            if (line != null)
            {
                line.Content = content;
                return;
            }
        }

        public string GetAnnotationContent()
        {
            var shape = Annotation as C1.WPF.Chart.Annotation.Shape;
            if (shape != null)
                return shape.Content != null ? shape.Content : string.Empty;

            var text = Annotation as Text;
            if (text != null)
                return text.Content != null ? text.Content : string.Empty;

            var line = Annotation as C1.WPF.Chart.Annotation.Line;
            if (line != null)
                return line.Content != null ? line.Content : string.Empty;
            return null;
        }

        public virtual void UpdateEditorContent()
        {
            throw new NotImplementedException("UpdateEditorContent method is not implemented");
        }
    }
}