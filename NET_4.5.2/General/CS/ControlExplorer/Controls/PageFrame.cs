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
using C1.WPF;

namespace ControlExplorer
{
    public class PageFrame : ContentControl
    {
        public PageFrame()
        {
            DefaultStyleKey = typeof(PageFrame);
        }


        public Storyboard LoadStoryboard
        {
            get 
            {
                return GetTemplateChild("LoadStoryboard") as Storyboard;
            }
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Page parent = VTreeHelper.GetParentOfType(this, typeof(Page)) as Page;
        }
    }
}
