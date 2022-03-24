## ColumnFooters
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/ColumnFooters)
____
#### Shows how you can add column footer cells that remain visible when the user scrolls the control vertically.
____
In addition to regular data, the FlexGrid allows you to show row 
and column headers, and also column footers.

To create and use column footers, use the "ColumnFooters" property, which
returns a "GridPanel" object that has the same object model as the 
column headers, row headers, and cells.

For example, the following code adds and formats a column footer row to 
the grid:

```
 void AddColumnFooter(C1.WPF.FlexGrid.C1FlexGrid flex)
        {
            

            flex.ColumnFooters.Columns.Add(new Column());
            var gr = new C1.WPF.FlexGrid.GroupRow();

            // customize appearance of the new row
            gr.FontWeight = FontWeights.Bold;
            gr.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x80, 0x00, 0x00));
            gr.Foreground = new SolidColorBrush(Colors.White);
            // add the row to the ColumnFooters GridPanel
            flex.ColumnFooters.Rows.Add(gr);
            gr[0] = "Totals";
         
        }
```
If you add a "GroupRow" object to the "Rows" collection of the column footer
panel, and set the "GroupAggregate" property on one or more grid columns, then
the column footer row will automatically display and update the requested 
aggregate on each column. This works in bound and unbound modes.

If you want to control the information displayed in the column footers, you
can assign values directly to cells as you would do for any of the other 
grid panels. For example, the following code assigns the string "Totals" to
the first cell in the column footer row:

```
	_flex.ColumnFooters[0,0] = "Totals";
```