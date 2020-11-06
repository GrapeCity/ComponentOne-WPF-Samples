using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonTasksMVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs rea)
        {
            fvToolBar.SetHost(fvp);
            fvp.FocusFirstElement();
            fvp.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "DocumentSource")
                {
                    fvp.FocusFirstElement();
                }
            };

            copyMenuItem.Command = fvp.CopyTextCommand;
            selectAllMenuItem.Command = fvp.SelectAllCommand;
            deselectMenuItem.Command = fvp.DeselectAllCommand;
            fvp.ContextMenuPopup += (s, e) =>
            {
                var cm = fvpContextMenu;
                cm.Placement = PlacementMode.Absolute;
                cm.HorizontalOffset = e.X;
                cm.VerticalOffset = e.Y;
                fvp.Focus();
                cm.IsOpen = true;
            };
            fvpContextMenu.Closed += (s, e) =>
            {
                fvp.FocusFirstElement();
            };
        }
    }
}
