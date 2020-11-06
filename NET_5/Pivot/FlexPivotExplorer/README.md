## FlexPivotExplorer
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/\NET_5\Pivot\FlexPivotExplorer)
____
#### Shows samples of FlexPivot controls
____
Shows the following samples


* FlexPivotDemo: shows how to bind data, add custom menu item.


* CustomCellFactory: Shows how to apply conditional formatting to an FlexPivotGrid control using the FlexGrid's CustomCellFactory feature.
The FlexPivotGrid derives from the FlexGrid control, so you can use the standard CustomCellFactory mechanism to apply styles to cells based on their contents (or to draw the entire cell if you prefer).
This sample shows a grid where values greater than 500 appear with a light green background


* CustomColumns: Shows a custom calculated column in FlexPivotGrid
This project shows sales by country and category. It also shows a couple calculated columns that show the difference in product sales a custom total calculation.


* CustomPage: Shows how you can customize the FlexPivotPage control.
The sample creates a default view that is persisted across sessions in isolated storage. It also adds a new menu to the FlexPivotPage that contains a list of predefined views.
The IsolatedStorageSettings.ApplicationSettings class allows you to save and load application settings very easily.
The predefined views defined in this application show how you can use the FlexPivotField.Format property to group date values and analyze sales by year, month, and weekday.


* MultiValues: Shows how you can use FlexPivot to analyze multiple fields in one view.
The C1FlexPivotFieldList class has a new MaxItems property. This property allows you to determine how many fields are allowed in each field list (Rows, Columns, Filters, and Values).
If you set the MaxItems of the Values list to a number higher than one, users will be able to add multiple fields to the values list, and the analysis will be performed on all of them at once.
You can also use the MaxItems property on the Rows, Columns, and Filters lists if you want to limit the number of fields users can add to those lists. It rarely makes sense to have more than three or four Row or Column fields for example.


* CustomTemplate: Shows how to customize the FlexPivotPage component by creating a custom template based on the default one.
The sample creates a custom template (located in App.xaml) for the FlexPivotPage, this template is a customized version of the default one, the changes made to the template are:
The FlexPivotPanel is located on the right side of the FlexPivotPage.
The FlexPivotChart has been removed from the TabPanel at the top of the page and is shown below the FlexPivotGrid


* DataEngine: This sample shows how to get data to C1 DataEngine to perform analytics. C1DataEngine is capable of handling large amount of data, millions of rows in seconds or less. Data is retrieved from a database.
