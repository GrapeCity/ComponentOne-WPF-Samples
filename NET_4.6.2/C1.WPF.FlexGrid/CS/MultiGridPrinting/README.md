## MultiGridPrinting
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/MultiGridPrinting)
____
#### Shows how you can print multiple grids into a single document.
____
The C1FlexGrid has a Print method that prints the grid. The Print method allows
you to specify basic printing parameters such as document title, margins, and
scaling mode. The Print method is the easy way to provide basic printing.

If you need more flexibility, the C1FlexGrid also provides a GetPageImages 
method that returns a list of images that represent a paged view of the grid.
These images can then be added to pages in a PrintDocument to print the grid 
by itself or in combination with other elements.

The sample achieves this by creating a PrintDocument object and implementing
a handler for the PrintPage event that creates images for all the grids that
have to be rendered.

The GetPageImages method is typically used as follows:

1) Create a DocumentPaginator object and use the GetPageImages method to 
   create a list of elements that represent portions of the document.

2) Override the GetPage method to return the DocumentPage object that
   represents the page to be printed. Also override the other required
   properties PageCount, Source, PageSize, and IsPageCountValid.

3) Pass the DocumentPaginator object as a parameter in a call to the
   PrintDialog.PrintDocument method.

In this sample, all the paged views of the FlexGrids are added to a list of FrameWork Elements 
and are printed using the PrintDocument method
