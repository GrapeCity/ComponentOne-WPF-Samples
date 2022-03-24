## CustomDialogs
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.Schedule/VB/CustomDialogs)
____
#### Demonstrates replacing default C1Scheduler dialogs by custom ones.
____
This sample contains 4 user controls:

* EditAppointmentControl - represents the content of Edit Appointment dialog;
* EditRecurrenceControl - represents the content of Edit Recurrence dialog;
* SelectFromListScene - represents the content of Select From List dialog;
* ShowRemindersControl - represents the content of Show Reminders window.

In MainPage.xaml file these controls are used to replace default scheduler dialogs:

```
   <UserControl.Resources>
		...
        <DataTemplate x:Key="customEditAppointmentTemplate">
             <local:EditAppointmentControl/>
        </DataTemplate>
        <DataTemplate x:Key="customSelectFromListSceneTemplate">
             <local:SelectFromListScene/>
        </DataTemplate>
        <DataTemplate x:Key="customShowRemindersTemplate">
             <local:ShowRemindersControl/>
        </DataTemplate>
        <DataTemplate x:Key="customEditRecurrenceTemplate">
             <local:EditRecurrenceControl/>
        </DataTemplate>
   </UserControl.Resources>
       ...
   <c1sched:C1Scheduler x:Name="sched1" 
                        EditAppointmentTemplate="{StaticResource customEditAppointmentTemplate}"
                        SelectFromListTemplate="{StaticResource customSelectFromListSceneTemplate}"
                        ShowRemindersTemplate="{StaticResource customShowRemindersTemplate}"
                        EditRecurrenceTemplate="{StaticResource customEditRecurrenceTemplate}"/>
```                             
Notes:

1. These controls represent a copy of corresponding scheduler controls. You can copy them into your project
and edit according your needs.   

2. In desktop application all dialogs are shown with standard Window class. In XBAP environment, all dialogs are shown in the C1.WPF.C1Window control.                         

