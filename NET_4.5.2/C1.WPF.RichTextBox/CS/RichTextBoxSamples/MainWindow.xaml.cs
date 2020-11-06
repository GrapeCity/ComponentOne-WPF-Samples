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
using System.Windows.Shapes;
using C1.WPF;

namespace RichTextBoxSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            C1TabItem item1 = new C1TabItem();
            item1.Header = "See it in action";
            item1.Content = new RichTextBoxSamples.DemoRichTextBox();

            C1TabItem item2 = new C1TabItem();
            item2.Header = "Formatting";
            item2.Content = new RichTextBoxSamples.MainPage();

            C1TabItem item3 = new C1TabItem();
            item3.Header = "Spell checking";
            item3.Content = new RichTextBoxSamples.AsYouTypeSpellCheck();

            C1TabItem item4 = new C1TabItem();
            item4.Header = "Import & export";
            item4.Content = new RichTextBoxSamples.DemoRichTextBoxFilter();
         
            C1TabItem item5 = new C1TabItem();
            item5.Header = "Custom ContextMenu";
            item5.Content = new CustomContextMenu();

            tab.Items.Add(item1);
            tab.Items.Add(item2);
            tab.Items.Add(item3);
            tab.Items.Add(item4);
            tab.Items.Add(item5);
        }
    }
}
