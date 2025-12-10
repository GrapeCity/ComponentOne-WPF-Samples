using C1.Util.DX.DirectWrite;
using C1.WPF.GanttView;
using Localization.DataSource;
using Localization.Interface;
using Localization.Service;
using Localization.ViewModel;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Localization
{
    public partial class MainWindow : Window
    {
        public LocalizationViewModel UIStrings { get; } = new();

        private readonly ICultureLocalizationService _localizationService = new CultureLocalizationService();
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();
            InitializeLocalizationCombo();
            
        }


        /// <summary>
        /// Initialize the Localization combination
        /// </summary>
        private void InitializeLocalizationCombo()
        {
            var cultures = _localizationService.GetSupportedCultures();

            localizationCombo.ItemsSource = cultures;
            localizationCombo.DisplayMemberPath = "DisplayName";

            localizationCombo.SelectedItem = _localizationService.GetClosestMatchOrDefault();

            localizationCombo.SelectionChanged += (_, __) =>
            {
                var selectedCulture = (CultureInfo)localizationCombo.SelectedItem;

                _localizationService.ApplyCulture(selectedCulture);
                Language = XmlLanguage.GetLanguage(selectedCulture.Name);


                if (selectedCulture != null)
                {
                    var culture = selectedCulture;

                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                    UIStrings.Refresh();
                }
            };
        }
    }
}
