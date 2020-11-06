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

namespace MediaPlayerSamples
{
    /// <summary>
    /// Interaction logic for DemoMediaPlayer.xaml
    /// </summary>
    public partial class DemoMediaPlayer : UserControl
    {
        public DemoMediaPlayer()
        {
            InitializeComponent();
        }

        #region Object model

        public bool IsShowingAds
        {
            get { return (bool)GetValue(IsShowingAdsProperty); }
            set { SetValue(IsShowingAdsProperty, value); }
        }

        public static readonly DependencyProperty IsShowingAdsProperty =
            DependencyProperty.Register("IsShowingAds", typeof(bool), typeof(DemoMediaPlayer),
            new FrameworkPropertyMetadata(false));

        public bool IsEnabled
        {
            get
            {
                return mediaPlayer.IsEnabled;
            }
            set
            {
                mediaPlayer.IsEnabled = value;
            }
        }

        public bool IsChapterListButtonVisible
        {
            get
            {
                return mediaPlayer.IsChapterListButtonVisible;
            }
            set
            {
                mediaPlayer.IsChapterListButtonVisible = value;
            }
        }

        public bool IsFullScreenButtonVisible
        {
            get
            {
                return mediaPlayer.IsFullScreenButtonVisible;
            }
            set
            {
                mediaPlayer.IsFullScreenButtonVisible = value;
            }
        }
        
        public bool IsItemListButtonVisible
        {
            get
            {
                return mediaPlayer.IsItemListButtonVisible;
            }
            set
            {
                mediaPlayer.IsItemListButtonVisible = value;
            }
        }

        public bool IsLoopButtonVisible
        {
            get
            {
                return mediaPlayer.IsLoopButtonVisible;
            }
            set
            {
                mediaPlayer.IsLoopButtonVisible = value;
            }
        }

        public bool IsNextButtonVisible
        {
            get
            {
                return mediaPlayer.IsNextButtonVisible;
            }
            set
            {
                mediaPlayer.IsNextButtonVisible = value;
            }
        }

        public bool IsPlayButtonVisible
        {
            get
            {
                return mediaPlayer.IsPlayButtonVisible;
            }
            set
            {
                mediaPlayer.IsPlayButtonVisible = value;
            }
        }

        public bool IsPositionSliderVisible
        {
            get
            {
                return mediaPlayer.IsPositionSliderVisible;
            }
            set
            {
                mediaPlayer.IsPositionSliderVisible = value;
            }
        }

        public bool IsPreviousButtonVisible
        {
            get
            {
                return mediaPlayer.IsPreviousButtonVisible;
            }
            set
            {
                mediaPlayer.IsPreviousButtonVisible = value;
            }
        }

        public bool IsStopButtonVisible
        {
            get
            {
                return mediaPlayer.IsStopButtonVisible;
            }
            set
            {
                mediaPlayer.IsStopButtonVisible = value;
            }
        }

        public bool IsTimePresenterVisible
        {
            get
            {
                return mediaPlayer.IsTimePresenterVisible;
            }
            set
            {
                mediaPlayer.IsTimePresenterVisible = value;
            }
        }

        public bool IsTitleVisible
        {
            get
            {
                return mediaPlayer.IsTitleVisible;
            }
            set
            {
                mediaPlayer.IsTitleVisible = value;
            }
        }

        public bool IsVolumeControlVisible
        {
            get
            {
                return mediaPlayer.IsVolumeControlVisible;
            }
            set
            {
                mediaPlayer.IsVolumeControlVisible = value;
            }
        }

        public bool SuperimposeButtonsFullScreen
        {
            get
            {
                return mediaPlayer.SuperimposeButtonsFullScreen;
            }
            set
            {
                mediaPlayer.SuperimposeButtonsFullScreen = value;
            }
        }

        public bool SuperimposeButtonsWindowed
        {
            get
            {
                return mediaPlayer.SuperimposeButtonsWindowed;
            }
            set
            {
                mediaPlayer.SuperimposeButtonsWindowed = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return mediaPlayer.Background;
            }
            set
            {
                mediaPlayer.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return mediaPlayer.Foreground;
            }
            set
            {
                mediaPlayer.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return mediaPlayer.BorderBrush;
            }
            set
            {
                mediaPlayer.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return mediaPlayer.MouseOverBrush;
            }
            set
            {
                mediaPlayer.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return mediaPlayer.PressedBrush;
            }
            set
            {
                mediaPlayer.PressedBrush = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return mediaPlayer.FocusBrush;
            }
            set
            {
                mediaPlayer.FocusBrush = value;
            }
        }

        public Brush ButtonBackground
        {
            get
            {
                return mediaPlayer.ButtonBackground;
            }
            set
            {
                mediaPlayer.ButtonBackground = value;
            }
        }

        public Brush ButtonForeground
        {
            get
            {
                return mediaPlayer.ButtonForeground;
            }
            set
            {
                mediaPlayer.ButtonForeground = value;
            }
        }

        #endregion

        #endregion

        private void mediaPlayer_Unloaded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }
    }
}
