## MultiColumnCombo
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_462/FlexGrid/CS/MultiColumnCombo/MultiColumnCombo)
____
#### Shows how to implement a multi-column combo box using the C1FlexGrid.
____
The sample contains a MultiColumnDropDown control that is similar to the standard
ComboBox control but contains a C1FlexGrid in the drop-down instead of a simple
ListBox.

The MultiColumnDropDown control has the following main properties:


* ItemsSource: This property contains a list of objects that represents valid
choices.


* SelectedIndex: Gets or set the index of the item currently selected.


* DisplayMemberPath: Gets or sets the name of the property whose value appears
in the control as the user selects or types new entries. For example, if
ItemsSource contains a list of products and DisplayMemberPath is set to
"ProductName", then the Text property will contain the name of the product
that is currently selected. Also, the control will search and select values
by product name as the user types into the control.


* SelectedValuePath: Gets or sets the name of the property that defines the
value returned by the SelectedValue property. For example, if ItemsSource 
contains a list of products and SelectedValuePath is set to "ProductID", 
then the SelectedValue property will contain the ID of the product
that is currently selected.


* SelectedValue: Gets or sets the item selected based on the items available
in the ItemsSource property and on the setting of the SelectedValuePath
property.


* DropDownGrid: Gets or sets a C1FlexGrid used as a template for the grid
in the drop-down area of the control. If this property is not set, then
the MultiColumnCombo shows only a single column containing the values
of the property defined by the DisplayMemberPath property. If the
DropDownGrid property is set, then the drop-down will contain the grid
defined by this property, including all the columns, behavior and appearance
properties.

For example, the code below creates a MultiColumnCombo that allows users
to pick a product category from a list. The control will display the 
category name in the TextBox area, and the drop-down will display a
grid with three columns containing the category ID, name, and description.
The SelectedValue property will contain the category ID:

```
    <local:MultiColumnComboBox
        Background="White" BorderThickness="1" Width="200"
        ItemsSource="{Binding Categories}" 
		SelectedValuePath="CategoryID" 
		DisplayMemberPath="CategoryName" >
        <local:MultiColumnComboBox.DropDownGrid>
        <c1:C1FlexGrid
            AutoGenerateColumns="False" SelectionMode="Row"
            RowBackground="White" AlternatingRowBackground="White" >
            <c1:C1FlexGrid.Columns>
            <c1:Column Binding="{Binding CategoryID}" Header="ID" Width="50" />
            <c1:Column Binding="{Binding CategoryName}" Header="Name" Width="200" Background="#FFFADE" />
            <c1:Column Binding="{Binding Description}" Width="*" MinWidth="350"/>
            </c1:C1FlexGrid.Columns>
        </c1:C1FlexGrid>
        </local:MultiColumnComboBox.DropDownGrid>
    </local:MultiColumnComboBox>
```
The MultiColumnCombo control can be used as a cell editor in the C1FlexGrid control. For example,
supposed you have a grid bound to a collection of Product objects, and you would like the 
Category column to be bound to the product's CategoryID property and to display the category name.
You could accomplish this by setting the column's ValueConverter property to a ColumnConverter
created to map category IDs and names:

```
	public CategoryConverter()
    {
		var vm = new ViewModel();
        base.SetSource(vm.Categories, "CategoryID", "CategoryName");
	}
```
This converter would allow the grid to bind to the CategoryID property of the Product, but
to display the CategoryName property instead. The default editor in this case would be a 
combo box containing the category names.

If you wanted to use a MultiColumnCombo instead of the built-in editor, you would specify 
it in a CellEditingTemplate. For example:

```
    <c1:Column 
		Binding="{Binding CategoryID, Mode=TwoWay}" 
		Header="Category" Width="200" ValueConverter="{StaticResource _cvtCat}">
        <c1:Column.CellEditingTemplate>
        <DataTemplate>
            <local:MultiColumnComboBox
                ItemsSource="{Binding Categories, Source={StaticResource _vm}}"
                SelectedValuePath="CategoryID"
                DisplayMemberPath="CategoryName"
                SelectedValue="{Binding CategoryID, Mode=TwoWay}" >
            <local:MultiColumnComboBox.DropDownGrid>
                <c1:C1FlexGrid
                AutoGenerateColumns="False" SelectionMode="Row"
                RowBackground="White" AlternatingRowBackground="White" >
                <c1:C1FlexGrid.Columns>
                    <c1:Column Binding="{Binding CategoryID}" Header="ID" Width="50" />
                    <c1:Column Binding="{Binding CategoryName}" Header="Name" Width="200" Background="#FFFADE" />
                    <c1:Column Binding="{Binding Description}" Width="*" MinWidth="350"/>
                </c1:C1FlexGrid.Columns>
                </c1:C1FlexGrid>
            </local:MultiColumnComboBox.DropDownGrid>
            </local:MultiColumnComboBox>
            </DataTemplate>
        </c1:Column.CellEditingTemplate>
    </c1:Column>
```
Notice that you could customize the drop-down grid extensively in the markup. For example, you
could use custom fonts and colors, decide which columns should be displayed, whether to show
row and column header cells, etc.
