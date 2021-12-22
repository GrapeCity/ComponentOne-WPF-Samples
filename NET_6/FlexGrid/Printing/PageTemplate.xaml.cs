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

namespace Printing
{
    public partial class PageTemplate : UserControl
    {
        public PageTemplate()
        {
            InitializeComponent();
            FooterLeft.Text = DateTime.Today.ToShortDateString();
        }
        public void SetPageMargin(Thickness margin)
        {
            LayoutRoot.RowDefinitions[0].Height = new GridLength(margin.Top);
            LayoutRoot.RowDefinitions[2].Height = new GridLength(margin.Bottom);
            LayoutRoot.ColumnDefinitions[0].Width = new GridLength(margin.Left);
            LayoutRoot.ColumnDefinitions[2].Width = new GridLength(margin.Right);
        }
    }
}
