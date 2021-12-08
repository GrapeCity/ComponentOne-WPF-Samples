using C1.WPF.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for DemoMaskedTextBox.xaml
    /// </summary>
    public partial class DemoMaskedTextBox : UserControl
    {
        C1MaskedTextBoxSettings dataContext;

        public DemoMaskedTextBox()
        {
            InitializeComponent();
            Tag = Properties.Resources.MaskedDemoDes;
            dataContext = new C1MaskedTextBoxSettings();
            dataContext.Mask = C1MaskedTextBoxSettings.MaskTemplates[0].Mask;
            this.DataContext = dataContext;
        }

        private void lstMaskTemplates_SelectionChanged_1(object sender, C1.WPF.Core.SelectionChangedEventArgs<int> e)
        {
            var maskTemplate = lstMaskTemplates.SelectedItem as MaskTemplate;
            dataContext.Mask = maskTemplate.Mask;
        }
    }

    public class C1MaskedTextBoxSettings : INotifyPropertyChanged
    {
        private string _mask;
        public string Mask { get => _mask; set => SetProperty(ref _mask, value); }

        private string _promtChar;
        public string PromptChar { get => _promtChar; set => SetProperty(ref _promtChar, value); }

        private MaskFormat _maskFormat = MaskFormat.ExcludePromptAndLiterals;
        public MaskFormat MaskFormat { get => _maskFormat; set => SetProperty(ref _maskFormat, value); }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

        public static readonly IList<MaskFormat> MaskFormats = new List<MaskFormat> { MaskFormat.IncludePrompt, MaskFormat.IncludeLiterals,
            MaskFormat.IncludePromptAndLiterals, MaskFormat.ExcludePromptAndLiterals };

        public static readonly IList<MaskTemplate> MaskTemplates = new List<MaskTemplate>
        {
            new MaskTemplate()
            {
                Mask = "000-00-0000",
                Name = "Social Security Number",
                SampleResult = "213-21-3213"
            },
            new MaskTemplate()
            {
                Mask = "00000",
                Name = "Zip Code",
                SampleResult = "12312"
            },
            new MaskTemplate()
            {
                Mask = "00000-0000",
                Name = "Zip+4 Code",
                SampleResult = "32132-1321"
            },
            new MaskTemplate()
            {
                Mask = "(999) 000-0000",
                Name = "Phone Number",
                SampleResult = "(213) 123-2132"
            },
            new MaskTemplate()
            {
                Mask = "<LLL",
                Name = "Shift down",
                SampleResult = "aaa"
            },
            new MaskTemplate()
            {
                Mask = ">LLL",
                Name = "Shift up",
                SampleResult = "AAA"
            },
            new MaskTemplate()
            {
                Mask = "\\\\### ###",
                Name = "Escape",
                SampleResult = "\\321 232"
            },
            new MaskTemplate()
            {
                Mask = "[\\9\\9] \\### \\###",
                Name = "Complicated Escape",
                SampleResult = "[99] #21 #21"
            },
            new MaskTemplate()
            {
                Mask = "[\\9\\9] \\### \\### >LLL <LLL",
                Name = "Complicated Escape & Shift",
                SampleResult = "[99] #32 #32 AAA aaa"
            }
        };

        public static char DefaultPrompt = '_';
        public static string NotAvailableText = "N/A";
    }

    public class MaskTemplate
    {
        public string Name { get; set; }
        public string Mask { get; set; }
        public string SampleResult { get; set; }
    }
}
