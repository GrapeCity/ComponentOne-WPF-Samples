using C1.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DrillDown
{
    /// <summary>
    /// Interaction logic for AsyncDrillDownDemo.xaml
    /// </summary>
    public partial class AsyncDrillDownDemo : UserControl
    {
        private DrillDownViewModel _vm;
        private IDictionary<int, string> _paths;
        private object _currentValue;

        public AsyncDrillDownDemo()
        {
            InitializeComponent();
            _paths = new Dictionary<int, string>();
            _vm = new DrillDownViewModel();
            this.DataContext = _vm;
            _vm.AsyncDrillDownManager.BeforeDrill += Manager_BeforeDrill;
            _vm.AsyncDrillDownManager.AfterDrill += Manager_AfterDrill;
            _vm.AsyncDrillDownManager.Refresh();
        }

        private void Manager_AfterDrill(object sender, DrillDownEventArgs e)
        {
            switch (e.DrillDownLevel)
            {
                case 0:
                case 1:
                default:
                    flexChart.ChartType = C1.Chart.ChartType.Column;
                    break;
                case 2:
                case 3:
                    flexChart.ChartType = C1.Chart.ChartType.SplineSymbols;
                    break;
                case 4:
                    flexChart.ChartType = C1.Chart.ChartType.Spline;
                    break;
            }
            flexChart.Footer = string.Format("{0}-wise Sales", _vm.AsyncDrillDownManager.GroupNames[e.DrillDownLevel]);
            dynamic current = _vm.AsyncDrillDownManager.Current;
            if (_currentValue != null && current.Name == null)
                current.Name = _currentValue.ToString();
            if(e.IsDrillDown)
            {
                if (_paths.ContainsKey(e.DrillDownLevel))
                    _paths.Remove(e.DrillDownLevel);
                _paths.Add(e.DrillDownLevel, current.Name);
            }
            UpdateNavBar();
        }

        private void Manager_BeforeDrill(object sender, DrillDownEventArgs e)
        {
            if (e.IsDrillDown == false && e.DrillDownLevel > 0)
                _paths.Remove(e.DrillDownLevel);

            if (e.DrillDownLevel > 3 && e.IsDrillDown)
                e.Cancel = true;
        }

        private async void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_vm.IsBusy)
                return;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(flexChart);
                var hitInfo = flexChart.HitTest(pos);
                if (hitInfo != null && hitInfo.Distance < 2)
                {
                    _currentValue = hitInfo.X;
                    tbWaitMessage.Text = "Please wait while the details are being fetched";

                    flexChart.Cursor = Cursors.Wait;
                    Console.WriteLine("Start fetching details");
                    IEnumerable<object> data = await _vm.FetchDataAsync(_vm.AsyncDrillDownManager.DrillDownLevel, _currentValue);
                    Console.WriteLine("Fetching details completed");

                    tbWaitMessage.Text = "";
                    flexChart.Cursor = Cursors.Arrow;
                    _vm.AsyncDrillDownManager.DrillDown(data);
                }
            }
        }

        private void UpdateNavBar()
        {
            var manager = _vm.AsyncDrillDownManager;
            pnlNavBar.Children.Clear();

            if (manager.DrillDownLevel > 0)
            {
                for (int i = 0; i < _paths.Count; i++)
                {
                    var path = _paths.ElementAt(i);
                    if (i == _paths.Count - 1)
                    {
                        AddToNavBar(typeof(TextBlock), path.Value);
                        AddToNavBar(typeof(TextBlock), "-");
                    }
                    else
                    {
                        AddToNavBar(typeof(C1HyperlinkButton), path.Value, path.Key);
                        AddToNavBar(typeof(TextBlock), "\\");
                    }
                }
            }

            string tail = null;
            string aggregate = manager.AggregateType.ToString();
            string currentPath = manager.GroupNames[manager.DrillDownLevel];
            tail = string.Format("{0} By {1}", aggregate, currentPath);
            AddToNavBar(typeof(TextBlock), tail);

        }

        private void AddToNavBar(Type type, string text, int level = -1)
        {
            UIElement lbl = (UIElement)Activator.CreateInstance(type);
            if (lbl is C1HyperlinkButton)
            {
                var link = lbl as C1HyperlinkButton;
                link.Content = text;
                link.FontSize = 15;
                link.Margin = new Thickness(2);
                link.Click += (s, e) => _vm.AsyncDrillDownManager.DrillUp(level);
            }
            else if (lbl is TextBlock)
            {
                var textBlock = lbl as TextBlock;
                textBlock.Text = text;
                textBlock.FontSize = 15;
                textBlock.Margin = new Thickness(2);
            }
            pnlNavBar.Children.Add(lbl);
        }
    }
}
