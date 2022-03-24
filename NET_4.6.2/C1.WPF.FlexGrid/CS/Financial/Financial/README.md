## Financial
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/Financial/Financial)
____
#### Shows a simulated live data feed of stock quotes for over 3,000 companies.
____
This FinancialDataList class is responsible for storing the collection of stock symbols, their values and updating them on a regular frequency (using a timer). The FinancialData class represents a single company and it even stores up to the 5 most recent values for each of its properties. This allows us to visualize historic data in a small sparkline. FinancialData also implements INotifyPropertyChanged, which means the UI will be notified of updates to our data values.

We display spark lines in the FlexGrid using a CellFactory and a Polyline element bound to historic values for each symbol.
