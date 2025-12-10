using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace GridShowCase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            InitializeComponent();
            Title = Properties.Resources.Title;
        }
        
    }
}
