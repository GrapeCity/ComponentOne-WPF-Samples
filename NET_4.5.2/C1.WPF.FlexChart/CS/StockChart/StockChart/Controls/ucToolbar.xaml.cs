using C1.Chart;
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

namespace StockChart
{
    /// <summary>
    /// Interaction logic for ucToolbar.xaml
    /// </summary>
    public partial class ucToolbar : UserControl
    {
        public ucToolbar()
        {
            this.DataContext = ViewModel.ChartViewModel.Instance;
            InitializeComponent();

            this.Loaded += (o, e) =>
              {
                  this.ExportCommand = ViewModel.ChartViewModel.Instance.ExportCommand;
              };
        }

        #region DependencyProperty
        public ICommand AddSymbolCommand
        {
            get { return (ICommand)this.GetValue(AddSymbolCommandProperty); }
            set { SetValue(AddSymbolCommandProperty, value); }
        }

        public static readonly DependencyProperty AddSymbolCommandProperty =
           DependencyProperty.Register("AddSymbolCommand", typeof(ICommand), typeof(ucToolbar), new PropertyMetadata(null));

        public object AddSymbolCommandParameter
        {
            get { return this.GetValue(AddSymbolCommandParameterProperty); }
            set { SetValue(AddSymbolCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty AddSymbolCommandParameterProperty =
           DependencyProperty.Register("AddSymbolCommandParameter", typeof(object), typeof(ucToolbar), new PropertyMetadata(null));

        

        public ICommand ExportCommand
        {
            get { return (ICommand)this.GetValue(ExportCommandProperty); }
            set { SetValue(ExportCommandProperty, value); }
        }

        public static readonly DependencyProperty ExportCommandProperty =
           DependencyProperty.Register("ExportCommand", typeof(ICommand), typeof(ucToolbar), new PropertyMetadata(null));

        public object ExportCommandParameter
        {
            get { return this.GetValue(ExportCommandParameterProperty); }
            set { SetValue(ExportCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty ExportCommandParameterProperty =
           DependencyProperty.Register("ExportCommandParameter", typeof(object), typeof(ucToolbar), new PropertyMetadata(null));

        
        






        #endregion
        

        private void cmbExport_DropDownClosed(object sender, EventArgs e)
        {
            this.ExportCommandParameter = cmbExport.SelectedValue;

            if (this.ExportCommand != null && this.ExportCommandParameter != null)
            {
                this.ExportCommand.Execute(this.ExportCommandParameter);
            }

            cmbExport.SelectedIndex = -1;
            cmbExport.Text = "Export To";
        }
    }
}
