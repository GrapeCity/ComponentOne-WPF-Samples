using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for HierarchicalTasks.xaml
    /// </summary>
    public partial class HierarchicalTasks : UserControl
    {
        public HierarchicalTasks()
        {
            InitializeComponent();

            #region ** Create tasks
            var task1 = new Task() { WBS = "1", Name = "Requirements", Duration = new TimeSpan(50, 0, 0, 0), Start = new DateTime(2009, 12, 4) };
            var task11 = new Task() { WBS = "1.1", Name = "Analysis", Duration = new TimeSpan(38, 3, 0, 0), Start = new DateTime(2009, 12, 4), ParentTask = task1 };
            var task111 = new Task() { WBS = "1.1.1", Name = "Analyze online reservations", Duration = new TimeSpan(12, 12, 0, 0), Start = new DateTime(2009, 12, 4), ParentTask = task11 };
            var task112 = new Task() { WBS = "1.1.2", Name = "Analyze query processes", Duration = new TimeSpan(12, 12, 0, 0), Start = new DateTime(2009, 12, 4), ParentTask = task11 };
            var task113 = new Task() { WBS = "1.1.3", Name = "Analyze multimedia enhancements", Duration = new TimeSpan(12, 12, 0, 0), Start = new DateTime(2010, 1, 4), ParentTask = task11 };
            var task114 = new Task() { WBS = "1.1.4", Name = "Draft Preliminary requirements", Duration = new TimeSpan(5, 0, 0, 0), Start = new DateTime(2010, 1, 14), ParentTask = task11 };
            var task115 = new Task() { WBS = "1.1.5", Name = "Review preliminary requirements", Duration = new TimeSpan(2, 12, 0, 0), Start = new DateTime(2010, 1, 14), ParentTask = task11 };
            var task116 = new Task() { WBS = "1.1.6", Name = "Incorporate feedback on requirements", Duration = new TimeSpan(2, 12, 0, 0), Start = new DateTime(2010, 1, 14), ParentTask = task11 };
            var task117 = new Task() { WBS = "1.1.7", Name = "Obtain approval to proceed", Duration = new TimeSpan(3, 2, 0, 0), Start = new DateTime(2010, 2, 6), ParentTask = task11 };
            task11.SubTasks = new List<Task> { task111, task112, task113, task114, task115, task116, task117 };
            var task12 = new Task() { WBS = "1.2", Name = "Acceptance Test Plan", Duration = new TimeSpan(12, 3, 0, 0), Start = new DateTime(2010, 6, 23), ParentTask = task1 };
            var task121 = new Task() { WBS = "1.2.1", Name = "Write acceptance test plans", Duration = new TimeSpan(5, 2, 0, 0), Start = new DateTime(2010, 6, 23), ParentTask = task12 };
            var task122 = new Task() { WBS = "1.2.2", Name = "Draft acceptance test plan", Duration = new TimeSpan(5, 0, 0, 0), Start = new DateTime(2010, 6, 23), ParentTask = task12 };
            var task123 = new Task() { WBS = "1.2.3", Name = "Review acceptance test plan", Duration = new TimeSpan(5, 6, 0, 0), Start = new DateTime(2010, 7, 4), ParentTask = task12 };
            var task124 = new Task() { WBS = "1.2.4", Name = "Incorporate feedback on acceptance", Duration = new TimeSpan(5, 0, 0, 0), Start = new DateTime(2010, 7, 4), ParentTask = task12 };
            task12.SubTasks = new List<Task> { task121, task122, task123, task124 };
            task1.SubTasks = new List<Task> { task11, task12 };
            var task2 = new Task() { WBS = "2", Name = "Design", Duration = new TimeSpan(55, 0, 0, 0), Start = new DateTime(2010, 8, 12) };
            var task21 = new Task() { WBS = "2.1", Name = "Top-level Design", Duration = new TimeSpan(27, 12, 0, 0), Start = new DateTime(2010, 8, 12), ParentTask = task2 };
            var task211 = new Task() { WBS = "2.1.1", Name = "Design online reservations", Duration = new TimeSpan(10, 0, 0, 0), Start = new DateTime(2010, 9, 7), ParentTask = task21 };
            var task212 = new Task() { WBS = "2.1.2", Name = "Design query processes", Duration = new TimeSpan(10, 12, 0, 0), Start = new DateTime(2010, 9, 14), ParentTask = task21 };
            var task213 = new Task() { WBS = "2.1.3", Name = "Design multimedia enhancements", Duration = new TimeSpan(10, 6, 0, 0), Start = new DateTime(2010, 10, 4), ParentTask = task21 };
            var task214 = new Task() { WBS = "2.1.4", Name = "Review design specification", Duration = new TimeSpan(5, 12, 0, 0), Start = new DateTime(2010, 10, 9), ParentTask = task21 };
            var task215 = new Task() { WBS = "2.1.5", Name = "Incorporate feedback into design", Duration = new TimeSpan(2, 2, 0, 0), Start = new DateTime(2010, 10, 9), ParentTask = task21 };
            var task216 = new Task() { WBS = "2.1.6", Name = "Top-level design approved", Duration = new TimeSpan(1, 0, 0, 0), Start = new DateTime(2010, 12, 1), ParentTask = task21 };
            task21.SubTasks = new List<Task> { task211, task212, task213, task214, task215, task216 };
            var task22 = new Task() { WBS = "2.2", Name = "Detailed Design", Duration = new TimeSpan(23, 0, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task2 };
            var task221 = new Task() { WBS = "2.2.1", Name = "Draft design specifications", Duration = new TimeSpan(17, 12, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task22 };
            var task222 = new Task() { WBS = "2.2.2", Name = "Review design specifications", Duration = new TimeSpan(17, 0, 0, 0), Start = new DateTime(2010, 12, 8), ParentTask = task22 };
            var task223 = new Task() { WBS = "2.2.3", Name = "Incorporate feedback on design specifications", Duration = new TimeSpan(17, 6, 0, 0), Start = new DateTime(2010, 12, 14), ParentTask = task22 };
            var task224 = new Task() { WBS = "2.2.4", Name = "Obtain approval to proceed", Duration = new TimeSpan(5, 0, 0, 0), Start = new DateTime(2010, 12, 24), ParentTask = task22 };
            var task225 = new Task() { WBS = "2.2.5", Name = "Detailed design approved", Duration = new TimeSpan(2, 12, 0, 0), Start = new DateTime(2010, 12, 28), ParentTask = task22 };
            task22.SubTasks = new List<Task> { task221, task222, task223, task224, task225 };
            task2.SubTasks = new List<Task> { task21, task22 };
            var task3 = new Task() { WBS = "3", Name = "Code and Unit Test", Duration = new TimeSpan(32, 4, 0, 0), Start = new DateTime(2010, 12, 4) };
            var task31 = new Task() { WBS = "3.1", Name = "Assign development staff", Duration = new TimeSpan(2, 3, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task3 };
            var task32 = new Task() { WBS = "3.2", Name = "Develop Code", Duration = new TimeSpan(10, 3, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task3 };
            var task321 = new Task() { WBS = "3.2.1", Name = "Develop online reservations", Duration = new TimeSpan(10, 2, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task32 };
            var task322 = new Task() { WBS = "3.2.2", Name = "Test online reservations", Duration = new TimeSpan(1, 11, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task32 };
            var task323 = new Task() { WBS = "3.2.3", Name = "Develop query processes", Duration = new TimeSpan(10, 0, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task32 };
            var task324 = new Task() { WBS = "3.2.4", Name = "Test query processes", Duration = new TimeSpan(1, 4, 0, 0), Start = new DateTime(2010, 12, 4), ParentTask = task32 };
            task32.SubTasks = new List<Task> { task321, task322, task323, task324 };
            task3.SubTasks = new List<Task> { task31, task32 };
            var tasks = new List<Task> { task1, task2, task3 };
            #endregion

            grid.ItemsSource = Task.GetAllTasks(tasks);
        }

        private void grid_LoadingRow(object sender, C1.WPF.DataGrid.DataGridRowEventArgs e)
        {
            var task = e.Row.DataItem as Task;
            task.PropertyChanged += (s, a) =>
            {
                if (a.PropertyName == "IsVisible")
                {
                    var subTask = s as Task;
                    var row = grid.Rows[subTask];
                    if (row != null && subTask != null)
                    {
                        UpdateRowVisibility(row, subTask);
                    }
                }
            };
            UpdateRowVisibility(e.Row, task);
        }

        private static void UpdateRowVisibility(C1.WPF.DataGrid.DataGridRow row, Task task)
        {
            row.Visibility = task.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void grid_LoadedRowPresenter(object sender, C1.WPF.DataGrid.DataGridRowEventArgs e)
        {
            var task = e.Row.DataItem as Task;
            e.Row.Presenter.FontWeight = task.SubTasks.Count > 0 ? FontWeights.Bold : FontWeights.Normal;
        }

        private void grid_LoadedRowHeaderPresenter(object sender, C1.WPF.DataGrid.DataGridRowEventArgs e)
        {
            var task = e.Row.DataItem as Task;
            var rowHeaderPresenter = e.Row.HeaderPresenter;
            if (task.SubTasks.Count > 0)
            {
                var toggleButton = new ToggleButton();
                toggleButton.Style = Resources["TButtonStyle"] as Style;
                toggleButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsExpanded")
                {
                    Source = task,
                    Mode = BindingMode.TwoWay
                });
                toggleButton.IsThreeState = false;
                toggleButton.Tag = task;
                rowHeaderPresenter.Content = toggleButton;
            }
            else
            {
                rowHeaderPresenter.Content = null;
            }
        }

        private void grid_AutoGeneratingColumn(object sender, C1.WPF.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Property.Name == "WBS")
            {
                e.Column.Width = C1.WPF.DataGrid.DataGridLength.Auto;
            }
            if (e.Column is DataGridDateTimeColumn)
            {
                ((DataGridDateTimeColumn)e.Column).Format = "ddd d/M/yyyy";
            }
            if (e.Property.Name == "Duration" && e.Column is C1.WPF.DataGrid.DataGridBoundColumn)
            {
                ((C1.WPF.DataGrid.DataGridBoundColumn)e.Column).Binding.Converter = CustomConverter.Create((value, type, parameter, culture) =>
                {
                    var valueTS = (TimeSpan)value;
                    return string.Format("{0:N1} days?", valueTS.TotalDays);
                });
            }
            if (new string[] { "Level", "ParentTask", "SubTasks", "IsExpanded", "IsVisible" }.Contains(e.Property.Name))
            {
                e.Cancel = true;
            }
        }

        private void grid_LoadedCellPresenter(object sender, DataGridCellEventArgs e)
        {
            if (e.Cell.Column.Name == "Name")
            {
                var task = e.Cell.Row.DataItem as Task;
                e.Cell.Presenter.Padding = new Thickness(task.Level * 16, 0, 0, 0);
            }
            else
            {
                e.Cell.Presenter.Padding = new Thickness();
            }
        }
    }

    public class Task : INotifyPropertyChanged
    {
        private bool _isExpanded = true;
        public Task()
        {
            SubTasks = new List<Task>();
            IsExpanded = true;
        }

        public string WBS { get; set; }

        //[Display(Name = "Task Name")]
        public string Name { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime Start { get; set; }

        public DateTime Finish
        {
            get
            {
                return Start + Duration;
            }
        }

        //[Display(AutoGenerateField = false)]
        public Task ParentTask { get; set; }

        //[Display(AutoGenerateField = false)]
        public List<Task> SubTasks { get; set; }

        //[Display(AutoGenerateField = false)]
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
                OnIsVisibleChanged();
            }
        }

        private void OnIsVisibleChanged()
        {
            OnPropertyChanged("IsVisible");
            foreach (var subTask in SubTasks)
            {
                subTask.OnIsVisibleChanged();
            }
        }

        //[Display(AutoGenerateField = false)]
        public bool IsVisible
        {
            get
            {
                if (ParentTask == null)
                {
                    return true;
                }
                else if (!ParentTask.IsExpanded)
                {
                    return false;
                }
                else
                {
                    return ParentTask.IsVisible;
                }
            }
        }

        //[Display(AutoGenerateField = false)]
        public int Level
        {
            get
            {
                if (ParentTask == null)
                {
                    return 0;
                }
                else
                {
                    return ParentTask.Level + 1;
                }
            }
        }

        public static IEnumerable<Task> GetAllTasks(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                yield return task;
                foreach (var subTask in Task.GetAllTasks(task.SubTasks))
                {
                    yield return subTask;
                }
            }
        }

        #region INotifyPropertyChanged Members

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
