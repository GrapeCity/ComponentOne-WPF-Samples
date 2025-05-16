using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using C1.WPF.GanttView;
using C1.GanttView;

namespace CustomColumn
{
    /// <summary>
    /// Interaction logic for CustomColumn.xaml
    /// </summary>
    public partial class CustomColumnDemo : UserControl
    {
        bool _updating;
        public CustomColumnDemo()
        {
            this.Tag = CustomColumn.Properties.Resources.Tag;
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
            if (gv.Tasks.Count > 1)
            {
                gv.ScrollToTask(gv.Tasks[1], true);
            }
        }

        void UpdateCost()
        {
            if (!_updating)
            {
                _updating = true;
                foreach (C1.GanttView.Task task in gv.Tasks)
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
