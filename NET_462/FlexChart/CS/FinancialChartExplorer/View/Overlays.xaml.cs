using C1.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovingAverageType = C1.Chart.Finance.MovingAverageType;

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for Overlays.xaml
    /// </summary>
    public partial class Overlays : UserControl
    {
        DataService dataService = DataService.GetService();

        public Overlays()
        {
            InitializeComponent();
            this.Loaded += Overlays_Loaded;
        }

        private void Overlays_Loaded(object sender, RoutedEventArgs e)
        {
            cbOverlays.SelectedIndex = 0;
            cbTypes.SelectedIndex = 0;
        }

        public List<Quote> Data
        {
            get
            {
                return dataService.GetSymbolData("box");
            }
        }

        public List<string> OverlayTypes
        {
            get
            {
                return "Bollinger Bands,Envelopes,Ichimoku Cloud,Alligator, ZigZag".Split(',').ToList();
            }
        }

        public List<string> Types
        {
            get
            {
                return Enum.GetNames(typeof(MovingAverageType)).ToList();
            }
        }

        private void financialChart_MouseMove(object sender, MouseEventArgs e)
        {
            var pt = e.GetPosition(overlayChart);
            var hitTest = overlayChart.HitTest(pt, false);
            var ser = hitTest.Series;
            if (ser != null)
            {
                if (ser == bollinger)
                {
                    overlayChart.ToolTipContent = "{seriesName}\u000ADate: {Date}\u000AUpper: {Upper:n2}\u000AMiddle: {Middle:n2}\u000ALower: {Lower:n2}";
                }
                else if (ser == envelopes)
                {
                    overlayChart.ToolTipContent = "{seriesName}\u000ADate: {Date}\u000AUpper: {Upper:n2}\u000ALower: {Lower:n2}";
                }
                else
                {
                    overlayChart.ToolTipContent = "{}{seriesName}\u000ADate: {Date}\u000AY: {y:n2}\u000AVolume: {Volume:n0}";
                }
            }
        }

        private void cbSize_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (envelopes != null)
            {
                var size = e.NewValue;
                size = Math.Min(Math.Max(0, size), 1);
                envelopes.Size = size;
            }
        }

        private void cbEnvelopesPeriod_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (envelopes != null)
            {
                var period = (int)e.NewValue;
                period = Math.Min(Math.Max(2, period), 86);
                envelopes.Period = period;
            }
        }

        private void cbMultiplier_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (bollinger != null)
            {
                var multiplier = (int)e.NewValue;
                multiplier = Math.Min(Math.Max(1, multiplier), 100);
                bollinger.Multiplier = multiplier;
            }
        }

        private void cbPeriod_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (bollinger != null)
            {
                var period = (int)e.NewValue;
                period = Math.Min(Math.Max(2, period), 86);
                bollinger.Period = period;
            }
        }

        private void baseNumberic_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (ichimoku != null)
            {
                ichimoku.BasePeriod = (int)e.NewValue;
            }
        }

        private void conversionNumberic_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (ichimoku != null)
            {
                ichimoku.ConversionPeriod = (int)e.NewValue;    
            }
        }

        private void leadingNumberic_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (ichimoku != null)
            {
                ichimoku.LeadingPeriod = (int)e.NewValue;
            }
        }

        private void laggingNumberic_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (ichimoku != null)
            {
                ichimoku.LaggingPeriod = (int)e.NewValue;
            }
        }

        private void cbJawPeriod_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            alligator.JawPeriod = (int)e.NewValue;
        }

        private void cbTeethPeriod_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            alligator.TeethPeriod = (int)e.NewValue;
        }

        private void cbLipsPeriod_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            alligator.LipsPeriod = (int)e.NewValue;
        }

        private void cbDistance_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            zigZag.Distance = (int)e.NewValue;
        }
    }
}
