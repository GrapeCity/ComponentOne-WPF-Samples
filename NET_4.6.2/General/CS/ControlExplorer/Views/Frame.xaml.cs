using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows.Navigation;
using System.Windows.Controls;
using C1.WPF;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;

namespace ControlExplorer
{
    public partial class Frame : Window
    {
        Uri _currentUri, _targetUri;
        private Uri _navigatingUri = null;
        private NavigationMode _navigationMode = NavigationMode.New;
        Queue<Storyboard> _storyboardsToRun = new Queue<Storyboard>();
        FrameworkElement _homePage = null;

        private const string LOAD_STORYBOARD_KEY = "LoadStoryboard";
        private const string SAMPLEURL_PREFIX = "Views/SamplePage.xaml#";

        public Frame()
        {
            InitializeComponent();
            this.Title = ControlExplorer.Properties.Resources.FramePage_TitleText;

            string ci = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if ("en".Equals(ci))
            {
                ci = "";
            }
            else
            {
                ci += "/";
            }
            try
            {
                logo.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri($"/Resources/{ci}headerLogo.png", UriKind.Relative));
                this.Icon = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri($"pack://application:,,,/WPFControlExplorer;component/Resources/{ci}C1-ball.ico"));
            }
            catch
            {
                logo.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Resources/headerLogo.png", UriKind.Relative));
                this.Icon = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri("pack://application:,,,/WPFControlExplorer;component/Resources/C1-ball.ico"));
            }
        }

        public void ContentLoaded(Uri currentUri, Uri targetUri)
        {
            _homePage = NavigationFrame.Content as FrameworkElement;
            if (currentUri == targetUri)
                return;

            if (targetUri == null)
            {
                return;
            }
            if (currentUri == null)
            {
                HandleStartFromEmptyPage(targetUri);
            }
            else if (!currentUri.OriginalString.Equals(targetUri.OriginalString))
            {
                if (IsSampleUri(currentUri) && IsSampleUri(targetUri) && !InDifferentControls(currentUri, targetUri))
                {
                    HandleSampleChange(currentUri, targetUri);
                }
                else if (InDifferentControls(currentUri, targetUri) || IsHomeUri(currentUri) || IsSampleUri(targetUri))
                {
                    HandleLoadFromRight(currentUri, targetUri);
                }
                else if (IsSampleUri(currentUri) || IsHomeUri(targetUri))
                {
                    HandleLoadFromLeft(currentUri, targetUri);
                }
            }
            // run storyboards
            st_Completed(null, null);
        }

        private void HandleStartFromEmptyPage(Uri targetUri)
        {
            Storyboard storyboard = Resources[LOAD_STORYBOARD_KEY] as Storyboard;
            if (storyboard != null)
            {
                _storyboardsToRun.Enqueue(storyboard);
            }
            AddContentLoadedAnimation(LOAD_STORYBOARD_KEY, false);
        }

        private void HandleLoadFromLeft(Uri currentUri, Uri targetUri)
        {
            Storyboard storyboard = Resources["LoadFromLeftStoryboard"] as Storyboard;
            if (storyboard != null)
            {
                _storyboardsToRun.Enqueue(storyboard);
            }
        }

        private void HandleLoadFromRight(Uri currentUri, Uri targetUri)
        {
            Storyboard storyboard = Resources["LoadFromRightStoryboard"] as Storyboard;
            if (storyboard != null)
            {
                _storyboardsToRun.Enqueue(storyboard);
            }
            if (IsSampleUri(targetUri))
            {
                AddContentLoadedAnimation(LOAD_STORYBOARD_KEY, true);
            }
        }

        private void HandleSampleChange(Uri currentUri, Uri targetUri)
        {
            AddContentLoadedAnimation("ChangeSampleStoryboard", true);
        }

        private void AddContentLoadedAnimation(string key, bool enque)
        {
            if (_homePage != null)
            {
                var contentStoryboard = _homePage.Resources[key] as Storyboard;
                if (contentStoryboard != null)
                {
                    if (enque)
                    {
                        _storyboardsToRun.Enqueue(contentStoryboard);
                    }
                    else
                    {
                        // run immediately
                        contentStoryboard.Begin();
                    }
                }
            }
        }

        void st_Completed(object sender, EventArgs e)
        {
            Storyboard st = sender as Storyboard;
            if (st != null)
            {
                st.Completed -= new EventHandler(st_Completed);
            }
            if (_storyboardsToRun.Count > 0)
            {
                st = _storyboardsToRun.Dequeue();
                if (st != null)
                {
                    st.Completed += new EventHandler(st_Completed);
                    st.Begin();
                }
            }
        }

        public void ContentUnloaded(Uri currentUri, Uri targetUri, Action completed)
        {
            SearchViewModel.Instance.ShowSearchList = false;

            Storyboard storyboard = null;
            if ((IsHomeUri(currentUri) && !IsHomeUri(targetUri)) || IsSampleUri(targetUri))
            {
                storyboard = Resources["UnloadToLeftStoryboard"] as Storyboard;
            }
            else if ((IsSampleUri(currentUri) && !IsSampleUri(targetUri)) || IsHomeUri(targetUri))
            {
                storyboard = Resources["UnloadToRightStoryboard"] as Storyboard;
            }
            if (storyboard != null)
            {
                EventHandler c = null;
                c = (s, e) =>
                {
                    storyboard.Completed -= c;
                    completed();
                };
                storyboard.Completed += c;
                storyboard.Begin();
            }
            else
            {
                completed();
            }
        }

        public void Navigate(Uri targetUri, Uri currentUri)
        {
            try
            {
                NavigationFrame.Navigate(targetUri);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("** " + e.ToString());
                System.Diagnostics.Debug.WriteLine("** '" + targetUri.OriginalString + "'");
                ShowNavigationError(targetUri);
            }
        }

        public void ShowNavigationError(Uri targetUri)
        {
            C1MessageBox.Show(targetUri.OriginalString + "\r\n" + new System.IO.FileNotFoundException().Message,
               "Navigation Error", C1MessageBoxIcon.Warning,
               new Action<MessageBoxResult>((r) =>
               {
               }));
        }

        private bool IsHomeUri(Uri currentUri)
        {
            return currentUri.OriginalString.Contains("HomePage.xaml");
        }

        private bool IsSampleUri(Uri currentUri)
        {
            return !IsHomeUri(currentUri);
        }

        private bool InDifferentControls(Uri currentUri, Uri targetUri)
        {
            bool result = false;
            if (IsSampleUri(targetUri) && IsSampleUri(currentUri))
            {
                result = GetControlFromUri(targetUri) != GetControlFromUri(currentUri);
            }

            return result;
        }

        private string GetControlFromUri(Uri uri)
        {
            string control = string.Empty;
            string originalString = uri.OriginalString;
            if (originalString.Contains(SAMPLEURL_PREFIX))
            {
                originalString = originalString.Replace(SAMPLEURL_PREFIX, string.Empty);
            }
            control = originalString.Split('/')[0];
            return control;
        }

        private void NavigationFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (_navigatingUri == null ||
                _navigatingUri.OriginalString != e.Uri.OriginalString)
            {
                Uri uri = e.Uri ?? NavigationFrame.CurrentSource;
                #region ** uri mappings
                if (IsSampleUri(uri))
                {
                    _navigatingUri = new Uri(SAMPLEURL_PREFIX + uri.OriginalString, UriKind.Relative);
                }
                else
                {
                    _navigatingUri = uri;
                }

                #endregion

                _navigationMode = e.NavigationMode;
                e.Cancel = true;

                Action a = () =>
                {
                    Loader.Visibility = Visibility.Collapsed;
                    _targetUri = _navigatingUri;
                    NavigationFrame.Navigate(_navigatingUri);
                };
                if (_currentUri != null && InDifferentControls(_navigatingUri, _currentUri) || _currentUri != null && (!IsSampleUri(_currentUri) || !IsSampleUri(_navigatingUri)))
                {
                    ContentUnloaded(_currentUri, _navigatingUri, a);
                }
                else if (_navigatingUri != null && _currentUri != _navigatingUri)
                {
                    Dispatcher.BeginInvoke(new Action(a));
                }
            }
        }

        private void NavigationFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            SearchViewModel.Instance.SearchCriteria = string.Empty;
            _navigatingUri = null;
            UpdateLayout();
            ContentLoaded(_currentUri, _targetUri);
            _currentUri = _targetUri;
        }

        private void NavigationFrame_FragmentNavigation(object sender, System.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            var samplePage = NavigationFrame.Content as SamplePage;
            if (samplePage != null)
            {
                samplePage.OnNavigatedTo(e.Fragment);
            }
        }
    }

    public class UriConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Uri)
            {
                return ((Uri)value).ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                try
                {
                    return new Uri(value as string);
                }
                catch
                {
                }
            }
            return value;
        }

        #endregion
    }
}
