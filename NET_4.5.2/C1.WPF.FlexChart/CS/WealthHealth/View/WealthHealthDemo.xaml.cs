using C1.WPF.Chart;
using System.Windows.Controls;
using System.Windows.Media;

namespace WealthHealth
{
    /// <summary>
    /// Interaction logic for WealthHealthDemo.xaml
    /// </summary>
    public partial class WealthHealthDemo : UserControl
    {
        public WealthHealthDemo()
        {
            InitializeComponent();
        }

        void Series_SymbolRendering(object sender, RenderSymbolEventArgs e)
        {
            var country = e.Item as Country;
            if (country != null)
            {
                int selectedIndex = flexChart.SelectedIndex;
                var fill = new SolidColorBrush(GetColorByRegion(country.Region));
                var engine = e.Engine;
                engine.SetStroke(null);
                if (selectedIndex == -1)
                {
                    fill = engine.SetOpacity(fill, 0.5) as SolidColorBrush;
                    engine.SetFill(fill);
                }
                else
                {
                    var dataContext = Root.DataContext as WealthHealthViewModel;
                    if (dataContext.Countries[selectedIndex] == country)
                    {
                        var strokeClr = (Color)ColorConverter.ConvertFromString("#b6ff00");
                        engine.SetStroke(new SolidColorBrush(strokeClr));
                        engine.SetStrokeThickness(6.0); 
                        engine.SetFill(fill);
                    }
                    else
                    {
                        fill = engine.SetOpacity(fill, 0.2) as SolidColorBrush;
                        engine.SetFill(fill);
                    }
                }
            }
        }

        Color GetColorByRegion(string region)
        {
            string clr = string.Empty;

            switch (region)
            {
                case "Sub-Saharan Africa":
                    clr = "#FF1F77B4";
                    break;
                case "South Asia":
                    clr = "#FFFF7F0E";
                    break;
                case "Middle East & North Africa":
                    clr = "#FF2CA02C";
                    break;
                case "America":
                    clr = "#FFD62728";
                    break;
                case "Europe & Central Asia":
                    clr = "#FF9467BD";
                    break;
                case "East Asia & Pacific":
                    clr = "#FF8C564B";
                    break;
            }
            if (string.IsNullOrEmpty(clr))
                return Colors.Black;

            return (Color)ColorConverter.ConvertFromString(clr);
        }

        private void flexChart_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var hitInfo = flexChart.HitTest(e.GetPosition(flexChart));
            Country selectedCountry = hitInfo.Item as Country;
            var dataContext = Root.DataContext as WealthHealthViewModel;
            if (selectedCountry != null && hitInfo.Distance < 3)
            {
                if (dataContext != null)
                {
                    tbTip.Visibility = System.Windows.Visibility.Collapsed;
                    tbTrack.Visibility = System.Windows.Visibility.Visible;
                    dataContext.TrackContent = selectedCountry.Name;
                }
            }
            else
            {
                tbTip.Visibility = System.Windows.Visibility.Visible;
                tbTrack.Visibility = System.Windows.Visibility.Collapsed;
            }
            dataContext.StopAnimation();
        }
    }
}
