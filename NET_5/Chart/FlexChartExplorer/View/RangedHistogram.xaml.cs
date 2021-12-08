using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using System.Windows.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using FlexChartExplorer.Resources;

namespace FlexChartExplorer
{
    public sealed partial class RangedHistogram : UserControl
    {
        public RangedHistogram()
        {
            this.InitializeComponent();
            Tag = AppResources.RangedHistogramTag;
        }

        public object[] Data
        {
            get
            {
                return  new[]{
                    new { Name = "A", Value = 20 },
                    new { Name = "B", Value = 35 },
                    new { Name = "C", Value = 40 },
                    new { Name = "A", Value = 55 },
                    new { Name = "B", Value = 80 },
                    new { Name = "C", Value = 60 },
                    new { Name = "A", Value = 61 },
                    new { Name = "B", Value = 85 },
                    new { Name = "C", Value = 80 },
                    new { Name = "A", Value = 64 },
                    new { Name = "B", Value = 80 },
                    new { Name = "C", Value = 75 },
                    new { Name = "B", Value = 133 },
                };
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void flexChart_Rendered(object sender, C1.WPF.Chart.RenderEventArgs e)
        {
            tbBinWidth.Text = histogramSeries.BinWidth.ToString("0");
            tbBinNum.Text = histogramSeries.NumberOfBins.ToString();
            tbOverflow.Text = histogramSeries.OverflowBin.ToString("0");
            tbUnderflow.Text = histogramSeries.UnderflowBin.ToString("0");
        }

        private void cbCategory_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            histogramSeries.BindingX = "Name";
            histogramSeries.SeriesName = "Category Total";

            cbMode.IsEnabled = false;
            cbOverflow.IsEnabled = false;
            tbOverflow.IsEnabled = false;
            cbUnderflow.IsEnabled = false;
            tbUnderflow.IsEnabled = false;
            tbBinWidth.IsEnabled = false;
            tbBinNum.IsEnabled = false;
        }

        private void cbCategory_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            histogramSeries.BindingX = "";
            histogramSeries.SeriesName = "Frequency";

            cbMode.IsEnabled = true;
            cbOverflow.IsEnabled = true;
            tbOverflow.IsEnabled = cbOverflow.IsChecked != null && cbOverflow.IsChecked.Value;
            cbUnderflow.IsEnabled = true;
            tbUnderflow.IsEnabled = cbUnderflow.IsChecked != null && cbUnderflow.IsChecked.Value;
            tbBinWidth.IsEnabled = histogramSeries.BinMode == HistogramBinning.BinWidth;
            tbBinNum.IsEnabled = histogramSeries.BinMode == HistogramBinning.NumberOfBins;
        }

        List<string> _modes;
        public List<string> Mode
        {
            get
            {
                if (_modes == null)
                {
                    _modes = Enum.GetNames(typeof(HistogramBinning)).ToList();
                }

                return _modes;
            }
        }

        private void tbBinNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            int result;
            if (Int32.TryParse(tbBinNum.Text, out result))
            {
                histogramSeries.NumberOfBins = result;
            }
        }

        private void tbBinWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            double result;
            if (Double.TryParse(tbBinWidth.Text, out result))
            {
                histogramSeries.BinWidth = result;
            }
        }

        private void cbOverflow_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            tbOverflow.IsEnabled = true;
            histogramSeries.ShowOverflowBin = true;
            UpdateOverflowBin();
        }

        private void cbOverflow_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            histogramSeries.ShowOverflowBin = false;
            tbOverflow.IsEnabled = false;
        }

        private void tbOverflow_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateOverflowBin();
        }

        private void cbUnderflow_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            tbUnderflow.IsEnabled = true;
            histogramSeries.ShowUnderflowBin = true;
            UpdateUnderflowBin();
        }

        private void cbUnderflow_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            histogramSeries.ShowUnderflowBin = false;
            tbUnderflow.IsEnabled = false;
        }

        private void tbUnderflow_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUnderflowBin();
        }

        private void UpdateOverflowBin()
        {
            double result;
            if (cbOverflow.IsChecked != null && cbOverflow.IsChecked.Value && Double.TryParse(tbOverflow.Text, out result))
            {
                histogramSeries.OverflowBin = result;
            }
        }

        private void UpdateUnderflowBin()
        {
            double result;
            if (cbUnderflow.IsChecked != null && cbUnderflow.IsChecked.Value && Double.TryParse(tbUnderflow.Text, out result))
            {
                histogramSeries.UnderflowBin = result;
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (tbBinNum == null) return;
            switch (cbMode.SelectedIndex)
            {
                case 0:
                    //Automatic
                    tbBinNum.IsEnabled = false;
                    tbBinWidth.IsEnabled = false;
                    break;
                case 1:
                    //Bin width
                    tbBinWidth.IsEnabled = true;
                    tbBinNum.IsEnabled = false;
                    break;
                case 2:
                    //Number of bins
                    tbBinNum.IsEnabled = true;
                    tbBinWidth.IsEnabled = false;
                    break;
            }
        }
    }
}
