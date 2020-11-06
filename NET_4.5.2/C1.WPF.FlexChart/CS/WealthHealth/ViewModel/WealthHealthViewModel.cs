using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Collections.Generic;

namespace WealthHealth
{
    public class WealthHealthViewModel : DependencyObject, INotifyPropertyChanged
    {
        DataService dataService = new DataService();
        List<Country> _countries;
        string _content;
        string _trackContent;
        const int YearMin = 1800;
        const int YearMax = 2009;
        const double AnimLength = 15000;
        const string PauseTip = "\uE102";
        const string ResumeTip = "\uE103";
        Storyboard _sb;

        public WealthHealthViewModel()
        {
            _countries = dataService.CreateData();
            ToggleAnimation();
        }

        public List<Country> Countries
        {
            get
            {
                return _countries;
            }
            set
            {
                if (_countries != null)
                {
                    _countries = value;
                    NotifyPropertyChanged("Countries");
                }
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    NotifyPropertyChanged("Content");
                }
            }
        }

        public PlayCommand PlayAnimation
        {
            get
            {
                return new PlayCommand(()=> { PerformAnimation(); });
            }
        }

        public string TrackContent
        {
            get
            {
                return _trackContent;
            }
            set
            {
                if (_trackContent != value)
                {
                    _trackContent = value;
                    NotifyPropertyChanged("TrackContent");
                }
            }
        }

        void UpdateData(int year)
        {
            Countries = dataService.UpdateData(year);
        }

        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(int), typeof(WealthHealthViewModel), new PropertyMetadata(1800, OnYearPropertyChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        static void OnYearPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var w = obj as WealthHealthViewModel;
            if (w != null)
            {
                var newValue = (int)e.NewValue;
                w.UpdateData(newValue);
            }
        }

        void ToggleAnimation()
        {
            var animation = new Int32Animation()
            {
                From = YearMin,
                To = YearMax,
                Duration = new Duration(TimeSpan.FromMilliseconds(AnimLength))
            };
            if (_sb == null)
            {
                _sb = new Storyboard();
                _sb.Completed += _sb_Completed;
                _sb.Children.Add(animation);
            }
            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Year"));
            _sb.Begin();
            _content = ResumeTip;
        }

        private void _sb_Completed(object sender, EventArgs e)
        {
            Content = PauseTip;
        }

        void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        void PerformAnimation()
        {
            if (_sb != null)
            {
                if (Year == YearMax)
                {
                    Content = ResumeTip;
                    _sb.Begin();
                }
                else
                {
                    if (_sb.GetCurrentProgress() == 1 || _sb.GetIsPaused())
                    {
                        Content = ResumeTip;
                        _sb.Seek(TimeSpan.FromMilliseconds((double)(Year - YearMin) / (double)(YearMax - YearMin) * AnimLength), TimeSeekOrigin.BeginTime);
                        _sb.Resume();
                    }
                    else
                    {
                        Content = PauseTip;
                        _sb.Pause();
                    }
                }
            }
        }

        public void StopAnimation()
        {
            if (_sb != null && !_sb.GetIsPaused())
            {
                _sb.Pause();
                Content = PauseTip;
            }
        }
    }
}
