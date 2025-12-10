using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace DynamicTreeView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ///<inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var culture = CultureInfo.CurrentUICulture;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture.Name)));
        }
    }

}
