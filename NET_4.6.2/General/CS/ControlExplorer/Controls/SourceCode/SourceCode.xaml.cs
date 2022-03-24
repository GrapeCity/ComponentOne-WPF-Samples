using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using C1.C1Zip;
using C1.WPF;

namespace ControlExplorer
{
    /// <summary>
    /// Interaction logic for SourceCode.xaml
    /// </summary>
    public partial class SourceCode : UserControl
    {
        SourceCodeViewModel _viewData = null;
        internal bool needsRefresh { get; set; }
        internal bool ContainsSample { get; set; }
        bool IsShown
        {
            get
            {
                var tabItem = this.Parent as C1TabItem;
                return tabItem != null && tabItem.IsSelected;
            }
        }
        public SourceCode()
        {
            InitializeComponent();
            needsRefresh = false;
            this.Loaded += SourceCode_Loaded;
            if (_viewData == null)
                _viewData = new SourceCodeViewModel();
            this.DataContext = _viewData;
            _viewData.XamlFileName = "*.xaml";
            _viewData.CodeFileName = "*.xaml.cs";
        }

        void SourceCode_Loaded(object sender, RoutedEventArgs e)
        {
            if(IsShown && needsRefresh)
                ShowSourceCodes();
        }

        #region Source
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                "Source",
                typeof(string),
                typeof(SourceCode),
                new PropertyMetadata(OnSourcePropertyChanged));
        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SourceCode sender = d as SourceCode;
            sender.needsRefresh = true;
            if(sender.IsShown)
                sender.ShowSourceCodes();
        }
        #endregion
        void ShowSourceCodes()
        {
            progressIndicator.IsActive = true;
            try
            {
                StreamResourceInfo resource = null;
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
                    resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + $";component/Resources/{ci}SamplesCodes.zip", UriKind.Relative));
                }
                catch
                {
                    resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/SamplesCodes.zip", UriKind.Relative));
                }
                
                var zipFile = resource.Stream;
                if (zipFile != null && !String.IsNullOrEmpty(Source))
                {
                    var sourceArray = Source.Split('\\');
                    string xaml = String.Format("{0}/{1}", sourceArray.First(), sourceArray.Last());
                    string cs = String.Format("{0}/{1}.cs", sourceArray.First(), sourceArray.Last());
                    using (C1ZipFile zip = new C1ZipFile(zipFile))
                    {
                        if(zip.Entries.Contains(xaml) && zip.Entries.Contains(cs))
                        {
                            _viewData.XamlFileName = sourceArray.Last();
                            _viewData.Xaml = GetContent(zip.Entries[xaml]);

                            _viewData.CodeFileName = sourceArray.Last() + ".cs";
                            _viewData.Code = GetContent(zip.Entries[cs]);

                            this.DataContext = null;
                            this.DataContext = _viewData;
                            needsRefresh = false;
                        }
                        else
                        {
                            ShowMessageBox();
                        }
                    }
                }
                else
                {
                    ShowMessageBox();
                }
            }
            catch
            {
                ShowMessageBox();
            }
            finally
            {
                progressIndicator.IsActive = false;
            }
        }

        string GetContent(C1ZipEntry entry)
        {
            using(Stream entryStream = entry.OpenReader())
            {
                using(StreamReader streamReader = new StreamReader(entryStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        void ShowMessageBox()
        {
            if(ContainsSample)
                //no sample source found
                MessageBox.Show(ControlExplorer.Properties.Resources.NoSampleFound_Text);
        }
    }
}
