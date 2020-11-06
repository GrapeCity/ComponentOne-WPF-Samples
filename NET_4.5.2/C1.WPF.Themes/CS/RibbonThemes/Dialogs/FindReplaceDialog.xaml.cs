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
using System.Windows.Shapes;

namespace RibbonThemes
{
    /// <summary>
    /// Interaction logic for FindReplaceDialog.xaml
    /// </summary>
    public partial class FindReplaceDialog : Window
    {
        public FindReplaceDialog()
        {
            InitializeComponent();
            InitializeCommandBindings();
        }

        public FindReplaceDialog(int index)
        {
            InitializeComponent();
            InitializeCommandBindings();
            _tab.SelectedIndex = index;
        }

        #region Events handler
        private void _tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;

            if (tabControl != null)
            {
                if (tabControl.SelectedIndex != 0)
                {
                    _btnReplace.Visibility = Visibility.Visible;
                    _btnReplaceAll.Visibility = Visibility.Visible;
                    _lbReplace.Visibility = Visibility.Visible;
                    _comboReplace.Visibility = Visibility.Visible;
                }
                else
                {
                    _btnReplace.Visibility = Visibility.Hidden;
                    _btnReplaceAll.Visibility = Visibility.Hidden;
                    _lbReplace.Visibility = Visibility.Hidden;
                    _comboReplace.Visibility = Visibility.Hidden;
                }
            }
        }

        private void _btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region CommandBindings
        void InitializeCommandBindings()
        {
            CommandBinding closeCmdBinding = new CommandBinding(SystemCommands.CloseWindowCommand, CloseCommandHandler);
            CommandBinding minCmdBinding = new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeCommandHandler);
            CommandBinding maxCmdBinding = new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeCommandHandler);
            CommandBinding resCmdBinding = new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreCommandHandler);

            this.CommandBindings.Add(closeCmdBinding);
            this.CommandBindings.Add(minCmdBinding);
            this.CommandBindings.Add(maxCmdBinding);
            this.CommandBindings.Add(resCmdBinding);
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
        private void MinimizeCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        private void MaximizeCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }
        private void RestoreCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
        #endregion
    }
}
