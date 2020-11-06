Imports System.Text
Imports System.ComponentModel
Imports C1.WPF.FlexGrid

Namespace Printing
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private _data As ICollectionView = Product.GetProducts(100)

		Public Sub New()
			InitializeComponent()

			' populate grid
			_flex.ItemsSource = _data
			_flex.Columns("Line").AllowMerging = True
			_flex.Columns("Color").AllowMerging = True

			' for testing
			_flex.AllowResizing = AllowResizing.Both
		End Sub

		#Region "** event handlers"

		' basic printing
		Private Sub _btnBasicPrint_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' get margins, scale mode
			Dim margin = If(_cmbMargins.SelectedIndex = 0, 96.0 / 4, If(_cmbMargins.SelectedIndex = 1, 96.0 / 2, 96.0))
            Dim sm = If(_cmbZoom.SelectedIndex = 0, ScaleMode.ActualSize, If(_cmbZoom.SelectedIndex = 1, ScaleMode.PageWidth, ScaleMode.SinglePage))
            Dim showPrintDialog = _chkShowPrintDialog.IsChecked.Value

            If (_cmbOrientation.SelectedIndex > 0) Or showPrintDialog Then
                ' setup advanced print parameters according to user selection
                Dim pp As PrintParameters
                pp = New PrintParameters()
                pp.Margin = New Thickness(margin)
                pp.ScaleMode = sm
                pp.MaxPages = 20
                pp.ShowPrintDialog = showPrintDialog
                If (_cmbOrientation.SelectedIndex > 0) Then

                    pp.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue()
                    pp.PrintTicket = pp.PrintQueue.DefaultPrintTicket
                    pp.PrintTicket.PageOrientation = CType(IIf((_cmbOrientation.SelectedIndex = 1),
                        System.Printing.PageOrientation.Portrait, System.Printing.PageOrientation.Landscape), System.Printing.PageOrientation?)
                End If
                pp.DocumentName = "C1FlexGrid printing example"
                ' print the grid
                _flex.Print(pp)
                Else

                    ' print the grid
                    _flex.Print("C1FlexGrid printing example", sm, New Thickness(margin), 20)
                End If
        End Sub

		' advanced printing
		Private Sub _btnAdvancedPrint_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' get margins, scale mode
            Dim margin = If(_cmbMargins.SelectedIndex = 0, 96.0 / 4, If(_cmbMargins.SelectedIndex = 1, 96.0 / 2, 96.0))
            Dim sm = If(_cmbZoom.SelectedIndex = 0, ScaleMode.ActualSize, If(_cmbZoom.SelectedIndex = 1, ScaleMode.PageWidth, ScaleMode.SinglePage))

            Dim pd = New PrintDialog()
            If (_cmbOrientation.SelectedIndex > 0) Then

                pd.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue()
                pd.PrintTicket = pd.PrintQueue.DefaultPrintTicket
                pd.PrintTicket.PageOrientation = CType(IIf((_cmbOrientation.SelectedIndex = 1),
                    System.Printing.PageOrientation.Portrait, System.Printing.PageOrientation.Landscape), System.Printing.PageOrientation?)
            End If

            Dim showPrintDialog = _chkShowPrintDialog.IsChecked.Value

            If Not showPrintDialog Or pd.ShowDialog().Value Then

                ' calculate page size
                Dim sz = New Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight)

                ' create paginator
                Dim paginator = New FlexPaginator(_flex, sm, sz, New Thickness(margin), 100)
                pd.PrintDocument(paginator, "C1FlexGrid printing example")
            End If
        End Sub

		' toggle grouping
		Private Sub _chkGroup_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Using _data.DeferRefresh()
				Dim gd = _data.GroupDescriptions
				gd.Clear()
				If _chkGroup.IsChecked.Value Then
					_data.GroupDescriptions.Add(New PropertyGroupDescription("Line"))
					_data.GroupDescriptions.Add(New PropertyGroupDescription("Color"))
				End If
			End Using
		End Sub

		' toggle merging
		Private Sub _chkMerge_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_flex.AllowMerging = If(_chkMerge.IsChecked.Value, AllowMerging.Cells, AllowMerging.None)
		End Sub

		' toggle freezing
		Private Sub _chkFreeze_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If _chkFreeze.IsChecked.Value Then
				_flex.Rows.Frozen = Math.Min(12, _flex.Selection.Row)
				_flex.Columns.Frozen = Math.Min(2, _flex.Selection.Column)

			Else
				_flex.Rows.Frozen = 0
				_flex.Columns.Frozen = 0
			End If
		End Sub
#End Region

#Region "** printing"

        ''' <summary>
        ''' <see cref="DocumentPaginator"/> class used to render <see cref="C1FlexGrid"/> controls.
        ''' </summary>
        ''' <remarks>
        ''' <para>This class is based on the following article:</para>
        ''' <para>http://www.switchonthecode.com/tutorials/wpf-printing-part-2-pagination</para>
        ''' </remarks>
        Public Class FlexPaginator
            Inherits DocumentPaginator
            Private _margin As Thickness
            Private _pageSize As Size
            Private _contentSize As Size
            Private _contentLocation As Point
            Private _scaleMode As ScaleMode
            Private _pages As List(Of FrameworkElement)

            Public Sub New(ByVal flex As C1FlexGrid, ByVal scaleMode_Renamed As ScaleMode, ByVal pageSize As Size, ByVal margin As Thickness, ByVal maxPages As Integer)
                ' save parameters
                _margin = margin
                _scaleMode = scaleMode_Renamed
                _pageSize = pageSize

                ' adjust page size for margins before building grid images
                _contentSize = New Size(pageSize.Width - margin.Left - margin.Right, pageSize.Height - margin.Top - margin.Bottom)
                _contentLocation = New Point(margin.Left, margin.Top)

                ' get grid images for each page
                _pages = flex.GetPageImages(scaleMode_Renamed, pageSize, maxPages)
            End Sub
            Public Overrides Function GetPage(ByVal pageNumber As Integer) As DocumentPage
                ' create page element
                Dim pageTemplate_Renamed = New PageTemplate()

                ' set margins
                pageTemplate_Renamed.SetPageMargin(_margin)

                ' set content
                pageTemplate_Renamed.PageContent.Child = _pages(pageNumber)
                pageTemplate_Renamed.PageContent.Stretch = If(_scaleMode = ScaleMode.ActualSize, Stretch.None, Stretch.Uniform)

                ' set footer text
                pageTemplate_Renamed.FooterRight.Text = String.Format("Page {0} of {1}", pageNumber + 1, _pages.Count)

                ' arrange the elements on the page
                pageTemplate_Renamed.Arrange(New Rect(0, 0, _pageSize.Width, _pageSize.Height))

                ' return new document page
                Return New DocumentPage(pageTemplate_Renamed, _pageSize,
                   New Rect(_contentLocation, _contentSize), New Rect(_contentLocation, _contentSize))
            End Function
            Public Overrides ReadOnly Property PageCount() As Integer
                Get
                    Return _pages.Count
                End Get
            End Property
            Public Overrides ReadOnly Property Source() As IDocumentPaginatorSource
                Get
                    Return Nothing
                End Get
            End Property
            Public Overrides Property PageSize() As Size
                Get
                    Return _pageSize
                End Get
                Set(ByVal value As Size)
                    Throw New NotImplementedException()
                End Set
            End Property
            Public Overrides ReadOnly Property IsPageCountValid() As Boolean
                Get
                    Return True
                End Get
            End Property
        End Class

#End Region
    End Class
End Namespace
