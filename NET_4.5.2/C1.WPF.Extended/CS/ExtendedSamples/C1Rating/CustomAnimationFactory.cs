using C1.WPF.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace ExtendedSamples.C1Rating
{
    class CustomAnimationFlickerFactory : IAnimationFactory
    {
        public Storyboard GetForwardAnimation(FrameworkElement target, AnimationType animationType)
        {
            return GetFadeAnimation(target, true);
        }
        public Storyboard GetBackwardAnimation(FrameworkElement target, AnimationType animationType)
        {
            return GetFadeAnimation(target, false);
        }
        private Storyboard GetFadeAnimation(FrameworkElement target, bool forward)
        {
            RotateTransform st = new RotateTransform();
            st.Angle = 360;
            Point p = new Point(0.5, 0.5);
            target.RenderTransformOrigin = p;
            target.RenderTransform = st;
            Storyboard sb = new Storyboard();
            var duration = TimeSpan.FromSeconds(0.25);
            EasingFunctionBase easing = new ElasticEase()
            {
                EasingMode = EasingMode.EaseInOut,
                Oscillations=20,
                Springiness=5
            };
            DoubleAnimation da1 = new DoubleAnimation();
            DoubleAnimation da2 = new DoubleAnimation();

            if (forward)
            {
                da1.From = 1.0;
                da1.To = 0.0;
                da2.To = 1.0;
            }
            else
            {
                da1.From = 0.0;
                da1.To = 1.0;
            }
            da1.Duration = duration;
            da2.Duration = duration;

            da1.EasingFunction = easing;
            da2.EasingFunction = easing;

            sb.Children.Add(da1);
            sb.Children.Add(da2);

            Storyboard.SetTargetProperty(da2, new PropertyPath("RenderTransform.Angle"));
            Storyboard.SetTargetProperty(da1, new PropertyPath("Opacity"));
            return sb;
        }
    
       
    }
    class CustomAnimationRotateFactory : IAnimationFactory
    {
        public Storyboard GetForwardAnimation(FrameworkElement target, AnimationType animationType)
        {
            return GetBlindAnimation(target, true);
        }
        public Storyboard GetBackwardAnimation(FrameworkElement target, AnimationType animationType)
        {
            return GetBlindAnimation(target, false);
        }
        private Storyboard GetBlindAnimation(FrameworkElement target, bool forward)
        {
            RotateTransform st = new RotateTransform();
            st.Angle = 360;
            Point p = new Point(0.5, 0.5);
            target.RenderTransformOrigin = p;
            target.RenderTransform = st;
            Storyboard sb = new Storyboard();
            var duration = TimeSpan.FromSeconds(0.3);
            DoubleAnimation da = new DoubleAnimation();
            if (forward)
            {
                da.To = 1.0;
            }
            else
            {
                da.From = 0.0;
            }
            da.Duration = duration;
            sb.Children.Add(da);
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Angle"));
            return sb;
        }
    }
}
