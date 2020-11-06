using C1.GanttView;
using C1.WPF.GanttView;
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

namespace GanttViewSample
{
    /// <summary>
    /// Interaction logic for GanttViewDemo.xaml
    /// </summary>
    public partial class GanttViewDemo : UserControl
    {
        public GanttViewDemo()
        {
            InitializeComponent();

            gv.Tasks.Clear();

            Task t = new Task();
            t.Mode = TaskMode.Manual;
            t.Name = "Taskbar Interactivity";
            t.Start = new DateTime(2016, 4, 5);
            t.Duration = 5;
            t.PercentComplete = 0.5;

            Task t1 = new Task();
            t1.Mode = TaskMode.Manual;
            t1.Name = "Today Marker";
            t1.Start = new DateTime(2016, 4, 20);
            t1.Duration = 8;
            t1.PercentComplete = 0;

            Task t2 = new Task();
            t2.Mode = TaskMode.Manual;
            t2.Name = "Task Management";
            t2.Start = new DateTime(2016, 4, 8);
            t2.Duration = 4;
            t2.PercentComplete = 0.6;

            Task t3 = new Task();
            t3.Mode = TaskMode.Manual;
            t3.Name = "Bound Mode";
            t3.Start = new DateTime(2016, 5, 1);
            t3.Duration = 10;
            t3.PercentComplete = 0.2;

            Task t4 = new Task();
            t4.Mode = TaskMode.Manual;
            t4.Name = "Samples";
            t4.Start = new DateTime(2016, 4, 16);
            t4.Duration = 7;
            t4.PercentComplete = 0.2;

            Task t5 = new Task();
            t5.Mode = TaskMode.Manual;
            t5.Name = "Save and Load XML file";
            t5.Start = new DateTime(2016, 3, 29);
            t5.Duration = 1;
            t5.PercentComplete = 0.5;

            Task t6 = new Task();
            t6.Mode = TaskMode.Manual;
            t6.Name = "Design time";
            t6.Start = new DateTime(2016, 5, 3);
            t6.Duration = 4;
            t6.PercentComplete = 1;

            Task t7 = new Task();
            t7.Mode = TaskMode.Manual;
            t7.Name = "CalendarInfo";
            t7.Start = new DateTime(2016, 5, 11);
            t7.Duration = 2;
            t7.PercentComplete = 0.8;

            Task t8 = new Task();
            t8.Mode = TaskMode.Manual;
            t8.Name = "Scroll to current task";
            t8.Start = new DateTime(2016, 5, 26);
            t8.Duration = 3;
            t8.PercentComplete = 0.5;

            Task t9 = new Task();
            t9.Mode = TaskMode.Manual;
            t9.Name = "Timescale";
            t9.Start = new DateTime(2016, 4, 20);
            t9.Duration = 3;
            t9.PercentComplete = 0;

            Task t10 = new Task();
            t10.Mode = TaskMode.Manual;
            t10.Name = "Sorting";
            t10.Start = new DateTime(2016, 5, 18);
            t10.Duration = 14;
            t10.PercentComplete = 0.7;

            Task t11 = new Task();
            t11.Mode = TaskMode.Manual;
            t11.Name = "Printing";
            t11.Start = new DateTime(2016, 5, 10);
            t11.Duration = 3;
            t11.PercentComplete = 0;

            gv.Tasks.Add(t);
            gv.Tasks.Add(t1);
            gv.Tasks.Add(t2);
            gv.Tasks.Add(t3);
            gv.Tasks.Add(t4);
            gv.Tasks.Add(t5);
            gv.Tasks.Add(t6);
            gv.Tasks.Add(t7);
            gv.Tasks.Add(t8);
            gv.Tasks.Add(t9);
            gv.Tasks.Add(t10);
            gv.Tasks.Add(t11);
        }
    }
}
