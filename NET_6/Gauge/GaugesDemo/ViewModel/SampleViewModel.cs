using C1.WPF.Gauge;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace GaugesDemo
{
    public class SampleViewModel : INotifyPropertyChanged
    {
        double _min = 0;
        double _max = 100;
        double _value;
        double _origin = double.NaN;
        double _step = 1;
        string _format;
        bool _showRanges;
        bool _autoScale;
        double _startAngle = 0;
        double _sweepAngle = 180;
        double _bad = 45;
        double _good = 70;
        double _target = 80;
        int _showTextSelectedIndex;
        int _directionSelectedIndex;
        bool _isReadOnly = false;
        GaugeTextVisibility _showText;

        public double Min
        {
            get { return _min; }
            set
            {
                _min = value;
                RaisePropertyChanged("Min");
            }
        }
        public double Max
        {
            get { return _max; }
            set
            {
                _max = value;
                RaisePropertyChanged("Max");
            }
        }
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
                RaisePropertyChanged("ValueSelectedIndex");
            }
        }
        public double Origin { get { return double.IsNaN(_origin) ? 0 : _origin; } set { _origin = value; RaisePropertyChanged("Origin"); } }
        public double Step { get { return _step; } set { _step = value; RaisePropertyChanged("Step"); } }
        public string Format { get { return _format; } set { _format = value; RaisePropertyChanged("Format"); } }
        public bool ShowRanges { get { return _showRanges; } set { _showRanges = value; RaisePropertyChanged("ShowRanges"); } }
        public double StartAngle { get { return _startAngle; } set { _startAngle = value; RaisePropertyChanged("StartAngle"); } }
        public double SweepAngle { get { return _sweepAngle; } set { _sweepAngle = value; RaisePropertyChanged("SweepAngle"); } }
        public double Bad { get { return _bad; } set { _bad = value; RaisePropertyChanged("Bad"); } }
        public double Good { get { return _good; } set { _good = value; RaisePropertyChanged("Good"); } }
        public double Target { get { return _target; } set { _target = value; RaisePropertyChanged("Target"); } }
        public bool AutoScale { get { return _autoScale; } set { _autoScale = value; RaisePropertyChanged("AutoScale"); } }
        public bool IsReadOnly { get { return _isReadOnly; } set { _isReadOnly = value; RaisePropertyChanged("IsReadOnly"); } }
        public GaugeTextVisibility ShowText { get { return _showText; } set { _showText = value; RaisePropertyChanged("ShowText"); } }
        public IList<string> ShowTextItems
        {
            get
            {
                return new List<string>
                {
                    GaugeTextVisibility.All.ToString(),
                    GaugeTextVisibility.MinMax.ToString(),
                    GaugeTextVisibility.Value.ToString(),
                    GaugeTextVisibility.None.ToString(),
                };
            }
        }
        public IList<string> DirectionItems
        {
            get
            {
                return new List<string> 
                { 
                    LinearGaugeDirection.Right.ToString(),
                    LinearGaugeDirection.Left.ToString(),
                    LinearGaugeDirection.Down.ToString(),
                    LinearGaugeDirection.Up.ToString(),
                };
            }
        }
        public LinearGaugeDirection Direction
        {
            get
            {
                return (LinearGaugeDirection)Enum.Parse(typeof(LinearGaugeDirection), DirectionItems[DirectionSelectedIndex]);
            }
        }

        public Orientation DirectionOrientation
        {
            get
            {
                return DirectionSelectedIndex > 1 ? Orientation.Horizontal : Orientation.Vertical;
            }
        }

        public int DirectionSelectedIndex { get { return _directionSelectedIndex < 0 ? 0 : _directionSelectedIndex; } set { _directionSelectedIndex = value; RaisePropertyChanged("DirectionSelectedIndex"); RaisePropertyChanged("Direction"); RaisePropertyChanged("DirectionOrientation"); RaisePropertyChanged("DirectionVerticalOptions"); RaisePropertyChanged("DirectionHorizontalOptions"); } }

        public int ShowTextSelectedIndex { get { return _showTextSelectedIndex < 0 ? 0 : _showTextSelectedIndex; } set { _showTextSelectedIndex = value; RaisePropertyChanged("ShowTextSelectedIndex"); RaisePropertyChanged("ShowTextViaSelectedIndex"); } }

        public int ValueSelectedIndex
        {
            get { return (int)_value; }
            set
            {
                if (Math.Abs(_value - value) > 1)
                {
                    _value = value;
                    RaisePropertyChanged("ValueSelectedIndex");
                    RaisePropertyChanged("Value");
                }
            }
        }

        public GaugeTextVisibility ShowTextViaSelectedIndex
        {
            get
            {
                return (GaugeTextVisibility)Enum.Parse(typeof(GaugeTextVisibility), ShowTextItems[ShowTextSelectedIndex]);
            }
        }

        public string StartAngleText
        {
            get { return _startAngle.ToString(); }
            set
            {
                double dValue;
                if (Double.TryParse(value, out dValue))
                {
                    _startAngle = dValue;
                    RaisePropertyChanged("StartAngle");
                }
            }
        }
        public string SweepAngleText
        {
            get { return _sweepAngle.ToString(); }
            set
            {
                double dValue;
                if (Double.TryParse(value, out dValue))
                {
                    _sweepAngle = dValue;
                    RaisePropertyChanged("SweepAngle");
                }
            }
        }

        public string BadText
        {
            get { return _bad.ToString(); }
            set
            {
                double dValue;
                if (Double.TryParse(value, out dValue))
                {
                    _bad = dValue;
                    RaisePropertyChanged("Bad");
                }
            }
        }

        public string GoodText
        {
            get { return _good.ToString(); }
            set
            {
                double dValue;
                if (Double.TryParse(value, out dValue))
                {
                    _good = dValue;
                    RaisePropertyChanged("Good");
                }
            }
        }

        public string TargetText
        {
            get { return _target.ToString(); }
            set
            {
                double dValue;
                if (Double.TryParse(value, out dValue))
                {
                    _target = dValue;
                    RaisePropertyChanged("Target");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
