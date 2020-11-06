using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;


namespace ChartSamples
{
    [System.ComponentModel.Description("This sample shows how to implement drag and drop with the C1Chart control.")]
    public partial class DragDrop : UserControl
    {
        Random rnd = new Random();

        // list of series is common for both charts
        List<DataSeries> list1;

        public DragDrop()
        {
            InitializeComponent();


            // create test data
            int nser = 8, npts = 8;
            list1 = CreateTestData(nser, npts);
            foreach (DataSeries ds in list1)
            {
                ds.PlotElementLoaded += new EventHandler(ds_Loaded);
                chart1.Data.Children.Add(ds);
            }

            RegisterDragDropWPF();

            CheckNoData();
        }

        void RegisterDragDropWPF()
        {
            chart1.AllowDrop = true;
            chart1.Drop += (s, e) =>
            {
                string lbl = (string)e.Data.GetData(DataFormats.StringFormat);
                DataSeries dser = list1.FirstOrDefault(ds => ds.Label == lbl);
                if (chart2.Data.Children.Contains(dser))
                {
                    chart2.Data.Children.Remove(dser);
                    chart1.Data.Children.Add(dser);
                }
                CheckNoData();
            };

            chart2.AllowDrop = true;
            chart2.Drop += (s, e) =>
            {
                string lbl = (string)e.Data.GetData(DataFormats.StringFormat);
                DataSeries dser = list1.FirstOrDefault(ds => ds.Label == lbl);
                if (chart1.Data.Children.Contains(dser))
                {
                    chart1.Data.Children.Remove(dser);
                    chart2.Data.Children.Add(dser);
                }
                CheckNoData();
            };
        }


        /// <summary>
        /// Fires when plot element was loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ds_Loaded(object sender, EventArgs e)
        {
            PlotElement pe = (PlotElement)sender;
            DataSeries dss = pe.DataPoint.Series;

            // "fix" the series color to avoid automatic palette
            if (pe is Lines)
            {
                dss.ConnectionFill = pe.Fill;
                dss.ConnectionStroke = pe.Stroke;
            }
            else
            {
                dss.SymbolFill = pe.Fill;
                dss.SymbolStroke = pe.Stroke;
            }

            pe.MouseDown += (se, ev) =>
            {
                // start drag & drop with series label as data object
                System.Windows.DragDrop.DoDragDrop(pe, pe.DataPoint.Series.Label, DragDropEffects.Move);
            };

        }

        /// <summary>
        /// show or hide "no data" message
        /// </summary>
        void CheckNoData()
        {
            if (chart1.Data.Children.Count == 0)
            {
                nodata.Visibility = Visibility.Visible;
                Grid.SetColumn(nodata, 0);
            }
            else if (chart2.Data.Children.Count == 0)
            {
                nodata.Visibility = Visibility.Visible;
                Grid.SetColumn(nodata, 1);
            }
            else
                nodata.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Creates specified number of data series with test data.
        /// </summary>
        /// <param name="ns">Number of data series.</param>
        /// <param name="npts">Number of points.</param>
        /// <returns>List of data series.</returns>
        List<DataSeries> CreateTestData(int ns, int npts)
        {
            List<DataSeries> list = new List<DataSeries>();
            for (int iser = 0; iser < ns; iser++)
            {
                double[] vals = new double[npts];
                for (int i = 0; i < npts; i++)
                    vals[i] = rnd.Next(1, 100);
                list.Add(new DataSeries() { ValuesSource = vals, Label = "s" + iser.ToString() });
            }
            return list;
        }
    }
}
