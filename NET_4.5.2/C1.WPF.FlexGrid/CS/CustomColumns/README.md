## CustomColumns
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.5.2/C1.WPF.FlexGrid/CS/CustomColumns)
____
#### Shows how you can combine custom columns with regular bound columns.
____
The sample shows a grid with several columns that display data as usual, plus
three custom columns that perform special actions:

ShowDetail: This column contains a button that brings up a 
message box showing details of the item that is bound to the current row.

MoveUpDown: This column contains two buttons that can be used to move the
item up or down within the data source.

DeleteRecord: This column contains a button that deletes the current item
from the data source.

All custom columns are implemented in XAML, using the Column.CellTemplate
property. This is what the column definitions look like:

```
   <c1:Column ColumnName="ShowDetail" >
                    <c1:Column.CellTemplate>
                        <DataTemplate>
                            <Button Content="Detail..." Click="ShowDetail_Click" />
                        </DataTemplate>
                    </c1:Column.CellTemplate>
                </c1:Column>
                <c1:Column ColumnName="MoveUpDown" >
                    <c1:Column.CellTemplate>
                        <DataTemplate>
                            <StackPanel Orientation="Horizontal" HorizontalAlignment="Center">
                                <Button Content="^" Click="MoveUp_Click" />
                                <Button Content="v" Click="MoveDown_Click" />
                            </StackPanel>
                        </DataTemplate>
                    </c1:Column.CellTemplate>
                </c1:Column>
                <c1:Column ColumnName="DeleteRecord" >
                    <c1:Column.CellTemplate>
                        <DataTemplate>
                            <Button Content="Delete..." Click="DeleteButton_Click" />
                        </DataTemplate>
                    </c1:Column.CellTemplate>
                </c1:Column>
```
The event handlers called when the user clicks buttons in the custom
columns determine the corresponding item in the data source using the DataContext
property of the element that triggered the event.

In this example, the data items are of type 'Product', and there is a GetProduct
method defined as follows:

```
    // get the product that is represented by a control on the grid
    Product GetProduct(object control)
    {
        FrameworkElement e = control as FrameworkElement;
        return e.DataContext as Product;
    }
```