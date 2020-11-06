## GapAnalysis
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/\NET_4.5.2\C1.WPF.FlexGrid\CS\GapAnalysis)
____
#### This sample highlights some of the main features in the C1FlexGrid.
____
The sample presents three tabs, each focusing on a specific scenario and 
highlighting specific C1FlexGrid features:


* Traditional

This tab shows a grid bound to a flat data source. Each item on the data
source represents a product item.

The tab demonstrates two-way binding, data-entry validation, grouping,
sorting, automatic group and sort updates, custom editors (combo and
check boxes), auto-complete, and cell selection.

The automatic group and sort updates are handled by the data source
(an ICollectionView). These are features supported by the grid but not
implemented by it.


* Hierarchical

This tab shows a grid bound to a hierarchical data source. The data source
consists of a list of File objects. Each File objects has a Files property
that contains a list of File objects, resulting in a hierarchical structure
similar to a directory tree.

The grid displays this hierarchical data as a tree, using the ChildItems
property which specifies that the "Files" property should be used to retrieve
the child items for each File object.

In this scenario, the grid acts as a TreeView control. The advantages of the
grid are that it shows multiple columns and provides virtualization.


* Form-Style

This tab shows a grid bound to a list of FormItem objects. Each form item may 
represent a data entry or a calculated field. Fields other than data-entry
cannot be edited. When a data entry field is edited, the calculated fields
are updated automatically.

The grid supports this scenario using a custom CellFactory that examines the
type of cell being created and customizes its properties accordingly (read-only,
background color, font, etc).

The grid also customizes the behavior of the Tab key, so pressing tab moves
the selection to the next data entry cell on the grid.

This sample illustrates the use of custom CellFactory objects, which convert
data objects into grid cells and correspond to the owner-draw feature found
in many WinForms controls. The mechanism is also similar to the CellTemplate
and CellEditingTemplate properties available on the grid, but the CellFactory
provides extra flexibility because it does not rely on markup only.

The CellFactory and CellTemplate/CellEditingTemplate properties are responsible
for allowing the use of custom UI elements to represent grid cells. They could
be used for example to provide custom cell editors or features like spark-lines.

