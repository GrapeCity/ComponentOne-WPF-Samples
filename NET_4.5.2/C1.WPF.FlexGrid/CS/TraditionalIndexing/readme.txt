TraditionalIndexing
--------------------------------------------------------------------------------
Shows how you can emulate the traditional indexing method used in the C1FlexGrid for WinForms.

One important difference between the WinForms C1FlexGrid control and the Silverlight/WPF 
version is that in the WinForms version of the control, the indices included fixed rows 
and columns.

In the Silverlight/WPF version, fixed rows and columns are not included in the count. Instead,
fixed cells are stored in separate objects accessible through the ColumnHeaders and RowHeaders
properties.

The new notation makes indexing easier because the indices match the index of the data items 
(row zero contains item zero) and the column count matches the number of properties being 
displayed.

However, if you prefer the old indexing scheme (or are porting large applications that rely
on it), you can use the approach illustrated in this sample. The sample implements a derived
class that overrides the Rows and Columns properties and return custom collections that 
include header and data cells, just as the original C1FlexGrid does.
