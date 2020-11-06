using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using C1.WPF.C1Chart;

namespace ChartSamples
{
    /// <summary>
    /// Interaction logic for ChartThemes.xaml
    /// </summary>
    public partial class ChartThemes : UserControl
    {
        public ChartThemes()
        {
            InitializeComponent();
        }

        private void palettes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
      // animate palette changing
            Storyboard sb = new Storyboard();

            DoubleAnimation da1 = new DoubleAnimation();
            da1.From = 0; da1.To = 1; da1.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            da1.BeginTime = TimeSpan.FromSeconds(0.1);
            Storyboard.SetTargetProperty(da1, new PropertyPath("Tag.GradientStops[1].Offset"));
            sb.Children.Add(da1);

            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 0; da2.To = 1; da2.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            Storyboard.SetTargetProperty(da2, new PropertyPath("Tag.GradientStops[2].Offset"));
            sb.Children.Add(da2);

      sb.Completed += (s, a) =>
        {
            if (c1Chart1 != null && c1Chart1.IsLoaded)
            {
                c1Chart1.Palette = (ColorGeneration)palettes.SelectedItem;
                AnimateMask();
            }
        };
      sb.Begin(palettes);
        }


        void AnimateMask()
        {
            if (palettes == null || !palettes.IsLoaded)
                return;

            Storyboard sb = new Storyboard();
            DoubleAnimation da1 = new DoubleAnimation();
            da1.From = 1; da1.To = 0; da1.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            Storyboard.SetTargetProperty(da1, new PropertyPath("Tag.GradientStops[1].Offset"));
            sb.Children.Add(da1);
            DoubleAnimation da2 = new DoubleAnimation();
            da2.BeginTime = TimeSpan.FromSeconds(0.1);
            da2.From = 1; da2.To = 0; da2.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            Storyboard.SetTargetProperty(da2, new PropertyPath("Tag.GradientStops[2].Offset"));
            sb.Children.Add(da2);
            sb.Begin(palettes);
        }

        private void PlotElement_Loaded(object sender, RoutedEventArgs e)
        {
            PlotElement pe = (PlotElement)sender;
            pe.OpacityMask = (Brush)FindResource("lgb");
        }

        private void themes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (c1Chart1 == null || !c1Chart1.IsLoaded)
                return;

      // animate theme changing
            Storyboard sb = new Storyboard();
            DoubleAnimation da1 = new DoubleAnimation();
            da1.From = 0; da1.To = 1.5; da1.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            Storyboard.SetTargetProperty(da1, new PropertyPath("OpacityMask.GradientStops[1].Offset"));
            sb.Children.Add(da1);

            DoubleAnimation da2 = new DoubleAnimation();
            da1.BeginTime = TimeSpan.FromSeconds(0.2);
            da2.From = 0; da2.To = 1.5; da2.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            Storyboard.SetTargetProperty(da2, new PropertyPath("OpacityMask.GradientStops[2].Offset"));
            sb.Children.Add(da2);

      sb.Completed += new EventHandler(sbChart_Completed);
            sb.Begin(c1Chart1);
        }

        void sbChart_Completed(object sender, EventArgs e)
        {
            if (c1Chart1 != null && c1Chart1.IsLoaded)
            {
                c1Chart1.Theme = ((ListBoxItem)themes.SelectedItem).Tag as ResourceDictionary;

                Storyboard sb = new Storyboard();

                DoubleAnimation da1 = new DoubleAnimation();
                da1.From = 1.5; da1.To = 0; da1.Duration = new Duration(TimeSpan.FromSeconds(0.4));
                Storyboard.SetTargetProperty(da1, new PropertyPath("OpacityMask.GradientStops[1].Offset"));
                sb.Children.Add(da1);

                DoubleAnimation da2 = new DoubleAnimation();
                da2.BeginTime = TimeSpan.FromSeconds(0.2);
                da2.From = 1.5; da2.To = 0; da2.Duration = new Duration(TimeSpan.FromSeconds(0.4));
                Storyboard.SetTargetProperty(da2, new PropertyPath("OpacityMask.GradientStops[2].Offset"));
                sb.Children.Add(da2);

                sb.Begin(c1Chart1);
            }
        }
    }

  // fading oscillation animation
    public class OscDoubleAnimation : DoubleAnimationBase
    {
        public static readonly DependencyProperty OscNumberProperty =
            DependencyProperty.Register("OscNumber",
            typeof(Double),
            typeof(OscDoubleAnimation), new PropertyMetadata(1.0));

        public static readonly DependencyProperty AttenuationProperty =
            DependencyProperty.Register("Attenuation",
            typeof(Double?),
            typeof(OscDoubleAnimation));

        public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register("From",
            typeof(Double?),
            typeof(OscDoubleAnimation));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To",
            typeof(Double?),
            typeof(OscDoubleAnimation));

        public static readonly DependencyProperty AmplitudeProperty =
            DependencyProperty.Register("Amplitude",
            typeof(Double?),
            typeof(OscDoubleAnimation));

        public double? From
        {
            get { return (double?)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public double? To
        {
            get { return (double?)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public double? Amplitude
        {
            get { return (double?)GetValue(AmplitudeProperty); }
            set { SetValue(AmplitudeProperty, value); }
        }

        public double OscNumber
        {
            get { return (double)GetValue(OscNumberProperty); }
            set { SetValue(OscNumberProperty, value); }
        }

        public double? Attenuation
        {
            get { return (double?)GetValue(AttenuationProperty); }
            set { SetValue(AttenuationProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new OscDoubleAnimation();
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            double from = From ?? defaultOriginValue;
            double to = To ?? defaultDestinationValue;

            double time = animationClock.CurrentProgress.Value;
            
            double delta = Amplitude ?? to - from;

            if (Attenuation != null)
                delta *= Math.Exp(-(double)Attenuation * time);

            double val = to + delta * Math.Sin(time * 2 * Math.PI * OscNumber);

            return val;
        }
    }
}
