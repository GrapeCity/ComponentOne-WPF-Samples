Imports System.Net
Imports System.Windows.Media.Animation

#If (SILVERLIGHT) Then
Imports C1.Silverlight.Chart
#Else
Imports C1.WPF.C1Chart
#End If

Namespace ChartSamples
    <System.ComponentModel.Description("Shows bubble chart")> _
  Partial Public Class Bubble
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            chart.BeginUpdate()

            chart.Data.ItemNames = New String() {"Sydney - 2000", "Athens - 2004", "Beijing - 2008"}
            chart.View.AxisY.ItemsSource = New String() {"USA", "China", "Russia"}
            chart.View.AxisX.Min = -0.5
            chart.View.AxisX.Max = 2.5
            chart.View.AxisY.Min = -0.5
            chart.View.AxisY.Max = 2.5
            chart.EndUpdate()
        End Sub
    End Class

  Public Class ToolTipConverter
	  Implements IValueConverter
	Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
	  Dim dp As DataPoint = TryCast(value, DataPoint)

	  If dp IsNot Nothing Then
		Return dp("SizeValues").ToString() & " medals"
	  End If

	  Return value
	End Function

	Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
	  Throw New NotImplementedException()
	End Function
  End Class
End Namespace
