Imports System.IO
Imports Microsoft.Win32
Imports C1.WPF.Document

Class MainWindow
    Dim pdfDocumentSource As C1PdfDocumentSource = New C1PdfDocumentSource()
    Dim openFileDialog As OpenFileDialog = New OpenFileDialog() With
        {
        .DefaultExt = "pdf",
        .Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*"
        }

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler fv.OpenAction, AddressOf FlexViewer_OpenAction
        AddHandler fv.CloseAction, AddressOf FlexViewer_CloseAction
        AddHandler Me.Loaded, AddressOf MainWindow_Loaded

        fv.DocumentSource = pdfDocumentSource
        If File.Exists("..\..\DefaultDocument.pdf") Then
            pdfDocumentSource.LoadFromFile("..\..\DefaultDocument.pdf")
        Else
            fv.CloseActionEnabled = False
        End If

    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs)
        fv.FocusPane()
    End Sub

    Private Sub FlexViewer_OpenAction(sender As Object, e As EventArgs)
        fv.FocusPane()

        If Not openFileDialog.ShowDialog(Me) Then
            Return
        End If

        While (True)
            Try
                pdfDocumentSource.LoadFromFile(openFileDialog.FileName)
                fv.CloseActionEnabled = True
                Return
            Catch pex As PdfPasswordException
                Dim password As String = PasswordWindow.DoEnterPassword(openFileDialog.FileName)
                If (password Is Nothing) Then
                    Return
                End If
                pdfDocumentSource.Credential.Password = password
            Catch ex As Exception
                MessageBox.Show(Me, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End Try
        End While
    End Sub

    Private Sub FlexViewer_CloseAction(sender As Object, e As EventArgs)
        pdfDocumentSource.ClearContent()
        fv.CloseActionEnabled = False
        fv.FocusPane()
    End Sub

End Class
