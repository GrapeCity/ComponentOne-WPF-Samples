Imports System.IO

Public Class PasswordWindow

    Public Function EnterPassword(fileName As String) As String
        lblFileName.Text = String.Format("'{0}' is protected.\r\nPlease enter a Document Open Password.", Path.GetFileName(fileName))
        tbPassword.Focus()
        If (ShowDialog() = True) Then
            Return tbPassword.Password
        End If
        Return Nothing
    End Function

    Public Shared Function DoEnterPassword(fileName As String) As String
        Dim f As PasswordWindow = New PasswordWindow()
        Return f.EnterPassword(fileName)
    End Function

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = True
    End Sub
End Class
