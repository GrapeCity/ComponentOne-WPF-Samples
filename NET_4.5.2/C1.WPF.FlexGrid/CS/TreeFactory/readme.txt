TreeFactory
---------------------------------------------------------------------------
Shows how to create a Tree with connector lines using a custom cell factory.

The sample implements a cell factory that replaces the standard tree/group 
nodes with custom nodes built from a Grid element.

The custom nodes use bitmap images for the nodes and build connector lines
with Rectangle elements.

You may customize the appearance of the connector lines by changing the
brush used to fill the rectangles (_connBrush variable in the sample) and
the thickness of the connectors (_connThickness constant in the sample).