## ComboBox
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-WPF-Samples/tree/master/NET_4.6.2/C1.WPF.FlexGrid/CS/ComboBox/ComboBox)
____
#### Shows how to bind a FlexGrid column using a CellEditingTemplate with a ComboBox.
____
The example creates a ViewModel that loads data from the NorthWind OData
service at http://services.odata.org/Northwind/Northwind.svc/. The service
was created by adding a Service Reference to the project at design time.

The ViewModel exposes two collections: Categories and Products. The Products
collection is populated using an Expand("Category") clause to retrieve the
category along with each product (instead of simply the CategoryID).

The grid is created and bound to the Products collection in XAML. The columns
are also defined in XAML, and the "Category" column uses a CellTemplate
defined as follows:

```
<c1:Column Header="Category" Width="200">
  <c1:Column.CellTemplate>
    <DataTemplate>
      <ComboBox
        ItemsSource="{Binding Categories, Source={StaticResource _vm}}"
        SelectedValue="{Binding Category}"
        IsSynchronizedWithCurrentItem="False" >
        <ComboBox.ItemTemplate>
          <DataTemplate>
            <Grid>
              <Grid.ColumnDefinitions>
                <ColumnDefinition Width="25"/>
                <ColumnDefinition />
              </Grid.ColumnDefinitions>
              <Image Source="{Binding Picture}" />
              <TextBlock Grid.Column="1" Text="{Binding CategoryName}" />
            </Grid>
          </DataTemplate>
        </ComboBox.ItemTemplate>
      </ComboBox>
    </DataTemplate>
  </c1:Column.CellTemplate>
</c1:Column>
```
Notice the following important items:

1) The column defines a CellTemplate, which allows you to use any controls to
represent the items in the column. The controls in the CellTemplate are bound
to the item bound to the grid row (in this case, a Product).

2) The CellTemplate contains a ComboBox control with ItemsSource set to the 
Categories collection and SelectedValue bound to the current product's Category
property. This populates each ComboBox with the categories exposed by the
ViewModel.

3) The IsSynchronizedWithCurrentItem property of the ComboBox is set to false.
This prevents the ComboBox from changing the current item in the single Categories
collection which is shared by all the ComboBox controls and by the ViewModel itself.

4) The ComboBox itself contains a template that shows the image and name of the 
selected category. The image is bound to the Picture property of the Category, which
is a byte array returned by the OData service.

