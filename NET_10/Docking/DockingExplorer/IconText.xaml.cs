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

namespace DockingExplorer
{
    partial class IconText
    {
        public IconText()
        {
            InitializeComponent();
        }

        public ImageSource Icon
        {
            set { Image.Source = value; }
            get { return Image.Source; }
        }

        public string Text
        {
            set { TextBlock.Text = value; }
            get { return TextBlock.Text; }
        }       
    }
}
