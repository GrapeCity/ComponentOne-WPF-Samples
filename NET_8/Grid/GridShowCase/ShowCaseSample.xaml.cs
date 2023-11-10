using C1.WPF.Core;
using C1.WPF.Grid;
using C1.WPF.Menu;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace GridShowCase
{
    /// <summary>
    /// Interaction logic for ShowCase.xaml
    /// </summary>
    public partial class ShowCaseSample : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public ShowCaseSample()
        {
            InitializeComponent();
            Tag = Properties.Resources.Tag;
            flexGrid.Rows.CollectionChanged += FlexGridOnCollectionChanged;
        }

        private void FlexGridOnCollectionChanged(object sender, EventArgs e)
        {
            if (Condition1.IsChecked)
            {
                OnConditionalFormattingItemClick(sender, new SourcedEventArgs() { Source = Condition1 });
            }

            if (Condition2.IsChecked)
            {
                OnConditionalFormattingItemClick(sender, new SourcedEventArgs() { Source = Condition2 });
            }

            if (Condition3.IsChecked)
            {
                OnConditionalFormattingItemClick(sender, new SourcedEventArgs() { Source = Condition3 });
            }
        }

        private void CsvExport_Click(object sender, RoutedEventArgs e)
        {
            Export(GridFileFormat.Csv);
        }

        private void TextExport_Click(object sender, RoutedEventArgs e)
        {
            Export(GridFileFormat.Text);
        }

        private void HtmlExport_Click(object sender, RoutedEventArgs e)
        {
            Export(GridFileFormat.Html);
        }
        private void Export(GridFileFormat type)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Comma Separated Values(*.csv)|*.csv|" + "HTML File (*.htm;*.html)|*.htm;*.html|" + "Text File (*.txt)|*.txt";
            switch (type)
            {
                case GridFileFormat.Csv:
                    dlg.FilterIndex = 1; break;
                case GridFileFormat.Text:
                    dlg.FilterIndex = 3; break;
                case GridFileFormat.Html:
                    dlg.FilterIndex = 2; break;
            }

            if (dlg.ShowDialog() == true)
            {
                flexGrid.Save(dlg.FileName, type, GridSaveOptions.SaveHeaders | GridSaveOptions.Formatted);
                Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
            }
        }

        private void OnColumnsItemClick(object sender, SourcedEventArgs e)
        {
            string columnName = (string)(e.Source as C1MenuItem).Header;
            flexGrid.Columns[columnName].IsVisible = !flexGrid.Columns[columnName].IsVisible;
        }

        private void OnConditionalFormattingItemClick(object sender, SourcedEventArgs e)
        {
            C1MenuItem item = e.Source as C1MenuItem;
            Point condition = (Point)(item).CommandParameter;
            var minDiscount = condition.X / 100;
            var maxDiscount = condition.Y / 100;
            foreach (var row in flexGrid.Rows)
            {
                if (row.DataItem is Product product)
                {
                    var discount = product.Discount;
                    bool satisfied = minDiscount > 0 && maxDiscount < 1
                        ? discount >= minDiscount && discount <= maxDiscount
                        : (minDiscount > 0 ? discount > minDiscount : discount < maxDiscount);
                    if (satisfied)
                    {
                        row.Background = item.IsChecked? new SolidColorBrush(Colors.LightSkyBlue) : null;
                    }
                }
            }
        }

        private async void ExcelExport_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Excel files(*.xlsx)|*.xlsx";
            if (dlg.ShowDialog() ?? false)
            {
                try
                {
                    await flexGrid.SaveAsync(dlg.FileName, "Sheet1", GrapeCity.Documents.Excel.SaveFileFormat.Xlsx);
                    Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
        }

        private void OnRatingTapped(object sender, MouseButtonEventArgs e)
        {
            var hitTest = flexGrid.HitTest(e);
            flexGrid.StartEditing(hitTest.CellRange.Row, hitTest.CellRange.Column, true);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
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
    /// <summary>
    /// 
    /// </summary>
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
}
