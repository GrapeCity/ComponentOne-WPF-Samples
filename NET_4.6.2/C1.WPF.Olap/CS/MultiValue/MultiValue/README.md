## MultiValue
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.Olap/CS/MultiValue/MultiValue)
____
#### Shows how you can use C1Olap to analyze multiple fields in one view.
____
By default, C1Olap only allows one value field per view. When the
user adds a field to the Values list, the new field replaces any 
other fields that might be there.

Starting with build 70, the C1OlapFieldList class has a new MaxItems
property. This property allows you to determine how many fields
are allowed in each field list (Rows, Columns, Filters, and Values).

If you set the MaxItems of the Values list to a number higher than
one, users will be able to add multiple fields to the values list,
and the analysis will be performed on all of them at once.

For example:

```
	// prepare to define Olap view
    var olap = this.c1OlapPage1.OlapEngine;
	olap.BeginUpdate();

	// group data by product and country
    olap.RowFields.Add("ProductName");
    olap.ColumnFields.Add("Country");

	// show total extended price and freight values
    olap.ValueFields.MaxItems = 5;
    olap.ValueFields.Add("ExtendedPrice", "Freight");

	// ready
	olap.EndUpdate();
```
You can also use the MaxItems property on the Rows, Columns, and Filters lists
if you want to limit the number of fields users can add to those lists. It rarely
makes sense to have more than three or four Row or Column fields for example.
