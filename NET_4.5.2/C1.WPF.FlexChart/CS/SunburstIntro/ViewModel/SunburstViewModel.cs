using C1.Chart;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace SunburstIntro
{
    public class SunburstViewModel
    {
        public List<DataItem> HierarchicalData
        {
            get
            {
                return DataService.CreateHierarchicalData();
            }
        }

        public ICollectionView View
        {
            get
            {
                return DataService.CreateGroupCVData();
            }
        }

        public List<FlatDataItem> FlatData
        {
            get
            {
                return DataService.CreateFlatData();
            }
        }

        public List<string> Positions
        {
            get
            {
                return Enum.GetNames(typeof(Position)).ToList();
            }
        }

        public List<string> Palettes
        {
            get
            {
                return Enum.GetNames(typeof(Palette)).ToList();
            }
        }

        public List<string> LabelPositions
        {
            get
            {
                return Enum.GetNames(typeof(PieLabelPosition)).ToList();
            }
        }

        public List<string> LabelOverlapping
        {
            get
            {
                return Enum.GetNames(typeof(PieLabelOverlapping)).ToList();
            }
        }
    }
}
