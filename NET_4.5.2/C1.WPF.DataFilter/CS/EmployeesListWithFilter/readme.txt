EmployeesListWithFilter
-----------------------------
Demonstrates using C1DataFilter control to generate filters using data annotation attributes.

This sample demonstrates basic functionality of the C1DataFilter control. 
The DataFilter control supports different kinds of filters to filter different types of data.
By default, filters are automatically generated depending on the type of the fields present in the ItemsSource.
When end-user changes filter conditions, DataFilter applies conditions to the underlying data source.

In this sample FlexGrid.ItemsSource property and DataFilter.ItemsSource property are both set to the same data collection. 
That allows to filter FlexGrid content based on multiple conditions selected in the C1DataFilter. 
