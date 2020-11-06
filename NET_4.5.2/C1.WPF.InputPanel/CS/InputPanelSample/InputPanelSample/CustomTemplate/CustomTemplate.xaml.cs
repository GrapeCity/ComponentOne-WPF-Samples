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

namespace InputPanelSample
{
    /// <summary>
    /// Interaction logic for CustomTemplate.xaml
    /// </summary>
    public partial class CustomTemplate : UserControl
    {
        public List<string> Countries
        {
            get
            {
                return "China|India|United States|Indonesia|Brazil|Pakistan|Bangladesh|Nigeria|Russia|Japan|Mexico|Philippines|Vietnam|Germany|Ethiopia|Egypt|Iran|Turkey|Congo|France|Thailand|United Kingdom|Italy|Myanmar".Split('|').ToList();
            }
        }

        public Data SourceData;
        public CustomTemplate()
        {
            InitializeComponent();

            SourceData = new Data();
            DataContext = SourceData;
        }
    }
}
