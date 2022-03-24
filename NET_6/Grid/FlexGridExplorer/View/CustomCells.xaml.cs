using FlexGridExplorer.Resources;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for CustomCells.xaml
    /// </summary>
    public partial class CustomCells : UserControl
    {
        public CustomCells()
        {
            InitializeComponent();
            Tag = AppResources.CustomCellsDescription;
            BindITunesGrid();
        }

        void BindITunesGrid()
        {
            var songs = MediaLibrary.Load();
            var view = new MyCollectionView(songs);
            using (view.DeferRefresh())
            {
                view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(new PropertyGroupDescription("Artist"));
                view.GroupDescriptions.Add(new PropertyGroupDescription("Album"));
            }
            var fg = _flexiTunes;
            fg.CellFactory = new MusicCellFactory();
            fg.MergeManager = null; // << review this, should not merge cells with content
            fg.Columns["Duration"].ValueConverter = new SongDurationConverter();
            fg.Columns["Size"].ValueConverter = new SongSizeConverter();
            fg.ItemsSource = view;

        }

        // converter for artist/album groups
        public class GroupHeaderConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                // return group name only (no counts)
                var group = value as CollectionViewGroup;
                if (group != null && targetType == typeof(string))
                {
                    return group.Name;
                }

                // default
                return value;
            }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        // converter for song durations (stored in milliseconds)
        class SongDurationConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var ts = TimeSpan.FromMilliseconds((long)value);
                return ts.Hours == 0
                    ? string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds)
                    : string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        // converter for song sizes (return x.xx MB)
        class SongSizeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return string.Format("{0:n2} MB", (long)value / 1024.0 / 1024.0);
            }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
