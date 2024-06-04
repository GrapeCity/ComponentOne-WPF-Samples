Imports System.ComponentModel
Imports C1.Data.Entities

' This file contains View Models used in the CategoryProductsView.

' The properties of the following two view models 
' must be virtual to make live view items modifiable.
Public Class CategoryViewModel
    Public Overridable Property CategoryID As Integer
    Public Overridable Property CategoryName As String
End Class


Public Class ProductViewModel
    Public Overridable Property ProductID As Integer
    Public Overridable Property ProductName As String
    Public Overridable Property CategoryID As Integer?
    Public Overridable Property CategoryName As String
    Public Overridable Property SupplierID As Integer?
    Public Overridable Property SupplierName As String
    Public Overridable Property UnitPrice As Decimal?
    Public Overridable Property QuantityPerUnit As String
    Public Overridable Property UnitsInStock As Short?
    Public Overridable Property UnitsOnOrder As Short?
End Class

Public Class CategoryProductsViewModel

    ' This client scope will be used to access the data.
    Private _scope As EntityClientScope

    ' Data grids in this view are bound to these data sources.
    Private _categories As ICollectionView
    Public Property Categories As ICollectionView
        Get
            Return _categories
        End Get
        Private Set(value As ICollectionView)
            _categories = value
        End Set
    End Property

    Private _products As ICollectionView
    Public Property Products As ICollectionView
        Get
            Return _products
        End Get
        Private Set(value As ICollectionView)
            _products = value
        End Set
    End Property

    Public Sub New()
        If Application.ClientCache Is Nothing Then
            Return
        End If

        _scope = Application.ClientCache.CreateScope()

        ' These live views will be used by the CategoryProductsView
        ' to display categories and products.
        Categories =
            From c In _scope.GetItems(Of Category)()
            Select New CategoryViewModel With
            {
                .CategoryID = c.CategoryID,
                .CategoryName = c.CategoryName
            }

        ' Products are filtered by CategoryID on the server.
        ' Filtering is performed automatically when the current Category changes.
        ' The product suppliers are fetched along with the products.
        Products =
            From p In _scope.GetItems(Of Product)().AsFilteredBound(Function(p) p.CategoryID).BindFilterKey(Categories, "CurrentItem.CategoryID").Include("Supplier")
            Select New ProductViewModel With
            {
                .ProductID = p.ProductID,
                .ProductName = p.ProductName,
                .CategoryID = p.CategoryID,
                .CategoryName = p.Category.CategoryName,
                .SupplierID = p.SupplierID,
                .SupplierName = p.Supplier.CompanyName,
                .UnitPrice = p.UnitPrice,
                .QuantityPerUnit = p.QuantityPerUnit,
                .UnitsInStock = p.UnitsInStock,
                .UnitsOnOrder = p.UnitsOnOrder
            }

    End Sub
End Class

