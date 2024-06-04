Imports C1.Data.Entities
Imports C1.Data

Public Class DataSourcesInCode

    ' This client scope will be used to load the data.
    Private _scope As EntityClientScope = Application.ClientCache.CreateScope()


    Public Sub New()
        InitializeComponent()

        ' Bind the category combo box to the category view
        ' and show the products of the first category in the data grid.

        Dim viewCategories As ClientView(Of Category) = _scope.GetItems(Of Category)()

        comboBox1.DisplayMemberPath = "CategoryName"
        comboBox1.ItemsSource = viewCategories

        BindGrid(viewCategories.First().CategoryID)
    End Sub

    Private Sub BindGrid(categoryID As Integer)
        ' Filter products by CategoryID on the server
        ' and bind them to the data grid.
        dataGrid1.ItemsSource =
                (From p In _scope.GetItems(Of Product)().AsFiltered(Function(p As Product) p.CategoryID.HasValue AndAlso p.CategoryID.Value = categoryID)
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
    End Sub


    Private Sub comboBox1_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles comboBox1.SelectionChanged
        ' Rebind the data grid, display the products of the selected category.
        If comboBox1.SelectedValue IsNot Nothing Then
            BindGrid(CType(comboBox1.SelectedValue, Category).CategoryID)
        End If
    End Sub

    Private Sub btnSaveChanges_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnSaveChanges.Click
        Application.ClientCache.SaveChanges()
    End Sub

End Class