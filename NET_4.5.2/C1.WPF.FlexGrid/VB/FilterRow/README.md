## FilterRow
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/VB/FilterRow)
____
#### Shows how you can add a FilterRow to the C1FlexGrid.
____
The C1FlexGrid does not have built-in filtering capabilities. It relies on the 
data source to provide this functionality.

This sample demonstrates how you can use a custom CellFactory to implement a filter
row.

The filter is located below the column headers and above the data. It contains a
'FilterEditor' control above each column, where the user may type filter conditions.

When the filter controls lose focus, or when the user presses the Return key,
the FilterEditor updates the filter by retrieving the ICollectionView being used 
as the grid's data source and applying a custom method (predicate) to the 
ICollectionView's Filter property.

The filter predicate is called for each item in the data source, and uses reflection
to compare the values of the item's properties against the filter criteria entered 
by the user. If the item satisfies all the criteria, then the filter predicate returns 
true; otherwise it returns false and the item is filtered out of the view.

The FilterCellRowFactory implemented in the sample as a Boolean 'UseRegEx' property
that determines whether the filter should use a simple 'contains' test or a more
powerful regular expression test.

If UseRegEx is set to true, then any percentage signs in the filter argument are
used to represent 'anything'. In this case, the user can type for example:

	"a%"	to filter strings that START WITH 'a'
	"%a"	to filter strings that END WITH 'a'
	"%a%'	to filter strings that CONTAIN 'a'

Note: the sample also shows how to apply filtering in unbound mode. To see how this
works, change the value of the UNBOUND constant in the main page to true.
