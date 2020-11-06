## Invalidate
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/VB/Invalidate)
____
#### Shows how to use the Invalidate method to update cells dynamically.
____
Normally, cell contents are bound directly to data sources. When the data 
changes, the cell contents are updated automatically.

In some situations, however, you may want to force cells to be re-created
to reflect state changes that are not necessarily data-related, or that
would be difficult to bind to.

This sample uses a custom CellFactory that keeps a track of the cell currently
under the mouse. Whenever that cell changes, the cell factory invalidates the
previous hot cell and the new one.

The custom CellFactory highlights the hot cell with two red triangles.

