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
using System.Reflection;

namespace PdfViewerSamples
{
    /// <summary>
    /// Interaction logic for ThumbnailsDemo.xaml
    /// </summary>
    public partial class ThumbnailsDemo : UserControl
    {
        public ThumbnailsDemo()
        {
            InitializeComponent();

            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/C1XapOptimizer.pdf", UriKind.Relative));
            pdf.LoadDocument(resource.Stream);
        }
    }
}
