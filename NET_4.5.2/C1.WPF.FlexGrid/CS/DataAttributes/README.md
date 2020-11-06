## DataAttributes
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/CS/DataAttributes)
____
#### Demonstrates how the FlexGrid supports the data attributes defined in the System.ComponentModel.DataAnnotations namespace.
____
Data attributes allow you to attach different kinds of meta-data to
data item properties. For example, you can specify the maximum length
of a string property, the order in which properties should be displayed
in a grid control, whether they should be displayed at all, the format
to be used for showing the values, etc.

Using data attributes instead of code has a couple of significant benefits:

1) It allows tools such as the ADO.NET Entity Framework to add meta-data 
to the classes it generates. This meta-data can then be used by all controls
that are bound to those data sources.

2) In manual scenarios, it allows you to specify how data should be 
displayed and validated in one place, ensuring that the specified behaviors
are consistent throughout the interface and are easy to maintain and update.
