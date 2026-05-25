using C1.WPF.ListView;
using GaugesDemo.Resources;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GaugesDemo
{
    public partial class GaugeSamples : Window
    {
        public GaugeSamples()
        {
            InitializeComponent();
            DataContext = GetSamples();
            Title = "GaugesDemo";
        }

        private List<Sample> GetSamples()
        {
            return new List<Sample>
            {
                new Sample() { Name = AppResources.GettingStartedTitle, Description = AppResources.GettingStartedDescription, SampleViewType = 1 , Thumbnail="gauge_basic.png"},
                new Sample() { Name = AppResources.DisplayingValuesTitle, Description = AppResources.DisplayingValuesDescription, SampleViewType = 2 , Thumbnail="gauge_radial.png"},
                new Sample() { Name = AppResources.UsingRangesTitle, Description = AppResources.UsingRangesDescription, SampleViewType = 3 , Thumbnail="gauge_ranges.png"},
                new Sample() { Name = AppResources.AutomaticScalingTitle, Description = AppResources.AutomaticScalingDescription, SampleViewType = 4 , Thumbnail="gauge_scaling.png"},
                new Sample() { Name = AppResources.DirectionTitle, Description = AppResources.DirectionDescription, SampleViewType = 5 , Thumbnail="gauge_linear.png"},
                new Sample() { Name = AppResources.BulletGraphTitle, Description = AppResources.BulletGraphDescription, SampleViewType = 6 , Thumbnail="gauge_bullet.png"},
                new Sample() { Name = AppResources.MarksAndLabelsTitle, Description = AppResources.MarksAndLabelsDescription, SampleViewType = 7 , Thumbnail="gauge_radial.png"},
                new Sample() { Name = AppResources.PointerTitle, Description = AppResources.PointerDescription, SampleViewType = 8 , Thumbnail="gauge_radial.png"},
            };
        }


        private void OnSelectionChanged(object sender, C1.WPF.Core.SelectionChangedEventArgs<int> e)
        {
            try
            {
                listView.IsEnabled = false;
                var sample = (sender as C1ListView)?.SelectedItem as Sample;
                if (sample == null)
                    return;
                var sampleControl = GetSample(sample.SampleViewType);
                samplePanel.Content = sampleControl;
                lblCaption.Text = sample.Name;
                lblDescr.Text = sampleControl?.Tag as string;
            }
            finally
            {
                listView.IsEnabled = true;
            }
        }

        private UserControl GetSample(int sampleViewType)
        {
            switch (sampleViewType)
            {
                case 1: return new GettingStarted();
                case 2: return new DisplayingValues();
                case 3: return new UsingRanges();
                case 4: return new AutomaticScaling();
                case 5: return new Direction();
                case 6: return new BulletGraph();
                case 7: return new MarksAndLabels();
                case 8: return new Pointer();
            }
            return null;
        }
    }
}
