Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
Imports Palette = C1.WPF.C1Chart.ColorGeneration
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Gallery of chart types, themes and palettes")> _
  Partial Public Class Gallery
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            cbChartType.ItemsSource = Utils.GetEnumValues(GetType(ChartType))
            cbPalette.ItemsSource = Utils.GetEnumValues(GetType(Palette))

            cbTheme.SelectedIndex = 0
            cbChartType.SelectedItem = chart.ChartType.ToString()
            cbPalette.SelectedItem = chart.C1Chart_ColorGeneration.ToString()
        End Sub

        Private Sub cbTheme_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
#If (SILVERLIGHT) Then
	  If TypeOf cbTheme.SelectedItem Is String Then
		  chart.Theme = CType(System.Enum.Parse(GetType(ChartTheme), CStr(cbTheme.SelectedItem), False), ChartTheme)
	  End If
#Else
            chart.c1chart.Theme = TryCast((CType(cbTheme.SelectedItem, ListBoxItem)).Tag, ResourceDictionary)
#End If
        End Sub

        Private Sub cbPalette_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            If TypeOf cbPalette.SelectedItem Is String Then
                chart.C1Chart_ColorGeneration = CType(System.Enum.Parse(GetType(C1.WPF.C1Chart.ColorGeneration), CStr(cbPalette.SelectedItem), False), C1.WPF.C1Chart.ColorGeneration)
            End If
        End Sub

        Private Sub cbChartType_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            If TypeOf cbChartType.SelectedItem Is String Then
                Dim chartType As ChartType = CType(System.Enum.Parse(GetType(ChartType), CStr(cbChartType.SelectedItem), False), ChartType)
                chart.ChartType = chartType
            End If
        End Sub
    End Class

  Friend Class Utils
	Public Shared Function GetEnumValues(ByVal type As Type) As Object()
	  Dim list As New List(Of Object)()
	  Dim i As Integer=0
	  Do
		  Dim val As String = System.Enum.GetName(type, i)
		  If Not String.IsNullOrEmpty(val) Then
			list.Add(val)
		  Else
			Exit Do
		  End If
		  i += 1
	  Loop

	  Return list.ToArray()
	End Function
  End Class
End Namespace
