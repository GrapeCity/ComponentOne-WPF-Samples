## FilterEditor
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.DataFilter/CS/FilterEditor)
____
#### This sample shows the basic features of C1FilterEditor.
____
This sample demonstrates basic functionality of the C1FilterEditor control. 
The FilterEditor control represents a filter in the form of a tree. Tree nodes can be logical conditions "And" and "Or" or a filter for a data source property.
The C1FilterEditor.SetExpression method is used to load predefined filter.
You can use the GetExpression method to get the current filter expression, which you can use for xml serialization.
In this sample FlexGrid.DataSource property and FilterEditor.DataSource property are both set to the same data collection. 
That allows to filter FlexGrid content based on multiple conditions selected in the C1FilterEditor.
