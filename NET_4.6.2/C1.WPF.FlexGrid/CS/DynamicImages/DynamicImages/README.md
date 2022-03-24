## DynamicImages
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/DynamicImages/DynamicImages)
____
#### Shows how you can use a custom cell factory to implement cells with dynamic images.
____
The sample uses a custom cell factory that creates cells with images bound to a
property on the data source. When the value of the property changes, the image
changes to reflect the new value.

This is done with a value converter that takes an 'ItemType' value and creates
the appropriate ImageSource for the cell.

There are two key elements in the sample:

1) The code that creates the image cells:

```
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
```
2) The ImageConverter that creates the image source based on the ItemType:

```
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
```