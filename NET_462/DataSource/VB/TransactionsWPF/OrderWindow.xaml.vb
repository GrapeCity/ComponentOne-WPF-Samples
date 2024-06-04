Imports C1.Data.Transactions
Imports C1.Data.Entities
Imports C1.WPF.LiveLinq

Public Class OrderWindow

    ' All changes in this form are made in the scope of this client transaction.
    Private _transaction As ClientTransaction

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(order As Order, transaction As ClientTransaction)
        InitializeComponent()
        _transaction = transaction

        ' Create a proxy for the order that automatically opens the transaction scope
        ' each time a value is assigned to a property.
        DataContext = transaction.ScopeDataContext(order)

        ' Define a live view of order details.
        Dim orderDetailsView = From od In order.Order_Details.AsLive()
                               Select New OrderDetailsInfo With
                               {
                                   .OrderID = od.OrderID,
                                   .ProductID = od.ProductID,
                                   .UnitPrice = od.UnitPrice,
                                   .Discount = od.Discount,
                                   .Quantity = od.Quantity
                               }
        ' All changes in this view are made in the transaction scope.
        orderDetailsView.SetTransaction(_transaction)
        orderDetailsGrid.ItemsSource = orderDetailsView
    End Sub

    Private Sub ok_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub cancel_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        DialogResult = False
    End Sub

End Class

' This class is used as the result item type of an order detail view defined above.
Public Class OrderDetailsInfo
    Public Overridable Property OrderID As Integer
    Public Overridable Property ProductID As Integer
    Public Overridable Property UnitPrice As Decimal
    Public Overridable Property Discount As Single
    Public Overridable Property Quantity As Decimal
End Class
