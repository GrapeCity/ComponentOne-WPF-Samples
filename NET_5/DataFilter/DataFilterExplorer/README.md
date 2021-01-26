## DataFilterExplorer
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_5/DataFilter/DataFilterExplorer)
____
#### Shows samples of DataFilter/FilterEditor controls
____
Shows the following samples

* CarsListControl: This sample demonstrates how to use C1DataFilter control to filter the C1FlexGrid control. Also demonstrates how to modify automatically generated filters, save and restore filter expressions.


* FilterEditorControl: This sample shows the basic features of C1FilterEditor.
	This sample demonstrates basic functionality of the C1FilterEditor control. 
	The FilterEditor control represents a filter in the form of a tree. Tree nodes can be logical conditions "And" and "Or" or a filter for a data source property.
	The C1FilterEditor.SetExpression method is used to load predefined filter.
	You can use the GetExpression method to get the current filter expression, which you can use for xml serialization.
	In this sample FlexGrid.DataSource property and FilterEditor.DataSource property are both set to the same data collection. 
	That allows to filter FlexGrid content based on multiple conditions selected in the C1FilterEditor.


* FilterSummaryControl: This sample demonstrates how to use the FilterSummary for the Checklist filter. Also shows how to use different aggregate expressions and custom format of filter summaries.


* CustomFiltersSample: Demonstrates using C1DataFilter control to show custom filters.
	There is C1TreeView and C1DataFilter on the window.
	The C1TreeView uses CustomContentPresenter to shows data.
	The C1DataFilter uses three custom filters:
	+ ColorFilter based on CustomFilter, allows to choose the color of the car;
	+ MapFilter based on CustomFilter, allows to choose the store on map;
	+ ModelFilter based on CustomFilter, allows to choose the model of the car;
	+ PriceFilter based on ChecklistFilter, allows to choose the price range of cars.
