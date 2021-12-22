SQLiteDataBase
--------------------------------------------------------------------------------
Demonstrates binding a SQLite database to FlexGrid control.

FlexGrid can be bound to EntityFrameworCore source through C1EntityFrameworkCoreVirtualDataCollection and C1EntityFrameworkCoreCursorDataCollection from the C1.DataCollection.EntityFrameworkCore nuget package.

<code>
    var cv = new C1EntityFrameworkCoreVirtualDataCollection<Person>(db);
    grid.ItemsSource = cv;
</code>
