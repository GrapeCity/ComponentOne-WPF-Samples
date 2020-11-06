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

namespace StockAnalysis.Partial.UserControls
{
    /// <summary>
    /// Interaction logic for Overlay.xaml
    /// </summary>
    public partial class Overlays : UserControl
    {
        public Overlays()
        {
            InitializeComponent();
        }

        public ViewModel.ViewModel Model
        {
            get { return ViewModel.ViewModel.Instance; }
        }

        private ClearListCommand clearCommand = null;
        public ClearListCommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new ClearListCommand();
                }
                return clearCommand;
            }
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Data.OverlayItem item in e.AddedItems)
            {
                if (!Model.OverlayTypes.Contains(item.Type))
                {
                    Model.OverlayTypes.Add(item.Type);
                }
            }
            foreach (Data.OverlayItem item in e.RemovedItems)
            {
                if (Model.OverlayTypes.Contains(item.Type))
                {
                    Model.OverlayTypes.Remove(item.Type);
                }
            }
        }
    }

    public class ClearListCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ListBox list = parameter as ListBox;
            if(list != null)
                list.UnselectAll();
        }
    }

}
