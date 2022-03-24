Imports C1.Data
Imports C1.Data.Entities
Imports C1.LiveLinq.LiveViews

Public Class MainWindow
    ' A client cache and a client scope that are used to load the data.
    Private _clientCache As EntityClientCache
    Private _scope As EntityClientScope

    Public Sub New()
        InitializeComponent()

        ' Initialize the client cache and the client scope.
        _clientCache = New EntityClientCache(New NORTHWNDEntities())
        _scope = New EntityClientScope(_clientCache)

        ' Bind a combo box to the list of cities
        ' using a live view of customers grouped by city.
        comboCity.DisplayMemberPath = "City"
        comboCity.ItemsSource =
            (From c In _scope.GetItems(Of Customer)()
             Group c By Key = c.City Into g = Group
             Order By Key
             Select New With
             {
                 .City = Key,
                 .Customers = g
             }).AsDynamic()
    End Sub

    Private Sub EditOrdersForCustomer()
        If Not (TypeOf listCustomers.SelectedItem Is Customer) Then
            Exit Sub
        End If
        ' Start editing the selected customer.
        Dim customer As Customer = CType(listCustomers.SelectedItem, Customer)

        ' If the customer is already being edited, then select the tab associated with that customer.
        For Each tabItem As TabItem In tabs.Items
            If tabItem.Header = customer.ContactName Then
                tabItem.IsSelected = True
                Exit Sub
            End If
        Next

        ' Create a CustomerControl for the current customer
        ' and display it in the tab control.
        Dim tab As TabItem = New TabItem()
        Dim customerControl As CustomerControl = New CustomerControl(Me, tab, customer, _clientCache)
        tab.Header = customer.ContactName
        tab.Content = customerControl
        tabs.Items.Add(tab)
        tab.IsSelected = True
    End Sub

    Public Sub CloseRequested(tab As TabItem)
        ' Remove the tab when its Close button is pressed.
        tabs.Items.Remove(tab)
    End Sub

    Private Sub listCustomers_MouseDoubleClick(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        ' Start editing the selected customer on double click.
        EditOrdersForCustomer()
    End Sub

End Class

