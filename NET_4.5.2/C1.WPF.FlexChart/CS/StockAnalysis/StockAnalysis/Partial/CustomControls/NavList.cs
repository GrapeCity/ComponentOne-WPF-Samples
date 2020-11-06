using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockAnalysis.Partial.CustomControls
{
    public class NavList : ListBox
    {
        static NavList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavList), new FrameworkPropertyMetadata(typeof(NavList)));
        }
        public bool IsAutoCloseParentPopup
        {
            get { return (bool)this.GetValue(IsAutoCloseParentPopupProperty); }
            set { this.SetValue(IsAutoCloseParentPopupProperty, value); }
        }
        
        public static DependencyProperty IsAutoCloseParentPopupProperty = DependencyProperty.Register(
            "IsAutoCloseParentPopup",
            typeof(bool),
            typeof(NavList),
            new PropertyMetadata(false, new PropertyChangedCallback((o,e)=>
            {
                NavList nav = o as NavList;
                if (Convert.ToBoolean(e.NewValue))
                {
                    nav.SelectionChanged -= Nav_SelectionChanged;
                    nav.SelectionChanged += Nav_SelectionChanged;
                }
                else
                {
                    nav.SelectionChanged -= Nav_SelectionChanged;
                }
            }))
        );

        private static void Nav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavList nav = sender as NavList;
            Command.CloseDropdownCommand cmd = new Command.CloseDropdownCommand();
            if (nav.IsLoaded)
            {
                cmd.Execute(sender);
            }
        }
    }
}
