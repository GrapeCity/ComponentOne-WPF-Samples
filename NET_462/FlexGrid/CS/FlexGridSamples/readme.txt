FlexGrid Samples
---------------------------------------------------
Shows the main features in the C1FlexGrid control.

The application has a tabbed interface. Each tab demonstrates a specific feature:

* Grouping:
Demonstrates how the grid supports the grouping feature in ICollectionView data 
sources. A checkbox allows users to group data by country or by country initial.
Also demonstrates cell merging, sorting, column dragging, resizing, and auto-sizing.

* Filtering:
Demonstrates how to use the FlexGridFilter extension dll to add column filtering
to the C1FlexGrid. Once you add the C1.WPF.FlexGridFilter assembly to the
project, call the C1FlexGrid.EnableFiltering extension method and users will be
able to filter the grid data based on values or conditions.

* Unbound:
Demonstrates how to use the grid in unbound mode (not bound to any data sources).
Also demonstrates cell merging.

* SelectionMode:
Shows all selection modes available in the C1FlexGrid.

* iTunes:
Demonstrates how to implement custom cells to change the look and feel of the grid.
Also demonstrates how to customize the group rows to display subtotals on each
cell, and how to implement a Search box using the ICollectionView filter feature.

* Financial:
Demonstrates how to implement custom cells with change flashes and sparklines.
Allows the user to control the speed with which data is updated to demonstrate
grid performance.

* Editing:
Demonstrates the editing features of the C1FlexGrid, including auto-complete 
columns, mapped columns, and Excel-like full and quick edit modes.

This same sample is available in Silverlight and WPF.
