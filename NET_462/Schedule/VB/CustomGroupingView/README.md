## C1Scheduler Custom Grouping View
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_462/Schedule/VB/CustomGroupingView)
____
#### Demonstrates using of custom-defined Day/Working Week/Week C1Scheduler grouping styles instead of predefined ones.
____
CustomViews.xaml defines 3 scheduler styles:

* OneDayStyle;
* WorkingWeekStyle;
* WeekStyle.

Default scheduler grouping look like this:

+----------------------------------+----------------------------------+
|            John                  |            Dave                  |
+----+----+----+----+----+----+----+----+----+----+----+----+----+----+
| Su | Mo | Tu | We | Th | Fr | Sa | Su | Mo | Tu | We | Th | Fr | Sa |
+----+----+----+----+----+----+----+----+----+----+----+----+----+----+
|    |    |    |    |    |    |    |    |    |    |    |    |    |    |
+----+----+----+----+----+----+----+----+----+----+----+----+----+----+
|    |    |    |    |    |    |    |    |    |    |    |    |    |    |
+----+----+----+----+----+----+----+----+----+----+----+----+----+----+

CustomViews.xaml defines different layout:

+---------+---------+---------+---------+---------+---------+---------+
|    Su   |    Mo   |    Tu   |    We   |    Th   |    Fr   |    Sa   |
+----+----+----+----+----+----+----+----+----+----+----+----+----+----+
|John|Dave|John|Dave|John|Dave|John|Dave|John|Dave|John|Dave|John|Dave|
+----+----+----+----+----+----+----+----+----+----+----+----+----+----+
|    |    |    |    |    |    |    |    |    |    |    |    |    |    |
+----+----+----+----+----+----+----+----+----+----+----+----+----+----+
|    |    |    |    |    |    |    |    |    |    |    |    |    |    |
+----+----+----+----+----+----+----+----+----+----+----+----+----+----+


The C1Scheduler.Theme property is set to CustomViews.xaml ResourceDictionary.
