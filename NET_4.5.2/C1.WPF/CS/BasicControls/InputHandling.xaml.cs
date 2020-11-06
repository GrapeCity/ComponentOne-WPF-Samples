using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using C1.WPF;

namespace BasicControls
{
    public partial class InputHandling : UserControl
    {
        private C1DragHelper _dragHelper;
        private C1ZoomHelper _zoomHelper;
        private C1TapHelper _tapHelper;
        private Random _rand = new Random();

        public InputHandling()
        {
            InitializeComponent();

            _dragHelper = new C1DragHelper(FramePanel, C1DragHelperMode.TranslateXY | C1DragHelperMode.Inertia, captureElementOnPointerPressed: false);
            _dragHelper.DragStarting += OnDragStarting;
            _dragHelper.DragStarted += OnDragStarted;
            _dragHelper.DragDelta += OnDragDelta;
            _dragHelper.DragCompleted += OnDragCompleted;
            _dragHelper.DragInertiaStarted += OnDragInertiaStarted;

            _zoomHelper = new C1ZoomHelper(FramePanel, ctrlRequired: false);
            _zoomHelper.ZoomStarted += OnZoomStarted;
            _zoomHelper.ZoomDelta += OnZoomDelta;
            _zoomHelper.ZoomCompleted += OnZoomCompleted;

            _tapHelper = new C1TapHelper(Rectangle);
            _tapHelper.Tapped += OnTapped;
        }

        #region ** click

        void OnTapped(object sender, C1TappedEventArgs e)
        {
            Rectangle.Background = GetRandomBrush();
        }

        #endregion

        #region ** drag

        double _initialX, _initialY;

        void OnDragStarting(object sender, C1DragStartingEventArgs e)
        {
        }

        void OnDragStarted(object sender, EventArgs e)
        {
            var position = GetPosition();
            _initialX = position.X;
            _initialY = position.Y;
        }

        void OnDragDelta(object sender, C1DragDeltaEventArgs e)
        {
            var zoom = GetZoom();
            var maxX = FramePanel.ActualWidth - Rectangle.ActualWidth * zoom;
            var maxY = FramePanel.ActualHeight - Rectangle.ActualHeight * zoom;
            var newX = _initialX + e.CumulativeTranslation.X;
            var newY = _initialY + e.CumulativeTranslation.Y;
            if (e.IsInertial)
                Bounce(maxX, maxY, ref newX, ref newY);
            else
                Clip(maxX, maxY, ref newX, ref newY);
            SetPosition(new Point(newX, newY));
        }

        void OnDragInertiaStarted(object sender, EventArgs e)
        {
        }

        void OnDragCompleted(object sender, EventArgs e)
        {
            Rectangle.Background = GetRandomBrush();
        }

        #endregion

        #region ** zoom

        double _initialZoom;
        Point _initialPosition;
        Point _relativePosition;

        void OnZoomStarted(object sender, C1ZoomStartedEventArgs e)
        {
            _dragHelper.Complete();
            _initialZoom = GetZoom();
            _initialPosition = GetPosition();
            _relativePosition = e.GetPosition(Rectangle);
        }

        void OnZoomDelta(object sender, C1ZoomDeltaEventArgs e)
        {
            var maxZoom = Math.Min(ActualWidth / Rectangle.ActualWidth, ActualHeight / Rectangle.ActualHeight) / 2;
            var newZoom = Math.Min(maxZoom, _initialZoom * e.UniformCumulativeScale);
            var maxX = FramePanel.ActualWidth - Rectangle.ActualWidth * newZoom;
            var maxY = FramePanel.ActualHeight - Rectangle.ActualHeight * newZoom;
            double newX, newY;
            Zoom(maxZoom, _initialZoom, _initialPosition, _relativePosition, newZoom, out newX, out newY);
            Clip(maxX, maxY, ref newX, ref newY);
            SetZoom(newZoom, newX, newY, false);
        }

        void OnZoomInertiaStarted(object sender, EventArgs e)
        {
        }

        void OnZoomCompleted(object sender, C1ZoomCompletedEventArgs e)
        {
            var maxZoom = Math.Min(ActualWidth / Rectangle.ActualWidth, ActualHeight / Rectangle.ActualHeight) / 2;
            var newZoom = Math.Min(maxZoom, _initialZoom * e.UniformCumulativeScale);
            var maxX = FramePanel.ActualWidth - Rectangle.ActualWidth * newZoom;
            var maxY = FramePanel.ActualHeight - Rectangle.ActualHeight * newZoom;
            double newX, newY;
            Zoom(maxZoom, _initialZoom, _initialPosition, _relativePosition, newZoom, out newX, out newY);
            Clip(maxX, maxY, ref newX, ref newY);
            SetZoom(newZoom, newX, newY, true);
        }

        #endregion

        #region ** implementation

        private SolidColorBrush GetRandomBrush()
        {
            byte[] buffer = new byte[3];
            _rand.NextBytes(buffer);
            return new SolidColorBrush(Color.FromArgb(0xFF, buffer[0], buffer[1], buffer[2]));
        }

        private static void Clip(double maxX, double maxY, ref double newX, ref double newY)
        {
            newX = Math.Max(0, Math.Min(maxX, newX));
            newY = Math.Max(0, Math.Min(maxY, newY));
        }

        private static void Bounce(double maxX, double maxY, ref double newX, ref double newY)
        {
            if (newX < 0)
                newX = (int)(newX / maxX) % 2 == 0 ? -(newX % maxX) : maxX + (newX % maxX);
            else
                newX = (int)(newX / maxX) % 2 == 0 ? newX % maxX : maxX - (newX % maxX);
            if (newY < 0)
                newY = (int)(newY / maxY) % 2 == 0 ? -(newY % maxY) : maxY + (newY % maxY);
            else
                newY = (int)(newY / maxY) % 2 == 0 ? newY % maxY : maxY - (newY % maxY);
        }

        private static void Zoom(double maxZoom, double initialZoom, Point initialPosition, Point relativePosition, double zoom, out double newX, out double newY)
        {
            newX = initialPosition.X + ((relativePosition.X * initialZoom) - (relativePosition.X * zoom));
            newY = initialPosition.Y + ((relativePosition.Y * initialZoom) - (relativePosition.Y * zoom));
        }

        private Point GetPosition()
        {
            var translateTransform = (Rectangle.RenderTransform as TransformGroup).Children[0] as TranslateTransform;
            return new Point(translateTransform.X, translateTransform.Y);
        }

        private void SetPosition(Point position)
        {
            var translateTransform = (Rectangle.RenderTransform as TransformGroup).Children[0] as TranslateTransform;
            translateTransform.X = position.X;
            translateTransform.Y = position.Y;
        }

        private double GetZoom()
        {
            var scaleTransform = (Rectangle.RenderTransform as TransformGroup).Children[1] as ScaleTransform;
            return scaleTransform.ScaleX;
        }

        private void SetZoom(double zoom, double x, double y, bool setRenderAtScale)
        {
            var translateTransform = (Rectangle.RenderTransform as TransformGroup).Children[0] as TranslateTransform;
            var scaleTransform = (Rectangle.RenderTransform as TransformGroup).Children[1] as ScaleTransform;
            scaleTransform.ScaleX = zoom;
            scaleTransform.ScaleY = zoom;
            translateTransform.X = x;
            translateTransform.Y = y;
        }

        #endregion
    }
}
