Imports System.ComponentModel
Imports System.Text

Namespace ChartSamples
  ''' <summary>
  ''' Interaction logic for Window1.xaml
  ''' </summary>
  Partial Public Class Window1
	  Inherits Window
	Public Sub New()
	  InitializeComponent()

	  AddHandler Loaded, AddressOf Window1_Loaded
	End Sub

	Private Sub Window1_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
	  CType(tv.Items(0), TreeViewItem).IsSelected = True
	End Sub

	Private Sub tv_SelectedItemChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Object))
	  Dim tvi As TreeViewItem = TryCast(tv.SelectedItem, TreeViewItem)
	  If tvi IsNot Nothing Then
		Dim sample = TryCast(tvi.Tag, UIElement)
		If sample Is Nothing AndAlso tvi.Items.Count > 0 Then
		  sample = TryCast((CType(tvi.Items(0), TreeViewItem)).Tag, UIElement)
		End If

		sampleContainer.Child = sample
		Dim da As DescriptionAttribute = Nothing
		If sample IsNot Nothing Then
		  da = TryCast(sample.GetType().GetCustomAttributes(GetType(DescriptionAttribute), False).FirstOrDefault(Function(o) TypeOf o Is DescriptionAttribute), DescriptionAttribute)
		End If

		If da IsNot Nothing Then
		  ControlDescription.Text = da.Description
		Else
		  ControlDescription.Text = ""
		End If

	  End If
	End Sub
  End Class
End Namespace
