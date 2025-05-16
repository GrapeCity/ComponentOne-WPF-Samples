using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
	public partial class Hyperlinks : UserControl
	{
		public Hyperlinks()
		{
			InitializeComponent();

			var items = new List<Item>
			{
				new Item
                { 
                    Link = "www.microsoft.com", 
                    LinkImage="https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE1Mu3b?ver=5c31"
				},
				new Item
                { 
                    Link = "www.bing.com", 
                    LinkImage="https://cdn.iconscout.com/icon/free/png-256/bing-1-283055.png"
				},
				new Item
                { 
                    Link = "www.google.com", 
                    LinkImage="http://www.google.com/profiles/c/photos/public/AIbEiAIAAABECMuvxL_O4o-c9wEiC3ZjYXJkX3Bob3RvKigzZmYwZjBmN2UxZDQ1MmJkNTg4ZDg3ZjQ3NTUxOTFjZDFhNzYxMTU2MAHZbpvFrv9hD-0kBIss_6QH2NdG1g"
                }
			};
			grid.ItemsSource = items;
		}

        private void DataGridHyperlinkColumn_Click(object sender, C1.WPF.DataGrid.DataGridRowEventArgs e)
		{
			msg.Text = string.Format("Hyperlink {0} clicked.", (e.Row.DataItem as Item).Link);
		}
	}


	public class Item
	{
		public string Link { get; set; }
		public string LinkImage { get; set; }
	}
}
