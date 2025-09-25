CustomLocalization
----------------------------------------
Demonstrates how you can add custom localized resources for C1 controls.

This sample includes resx files for Russian culture under Properties folder. 
All resource files are copied from the Resources folder shipped along with product installation with the same folder structure. 
All included resource values have "::" in the beginning to distinguish them from embedded Russian resources. 
You can switch UI culture at runtime and make sure that custom resources are used instead of embedded ones.
In the same way you can override any C1 resources or add missing resources for your culture.

Known limitation: some controls like menu in C1DockingTabControl don't update resources if culture is changed at runtime. 
If control was first shown with some culture, it keeps showing the same text regardless of further culture changes.
