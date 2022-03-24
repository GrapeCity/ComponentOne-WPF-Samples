Imports C1.WPF.FlexGrid

Class MainWindow

    Private Sub MainWindow_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        ' populate bound grid
        Dim i As Integer
        Dim list As New List(Of Rect)
        For i = 1 To 100
            list.Add(New Rect(i, i, i, i))
        Next
        _fgBound.ItemsSource = list

        ' populate unbound grid
        For i = 1 To 10
            _fgUnbound.Rows.Add(New Row())
            _fgUnbound.Columns.Add(New Column())
        Next

        ' add some simple values to the cells
        _fgUnbound(0, 0) = "Hello World"
        _fgUnbound(1, 0) = 123.456
        _fgUnbound(2, 0) = New DateTime(2012, 10, 5)
        _fgUnbound(3, 0) = True
        _fgUnbound(4, 0) = False

        ' and add some controls too
        Dim chk As New CheckBox()
        ToolTipService.SetToolTip(chk, "This CheckBox is stored in a grid cell.")
        chk.VerticalAlignment = VerticalAlignment.Center
        chk.HorizontalAlignment = HorizontalAlignment.Center
        _fgUnbound(0, 1) = chk

        Dim cmb As New ComboBox()
        cmb.Items.Add("First")
        cmb.Items.Add("Second")
        cmb.Items.Add("Third")
        ToolTipService.SetToolTip(cmb, "This ComboBox is stored in a grid cell.")
        _fgUnbound(1, 1) = cmb

        ' connect custom cell factory to unbound grid
        ' (to show booleans as checkboxes)
        _fgUnbound.CellFactory = New MyCellFactory()

        ' prevent internal editor from taking over boolean and custom cells
        AddHandler Me._fgUnbound.BeginningEdit, New System.EventHandler(Of C1.WPF.FlexGrid.CellEditEventArgs)(AddressOf Me._fgUnbound_BeginningEdit)

    End Sub

    ' prevent internal editor from taking over boolean and custom cells
    Private Sub _fgUnbound_BeginningEdit(sender As System.Object, e As C1.WPF.FlexGrid.CellEditEventArgs)
        Dim ctl As Control = TryCast(_fgUnbound(e.Row, e.Column), Control)
        If (Not IsNothing(ctl)) Then
            e.Cancel = True
        End If
        If TypeOf _fgUnbound(e.Row, e.Column) Is Boolean Then
            e.Cancel = True
        End If
    End Sub
End Class
