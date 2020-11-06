using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.Docking;

namespace DockingExplorer
{
    public class TabItem : C1DockTabItem
    {
        protected override void OnMouseMove(MouseEventArgs e)
        {
            var parent = Parent as C1DockTabControl;
            if (parent != null && parent.DockMode == DockMode.Sliding)
            {
                IsSelected = true;
                parent.SlideOpen();
            }
        }
    }
}
