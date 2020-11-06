using C1.WPF.Toolbar;
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

namespace ToolbarSamples
{
    /// <summary>
    /// Interaction logic for SimplifiedRibbon.xaml
    /// </summary>
    public partial class SimplifiedRibbon : UserControl
    {
        public SimplifiedRibbon()
        {
            InitializeComponent();
            foreach (var key in Resources.Keys)
            {
                var cmd = Resources[key] as C1ToolbarCommand;
                if (cmd != null)
                {
                    CommandManager.RegisterClassCommandBinding(GetType(), new CommandBinding(
                      cmd, (s, e) => Execute(cmd.LabelTitle), (s, e) => e.CanExecute = true));
                }
            }
        }

        void Execute(string text)
        {
            tb.Text += text + "\n";
            tb.SelectionStart = tb.Text.Length;
        }

    }
}
