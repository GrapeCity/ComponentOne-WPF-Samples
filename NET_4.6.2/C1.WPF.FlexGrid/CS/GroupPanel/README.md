## GroupPanel
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/GroupPanel)
____
#### Shows how you can use the C1FlexGridGroupPanel to provide custom grouping to FlexGrid controls.
____
The sample shows two tabs that demonstrate the following:

1) Basic Grouping

This tab shows basic grouping features provided by the C1FlexgGridGroupPanel class. You can drag
column headers into the grouping area to create groups. The group markers can also be dragged
to re-arrange the grouping hierarchy, or clicked to sort or remove groups.

When you create groups, the corresponding columns are automatically hidden in the grid. This
is the C1FlexgGridGroupPanel's default behavior, which you can change by setting the 
HideGroupedColumns property to false.

2) Custom Grouping

This tab shows how you can customize the C1FlexgGridGroupPanel to support custom group converters,
prevent the C1FlexgGridGroupPanel from hiding grouped columns, and specify the maximum number of 
groups allowed.

The custom group converters are used to show amounts in categories (high, medium, low) and dates
in ranges (this week, this month, this year, etc).
