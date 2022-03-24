Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Drag'n'drop data series")> _
  Partial Public Class DragDrop
        Inherits UserControl
        Private rnd As New Random()

        ' list of series is common for both charts
        Private list1 As List(Of DataSeries)

        Public Sub New()
            InitializeComponent()

            ' create test data
            Dim nser As Integer = 8, npts As Integer = 8
            list1 = CreateTestData(nser, npts)
            For Each ds As DataSeries In list1
                AddHandler ds.PlotElementLoaded, AddressOf ds_Loaded
                chart1.Data.Children.Add(ds)
            Next ds

#If (SILVERLIGHT) Then
	  RegisterDragDropSL()
#Else
            RegisterDragDropWPF()
#End If

            CheckNoData()
        End Sub

#If (SILVERLIGHT) Then
	Private _ddManager As C1DragDropManager

	Private Sub RegisterDragDropSL()
	  ' create and setup drag and drop manager
	  _ddManager = New C1DragDropManager()
	  _ddManager.RegisterDropTarget(chart1, True)
	  _ddManager.RegisterDropTarget(chart2, True)

	  AddHandler _ddManager.DragDrop, Sub(s, e)
		Dim dser As DataSeries = (CType(e.DragSource, PlotElement)).DataPoint.Series

		If e.DropTarget = chart1 Then
		  If chart2.Data.Children.Contains(dser) Then
			_ddManager.ClearSources() ' clean up old plot elements
			chart2.Data.Children.Remove(dser)
			chart1.Data.Children.Add(dser)
		  End If
		ElseIf e.DropTarget = chart2 Then
		  If chart1.Data.Children.Contains(dser) Then
			_ddManager.ClearSources() ' clean up old plot elements
			chart1.Data.Children.Remove(dser)
			chart2.Data.Children.Add(dser)
		  End If
		End If
		CheckNoData()
	  End Sub
	End Sub

#Else
        Private Sub RegisterDragDropWPF()
            chart1.AllowDrop = True
            AddHandler chart1.Drop, Function(s, e) AnonymousMethod2(s, e)
            

            chart2.AllowDrop = True
            AddHandler chart2.Drop, Function(s, e) AnonymousMethod3(s, e)
            
        End Sub

        Private Function AnonymousMethod2(ByVal s As Object, ByVal e As Object) As Object
            Dim lbl As String = CStr(e.Data.GetData(DataFormats.StringFormat))
            Dim dser As DataSeries = list1.FirstOrDefault(Function(ds) ds.Label = lbl)
            If chart2.Data.Children.Contains(dser) Then
                chart2.Data.Children.Remove(dser)
                chart1.Data.Children.Add(dser)
            End If
            CheckNoData()
            Return Nothing
        End Function

        Private Function AnonymousMethod3(ByVal s As Object, ByVal e As Object) As Object
            Dim lbl As String = CStr(e.Data.GetData(DataFormats.StringFormat))
            Dim dser As DataSeries = list1.FirstOrDefault(Function(ds) ds.Label = lbl)
            If chart1.Data.Children.Contains(dser) Then
                chart1.Data.Children.Remove(dser)
                chart2.Data.Children.Add(dser)
            End If
            CheckNoData()
            Return Nothing
        End Function
#End If

        ''' <summary>
        ''' Fires when plot element was loaded.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ds_Loaded(ByVal sender As Object, ByVal e As EventArgs)
            Dim pe As PlotElement = CType(sender, PlotElement)
            Dim dss As DataSeries = pe.DataPoint.Series

            ' "fix" the series color to avoid automatic palette
            If TypeOf pe Is Lines Then
                dss.ConnectionFill = pe.Fill
                dss.ConnectionStroke = pe.Stroke
            Else
                dss.SymbolFill = pe.Fill
                dss.SymbolStroke = pe.Stroke
            End If

#If (SILVERLIGHT) Then
	  ' register as data source
	  _ddManager.RegisterDragSource(pe, DragDropEffect.Move, ModifierKeys.None)
#Else
            AddHandler pe.MouseDown, Function(se, ev) AnonymousMethod4(se, ev, pe)
#End If
        End Sub

        Private Function AnonymousMethod4(ByVal se As Object, ByVal ev As Object, ByVal pe As PlotElement) As Object
            System.Windows.DragDrop.DoDragDrop(pe, pe.DataPoint.Series.Label, DragDropEffects.Move)
            Return Nothing
        End Function

        ''' <summary>
        ''' show or hide "no data" message
        ''' </summary>
        Private Sub CheckNoData()
            If chart1.Data.Children.Count = 0 Then
                nodata.Visibility = Visibility.Visible
                Grid.SetColumn(nodata, 0)
            ElseIf chart2.Data.Children.Count = 0 Then
                nodata.Visibility = Visibility.Visible
                Grid.SetColumn(nodata, 1)
            Else
                nodata.Visibility = Visibility.Collapsed
            End If
        End Sub

        ''' <summary>
        ''' Creates specified number of data series with test data.
        ''' </summary>
        ''' <param name="ns">Number of data series.</param>
        ''' <param name="npts">Number of points.</param>
        ''' <returns>List of data series.</returns>
        Private Function CreateTestData(ByVal ns As Integer, ByVal npts As Integer) As List(Of DataSeries)
            Dim list As New List(Of DataSeries)()
            For iser As Integer = 0 To ns - 1
                Dim vals(npts - 1) As Double
                For i As Integer = 0 To npts - 1
                    vals(i) = rnd.Next(1, 100)
                Next i
                list.Add(New DataSeries() With {.ValuesSource = vals, .Label = "s" & iser.ToString()})
            Next iser
            Return list
        End Function
    End Class
End Namespace
