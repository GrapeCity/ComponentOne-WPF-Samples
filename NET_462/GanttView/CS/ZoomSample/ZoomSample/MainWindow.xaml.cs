using System;
using System.Windows;

namespace ZoomSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.gv.Project.StartDate = this.gv.Tasks[1].Start.Value;
        }

        private void btnZoomSelectedTask_Click(object sender, RoutedEventArgs e)
        {
            gv.ZoomTask(gv.SelectedTask);
        }

        private void btnZoomEntire_Click(object sender, RoutedEventArgs e)
        {
            gv.ZoomEntireProject();
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            gv.ZoomIn();
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            gv.ZoomOut();
        }

        private void gv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gv.SelectedTask == null || !gv.SelectedTask.Initialized)
                btnZoomSelectedTask.IsEnabled = false;
            else
                btnZoomSelectedTask.IsEnabled = true;
        }
    }
}
