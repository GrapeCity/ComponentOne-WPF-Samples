using GaugesDemo.Resources;
using System;
using System.Windows.Controls;


namespace GaugesDemo
{
    public partial class Direction : UserControl
    {
        public Direction()
        {
            InitializeComponent();
            Tag = AppResources.DirectionDescription;
            this.lblDir.Content = AppResources.Direction;
            DataContext = new SampleViewModel() { Value = 80 };
        }
    }
}
