using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using C1.WPF.Chart.Interaction;

namespace GestureChartSample
{
    class GestureChartDemoModel : INotifyPropertyChanged
    {
        List<DataPoint> _data;
        int _currentPointsCount = 1000;

        public List<DataPoint> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.Create(_currentPointsCount);
                }
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public List<string> GestureMode
        {
            get
            {
                return Enum.GetNames(typeof(GestureMode)).ToList();
            }
        }

        public string Description
        {
            get
            {
                return "1. Zoom Mode: Use pinch gesture, mouse wheel or double click to zoom in/out.\n" +
                       "2. Translation Mode: Use pan gesture or drag the chart area to change the display range.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
