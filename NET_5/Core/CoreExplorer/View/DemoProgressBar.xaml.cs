using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using C1.WPF.Core;
using CoreExplorer.Resources;

namespace CoreExplorer
{
    public partial class DemoProgressBar : UserControl
    {
        BackgroundWorker worker = new BackgroundWorker();

        public DemoProgressBar()
        {
            InitializeComponent();
            this.Tag = AppResources.DemoProgressBarDescription;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.DoWork += Worker_DoWork;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            this.Loaded += DemoProgressBar_Loaded;
            this.Unloaded += DemoProgressBar_Unloaded;
        }

        private void DemoProgressBar_Unloaded(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        private void DemoProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //btnRunAgain.Visibility = Visibility.Collapsed;
            for (int i = 0; i < 100; i++)
            {
                worker.ReportProgress(i);
                System.Threading.Thread.Sleep(10); //Simulate long tasks
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            c1ProgressBar1.Value = 100;
            c1ProgressBar2.Value = 100;
            defaultProgressBar.Value = 100;
            btnRunAgain.Visibility = Visibility.Visible;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            c1ProgressBar1.Value = e.ProgressPercentage;
            c1ProgressBar2.Value = e.ProgressPercentage;
            defaultProgressBar.Value = e.ProgressPercentage;
        }

        private void C1Button_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
            worker.RunWorkerAsync();
        }
    }
}
