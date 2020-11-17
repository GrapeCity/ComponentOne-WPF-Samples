using C1.Chart;
using FlexChartExplorer.Resources;
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

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for BoxWhisker.xaml
    /// </summary>
    public partial class BoxWhisker : UserControl
    {
        private List<string> _calculations = null;
        private List<ClassScore> _data = null;
        public BoxWhisker()
        {
            InitializeComponent();
            Tag = AppResources.BoxWhiskerTag;
        }

        public List<string> Calculations
        {
            get
            {
                if (_calculations == null)
                {
                    _calculations = DataCreator.CreateQuartileCalculations();
                }

                return _calculations;
            }
        }

        public List<ClassScore> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateSchoolScoreData();
                }

                return _data;
            }
        }

        private void CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (boxWhiskerA == null || boxWhiskerB == null || boxWhiskerC == null) return;

            if (sender == cbShowMeanLine)
            {
                boxWhiskerA.ShowMeanLine = cbShowMeanLine.IsChecked.Value;
                boxWhiskerB.ShowMeanLine = cbShowMeanLine.IsChecked.Value;
                boxWhiskerC.ShowMeanLine = cbShowMeanLine.IsChecked.Value;
            }
            else if (sender == cbShowInnerPoints)
            {
                boxWhiskerA.ShowInnerPoints = cbShowInnerPoints.IsChecked.Value;
                boxWhiskerB.ShowInnerPoints = cbShowMeanLine.IsChecked.Value;
                boxWhiskerC.ShowInnerPoints = cbShowMeanLine.IsChecked.Value;
            }
            else if (sender == cbShowOutliers)
            {
                boxWhiskerA.ShowOutliers = cbShowOutliers.IsChecked.Value;
                boxWhiskerB.ShowOutliers = cbShowOutliers.IsChecked.Value;
                boxWhiskerC.ShowOutliers = cbShowOutliers.IsChecked.Value;
            }
            else
            {
                boxWhiskerA.ShowMeanMarks = cbShowMeanMarks.IsChecked.Value;
                boxWhiskerB.ShowMeanMarks = cbShowMeanMarks.IsChecked.Value;
                boxWhiskerC.ShowMeanMarks = cbShowMeanMarks.IsChecked.Value;

            }
        }

        private void cboQuartileCalculation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (boxWhiskerA == null || boxWhiskerB == null || boxWhiskerC == null) return;

            var comboBox = sender as ComboBox;
            if (comboBox.SelectedValue != null)
            {
                var calculation = (QuartileCalculation)Enum.Parse(typeof(QuartileCalculation), comboBox.SelectedValue.ToString());
                boxWhiskerA.QuartileCalculation = calculation;
                boxWhiskerB.QuartileCalculation = calculation;
                boxWhiskerC.QuartileCalculation = calculation;
            }
        }
    }
}
