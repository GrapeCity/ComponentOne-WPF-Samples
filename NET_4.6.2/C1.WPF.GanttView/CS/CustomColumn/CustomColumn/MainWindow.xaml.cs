using System;
using System.Windows;
using C1.WPF.GanttView;
using C1.GanttView;
using System.ComponentModel;

namespace CustomColumn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _updating;

        public MainWindow()
        {
            InitializeComponent();

            gv.Loaded += Gv_Loaded;
        }

        private void Gv_Loaded(object sender, RoutedEventArgs e)
        {
            gv.Tasks.ListChanged += new ListChangedEventHandler(TasksResources_ListChanged);
            UpdateCost();
        }

        void TasksResources_ListChanged(object sender, ListChangedEventArgs e)
        {
            UpdateCost();
        }

        void UpdateCost()
        {
            if (!_updating)
            {
                _updating = true;
                foreach (Task task in gv.Tasks)
                {
                    if (task.Initialized)
                    {
                        decimal total = 0m;
                        double duration = task.GetDurationInDays();
                        if (double.IsNaN(duration))
                            duration = 0;
                        foreach (ResourceRef rr in task.ResourceRefs)
                        {
                            Resource r = rr.Resource;
                            if (r != null)
                            {
                                total += Convert.ToDecimal(rr.Amount * duration) * r.Cost;
                            }
                        }
                        task.SetFieldValue("ActualCost", total);
                    }
                }
                _updating = false;
            }
        }
    }
}
