using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using FlexChartExplorer.Resources;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PieIntroduction : UserControl
    {
        List<FruitDataItem> _data;
        List<string> _palettes;
        List<string> _labelPositions;

        public PieIntroduction()
        {
            this.InitializeComponent();
            Tag = AppResources.PieIntroductionTag;
        }

        public List<FruitDataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateFruit();
                }
                return _data;
            }
        }

        public List<double> Radius
        {
            get
            {
                return new List<double>() { 0, 0.25, 0.50, 0.75};
            }
        }

        public List<double> Offsets
        {
            get
            {
                return new List<double>() { 0, 0.1, 0.2, 0.3, 0.4, 0.5 };
            }
        }

        public List<double> StartAngles
        {
            get
            {
                return new List<double>() { 0, 90, 180, -90 };
            }
        }

        public List<string> Palettes
        {
            get
            {
                if (_palettes == null)
                {
                    _palettes = Enum.GetNames(typeof(Palette)).ToList<string>();
                    _palettes.Remove("Custom");
                }
                return _palettes;
            }
        }

        public List<string> LabelPositions
        {
            get
            {
                if (_labelPositions == null)
                {
                    _labelPositions = Enum.GetNames(typeof(PieLabelPosition)).ToList<string>();
                }
                return _labelPositions;
            }
        }

        public List<bool> LabelsBorders
        {
            get
            {
                return new List<bool>(){ false, true };
            }
        }
    }
}
