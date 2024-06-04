## ConditionalFormatting
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_462/FlexGrid/CS/ConditionalFormatting)
____
#### Shows how you can implement conditional formatting using a C1FlexGrid.
____
The sample shows a grid where cells above or below certain values are displayed
in boldface green or red.

There are two ways to accomplish conditional formatting with the C1FlexGrid:

1) Code-based: This approach is based on using a custom CellFactory class to 
format the cells as they are generated. This sample does not use this approach.

2) XML-based: This approach is based on creating custom cells by specifying a
CellTemplate for each conditionally formatted column. The CellTemplate contains
a TextBlock element with several bound properties as shown below:

```
    <c1:Column Binding="{Binding Price}" >
        <c1:Column.CellTemplate>
            <DataTemplate>
                <TextBlock 
                    Text="{Binding Price, StringFormat=n0}" HorizontalAlignment="Right" 
                    Foreground="{Binding Price, Converter={StaticResource ForegroundConverter}, ConverterParameter={StaticResource PriceRange} }" 
                    FontWeight="{Binding Price, Converter={StaticResource FontWeightConverter}, ConverterParameter={StaticResource PriceRange} }" />
            </DataTemplate>
        </c1:Column.CellTemplate>
    </c1:Column>
```
This piece of XAML creates a grid column with a custom cell template. The template 
contains a TextBlock element and binds the "Text", "Foreground", and "FontWeight" properties
to the value of the "Price" field in the data source.

The "Text" property is bound directly to the data, which is of type double and can be converted
automatically into strings.

The "Foreground" and "FontWeight" properties cannot be bound directly to the data, because double
values cannot be automatically converted to brushes or FontWeight values. To enable these bindings,
the XAML above specifies converters (ForegroundConverter and FontWeightConverter). The converters
are responsible for converting double values into brushes and font weights.

The ForegroundConverter and FontWeightConverter converters support a parameter that specifies a 
range. The converter uses this range to select the proper brush or font weight. Both the converters
and their parameters are specified as page resources in XAML:

```
     <Window.Resources>
        <!--Converteer implmented in ForeGroundConverter.cs-->
        <local:ForegroundConverter x:Key="ForegroundConverter"></local:ForegroundConverter>
        <!--Converter implemented in FontWeightConverter.cs-->
        <local:FontWeightConverter x:Key="FontWeightConverter"></local:FontWeightConverter>
        <!-- converter ranges (implemented as Point values: 
             X is the low value and Y is the high value) -->
        <Point x:Key="PriceRange" X="500" Y="1500" />
        <Point x:Key="WeightRange" X="200" Y="1000" />
        <Point x:Key="CostRange" X="200" Y="1000" />
        <Point x:Key="VolumeRange" X="2000" Y="6000" />
    </Window.Resources>
```
And, finally, this is how the converters are implemented:

```
    public class ForegroundConverter : System.Windows.Data.IValueConverter
    {
        static SolidColorBrush _brBlack = new SolidColorBrush(Colors.Black);
        static SolidColorBrush _brRed = new SolidColorBrush(Colors.Red);
        static SolidColorBrush _brGreen = new SolidColorBrush(Colors.Green);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var range = (Point)parameter;
            double val = (double)value;
            return val < range.X ? _brRed : val > range.Y ? _brGreen : _brBlack;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class FontWeightConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var range = (Point)parameter;
            double val = (double)value;
            return val < range.X || val > range.Y ? FontWeights.Bold : FontWeights.Normal;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
+        }
    }
</code> 


