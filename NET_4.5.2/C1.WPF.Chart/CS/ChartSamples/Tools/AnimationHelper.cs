using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using C1.WPF.C1Chart;

namespace ChartSamples
{
  public enum AnimationOrigin
  {
    Center,
    Top,
    Left,
    Bottom,
    Right,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
  }

  public enum Easing
  {
    None,
    BackEase,
    BounceEase,
    CircleEase,
    CubicEase,
    ElasticEase,
    ExponentialEase,
    PowerEase,
    QuadraticEase,
    QuarticEase,
    QuinticEase,
    SineEase
  }

  public enum AnimationTransform
  {
    Scale,
    Rotation
  }

  public static class AnimationHelper
  {
    public static PlotElementAnimation CreateAnimation(AnimationTransform transform, AnimationOrigin origin, Easing easing, bool indexDelay)
    {
      var sb = new Storyboard();
      var duration = new Duration(TimeSpan.FromSeconds(0.5));

      var style = new Style();
      style.TargetType = typeof(PlotElement);
      style.Setters.Add(new Setter(PlotElement.OpacityProperty, 0.0));

      if (transform == AnimationTransform.Scale)
        style.Setters.Add(new Setter(PlotElement.RenderTransformProperty, new ScaleTransform() { ScaleX = 0, ScaleY = 0 }));
      else if (transform == AnimationTransform.Rotation)
        style.Setters.Add(new Setter(PlotElement.RenderTransformProperty, new RotateTransform() { Angle = 180 }));

      var point = new Point(0.5, 0.5);
      switch (origin)
      {
        case AnimationOrigin.Bottom:
          point = new Point(0.5, 2);
          break;
        case AnimationOrigin.Top:
          point = new Point(0.5, -2);
          break;
        case AnimationOrigin.Left:
          point = new Point(-2, 0.5);
          break;
        case AnimationOrigin.Right:
          point = new Point(2, 0.5);
          break;
        case AnimationOrigin.TopLeft:
          point = new Point(2, -2);
          break;
        case AnimationOrigin.TopRight:
          point = new Point(-2, -2);
          break;
        case AnimationOrigin.BottomLeft:
          point = new Point(2, 2);
          break;
        case AnimationOrigin.BottomRight:
          point = new Point(-2, 2);
          break;
        default:
          break;
      }

      style.Setters.Add(new Setter(PlotElement.RenderTransformOriginProperty, point));

      var da = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
      Storyboard.SetTargetProperty(da, new PropertyPath("Opacity"));
      sb.Children.Add(da);

      if (transform == AnimationTransform.Scale)
      {
        var da2 = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
        Storyboard.SetTargetProperty(da2, new PropertyPath("(RenderTransform).ScaleX"));

        var da3 = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
        Storyboard.SetTargetProperty(da3, new PropertyPath("(RenderTransform).ScaleY"));

        sb.Children.Add(da2);
        sb.Children.Add(da3);
      }
      else if (transform == AnimationTransform.Rotation)
      {
        var da2 = new DoubleAnimation() { To = 0, Duration = duration };
        Storyboard.SetTargetProperty(da2, new PropertyPath("(RenderTransform).Angle"));
        sb.Children.Add(da2);
      }

      if (indexDelay)
      {
        foreach (var anim in sb.Children)
          PlotElementAnimation.SetIndexDelay(anim, 0.5);
      }

#if CLR40      
      if (easing != Easing.None)
      {
        IEasingFunction ef = null;

        switch (easing)
        {
          case Easing.BackEase:
            ef = new BackEase(); break;
          case Easing.BounceEase:
            ef = new BounceEase(); break;
          case Easing.CircleEase:
            ef = new CircleEase(); break;
          case Easing.CubicEase:
            ef = new CubicEase(); break;
          case Easing.ElasticEase:
            ef = new ElasticEase(); break;
          case Easing.ExponentialEase:
            ef = new ExponentialEase(); break;
          case Easing.PowerEase:
            ef = new PowerEase(); break;
          case Easing.QuadraticEase:
            ef = new QuadraticEase(); break;
          case Easing.QuarticEase:
            ef = new QuarticEase(); break;
          case Easing.QuinticEase:
            ef = new QuinticEase(); break;
          case Easing.SineEase:
            ef = new SineEase(); break;

          default:
            break;
        }

        foreach (DoubleAnimation anim in sb.Children)
          anim.EasingFunction = ef;
      }
#endif

      return new PlotElementAnimation() { Storyboard = sb, SymbolStyle = style };
    }
  }
}
