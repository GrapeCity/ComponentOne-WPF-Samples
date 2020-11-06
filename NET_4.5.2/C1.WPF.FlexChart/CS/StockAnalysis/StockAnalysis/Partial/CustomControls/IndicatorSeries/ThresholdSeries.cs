using StockAnalysis.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace StockAnalysis.Partial.CustomControls.IndicatorSeries
{
    class ThresholdSeries : C1.WPF.Chart.Finance.FinancialSeries
    {
        public event EventHandler OnThesholdChanged;

        //Object.Threshold threshold;

        //public Object.Threshold Threshold
        //{
        //    get { return threshold; }
        //    set
        //    {
        //        if (value != threshold)
        //        {
        //            if (threshold != null)
        //            {
        //                threshold.PropertyChanged -= Threshold_PropertyChanged;
        //            }
        //            threshold = value;
        //            threshold.PropertyChanged += Threshold_PropertyChanged;
        //        }
        //    }
        //}
        public Threshold Threshold
        {
            get { return (Threshold)GetValue(ThresholdProperty); }
            set { SetValue(ThresholdProperty, value); }
        }

        public static readonly DependencyProperty ThresholdProperty =
            DependencyProperty.Register("Threshold", typeof(Threshold), typeof(ThresholdSeries),
            new PropertyMetadata(null, new PropertyChangedCallback(
                (o, e) =>
                {
                    ThresholdSeries series = o as ThresholdSeries;

                    if (e.NewValue is Threshold)
                    {
                        (e.NewValue as Threshold).PropertyChanged += series.Threshold_PropertyChanged;
                    }
                    if (e.OldValue is Threshold)
                    {
                        (e.OldValue as  Threshold).PropertyChanged -= series.Threshold_PropertyChanged;
                    }
                }
                )));


        public bool ZonesVisibility
        {
            get { return (bool)GetValue(ZonesVisibilityProperty); }
            set { SetValue(ZonesVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ZonesVisibilityProperty =
            DependencyProperty.Register("ZonesVisibility", typeof(bool), typeof(ThresholdSeries),
            new PropertyMetadata(true, new PropertyChangedCallback(
                (o, e) =>
                {
                    ThresholdSeries series = o as ThresholdSeries;
                    bool b = Convert.ToBoolean(e.NewValue);
                    series.Visibility = b ? C1.Chart.SeriesVisibility.Visible : C1.Chart.SeriesVisibility.Hidden;
                }
                )));

        

        private void Threshold_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Threshold threshold = sender as Threshold;
            if (e.PropertyName == "Brush")
            {
                this.Style.Stroke = this.Threshold.Brush;
            }
            if (this.OnThesholdChanged != null)
            {
                this.OnThesholdChanged(this, new EventArgs());
            }
        }

        public override double[] GetValues(int dim)
        {
            double[] result = null;
            if (this.Chart != null && this.Chart.AxisX != null)
            {
                var xs = new double[] { this.Chart.AxisX.Min, this.Chart.AxisX.Max };
                var ys = new double[] { Threshold.Value, Threshold.Value };

                switch (dim)
                {
                    case 0:
                        result = ys;
                        break;
                    case 1:
                        result = xs;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        
    }
}
