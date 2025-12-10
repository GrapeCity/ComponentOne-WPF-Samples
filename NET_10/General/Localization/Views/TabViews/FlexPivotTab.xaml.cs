using C1.FlexPivot;
using C1.WPF.Pivot;
using Localization.ViewModel;
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

namespace Localization.Views.TabViews
{
    /// <summary>
    /// Interaction logic for FlexPivotTab.xaml
    /// </summary>
    public partial class FlexPivotTab : UserControl
    {
        public FlexPivotTab()
        {
            InitializeComponent();
            var vm = new FlexPivotChartViewModel();
            DataContext = vm;
            InitilizeC1FlexPivotEngine(vm);
        }

        #region C1FlexPivot Setup
        private void InitilizeC1FlexPivotEngine(FlexPivotChartViewModel vm)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (c1Pivot?.C1PivotEngine != null)
                {
                    c1Pivot.C1PivotEngine.DataSource = vm.Data;
                    c1Pivot.C1PivotEngine.RowFields.Add("Education");
                    c1Pivot.C1PivotEngine.RowFields.Add("BloodType");
                    c1Pivot.C1PivotEngine.ColumnFields.Add("Residence");
                    c1Pivot.C1PivotEngine.ValueFields.Add("Age");
                }
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }
        #endregion
    }
}
