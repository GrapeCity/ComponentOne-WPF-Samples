WealthHealth
-------------------------------------------------------------------------------------
Shows the evolution of income, life expectancy, and population in 178 countries over a period of 210 years.

It shows the dynamic fluctuation in per-capita income (x), life expectancy (y) and population (radius) 
of 180 nations over the last 209 years. Nations are colored by geographic region; mouseover to read 
their names.

As Mike Bostock and Tom Carden noted, there’s a surprising amount of work that goes into making something
look simple. For one, data collected in recent years is consistent, while data prior to 1950 is sparse;
although potentially misleading, these visualizations use linear interpolation for missing data points.

The sample loads the base data into a List and binds that to a FlexChart. It keeps track of 
the current year and interpolates the life expectancy, income, and population for each data point
as the current year changes. The chart updates automatically, and very quickly.

The data and original charts were created by <a href="https://www.gapminder.org/world/">GapMinder</a>.

This is one of the best charts I have ever seen. It uses four dimensions (income, life expectancy, 
population, and time) to provide an amazing amount of information in a single chart. It condenses
a huge amount of data into a single, easy-to-understand chart that tells a compelling and very 
interesting story.
