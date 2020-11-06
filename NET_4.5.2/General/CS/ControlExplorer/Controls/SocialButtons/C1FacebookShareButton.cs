using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Xml.Linq;
using C1.WPF;
using System.Collections.Generic;

namespace ControlExplorer
{
    public class C1FacebookShareButton : C1HyperlinkButton
    {
        #region ** Title

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(C1FacebookShareButton), new PropertyMetadata(""));

        #endregion

        #region ** Counter

        public string Counter
        {
            get { return (string)GetValue(CounterProperty); }
            set { SetValue(CounterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Counter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CounterProperty =
            DependencyProperty.Register("Counter", typeof(string), typeof(C1FacebookShareButton), new PropertyMetadata("0"));


        #endregion

        #region ** Uri

        public static readonly DependencyProperty UrlProperty =
          DependencyProperty.Register("Url", typeof(string), typeof(C1FacebookShareButton),
          new PropertyMetadata(null, OnUriChanged));

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        static void OnUriChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var button = (C1FacebookShareButton)sender;
            button.UpdateUri();
        }

        #endregion

        #region ** constructor

        public C1FacebookShareButton()
        {
            DefaultStyleKey = typeof(C1FacebookShareButton);
            _wc = new WebClient();
        }

        #endregion

        #region ** implementation

        const string svcurl = @"https://graph.facebook.com?id={0}&fields=og_object{{engagement{{count}}}}"; // <<IP>> new graph api
        const string navurl = @"http://www.facebook.com/sharer/sharer.php?s=100&p[title]={1}&p[url]={0}"; 
        
        WebClient _wc;
        int maxretries = 10;
        int retry = 0;


        void UpdateUri()
        {
            this.NavigateUri = GetNavigateUrl();
            this.TargetName = "_blank";
            if (_wc.IsBusy)
                return;

            _wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_wc_DownloadStringCompleted);
            BeginDownload();
        }

        void BeginDownload()
        {
            try
            {
                if (Url != null)
                {
                    string s = string.Format(svcurl, Uri.EscapeUriString(Url));
                    _wc.DownloadStringAsync(new Uri(s));
                }
            }
            catch
            {
                // in case facebook is not accessible. We don't need to break application.
            }
        }

        void _wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                Counter = GetCount(e.Result).ToString();
            }
            else if (e.Error != null && e.Error.Message.Contains("400"))
            {
                // bad url, probably facebook API changed or something like this, do nothing
                System.Diagnostics.Trace.WriteLine(e.Error.Message);
            }
            else
            {
                if (retry++ < maxretries)
                    BeginDownload();
            }
        }

        int GetCount(string xml)
        {
            try
            {
                var json = new System.Web.Script.Serialization.JavaScriptSerializer().DeserializeObject(xml) as Dictionary<string, object>;
                while (json != null && !json.ContainsKey("engagement"))
                {
                    json = json.ElementAt(0).Value as Dictionary<string, object>;
                }
                if (json != null)
                {
                    // get engagements count
                    var count = (int)((Dictionary<string, object>)json["engagement"])["count"];
                    return count;
                }
            }
            catch
            {
                // in case if facebook changes api or something else of this kind. We don't need to break application
            }
            return 0;
        }

        private Uri GetNavigateUrl()
        {
            if (Url == null)
                return null;
            // escape strings as data, not as uri. In other case relative links (with '#' symbol) won't work
            return new Uri(string.Format(navurl, Uri.EscapeDataString(Url), Uri.EscapeDataString(Title)), UriKind.Absolute);
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateUri();
        }
    }
}
