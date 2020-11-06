BasicBinding
------------------------------------------------------------------------
Bind your controls to LINQ queries, that is, to LiveLinq live views.

Standard LINQ queries are static, their results do not change automatically
when underlying data change. 

LiveLinq query result is a live view, which means it is kept constantly
up-to-date without re-populating it every time its base data changes.
This makes LiveLinq extremely useful in common data-binding scenarios where
objects are edited and may be filtered in or out of views, have their
associated subtotals updated, and so on.

