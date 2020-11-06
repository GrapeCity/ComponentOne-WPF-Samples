## ExtendLastColumn
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/\NET_4.5.2\C1.WPF.FlexGrid\CS\ExtendLastColumn\ExtendLastColumn)
____
#### Shows how to implement ExtendLastCol functionality in the C1FlexGrid for SL/WPF.
____
The C1FlexGrid for WinForms has an ExtendLastCol property that causes
the grid to extend the width of the last column to fill the control, 
eliminating any extra empty space left over after the last column.

The C1FlexGrid for Silverlight and WPF does not have an ExtendLastCol property.
Instead, they implement column star-sizing, which allows the developer to
control which columns should extend and by how much. This is a much more
powerful and flexible mechanism.

However, note that simply setting the width of the last grid column to a
star length is not enough to achieve the functionality of the ExtendLastCol 
property. You must also set the column's MinWidth property to ensure that
the column will not shrink to zero width (ExtendLastCol used the column's
Width property as the minimum).

Summarizing, the code below is all you need to mimic the ExtendLastCol 
functionality in the C1FlexGrid control for Silverlight or WPF:

```
	// C#
	_flex1.Columns[2].MinWidth = 180;
    _flex1.Columns[2].Width = new GridLength(1, GridUnitType.Star);
```
```
	<!-- XAML -->
	<c1:C1FlexGrid.Columns>
		<c1:Column Binding="{Binding FirstName}" />
		<c1:Column Binding="{Binding LastName}" />
		<c1:Column Binding="{Binding Address}" Width="*" MinWidth="180" />
	</c1:C1FlexGrid.Columns>
```