## Printing
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_6/Grid/Printing)
____
#### Demonstrates the printing features of the FlexGrid control.
____
The FlexGrid has a Print method that prints the grid. The Print method allows
you to specify basic printing parameters such as document title, margins, and
scaling mode. The Print method is the easy way to provide basic printing.

If you need more flexibility, the FlexGrid also provides a GetPageImages 
method that returns a list of images that represent a paged view of the grid.
These images can then be added to pages in a PrintDocument to print the grid 
by itself or in combination with other elements.

The sample shows how you can print grids using either method (Print or 
GetPageImages).

The sample also provides a choice of margins, scaling (actual size, fit 
to page width, fit to a single page, print selection) and page orientation.

The GetPageImages method is typically used as follows:

1) Create a DocumentPaginator object and use the GetPageImages method to 
   create a list of elements that represent portions of the document.

2) Override the GetPage method to return the DocumentPage object that
   represents the page to be printed. Also override the other required
   properties PageCount, Source, PageSize, and IsPageCountValid.

3) Pass the DocumentPaginator object as a parameter in a call to the
   PrintDialog.PrintDocument method.

The sample implements a FlexPaginator class based on the following article:

	http://www.switchonthecode.com/tutorials/wpf-printing-part-2-pagination

The FlexPaginator performs all the steps listed above. You can use it
as follows:

```
    var pd = new PrintDialog();
    if (pd.ShowDialog().Value)
    {
        // get margins, scale mode
        var margin =
            _cmbMargins.SelectedIndex == 0 ? 96.0 / 4 :
            _cmbMargins.SelectedIndex == 1 ? 96.0 / 2 :
            96.0;
        var scaleMode =
            _cmbZoom.SelectedIndex == 0 ? ScaleMode.ActualSize :
            _cmbZoom.SelectedIndex == 1 ? ScaleMode.PageWidth :
            ScaleMode.SinglePage;

        // calculate page size
        var sz = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

        // create paginator
        var paginator = new FlexPaginator(_flex, scaleMode, sz, new Thickness(margin), 100);
        pd.PrintDocument(paginator, "FlexGrid printing example");
    }
```