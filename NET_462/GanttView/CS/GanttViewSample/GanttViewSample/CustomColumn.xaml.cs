using C1.GanttView;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GanttViewSample
{
    /// <summary>
    /// Interaction logic for CustomColumn.xaml
    /// </summary>
    public partial class CustomColumn : UserControl
    {
        bool _updating;

        public CustomColumn()
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
