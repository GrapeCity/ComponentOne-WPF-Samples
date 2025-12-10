using C1.WPF.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Localization.ViewModel 
{
    class ScheduleTabViewModel 
    {
        public IEnumerable<ViewType> ViewTypeOptions =>
            Enum.GetValues(typeof(ViewType)).Cast<ViewType>();

        public ObservableCollection<ViewType> AvailableViewTypes { get; }
        
        public ScheduleTabViewModel()
        {
            AvailableViewTypes = new ObservableCollection<ViewType>
            {
                ViewType.Month,
                ViewType.Day,
                ViewType.Week,
                ViewType.WorkingWeek,
                ViewType.TimeLine
            };
        }

    }
}
