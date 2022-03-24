## DynamicConditionalFormatting
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/DynamicConditionalFormatting)
____
#### Shows how to apply conditional formatting to a grid based on dynamic value ranges.
____
This sample allows users to select desired ranges for price and weight values, then
uses these ranges to apply conditional formatting to grid cells. Cells with values 
below the minimum are shown in red; values within range are shown in gray; and values
above the maximum are shown in green.

The sample demonstrates several useful techniques.

First, it uses range sliders with attached histograms. The histograms allow users
to see how values are distributed over the range, so it is easy to determine whether
the selected range contains a significant portion of the data or only a few points.
A similar type of slider is used by Google in their excellent financial site:
	
	http://www.google.com/finance
	
The VisualRangeSlider control was built using a C1RangeSlider with a superimposed
Polygon element. The control has a Values property that contains the complete list
of values in the population. This list is used to build the histogram and to 
automatically set the control's Minimum and Maximum properties.

The sample declares two VisualRangeSliders as page resources using this XAML:

```
     <local:VisualRangeSlider
            x:Name="_rngPrice" Grid.Row="1" Grid.Column="1" Margin="12" MinWidth="100" MaxWidth="400" Height="40" HorizontalAlignment="Left"
            Fill="#80008000"
            LowerValue="50"
            UpperValue="100"
			
	<local:VisualRangeSlider
            x:Name="_rngWeight" Grid.Row="2" Grid.Column="1" Margin="12" MinWidth="100" MaxWidth="400" Height="40" HorizontalAlignment="Left"
            Fill="#80f00000"
            Minimum="0"
            Maximum="500"
            LowerValue="10"
            UpperValue="50" />

```
At run time, values are assigned to the HistogramValues property of the range 
sliders using a simple LINQ statement:

```
			
			//Creates Products
			var list = Product.GetProducts(1200);
			// bind to range sliders
            _rngPrice.HistogramValues = from p in list select p.Price;
            _rngPrice.ValueChanged += _rngPrice_ValueChanged;

            _rngWeight.HistogramValues = from p in list select p.Weight;
            _rngWeight.ValueChanged += _rngWeight_ValueChanged;

            // bind to grids
            _flex.ItemsSource = list;
```
Next, the sample implements a DynamicRangeConverter class that converts numeric
values (such as price and weight) into brushes that can be bound to the Foreground
property of TextBlock elements. The DynamicRangeConverter has properties that 
define the lower and upper range values, as well as brushes to be used to render
values below, within, or above the range.

The sample uses two DynamicRangeConverter objects which are bound to the range
sliders as follows:

```
   <Window.Resources>
        <!-- binding converter: change prices into brushes -->
        <local:DynamicRangeConverter 
            x:Key="DRCPrice" 
            LowerValue="{Binding LowerValue, ElementName=_rngPrice}"
            UpperValue="{Binding UpperValue, ElementName=_rngPrice}"
            LowValueBrush="Red"
            HighValueBrush="Green"
            NormalValueBrush="LightGray" />
        <!-- binding converter: change weights into brushes -->
        <local:DynamicRangeConverter 
            x:Key="DRCWeight" 
            LowerValue="{Binding LowerValue, ElementName=_rngWeight}"
            UpperValue="{Binding UpperValue, ElementName=_rngWeight}"
            LowValueBrush="Red"
            HighValueBrush="Green" 
            NormalValueBrush="LightGray" />
    </Window.Resources>
```
Finally, the converters are used in the FlexGrid bindings to provide conditional
formatting. When the user modifies the range in the slider, the changes are 
applied to the converters and passed on to the grid:

```
   <c1:C1FlexGrid Name="_flex" Grid.Row="4" Grid.ColumnSpan="2" AutoGenerateColumns="False" >
            <c1:C1FlexGrid.Columns>
                <c1:Column Binding="{Binding Line}" />
                <c1:Column Binding="{Binding Color}" />
                <c1:Column Binding="{Binding Name}" />

                <c1:Column Binding="{Binding Price}" >
                    <c1:Column.CellTemplate>
                        <DataTemplate>
                            <TextBlock 
                                Text="{Binding Price, StringFormat=n0}"
                                Foreground="{Binding Price, Converter={StaticResource DRCPrice}}" 
                                HorizontalAlignment="Right" 
                                VerticalAlignment="Center" />
                        </DataTemplate>
                    </c1:Column.CellTemplate>
                </c1:Column>

                <c1:Column Binding="{Binding Weight}" >
                    <c1:Column.CellTemplate>
                        <DataTemplate>
                            <TextBlock 
                                Text="{Binding Weight, StringFormat=n0}"
                                Foreground="{Binding Weight, Converter={StaticResource DRCWeight}}" 
                                HorizontalAlignment="Right"
                                VerticalAlignment="Center" />
                        </DataTemplate>
                    </c1:Column.CellTemplate>
                </c1:Column>

                <c1:Column Binding="{Binding Cost}" HorizontalAlignment="Right" />
                <c1:Column Binding="{Binding Discontinued}" HorizontalAlignment="Center" />
                <c1:Column Binding="{Binding Rating}" HorizontalAlignment="Center" />
            </c1:C1FlexGrid.Columns>
        </c1:C1FlexGrid>
```
This should be all that is required, but one additional piece of code is necessary to make
this work:

```
    // this shouldn't be necessary, seems like a WPF limitation
    // (bindings with converters don't seem to update correctly in column templates)
    void _rngPrice_ValueChanged(object sender, EventArgs e)
        {
            var c = _flex.Columns["Price"].Index;
            _flex.Invalidate(new C1.WPF.FlexGrid.CellRange(0, c, int.MaxValue, c));
            DynamicRangeConverter DRCPrice = this.Resources["DRCPrice"] as DynamicRangeConverter;
            DRCPrice.UpperValue = _rngPrice.UpperValue;
            DRCPrice.LowerValue = _rngPrice.LowerValue;
        }
        void _rngWeight_ValueChanged(object sender, EventArgs e)
        {
            var c = _flex.Columns["Weight"].Index;
            _flex.Invalidate(new C1.WPF.FlexGrid.CellRange(0, c, int.MaxValue, c));

            DynamicRangeConverter DRCWeight = this.Resources["DRCWeight"] as DynamicRangeConverter;
            DRCWeight.UpperValue = _rngWeight.UpperValue;
            DRCWeight.LowerValue = _rngWeight.LowerValue;
        }
```
This code detects changes in the sliders and forces the grid to refresh the corresponding
column. This seems to be a limitation in WPF. Changes to the converter parameters
are detected correctly for top level controls, but they are not transmitted to elements 
created from templates (the same behavior occurs if you replace the FlexGrid with a regular
DataGrid control).

The bright side of this limitation is that it illustrates the use of the FlexGrid's 
Invalidate method, which forces the grid to refresh the cells in a given range. In this
case, Invalidate is used to refresh the price column when the price range is changed by
the user.





