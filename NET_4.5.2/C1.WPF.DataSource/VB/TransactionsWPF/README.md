## TransactionsWPF
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.DataSource/VB/TransactionsWPF)
____
#### Client-side transactions in WPF.
____
Client-side transactions is a mechanism implemented in C1DataSource
for canceling changes on the client without involving
the server. For details, see "Client-Side Transactions" in
"Programming Guilde" in the documentation.

This demo application shows how to undo changes with a Cancel button,
including in nested (child) dialogs/forms, using all three methods
of working with client-side transactions:
(1) associating a transaction with a live view, (2) binding to 
an object tracking changes using ScopeDataContext method, (3) opening
a transaction Scope() manually in code.

This sample is for WPF. Corresponding WinForms and Silverlight samples
are also provided.
