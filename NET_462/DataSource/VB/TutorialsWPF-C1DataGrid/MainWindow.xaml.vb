Class MainWindow 

    Private Sub showSimpleBinding(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As SimpleBinding = New SimpleBinding
        form.Show()
    End Sub

    Private Sub showServerSideFiltering(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As ServerSideFiltering = New ServerSideFiltering
        form.Show()
    End Sub

    Private Sub showMasterDetailBinding(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As MasterDetailBinding = New MasterDetailBinding
        form.Show()
    End Sub

    Private Sub showPaging(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As Paging = New Paging
        form.Show()
    End Sub

    Private Sub showLargeDataSet(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As LargeDataset = New LargeDataset
        form.Show()
    End Sub

    Private Sub showCustomColumns(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As CustomColumns = New CustomColumns
        form.Show()
    End Sub

    Private Sub showDataSourcesInCode(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As DataSourcesInCode = New DataSourcesInCode
        form.Show()
    End Sub

    Private Sub showClientSideQuerying(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As ClientSideQuerying = New ClientSideQuerying
        form.Show()
    End Sub

    Private Sub showCategoryProductsView(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim form As CategoryProductsView = New CategoryProductsView
        form.Show()
    End Sub
End Class
