## C1TreeView Drag Drop
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF/VB/C1TreeViewDragDropSample)
____
#### Shows how to perform drag-drop operations within a C1TreeView control.
____
The C1TreeView control has an AllowDragDrop property that can be used to
enable standard drag drop operations within the tree.

The sample defines a class called CustomDragDrop that is used to customize
the drag drop operations by validating the drag drop operations and allowing 
move and copy actions.

In this sample, the data shows a company's employees and departments. 
Suppose we want the user to associate each employee with the department he or 
she works for. In this case, we don't allow dragging department nodes but 
only employee nodes.

The sample also shows different effects when dropping an employee into a department.
Moving an employee from one department to another reflects a transfer. Copying
the employee to a new department means he or she is now assigned to both.
