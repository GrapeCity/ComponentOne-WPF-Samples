using C1.Chart;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexRadarIntro
{
    public class FlexRadarViewModel
    {
        int n = 6;
        Random rnd = new Random();
        List<DataItem> _pts1;
        List<DataItem> _pts2;

        public List<DataItem> Points1
        {
            get
            {
                if (_pts1 == null)
                {
                    _pts1 = new List<DataItem>();
                    for (var i = 0; i < n; i++)
                    {
                        _pts1.Add(new DataItem() { Name = "Item" + i.ToString(), Value = rnd.Next(100) });
                    }
                }

                return _pts1;
            }
        }

        public List<DataItem> Points2
        {
            get
            {
                if (_pts2 == null)
                {
                    _pts2 = new List<DataItem>();
                    for (var i = 0; i < n; i++)
                    {
                        _pts2.Add(new DataItem() { Name = "Item" + i.ToString(), Value = rnd.Next(50) });
                    }
                }

                return _pts2;
            }
        }

        public List<string> ChartTypes
        {
            get
            {
                return Enum.GetNames(typeof(RadarChartType)).ToList();
            }
        }

        public List<string> Stackings
        {
            get
            {
                return Enum.GetNames(typeof(Stacking)).ToList();
            }
        }

        public List<string> Palettes
        {
            get
            {
                return Enum.GetNames(typeof(Palette)).ToList();
            }
        }
    }
}
