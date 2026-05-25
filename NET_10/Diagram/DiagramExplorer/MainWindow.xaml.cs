using C1.Diagram;
using DiagramExplorer.ViewModel;
using System.ComponentModel;
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

#pragma warning disable 1591
namespace DiagramExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ISampleItem? sample;

        bool darkTheme = false;

        public MainWindow()
        {
            InitializeComponent();
            
            SwitchTheme(new Uri("Themes/LightTheme.xaml", UriKind.Relative));

            Title = "WPF FlexDiagram Explorer";

            var dataSource = new SampleDataSource();
            DataContext = dataSource;

            comboBoxDirection.ItemsSource = Enum.GetValues<C1.Diagram.DiagramDirection>();
            comboBoxDirection.SelectionChanged += (s, e) =>
            {
                var diagram = Sample?.Diagram;
                if (diagram != null)
                    diagram.Direction = (DiagramDirection)comboBoxDirection.SelectedItem;
            };

            comboBoxEdgeRouting.ItemsSource = Enum.GetValues<C1.Diagram.EdgeRouting>();
            comboBoxEdgeRouting.SelectionChanged += (s, e) =>
            {
                var diagram = Sample?.Diagram;
                if (diagram != null)
                    diagram.EdgeRouting = (EdgeRouting)comboBoxEdgeRouting.SelectedItem;
            };

            comboBoxPalette.ItemsSource = Enum.GetValues<C1.Chart.Palette>();
            comboBoxPalette.SelectionChanged += (s, e) =>
            {
                var diagram = Sample?.Diagram;
                if (diagram != null)
                    diagram.Palette = (C1.Chart.Palette)comboBoxPalette.SelectedItem;
            };

            Loaded += (s, e) =>
            {
                var tvi = treeView.ItemContainerGenerator.ContainerFromItem(dataSource.AllItems[0])
                    as TreeViewItem;

                if (tvi != null)
                {
                    tvi.IsSelected = true;
                }
            };
        }

        public ISampleItem? Sample 
        {
            get => sample;
            set
            {
                sample = value;
                sampleContent.Content = sample?.Sample;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sample)));
                samplePage.DataContext = sample;
                
                var diagram = sample?.Diagram;
                if (diagram != null)
                {
                    comboBoxDirection.SelectedValue = diagram.Direction;
                    comboBoxEdgeRouting.SelectedValue = diagram.EdgeRouting;
                }

                if (sample != null)
                {
                    comboBoxDirection.Visibility = sample.Controls.Contains("Direction") ? Visibility.Visible : Visibility.Collapsed;
                    comboBoxEdgeRouting.Visibility = sample.Controls.Contains("EdgeRouting") ? Visibility.Visible : Visibility.Collapsed;
                    comboBoxPalette.Visibility = sample.Controls.Contains("Palette") ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Sample = treeView.SelectedItem as ISampleItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            darkTheme = !darkTheme;

            if(darkTheme)
                SwitchTheme(new Uri("Themes/DarkTheme.xaml", UriKind.Relative));
            else
                SwitchTheme(new Uri("Themes/LightTheme.xaml", UriKind.Relative));

            buttonTheme.Content = darkTheme ? "☀️" : "🌙";
        }

        void SwitchTheme(Uri themeUri)
        {
            // Create a new resource dictionary for the selected theme
            ResourceDictionary newTheme = new ResourceDictionary() { Source = themeUri };

            // Clear existing theme dictionaries (optional, depending on your setup)
            Application.Current.Resources.MergedDictionaries.Clear();

            // Add the new theme
            Application.Current.Resources.MergedDictionaries.Add(newTheme);
        }
    }
}