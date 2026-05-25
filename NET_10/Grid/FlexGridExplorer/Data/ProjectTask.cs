using CommunityToolkit.Mvvm.ComponentModel;
using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlexGridExplorer
{
    public partial class ProjectTask : ObservableObject
    {
        private bool _isExpanded = true;
        public ProjectTask()
        {
            SubTasks = new List<ProjectTask>();
            IsExpanded = true;
        }

        [ObservableProperty]
        [Display(Name = nameof(AppResources.WBSLabel), ResourceType = typeof(AppResources), Order = 0)]
        public string wBS;

        [Display(Name = nameof(AppResources.TaskNameLabel), ResourceType = typeof(AppResources), Order = 1)]
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        [Display(Name = nameof(AppResources.DurationLabel), ResourceType = typeof(AppResources), Order = 2)]
        public TimeSpan duration;

        [ObservableProperty]
        [Display(Name = nameof(AppResources.StartLabel), ResourceType = typeof(AppResources), Order = 3)]
        public DateTime start;

        [Display(Name = nameof(AppResources.FinishLabel), ResourceType = typeof(AppResources), Order = 4)]
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
    }

}
