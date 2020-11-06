## ColumnPicker
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/CS/ColumnPicker)
____
#### Shows how to implement a column picker context menu in the C1FlexGrid.
____
The sample handles the right-click event to detect what type of cell was
clicked.

If the user right-clicks a column header, the sample shows a context menu
with a list a CheckBox for each column. The CheckBox value corresponds to 
column visibility, so the user can see and toggle column visibility with
the mouse.

If the user right-clicks the cell area, the sample shows a context menu
with clipboard commands (cut, copy, paste, clear).

The sample also shows how to use the ColumnLayout property to save and
restore column layout information. The ColumnLayout property gets/sets 
an XML string that contains the order, visibility, and width of the 
columns on the grid.

The column layout string is automatically persisted across sessions, and
the user can choose to save and restore layouts using the buttons above
the grid.
