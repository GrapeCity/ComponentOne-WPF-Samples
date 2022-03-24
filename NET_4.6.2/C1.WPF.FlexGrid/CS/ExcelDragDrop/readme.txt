ExcelDragDrop
--------------------------------------------------------------------------------------------
Shows how you can implement Excel-style drag drop features in the C1FlexGrid.

The sample uses the C1DragDropManager class to implement Excel-style drag drop features.

Select a range of cells and move the mouse near the edges of the selection. When you
do that, the mouse cursor will change into a hand to indicate you can start dragging
the selection. At this point, you can press the mouse button and start dragging the
selection to a new location, either within the same grid or to a new one.

Release the mouse button on a valid location and the source data will be copied to 
the destination.

The sample uses the PreviewMouseLeftButtonDown event to monitor mouse down events 
before they are consumed by the C1FlexGrid. If the mouse down occurs in a valid 
drag location, then the code calls the C1DragDropManager.DoDragDrop method to 
initiate the drag process. This is done with the following code:

<code>
  // start dragging when the user clicks the mouse in the drag zone
  void _flex_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    var flex = sender as C1FlexGrid;
    if (MouseInDragArea(flex, e))
    {
      _dd.DoDragDrop(flex.Marquee, e, DragDropEffect.Copy);
      e.Handled = true;
    }
  }
</code>

The sample uses the GetGridImage method to provide a semi-transparent copy of the
data being dragged over the source. This is done with the following code:

<code>
  // create image for dragging
  var flexImage = flex.GetGridImage(flex.Selection) as C1FlexGrid;
  flexImage.HeadersVisibility = HeadersVisibility.None;
  flexImage.Opacity = 0.5;
  _dd.TargetMarker.Child = flexImage;
</code>

Finally, the sample shows two ways of transferring the data between the source and
target areas. One option is to use a loop and copy the data one cell at a time.
The other option is to use the clipboard, using the following code:

<code>
  void _dd_DragDrop(object source, DragDropEventArgs e)
  {
    // get source and target grids
    var src = GetParentOfType<C1FlexGrid>(e.DragSource);
    var dst = GetDropTarget(e);

    // transfer data using the clipboard
    // (selection is already set on the destination grid)
    src.ClipboardCopyMode = ClipboardCopyMode.ExcludeHeader;
    src.Copy();
    dst.Paste();
  }
</code>
