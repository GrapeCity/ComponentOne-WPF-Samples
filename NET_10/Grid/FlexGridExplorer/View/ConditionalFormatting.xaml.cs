using C1.WPF.Grid;
using C1.WPF.RulesManager;
using FlexGridExplorer.Resources;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public partial class ConditionalFormatting : UserControl
    {
        public ConditionalFormatting()
        {
            InitializeComponent();
            Tag = AppResources.ConditionalFormattingDescription;
            grid.Columns[4].DataMap = new GridDataMap() { ItemsSource = Customer.GetCountries(), DisplayMemberPath = "Value", SelectedValuePath = "Key" };
            //grid.CellFactory = new MyCellFactory();
            grid.MinColumnWidth = 85;

            LoadData();
        }

        private async void LoadData()
        {
            var data = await Task.Run(() => Customer.GetCustomerList(100));
            grid.ItemsSource = data;
        }

        public RulesEngineStyle GetFirstNameStyle(RulesEngineSource source, int index, string field)
        {
            var firstNames = source.GetFieldValues<string>(field).ToList();
            double maxLength = firstNames.Max(firstName => firstName?.Length ?? 0);
            var firstName = source.GetValue<string>(index, field) ?? "";
            return new RulesEngineStyle { DataBarValue = firstName.Length / maxLength };
        }

        private void OnSaveRules(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog()
                {
                    DefaultExt = ".xml",
                    FileName = "rules",
                    Filter = "Xml Files (*.xml)|*.xml",
                };
                if (saveDialog.ShowDialog() ?? false)
                {
                    using (var file = File.Create(saveDialog.FileName))
                    {
                        rulesManager.Save(file);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnLoadRules(object sender, RoutedEventArgs e)
        {
            try
            {

                var openDialog = new OpenFileDialog()
                {
                    DefaultExt = ".xml",
                    FileName = "rules",
                    Filter = "Xml Files (*.xml)|*.xml",
                };
                if (openDialog.ShowDialog() ?? false)
                {
                    using (var file = openDialog.OpenFile())
                    {
                        rulesManager.Load(file);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    //public class MyCellFactory : GridCellFactory
    //{
    //    static SolidColorBrush RedBackground = new SolidColorBrush(Color.FromArgb(0xFF, 0xC4, 0x2B, 0x1C));
    //    static SolidColorBrush RedForeground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFD, 0xE7, 0xE9));
    //    static SolidColorBrush GreenBackground = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x7B, 0x0F));
    //    static SolidColorBrush GreenForeground = new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0xF6, 0xDD));

    //    public override void PrepareCell(GridCellType cellType, GridCellRange range, GridCellView cell, Thickness internalBorders)
    //    {
    //        base.PrepareCell(cellType, range, cell, internalBorders);
    //        var orderCountColumn = Grid.Columns["OrderCount"];
    //        if (cellType == GridCellType.Cell && range.Column == orderCountColumn.Index)
    //        {
    //            var cellValue = Grid[range.Row, range.Column] as int?;
    //            if (cellValue.HasValue)
    //            {
    //                cell.Background = cellValue < 50.0 ? RedForeground : GreenForeground;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Binds the content of the cell.
    //    /// </summary>
    //    /// <param name="cellType">Type of the cell.</param>
    //    /// <param name="range">The range.</param>
    //    /// <param name="cellContent">Content of the cell.</param>
    //    public override void BindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
    //    {
    //        base.BindCellContent(cellType, range, cellContent);
    //        var orderTotalColumn = Grid.Columns["OrderTotal"];
    //        if (cellType == GridCellType.Cell && range.Column == orderTotalColumn.Index)
    //        {
    //            var label = cellContent as TextBlock;
    //            if (label != null)
    //            {
    //                var cellValue = Grid[range.Row, range.Column] as double?;
    //                if (cellValue.HasValue)
    //                {
    //                    label.Foreground = cellValue < 5000.0 ? RedBackground : GreenBackground;
    //                }
    //            }
    //        }
    //    }

    //    public override void UnbindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
    //    {
    //        base.UnbindCellContent(cellType, range, cellContent);
    //        var label = cellContent as TextBlock;
    //        if (label != null)
    //        {
    //            label.ClearValue(Label.ForegroundProperty);
    //        }
    //    }
    //}
}
