Imports Microsoft.Win32
Imports System.IO

Imports C1.WPF
Imports C1.WPF.Document
Imports C1.WPF.Document.Export

Class MainWindow
    Dim pdfDocumentSource As C1PdfDocumentSource = New C1PdfDocumentSource()
    Dim printDialog As PrintDialog = New PrintDialog() With {.UserPageRangeEnabled = True}
    Dim saveFileDialog As SaveFileDialog = New SaveFileDialog()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        fpFile.SelectedFile = New IO.FileInfo("..\..\DefaultDocument.pdf")

        cbAction.Items.Add(New C1ComboBoxItem() With {.Content = "Print..."})
        Dim supportedExportProviders = pdfDocumentSource.SupportedExportProviders
        For Each sep In supportedExportProviders
            cbAction.Items.Add(New C1ComboBoxItem() With {.Content = String.Format("Export to {0}...", sep.FormatName), .Tag = sep})
        Next
        cbAction.SelectedIndex = 0
    End Sub

    Private Sub DoPrint(pds As C1PdfDocumentSource)
        printDialog.MaxPage = pds.PageCount
        If (printDialog.ShowDialog() <> True) Then
            Return
        End If

        Try
            Dim po As C1PrintOptions = New C1PrintOptions()
            po.PrintQueue = printDialog.PrintQueue
            po.PrintTicket = printDialog.PrintTicket
            If printDialog.PageRangeSelection = PageRangeSelection.UserPages Then
                po.OutputRange = New OutputRange(printDialog.PageRange.PageFrom, printDialog.PageRange.PageTo)
            End If
            pds.Print(po)
            MessageBox.Show(Me, "Document was successfully printed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information)
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub DoExport(pds As C1PdfDocumentSource, ep As ExportProvider)
        saveFileDialog.DefaultExt = "." + ep.DefaultExtension
        saveFileDialog.FileName = Path.GetFileName(fpFile.SelectedFile.FullName) + "." + ep.DefaultExtension
        saveFileDialog.Filter = String.Format("{0} (*.{1})|*.{1}|All files (*.*)|*.*", ep.FormatName, ep.DefaultExtension)
        If (saveFileDialog.ShowDialog(Me) <> True) Then
            Return
        End If

        Try
            Dim exporter = ep.NewExporter()
            exporter.ShowOptions = False
            exporter.Preview = True
            exporter.FileName = saveFileDialog.FileName
            pds.Export(exporter)
            MessageBox.Show(Me, "Document was successfully exported.", "Information", MessageBoxButton.OK, MessageBoxImage.Information)
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnExecute_Click(sender As Object, e As RoutedEventArgs) Handles btnExecute.Click
        If (fpFile.SelectedFile Is Nothing) Then
            MessageBox.Show(Me, "Please select a PDF file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        ' load document
        While (True)
            Try
                pdfDocumentSource.LoadFromFile(fpFile.SelectedFile.FullName)
                Exit While
            Catch pex As PdfPasswordException
                Dim password As String = PasswordWindow.DoEnterPassword(fpFile.SelectedFile.FullName)
                If (password Is Nothing) Then
                    Return
                End If
                pdfDocumentSource.Credential.Password = password
            Catch ex As Exception
                MessageBox.Show(Me, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End Try
        End While

        ' execute action
        If (cbAction.SelectedIndex = 0) Then
            DoPrint(pdfDocumentSource)
        Else
            DoExport(pdfDocumentSource, (CType(cbAction.SelectedItem, C1ComboBoxItem).Tag))
        End If
    End Sub
End Class
