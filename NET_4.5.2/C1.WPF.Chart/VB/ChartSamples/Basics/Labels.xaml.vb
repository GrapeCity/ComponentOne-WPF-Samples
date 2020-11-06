Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Labels and tooltips for data points")> _
  Partial Public Class Labels
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            AddHandler Loaded, AddressOf Labels_Loaded

            c1chart.BeginUpdate()
            c1chart.View.AxisY.Min = 0
            c1chart.View.AxisY.Max = 10

            cbChartType.ItemsSource = New String() {"Column", "LineSymbols", "Pie"}
            cbChartType.SelectedItem = "Column"
            c1chart.EndUpdate()
        End Sub

        Private Sub Labels_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' seems radiobutton does not use parent Foreground
            rbLabels.Foreground = Foreground
            rbTooltips.Foreground = Foreground
        End Sub

        Private Sub rbLabel_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If c1chart Is Nothing Then
                Return
            End If

            Dim lbl As DataTemplate = Nothing
            Dim tt As DataTemplate = Nothing
            If sender Is rbLabels Then
                lbl = CType(Resources("lbl"), DataTemplate)
            ElseIf sender Is rbTooltips Then
                tt = CType(Resources("lbl"), DataTemplate)
            End If

            c1chart.BeginUpdate()
            For Each ds As DataSeries In c1chart.Data.Children
                ds.PointLabelTemplate = lbl
                ds.PointTooltipTemplate = tt
            Next ds
            c1chart.EndUpdate()
        End Sub

        Private Sub cbChartType_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            If TypeOf cbChartType.SelectedItem Is String Then
                c1chart.ChartType = CType(System.Enum.Parse(GetType(ChartType), CStr(cbChartType.SelectedItem), False), ChartType)
            End If
        End Sub
    End Class
End Namespace
