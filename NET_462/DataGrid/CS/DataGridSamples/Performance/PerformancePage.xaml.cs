using System;
using System.Collections.Generic;
using System.Collections;
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
using C1.WPF;
using C1.WPF.DataGrid;
using System.Globalization;
using System.ComponentModel;
using System.Threading;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for PerformancePage.xaml
    /// </summary>
    public partial class Performance : UserControl
    {
        public Performance()
        {
            InitializeComponent();

            // initialize viewport size combo
            cmbViewPortSize.Items.Add("Small Viewport");
            cmbViewPortSize.Items.Add("Full Viewport");
            cmbViewPortSize.SelectionChanged += new SelectionChangedEventHandler(cmbViewPortSize_SelectionChanged);
            cmbViewPortSize.SelectedIndex = 0;
        }

        const short columns = 1000;
        const int rows = 100000;
        BackgroundWorker worker = new BackgroundWorker();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            info.Visibility = Visibility.Visible;
            grid.Opacity = 100;
            btn.Visibility = Visibility.Collapsed;

            for (int i = 0; i < columns; i++)
            {
                grid.Columns.Add(new DataGridNumericColumn()
                {
                    Header = new TextBlock()
                    {
                        Text = "Column" + (i + 1).ToString(),
                        TextAlignment = TextAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 5, 0),
                    },
                    Binding = new System.Windows.Data.Binding()
                    {
                        Path = new PropertyPath("[" + i + "]"),
                        Mode = System.Windows.Data.BindingMode.OneTime,
                    }
                });
            }

            progress.Minimum = 0;
            progress.Maximum = rows;

            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
        }

        #region Background worker

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int id2 = Thread.CurrentThread.ManagedThreadId;
            Random rnd = new Random();

            var list = new List<List<short>>(rows);
            for (int i = 0; i < rows; i++)
            {
                if (i % 1000 == 0)
                {
                    worker.ReportProgress(0, i);
                    Thread.Sleep(20);
                }
                var values = new List<short>(columns);

                for (int j = 0; j < columns; j++)
                {
                    values.Add((short)rnd.Next(short.MaxValue));
                }

                list.Add(values);
            }

            e.Result = list;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            grid.ItemsSource = (IList)e.Result;

            info.Visibility = Visibility.Collapsed;
            grid.Visibility = Visibility.Visible;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = (int)e.UserState;
            txt.Text = string.Format("{0} / {1} rows",
                        ((int)e.UserState).ToString("n0", CultureInfo.CurrentCulture),
                        rows.ToString("n0", CultureInfo.CurrentCulture));
        }

        #endregion


        void cmbViewPortSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbViewPortSize.SelectedItem.ToString() == "Small Viewport")
            {
                grid.Width = 560;
                grid.Height = 320;
            }
            else
            {
                grid.Width = double.NaN;
                grid.Height = double.NaN;
            }

            ResetRecyclingState();

        }

        private void ResetRecyclingState()
        {
            /*
             *  The C1DataGrid is not removing the recycling elements in size changes
             *  This might impact the performance in the small scenario, as more elements
             *  are being kept in the panels for future usage.
             */

            // save default template
            var tmp = grid.Template;

            grid.Template = new ControlTemplate();
            grid.ApplyTemplate();

            grid.Template = tmp;
            grid.ApplyTemplate();
        }
    }
}
