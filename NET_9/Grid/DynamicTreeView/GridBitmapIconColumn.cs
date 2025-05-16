using C1.WPF.Core;
using C1.WPF.Grid;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DynamicTreeView
{
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
            TrySetBitmapSource(bitmapIcon, cellType, row);
            textBlock.Text = Grid.GetCellText(cellType, row, this);
        }

        private async void TrySetBitmapSource(C1BitmapIcon bitmapIcon, GridCellType cellType, GridRow row)
        {
            try
            {
                CacheBitmapSourceGetterFunction();
            }
            catch { }
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
                // In case the backspace button is pressed
                // the cell value will be null, and the DefaultBitmapSource 
                // is a non-transparent renderable image, so image source is set to null to make
                // cell visually appear empty
                if (Grid.GetCellValue(cellType, row, this) == null)
                {
                    bitmapIcon.Source = null;
                }
                else
                {
                    bitmapIcon.Source = DefaultBitmapSource;
                }
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
                catch (Exception ex) { }
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
}
