using ControlExplorer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace ControlExplorer2010
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
               typeof(FrameworkElement),
               new FrameworkPropertyMetadata(
               System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            string ci = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if ("en".Equals(ci))
            {
                ci = "";
            }
            else
            {
                ci += "/";
            }

            SplashScreen splash = null;
            try
            {
                splash = new SplashScreen($"/Resources/{ci}WPF-SplashScreen.png");
                splash.Show(true);
            }
            catch
            {
                splash = new SplashScreen("/Resources/WPF-SplashScreen.png"); //Fallback to default
                splash.Show(true);
            }

            Frame frame = new Frame();
            frame.Show();
            base.OnStartup(e);
        }

    }
}
