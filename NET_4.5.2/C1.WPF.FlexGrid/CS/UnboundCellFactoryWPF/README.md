## UnboundCellFactory Extended
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/\NET_4.5.2\C1.WPF.FlexGrid\CS\UnboundCellFactoryWPF)
____
#### Shows how you can create custom cells in an unbound C1FlexGrid.
____
The sample defines a CellFactory that creates cells using several different types
of elements (TextBox, CheckBox, ComboBox, Slider, and ColorPicker). 

The elements types in the sample are specified by assigning their type to each 
row's Tag property. Different applications would probably use different
schemes for deciding the type of element that should be used to edit each cell.

The edited values are stored directly into the grid. The sample allows you to
disable the custom cell factory so you can see the values stored in the grid
as text.

Important points to notice are:

1) The custom cell factory disables the grid's built-in editing for all custom
cells. This is done because the elements created by the custom cell factory are
used both to display and to edit the cell values.

2) The custom cell factory handles the grid's ScrollPositionChanging event to
finish any pending edits. This ensures that any values being edited are committed
to the grid's storage before the editor scrolls off view.

3) The custom cell factory uses the element's LostFocus event to store the current 
value of each cell into the grid. This is usually the best strategy since
changing the value stored in the grid causes it to invalidate the cell, 
re-creating the editor.

