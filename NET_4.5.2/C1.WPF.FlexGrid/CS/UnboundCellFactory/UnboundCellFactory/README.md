## UnboundCellFactory
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/CS/UnboundCellFactory/UnboundCellFactory)
____
#### Shows how to use a custom CellFactory to create custom cells in an unbound C1FlexGrid control.
____
The sample creates and populates an unbound C1FlexGrid, and then assigns it a 
custom CellFactory that implements the following cell types:

1) Checkboxes for boolean columns
2) Image indicators for "FaultType" columns
3) Radio buttons for "TestType" columns

In adition to these custom cell types, the CellFactory implements a notification 
mechanism that fires an events when the user edits any of the custom cells, or 
when he selects items from a list.

The event can be used to apply new values to cells, to customize rows, or to take 
any other action required by the application logic.

