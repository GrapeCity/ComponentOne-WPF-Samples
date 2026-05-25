using C1.WPF.Gauge;
using GaugesDemo.Resources;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GaugesDemo
{
    public partial class GettingStarted : UserControl
    {
        public GettingStarted()
        {
            InitializeComponent();
            Tag = AppResources.GettingStartedDescription;
            DataContext = new SampleViewModel() { Value = 25, ShowText = GaugeTextVisibility.None, IsReadOnly = false };
            (Resources["AnimateGauges"] as Storyboard)?.Begin();
        }

        private bool IsAnimating
        {
            get
            {
                return !(Resources["AnimateGauges"] as Storyboard)?.GetIsPaused() ?? false;
            }
        }

        private void StartAnimation()
        {
            AnimationButton.Content = AppResources.PauseAnimationLabel;
            (Resources["AnimateGauges"] as Storyboard)?.Resume();
        }
        private void StopAnimation()
        {
            AnimationButton.Content = AppResources.ResumeAnimationLabel;
            (Resources["AnimateGauges"] as Storyboard)?.Pause();
        }

        private void AnimationButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsAnimating)
                StopAnimation();
            else
                StartAnimation();
        }
    }
}
