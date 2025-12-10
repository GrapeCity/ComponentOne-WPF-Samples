using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using C1.GanttView;
using Localization.DataSource;

namespace Localization.ViewModel
{
    /// <summary>
    /// ViewModel for binding tasks to the GanttView control.
    /// </summary>
    public class GanttViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Task> _tasks;

        /// <summary>
        /// Initializes the ViewModel and loads default tasks.
        /// </summary>
        public GanttViewModel()
        {
            Tasks = new ObservableCollection<Task>();
            LoadTasks();
        }

        /// <summary>
        /// Task collection bound to the GanttView.
        /// </summary>
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Loads tasks using the TaskFactoryGV helper class.
        /// </summary>
        private void LoadTasks()
        {
            var factoryTasks = TaskFactoryGV.GetDefaultTasks();
            foreach (var task in factoryTasks)
            {
                Tasks.Add(task);
            }
        }

        /// <summary>
        /// PropertyChanged event for data binding.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies of property changes to bound controls.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}