using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Threading;

namespace Localization
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			InitializeComponent();
            cmbCultures.Items.Add("default");
            cmbCultures.Items.Add("ar-SA");
            cmbCultures.Items.Add("cs-CZ");
            cmbCultures.Items.Add("da-DK");
            cmbCultures.Items.Add("de-DE");
            cmbCultures.Items.Add("el-GR");
            cmbCultures.Items.Add("en-US");
            cmbCultures.Items.Add("es-ES");
            cmbCultures.Items.Add("fi-FI");
            cmbCultures.Items.Add("fr-FR");
            cmbCultures.Items.Add("he-IL");
            cmbCultures.Items.Add("it-IT");
            cmbCultures.Items.Add("ja-JP");
            cmbCultures.Items.Add("nl-NL");
            cmbCultures.Items.Add("nb-NO");
            cmbCultures.Items.Add("pl-PL");
            cmbCultures.Items.Add("pt-BR");
            cmbCultures.Items.Add("ru-RU");
            cmbCultures.Items.Add("ro-RO");
            cmbCultures.Items.Add("sk-SK");
            cmbCultures.Items.Add("sv-SE");
            cmbCultures.Items.Add("tr-TR");
            cmbCultures.Items.Add("zh");
            cmbCultures.Items.Add("zh-Hans");
            cmbCultures.Items.Add("zh-Hant");
            CultureInfo currCulture = CultureInfo.CurrentUICulture;
            object o = null; // current culture
            foreach (string cultureName in cmbCultures.Items)
            {
                if (!cultureName.Equals("default"))
                {
                    if (cultureName.StartsWith(currCulture.TwoLetterISOLanguageName) ||
                        (currCulture.TwoLetterISOLanguageName == "no" && cultureName.Equals("nb-NO")))
                    {
                        o = cultureName;
                        break;
                    }
                }
            }
            if (o != null)
            {
                cmbCultures.SelectedItem = o;
            }
            else
            {
                cmbCultures.SelectedIndex = 0;
            }
            sched1.StyleChanged += new EventHandler<RoutedEventArgs>(sched1_StyleChanged);
            sched1_StyleChanged(null, null);
        }

        void sched1_StyleChanged(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state according to the current C1Scheduler view
            if (sched1.Style == sched1.WorkingWeekStyle)
            {
                btnWorkWeek.IsChecked = true;
            }
            else if (sched1.Style == sched1.WeekStyle)
            {
                btnWeek.IsChecked = true;
            }
            else if (sched1.Style == sched1.MonthStyle)
            {
                btnMonth.IsChecked = true;
            }
            else if (sched1.Style == sched1.OneDayStyle)
            {
                btnDay.IsChecked = true;
            }
            else
            {
                btnTimeLine.IsChecked = true;
            }
        }

        private void cmbCultures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cultureName = cmbCultures.SelectedIndex == 0 ? "en-US" : (string)cmbCultures.SelectedItem;
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(cultureName);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
        }

	}
}
