using System;
using System.Windows;
using System.Windows.Controls;
using C1.WPF.Calendar;
using CalendarExplorer.Resources;

namespace CalendarExplorer
{
    /// <summary>
    ///     Interaction logic for CustomUI.xaml
    /// </summary>
    public partial class CustomUI : UserControl
    {
        public CustomUI()
        {
            InitializeComponent();

            Tag = AppResources.CustomUIDescription;

            calendar.ViewModeAnimation.AnimationMode = CalendarViewModeAnimationMode.ZoomOutIn;
            calendar.ViewModeAnimation.ScaleFactor = 1.1;
            calendar.ViewModeAnimation.Duration = TimeSpan.FromMilliseconds(150);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var date = (DateTime) (sender as Button).DataContext;

            LocalDataStorage.Note(date);

            calendar.Refresh();
        }
    }
}