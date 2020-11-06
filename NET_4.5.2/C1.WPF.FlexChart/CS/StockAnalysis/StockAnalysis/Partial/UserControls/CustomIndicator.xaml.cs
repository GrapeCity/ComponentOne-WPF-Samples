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
    /// Interaction logic for CustomIndicator.xaml
    /// </summary>
    public partial class CustomIndicator : UserControl
    {
        public CustomIndicator()
        {
            InitializeComponent();
        }

        public ViewModel.ViewModel Model
        {
            get { return ViewModel.ViewModel.Instance; }
        }

        private void SettableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var check = sender as CustomControls.SettableCheckBox;
            Type type = check.Tag as Type;
            
            if (type != null && Model.IndicatorCharts.Count < 3 && !Model.IndicatorCharts.Contains(type))
            {
                Model.IndicatorCharts.Add(type);
            }
            else
            {
                MessageBox.Show(String.Message.MaximumIndicatorMessage);
                check.IsChecked = false;
            }
            
        }

        private void SettableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var check = sender as CustomControls.SettableCheckBox;
            if (check != null)
            {
                Type type = check.Tag as Type;
                if (Model.IndicatorCharts.Contains(type))
                    Model.IndicatorCharts.Remove(type);
            }
        }
    }
}
