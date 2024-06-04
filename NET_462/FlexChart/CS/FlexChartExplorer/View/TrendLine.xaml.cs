using C1.Chart;
using C1.WPF;
using C1.WPF.Chart;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for TrendLine.xaml
    /// </summary>
    public partial class TrendLine : UserControl
    {
        Random rnd = new Random();
        ObservableCollection<DataItem> _data;
        List<string> _fitTypes;
        DataItem clickedItem;
        C1DragHelper _dragHelper;

        public TrendLine()
        {
            InitializeComponent();
            this.Loaded += HandleLoaded;
        }

        

        public ObservableCollection<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new ObservableCollection<DataItem>();
                    for (int i = 1; i < 11; i++)
                    {
                        _data.Add(new DataItem() { X = i, Y = rnd.Next(100) });
                    }
                }

                return _data;
            }
        }

        public List<string> FitTypes
        {
            get
            {
                if (_fitTypes == null)
                {
                    _fitTypes = Enum.GetNames(typeof(FitType)).ToList();
                }

                return _fitTypes;
            }
        }

        string GetEquationString(C1.WPF.Chart.TrendLine trendLine)
        {
            string result = String.Empty;
            int X = 1, Y0 = 0;
            var order = trendLine.Order;

            switch (trendLine.FitType)
            {
                case FitType.Linear:
                    result = String.Format("y={1:0.0000}x{0:+0.0000;-0.0000;+0}", trendLine.Coefficients[0], trendLine.Coefficients[1]);
                    break;
                case FitType.Exponent:
                    result = String.Format("y={0:0.0000}e<sup>{1:0.0000}x</sup>", trendLine.Coefficients[0], trendLine.Coefficients[1]);
                    break;
                case FitType.Logarithmic:
                    result = String.Format("y={1:0.0000}ln(x){0:+0.0000;-0.0000;+0}", trendLine.Coefficients[0], trendLine.Coefficients[1]);
                    break;
                case FitType.Power:
                    result = String.Format("y={0:0.0000}x<sup>{1:0.0000}</sup>", trendLine.Coefficients[0], trendLine.Coefficients[1]);
                    break;
                case FitType.Polynom:
                    result = String.Format("{1:+0.0000;-0.0000;+0}x{0:+0.0000;-0.0000;+0}", trendLine.Coefficients[0], trendLine.Coefficients[1]);
                    for (int i = 2; i <= (int)order; i++)
                        result = result.Insert(0, String.Format("{0:+0.000;-0.0000;+0}x<sup>{1}</sup>", trendLine.Coefficients[i], i));
                    result = result.Remove(0, 1).Insert(0, "y=");
                    break;
                case FitType.Fourier:
                    result = String.Format("{0:+0.0000;-0.0000;+0}", trendLine.Coefficients[0]);
                    for (int i = 2, a = 1; i <= (int)order; i++, a = i % 2 == 0 ? a + 1 : a)
                        result += String.Format("{0:+0.000;-0.0000;+0}{2}({1}x)", trendLine.Coefficients[i - 1], a == 1 ? "" : a.ToString(), (i) % 2 == 0 ? "cos" : "sin");
                    result = result.Remove(0, 1).Insert(0, "y=");
                    break;
                case FitType.MaxX: result = "x=" + trendLine.GetValues(X).Max(); break;
                case FitType.MinX: result = "x=" + trendLine.GetValues(X).Min(); break;
                case FitType.MaxY: result = "y=" + trendLine.GetValues(Y0).Max(); break;
                case FitType.MinY: result = "y=" + trendLine.GetValues(Y0).Min(); break;
                case FitType.AverageX: result = "x=" + trendLine.GetValues(X).Average(); break;
                case FitType.AverageY: result = "y=" + trendLine.GetValues(Y0).Average(); break;
            }
            return result;
        }

        #region Event Handler

        void HandleLoaded(object sender, RoutedEventArgs e)
        {
            if (_dragHelper == null)
            {
                _dragHelper = new C1DragHelper(flexChart, C1DragHelperMode.TranslateY);
                _dragHelper.DragStarted += HandleDragStarted;
                _dragHelper.DragDelta += HandleDragDelta;
                _dragHelper.DragCompleted += HandleDragCompleted;
            }
        }

        void HandleDragStarted(object sender, C1DragStartedEventArgs e)
        {
            var ht = flexChart.HitTest(e.GetPosition(flexChart));
            if (ht.Distance < 3 && ht.X != null && ht.Y != null)
            {
                clickedItem = ht.Item as DataItem;
            }
        }

        void HandleDragDelta(object sender, C1DragDeltaEventArgs e)
        {
            if (clickedItem != null)
            {
                var newY = (int)flexChart.PointToData(e.GetPosition(flexChart)).Y;
                if (newY >= 0 && newY <= 100)
                {
                    clickedItem.Y = newY;
                }
            }
        }

        void HandleDragCompleted(object sender, C1DragCompletedEventArgs e)
        {
            clickedItem = null;
        }

        void flexChart_Rendered(object sender, RenderEventArgs e)
        {
            rich.Html = GetEquationString(trendLine);
        }

        #endregion

        #region DataItem

        public class DataItem : INotifyPropertyChanged
        {
            int _y;
            public int X { get; set; }

            public int Y
            {
                get { return _y; }
                set
                {
                    if (value == _y) return;
                    _y = value;

                    OnPropertyChanged("Y");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
