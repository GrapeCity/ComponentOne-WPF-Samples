Imports System.Text

Namespace Printing
	''' <summary>
	''' Shows the <see cref="DocumentViewer"/> control.
	''' </summary>
	Partial Public Class PrintPreviewWindow
		Inherits Window
		''' <summary>
		''' Initializes a new instance of the <see cref="PrintPreviewWindow"/> window.
		''' </summary>
		Public Sub New()
			InitializeComponent()
		End Sub

		''' <summary>
		''' Gets or sets an <see cref="IDocumentPaginatorSource"/> object which should 
		''' be shown in the <see cref="DocumentViewer"/> control. 
		''' </summary>
		Public Property Document() As IDocumentPaginatorSource
			Get
				Return viewer.Document
			End Get
			Set(ByVal value As IDocumentPaginatorSource)
				viewer.Document = value
			End Set
		End Property
	End Class
End Namespace
