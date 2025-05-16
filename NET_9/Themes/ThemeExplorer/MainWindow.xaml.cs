using System.Globalization;
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
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            Title = Properties.Resources.Title;
            InitializeComponent();
        }
     }
}
