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
using C1.WPF.RichTextBox;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace MenuExplorer.Tools
{
    /// <summary>
    /// Interaction logic for InsertTableTool.xaml
    /// </summary>
    public partial class InsertTableTool : UserControl
    {
        public Window window
        {
            get;
            set;
        }

        public C1RichTextBox RichTextBox
        {
            get;
            set;
        }

        public InsertTableTool()
        {
            this.InitializeComponent();
           
        }

        public void Show()
        {
            window = new Window();
            window.Title = "Insert Table";
            window.Content = this;
            window.Foreground = RichTextBox.Foreground;
            window.ShowInTaskbar = false;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.WindowStyle = WindowStyle.ToolWindow;
            var elementSource = HwndSource.FromVisual(RichTextBox) as HwndSource;
            if (elementSource != null)
            {
                int GA_ROOT = 2;
                IntPtr root = GetAncestor(elementSource.Handle, GA_ROOT);
                new WindowInteropHelper(window).Owner = root;
            }
            window.Closed += delegate
            {
                RichTextBox.Focus();
            };
            window.ShowDialog();
        }

        public void Close()
        {
            window.Close();
        }

        private void btnInsertTable_Click(object sender, RoutedEventArgs e)
        {
            int rows = (int)numRowsBox.Value;
            int cols = (int)numColsBox.Value;
            if (RichTextBox != null && rows > 0 && cols > 0)
                RichTextBox.Selection.Start.InsertTable(rows, cols);
            Close();
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hWnd, int flags);
    }
}
