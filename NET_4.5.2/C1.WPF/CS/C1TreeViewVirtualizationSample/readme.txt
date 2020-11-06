C1TreeView Virtualization
---------------------------
Shows how to use IsVirtualizing property to optimize the performance.


You may want to show many hierarchical C1TreeViewItem elements. But it 
always take very long time to create these elements and add them into 
the visual tree. Now you can use IsVirtualizing property to improve the
 performance. Only the C1TreeViewItem elements in view port size would 
be created and added into the visual tree if you set IsVirtualizing to true.

