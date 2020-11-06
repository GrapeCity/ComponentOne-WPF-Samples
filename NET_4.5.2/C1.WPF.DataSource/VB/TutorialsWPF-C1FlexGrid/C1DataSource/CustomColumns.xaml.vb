Public Class CustomColumns

    Public Sub New()
        InitializeComponent()

        ' Define a live view with custom fields.
        ' The data grid will automatically generate columns based on these fields.
        dataGrid1.ItemsSource = _
            (From p In c1DataSource1("Products").AsLive(Of Product)()
             Select New With
             {
                 p.ProductID,
                 p.ProductName,
                 p.CategoryID,
                 p.Category.CategoryName,
                 p.SupplierID,
                 .Supplier = p.Supplier.CompanyName,
                 p.UnitPrice,
                 p.QuantityPerUnit,
                 p.UnitsInStock,
                 p.UnitsOnOrder
             }).AsDynamic() ' AsDynamic() is required for data binding because an anonymous class is used (select new ...)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        ' add a new product
        Dim newItem = CType(dataGrid1.ItemsSource, System.ComponentModel.IEditableCollectionView).AddNew()
        dataGrid1.ScrollIntoView(dataGrid1.Rows.Count - 1, 0)
    End Sub

End Class
