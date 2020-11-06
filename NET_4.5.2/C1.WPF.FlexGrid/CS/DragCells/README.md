## DragCells
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/\NET_4.5.2\C1.WPF.FlexGrid\CS\DragCells)
____
#### Shows how to perform cell drag and drop operations within the grid.
____
The sample stores custom data objects in grid cells (unbound mode), and uses
a custom CellFactory to create visual representations for the cells in the
current ViewRange.

The data objects are stored in the grid itself, and are permanent; the 
visuals are created when cells become visible and are automatically
destroyed when the cells scroll out of view.

The sample uses a C1DragDropManager to handle the actual drag/drop operations.
When the user drops a cell in a new location, the sample moves the data
object from the original cell to the target cell:

	// move object to target cell 
	var oldRange = new CellRange(bdr);
	_flex[oldRange.Row, oldRange.Column] = null;
	_flex[ht.Row, ht.Column] = viewObject.DataObject;

The grid detects the change and updates the view automatically, removing
the original view object and creating a new one at the target location.
