using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using C1.WPF.Grid;

using System.Windows.Media;
using C1.WPF.Core;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows.Shapes;
using C1.WPF.Menu;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

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
                flexGrid.Save(dlg.FileName, type, GridSaveOptions.Formatted);
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
            Point condition = (Point)(e.Source as C1MenuItem).CommandParameter;
            foreach (var row in flexGrid.Rows)
            {
                if (row.DataItem is Product product)
                {
                    var discount = product.Discount;
                    if (discount > condition.X / 100 && discount < condition.Y / 100)
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

    public class GridBitmapIconColumn : GridColumn
    {
        private HttpClient _httpClient = new HttpClient();

        public ImageSource DefaultBitmapSource { get; set; }
        public string BitmapSourceBinding { get; set; }
        public double IconWidth { get; set; }
        public bool ShowAsMonochrome { get; set; }

        protected override object GetCellContentType(GridCellType cellType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                return base.GetCellContentType(cellType, row);
            }
            return typeof(GridBitmapIconColumn);
        }

        protected override FrameworkElement CreateCellContent(GridCellType cellType, object cellContentType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                return base.CreateCellContent(cellType, cellContentType, row);
            }
            var grid = new Grid();
            grid.Margin = Grid.CellPadding;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            var bitmapImage = new C1BitmapIcon() { Width = IconWidth, ShowAsMonochrome = ShowAsMonochrome, Margin = new Thickness(0, 0, 5, 0), VerticalAlignment = VerticalAlignment.Center };
            var textBlock = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
            System.Windows.Controls.Grid.SetColumn(textBlock, 1);
            grid.Children.Add(bitmapImage);
            grid.Children.Add(textBlock);
            return grid;
        }

        protected override void BindCellContent(FrameworkElement cellContent, GridCellType cellType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                base.BindCellContent(cellContent, cellType, row);
                return;
            }
            var grid = cellContent as Grid;
            var bitmapIcon = grid.Children[0] as C1BitmapIcon;
            var textBlock = grid.Children[1] as TextBlock;
            TrySetBitmapSource(bitmapIcon, row);
            textBlock.Text = Grid.GetCellText(cellType, row, this);
        }

        private async void TrySetBitmapSource(C1BitmapIcon bitmapIcon, GridRow row)
        {
            CacheBitmapSourceGetterFunction();
            if (_imageSourceGetter == null)
            {
                bitmapIcon.Source = DefaultBitmapSource;
                return;
            }

            var value = _imageSourceGetter.Invoke(row.DataItem);
            if (value is ImageSource imageSource)
            {
                bitmapIcon.Source = imageSource;
            }
            else
            {
                var tag = Guid.NewGuid();
                bitmapIcon.Source = DefaultBitmapSource;
                bitmapIcon.Tag = tag;

                try
                {
                    Uri imageSourceUrl = value as Uri;
                    if (value is string)
                        imageSourceUrl = new Uri(value as string);
                    var responseStream = await _httpClient.GetStreamAsync(imageSourceUrl);

                    if (!(bitmapIcon.Tag is Guid) || ((Guid)bitmapIcon.Tag) != tag)
                        return;

                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new MemoryStream())
                    {
                        await responseStream.CopyToAsync(memoryStream);

                        if (!(bitmapIcon.Tag is Guid) || ((Guid)bitmapIcon.Tag) != tag)
                            return;

                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                    }
                    bitmapIcon.Source = bitmapImage;
                }
                catch { }
            }
        }

        protected override void UnbindCellContent(FrameworkElement cellContent, GridCellType cellType, GridRow row)
        {

            var grid = cellContent as Grid;
            var bitmapIcon = grid?.Children?[0] as C1BitmapIcon;
            if (bitmapIcon != null)
            {
                bitmapIcon.Source = null;
                bitmapIcon.Tag = null;
            }
            else
            {
                base.UnbindCellContent(cellContent, cellType, row);
            }
        }

        private Func<object, object> _imageSourceGetter;

        private void CacheBitmapSourceGetterFunction()
        {
            if (_imageSourceGetter == null && !string.IsNullOrWhiteSpace(BitmapSourceBinding) && Grid?.DataCollection != null)
            {
                var itemType = Grid.DataCollection.GetItemType();
                _imageSourceGetter = CreateBindingFunction(itemType, BitmapSourceBinding);
            }
        }

    }

    public class GridColorColumn : GridColumn
    {
        public double ColorRectangleSize { get; set; } = 20;

        protected override object GetCellContentType(GridCellType cellType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                return base.GetCellContentType(cellType, row);
            }
            return typeof(GridColorColumn);
        }

        protected override FrameworkElement CreateCellContent(GridCellType cellType, object cellContentType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                return base.CreateCellContent(cellType, cellContentType, row);
            }
            var grid = new Grid();
            grid.Margin = Grid.CellPadding;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            var bitmapImage = new Rectangle() { Width = ColorRectangleSize, Height = ColorRectangleSize, Margin = new Thickness(0, 0, 5, 0), VerticalAlignment = VerticalAlignment.Center };
            var textBlock = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
            System.Windows.Controls.Grid.SetColumn(textBlock, 1);
            grid.Children.Add(bitmapImage);
            grid.Children.Add(textBlock);
            return grid;
        }

        protected override void BindCellContent(FrameworkElement cellContent, GridCellType cellType, GridRow row)
        {
            if (cellType != GridCellType.Cell)
            {
                base.BindCellContent(cellContent, cellType, row);
                return;
            }
            var grid = cellContent as Grid;
            var colorRectangle = grid.Children[0] as Rectangle;
            var textBlock = grid.Children[1] as TextBlock;
            var colorString = Grid.GetCellValue(cellType, row, this) as string;
            if (string.IsNullOrEmpty(colorString))
            {
                colorRectangle.Fill = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                colorRectangle.Fill = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString));
            }
            textBlock.Text = Grid.GetCellText(cellType, row, this);
        }
    }
}
