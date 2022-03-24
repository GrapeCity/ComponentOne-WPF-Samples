Imports System.Net
Imports System.Windows.Media.Animation

Namespace Printing
	Partial Public Class PageTemplate
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
			FooterLeft.Text = Date.Today.ToShortDateString()
		End Sub
		Public Sub SetPageMargin(ByVal margin As Thickness)
			LayoutRoot.RowDefinitions(0).Height = New GridLength(margin.Top)
			LayoutRoot.RowDefinitions(2).Height = New GridLength(margin.Bottom)
			LayoutRoot.ColumnDefinitions(0).Width = New GridLength(margin.Left)
			LayoutRoot.ColumnDefinitions(2).Width = New GridLength(margin.Right)
		End Sub
	End Class
End Namespace
