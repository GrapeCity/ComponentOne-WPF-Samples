## MultiUser
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/\NET_4.5.2\C1.WPF.Schedule\VB\MultiUser)
____
#### Demonstrates showing multi-user calendar.
____
AppoinmentStorage, OwnerStorage and ContactStorage are bound to data from NWind database 
(which is included into solution). Other storages are used in unbound mode.
C1Scheduler.GroupBy property is set to "Owner".

Appointments table from Nwind.mdb contains Owner column for keeping 
information about Appointment's owner. This column contains key values from the Employees table.

The list of all users is shown in the left bottom corner. C1Scheduler only shows groups for the checked users.
"Calendar" group shows appointments with an empty owner.

Window1.xaml contains custom-defined group header template. It shows additional navigation buttons and additional 
information from the Employee data table.

End-user can drag appointments from the one calendar to another. In such case Appointment.Owner property gets changed.



