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

namespace BasicControls
{
    public partial class WindowContent 
        : UserControl
    {
        public WindowContent()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
			{
                this.combo.Items.Add(String.Format("Item {0}", i.ToString()));
			}
        }
    }
}
