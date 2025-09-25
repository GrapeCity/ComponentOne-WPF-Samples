using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace ThemeExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            Title = Properties.Resources.Title;
            InitializeComponent();
        }
     }
}
