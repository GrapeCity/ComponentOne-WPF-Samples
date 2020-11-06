using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using C1.WPF.DataGrid;
using System.Reflection;

namespace DataGridSamples
{
    public static class Common
    {

        // *******************************************************************
        // common code for Column Auto-generation
        // i.e. creation of combo columns (we show the same data in all the grids)
        // *******************************************************************
        public static void HandleColumnAutoGeneration(DataGridAutoGeneratingColumnEventArgs e)
        {
            if (new string[] { "Color", "ID", "ProductSubcategoryID", "Description", "GifImage" }.Contains(e.Property.Name))
            {
                e.Cancel = true;
                return;
            }
            if (e.Property.Name == "ProductModelID")
            {
                var comboCol = new DataGridComboBoxColumn(e.Property);
                var models = Data.GetModels().OrderBy(m => m.Name).ToList();
                models.Insert(0, new Model { Name = "Not specified", ProductModelID = 0 });
                comboCol.ItemsSource = models;
                comboCol.DisplayMemberPath = "Name";
                comboCol.SelectedValuePath = "ProductModelID";
                comboCol.Header = "Model";
                e.Column = comboCol;
            }
            if (e.Property.Name == "ImageUrl")
            {
                //CLR40
                string imgPath = "/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/";
                var imageColumn = new DataGridImageColumn(e.Property);
                imageColumn.SortMemberPath = "ImageUrl";
                imageColumn.Binding.Converter = new ImageSourceConverter();
                imageColumn.Binding.ConverterParameter = imgPath;
                imageColumn.Width = new DataGridLength(85);
                imageColumn.GroupContentConverter = new ImageSourceConverter();
                imageColumn.Format = imgPath;
                imageColumn.Header = "Image";
                e.Column = imageColumn;
            }
            if (e.Property.Name == "ExpirationDate")
            {
                //(e.Column as DataGridDateColumn).SelectedDateFormat = C1DatePickerFormat.Custom;
                e.Column.GroupConverter = new OutlookDateGroupConverter();
                e.Column.GroupContentConverter = new OutlookDateGroupNameConverter();
                e.Column.Header = "Expiration Date";
            }
            if (e.Property.Name == "Name")
            {
                IValueConverter converter = new AlphabeticTextGroupConverter();
                e.Column.GroupConverter = converter;
            }
            if (e.Property.Name == "ProductNumber")
            {
                IValueConverter converter = new ProductGroupConverter();
                e.Column.GroupConverter = converter;
                e.Column.GroupContentConverter = converter;
                e.Column.Header = "Product Number";
                (e.Column as DataGridTextColumn).MaxLength = 10;
            }
            if (e.Property.Name == "StandardCost" && e.Column is C1.WPF.DataGrid.DataGridBoundColumn)
            {
                e.Column.GroupConverter = new NumberRangeGroupConverter();
                e.Column.GroupContentConverter = new NumberRangeGroupNameConverter();
                e.Column.Header = "Standard Cost";
                ((C1.WPF.DataGrid.DataGridBoundColumn)e.Column).Format = "C";
            }
        }
    }
}
