C1Scheduler Custom Grouping View
---------------------------
Demonstrates using of custom-defined Day/Working Week/Week C1Scheduler grouping styles instead of predefined ones.

<pre> 
CustomViews.xaml defines 3 scheduler styles:
- OneDayStyle;
- WorkingWeekStyle;
- WeekStyle.

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
</pre> 

The C1Scheduler.Theme property is set to CustomViews.xaml ResourceDictionary.
