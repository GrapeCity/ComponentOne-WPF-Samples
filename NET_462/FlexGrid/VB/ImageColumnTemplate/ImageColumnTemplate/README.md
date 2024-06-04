## ImageColumnTemplate
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_462/FlexGrid/VB/ImageColumnTemplate/ImageColumnTemplate)
____
#### Shows how to display images in a bound grid column.
____
The sample uses the Column.CellTemplate property to bind a column to a DataTemplate that 
contains an Image element. The image element is bound to a numeric property, and has a
ValueConverter that automatically converts the numeric value into an image.

The relevant XAML looks like this:

```
  <c1:Column Header="Image" Width="*" >
    <c1:Column.CellTemplate>
      <DataTemplate>
        <Image 
          Margin="4" 
          Source="{Binding Path=Alert, Converter={StaticResource AlertImageConverter}}"/>
      </DataTemplate>
    </c1:Column.CellTemplate>
  </c1:Column>
```
The Converter property of the binding is set to a static resource named AlertImageConverter. This
is a public class defined in the main application (in C#) which is added to the application resources
with this XAML snippet:

```
  <Window.Resources>
    <local:AlertImageConverter x:Key="AlertImageConverter" />
  </Window.Resources>
```
Finally, the AlertImageConverter class is implemented as follows:

```
  public class AlertImageConverter : IValueConverter
  {
    // load static images to show on the grid from application resources
    static BitmapImage _bmpRed, _bmpYellow, _bmpGreen;
    static AlertImageConverter()
    {
      _bmpRed = new BitmapImage(new Uri("/Resources/redBell.png", UriKind.Relative));
      _bmpYellow = new BitmapImage(new Uri("/Resources/yellowBell.png", UriKind.Relative));
      _bmpGreen = new BitmapImage(new Uri("/Resources/greenBell.png", UriKind.Relative));
    }

	// convert 'Alert' int value into corresponding image
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      switch ((int)value)
      {
        case 1: return _bmpRed;
        case 2: return _bmpYellow;
      }
      return _bmpGreen;
    }

	// one-way conversion only (ConvertBack is not used)
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
```