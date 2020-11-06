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

namespace CustomEngineSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            treeView.ItemsSource = StoreCollection.GetData();

            var ce = new CustomEngine();
            var cl = new CustomLexer();
            editor.SetCustomEngine(ce, cl);
            editor.DataSource = treeView.ItemsSource;
            editor.Expression = "[Name] + \": \" + [ProductsGroups].Where(x => x.Name == \"Notebooks\").Sum(x => x.Products.Sum(p => p.Count))";
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            text.Text = "";
            var sc = (StoreCollection)treeView.ItemsSource;
            foreach (var store in sc)
            {
                editor.ItemContext = store;
                var res = editor.Evaluate();
                if (editor.IsValid)
                    text.Text += res + "\n";
                else
                    foreach (var error in editor.GetErrors())
                        text.Text += error.Message + "\n";
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            text.Text = "";
        }
    }
}
