using C1.WPF.Sparkline;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Themes
{
    class SampleData
    {
        Dictionary<string, Color> _defaultColors;
        Dictionary<string, SparklineType> _types;

        public List<double> DefaultData
        {
            get
            {
                List<double> data = new List<double>() { 1.0, -2.0, -1.0, 6.0, 4.0, -4.0, 3.0, 8.0 };
                return data;
            }
        }

        public Dictionary<string, Color> DefaultColors
        {
            get
            {
                if (_defaultColors == null)
                {
                    _defaultColors = new Dictionary<string, Color>();
                    _defaultColors.Add("Sky Blue", Color.FromArgb(0xFF, 0x88, 0xBD, 0xE6));
                    _defaultColors.Add("Sandy Brown", Color.FromArgb(0xFF, 0xFB, 0xB2, 0x58));
                    _defaultColors.Add("Light Green", Color.FromArgb(0xFF, 0x90, 0xCD, 0x97));
                    _defaultColors.Add("Light Pink", Color.FromArgb(0xFF, 0xF6, 0xAA, 0xC9));
                    _defaultColors.Add("Dark Khaki", Color.FromArgb(0xFF, 0xBF, 0xA5, 0x54));
                    _defaultColors.Add("Medium Orchid", Color.FromArgb(0xFF, 0xBC, 0x99, 0xC7));
                    _defaultColors.Add("Gold", Color.FromArgb(0xFF, 0xED, 0xDD, 0x46));
                    _defaultColors.Add("Light Coral", Color.FromArgb(0xFF, 0xF0, 0x7E, 0x6E));
                    _defaultColors.Add("Gray", Color.FromArgb(0xFF, 0x8C, 0x8C, 0x8C));
                }

                return _defaultColors;
            }
        }

        public Dictionary<string, SparklineType> SparklineTypes
        {
            get
            {
                if (_types == null)
                {
                    _types = new Dictionary<string, SparklineType>();
                    _types.Add("Line", SparklineType.Line);
                    _types.Add("Column", SparklineType.Column);
                    _types.Add("Winloss", SparklineType.Winloss);
                }

                return _types;
            }
        }
    }
}
