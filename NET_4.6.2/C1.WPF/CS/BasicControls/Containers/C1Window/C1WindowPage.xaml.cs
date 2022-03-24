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
using C1.WPF;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoWindow.xaml
    /// </summary>
    public partial class DemoWindow : UserControl
    {
        C1Window _wnd;
        public DemoWindow()
        {
            InitializeComponent();
            Unloaded += DemoWindow_Unloaded;
        }

        private void DemoWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_wnd != null)
            {
                _wnd.Close();
                _wnd = null;
            }
        }

        private void CreateWindow_Click(object sender, RoutedEventArgs e)
        {
            _wnd = new C1Window();
            _wnd.Header = "This is the header.";
            _wnd.Height = 300;
            _wnd.Width = 300;
            _wnd.Content = new WindowContent();
            _wnd.CenterOnScreen();
            _wnd.Show();
        }


        private void CreateModal_Click(object sender, RoutedEventArgs e)
        {
            C1Window wnd = new C1Window();
            wnd.Header = "This is the header.";
            wnd.Height = 300;
            wnd.Width = 300;
            wnd.Content = new WindowContent();
            wnd.CenterOnScreen();
            wnd.ShowModal();
        }
    }
}
