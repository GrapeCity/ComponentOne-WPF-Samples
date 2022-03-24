using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrintDocTemplates
{
	/// <summary>
	/// Shows the <see cref="DocumentViewer"/> control.
	/// </summary>
	public partial class PrintPreviewWindow : Window
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PrintPreviewWindow"/> window.
		/// </summary>
		public PrintPreviewWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets or sets an <see cref="IDocumentPaginatorSource"/> object which should 
		/// be shown in the <see cref="DocumentViewer"/> control. 
		/// </summary>
		public IDocumentPaginatorSource Document
		{
			get
			{
				return viewer.Document;
			}
			set
			{
				viewer.Document = value;
			}
		}
	}
}
