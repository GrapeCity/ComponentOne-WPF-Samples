Imports C1.WPF.Olap
Imports System.Data
Imports System.Reflection
Imports C1.Zip

Class MainWindow
    Dim _c1OlapPage As New C1OlapPage()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        LayoutRoot.Children.Add(_c1OlapPage)

    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ' load data from embedded zip resource
        Dim ds = New DataSet()
        Dim asm = Assembly.GetExecutingAssembly()
        Using s = asm.GetManifestResourceStream("CustomColumns.nwind.zip")
            Dim zip = New C1ZipFile(s)
            Using zr = zip.Entries(0).OpenReader()
                ' load data
                ds.ReadXml(zr)
            End Using
        End Using

        ' show sales by country and category
        Dim olap = _c1OlapPage.OlapPanel.OlapEngine
        olap.BeginUpdate()
        AddHandler olap.Updated, AddressOf olap_Updated
        olap.ValueFields.MaxItems = 3
        olap.DataSource = ds.Tables(0).DefaultView
        olap.RowFields.Add("Country")
        olap.ColumnFields.Add("Category")
        olap.ValueFields.Add("Sales")
        olap.Fields("Sales").Format = "n0"
        olap.EndUpdate()

    End Sub

    Private Sub olap_Updated(sender As Object, e As EventArgs)
        ' add a new calculated column to the output table
        Dim olap = _c1OlapPage.OlapEngine
        Dim dt = olap.OlapTable
        If dt.Columns.Count >= 2 Then
            dt.Columns.Add("Diff", GetType(Double), String.Format("[{0}] - [{1}]", dt.Columns(1).ColumnName, dt.Columns(0).ColumnName))
            dt.Columns.Add("Prod", GetType(Double), String.Format("[{0}] * [{1}]", dt.Columns(1).ColumnName, dt.Columns(0).ColumnName))

            ' format the new columns on the grid
            Dim cols = _c1OlapPage.OlapGrid.Columns
            For i As Integer = 0 To 1
                Dim col = cols(cols.Count - 1 - i)
                col.Format = "n2"
                col.HeaderHorizontalAlignment = HorizontalAlignment.Center
            Next
        End If
    End Sub

End Class
