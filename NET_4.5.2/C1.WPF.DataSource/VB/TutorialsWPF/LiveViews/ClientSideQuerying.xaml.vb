Imports C1.Data.Entities
Imports C1.Data
Imports C1.LiveLinq
Imports C1.LiveLinq.LiveViews

Public Class ClientSideQuerying
    ' This client scope will be used to load the data.
    Private _scope As EntityClientScope = Application.ClientCache.CreateScope()
    Private _seafoodProductsView As ClientView(Of Product)
    Private _viewProducts As View(Of Object)

    Public Sub New()
        InitializeComponent()

        ' Define and bind a live view of expensive non-discontinued products ordered by price.
        _viewProducts =
            (From p In _scope.GetItems(Of Product)()
             Where Not p.Discontinued And p.UnitPrice >= 30
             Order By p.UnitPrice
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
             }).AsDynamic() ' AsDynamic() is required for data binding because an anonymous class is used (select new...)

        dataGrid1.ItemsSource = _viewProducts

        ' Define a view of seafood products. Filtering is performed on the server.
        _seafoodProductsView = _scope.GetItems(Of Product)().AsFiltered(Function(p) p.CategoryID.HasValue AndAlso p.CategoryID.Value = 8)

        ' Bind the label text to the number of products in the view
        textBlockCount.SetBinding(TextBlock.TextProperty, New Binding("Value") With {.Source = _viewProducts.LiveCount()})
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        ' Increase the price of the seafood products.
        ' The data grid contents will be updated automatically.
        For Each p In _seafoodProductsView
            p.UnitPrice *= 1.2
        Next
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button2.Click
        ' Decrease the price of the seafood products.
        ' The data grid contents will be updated automatically.
        For Each p In _seafoodProductsView
            p.UnitPrice /= 1.2
        Next
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button3.Click
        ' add a new product
        Dim newItem = CType(dataGrid1.ItemsSource, System.ComponentModel.IEditableCollectionView).AddNew()
        dataGrid1.ScrollIntoView(newItem)
    End Sub
End Class
