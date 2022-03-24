Imports C1.WPF.Toolbar

Class MainWindow

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        For Each key In Resources.Keys
            Dim cmd = TryCast(Resources(key), C1ToolbarCommand)
            If cmd IsNot Nothing Then
                CommandManager.RegisterClassCommandBinding([GetType](), New CommandBinding(cmd, Sub(s, e) Execute(cmd.LabelTitle), Sub(s, e) e.CanExecute = True))
            End If
        Next

    End Sub
    Private Sub C1Toolbar_HelpClick(sender As System.Object, e As System.EventArgs)
        Execute("Help!")
    End Sub
    Private Sub Execute(text As String)
        tb.Text += text & vbLf
        tb.SelectionStart = tb.Text.Length
    End Sub


End Class
