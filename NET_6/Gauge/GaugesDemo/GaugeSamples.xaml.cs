using GaugesDemo.Resources;
using System;
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
            };
        }

        private void OnSelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                listView.IsEnabled = false;
                var sample = (sender as ListView)?.SelectedItem as Sample;
                if (sample == null)
                    return;
                var sampleControl = GetSample(sample.SampleViewType);
                samplePanel.Content = sampleControl;
                lblCaption.Text = sampleControl?.Tag as string;
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
            }
            return null;
        }
    }
}
