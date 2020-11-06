using C1.WPF;
using C1.WPF.FlexGrid;
using C1.WPF.Theming;
using C1.WPF.Theming.BureauBlack;
using C1.WPF.Theming.C1Blue;
using C1.WPF.Theming.Cosmopolitan;
using C1.WPF.Theming.CosmopolitanDark;
using C1.WPF.Theming.ExpressionDark;
using C1.WPF.Theming.ExpressionLight;
using C1.WPF.Theming.Material;
using C1.WPF.Theming.MaterialDark;
using C1.WPF.Theming.Office2007;
using C1.WPF.Theming.Office2010;
using C1.WPF.Theming.Office2013;
using C1.WPF.Theming.Office2016;
using C1.WPF.Theming.ShinyBlue;
using C1.WPF.Theming.WhistlerBlue;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace ShowCase
{
    /// <summary>
    /// Interaction logic for ShowCase.xaml
    /// </summary>
    public partial class ShowCaseSample : UserControl
    {
        public ShowCaseSample()
        {
            InitializeComponent();
            AddColumnFooter(flexGrid);
            cmbTheme.ItemsSource = typeof(C1AvailableThemes).GetEnumValues<C1AvailableThemes>();
            cmbTheme.SelectedItem = C1AvailableThemes.Default;
        }
        void AddColumnFooter(C1.WPF.FlexGrid.C1FlexGrid flex)
        {
            var gr = new GroupRow();

            // customize appearance of the new row
            gr.CellStyle = new CellStyle() 
            { 
                BorderThickness = new Thickness(0), 
                FontWeight = FontWeights.Bold, 
                HorizontalAlignment = HorizontalAlignment.Right
            };

            // add the row to the ColumnFooters GridPanel
            flex.ColumnFooters.Rows.Add(gr);
            
            var aggregatePriceValue = flexGrid.GetAggregate(Aggregate.Average, new CellRange(0, 5, flexGrid.Rows.Count - 1, 5));
            var aggregateDiscountValue = flexGrid.GetAggregate(Aggregate.Average, new CellRange(0, 8, flexGrid.Rows.Count - 1, 8));
            
            gr[flexGrid.Columns["Price"]] = String.Format("Average Price: {0:C2}", aggregatePriceValue);
            gr[flexGrid.Columns["Discount"]] = String.Format("Average Discount : {0:F1}%", aggregateDiscountValue);
        }
        private void cmbTheme_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            var theme = C1ThemeFactory.GetTheme((C1AvailableThemes)cmbTheme.SelectedItem);

            C1Theme.ApplyTheme(LayoutRoot, theme);

            var adornerLayer = AdornerLayer.GetAdornerLayer(LayoutRoot);
            if (adornerLayer != null)
            {
                // this will apply theme to everything displayed in adorner, including any C1Window instances
                C1Theme.ApplyTheme(adornerLayer, theme);
            }

            ApplyThemeStyle();
        }
        private void ApplyThemeStyle()
        {
            foreach (var item in flexGrid.Columns)
            {
                item.Foreground = flexGrid.Foreground;
            }
        }

        private void CsvExport_Click(object sender, RoutedEventArgs e)
        {
            Export(C1.WPF.FlexGrid.FileFormat.Csv);
        }

        private void TextExport_Click(object sender, RoutedEventArgs e)
        {
            Export(C1.WPF.FlexGrid.FileFormat.Text);
        }

        private void HtmlExport_Click(object sender, RoutedEventArgs e)
        {
            Export(C1.WPF.FlexGrid.FileFormat.Html);
        }
        private void Export(FileFormat type)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Comma Separated Values(*.csv)|*.csv|" + "HTML File (*.htm;*.html)|*.htm;*.html|" + "Text File (*.txt)|*.txt";
            switch (type)
            {
                case FileFormat.Csv:
                    dlg.FilterIndex = 1; break;
                case FileFormat.Text:
                    dlg.FilterIndex = 3; break;
                case FileFormat.Html:
                    dlg.FilterIndex = 2; break;
            }

            if (dlg.ShowDialog() == true)
            {
                flexGrid.Save(dlg.FileName, type, SaveOptions.Formatted);
                Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
            }
        }

        private void MenuItem_VisibleColumn_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string columnName = (string)item.Header;
            flexGrid.Columns[columnName].Visible = !flexGrid.Columns[columnName].Visible;
        }

        private void MenuItem_ConditionalFormating_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            Point condition = (Point)item.CommandParameter;
            foreach (var row in flexGrid.Rows)
            {
                if (row["Discount"] != null)
                {
                    var discount = Double.Parse(row["Discount"].ToString());
                    if (discount > condition.X && discount < condition.Y)
                    {
                        if (row.Background == null)
                            row.Background = new SolidColorBrush(Colors.LightSkyBlue);
                        else
                            row.Background = null;
                    }
                }
            }
        }
    }
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value.ToString().ToUpper())
            {
                case "#FF000000": return "Black";
                case "#FFFFA500": return "Orange";
                case "#FFFF0000": return "Red";
                case "#FF008000": return "Green";
                case "#FF0000FF": return "Blue";
            }

            return value;
        }
    }
    public class DiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format("{0} %", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result;
            double.TryParse(value.ToString().Replace("%", ""), out result);
            return result;
        }
    }
    public static class C1ThemeFactory
    {
        static C1Theme _bureauBlack;
        static C1Theme _expDark;
        static C1Theme _expLight;
        static C1Theme _shinyBlue;
        static C1Theme _whistlerBlue;
        static C1Theme _cosmopolitan;
        static C1Theme _cosmopolitanDark;
        static C1Theme _office2007Blue;
        static C1Theme _office2007Black;
        static C1Theme _office2007Silver;
        static C1Theme _office2010Blue;
        static C1Theme _office2010Black;
        static C1Theme _office2010Silver;
        static C1Theme _office2013White;
        static C1Theme _office2013LightGray;
        static C1Theme _office2013DarkGray;
        static C1Theme _office2016Colorful;
        static C1Theme _office2016Black;
        static C1Theme _office2016DarkGray;
        static C1Theme _office2016White;
        static C1Theme _c1Blue;
        static C1Theme _material;
        static C1Theme _materialDark;

        public static C1Theme GetTheme(C1AvailableThemes pickedTheme)
        {
            C1Theme theme = null;
            switch (pickedTheme)
            {
                case C1AvailableThemes.BureauBlack:
                    if (_bureauBlack == null) _bureauBlack = new C1ThemeBureauBlack();
                    theme = _bureauBlack;
                    break;
                case C1AvailableThemes.ExpressionDark:
                    if (_expDark == null) _expDark = new C1ThemeExpressionDark();
                    theme = _expDark;
                    break;
                case C1AvailableThemes.ExpressionLight:
                    if (_expLight == null) _expLight = new C1ThemeExpressionLight();
                    theme = _expLight;
                    break;
                case C1AvailableThemes.ShinyBlue:
                    if (_shinyBlue == null) _shinyBlue = new C1ThemeShinyBlue();
                    theme = _shinyBlue;
                    break;
                case C1AvailableThemes.WhistlerBlue:
                    if (_whistlerBlue == null) _whistlerBlue = new C1ThemeWhistlerBlue();
                    theme = _whistlerBlue;
                    break;
                case C1AvailableThemes.Cosmopolitan:
                    if (_cosmopolitan == null) _cosmopolitan = new C1ThemeCosmopolitan();
                    theme = _cosmopolitan;
                    break;
                case C1AvailableThemes.CosmopolitanDark:
                    if (_cosmopolitanDark == null) _cosmopolitanDark = new C1ThemeCosmopolitanDark();
                    theme = _cosmopolitanDark;
                    break;
                //#if !CLR40
                case C1AvailableThemes.Office2007Blue:
                    if (_office2007Blue == null) _office2007Blue = new C1ThemeOffice2007Blue();
                    theme = _office2007Blue;
                    break;
                case C1AvailableThemes.Office2007Black:
                    if (_office2007Black == null) _office2007Black = new C1ThemeOffice2007Black();
                    theme = _office2007Black;
                    break;
                case C1AvailableThemes.Office2007Silver:
                    if (_office2007Silver == null) _office2007Silver = new C1ThemeOffice2007Silver();
                    theme = _office2007Silver;
                    break;
                case C1AvailableThemes.Office2010Blue:
                    if (_office2010Blue == null) _office2010Blue = new C1ThemeOffice2010Blue();
                    theme = _office2010Blue;
                    break;
                case C1AvailableThemes.Office2010Black:
                    if (_office2010Black == null) _office2010Black = new C1ThemeOffice2010Black();
                    theme = _office2010Black;
                    break;
                case C1AvailableThemes.Office2010Silver:
                    if (_office2010Silver == null) _office2010Silver = new C1ThemeOffice2010Silver();
                    theme = _office2010Silver;
                    break;
                case C1AvailableThemes.Office2013White:
                    if (_office2013White == null) _office2013White = new C1ThemeOffice2013White();
                    theme = _office2013White;
                    break;
                case C1AvailableThemes.Office2013LightGray:
                    if (_office2013LightGray == null) _office2013LightGray = new C1ThemeOffice2013LightGray();
                    theme = _office2013LightGray;
                    break;
                case C1AvailableThemes.Office2013DarkGray:
                    if (_office2013DarkGray == null) _office2013DarkGray = new C1ThemeOffice2013DarkGray();
                    theme = _office2013DarkGray;
                    break;
                //#endif
                case C1AvailableThemes.C1Blue:
                    if (_c1Blue == null) _c1Blue = new C1ThemeC1Blue();
                    theme = _c1Blue;
                    break;
                case C1AvailableThemes.Office2016Colorful:
                    if (_office2016Colorful == null) _office2016Colorful = new C1ThemeOffice2016Colorful();
                    theme = _office2016Colorful;
                    break;
                case C1AvailableThemes.Office2016Black:
                    if (_office2016Black == null) _office2016Black = new C1ThemeOffice2016Black();
                    theme = _office2016Black;
                    break;
                case C1AvailableThemes.Office2016DarkGray:
                    if (_office2016DarkGray == null) _office2016DarkGray = new C1ThemeOffice2016DarkGray();
                    theme = _office2016DarkGray;
                    break;
                case C1AvailableThemes.Office2016White:
                    if (_office2016White == null) _office2016White = new C1ThemeOffice2016White();
                    theme = _office2016White;
                    break;
                case C1AvailableThemes.Material:
                    if (_material == null) _material = new C1ThemeMaterial();
                    theme = _material;
                    break;
                case C1AvailableThemes.MaterialDark:
                    if (_materialDark == null) _materialDark = new C1ThemeMaterialDark();
                    theme = _materialDark;
                    break;
                default:
                    break;
            }
            return theme;
        }
    }
    public enum C1AvailableThemes
    {
        Default,
        Material,
        MaterialDark,
        Office2016Colorful,
        Office2016DarkGray,
        Office2016Black,
        Office2016White,
        Cosmopolitan,
        CosmopolitanDark,
        Office2013White,
        Office2013LightGray,
        Office2013DarkGray,
        Office2010Blue,
        Office2010Black,
        Office2010Silver,
        Office2007Blue,
        Office2007Black,
        Office2007Silver,
        BureauBlack,
        ExpressionDark,
        ExpressionLight,
        ShinyBlue,
        WhistlerBlue,
        C1Blue
    }
}
