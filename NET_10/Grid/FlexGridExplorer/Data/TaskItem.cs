using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexGridExplorer
{
    public class TaskItem
    {
        static Random _rnd = new Random();
        static string[] _firstNames = "Andy|Ben|Charlie|Dan|Ed|Fred|Gil|Herb|Jack|Karl|Larry|Mark|Noah|Oprah|Paul|Quince|Rich|Steve|Ted|Ulrich|Vic|Xavier|Zeb".Split('|');
        static string[] _lastNames = "Ambers|Bishop|Cole|Danson|Evers|Frommer|Griswold|Heath|Jammers|Krause|Lehman|Myers|Neiman|Orsted|Paulson|Quaid|Richards|Stevens|Trask|Ulam".Split('|');
        static string[] _subjects = "Winter tires for free|20 car washes for free|Mobile App|Photo Contests|Multi Utility".Split('|');

        [Display(Name = nameof(AppResources.SubjectLabel), ResourceType = typeof(AppResources))]
        public string Subject { get; set; }
        
        [Display(AutoGenerateFilter = false, Name = nameof(AppResources.AssignToLabel), ResourceType = typeof(AppResources))]
        public string AssignTo { get; set; }

        [Display(Name = nameof(AppResources.OwnedByLabel), ResourceType = typeof(AppResources))]
        public string OwnedBy { get; set; }

        [Display(Name = nameof(AppResources.DueDateLabel), ResourceType = typeof(AppResources))]
        public DateTime DueDate { get; set; }
        
        [Display(Name = nameof(AppResources.CompleteLabel), ResourceType = typeof(AppResources))]
        public double Complete { get; set; }

        [Display(Name = nameof(AppResources.DeferredLabel), ResourceType = typeof(AppResources))]
        public bool Deferred { get; set; }

        [Display(Name = nameof(AppResources.UrgentLabel), ResourceType = typeof(AppResources))]
        public bool Urgent { get; set; }

        public static IEnumerable<TaskItem> GetRandomList(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var taskItem = new TaskItem
                {
                    Subject = GetRandomString(_subjects),
                    AssignTo = GetName(),
                    OwnedBy = GetName(),
                    DueDate = DateTime.Now.AddDays(_rnd.NextDouble() * 100),
                    Complete = Math.Min(1, _rnd.NextDouble() * 1.4),
                    Deferred = _rnd.NextDouble() > 0.9,
                    Urgent = _rnd.NextDouble() > 0.9,
                };
                yield return taskItem;
            }
        }

        static string GetRandomString(string[] arr)
        {
            return arr[_rnd.Next(arr.Length)];
        }
        static string GetName()
        {
            return string.Format("{0} {1}", GetRandomString(_firstNames), GetRandomString(_lastNames));
        }

    }
}
