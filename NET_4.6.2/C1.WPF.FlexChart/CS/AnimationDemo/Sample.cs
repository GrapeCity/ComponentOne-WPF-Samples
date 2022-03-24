using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;

namespace AnimationDemo
{
    public class Sample
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public UserControl Demo { get; set; }
    }

    public class SamplesDataSource
    {
        List<Sample> _samples;

        public List<Sample> Samples
        {
            get
            {
                if (_samples == null)
                {
                    _samples = new List<Sample>()
                    {
                        new Sample() {
                            Title = "FlexChart",
                            Description = "Shows advanced animation options for C1FlexChart control.",
                            Demo = new Views.FlexChartAnimation()
                        },
                        new Sample() {
                            Title = "FlexPie",
                            Description = "Shows advanced animation options for C1FlexPie control.",
                            Demo = new Views.FlexPieAnimation()
                        },
                        new Sample() {
                            Title = "Custom Animation",
                            Description = "Shows how to customize animation with AnimationTransform event.",
                            Demo = new Views.CustomAnimation()
                        },
                    };
                }

                return _samples;
            }
        }
    }
}
