## CustomTypeDescriptor
#### [Download as zip](https://downgit.github.io/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/VB/CustomTypeDescriptor)
____
#### Demonstrates the C1FlexGrid support for the ICustomTypeDescriptor interface.
____
Most data binding in WPF and Silverlight is done through reflection.

But WPF allows data sources to customize the name and type of the properties
it exposes for data binding using the ICustomTypeDescriptor interface. That
allows WPF controls to support binding to data sources such as the 
System.Data.DataTable for example, and it also allows you to create generic
classes that expose a custom set of properties.

This sample shows how the C1FlexGrid supports binding to sources that
implement the ICustomTypeDescriptor interface. All the usual features are 
supported, including sorting, grouping, and filtering.

The ICustomTypeDescriptor interface is not available in Silverlight.

