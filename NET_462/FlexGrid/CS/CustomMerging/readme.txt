CustomMerging
------------------------------------------------------------------------
Implement your own merging logic to create a TV-guide display.

The C1FlexGrid provides a default merging mechanism that automatically
merges cells that contain the same content.

You can implement your own custom merging logic by creating a class
that implements the IMergeManager interface and assigning it to the
MergeManager property.

This sample shows implements a custom IMergeManager that creates merged
ranges with multiple rows and columns. The sample creates a TV schedule 
and merges programs across weekdays (columns) and show times (rows).

The original version of this sample was written for the WinForms C1FlexGrid.

