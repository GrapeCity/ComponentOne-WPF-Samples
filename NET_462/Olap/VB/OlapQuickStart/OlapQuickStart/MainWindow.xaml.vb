Imports System.Data
Imports System.Reflection
Imports C1.Zip

Class MainWindow

    Dim ds = New DataSet()
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' load data from embedded zip resource
        Dim asm = Assembly.GetExecutingAssembly()
        Using s = asm.GetManifestResourceStream("OlapQuickStart.nwind.zip")
            Dim zip = New C1ZipFile(s)
            Using zr = zip.Entries(0).OpenReader()
                ' load data
                ds.ReadXml(zr)
            End Using
        End Using


    End Sub

    Private Sub _c1OlapPage_Loaded(sender As Object, e As RoutedEventArgs)
        ' bind olap page to data
        _c1OlapPage.DataSource = ds.Tables(0).DefaultView

        ' show sales by country and category
        Dim olap = _c1OlapPage.OlapPanel.OlapEngine
        olap.DataSource = ds.Tables(0).DefaultView
        olap.BeginUpdate()
        olap.RowFields.Add("Country")
        olap.ColumnFields.Add("Category")
        olap.ValueFields.Add("Sales")
        olap.Fields("Sales").Format = "n0"
        olap.EndUpdate()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ' regenerate the olap view
        _c1OlapPage.OlapPanel.OlapEngine.Update()
    End Sub
End Class
