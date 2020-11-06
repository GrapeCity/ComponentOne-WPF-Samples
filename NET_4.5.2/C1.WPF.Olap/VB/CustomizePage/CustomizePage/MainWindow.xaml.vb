Imports System.Data
Imports System.Reflection
Imports C1.C1Zip
Imports System.Xml
Imports C1.WPF

Class MainWindow
    Const VIEWDEF_KEY As String = "C1OlapViewDefinition"
    Dim collapseAllView As C1MenuItem = New C1MenuItem()

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ' load data from embedded zip resource
        Dim ds = New DataSet()
        Dim asm = Assembly.GetExecutingAssembly()
        Using s = asm.GetManifestResourceStream("CustomizePage.nwind.zip")
            Dim zip = New C1ZipFile(s)
            Using zr = zip.Entries(0).OpenReader()
                ' load data
                ds.ReadXml(zr)
            End Using
        End Using

        ' bind olap page to data
        _c1OlapPage.DataSource = ds.Tables(0).DefaultView

        ' view not found in storage, use default
        Dim olap = _c1OlapPage.OlapPanel.OlapEngine
        olap.DataSource = ds.Tables(0).DefaultView
        olap.BeginUpdate()
        olap.RowFields.Add("Country")
        olap.ColumnFields.Add("Category")
        olap.ValueFields.Add("Sales")
        olap.Fields("Sales").Format = "n0"
        olap.EndUpdate()

        ' get predefined views from XML resource
        Dim views = New Dictionary(Of String, String)()
        Using s = asm.GetManifestResourceStream("CustomizePage.OlapViews.xml")
            Using reader = XmlReader.Create(s)
                ' read predefined view definitions
                While reader.Read()
                    If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "C1OlapPage" Then
                        Dim id = reader.GetAttribute("id")
                        Dim def = reader.ReadOuterXml()
                        views(id) = def
                    End If
                End While
            End Using
        End Using

        ' build new menu with predefined views
        Dim menuViews = New C1MenuItem()
        menuViews.Header = "View"
        menuViews.Icon = GetImage("Resources/views.png")
        menuViews.VerticalAlignment = VerticalAlignment.Center
        ToolTipService.SetToolTip(menuViews, "Select a predefined Olap view.")
        For Each id As Object In views.Keys
            Dim mi = New C1MenuItem()
            mi.Header = id
            mi.Tag = views(id)
            AddHandler mi.Click, AddressOf mi_Click
            menuViews.Items.Add(mi)
        Next

        ' add new menu to the page's main menu
        _c1OlapPage.MainMenu.Items.Insert(6, menuViews)

        AddHandler _c1OlapPage.Updated, AddressOf c1OlapPage1_Updated

        ' add collapseall menu to page's main menu
        collapseAllView.Header = "CollapseAll"
        collapseAllView.Icon = GetImage("Resources/collapseAll.png")
        collapseAllView.VerticalAlignment = VerticalAlignment.Center
        ToolTipService.SetToolTip(collapseAllView, "Collapse all the subtotals rows and columns.")
        AddHandler collapseAllView.Click, AddressOf collapseAllView_Click
        _c1OlapPage.MainMenu.Items.Insert(11, collapseAllView)

    End Sub

    Private Sub collapseAllView_Click(ByVal sender As Object, ByVal e As SourcedEventArgs)
        _c1OlapPage.OlapGrid.CollapseAllCols()
        _c1OlapPage.OlapGrid.CollapseAllRows()
    End Sub

    ' apply a predefined view
    Private Sub mi_Click(sender As Object, e As SourcedEventArgs)
        Dim mi = TryCast(sender, C1MenuItem)
        Dim viewDef = TryCast(mi.Tag, String)
        _c1OlapPage.ViewDefinition = viewDef
    End Sub

    Private Sub c1OlapPage1_Updated(ByVal sender As Object, ByVal e As EventArgs)
        ' update button status of collapseAllView.
        If (_c1OlapPage.ShowTotalsColumns = C1.Olap.ShowTotals.Subtotals Or _c1OlapPage.ShowTotalsRows = C1.Olap.ShowTotals.Subtotals) Then
            collapseAllView.IsEnabled = True
        Else
            collapseAllView.IsEnabled = False
        End If
    End Sub

    ' utility to load an image from a URI
    Private Shared Function GetImage(name As String) As Image
        Dim uri = New Uri(name, UriKind.Relative)
        Dim img = New Image()
        img.Source = New BitmapImage(uri)
        img.Stretch = Stretch.None
        img.VerticalAlignment = VerticalAlignment.Center
        img.HorizontalAlignment = HorizontalAlignment.Center
        Return img
    End Function

    ' regenerate the olap view
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        _c1OlapPage.OlapPanel.OlapEngine.Update()
    End Sub
End Class
