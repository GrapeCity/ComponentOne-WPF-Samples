## ExcelBook
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/ExcelBook/ExcelBook)
____
#### Shows a class that extends the C1FlexGrid to provide many Excel-like features.
____
This sample contains two projects: a reusable C1FlexGridBook class that 
extends the C1FlexGrid and provides Excel-like features, and an ExcelBook 
project that shows the C1FlexGridBook class in action.

The reusable C1FlexGridBook class provides the following features:


* Multi-sheet tabbed interface:

The sample is called "ExcelBook" because it shows a grid with multiple sheets.
The sheets are represented by tabs that appear below the grid, to the left
of the grid's horizontal scrollbar.


* Save and Load XLSX files:

The sample uses the C1.Silverlight.Excel component to load and save multi-sheet,
styled workbooks in XLSX format.


* Load TXT/CSV files:

Add support to load txt and csv file.


* Formula support:

The C1FlexGridBook control has a "CalcEngine" class that parses and evaluates
Excel-style formulas. The engine currently supports 69 functions and is easily
extensible. The engine also evaluates properties of the grid's DataContext.
For example:

```
	// show the amount of taxes owed by a customer in cell "A1"
	// (assuming the tax is calculated as 8.5% of the value of the 
	// Customer.Amount property)
	_flex.DataContext = new Customer();
	_flex[0, 0] = "= Amount * 8.25%";
```
All strings starting with an equals sign are treated as formulas. To display an
actual string that starts with an equals sign, precede it with a single quote
(as in Excel). For example:
	
```
	_flex[0,0] = "'==============";
```

* Hyperlink support:

The C1FlexGridBook supports the HYPERLINK function. If you enter a formula such as
"=HYPERLINK('https://developer.mescius.com/componentone', 'ComponentOne')" in a cell, then the
grid will create a HyperlinkButton element in the cell. The element can be clicked
to navigate to the URL provided, and it can be edited and formatted like any other
cell.


* ColorScheme:

The C1FlexGridBook control has a ColorScheme property that allows you to select 
color schemes that match the Microsoft Office schemes: Blue, Silver, and Black.


* Row and Column Headers:

The C1FlexGridBook control has a custom CellFactory that generates labels for
the column and row headers. Column headers display letters and row headers
display the row index.


* Custom Outlines:

The C1FlexGridBook control CellFactory generates images for expanded and collapsed
nodes that look like the icons used in Excel.


* Pdf Export:

The C1FlexGridBook control uses the C1.Silverlight.Pdf assembly and the GetPageImages
method to export the grid to Pdf files.


* Undo/Redo:

The C1FlexGridBook control implements a flexible and extensible undo/redo stack.


* Range Sorting:

The C1FlexGridBook control provides range-based, unbound sorting.


* Enhanced Filtering:

The C1FlexGridBook control extends the standard column filtering capabilities in 
the C1FlexGrid to provide the following additional features:


* sort control
* filtered value selection in the value filter editor
* resizable editor
* Excel appearance

