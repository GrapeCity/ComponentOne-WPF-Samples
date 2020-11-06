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

namespace Orders
{
    /// <summary>
    /// Interaction logic for ClosableTabItemHeader.xaml
    /// </summary>
    public partial class ClosableTabItemHeader : UserControl
    {
        public ClosableTabItemHeader()
        {
            InitializeComponent();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            // Raise a close request event
            CloseRequested(this, e);
        }
        // This event is raised when the Close button is clicked. MainWindow removes the tab page containing this header.
        public event EventHandler CloseRequested = delegate { };

        // Gets or sets the header text.
        public string Title
        {
            get { return title.Text; }
            set { title.Text = value; }
        }
    }
}
