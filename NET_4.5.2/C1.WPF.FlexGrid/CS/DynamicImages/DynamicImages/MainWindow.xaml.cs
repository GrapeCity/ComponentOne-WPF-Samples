using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

namespace DynamicImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // create some data
            List<Item> list = new List<Item>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Item() { Name = string.Format("Item {0}", i), Rating = i, ItemType = (ItemType)(i % 3) });
            }

            // bind grid to data
            _flex.ItemsSource = list;

            // attach custom cell factory
            _flex.CellFactory = new MyCellFactory();
        }
    }

    public class MyCellFactory : C1.WPF.FlexGrid.CellFactory
    {
        public override void CreateCellContent(C1.WPF.FlexGrid.C1FlexGrid grid, Border bdr, C1.WPF.FlexGrid.CellRange range)
        {
            var col = grid.Columns[range.Column];
            if (col.ColumnName == "ItemImage")
            {
                // create binding
                var b = new Binding();
                b.Path = new PropertyPath("ItemType");
                b.Converter = new MyImageConverter();

                // create image, bind source property using new binding
                var img = new Image();
                img.SetBinding(Image.SourceProperty, b);
                
                // assign to cell
                bdr.Child = img;
            }
            else
            {
                base.CreateCellContent(grid, bdr, range);
            }
        }
        public class MyImageConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                string path = null;

                switch (value.ToString())
                {
                    case "Album":
                    case "Artist":
                    case "Song":
                        path = value.ToString();
                        break;

                }
                if (path != null)
                {
                    var uri = string.Format("pack://application:,,,/Images/{0}.png", path);
                    return new BitmapImage(new Uri(uri));
                }

                return null;
            }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }

    public enum ItemType
    {
        Album,
        Artist,
        Song
    }
    public class Item
    {
        [Display(Name = "ItemType")]
        public ItemType ItemType { get; set; }

        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
