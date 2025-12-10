using C1.Util.DX.Direct2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Localization.ViewModel
{
    public static class LocalizeManager
    {
        public static string GetString(string key)
        {
            return Properties.Resources.ResourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture);
        }
    }
    public class LocalizationViewModel : INotifyPropertyChanged
    {

        public string BrushPicker => LocalizeManager.GetString("BrushPicker");
        public string ColorPicker => LocalizeManager.GetString("ColorPicker");
        public string DatePicker => LocalizeManager.GetString("DatePicker");
        public string LineNumber => LocalizeManager.GetString("LineNumber");
        public string PickLanguage => LocalizeManager.GetString("PickLanguage");
        public string ShowWeekTabs => LocalizeManager.GetString("ShowWeekTabs");
        public string ScheduleView => LocalizeManager.GetString("ScheduleView");
        public string EnableGrouping => LocalizeManager.GetString("EnableGrouping");
        public string Scheduling => LocalizeManager.GetString("Scheduling");
        public void Refresh()
        {
            OnPropertyChanged(nameof(BrushPicker));
            OnPropertyChanged(nameof(ColorPicker));
            OnPropertyChanged(nameof(DatePicker));
            OnPropertyChanged(nameof(LineNumber));
            OnPropertyChanged(nameof(PickLanguage));
            OnPropertyChanged(nameof(ShowWeekTabs));
            OnPropertyChanged(nameof(ScheduleView));
            OnPropertyChanged(nameof(EnableGrouping));
            OnPropertyChanged(nameof(Scheduling));
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
