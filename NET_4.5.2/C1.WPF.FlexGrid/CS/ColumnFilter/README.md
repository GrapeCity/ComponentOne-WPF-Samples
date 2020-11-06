## ColumnFilter
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/CS/ColumnFilter)
____
#### Demonstrates the use of the C1FlexGridFilter extension class to implement column filters in a C1FlexGrid control.
____
The C1FlexGridFilter extension class extends the C1FlexGrid to provide Excel-style column
filtering. Once column filtering is enabled, moving the mouse over the column headers shows
a drop-down button that can be used to edit the filter applied to the column. The filter
supports two modes:


* Value Mode: The editor displays a list containing all the values present in the column. Users may
  use the list to select one or more values to be displayed.


* Condition Mode: The editor displays two conditions that consist of an operator and a comparison
  parameter (e.g. 'greater than' and '2.5'). The conditions are combined with an 'and' or an 'or'
  operator.

Filters may be set by the user using the editor provided, or they can be set in code. For example, 
the code below shows all 'blue' products that cost more than $900:

```
	// get grid filter
	var f = C1FlexGridFilterService.GetFlexGridFilter(_flex);

	// customize color filter (Value mode)
	var c = _flex.Columns["Color"];
	var cf = f.GetColumnFilter(c);
	cf.ConditionFilter.Clear();
	cf.ValueFilter.Values = new string[] { "Blue" };

	// customize price filter (Condition mode)
	c = _flex.Columns["Price"];
	cf = f.GetColumnFilter(c);
	cf.ValueFilter.Clear();
	var c1 = cf.ConditionFilter.Condition1;
	c1.Operator = ConditionOperator.IsGreaterThan;
	c1.Parameter = 900;

	// apply changes
	f.Apply();
</code> 

By default, filters are applied to the grid by setting the Visible property on each row. Rows that
pass the filter become visible, and rows that are rejected become invisible. When using this mode,
the total row count does not change when the filter is applied. You can count how many rows passed
the filter using a simple LINQ expression such as this one:

```
	int visibleRows = _flex.Rows.Count(r => r.Visible);
```
If the grid is bound to a data source that supports filtering, you may set the filter's 
UseCollectionView property to true. In this case, the filter will be applied directly to the data 
source (ICollectionView.Filter). When using this mode, changes to the filter do affect the grid's
row count (as well as the item count on any other controls bound to the same data source).

NOTE: To use the C1FlexGridFilter, your project must include a reference to the 
C1.WPF.FlexGridFilter.4.dll assembly.

