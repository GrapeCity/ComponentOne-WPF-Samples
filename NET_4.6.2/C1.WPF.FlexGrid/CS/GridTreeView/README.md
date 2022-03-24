## GridTreeView
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/GridTreeView)
____
#### Shows how you can use the C1FlexGrid to implement a bound hierarchical TreeView.
____
The ChildItemsPath property allows you to use the FlexGrid as a bound TreeView 
control.

To use it, the data source must contain items that have properties which are 
collections of the same type as the main items.
	
The sample defines a Person class with a Children property that contains 
a list of Person objects (which themselves may have children). 

It then defines a person with a whole family tree under it, and binds this person
to a FlexGrid and to a TreeView control.

This is done with the following XAML:

```
   <c1:C1FlexGrid x:Name="_flex" Grid.Column="1" Grid.Row="2"
            AutoGenerateColumns="False" 
            ChildItemsPath="Children" >
            <c1:C1FlexGrid.Columns>
                <c1:Column Header="Name" Binding="{Binding Name, Mode=TwoWay}" Width="*" />
                <c1:Column Header="Children" Binding="{Binding Children.Count}" HorizontalAlignment="Right" />
            </c1:C1FlexGrid.Columns>
        </c1:C1FlexGrid>
```
The grid automatically displays each Person that has children as a group row, and each 
person that doesn't have children as a regular row.

The sample also populates a FlexGrid and a TreeView to the same data source, to show
how you could do it if you didn't want to use data binding.

Notice that unlike the TreeView, the FlexGrid allows you to show multiple columns for each
object, to edit values, and it offers methods such as CollapseGroupsToLevel that allow you 
to control the tree in ways that are not possible using a TreeView control.

