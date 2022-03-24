## CheckBoxes
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/VB/CheckBoxes/CheckBoxes)
____
#### Shows how you can use a custom CellFactory to display check boxes in unbound grids.
____
In bound mode, the FlexGrid uses checkboxes to show boolean values. This is the 
same behavior as most other grids, including the FlexGrid for WinForms. The simplest 
example of bound mode with checkboxes would be something like this:

```
    ' create a list of Rect objects (the data source
    Dim i As Integer
    Dim list As New List(Of Rect)
    For i = 1 To 100
        list.Add(New Rect(i, i, i, i))
    Next

    ' bind the list to the grid
    _fgBound.ItemsSource = list
```
By default, the grid will show all properties of the Rect objects, including the boolean 
IsEmpty which is shown as checkboxes.

In unbound mode, the grid stores values directly in cells. The values stored in the cells 
are of type Object, which means any cell can contain any value. In this case, the grid 
does not try to interpret the value at all. It simply converts the objects to strings and 
shows the result. So boolean values are shown as True/False, as in Excel.

The FlexGrid allows you to override this default behavior by using a custom CellFactory. 
This is an extremely powerful feature, a little harder to use than the SetCellChecked method 
in the WinForms version, but a lot more flexible and powerful. For example, you could create 
a custom CellFactory that automatically creates checkboxes to show boolean values. Or you 
could store the checkboxes (or any other control) directly in the cells, and have the cell
factory simply display those controls in the cells.

This sample attached here shows how you can do all this. It implements a custom CellFactory 
that does two things:

1) If a cell contains a boolean value, the factory creates a checkbox to represent the cell

2) If a cell contains a Control object, the factory shows the control in the cell.

You could use the same approach and extend the factory to show date pickers, rich text boxes, 
sliders, charts, or any other control at all. The CellFactory feature is is conceptually similar 
to the OwnerDrawCell feature in the WinForms FlexGrid. It provides total flexibility so you can 
define the appearance and behavior of every cell.
