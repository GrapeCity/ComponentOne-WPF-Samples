Imports C1.Data.DataSource

Public Class Paging
    Private _view As ClientCollectionView

    Public Sub New()
        InitializeComponent()

        ' Update the displayed page index and count 
        ' when they change in C1DataSource.
        
        _view = C1DataSource1("Orders")

        RefreshPageInfo()

        AddHandler _view.PropertyChanged, AddressOf RefreshPageInfo

    End Sub

    Private Sub RefreshPageInfo()
        pageInfo.Text = String.Format("{0} / {1}", _view.PageIndex + 1, _view.PageCount)
    End Sub

    Private Sub btnPrevPage_Click(sender As System.Object, e As System.EventArgs) Handles btnPrevPage.Click
        _view.MoveToPreviousPage()
    End Sub

    Private Sub btnNextPage_Click(sender As System.Object, e As System.EventArgs) Handles btnNextPage.Click
        _view.MoveToNextPage()
    End Sub
End Class