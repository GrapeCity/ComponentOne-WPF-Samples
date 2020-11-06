using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // select the culture (de, it, fr, tr are supported)
            var ci = new System.Globalization.CultureInfo("hi"); // de-DE, etc

            // this controls the UI strings
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;


            // initialize the component
            InitializeComponent();

            // show current culture
            _txtCI.Text = ci.Name;

            // give users a hint
            _flex[0, 0] = "Check the localized filter";
            _flex[1, 0] = "using the drop-downs on the";
            _flex[2, 0] = "column headers.";
        }
    }
}
