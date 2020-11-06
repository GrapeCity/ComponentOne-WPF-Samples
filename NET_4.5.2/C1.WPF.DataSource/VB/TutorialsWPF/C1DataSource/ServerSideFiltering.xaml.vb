Public Class ServerSideFiltering

    Private Sub comboBox1_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles comboBox1.SelectionChanged
        ' Update the filter value. It will trigger automatic loading.
        c1DataSource1.ViewSources("Products").FilterDescriptors(0).Value = CType(comboBox1.SelectedValue, Category).CategoryID
    End Sub

    Private Sub button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles button1.Click
        c1DataSource1.ClientCache.SaveChanges()
    End Sub
End Class