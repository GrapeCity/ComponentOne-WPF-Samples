﻿ExcelStyleMerge
--------------------------------------------------------------------------
Implement Excel-style merging with a custom CellFactory.

By default, the C1FlexGrid merges cells based on their content.

Excel, on the other hand, allows users to merge arbitrary selections.

This sample implements Excel-style merging using a custom MergeManager 
class that keeps a list of merged ranges. The application calls the 
merge manager's AddMergedRange and RemoveMergedRange to add or 
remove arbitrary merged ranges to the list.
