Imports System.Data
Imports C1.C1Zip
Imports System.Reflection

Class MainWindow

    Dim ds = New DataSet()
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' load Invoices data from embedded resource
        Dim asm = Assembly.GetExecutingAssembly()
        Using s = asm.GetManifestResourceStream("ConditionalFormatting.nwind.zip")
            Dim zip = New C1ZipFile(s)
            Using zr = zip.Entries(0).OpenReader()
                ' load data
                ds.ReadXml(zr)
            End Using
        End Using

    End Sub

    Private Sub _c1OlapPage_Loaded(sender As Object, e As RoutedEventArgs)
        ' prepare to build view 
        _c1OlapPage.DataSource = ds.Tables(0).DefaultView
        Dim olap = _c1OlapPage.OlapPanel.OlapEngine
        olap.BeginUpdate()

        ' show sales by country and category/product
        olap.ColumnFields.Add("Country")
        olap.RowFields.Add("Category", "Product")
        olap.Fields("Product").Width = 250
        Dim value = olap.Fields("Sales")
        olap.ValueFields.Add(value)
        value.Format = "n0"

        ' show values in the top 90% percentile in green
        Dim sh = value.StyleHigh
        sh.Value = 0.9
        sh.ConditionType = C1.Olap.ConditionType.Percentage
        sh.ForeColor = Color.FromArgb(&HFF, 0, 50, 0)
        sh.BackColor = Color.FromArgb(&HFF, 200, 250, 200)
        sh.FontBold = True

        ' show values in the bottom 2% percentile in red
        Dim sl = value.StyleLow
        sl.Value = 0.02
        sl.ConditionType = C1.Olap.ConditionType.Percentage
        sl.ForeColor = Color.FromArgb(&HFF, 50, 0, 0)
        sl.BackColor = Color.FromArgb(&HFF, 250, 200, 200)
        sl.FontBold = True

        ' view is ready
        olap.EndUpdate()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ' refresh olap output
        _c1OlapPage.OlapPanel.OlapEngine.Update()
    End Sub
End Class
