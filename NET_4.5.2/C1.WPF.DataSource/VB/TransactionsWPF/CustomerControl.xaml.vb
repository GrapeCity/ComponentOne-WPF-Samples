Imports C1.Data
Imports C1.Data.Entities
Imports C1.Data.Transactions
Imports C1.WPF.LiveLinq


Public Class CustomerControl
    Private _mainForm As MainWindow
    Private _tab As TabItem
    ' The customer being edited.
    Public Customer As Customer
    ' This view contains the orders of the customer.
    Private _ordersView As C1.LiveLinq.LiveViews.View
    ' All changes in this control are made in the scope of this transaction.
    Private _transaction As ClientTransaction
    Private _cache As EntityClientCache

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(mainFrm As MainWindow, tab As TabItem, cust As Customer, clientCache As EntityClientCache)
        InitializeComponent()
        _mainForm = mainFrm
        _tab = tab
        _cache = clientCache
        Customer = cust

        ' Define and bind a live view of orders of the current customer.
        _ordersView = From o In Customer.Orders.AsLive()
                      Select New OrderInfo(o) With
                      {
                          .OrderID = o.OrderID,
                          .OrderDate = o.OrderDate,
                          .Freight = o.Freight,
                          .ShipName = o.ShipName,
                          .ShipCity = o.ShipCity
                      }
        CreateTransaction()
        ordersGrid.ItemsSource = _ordersView
    End Sub

    Private Sub transaction_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs)
        ' The Undo button is enabled if and only if there are changes made in the transaction scope.
        buttonUndo.IsEnabled = _transaction.HasChanges
    End Sub

    Private Sub CreateTransaction()
        ' Initialize the client transaction.
        ' Specify that all changes to _ordersView are automatically considered changes made in the scope of this transaction.
        _transaction = _cache.CreateTransaction()
        _ordersView.SetTransaction(_transaction)

        ' Enable/disable the Undo button when the _transaction.HasChanges property value changes.
        AddHandler _transaction.PropertyChanged, AddressOf transaction_PropertyChanged
        buttonUndo.IsEnabled = False
    End Sub

    Private Sub close_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If _transaction.HasChanges Then
            ' Ask the user whether they will save or cancel the changes made by this control.
            Select Case MessageBox.Show("Do you want to save changes?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question)
                Case MessageBoxResult.Yes
                    _transaction.Commit()
                Case MessageBoxResult.No
                    _transaction.Rollback()
                Case MessageBoxResult.Cancel
                    Exit Sub
            End Select
        End If
        ' Tell MainForm to remove the tab containing this control.
        _mainForm.CloseRequested(_tab)
    End Sub

    Private Sub undo_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        ' Cancel the changes made by this control
        ' by rolling back the _transaction.
        RemoveHandler _transaction.PropertyChanged, AddressOf transaction_PropertyChanged
        _transaction.Rollback()
        ' Create a new transaction.
        CreateTransaction()
    End Sub

    Private Sub addOrder(sender As System.Object, e As System.Windows.RoutedEventArgs)
        ' Add a new Order in a child transaction scope.
        ' Rolling back the child transaction will not roll back the parent transaction.
        Using tran2 As New ClientTransaction(_transaction)
            Dim order As Order = New Order()
            Using scope As Object = tran2.Scope()
                _cache.ObjectContext.AddObject("Orders", order)
            End Using
            Dim window As OrderWindow = New OrderWindow(order, tran2)
            If window.ShowDialog() = True Then
                ' Add the order to the Customer.Orders in the child transaction scope and commit the transaction.
                Using scope As Object = tran2.Scope()
                    Customer.Orders.Add(order)
                    tran2.Commit()
                End Using
            End If
        End Using ' The child transaction will be rolled back automatically if it is not committed.
    End Sub

    Private Sub editOrder(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not (TypeOf ordersGrid.SelectedItem Is OrderInfo) Then
            Exit Sub
        End If
        Dim order As OrderInfo = CType(ordersGrid.SelectedItem, OrderInfo)
        ' Edit the order in a child transaction scope.
        ' Rolling back the child transaction will not roll back the parent transaction.
        Using tran2 As New ClientTransaction(_transaction)
            Dim window As OrderWindow = New OrderWindow(order.GetOrder(), tran2)
            If window.ShowDialog() = True Then
                tran2.Commit()
            End If
        End Using ' The child transaction will be rolled back automatically if it is not committed.
    End Sub

    Private Sub deleteOrder(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not (TypeOf ordersGrid.SelectedItem Is OrderInfo) Then
            Exit Sub
        End If
        Dim order As Order = CType(ordersGrid.SelectedItem, OrderInfo).GetOrder()
        ' Remove the selected order.
        Using scope As Object = _transaction.Scope()
            ' Make the change in the transaction scope.
            Customer.Orders.Remove(order)
        End Using
    End Sub

    Private Sub orderDoubleClick(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        ' Start editing the order on double click.
        editOrder(sender, e)
    End Sub
End Class


' This class is used as the result item type of an order view defined above.
Public Class OrderInfo
    Private _order As Order

    Public Sub New(o As Order)
        _order = o
    End Sub

    Public Function GetOrder() As Order
        Return _order
    End Function

    Public Overridable Property OrderID As Integer
    Public Overridable Property OrderDate As DateTime?
    Public Overridable Property Freight As Decimal?
    Public Overridable Property ShipName As String
    Public Overridable Property ShipCity As String
End Class
