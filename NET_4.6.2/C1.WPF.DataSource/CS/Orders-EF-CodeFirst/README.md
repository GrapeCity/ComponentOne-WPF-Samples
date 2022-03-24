## Orders-EF
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.DataSource/CS/Orders-EF-CodeFirst)
____
#### Orders demo application with Entity Framework and WPF
____
This demo shows how an entire application can be built with 
C1DataSource with very little manual coding.

It shows the power of client-side data cache allowing to display and edit 
data on multiple tabs using a simple single data context, avoiding
difficulties of dealing with multiple contexts. Data on all tabs is
always in sync, changes made on one tab are automatically reflected
by all others.

"Edit Orders" tab shows master-detail data filtered by City.
Filtering is performed on the server to minimize data traffic,
and it is done without code. Changes can be made for multiple cities
and then saved to the database all together instead of saving
separately for each city (which would be necessary without C1DataSource).
Performance is increased by the data cache because repeating requests
for a city don't need to go to the server.

"Orders by Country" tab shows how all these advantages seamlessly
combine with paging.

"All Orders" tab shows how the innovative "virtual mode" technology
allows to show and edit large amounts of data in any standard controls
such as a data grid, without cumbersome paging, and with no delays
on many thousands or even millions of records, virtually independent
of the number of records.

On the "Products" tab you can try changing data and see how the
changes are automatically reflected on other tabs, without any
special synchronizing code. That is the power of "live views",
the LiveLinq technology.
