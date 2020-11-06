## CollectionViewFilter
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/CS/CollectionViewFilter/CollectionViewFilter)
____
#### Shows an easy way to create ICollectionView filters based on string expressions.
____
The ICollectionView interface has a 'Filter' member that allows you to specify a predicate
to be used for filtering the collection members.

This is a flexible approach since the predicate is simply a method that takes an item and 
returns true if the item should be included in the view.

In many cases however you may want to serialize the filter so it can be saved and re-applied
later. This was easy to do with the System.Data classes, where the DataView class has a 
RowFilter property that takes a string as a parameter. For example:

```
	dv.RowFilter = "Name LIKE 'a*'"; // Name starts with 'A'
	dv.RowFilter = "Name LIKE '*a*'"; // Name contains 'A'
	dv.RowFilter = "NOT (Name LIKE '*a*')"; // Name does not contain 'A'
	dv.RowFilter = "Name LIKE 'a*' OR Name Like 'b*'"; // Name starts with 'A' or 'B'
```
This sample shows how you can leverage the expression parser in the System.Data classes to
use string expressions as filters in ICollectionView classes.

The ViewFilter class works by creating a DataTable columns for each property of the objects
in the source collection view, plus one additional calculated column with the filter 
expression. To apply the filter, the class populates the row with data from the item, then
returns the value of the calculated expression.

Here's how you could use the ViewFilter class:

```
	// create a regular ICollectionView
	var view = new ListCollectionView(Customer.GetCustomerList(200));

	// create the ViewFilter class to control the view filter
    var filter = new ViewFilter(view);

	// apply filter expression at any time
    filter.FilterExpression = "State = 'PENDING' OR State = 'FAILURE'";

	// bind view to controls
	_grid.ItemsSource = view;
```
Note that this example works with any control, not just the C1FlexGrid.
