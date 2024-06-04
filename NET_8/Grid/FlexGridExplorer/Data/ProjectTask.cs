using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FlexGridExplorer
{
    public class ProjectTask : INotifyPropertyChanged
    {
        private bool _isExpanded = true;
        public ProjectTask()
        {
            SubTasks = new List<ProjectTask>();
            IsExpanded = true;
        }

        public string WBS { get; set; }

        [Display(Name = "Task Name")]
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

        [Display(AutoGenerateField = false)]
        public ProjectTask ParentTask { get; set; }

        [Display(AutoGenerateField = false)]
        public List<ProjectTask> SubTasks { get; set; }

        [Display(AutoGenerateField = false)]
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
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

        [Display(AutoGenerateField = false)]
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

        [Display(AutoGenerateField = false)]
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

        #region INotifyPropertyChanged Members

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
