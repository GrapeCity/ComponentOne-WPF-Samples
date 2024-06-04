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

using C1.WPF.C1Chart;

namespace ChartSamples
{
    /// <summary>
    /// Interaction logic for LoadAnimation.xaml
    /// </summary>
    [System.ComponentModel.Description("C1Chart provides an easier API to provide loading animations that are very customizable.")]
    public partial class LoadAnimation : UserControl
    {
        Random rnd = new Random();

        public LoadAnimation()
        {
            InitializeComponent();
            cbChartType.ItemsSource = new ChartType[] 
      { 
        ChartType.Column, ChartType.ColumnStacked, ChartType.Bar, ChartType.BarStacked,
        ChartType.AreaStacked, ChartType.StepArea,
        ChartType.LineSymbols, ChartType.XYPlot, ChartType.Pie, ChartType.PieExplodedDoughnut
      };
            cbChartType.SelectedIndex = 0;
            cbChartType.SelectionChanged += (s, e) => chart.ChartType = (ChartType)cbChartType.SelectedItem;

            cbTransform.ItemsSource = Utils.GetEnumValues<AnimationTransform>();
            cbTransform.SelectedIndex = 0;

            cbOrigin.ItemsSource = Utils.GetEnumValues<AnimationOrigin>();
            cbOrigin.SelectedIndex = 0;

            cbEasing.ItemsSource = Utils.GetEnumValues<Easing>();
            cbEasing.SelectedIndex = 0;

            NewData();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewData();
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            cbTransform.SelectedIndex = rnd.Next(0, cbTransform.Items.Count);
            cbOrigin.SelectedIndex = rnd.Next(0, cbOrigin.Items.Count);
            cbEasing.SelectedIndex = rnd.Next(0, cbEasing.Items.Count);
            NewData();
        }

        void NewData()
        {
            chart.BeginUpdate();

            chart.Data.LoadAnimation = AnimationHelper.CreateAnimation(
                (AnimationTransform)cbTransform.SelectedItem,
                (AnimationOrigin)cbOrigin.SelectedItem,
                (Easing)cbEasing.SelectedItem,
                cbIndexDelay.IsChecked == true
              );

            int nser = rnd.Next(2, 6);
            int npts = rnd.Next(5, 10);

            chart.Data.Children.Clear();
            for (int i = 0; i < nser; i++)
                chart.Data.Children.Add(CreateDataSeries(npts));
            chart.EndUpdate();
        }

        public DataSeries CreateDataSeries(int npts, bool invert = false)
        {
            var ds = new XYDataSeries() { SymbolSize = new Size(20, 20) };

            int cnt = npts;
            var x = new double[cnt];
            var y = new double[cnt];

            for (int i = 0; i < cnt; i++)
            {
                x[i] = i; y[i] = rnd.NextDouble();
            }

            if (invert)
            {
                ds.XValuesSource = y; ds.ValuesSource = x;
            }
            else
            {
                ds.XValuesSource = x; ds.ValuesSource = y;
            }
            return ds;
        }
    }
}
