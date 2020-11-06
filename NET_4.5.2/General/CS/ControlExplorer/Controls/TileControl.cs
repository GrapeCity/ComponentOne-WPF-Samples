using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ControlExplorer
{
    public class TileControl : Control
    {
        private Storyboard storyboard = new Storyboard();
        private DoubleAnimation scaleAnimationX = new DoubleAnimation();
        private DoubleAnimation scaleAnimationY = new DoubleAnimation();
        private static double SCALE = 0.9;
        private static double SPRINGINESS = 0.5;
        private bool _begin = false;

        static TileControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TileControl),
                new FrameworkPropertyMetadata(typeof(TileControl)));
        }

        public TileControl()
        {
            RenderTransform = new ScaleTransform();
            RenderTransformOrigin = new Point(0.5, 0.5);

            Storyboard.SetTargetProperty(scaleAnimationX, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTargetProperty(scaleAnimationY, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(scaleAnimationX, this);
            Storyboard.SetTarget(scaleAnimationY, this);

            // add cool elastic ease effect
            var ef1 = new ElasticEase();
            ef1.Oscillations = 1;
            ef1.Springiness = SPRINGINESS;
            ef1.EasingMode = EasingMode.EaseOut;
            scaleAnimationX.EasingFunction = ef1;
            scaleAnimationX.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            // add cool elastic ease effect
            var ef2 = new ElasticEase();
            ef2.Oscillations = 1;
            ef2.Springiness = SPRINGINESS;
            ef2.EasingMode = EasingMode.EaseOut;
            scaleAnimationY.EasingFunction = ef2;
            scaleAnimationY.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            storyboard.Children.Add(scaleAnimationX);
            storyboard.Children.Add(scaleAnimationY);
        }

        public DataTemplate IconTemplate
        {
            get
            {
                return (DataTemplate)GetValue(IconTemplateProperty);
            }
            set
            {
                SetValue(IconTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(TileControl),
            new PropertyMetadata(null, (d, a) => { }));

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(TileControl),
            new PropertyMetadata(double.NaN));

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(TileControl),
            new PropertyMetadata(double.NaN));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TileControl), null);

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            try
            {
                if (_begin)
                    storyboard.Pause();
                scaleAnimationX.From = 1;
                scaleAnimationX.To = SCALE;
                scaleAnimationY.From = 1;
                scaleAnimationY.To = SCALE;

                storyboard.Begin();
                _begin = true;
            }
            catch { }
        }
        
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            try
            {
                if (_begin)
                    storyboard.Pause();
                scaleAnimationX.From = SCALE;
                scaleAnimationX.To = 1;
                scaleAnimationY.From = SCALE;
                scaleAnimationY.To = 1;

                storyboard.Begin();
                _begin = true;
            }
            catch { }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (CaptureMouse())
            {
                storyboard.Pause();
                scaleAnimationX.From = 1;
                scaleAnimationX.To = SCALE * 0.8;
                scaleAnimationY.From = 1;
                scaleAnimationY.To = SCALE * 0.8;
                storyboard.Begin();
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            storyboard.Completed += new EventHandler(OnClickStoryboardCompleted);
            storyboard.Pause();
            scaleAnimationX.From = SCALE * 0.8;
            scaleAnimationX.To = 1;
            scaleAnimationY.From = SCALE * 0.8;
            scaleAnimationY.To = 1;
            storyboard.Begin();
        }

        void OnClickStoryboardCompleted(object sender, EventArgs e)
        {
            storyboard.Completed -= new EventHandler(OnClickStoryboardCompleted);
            if (Click != null)
                Click(this, new EventArgs());
        }

        public event EventHandler Click;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
