## C1TreeView Virtualization
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_462/Core/CS/C1TreeViewVirtualizationSample)
____
#### Shows how to use IsVirtualizing property to optimize the performance.
____

You may want to show many hierarchical C1TreeViewItem elements. But it 
always take very long time to create these elements and add them into 
the visual tree. Now you can use IsVirtualizing property to improve the
 performance. Only the C1TreeViewItem elements in view port size would 
be created and added into the visual tree if you set IsVirtualizing to true.

