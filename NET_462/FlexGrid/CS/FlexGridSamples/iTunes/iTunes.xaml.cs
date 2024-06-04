using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using C1.WPF.FlexGrid;

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for iTunes.xaml
    /// </summary>
    public partial class iTunes : UserControl
    {
        public iTunes()
        {
            InitializeComponent();
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

            // configure search box
            _srchTunes.View = view;
            foreach (string name in "Artist|Album|Name".Split('|'))
            {
                _srchTunes.FilterProperties.Add(typeof(Song).GetProperty(name));
            }

            // show media library summary (using Linq)
            view.CollectionChanged += songs_CollectionChanged;
            UpdateSongStatus();
        }
        void songs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateSongStatus();
        }

        // update song status
        void UpdateSongStatus()
        {
            var view = _flexiTunes.ItemsSource as ICollectionView;
            var songs = view.OfType<Song>();
            _txtSongs.Text = string.Format("{0:n0} Artists; {1:n0} Albums; {2:n0} Songs; {3:n0} MB of storage; {4:n2} days of music.",
                (from s in songs select s.Artist).Distinct().Count(),
                (from s in songs select s.Album).Distinct().Count(),
                (from s in songs select s.Name).Count(),
                (double)(from s in songs select s.Size / 1024.0 / 1024.0).Sum(),
                (double)(from s in songs select s.Duration / 1000.0 / 3600.0 / 24.0).Sum());
        }

        // turn ownerdraw on and off
        void _chkOwnerDraw_Click(object sender, RoutedEventArgs e)
        {
            _flexiTunes.CellFactory = _chkOwnerDraw.IsChecked.Value
                ? new MusicCellFactory()
                : null;
        }

        // collapse/expand groups
        void _btnShowArtists_Click(object sender, RoutedEventArgs e)
        {
            ShowOutline(0);
        }
        void _btnShowAlbums_Click(object sender, RoutedEventArgs e)
        {
            ShowOutline(1);
        }
        void _btnShowSongs_Click(object sender, RoutedEventArgs e)
        {
            ShowOutline(int.MaxValue);
        }
        void ShowOutline(int level)
        {
            var rows = _flexiTunes.Rows;
            using (rows.DeferNotifications())
            {
                foreach (var gr in rows.OfType<GroupRow>())
                {
                    gr.IsCollapsed = gr.Level >= level;
                }
            }
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
