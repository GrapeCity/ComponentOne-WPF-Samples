using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using C1.WPF.Core;
using CoreExplorer.Resources;

namespace CoreExplorer
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
            Tag = AppResources.Tag;

            _dragHelper = new C1DragHelper(FramePanel, C1DragHelperMode.TranslateXY | C1DragHelperMode.Inertia, captureElementOnPointerPressed: false);
            _dragHelper.DragStarted += OnDragStarted;
            _dragHelper.DragDelta += OnDragDelta;
            _dragHelper.DragCompleted += OnDragCompleted;
            _dragHelper.DragInertiaStarted += OnDragInertiaStarted;

            _zoomHelper = new C1ZoomHelper(FramePanel, ctrlRequired: false);
            _zoomHelper.ZoomStarted += OnZoomStarted;
            _zoomHelper.ZoomDelta += OnZoomDelta;
            _zoomHelper.ZoomCompleted += OnZoomCompleted;

            _tapHelper = new C1TapHelper(FramePanel);
            _tapHelper.Tapped += OnTapped;
            _tapHelper.DoubleTapped += OnDoubleTapped;
            _tapHelper.Holding += OnHolding;
            _tapHelper.ManipulationStarted += OnManipulationStarted;
            _tapHelper.ManipulationCompleted += OnManipulationCompleted;

            Loaded += OnLoaded;
            FramePanel.SizeChanged += OnFrameSizeChagned;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            SetPosition(new Point((FramePanel.ActualWidth - Rectangle.ActualWidth) / 2, (FramePanel.ActualHeight - Rectangle.ActualHeight) / 2));
        }

        private void OnFrameSizeChagned(object sender, SizeChangedEventArgs e)
        {
            var zoom = GetZoom();
            var position = GetPosition();
            var maxX = FramePanel.GetActualWidth() - Rectangle.GetActualWidth() * zoom;
            var maxY = FramePanel.GetActualHeight() - Rectangle.GetActualHeight() * zoom;
            var newX = position.X;
            var newY = position.Y;
            Clip(maxX, maxY, ref newX, ref newY);
            SetPosition(new Point(newX, newY));
        }

        #region ** click

        private void OnManipulationStarted(object sender, C1InputEventArgs e)
        {
            FramePanel.Background = new SolidColorBrush(Color.FromArgb(255, 250,250,250));
        }

        private void OnManipulationCompleted(object sender, C1InputEventArgs e)
        {
            FramePanel.Background = new SolidColorBrush(Colors.White);
        }

        void OnTapped(object sender, C1TappedEventArgs e)
        {
            byte[] buffer = new byte[3];
            _rand.NextBytes(buffer);
            Rectangle.Background = new SolidColorBrush(Color.FromArgb(0xFF, buffer[0], buffer[1], buffer[2]));
        }

        void OnDoubleTapped(object sender, C1TappedEventArgs e)
        {
            Rectangle.BorderBrush = Rectangle.Background;
        }

        private void OnHolding(object sender, C1TappedEventArgs e)
        {
            Rectangle.BorderBrush = new SolidColorBrush(Colors.Orange);
        }

        #endregion

        #region ** drag

        double _initialX, _initialY;

        void OnDragStarted(object sender, C1DragStartedEventArgs e)
        {
            var position = GetPosition();
            _initialX = position.X;
            _initialY = position.Y;
            Rectangle.BorderBrush = new SolidColorBrush(Colors.GreenYellow);
        }

        void OnDragDelta(object sender, C1DragDeltaEventArgs e)
        {
            var zoom = GetZoom();
            var maxX = FramePanel.GetActualWidth() - Rectangle.GetActualWidth() * zoom;
            var maxY = FramePanel.GetActualHeight() - Rectangle.GetActualHeight() * zoom;
            var newX = _initialX + e.CumulativeTranslation.X;
            var newY = _initialY + e.CumulativeTranslation.Y;
            if (e.IsInertial)
                Bounce(maxX, maxY, ref newX, ref newY);
            else
                Clip(maxX, maxY, ref newX, ref newY);
            SetPosition(new Point((int)newX, (int)newY));
        }

        void OnDragInertiaStarted(object sender, C1DragInertiaStartedEventArgs e)
        {
        }

        void OnDragCompleted(object sender, C1DragCompletedEventArgs e)
        {
            Rectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
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
            Rectangle.BorderBrush = new SolidColorBrush(Colors.YellowGreen);
        }

        void OnZoomDelta(object sender, C1ZoomDeltaEventArgs e)
        {
            var maxZoom = Math.Min(FramePanel.GetActualWidth() / Rectangle.GetActualWidth(), FramePanel.GetActualHeight() / Rectangle.GetActualHeight()) / 2;
            var newZoom = Math.Min(maxZoom, _initialZoom * e.UniformCumulativeScale);
            var maxX = FramePanel.GetActualWidth() - Rectangle.GetActualWidth() * newZoom;
            var maxY = FramePanel.GetActualHeight() - Rectangle.GetActualHeight() * newZoom;
            double newX, newY;
            Zoom(maxZoom, _initialZoom, _initialPosition, _relativePosition, newZoom, out newX, out newY);
            newX = newX + e.CumulativeTranslation.X;
            newY = newY + e.CumulativeTranslation.Y;
            Clip(maxX, maxY, ref newX, ref newY);
            SetZoom(newZoom, newX, newY, false);
        }

        void OnZoomInertiaStarted(object sender, EventArgs e)
        {
        }

        void OnZoomCompleted(object sender, C1ZoomCompletedEventArgs e)
        {
            var maxZoom = Math.Min(FramePanel.GetActualWidth() / Rectangle.GetActualWidth(), FramePanel.GetActualHeight() / Rectangle.GetActualHeight()) / 2;
            var newZoom = Math.Min(maxZoom, _initialZoom * e.UniformCumulativeScale);
            var maxX = FramePanel.GetActualWidth() - Rectangle.GetActualWidth() * newZoom;
            var maxY = FramePanel.GetActualHeight() - Rectangle.GetActualHeight() * newZoom;
            double newX, newY;
            Zoom(maxZoom, _initialZoom, _initialPosition, _relativePosition, newZoom, out newX, out newY);
            newX = newX + e.CumulativeTranslation.X;
            newY = newY + e.CumulativeTranslation.Y;
            Clip(maxX, maxY, ref newX, ref newY);
            SetZoom(newZoom, newX, newY, true);
            Rectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        #endregion

        #region ** implementation

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
            return new Point((int)Rectangle.GetRenderTranslateX(), (int)Rectangle.GetRenderTranslateY());
        }

        private void SetPosition(Point position)
        {
            Rectangle.SetRenderTranslate(position.X, position.Y);
        }

        private double GetZoom()
        {
            return Rectangle.GetRenderScaleX();
        }

        private void SetZoom(double zoom, double x, double y, bool setRenderAtScale)
        {
            Rectangle.SetRenderScale(zoom, zoom);
            Rectangle.SetRenderTranslate(x, y);
        }

        #endregion
    }

    public static class FrameworkElementEx
    {
        internal static double GetRenderTranslateX(this FrameworkElement elem)
        {
            var transform = elem.RenderTransform.GetTransform<TranslateTransform>();
            if (transform != null)
            {
                return transform.X;
            }
            return 0.0;
        }

        internal static double GetRenderTranslateY(this FrameworkElement elem)
        {
            var transform = elem.RenderTransform.GetTransform<TranslateTransform>();
            if (transform != null)
            {
                return transform.Y;
            }
            return 0.0;
        }

        internal static void SetRenderTranslateX(this FrameworkElement elem, double translateX)
        {
            var transform = elem.RenderTransform.GetTransform<TranslateTransform>();
            if (transform != null)
            {
                transform.X = translateX;
            }
        }

        internal static void SetRenderTranslateY(this FrameworkElement elem, double translateY)
        {
            var transform = elem.RenderTransform.GetTransform<TranslateTransform>();
            if (transform != null)
            {
                transform.Y = translateY;
            }
        }

        internal static void SetRenderTranslate(this FrameworkElement elem, double translateX, double translateY)
        {
            var transform = elem.RenderTransform.GetTransform<TranslateTransform>();
            if (transform != null)
            {
                transform.X = translateX;
                transform.Y = translateY;
            }
        }

        internal static double GetRenderScaleX(this FrameworkElement elem)
        {
            var transform = elem.RenderTransform.GetTransform<ScaleTransform>();
            if (transform != null)
            {
                return transform.ScaleX;
            }
            return 1.0;
        }

        internal static double GetRenderScaleY(this FrameworkElement elem)
        {
            var transform = elem.RenderTransform.GetTransform<ScaleTransform>();
            if (transform != null)
            {
                return transform.ScaleY;
            }
            return 1.0;
        }

        internal static void SetRenderScaleX(this FrameworkElement elem, double scaleX)
        {
            var transform = elem.RenderTransform.GetTransform<ScaleTransform>();
            if (transform != null)
            {
                transform.ScaleX = scaleX;
            }
        }

        internal static void SetRenderScaleY(this FrameworkElement elem, double scaleY)
        {
            var transform = elem.RenderTransform.GetTransform<ScaleTransform>();
            if (transform != null)
            {
                transform.ScaleY = scaleY;
            }
        }

        internal static void SetRenderScale(this FrameworkElement elem, double uniformScale)
        {
            elem.SetRenderScale(uniformScale, uniformScale);
        }

        internal static void SetRenderScale(this FrameworkElement elem, double scaleX, double scaleY)
        {
            var transform = elem.RenderTransform.GetTransform<ScaleTransform>();
            if (transform != null)
            {
                transform.ScaleX = scaleX;
                transform.ScaleY = scaleY;
            }
        }

        internal static double GetActualWidth(this FrameworkElement view)
        {
            return view.ActualWidth;
        }

        internal static double GetActualHeight(this FrameworkElement view)
        {
            return view.ActualHeight;
        }

        private static T GetTransform<T>(this Transform transform) where T : Transform
        {
            var t = transform as T;
            if (t != null)
                return t;
            var transformGroup = transform as TransformGroup;
            if (transformGroup != null)
            {
                return transformGroup.Children.OfType<T>().FirstOrDefault();
            }
            return null;
        }
    }
}
