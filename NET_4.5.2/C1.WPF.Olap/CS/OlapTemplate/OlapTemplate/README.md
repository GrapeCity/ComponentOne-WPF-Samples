## OlapTemplate
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.Olap/CS/OlapTemplate/OlapTemplate)
____
#### Shows how to customize the OlapPage component by creating a custom template
____

The C1OlapPage has three main components that determine the visualization of
the data:

C1OlapPanel: the panel that is used to select and configure the fields that
will be used for Olap Analysis.

C1OlapChart: the component used to show the processed olap data in form of a
chat.

C1OlapGrid: the component used to show the processed olap data in form of a
grid.

The sample creates a custom template (located in App.xaml) for the C1OlapPage,
this template is a customized version of the default one, the changes made to
the template are:

* The C1OlapPanel is located on the right side of the C1OlapPage.
* The C1OlapChart has been removed from the TabPanel at the top of the page and
  is shown below the OlapGrid.
