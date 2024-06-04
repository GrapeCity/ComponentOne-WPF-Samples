CustomCellFactory
---------------------------------------------------------------------------------------------------------------
Shows how to apply conditional formatting to an OlapGrid control using the C1FlexGrid's CustomCellFactory feature.

The OlapGrid derives from the C1FlexGrid control, so you can use the standard CustomCellFactory
mechanism to apply styles to cells based on their contents (or to draw the entire cell 
if you prefer).

This sample shows a grid where values greater than 500 appear with a light green background.