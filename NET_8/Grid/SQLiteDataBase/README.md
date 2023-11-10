## SQLiteDataBase
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_8/Grid/SQLiteDataBase)
____
#### Demonstrates binding a SQLite database to FlexGrid control.
____
FlexGrid can be bound to EntityFrameworCore source through C1EntityFrameworkCoreVirtualDataCollection and C1EntityFrameworkCoreCursorDataCollection from the C1.DataCollection.EntityFrameworkCore nuget package.

```
    var cv = new C1EntityFrameworkCoreVirtualDataCollection<Person>(db);
    grid.ItemsSource = cv;
```