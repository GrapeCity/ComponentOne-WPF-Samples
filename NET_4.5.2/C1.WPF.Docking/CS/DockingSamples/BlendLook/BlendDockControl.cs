using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.Docking;
using C1.WPF;
using System.Windows;

namespace DockingSamples.BlendLook
{
    public class BlendDockControl : C1DockControl
    {
        protected override C1DockTabControl CreateDockTabControlOverride()
        {
            return new BlendTabControl();
        }

    }

    public class BlendTabControl : C1DockTabControl
    {
        public BlendTabControl()
        {
            TabStripPlacement = Dock.Top;
            TabItemShape = C1TabItemShape.Sloped;
            BorderThickness = new Thickness(0);
            ShowHeader = false;
            TabItemClose = C1TabItemCloseOptions.InEachTab;
        }
    }
}
